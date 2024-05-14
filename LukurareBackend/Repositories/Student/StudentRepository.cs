using EFDatabaseModel.Contexts;
using EFDatabaseModel.DbModel;
using EFDatabaseModel.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ProjectBase.KendoUiSupport;
using ProjectBase.Repository.Transaction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LukurareBackend.Repositories.Student
{
    public class StudentRepository : EFDatabaseModelBaseRepository<SchoolMsStudentClassHistory>
    {
        public StudentRepository(string currentModule)
            : base(currentModule) { }

        public async Task<ExecutionResult<IEnumerable<SchoolMsSubject>>> GetSubjects(
            NeedDataSourceEventArgs args
        )
        {
            var result = new ExecutionResult<IEnumerable<SchoolMsSubject>>(
                true,
                "",
                new HashSet<SchoolMsSubject>()
            );
            try
            {
                using (var context = new EFDatabaseModelDatabaseContext())
                {
                    args = NeedDataSourceEventArgs.EnsureArgsValid(args);
                    /*var filterValue = args.GetFilterValue(0).ToLower(); ;
                    if (string.IsNullOrEmpty(filterValue))
                        return result;*/

                    var returnData = await context.SchoolMsSubjects
                        .Where(r => true)
                        .OrderBy(r => r.SubjectCode)
                        .Skip(args.skip)
                        .Take(args.take)
                        .ToListAsync();

                    result.Result = returnData.Select(
                        r => new SchoolMsSubject { Id = r.Id, SubjectName = r.SubjectName, }
                    );
                }
            }
            catch (Exception ex)
            {
                result = result.CopyFromException(ex);
            }
            return result;
        }

        public async override Task<ExecutionResult<SchoolMsStudentClassHistory>> Add(
            SchoolMsStudentClassHistory model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsStudentClassHistory>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                executionResult = await this.IsValidInsert(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult; //if fail return the failed execution result

                //insert the okay object to the DB
                var modelInsert = CloneModel(model);

                if (model.SchoolMsStudent.AccountEntity.RegDate == null)
                {
                    modelInsert.SchoolMsStudent.AccountEntity.RegDate = DateTime.Now;
                }
                if (string.IsNullOrEmpty(model.SchoolMsStudent.AccountEntity.Email))
                {
                    modelInsert.SchoolMsStudent.AccountEntity.Email = "";
                }
                if (string.IsNullOrEmpty(model.SchoolMsStudent.AccountEntity.PhysicalAddress))
                {
                    modelInsert.SchoolMsStudent.AccountEntity.PhysicalAddress = "";
                }
                if (string.IsNullOrEmpty(model.SchoolMsStudent.AccountEntity.Phone1))
                {
                    Random random = new Random();
                    modelInsert.SchoolMsStudent.AccountEntity.Phone1 = "23d" + random.Next(1000);
                }

                if (model.SchoolMsStudent.AccountEntity.DateOfBirth == null)
                {
                    modelInsert.SchoolMsStudent.AccountEntity.DateOfBirth = DateTime.Now;
                }
                if (model.SchoolMsStudent.AccountEntity.SubAccountBranchId == 0)
                {
                    modelInsert.SchoolMsStudent.AccountEntity.SubAccountBranchId =
                        context.SubAccountBranches.FirstOrDefault().Id;
                }
                if (model.SchoolMsStudent.AccountEntity.IdentificationDocumentTypeId == 0)
                {
                    modelInsert.SchoolMsStudent.AccountEntity.IdentificationDocumentTypeId =
                        context.IdentificationDocumentTypes.FirstOrDefault().Id;
                }
                if (model.SchoolMsStudent.AccountEntity.AccountEntityActiveStateId == 0)
                {
                    modelInsert.SchoolMsStudent.AccountEntity.AccountEntityActiveStateId =
                        context.AccountEntityActiveStates.FirstOrDefault().Id;
                }

                if (
                    string.IsNullOrEmpty(
                        model.SchoolMsStudent.AccountEntity.IdentificationDocumentNumber
                    )
                )
                {
                    Random random = new Random();
                    modelInsert.SchoolMsStudent.AccountEntity.IdentificationDocumentNumber =
                        "23d" + random.Next(1000);
                }

                context.SchoolMsStudentClassHistories.Add(modelInsert);

                var accountEntityToInsert = modelInsert.SchoolMsStudent.AccountEntity;
                context.AccountEntities.Add(accountEntityToInsert);

                var studentToInsert = modelInsert.SchoolMsStudent;
                context.SchoolMsStudents.Add(studentToInsert);

                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Create operation was successful";
                    //executionResult.Result = CloneModel(modelInsert);
                    //var Document = new IdentificationDocumentType
                    //{
                    //    Id=modelInsert.IdentificationDocumentTypeId,
                    //    DocumentName=modelInsert.IdentificationDocumentType.DocumentName

                    //};
                    //modelInsert.IdentificationDocumentType=Document;
                    executionResult.Result = await GetModelForInsertUpdateFromId(
                        modelInsert.Id,
                        modelInsert,
                        context
                    );
                }
                else
                {
                    //incase the context was not created here return the object
                    //as inserted to the context
                    //helps the caller method to have log of the object in ChangeTraking inside DbContext
                    executionResult.Result = modelInsert;
                }
            }
            catch (Exception ex)
            {
                //if an error occured copy its data from the exception object into the execution result by
                executionResult.CopyFromException(ex);
            }
            finally
            {
                //incase the context was created in this method then dispose it here
                if (createContextHere)
                    await context.DisposeAsync();
            }
            return executionResult;
        }

        public async Task<ExecutionResult<SchoolMsStudentClassHistory>> AddFromFile(
            IFormFileCollection files,
            String pathForUploadedFiled
        )
        {
            var executionResult = new ExecutionResult<SchoolMsStudentClassHistory>(true);

            try
            {
                long uploaded_size = 0;

                var uploaded_files = files;

                int iCounter = 0;

                string sFiles_uploaded = "";
                string uploaded_Filename = "";
                foreach (var uploaded_file in uploaded_files)
                {
                    //----< Uploaded File >----

                    iCounter++;

                    uploaded_size += uploaded_file.Length;

                    sFiles_uploaded += "\n" + uploaded_file.FileName;

                    //< Filename >


                    uploaded_Filename = $" {Guid.NewGuid().ToString()}-{uploaded_file.FileName}";

                    string new_Filename_on_Server = pathForUploadedFiled + "\\" + uploaded_Filename;

                    //</ Filename >



                    //< Copy File to Target >

                    using (
                        FileStream stream = new FileStream(new_Filename_on_Server, FileMode.Create)
                    )
                    {
                        await uploaded_file.CopyToAsync(stream);
                    }

                    //< Copy File to Target >

                    //----</ Uploaded File >----
                }

                string message =
                    $"Upload successful!\n files uploaded: {iCounter} \nsize: {uploaded_size} \n  {sFiles_uploaded}";

                var contactsList = new List<SchoolMsStudentClassHistory>();
                FileInfo existingFile = new FileInfo(pathForUploadedFiled + uploaded_Filename);

                using (var package = new ExcelPackage(existingFile))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.FirstOrDefault();

                    if (workSheet == null)
                    {
                        //No work sheet
                    }
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;
                    List<SchoolMsStudentClassHistory> studentDetails =
                        new List<SchoolMsStudentClassHistory>();

                    using (var context = new EFDatabaseModelDatabaseContext())
                    {
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            String SCode = workSheet.Cells[rowIterator, 1].Value.ToString();
                            string Name = workSheet.Cells[rowIterator, 2].Value.ToString();
                            string PiD = workSheet.Cells[rowIterator, 3].Value.ToString();
                            string DOB = workSheet.Cells[rowIterator, 4].Value.ToString();
                            string gender = workSheet.Cells[rowIterator, 5].Value.ToString();
                            string Grade = workSheet.Cells[rowIterator, 6].Value.ToString();
                            string EntryG = workSheet.Cells[rowIterator, 7].Value.ToString();
                            string AYear = workSheet.Cells[rowIterator, 8].Value.ToString();
                            string PoAddress = workSheet.Cells[rowIterator, 9].Value.ToString();
                            string PhAddress = workSheet.Cells[rowIterator, 10].Value.ToString();
                            string Phone = workSheet.Cells[rowIterator, 11].Value.ToString();

                            if (string.IsNullOrEmpty(Name))
                            {
                                executionResult.IsOkay = false;
                                executionResult.Message = "Please supply all the student names";
                                return await Task.FromResult(executionResult);
                            }

                            if (string.IsNullOrEmpty(Phone))
                            {
                                executionResult.IsOkay = false;
                                executionResult.Message =
                                    "Please supply all phone numbers for students";
                                return await Task.FromResult(executionResult);
                            }
                            if (string.IsNullOrEmpty(PiD))
                            {
                                executionResult.IsOkay = false;
                                executionResult.Message = "Please supply Pupil ID for " + Name;
                                return await Task.FromResult(executionResult);
                            }

                            var pIdInDb = context.AccountEntities.FirstOrDefault(
                                r => r.EntityNo.Equals(PiD.Trim())
                            );
                            if (pIdInDb != null)
                            {
                                executionResult.IsOkay = false;
                                executionResult.Message = $"Pupil ID '{PiD}' exist";
                                return await Task.FromResult(executionResult);
                            }

                            DateTime validDateOfBirth;
                            if (!DateTime.TryParse(DOB, out DateTime result))
                            {
                                executionResult.IsOkay = false;
                                executionResult.Message = $"Invalid date of birth for  {Name}";
                                return await Task.FromResult(executionResult);
                            }
                            validDateOfBirth = result;

                            var schoolInDb = context.SchoolMsSchools
                                .Join(
                                    context.SubAccounts,
                                    sc => sc.SubAccountId,
                                    sa => sa.Id,
                                    (sc, sa) => new { sc, sa }
                                )
                                .FirstOrDefault(r => r.sa.AccountNo.Equals(SCode.Trim()));
                            if (schoolInDb == null)
                            {
                                executionResult.IsOkay = false;
                                executionResult.Message =
                                    $"There is no learning center with code {SCode}";
                                return await Task.FromResult(executionResult);
                            }
                            var genderInDb = context.Genders.FirstOrDefault(
                                r => r.GenderName.Equals(gender.Trim())
                            );
                            if (genderInDb == null)
                            {
                                executionResult.IsOkay = false;
                                executionResult.Message = $"Gender '{gender}' does not exist";
                                return await Task.FromResult(executionResult);
                            }
                            var gradeInDb = context.SchoolMsClasses.FirstOrDefault(
                                r => r.ClassName.Equals(Grade)
                            );
                            if (gradeInDb == null)
                            {
                                executionResult.IsOkay = false;
                                executionResult.Message = $"Grade '{Grade}' does not exist";
                                return await Task.FromResult(executionResult);
                            }

                            var student = new SchoolMsStudentClassHistory
                            {
                                AcademicYear = "" + DateTime.UtcNow.Year,
                                IsCurrentClass = true,
                                EntryGrade = EntryG,
                                SchoolMsClassId = gradeInDb.Id,
                                SchoolMsStudent = new SchoolMsStudent
                                {
                                    //AccountEntityId = student.SchoolMsStudent.AccountEntityId,
                                    RegionStudentIdentifier = EntryG,
                                    SchoolMsSchoolId = schoolInDb.sc.Id,
                                    AccountEntity = new AccountEntity
                                    {
                                        EntityNo = PiD,
                                        EntityName = Name,
                                        DateOfBirth = validDateOfBirth,
                                        IsActive = true,
                                        PhysicalAddress = PhAddress,
                                        PostalAddress = PoAddress,
                                        IsBackUpRecord = false,
                                        GenderId = genderInDb.Id,
                                        Phone1 = Phone
                                    }
                                }
                            };

                            if (student.SchoolMsStudent.AccountEntity.RegDate == null)
                            {
                                student.SchoolMsStudent.AccountEntity.RegDate = DateTime.Now;
                            }
                            if (string.IsNullOrEmpty(student.SchoolMsStudent.AccountEntity.Email))
                            {
                                student.SchoolMsStudent.AccountEntity.Email = "";
                            }
                            if (
                                string.IsNullOrEmpty(
                                    student.SchoolMsStudent.AccountEntity.PhysicalAddress
                                )
                            )
                            {
                                student.SchoolMsStudent.AccountEntity.PhysicalAddress = "";
                            }
                            if (string.IsNullOrEmpty(student.SchoolMsStudent.AccountEntity.Phone1))
                            {
                                Random random = new Random();
                                student.SchoolMsStudent.AccountEntity.Phone1 =
                                    "23d" + random.Next(1000);
                            }

                            if (student.SchoolMsStudent.AccountEntity.DateOfBirth == null)
                            {
                                student.SchoolMsStudent.AccountEntity.DateOfBirth = DateTime.Now;
                            }
                            if (student.SchoolMsStudent.AccountEntity.SubAccountBranchId == 0)
                            {
                                student.SchoolMsStudent.AccountEntity.SubAccountBranchId =
                                    context.SubAccountBranches.FirstOrDefault().Id;
                            }
                            if (
                                student.SchoolMsStudent.AccountEntity.IdentificationDocumentTypeId
                                == 0
                            )
                            {
                                student.SchoolMsStudent.AccountEntity.IdentificationDocumentTypeId =
                                    context.IdentificationDocumentTypes.FirstOrDefault().Id;
                            }
                            if (
                                student.SchoolMsStudent.AccountEntity.AccountEntityActiveStateId
                                == 0
                            )
                            {
                                student.SchoolMsStudent.AccountEntity.AccountEntityActiveStateId =
                                    context.AccountEntityActiveStates.FirstOrDefault().Id;
                            }

                            if (
                                string.IsNullOrEmpty(
                                    student
                                        .SchoolMsStudent
                                        .AccountEntity
                                        .IdentificationDocumentNumber
                                )
                            )
                            {
                                Random random = new Random();
                                student.SchoolMsStudent.AccountEntity.IdentificationDocumentNumber =
                                    "23d" + random.Next(1000);
                            }

                            studentDetails.Add(student);
                        }
                        ;

                        foreach (var student in studentDetails)
                        {
                            context.SchoolMsStudentClassHistories.Add(student);

                            var accountEntityToInsert = student.SchoolMsStudent.AccountEntity;
                            context.AccountEntities.Add(accountEntityToInsert);

                            var studentToInsert = student.SchoolMsStudent;
                            context.SchoolMsStudents.Add(studentToInsert);
                        }

                        await context.SaveChangesAsync();
                        executionResult.Message = "Student details have been saved succefully";
                    }
                }
            }
            catch (Exception ex)
            {
                executionResult.IsOkay = false;
                executionResult.Message = ex.Message;
            }

            return executionResult;
        }

        public Task<ExecutionResult<SchoolMsStudentClassHistory>> DownloadFile(
            IFormFileCollection files,
            String pathForUploadedFiled
        )
        {
            var executionResult = new ExecutionResult<SchoolMsStudentClassHistory>(true);

            try { }
            catch (Exception ex)
            {
                executionResult.IsOkay = false;
                executionResult.Message = ex.Message;
            }

            return Task.FromResult(executionResult);
        }

        public override SchoolMsStudentClassHistory CloneModel(SchoolMsStudentClassHistory model)
        {
            return new SchoolMsStudentClassHistory
            {
                SchoolMsClassId = model.SchoolMsClass.Id,
                SchoolMsStudentId = model.SchoolMsStudentId,
                AcademicYear = "" + DateTime.UtcNow.Year,
                IsCurrentClass = true,
                EntryGrade = model.EntryGrade,
                SchoolMsStudent = new SchoolMsStudent
                {
                    AccountEntityId = model.SchoolMsStudent.AccountEntityId,
                    RegionStudentIdentifier = model.EntryGrade,
                    SchoolMsSchoolId = model.SchoolMsStudent.SchoolMsSchool.Id,
                    AccountEntity = new AccountEntity
                    {
                        Id = model.SchoolMsStudent.AccountEntity.Id,
                        SubAccountBranchId = model.SchoolMsStudent.AccountEntity.SubAccountBranchId,
                        EntityNo = model.SchoolMsStudent.AccountEntity.EntityNo,
                        EntityName = model.SchoolMsStudent.AccountEntity.EntityName,
                        RegDate = DateTime.UtcNow,
                        DateOfBirth = model.SchoolMsStudent.AccountEntity.DateOfBirth,
                        Phone1 = model.SchoolMsStudent.AccountEntity.Phone1,
                        Phone2 = model.SchoolMsStudent.AccountEntity.Phone2,
                        Email = model.SchoolMsStudent.AccountEntity.Email,
                        IsActive = true,
                        PhysicalAddress = model.SchoolMsStudent.AccountEntity.PhysicalAddress,
                        PostalAddress = model.SchoolMsStudent.AccountEntity.PostalAddress,
                        WhiteListedDomain = model.SchoolMsStudent.AccountEntity.WhiteListedDomain,
                        EntityUserName = model.SchoolMsStudent.AccountEntity.EntityUserName,
                        IsBackUpRecord = model.SchoolMsStudent.AccountEntity.IsBackUpRecord,
                        EntityDetailsValidated = model
                            .SchoolMsStudent
                            .AccountEntity
                            .EntityDetailsValidated,
                        IdentificationDocumentNumber = model
                            .SchoolMsStudent
                            .AccountEntity
                            .IdentificationDocumentNumber,
                        IdentificationDocumentTypeId = model
                            .SchoolMsStudent
                            .AccountEntity
                            .IdentificationDocumentTypeId,
                        GenderId = model.SchoolMsStudent.AccountEntity.GenderId,
                    }
                }
            };
        }

        public override async Task<ExecutionResult<SchoolMsStudentClassHistory>> GetItem(
            int id,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsStudentClassHistory>(
                true,
                "Entry found"
            );
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                DbInstance = await context.SchoolMsStudentClassHistories.FirstOrDefaultAsync(
                    r => r.Id == id
                );
                if (DbInstance == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = $"The entry with id {id} supplied does not exist";
                    return executionResult;
                }

                //set the Result of executionResult object to the item found in the database
                //to avoid issues with serialization always use new

                // executionResult.Result = CloneModel(DbInstance);
            }
            catch (Exception ex)
            {
                //if an error occured copy its data from the exception object into the execution result by
                executionResult.CopyFromException(ex);
            }
            finally
            {
                //incase the context was created in this method then dispose it here
                if (createContextHere)
                    await context.DisposeAsync();
            }
            return executionResult;
        }

        public async Task<ExecutionResult<IEnumerable<SubAccount>>> GetSchools(
            NeedDataSourceEventArgs args
        )
        {
            var result = new ExecutionResult<IEnumerable<SubAccount>>(
                true,
                "",
                new HashSet<SubAccount>()
            );
            try
            {
                using (var context = new EFDatabaseModelDatabaseContext())
                {
                    args = NeedDataSourceEventArgs.EnsureArgsValid(args);
                    /*var filterValue = args.GetFilterValue(0).ToLower(); ;
                    if (string.IsNullOrEmpty(filterValue))
                        return result;*/

                    var schoolDetails = await context.SubAccounts
                        .Join(
                            context.SchoolMsSchools,
                            sa => sa.Id,
                            sc => sc.SubAccountId,
                            (sa, sc) => new { sa, sc }
                        )
                        .Where(r => true)
                        .OrderBy(r => r.sa.Id)
                        .Skip(args.skip)
                        .Take(args.take)
                        .ToListAsync();

                    result.Result = schoolDetails.Select(
                        r =>
                            new SubAccount
                            {
                                Id = r.sa.Id,
                                AccountName = r.sa.AccountName,
                                SchoolMsSchool = new SchoolMsSchool { Id = r.sc.Id }
                            }
                    );
                }
            }
            catch (Exception ex)
            {
                result = result.CopyFromException(ex);
            }
            return result;
        }

        public override async Task<ExecutionResult<SchoolMsStudentClassHistory>> Update(
            SchoolMsStudentClassHistory model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsStudentClassHistory>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                executionResult = await this.IsValidUpdate(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult; //if fail return the failed executionn result

                //dbInstance is updated in GETITEM in IsValidUpdate

                DbInstance.SchoolMsClassId = model.SchoolMsClassId;
                DbInstance.EntryGrade = model.EntryGrade;

                var studentDetailsToUpdate = context.SchoolMsStudents.FirstOrDefault(
                    r => r.Id == model.SchoolMsStudent.Id
                );
                studentDetailsToUpdate.RegionStudentIdentifier = model
                    .SchoolMsStudent
                    .RegionStudentIdentifier;
                studentDetailsToUpdate.SchoolMsSchoolId = model.SchoolMsStudent.SchoolMsSchool.Id;

                var accountEntityDetailsToUpdate = context.AccountEntities.FirstOrDefault(
                    r => r.Id == model.SchoolMsStudent.AccountEntity.Id
                );
                accountEntityDetailsToUpdate.EntityNo = model
                    .SchoolMsStudent
                    .AccountEntity
                    .EntityNo;
                accountEntityDetailsToUpdate.Phone1 = model.SchoolMsStudent.AccountEntity.Phone1;
                accountEntityDetailsToUpdate.EntityName = model
                    .SchoolMsStudent
                    .AccountEntity
                    .EntityName;
                accountEntityDetailsToUpdate.DateOfBirth = model
                    .SchoolMsStudent
                    .AccountEntity
                    .DateOfBirth;
                accountEntityDetailsToUpdate.GenderId = model
                    .SchoolMsStudent
                    .AccountEntity
                    .GenderId;
                accountEntityDetailsToUpdate.IsActive = model
                    .SchoolMsStudent
                    .AccountEntity
                    .IsActive;

                var studentClassHistory = context.SchoolMsStudentClassHistories.FirstOrDefault(
                    r => r.SchoolMsStudentId == model.SchoolMsStudent.Id && r.IsCurrentClass == true
                );
                if (studentClassHistory != null)
                {
                    studentClassHistory.SchoolMsClassId = model.SchoolMsClass.Id;
                }

                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Update operation was successful";
                    executionResult.Result = await GetModelForInsertUpdateFromId(
                        studentClassHistory.Id,
                        studentClassHistory,
                        context
                    );
                }
                else
                {
                    //incase the context was not created here return the object
                    //thats removed from the context
                    //helps the caller method to have log of the object in ChangeTraking inside DbContext
                    executionResult.Result = DbInstance;
                }
            }
            catch (Exception ex)
            {
                //if an error occured copy its data from the exception object into the execution result by
                executionResult.CopyFromException(ex);
            }
            finally
            {
                //incase the context was created in this method then dispose it here
                if (createContextHere)
                    await context.DisposeAsync();
            }
            return executionResult;
        }

        public override async Task<ExecutionResult<SchoolMsStudentClassHistory>> Delete(
            SchoolMsStudentClassHistory model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsStudentClassHistory>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                var studentDetailsToDelete = context.AccountEntities
                    .Join(
                        context.SchoolMsStudents,
                        ae => ae.Id,
                        st => st.AccountEntityId,
                        (ae, st) => new { ae, st }
                    )
                    .Join(
                        context.SchoolMsStudentClassHistories,
                        aest => aest.st.Id,
                        ch => ch.SchoolMsStudentId,
                        (aest, ch) => new { aest, ch }
                    )
                    .Join(
                        context.SchoolMsClasses,
                        aestch => aestch.ch.SchoolMsClassId,
                        cl => cl.Id,
                        (aestch, cl) => new { aestch, cl }
                    )
                    .Join(
                        context.Genders,
                        aestchg => aestchg.aestch.aest.ae.GenderId,
                        g => g.Id,
                        (aestchg, g) => new { aestchg, g }
                    )
                    .Join(
                        context.SchoolMsSchools,
                        aestchgsc => aestchgsc.aestchg.aestch.aest.st.SchoolMsSchoolId,
                        sc => sc.Id,
                        (aestchgsc, sc) => new { aestchgsc, sc }
                    )
                    .Join(
                        context.SubAccounts,
                        aestchgscsa => aestchgscsa.sc.SubAccountId,
                        sa => sa.Id,
                        (aestchgscsa, sa) => new { aestchgscsa, sa }
                    )
                    .FirstOrDefault(r => r.aestchgscsa.aestchgsc.aestchg.aestch.ch.Id == model.Id);

                if (studentDetailsToDelete == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message =
                        "The student record you are tring to deactivate does not exist";
                    return executionResult;
                }
                if (
                    studentDetailsToDelete.aestchgscsa.aestchgsc.aestchg.aestch.aest.ae.IsActive
                    == false
                )
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "The student is inactive";
                    return executionResult;
                }
                studentDetailsToDelete.aestchgscsa.aestchgsc.aestchg.aestch.aest.ae.IsActive =
                    false;

                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Student deactivated";
                    executionResult.Result = CloneModel(
                        studentDetailsToDelete.aestchgscsa.aestchgsc.aestchg.aestch.ch
                    );
                }
                else
                {
                    executionResult.Result = model;
                }
            }
            catch (Exception ex)
            {
                //if an error occured copy its data from the exception object into the execution result by
                executionResult.CopyFromException(ex);
            }
            finally
            {
                //incase the context was created in this method then dispose it here
                if (createContextHere)
                    await context.DisposeAsync();
            }
            return executionResult;
        }

        public override async Task<
            ExecutionResult<ItemsRequestedResult<NeedDataSourceEventArgs>>
        > GetKendoGridFiltered(
            NeedDataSourceEventArgs args,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<
                ItemsRequestedResult<NeedDataSourceEventArgs>
            >(true, "Class instance found");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                args = NeedDataSourceEventArgs.EnsureArgsValid(args);
                int filterValueIntId = -1;
                string filterValueId = string.Empty;
                if (args.HasFilterFields("Id"))
                    filterValueId = args.GetFilterValuePredicate(r => r.field == "Id", 0);

                var parsedInt = int.TryParse(filterValueId, out filterValueIntId);
                if (string.IsNullOrEmpty(filterValueId) || parsedInt == false)
                    filterValueIntId = -1;

                var studentDetails = (
                    await context.AccountEntities
                        .Join(
                            context.SchoolMsStudents,
                            ae => ae.Id,
                            st => st.AccountEntityId,
                            (ae, st) => new { ae, st }
                        )
                        .Join(
                            context.SchoolMsStudentClassHistories,
                            aest => aest.st.Id,
                            ch => ch.SchoolMsStudentId,
                            (aest, ch) => new { aest, ch }
                        )
                        .Join(
                            context.SchoolMsClasses,
                            aestch => aestch.ch.SchoolMsClassId,
                            cl => cl.Id,
                            (aestch, cl) => new { aestch, cl }
                        )
                        .Join(
                            context.Genders,
                            aestchg => aestchg.aestch.aest.ae.GenderId,
                            g => g.Id,
                            (aestchg, g) => new { aestchg, g }
                        )
                        .Join(
                            context.SchoolMsSchools,
                            aestchgsc => aestchgsc.aestchg.aestch.aest.st.SchoolMsSchoolId,
                            sc => sc.Id,
                            (aestchgsc, sc) => new { aestchgsc, sc }
                        )
                        .Join(
                            context.SubAccounts,
                            aestchgscsa => aestchgscsa.sc.SubAccountId,
                            sa => sa.Id,
                            (aestchgscsa, sa) => new { aestchgscsa, sa }
                        )
                        .Where(
                            r =>
                                filterValueIntId == -1
                                    ? true
                                    : r.aestchgscsa.aestchgsc.aestchg.aestch.ch.Id
                                        == filterValueIntId
                                        && r.aestchgscsa.aestchgsc.aestchg.aestch.ch.IsCurrentClass
                                            == true
                        )
                        .OrderByDescending(
                            r => r.aestchgscsa.aestchgsc.aestchg.aestch.aest.ae.IsActive
                        )
                        .Skip(0)
                        .Take(args.take * args.page)
                        .ToListAsync()
                ).Select(
                    r =>
                        new SchoolMsStudentClassHistory
                        {
                            Id = r.aestchgscsa.aestchgsc.aestchg.aestch.ch.Id,
                            EntryGrade = r.aestchgscsa.aestchgsc.aestchg.aestch.ch.EntryGrade,
                            IsCurrentClass = r.aestchgscsa
                                .aestchgsc
                                .aestchg
                                .aestch
                                .ch
                                .IsCurrentClass,
                            SchoolMsClassId = r.aestchgscsa
                                .aestchgsc
                                .aestchg
                                .aestch
                                .ch
                                .SchoolMsClassId,
                            SchoolMsClass = new SchoolMsClass
                            {
                                Id = r.aestchgscsa.aestchgsc.aestchg.cl.Id,
                                ClassName = r.aestchgscsa.aestchgsc.aestchg.cl.ClassName,
                            },
                            SchoolMsStudent = new SchoolMsStudent
                            {
                                Id = r.aestchgscsa.aestchgsc.aestchg.aestch.aest.st.Id,
                                AccountEntityId = r.aestchgscsa
                                    .aestchgsc
                                    .aestchg
                                    .aestch
                                    .aest
                                    .st
                                    .AccountEntityId,
                                RegionStudentIdentifier = r.aestchgscsa
                                    .aestchgsc
                                    .aestchg
                                    .aestch
                                    .aest
                                    .st
                                    .RegionStudentIdentifier,
                                SchoolMsSchool = new SchoolMsSchool
                                {
                                    Id = r.aestchgscsa.sc.Id,
                                    SubAccount = new SubAccount
                                    {
                                        Id = r.sa.Id,
                                        AccountName = r.sa.AccountName,
                                    }
                                },
                                AccountEntity = new AccountEntity
                                {
                                    Id = r.aestchgscsa.aestchgsc.aestchg.aestch.aest.ae.Id,
                                    IsActive = r.aestchgscsa
                                        .aestchgsc
                                        .aestchg
                                        .aestch
                                        .aest
                                        .ae
                                        .IsActive,
                                    Email = r.aestchgscsa.aestchgsc.aestchg.aestch.aest.ae.Email,
                                    Phone1 = r.aestchgscsa.aestchgsc.aestchg.aestch.aest.ae.Phone1,
                                    DateOfBirth = r.aestchgscsa
                                        .aestchgsc
                                        .aestchg
                                        .aestch
                                        .aest
                                        .ae
                                        .DateOfBirth,
                                    EntityName = r.aestchgscsa
                                        .aestchgsc
                                        .aestchg
                                        .aestch
                                        .aest
                                        .ae
                                        .EntityName,
                                    EntityNo = r.aestchgscsa
                                        .aestchgsc
                                        .aestchg
                                        .aestch
                                        .aest
                                        .ae
                                        .EntityNo,
                                    GenderId = r.aestchgscsa
                                        .aestchgsc
                                        .aestchg
                                        .aestch
                                        .aest
                                        .ae
                                        .GenderId,
                                    Gender = new Gender
                                    {
                                        Id = r.aestchgscsa.aestchgsc.g.Id,
                                        GenderName = r.aestchgscsa.aestchgsc.g.GenderName
                                    },
                                }
                            }
                        }
                );

                var filterSupporter = new FilterSupporter<SchoolMsStudentClassHistory>(
                    studentDetails,
                    args
                );
                var filteredData = filterSupporter.FilterData<long>(r => r.Id);
                executionResult.Result = filterSupporter.itemsResult;
            }
            catch (Exception ex)
            {
                //if an error occured copy its data from the exception object into the execution result by
                executionResult.CopyFromException(ex);
            }
            finally
            {
                //incase the context was created in this method then dispose it here
                if (createContextHere)
                    await context.DisposeAsync();
            }
            return executionResult;
        }

        public override async Task<ExecutionResult<SchoolMsStudentClassHistory>> IsValidGeneral(
            SchoolMsStudentClassHistory model,
            EFDatabaseModelDatabaseContext context
        )
        {
            return await Task.Run<ExecutionResult<SchoolMsStudentClassHistory>>(() =>
            {
                var result = new ExecutionResult<SchoolMsStudentClassHistory>(true);
                if (model == null)
                {
                    result.IsOkay = false;
                    result.Message = "model is null";
                    return result;
                }
                if (string.IsNullOrEmpty(model.SchoolMsStudent.AccountEntity.EntityName))
                {
                    result.IsOkay = false;
                    result.Message = "Please supply the student name";
                    return result;
                }
                if (string.IsNullOrEmpty(model.SchoolMsStudent.AccountEntity.Phone1))
                {
                    result.IsOkay = false;
                    result.Message = "Please supply the student's Primary phone number";
                    return result;
                }

                if (string.IsNullOrEmpty(model.SchoolMsStudent.AccountEntity.EntityNo))
                {
                    result.IsOkay = false;
                    result.Message = "Please supply the pupil ID number";
                    return result;
                }

                //var classNameInDb = context.SchoolMsClasses.FirstOrDefault(r =>  string.Equals(model.ClassName, r.ClassName, StringComparison.OrdinalIgnoreCase));
                var genderInDb = context.Genders.FirstOrDefault(
                    r => r.Id == model.SchoolMsStudent.AccountEntity.GenderId
                );
                if (genderInDb == null)
                {
                    result.IsOkay = false;
                    result.Message = "Please select gender";
                    return result;
                }
                var pupilId = context.AccountEntities.FirstOrDefault(
                    r =>
                        r.EntityNo.Equals(model.SchoolMsStudent.AccountEntity.EntityNo)
                        && r.Id != model.SchoolMsStudent.AccountEntity.Id
                );
                if (pupilId != null)
                {
                    result.IsOkay = false;
                    result.Message = "There is a differen student with the supplied pupil ID ";
                    return result;
                }
                var classInDb = context.SchoolMsClasses.FirstOrDefault(
                    r => r.Id == model.SchoolMsClass.Id
                );
                if (classInDb == null)
                {
                    result.IsOkay = false;
                    result.Message = "PLease select class";
                    return result;
                }

                return result;
            });
        }

        public override async Task<ExecutionResult<SchoolMsStudentClassHistory>> IsValidInsert(
            SchoolMsStudentClassHistory model,
            EFDatabaseModelDatabaseContext context
        )
        {
            var result = await this.IsValidGeneral(model, context);
            if (result.IsOkay == false)
                return result;

            return result;
        }

        public override async Task<ExecutionResult<SchoolMsStudentClassHistory>> IsValidUpdate(
            SchoolMsStudentClassHistory model,
            EFDatabaseModelDatabaseContext context
        )
        {
            //if it passes general validation then use get item to complete update validation
            var result = await this.IsValidGeneral(model, context);
            if (result.IsOkay == false)
                return result;

            result = await GetItem((int)model.Id, context);
            if (result.IsOkay == false)
                return result;

            return result;
        }
    }
}
