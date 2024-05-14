using EFDatabaseModel.Contexts;
using EFDatabaseModel.DbModel;
using EFDatabaseModel.Models.Authentication;
using EFDatabaseModel.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectBase.KendoUiSupport;
using ProjectBase.Repository.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukurareBackend.Repositories.Contacts
{
    public class AccountEntityContactRepository
        : EFDatabaseModelBaseRepository<AccountEntityContact>
    {
        public AccountEntityContactRepository(string currentModule)
            : base(currentModule) { }

        /// <summary>
        /// Helps to create a copy of the current Model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override AccountEntityContact CloneModel(AccountEntityContact model)
        {
            var returnValue = new AccountEntityContact
            {
                Id = model.Id,
                ContactName = model.ContactName,
                AccountEntityId = UserPrinciple.User.AccountEntityId,
                ContactPhone = model.ContactPhone,
                ContactEmail = model.ContactEmail,
                CountryId = model.Country.Id,
                //BillReferenceServiceTypeId= model.Country.Id
            };

            return returnValue;
        }

        public async override Task<ExecutionResult<AccountEntityContact>> Add(
            AccountEntityContact model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<AccountEntityContact>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                executionResult = await this.IsValidInsert(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult; //if fail return the failed executionn result

                //check if the phone number exist





                //insert the okay object to the DB
                var modelInsert = CloneModel(model);
                if (modelInsert.ContactPhone.StartsWith("+254"))
                {
                    modelInsert.ContactPhone = modelInsert.ContactPhone.Replace("+254", "0");
                }
                if (modelInsert.ContactPhone.StartsWith("254"))
                {
                    string x = modelInsert.ContactPhone.Substring(3);
                    modelInsert.ContactPhone = "0" + x;
                }
                var contactExist = context.AccountEntityContacts.FirstOrDefault(
                    r =>
                        r.ContactPhone.Equals(modelInsert.ContactPhone)
                        && r.AccountEntityId == UserPrinciple.User.AccountEntityId
                );
                if (contactExist != null)
                {
                    executionResult.Message = "A contact with this phone number already exists";
                    return executionResult; //if fail return the failed executionn result
                }
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

                if (telkom.Contains(modelInsert.ContactPhone.Substring(0, 4)))
                {
                    network = context.BillReferenceServiceTypes.FirstOrDefault(
                        r => r.ServiceId == "2"
                    );
                }
                else if (airtel.Contains(modelInsert.ContactPhone.Substring(0, 4)))
                {
                    network = context.BillReferenceServiceTypes.FirstOrDefault(
                        r => r.ServiceId == "1"
                    );
                }
                else if (safaricom.Contains(modelInsert.ContactPhone.Substring(0, 4)))
                {
                    network = context.BillReferenceServiceTypes.FirstOrDefault(
                        r => r.ServiceId == "3"
                    );
                }
                modelInsert.BillReferenceServiceTypeId = network.Id;
                context.AccountEntityContacts.Add(modelInsert);

                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Create operation was successful";
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

        public async override Task<ExecutionResult<AccountEntityContact>> Update(
            AccountEntityContact model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<AccountEntityContact>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                executionResult = await this.IsValidUpdate(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult; //if fail return the failed executionn result

                //dbInstance is updated in GETITEM in IsValidUpdate



                DbInstance.ContactName = model.ContactName;
                DbInstance.ContactPhone = model.ContactPhone;
                DbInstance.ContactEmail = model.ContactEmail;

                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Update operation was successful";
                    //executionResult.Result = await GetModelForInsertUpdateFromId(DbInstance.Id, DbInstance, context);
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

        public async override Task<ExecutionResult<AccountEntityContact>> Delete(
            AccountEntityContact model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<AccountEntityContact>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //use get item to check if this ID is valid and incase it is
                //Get item will set the DbInstance using the current context if Succefully looaded object
                executionResult = await IsValidGeneral(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult;

                executionResult = await GetItem((int)model.Id, context);
                if (executionResult.IsOkay == false)
                    return executionResult;
                var groupContact = context.AccountEntityGroupContacts.FirstOrDefault(
                    r => r.AccountEntityContactId.Equals(model.Id)
                );
                if (groupContact != null)
                {
                    var group = context.AccountEntityGroups.FirstOrDefault(
                        r => r.Id.Equals(groupContact.AccountEntityGroupId)
                    );
                    executionResult.Message =
                        $"The Group {group.GroupName} has this contact.Delete it from the group first!";

                    return executionResult;
                }
                var billref = context.BillReferenceServiceRequests.FirstOrDefault(
                    r => r.AccountEntityContactId.Equals(model.Id)
                );
                if (billref != null)
                {
                    //var group = context.AccountEntityGroups.FirstOrDefault(r => r.Id.Equals(groupContact.AccountEntityGroupId));
                    executionResult.Message = "This contact has  credit record attached to it!";

                    return executionResult;
                }

                context.AccountEntityContacts.Remove(DbInstance);
                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Delete operation was successful";
                    executionResult.Result = DbInstance;
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

        public async override Task<ExecutionResult<AccountEntityContact>> GetItem(
            int id,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<AccountEntityContact>(true, "Entry found");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                DbInstance = await context.AccountEntityContacts.FirstOrDefaultAsync(
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
            >(true, "AccountEntityContact instance found");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                args = NeedDataSourceEventArgs.EnsureArgsValid(args);
                //since this will be loaded from a detail table of SmsGateway then ensure the Filter SmsGateway Id Is Passed
                int filterValueInt = -1;
                string filterValue = null;
                if (args.HasFilterFields("AccountEntityId"))
                    filterValue = args.GetFilterValuePredicate(
                        r => r.field == "AccountEntityId",
                        0
                    );

                bool parsedInt = int.TryParse(filterValue, out filterValueInt);
                if (string.IsNullOrEmpty(filterValue) == false && parsedInt == false)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = string.Format(
                        "Sorry Account Entity Id filter value has to be provided that is a valid integer",
                        new object[0]
                    );
                    return executionResult;
                }
                var senders = (
                    await context.AccountEntityContacts
                        .Join(
                            context.AccountEntities,
                            aeg => aeg.AccountEntityId,
                            ae => ae.Id,
                            (aeg, ae) => new { aeg, ae }
                        )
                        .Join(
                            context.Countries,
                            x => x.aeg.CountryId,
                            c => c.Id,
                            (x, c) => new { x, c }
                        )
                        .Join(
                            context.BillReferenceServiceTypes,
                            y => y.x.aeg.BillReferenceServiceTypeId,
                            z => z.Id,
                            (y, z) => new { y, z }
                        )
                        .Where(r => true)
                        //.Where(r => parsedInt ? r.x.aeg.AccountEntityId == filterValueInt : true)
                        .Where(r => r.y.x.aeg.AccountEntityId == UserPrinciple.User.AccountEntityId)
                        .Skip(args.skip)
                        .Take(args.take)
                        .OrderBy(r => r.y.x.aeg.ContactName)
                        .ToListAsync()
                ).Select(
                    r =>
                        new AccountEntityContact
                        {
                            Id = r.y.x.aeg.Id,
                            ContactName = r.y.x.aeg.ContactName,
                            ContactPhone = r.y.x.aeg.ContactPhone,
                            ContactEmail = r.y.x.aeg.ContactEmail,
                            AccountEntity = new AccountEntity { Id = r.y.x.ae.Id },
                            Country = new Country
                            {
                                Id = r.y.c.Id,
                                CountryCode = r.y.c.CountryCode,
                                CountryName = r.y.c.CountryName
                            },
                            BillReferenceServiceType = new BillReferenceServiceType
                            {
                                Id = r.z.Id,
                                ServiceName = r.z.ServiceName
                            }
                        }
                );
                args.filter = null;
                var filterSupporter = new FilterSupporter<AccountEntityContact>(senders, args);
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

        public override Task<ExecutionResult<AccountEntityContact>> IsValidGeneral(
            AccountEntityContact model,
            EFDatabaseModelDatabaseContext context
        )
        {
            //since this IsValid general does have a Asyn Call force it to be asyn by using task.Run

            var result = new ExecutionResult<AccountEntityContact>(true);
            if (model == null)
            {
                result.IsOkay = false;
                result.Message = "model is null";
                return Task.FromResult(result);
            }

            return Task.FromResult(result);
        }

        public async override Task<ExecutionResult<AccountEntityContact>> IsValidInsert(
            AccountEntityContact model,
            EFDatabaseModelDatabaseContext context
        )
        {
            var result = await this.IsValidGeneral(model, context);
            if (result.IsOkay == false)
                return result;

            if (this.UserPrinciple == null)
            {
                result.IsOkay = false;
                result.Message = "UserPrinciple is null";
                return result;
            }
            if (this.UserPrinciple.User == null)
            {
                result.IsOkay = false;
                result.Message = "UserPrinciple.User is null";
                return result;
            }
            model.AccountEntityId = UserPrinciple.User.AccountEntityId;

            return result;
        }

        public async override Task<ExecutionResult<AccountEntityContact>> IsValidUpdate(
            AccountEntityContact model,
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

        //public async override Task<ExecutionResult<AccountEntityContact>> Update(AccountEntityContact model, EFDatabaseModelDatabaseContext context = null)
        //{
        //    //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
        //    var createContextHere = context == null;
        //    var executionResult = new ExecutionResult<AccountEntityContact>(true);
        //    try
        //    {
        //        if (createContextHere)
        //            context = new EFDatabaseModelDatabaseContext();

        //        //validate wether the supplied model object is valid
        //        executionResult = await this.IsValidUpdate(model, context);
        //        if (executionResult.IsOkay == false)
        //            return executionResult;//if fail return the failed executionn result

        //        //dbInstance is updated in GETITEM in IsValidUpdate



        //        DbInstance.ContactName = model.ContactName;
        //        DbInstance.ContactPhone = model.ContactPhone;
        //        DbInstance.ContactEmail = model.ContactEmail;

        //        if (createContextHere)
        //        {
        //            await context.SaveChangesAsync();
        //            executionResult.Message = "Update operation was successful";
        //            //executionResult.Result = await GetModelForInsertUpdateFromId(DbInstance.Id, DbInstance, context);
        //            //executionResult.Result = CloneModel(DbInstance);
        //        }
        //        else
        //        {
        //            //incase the context was not created here return the object
        //            //thats removed from the context
        //            //helps the caller method to have log of the object in ChangeTraking inside DbContext
        //            executionResult.Result = DbInstance;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //if an error occured copy its data from the exception object into the execution result by
        //        executionResult.CopyFromException(ex);
        //    }
        //    finally
        //    {
        //        //incase the context was created in this method then dispose it here
        //        if (createContextHere)
        //            await context.DisposeAsync();
        //    }
        //    return executionResult;
        //}
        public async Task<ExecutionResult<AccountEntityContact>> CheckExistingContact(
            AccountEntityContact model
        )
        {
            using (var context = new EFDatabaseModelDatabaseContext())
            {
                var list = await context.AccountEntityContacts
                    .OrderByDescending(r => r.Id)
                    .FirstOrDefaultAsync(
                        r =>
                            r.ContactPhone == model.ContactPhone
                            && r.AccountEntityId == UserPrinciple.User.AccountEntityId
                    );

                return new ExecutionResult<AccountEntityContact>(true, "", list);
            }
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
                    //args = NeedDataSourceEventArgs.EnsureArgsValid(args);
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

        #endregion
    }
}
