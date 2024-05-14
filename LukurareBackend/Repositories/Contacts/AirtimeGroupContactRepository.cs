using LukurareBackend.Models.NetworkPrefix;
using EFDatabaseModel.Contexts;
using EFDatabaseModel.DbModel;
using EFDatabaseModel.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using ProjectBase.KendoUiSupport;
using ProjectBase.Repository.Transaction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukurareBackend.Repositories.Contacts
{
    public class AirtimeGroupContactRepository
        : EFDatabaseModelBaseRepository<AccountEntityGroupContact>
    {
        public AirtimeGroupContactRepository(string currentModule)
            : base(currentModule) { }

        /// <summary>
        /// Helps to create a copy of the current Model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override AccountEntityGroupContact CloneModel(AccountEntityGroupContact model)
        {
            var returnValue = new AccountEntityGroupContact
            {
                Id = model.Id,
                AccountEntityGroupId = model.AccountEntityGroup.Id,
                AccountEntityContactId = model.AccountEntityContact.Id
            };

            return returnValue;
        }

        public async override Task<ExecutionResult<AccountEntityGroupContact>> Add(
            AccountEntityGroupContact model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<AccountEntityGroupContact>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                executionResult = await this.IsValidInsert(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult; //if fail return the failed executionn result

                var modelInsert = CloneModel(model);
                string[] safaricom =
                {
                    "0110",
                    "0111",
                    "0112",
                    "0113",
                    "0114",
                    "0115",
                    "0700",
                    "0701",
                    "0702",
                    "0703",
                    "0704",
                    "0705",
                    "0706",
                    "0707",
                    "0708",
                    "0710",
                    "0711",
                    "0712",
                    "0713",
                    "0714",
                    "0715",
                    "0716",
                    "0717",
                    "0718",
                    "0719",
                    "0720",
                    "0720",
                    "0721",
                    "0722",
                    "0723",
                    "0724",
                    "0725",
                    "0726",
                    "0727",
                    "0728",
                    "0729",
                    "0740",
                    "0741",
                    "0742",
                    "0743",
                    "0744",
                    "0745",
                    "0757",
                    "0758",
                    "0759",
                    "0769",
                    "0746",
                    "0748",
                    "0790",
                    "0791",
                    "0792",
                    "0793",
                    "0794",
                    "0795",
                    "0796",
                    "0797",
                    "0798",
                    "0799",
                    "0768"
                };
                string[] airtel =
                {
                    "0100",
                    "0101",
                    "0102",
                    "0103",
                    "0104",
                    "0105",
                    "0106",
                    "0730",
                    "0731",
                    "0732",
                    "0733",
                    "0734",
                    "0735",
                    "0736",
                    "0737",
                    "0738",
                    "0739",
                    "0750",
                    "0751",
                    "0752",
                    "0753",
                    "0754",
                    "0755",
                    "0756",
                    "0762",
                    "0780",
                    "0781",
                    "0782",
                    "0783",
                    "0784",
                    "0785",
                    "0786",
                    "0787",
                    "0788",
                    "0789"
                };
                string[] telkom =
                {
                    "0202",
                    "0770",
                    "0771",
                    "0772",
                    "0773",
                    "0774",
                    "0775",
                    "0776",
                    "0777",
                    "0778",
                    "0779"
                };
                var network = context.BillReferenceServiceTypes.FirstOrDefault();
                if (model.AccountEntityContact.ContactPhone.StartsWith("+254"))
                {
                    model.AccountEntityContact.ContactPhone =
                        model.AccountEntityContact.ContactPhone.Replace("+254", "0");
                }
                if (model.AccountEntityContact.ContactPhone.StartsWith("254"))
                {
                    string x = model.AccountEntityContact.ContactPhone.Substring(3);
                    model.AccountEntityContact.ContactPhone = "0" + x;
                }
                //var group= context.AccountEntityGroups.FirstOrDefault(r => r.Id.Equals(model.AccountEntityGroup.Id));
                var pIdInDb = context.AccountEntityContacts.FirstOrDefault(
                    r =>
                        r.ContactPhone.Equals(model.AccountEntityContact.ContactPhone)
                        && r.AccountEntityId == UserPrinciple.User.AccountEntityId
                );
                if (pIdInDb == null)
                {
                    if (telkom.Contains(model.AccountEntityContact.ContactPhone.Substring(0, 4)))
                    {
                        network = context.BillReferenceServiceTypes.FirstOrDefault(
                            r => r.ServiceId == "2"
                        );
                    }
                    else if (
                        airtel.Contains(model.AccountEntityContact.ContactPhone.Substring(0, 4))
                    )
                    {
                        network = context.BillReferenceServiceTypes.FirstOrDefault(
                            r => r.ServiceId == "1"
                        );
                    }
                    else if (
                        safaricom.Contains(model.AccountEntityContact.ContactPhone.Substring(0, 4))
                    )
                    {
                        network = context.BillReferenceServiceTypes.FirstOrDefault(
                            r => r.ServiceId == "3"
                        );
                    }
                    var contact = new AccountEntityContact
                    {
                        Id = model.AccountEntityContact.Id,
                        ContactPhone = model.AccountEntityContact.ContactPhone,
                        ContactEmail = model.AccountEntityContact.ContactEmail,
                        ContactName = model.AccountEntityContact.ContactName,
                        CountryId = model.AccountEntityContact.Country.Id,
                        BillReferenceServiceTypeId = network.Id,
                        AccountEntityId = UserPrinciple.User.AccountEntityId
                    };

                    modelInsert.AccountEntityContact = contact;
                }
                else
                {
                    modelInsert.AccountEntityContactId = model.AccountEntityContact.Id; //pIdInDb.Id;
                }

                modelInsert.AccountEntityGroupId = model.AccountEntityGroup.Id; // group.Id;

                context.AccountEntityGroupContacts.Add(modelInsert);

                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Create operation was successful";
                    //return a new instance of the inserted object
                    // executionResult.Result = CloneModel(modelInsert);
                    // executionResult.Result = await GetModelForInsertUpdateFromId(modelInsert.Id, modelInsert, context);
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

        public async override Task<ExecutionResult<AccountEntityGroupContact>> Delete(
            AccountEntityGroupContact model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<AccountEntityGroupContact>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //use group and contact id to retrieve the group contact

                var groupContact = context.AccountEntityGroupContacts.FirstOrDefault(
                    r =>
                        r.AccountEntityContactId.Equals(model.AccountEntityContact.Id)
                        && r.AccountEntityGroupId.Equals(model.AccountEntityGroup.Id)
                );
                //Get item will set the DbInstance using the current context if Succefully looaded object
                executionResult = await IsValidGeneral(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult;

                executionResult = await GetItem((int)groupContact.Id, context);
                if (executionResult.IsOkay == false)
                    return executionResult;

                context.AccountEntityGroupContacts.Remove(DbInstance);
                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Delete operation was successful";
                    ///executionResult.Result = CloneModel(DbInstance);
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

        public async override Task<ExecutionResult<AccountEntityGroupContact>> GetItem(
            int id,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<AccountEntityGroupContact>(
                true,
                "Entry found"
            );
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                DbInstance = await context.AccountEntityGroupContacts.FirstOrDefaultAsync(
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
                executionResult.Result = DbInstance;
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

        public async override Task<
            ExecutionResult<ItemsRequestedResult<NeedDataSourceEventArgs>>
        > GetKendoGridFiltered(
            NeedDataSourceEventArgs args,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<
                ItemsRequestedResult<NeedDataSourceEventArgs>
            >(true, "AccountEntityGroupContact instance found");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                args = NeedDataSourceEventArgs.EnsureArgsValid(args);
                ////since this will be loaded from a detail table of SmsGateway then ensure the Filter SmsGateway Id Is Passed
                int filterValueInt = -1;
                string filterValue = null;
                if (args.HasFilterFields("AccountEntityGroupId"))
                    filterValue = args.GetFilterValuePredicate(
                        r => r.field == "AccountEntityGroupId",
                        0
                    );
                //int filterValueInt = -1;
                //var  filterValue = GroupID;
                bool parsedInt = int.TryParse(filterValue, out filterValueInt);
                if (string.IsNullOrEmpty(filterValue) == false && parsedInt == false)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = string.Format(
                        "Sorry Account Entity Group Id filter value has to be provided that is a valid integer",
                        new object[0]
                    );
                    return executionResult;
                }
                var senders = (
                    await context.AccountEntityGroupContacts
                        .Join(
                            context.AccountEntityGroups,
                            gc => gc.AccountEntityGroupId,
                            g => g.Id,
                            (gc, g) => new { gc, g }
                        )
                        .Join(
                            context.AccountEntityContacts,
                            x => x.gc.AccountEntityContactId,
                            c => c.Id,
                            (x, c) => new { x, c }
                        )
                        .Join(
                            context.Countries,
                            y => y.c.CountryId,
                            country => country.Id,
                            (y, country) => new { y, country }
                        )
                        .Join(
                            context.BillReferenceServiceTypes,
                            z => z.y.c.BillReferenceServiceTypeId,
                            bill => bill.Id,
                            (z, bill) => new { z, bill }
                        )
                        .Where(r => r.z.y.c.AccountEntityId == UserPrinciple.User.AccountEntityId)
                        .Where(
                            r =>
                                parsedInt ? r.z.y.x.gc.AccountEntityGroupId == filterValueInt : true
                        )
                        .Skip(0)
                        .Take(50)
                        .OrderBy(r => r.z.y.c.ContactName)
                        .ToListAsync()
                ).Select(
                    r =>
                        new AccountEntityGroupContact
                        {
                            Id = r.z.y.x.gc.Id,
                            AccountEntityGroupId = r.z.y.x.gc.AccountEntityGroupId,
                            AccountEntityGroup = new AccountEntityGroup { Id = r.z.y.x.g.Id },
                            AccountEntityContact = new AccountEntityContact
                            {
                                Id = r.z.y.c.Id,
                                ContactName = r.z.y.c.ContactName,
                                ContactPhone = r.z.y.c.ContactPhone,
                                ContactEmail = r.z.y.c.ContactEmail,
                                Country = new Country
                                {
                                    Id = r.z.country.Id,
                                    CountryCode = r.z.country.CountryCode,
                                    CountryName = r.z.country.CountryName
                                },
                                BillReferenceServiceType = new BillReferenceServiceType
                                {
                                    Id = r.bill.Id,
                                    ServiceName = r.bill.ServiceName
                                }
                            }
                        }
                );
                args.filter = null;
                var filterSupporter = new FilterSupporter<AccountEntityGroupContact>(senders, args);
                var filteredData = filterSupporter.FilterData<int>(r => (int)r.Id);
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

        public override Task<ExecutionResult<AccountEntityGroupContact>> IsValidGeneral(
            AccountEntityGroupContact model,
            EFDatabaseModelDatabaseContext context
        )
        {
            //since this IsValid general does have a Asyn Call force it to be asyn by using task.Run

            var result = new ExecutionResult<AccountEntityGroupContact>(true);
            if (model == null)
            {
                result.IsOkay = false;
                result.Message = "model is null";
                return Task.FromResult(result);
            }

            return Task.FromResult(result);
        }

        public async override Task<ExecutionResult<AccountEntityGroupContact>> IsValidInsert(
            AccountEntityGroupContact model,
            EFDatabaseModelDatabaseContext context
        )
        {
            var result = await this.IsValidGeneral(model, context);
            if (result.IsOkay == false)
                return result;

            //perform insert specific validation
            if (model == null)
            {
                result.IsOkay = false;
                result.Message = "Model is null";
                return result;
            }
            if (model.AccountEntityContact.ContactPhone == null)
            {
                result.IsOkay = false;
                result.Message = "Contact is not supplied";
                return result;
            }

            return result;
        }

        public async override Task<ExecutionResult<AccountEntityGroupContact>> IsValidUpdate(
            AccountEntityGroupContact model,
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

            //ensure that there is not other object with a diffrent ID in db with the same country code as this one
            return result;
        }

        public async override Task<ExecutionResult<AccountEntityGroupContact>> Update(
            AccountEntityGroupContact model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<AccountEntityGroupContact>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                executionResult = await this.IsValidUpdate(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult; //if fail return the failed executionn result

                //dbInstance is updated in GETITEM in IsValidUpdate
                //DbInstance.AccountEntityGroupId = model.AccountEntityGroupId;
                //DbInstance.AccountEntityContactId = model.AccountEntityContactId;
                var record = context.AccountEntityContacts.FirstOrDefault(
                    r => r.Id.Equals(DbInstance.AccountEntityContact.Id)
                );
                if (record != null)
                {
                    record.ContactPhone = model.AccountEntityContact.ContactPhone;
                    record.ContactName = model.AccountEntityContact.ContactName;
                    record.ContactEmail = model.AccountEntityContact.ContactEmail;
                    //record.AccountEntityId = record.AccountEntityId;
                    record.CountryId = model.AccountEntityContact.Country.Id;
                    DbInstance.AccountEntityContact = record;
                }

                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Update operation was successful";
                    executionResult.Result = await GetModelForInsertUpdateFromId(
                        DbInstance.Id,
                        DbInstance,
                        context
                    );
                    //executionResult.Result = CloneModel(DbInstance);
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

        #region fill combo boxes
        public async Task<ExecutionResult<IEnumerable<Country>>> GetCountryCode(
            NeedDataSourceEventArgs args
        )
        {
            var result = new ExecutionResult<IEnumerable<Country>>(
                true,
                "",
                new HashSet<Country>()
            );
            try
            {
                using (var context = new EFDatabaseModelDatabaseContext())
                {
                    // args = NeedDataSourceEventArgs.EnsureArgsValid(args);
                    //var filterValue = args.GetFilterValue(0).ToLower(); ;
                    //if (string.IsNullOrEmpty(filterValue))
                    //    return result;

                    var returnData = await context.Countries
                        .Where(r => true)
                        .OrderBy(r => r.Id)
                        .Skip(0)
                        .Take(10)
                        .ToListAsync();

                    result.Result = returnData.Select(
                        r =>
                            new Country
                            {
                                Id = r.Id,
                                CountryName = r.CountryName,
                                CountryCode = r.CountryCode
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

        public async Task<ExecutionResult<IEnumerable<BillReferenceServiceType>>> GetNetworks(
            NeedDataSourceEventArgs args
        )
        {
            var result = new ExecutionResult<IEnumerable<BillReferenceServiceType>>(
                true,
                "",
                new HashSet<BillReferenceServiceType>()
            );
            try
            {
                using (var context = new EFDatabaseModelDatabaseContext())
                {
                    // args = NeedDataSourceEventArgs.EnsureArgsValid(args);
                    //var filterValue = args.GetFilterValue(0).ToLower(); ;
                    //if (string.IsNullOrEmpty(filterValue))
                    //    return result;

                    var returnData = await context.BillReferenceServiceTypes
                        .Where(r => true)
                        .OrderBy(r => r.Id)
                        .Skip(0)
                        .Take(10)
                        .ToListAsync();

                    result.Result = returnData.Select(
                        r => new BillReferenceServiceType { Id = r.Id, ServiceName = r.ServiceName }
                    );
                }
            }
            catch (Exception ex)
            {
                result = result.CopyFromException(ex);
            }
            return result;
        }

        public async Task<ExecutionResult<IEnumerable<AccountEntityGroup>>> GetGroups(
            NeedDataSourceEventArgs args
        )
        {
            var result = new ExecutionResult<IEnumerable<AccountEntityGroup>>(
                true,
                "",
                new HashSet<AccountEntityGroup>()
            );
            try
            {
                using (var context = new EFDatabaseModelDatabaseContext())
                {
                    // args = NeedDataSourceEventArgs.EnsureArgsValid(args);
                    //var filterValue = args.GetFilterValue(0).ToLower(); ;
                    //if (string.IsNullOrEmpty(filterValue))
                    //    return result;r.AccountEntityId.Equals(UserPrinciple.User.AccountEntityId

                    var returnData = await context.AccountEntityGroups
                        .Where(r => r.AccountEntityId.Equals(UserPrinciple.User.AccountEntityId))
                        .OrderBy(r => r.Id)
                        .Skip(0)
                        .Take(10)
                        .ToListAsync();

                    result.Result = returnData.Select(
                        r => new AccountEntityGroup { Id = r.Id, GroupName = r.GroupName }
                    );
                }
            }
            catch (Exception ex)
            {
                result = result.CopyFromException(ex);
            }
            return result;
        }

        public async Task<ExecutionResult<AccountEntityGroupContact>> UploadContacts(
            IFormFileCollection files,
            string path
        )
        {
            var executionResult = new ExecutionResult<AccountEntityGroupContact>(true);

            try
            {
                long uploaded_size = 0;

                string path_for_Uploaded_Files = $"{path}\\UploadedFiles\\";

                //</ init >



                //< get form_files >

                //IFormFile uploaded_File

                var uploaded_files = files;

                //</ get form_files >



                //------< @Loop: Uploaded Files >------

                int iCounter = 0;

                string sFiles_uploaded = "";

                string uploaded_Filename = "";
                string new_Filename_on_Server = "";

                foreach (var uploaded_file in uploaded_files)
                {
                    //----< Uploaded File >----

                    iCounter++;

                    uploaded_size += uploaded_file.Length;

                    sFiles_uploaded += "\n" + uploaded_file.FileName;

                    //< Filename >

                    uploaded_Filename = uploaded_file.FileName;

                    new_Filename_on_Server =
                        $"{path_for_Uploaded_Files}\\{Guid.NewGuid().ToString()}{uploaded_Filename}";

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

                //
                var contactsList = new List<AccountEntityGroupContact>();
                FileInfo existingFile = new FileInfo(new_Filename_on_Server);
                using (var package = new ExcelPackage(existingFile))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.FirstOrDefault();

                    if (workSheet == null)
                    {
                        executionResult.IsOkay = false;
                        executionResult.Message = "No worksheet found";
                        return executionResult;
                    }
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;

                    using (
                        EFDatabaseModelDatabaseContext context =
                            new EFDatabaseModelDatabaseContext()
                    )
                    {
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            string name = null,
                                Phone = null,
                                Email = null,
                                group = null;
                            if (workSheet.Cells[rowIterator, 1].Value != null)
                            {
                                name = workSheet.Cells[rowIterator, 1].Value.ToString();
                            }
                            if (workSheet.Cells[rowIterator, 2].Value != null)
                            {
                                Phone = workSheet.Cells[rowIterator, 2].Value.ToString();
                            }
                            if (workSheet.Cells[rowIterator, 3].Value != null)
                            {
                                Email = workSheet.Cells[rowIterator, 3].Value.ToString();
                            }
                            if (workSheet.Cells[rowIterator, 4].Value != null)
                            {
                                group = workSheet.Cells[rowIterator, 4].Value.ToString();
                            }
                            //if (workSheet.Cells[rowIterator, 5].Value != null)
                            //{
                            //    group = workSheet.Cells[rowIterator, 5].Value.ToString();

                            //}
                            //if (workSheet.Cells[rowIterator, 6].Value != null)
                            //{
                            //    network = workSheet.Cells[rowIterator, 6].Value.ToString();

                            //}




                            if (!(name == null && Phone == null && group == null))
                            {
                                if (string.IsNullOrEmpty(Phone))
                                {
                                    executionResult.IsOkay = false;
                                    executionResult.Message = "Please supply all Phone Numbers";
                                    return executionResult;
                                }
                                var groupRecord = context.AccountEntityGroups.FirstOrDefault(
                                    r =>
                                        r.GroupName.Equals(group)
                                        && r.AccountEntityId == UserPrinciple.User.AccountEntityId
                                );
                                if (groupRecord == null)
                                {
                                    var newgroup = new AccountEntityGroup
                                    {
                                        GroupName = group,
                                        AccountEntityId = UserPrinciple.User.AccountEntityId
                                    };
                                    context.AccountEntityGroups.Add(newgroup);
                                    await context.SaveChangesAsync();
                                    groupRecord = newgroup;

                                    //executionResult.IsOkay = false;
                                    //executionResult.Message = "This group name does not exist";
                                    //return executionResult;
                                }

                                var countryRecord = context.Countries.FirstOrDefault(
                                    r => r.CountryName.Equals("Kenya")
                                );
                                //if (countryRecord == null)
                                //{
                                //    executionResult.IsOkay = false;
                                //    executionResult.Message = "This country name does not exist";
                                //    return executionResult;
                                //}
                                //var networkRecord = context.BillReferenceServiceTypes.FirstOrDefault(r => r.ServiceName.Equals(network));
                                //if (networkRecord == null)
                                //{
                                //    executionResult.IsOkay = false;
                                //    executionResult.Message = "This bill reference service name does not exist";
                                //    return executionResult;
                                //}

                                var pIdInDb = context.AccountEntityContacts.FirstOrDefault(
                                    r =>
                                        r.ContactPhone.Equals(Phone)
                                        && r.AccountEntityId == UserPrinciple.User.AccountEntityId
                                );
                                if (pIdInDb == null)
                                {
                                    var row = new AccountEntityGroupContact
                                    {
                                        AccountEntityContact = new AccountEntityContact
                                        {
                                            Id = 0,
                                            ContactPhone = Phone,
                                            ContactName = name,
                                            ContactEmail = Email,
                                            AccountEntityId = UserPrinciple.User.AccountEntityId,
                                            Country = countryRecord
                                            //BillReferenceServiceType=networkRecord
                                        },
                                        AccountEntityGroup = groupRecord
                                    };
                                    contactsList.Add(row);
                                }
                                else
                                {
                                    var row = new AccountEntityGroupContact
                                    {
                                        AccountEntityContact = pIdInDb,
                                        AccountEntityGroup = groupRecord
                                    };
                                    contactsList.Add(row);
                                }
                            }
                        }
                    }

                    foreach (var row in contactsList)
                    {
                        executionResult = await Add(row);
                    }

                    //if (executionResult.IsOkay == false)
                    //{
                    //    return executionResult;
                    //}
                    //else
                    //{
                    //    await context.SaveChangesAsync();
                    //    executionResult.Message = "Contacts saved successfully";
                    //}
                }
            }
            catch (Exception ex) { }

            return executionResult;
        }

        #endregion
    }
}
