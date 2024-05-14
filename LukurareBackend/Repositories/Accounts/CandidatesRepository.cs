using EFDatabaseModel.Contexts;
using EFDatabaseModel.DbModel;
using EFDatabaseModel.Repository;
using LukurareBackend.Repositories.Entities;
using Microsoft.EntityFrameworkCore;
using ProjectBase.Database.Connection;
using ProjectBase.KendoUiSupport;
using ProjectBase.Repository.Configuration;
using ProjectBase.Repository.Transaction;
using RestSharp;
using LukurareBackend.Models;
using LukurareBackend.Models.Resume;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukurareBackend.Repositories.Accounts
{
    public class CandidatesRepository : PatnerAccountEntityRepository
    {
        public CandidatesRepository(string currentModule)
           : base(currentModule)
        { }

        public async Task<ExecutionResult<List<AccountEntity>>> GetAllCandidates()
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    //var accountEntityId = id;

                    var executionResult = new ExecutionResult<List<AccountEntity>>(true);

                    var allCandidates = await context.AccountEntities
                        .Join(context.AccountEntityTypes, ae => ae.AccountEntityTypeId, t => t.Id, (ae, t) => new { ae, t })                        
                        .Where(ts => ts.t.TypeName == DefaultConfiguration.AccountEntityType.InternTypeName)
                        .Select(k => new AccountEntity
                        {                            
                                Id = k.ae.Id,
                                EntityName = k.ae.EntityName,
                                PhysicalAddress = k.ae.PhysicalAddress,
                                ProfileImageUrl = k.ae.ProfileImageUrl

                        })
                        .ToListAsync();

                    executionResult.Result = allCandidates;

                    return executionResult;
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here, such as logging the exception details or throwing a custom exception.
                // You can also consider returning a specific error result instead.

                // Example: Log the exception
                Console.WriteLine($"An exception occurred: {ex}");

                // Example: Throw a custom exception
                throw new Exception("An error occurred while checking additional details", ex);
            }
        }

        public async Task<ExecutionResult<List<AccountEntity>>> GetMainCompanies()
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    //var accountEntityId = id;

                    var executionResult = new ExecutionResult<List<AccountEntity>>(true);

                    var allCandidates = await context.AccountEntities
                        .Join(context.AccountEntityTypes, ae => ae.AccountEntityTypeId, t => t.Id, (ae, t) => new { ae, t })
                        .Where(ts => ts.t.TypeName == DefaultConfiguration.AccountEntityType.companyTypeName)
                        .Select(k => new AccountEntity
                        {
                            Id = k.ae.Id,
                            EntityName = k.ae.EntityName,
                            PhysicalAddress = k.ae.PhysicalAddress,
                            ProfileImageUrl = k.ae.ProfileImageUrl,
                            PostalAddress = k.ae.PostalAddress
                        })
                        .ToListAsync();

                    executionResult.Result = allCandidates;

                    return executionResult;
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here, such as logging the exception details or throwing a custom exception.
                // You can also consider returning a specific error result instead.

                // Example: Log the exception
                Console.WriteLine($"An exception occurred: {ex}");

                // Example: Throw a custom exception
                throw new Exception("An error occurred while checking additional details", ex);
            }
        }

        public async Task<ExecutionResult<List<AccountEntityTypeAdditionalDetailsValue>>> GetCandidatesDetails(int id)
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = id;

                    var executionResult = new ExecutionResult<List<AccountEntityTypeAdditionalDetailsValue>>(true);

                    var additionalDetails = await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, ae => ae.TypeAdditionalDetailsId, t => t.Id, (ae, t) => new { ae, t })
                        .Join(context.AccountEntities, u => u.ae.AccountEntityId, v => v.Id, (u, v) => new { u, v })
                        .Join(context.Genders, sa => sa.v.GenderId, cd => cd.Id, (sa,cd) => new {sa,cd})
                        .Where(t => t.sa.v.Id == accountEntityId)
                        .Select(k => new AccountEntityTypeAdditionalDetailsValue
                        {
                            TypeAdditionalDetails = new AccountEntityTypeAdditionalDetail
                            {

                                KeyName = k.sa.u.t.KeyName

                            },
                            Value = k.sa.u.ae.Value,
                            AccountEntity = new AccountEntity
                            {
                                Id = k.sa.v.Id,
                                EntityName = k.sa.v.EntityName,
                                ProfileImageUrl = k.sa.v.ProfileImageUrl,
                                PhysicalAddress = k.sa.v.PhysicalAddress,
                                Phone1 = k.sa.v.Phone1,
                                Email = k.sa.v.Email,
                                DateOfBirth = k.sa.v.DateOfBirth,
                                Gender = new Gender
                                {
                                    GenderName = k.cd.GenderName
                                }
                            }
                        })
                        .ToListAsync();

                    if (additionalDetails.Count == 0 || additionalDetails.All(ad => string.IsNullOrEmpty(ad.TypeAdditionalDetails.KeyName) && string.IsNullOrEmpty(ad.Value)))
                    {                        
                        executionResult.Message = "The candidate has not updated their profile.";
                    }
                    else
                    {
                        executionResult.Result = additionalDetails;
                    }


                    return executionResult;
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here, such as logging the exception details or throwing a custom exception.
                // You can also consider returning a specific error result instead.

                // Example: Log the exception
                Console.WriteLine($"An exception occurred: {ex}");

                // Example: Throw a custom exception
                throw new Exception("An error occurred while checking additional details", ex);
            }
        }

        public async Task<ExecutionResult<List<AccountEntityTypeAdditionalDetailsValue>>> GetUserDetails()
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<List<AccountEntityTypeAdditionalDetailsValue>>(true);

                    var additionalDetails = await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, ae => ae.TypeAdditionalDetailsId, t => t.Id, (ae, t) => new { ae, t })
                        .Join(context.AccountEntities, u => u.ae.AccountEntityId, v => v.Id, (u, v) => new { u, v })
                        .Join(context.Genders, sa => sa.v.GenderId, cd => cd.Id, (sa, cd) => new { sa, cd })
                        .Where(t => t.sa.v.Id == accountEntityId)
                        .Select(k => new AccountEntityTypeAdditionalDetailsValue
                        {
                            TypeAdditionalDetails = new AccountEntityTypeAdditionalDetail
                            {

                                KeyName = k.sa.u.t.KeyName

                            },
                            Value = k.sa.u.ae.Value,
                            AccountEntity = new AccountEntity
                            {
                                Id = k.sa.v.Id,
                                EntityName = k.sa.v.EntityName,
                                ProfileImageUrl = k.sa.v.ProfileImageUrl,
                                PhysicalAddress = k.sa.v.PhysicalAddress,
                                Phone1 = k.sa.v.Phone1,
                                Email = k.sa.v.Email,
                                DateOfBirth = k.sa.v.DateOfBirth,
                                Gender = new Gender
                                {
                                    GenderName = k.cd.GenderName
                                }
                            }
                        })
                        .ToListAsync();

                    if (additionalDetails.Count == 0 || additionalDetails.All(ad => string.IsNullOrEmpty(ad.TypeAdditionalDetails.KeyName) && string.IsNullOrEmpty(ad.Value)))
                    {
                        executionResult.Message = "The candidate has not updated their profile.";
                    }
                    else
                    {
                        executionResult.Result = additionalDetails;
                    }


                    return executionResult;
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here, such as logging the exception details or throwing a custom exception.
                // You can also consider returning a specific error result instead.

                // Example: Log the exception
                Console.WriteLine($"An exception occurred: {ex}");

                // Example: Throw a custom exception
                throw new Exception("An error occurred while checking additional details", ex);
            }
        }

        public async Task<ExecutionResult<List<AccountEntityTypeAdditionalDetailsValue>>> GetCompanyDetails(int id)
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = id;

                    var executionResult = new ExecutionResult<List<AccountEntityTypeAdditionalDetailsValue>>(true);

                    var additionalDetails = await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, ae => ae.TypeAdditionalDetailsId, t => t.Id, (ae, t) => new { ae, t })
                        .Join(context.AccountEntities, u => u.ae.AccountEntityId, v => v.Id, (u, v) => new { u, v })                        
                        .Where(t => t.v.Id == accountEntityId)
                        .Select(k => new AccountEntityTypeAdditionalDetailsValue
                        {
                            TypeAdditionalDetails = new AccountEntityTypeAdditionalDetail
                            {

                                KeyName = k.u.t.KeyName

                            },
                            Value = k.u.ae.Value,
                            AccountEntity = new AccountEntity
                            {
                                Id = k.v.Id,
                                EntityName = k.v.EntityName,
                                ProfileImageUrl = k.v.ProfileImageUrl,
                                PhysicalAddress = k.v.PhysicalAddress,
                                Phone1 = k.v.Phone1,
                                Email = k.v.Email,
                                PostalAddress = k.v.PostalAddress
                            }
                        })
                        .ToListAsync();

                    if (additionalDetails.Count == 0 || additionalDetails.All(ad => string.IsNullOrEmpty(ad.TypeAdditionalDetails.KeyName) && string.IsNullOrEmpty(ad.Value)))
                    {
                        executionResult.Message = "The company has not updated their profile.";
                    }
                    else
                    {
                        executionResult.Result = additionalDetails;
                    }

                    return executionResult;
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here, such as logging the exception details or throwing a custom exception.
                // You can also consider returning a specific error result instead.

                // Example: Log the exception
                Console.WriteLine($"An exception occurred: {ex}");

                // Example: Throw a custom exception
                throw new Exception("An error occurred while checking additional details", ex);
            }
        }

        public async Task<ExecutionResult<List<AccountEntityTypeAdditionalDetailsValue>>> GetLoggedCompanyDetails()
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<List<AccountEntityTypeAdditionalDetailsValue>>(true);

                    var additionalDetails = await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, ae => ae.TypeAdditionalDetailsId, t => t.Id, (ae, t) => new { ae, t })
                        .Join(context.AccountEntities, u => u.ae.AccountEntityId, v => v.Id, (u, v) => new { u, v })
                        .Where(t => t.v.Id == accountEntityId)
                        .Select(k => new AccountEntityTypeAdditionalDetailsValue
                        {
                            TypeAdditionalDetails = new AccountEntityTypeAdditionalDetail
                            {

                                KeyName = k.u.t.KeyName

                            },
                            Value = k.u.ae.Value,
                            AccountEntity = new AccountEntity
                            {
                                Id = k.v.Id,
                                EntityName = k.v.EntityName,
                                ProfileImageUrl = k.v.ProfileImageUrl,
                                PhysicalAddress = k.v.PhysicalAddress,
                                Phone1 = k.v.Phone1,
                                Email = k.v.Email,
                                PostalAddress = k.v.PostalAddress
                            }
                        })
                        .ToListAsync();

                    if (additionalDetails.Count == 0 || additionalDetails.All(ad => string.IsNullOrEmpty(ad.TypeAdditionalDetails.KeyName) && string.IsNullOrEmpty(ad.Value)))
                    {
                        executionResult.Message = "The company has not updated their profile.";
                    }
                    else
                    {
                        executionResult.Result = additionalDetails;
                    }

                    return executionResult;
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here, such as logging the exception details or throwing a custom exception.
                // You can also consider returning a specific error result instead.

                // Example: Log the exception
                Console.WriteLine($"An exception occurred: {ex}");

                // Example: Throw a custom exception
                throw new Exception("An error occurred while checking additional details", ex);
            }
        }

        public async Task<ExecutionResult<MatchCandidate>> MatchCandidate(MatchCandidate model)
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {
                        var accountEntityId = model.CandidateId;

                        var executionResult = new ExecutionResult<MatchCandidate>(true);

                        var getCompany = await context.AccountEntities
                           .Join(context.SubAccountBranches, x => x.SubAccountBranchId, y => y.Id, (x, y) => new { x, y })
                           .Join(context.SubAccounts, e => e.y.SubAccountId, z => z.Id, (e, z) => new { e, z })
                           .Where(t => t.e.x.Id == model.AccountEntity.Id)
                           .FirstOrDefaultAsync();

                        var getCandidate = await context.AccountEntities
                         .Join(context.SubAccountBranches, x => x.SubAccountBranchId, y => y.Id, (x, y) => new { x, y })
                         .Join(context.SubAccounts, e => e.y.SubAccountId, z => z.Id, (e, z) => new { e, z })
                         .Where(t => t.e.x.Id == accountEntityId)
                         .FirstOrDefaultAsync();

                        if (getCandidate.z.ParentId == null)
                        {
                            getCandidate.z.ParentId = getCompany.z.Id;
                            
                            await context.SaveChangesAsync();

                            executionResult.Message = "Candidate Matched Successfully.";
                        }
                        else
                        {
                            executionResult.Message = "Candidate Already Matched";
                        }

                        return executionResult;

                    }

                }
                catch (Exception ex)
                {
                    // Handle the exception here, such as logging the exception details or throwing a custom exception.
                    // You can also consider returning a specific error result instead.

                    // Example: Log the exception
                    Console.WriteLine($"An exception occurred: {ex}");

                    // Example: Throw a custom exception
                    throw new Exception("An error occurred while matching Candidate", ex);
                }
            }

        }

        public async Task<ExecutionResult<ApplyProject>> ApplyProject(ApplyProject model)
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {
                        var accountEntityId = UserPrinciple.User.AccountEntityId;

                        var executionResult = new ExecutionResult<ApplyProject>(true);

                        var openPositions = await context.CompanyProjects
                                        .Where(p => p.OpenPosition > 0 && p.Id == model.CompanyProjectId)
                                        .FirstOrDefaultAsync();

                        if (openPositions != null)
                        {
                            // Check if the user has already applied for the project
                            var existingApplication = await context.ProjectApplications
                            .FirstOrDefaultAsync(a => a.AccountEntityId == accountEntityId && a.ProjectId == model.CompanyProjectId);

                            if (existingApplication != null)
                            {
                                return new ExecutionResult<ApplyProject>
                                {
                                    IsSuccessful = true,
                                    Message = "You have already applied for this project."
                                };
                            }

                            var statusType = await context.ProjectApplicationStatuses.FirstOrDefaultAsync(
                        r => r.ApplicationStatus == model.ApplicationStatus);

                            var apply = new ProjectApplication
                            {
                                AccountEntityId = accountEntityId,
                                ProjectId = model.CompanyProjectId,
                                ProjectApplicationStatusId = statusType.Id,
                                DateApplied = DateTime.UtcNow
                            };

                            context.ProjectApplications.Add(apply);

                            await context.SaveChangesAsync();

                            executionResult.IsSuccessful = true;
                            executionResult.Message = "Application sent Successfully.";
                            executionResult.Result = model;
                        } else
                        {
                            executionResult.IsSuccessful = false;
                            executionResult.Message = "No open positions available for this project";
                            executionResult.Result = model;
                        }

                        return executionResult;

                    }

                }
                catch (Exception ex)
                {
                    // Handle the exception here, such as logging the exception details or throwing a custom exception.
                    // You can also consider returning a specific error result instead.

                    // Example: Log the exception
                    Console.WriteLine($"An exception occurred: {ex}");

                    // Example: Throw a custom exception
                    throw new Exception("An error occurred while Sending Application", ex);
                }
            }

        }

        public async Task<ExecutionResult<List<ProjectApplication>>> GetApplications()
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {
                        var accountEntityId = UserPrinciple.User.AccountEntityId;

                        var executionResult = new ExecutionResult<List<ProjectApplication>>(true);

                        var applications = await context.ProjectApplications
                            .Join(context.AccountEntities, a => a.AccountEntityId, s => s.Id, (a, s) => new { a, s })
                            .Join(context.CompanyProjects, b => b.a.ProjectId, c => c.Id, (b,c) => new { b, c})
                            .Join(context.ProjectApplicationStatuses, ab => ab.b.a.ProjectApplicationStatusId, kc => kc.Id, (ab,kc) => new { ab,kc})
                            .Where(x => x.kc.ApplicationStatus == DefaultConfiguration.ProjectApplicationStatus.notReviewedTypeName)
                            .Select(u => new ProjectApplication
                            {
                                Id = u.ab.b.a.Id,
                                AccountEntity = new AccountEntity
                                {
                                    EntityName = u.ab.b.s.EntityName,
                                    ProfileImageUrl = u.ab.b.s.ProfileImageUrl,
                                    PhysicalAddress = u.ab.b.s.PhysicalAddress
                                },
                                Project = new CompanyProject
                                {
                                    ProjectName = u.ab.c.ProjectName
                                },
                                ProjectApplicationStatus = new ProjectApplicationStatus
                                {
                                    ApplicationStatus = u.kc.ApplicationStatus
                                }
                                


                            })
                            .ToListAsync();

                        executionResult.Result = applications;
                        return executionResult;
                    }

                }
                catch (Exception ex)
                {
                    // Handle the exception here, such as logging the exception details or throwing a custom exception.
                    // You can also consider returning a specific error result instead.

                    // Example: Log the exception
                    Console.WriteLine($"An exception occurred: {ex}");

                    // Example: Throw a custom exception
                    throw new Exception("An error occurred while getting applications", ex);
                }
            }

        }

        public async Task<ExecutionResult<List<AccountEntity>>> GetAllCompanies()
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {
                        var accountEntityId = UserPrinciple.User.AccountEntityId;

                        var executionResult = new ExecutionResult<List<AccountEntity>>(true);

                        var companies = await context.AccountEntities
                            .Join(context.AccountEntityTypes, a => a.AccountEntityTypeId, s => s.Id, (a, s) => new { a, s })
                            .Where(x => x.s.TypeName == DefaultConfiguration.AccountEntityType.companyTypeName)
                            .Select(u => new AccountEntity
                            {
                                Id = u.a.Id,
                                SubAccountBranchId = u.a.SubAccountBranchId,
                                EntityName = u.a.EntityName,
                                Phone1 = u.a.Phone1,
                                Email = u.a.Email

                            })
                            .ToListAsync();

                        executionResult.Result = companies;
                        return executionResult;
                    }

                }
                catch (Exception ex)
                {
                    // Handle the exception here, such as logging the exception details or throwing a custom exception.
                    // You can also consider returning a specific error result instead.

                    // Example: Log the exception
                    Console.WriteLine($"An exception occurred: {ex}");

                    // Example: Throw a custom exception
                    throw new Exception("An error occurred while selecting an Agent", ex);
                }
            }

        }

        public async Task<ExecutionResult<List<ProjectApplication>>> GetApplicationDetails(int id)
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = id;

                    var executionResult = new ExecutionResult<List<ProjectApplication>>(true);

                    var applicationDetails = await context.ProjectApplications
                        .Join(context.AccountEntities, ae => ae.AccountEntityId, t => t.Id, (ae, t) => new { ae, t })
                        .Join(context.CompanyProjects, u => u.ae.ProjectId, v => v.Id, (u, v) => new { u, v })
                        .Join(context.ProjectApplicationStatuses, cd => cd.u.ae.ProjectApplicationStatusId, s => s.Id, (cd, s) => new { cd, s })
                        .Where(t => t.s.ApplicationStatus == DefaultConfiguration.ProjectApplicationStatus.notReviewedTypeName)
                        .Select(k => new ProjectApplication
                        {
                            Id = k.cd.u.ae.Id,
                            AccountEntity = new AccountEntity
                            {
                                Id = k.cd.u.t.Id,
                                Phone1 = k.cd.u.t.Phone1,
                                Email = k.cd.u.t.Email,
                                EntityName = k.cd.u.t.EntityName,
                                ProfileImageUrl = k.cd.u.t.ProfileImageUrl,
                                PhysicalAddress = k.cd.u.t.PhysicalAddress
                            },
                            Project = new CompanyProject
                            {
                                Id = k.cd.v.Id,
                                ProjectRecommendation = k.cd.v.ProjectRecommendation,
                                WorkScope = k.cd.v.WorkScope,
                                ProjectDetails = k.cd.v.ProjectDetails,
                                ProjectCategory = k.cd.v.ProjectCategory,
                                ProjectName = k.cd.v.ProjectName,
                                ProjectStart = k.cd.v.ProjectStart,
                                ProjectEnd = k.cd.v.ProjectEnd,
                                ProjectLocation = k.cd.v.ProjectLocation
                            },
                            ProjectApplicationStatus = new ProjectApplicationStatus
                            {
                                ApplicationStatus = k.s.ApplicationStatus
                            }
                        })
                        .ToListAsync();

                    if (applicationDetails.Count == 0)
                    {
                        executionResult.Message = "The application has no details";
                    }
                    else
                    {
                        executionResult.Result = applicationDetails;
                    }

                    return executionResult;
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here, such as logging the exception details or throwing a custom exception.
                // You can also consider returning a specific error result instead.

                // Example: Log the exception
                Console.WriteLine($"An exception occurred: {ex}");

                // Example: Throw a custom exception
                throw new Exception("An error occurred while checking application details", ex);
            }
        }

        public async Task<ExecutionResult<AcceptApplication>> AcceptApplication(AcceptApplication model)
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<AcceptApplication>(true);

                    // Check if the application has already been accepted
                    var applicationStatus = DefaultConfiguration.ProjectApplicationStatus.acceptedTypeName;
                    var isAlreadyAccepted = await context.ProjectApplications
                                            .AnyAsync(p => p.Id == model.ApplicationId && p.ProjectApplicationStatus.ApplicationStatus == applicationStatus);

                    if (!isAlreadyAccepted)
                    {
                        // Check if there are open positions available
                        var openPositions = await context.CompanyProjects
                                        .Where(p => p.OpenPosition > 0 && p.Id == model.ProjectId)
                                        .FirstOrDefaultAsync();

                        if (openPositions != null)
                        {
                            // Update project application status to Accepted
                            var application = await context.ProjectApplications
                                              .Join(context.ProjectApplicationStatuses, b => b.ProjectApplicationStatusId, a => a.Id, (b, a) => new { b, a })
                                              .Where(c => c.b.Id == model.ApplicationId && c.a.ApplicationStatus == DefaultConfiguration.ProjectApplicationStatus.notReviewedTypeName)
                                              .FirstOrDefaultAsync();
                            if (application != null)
                            {
                                var statusType = await context.ProjectApplicationStatuses.FirstOrDefaultAsync(
                                                 r => r.ApplicationStatus == DefaultConfiguration.ProjectApplicationStatus.acceptedTypeName);

                                application.b.ProjectApplicationStatusId = statusType.Id;

                                var tableMessage = new StringBuilder();
                                tableMessage.AppendLine($"Dear {model.CandidateName},");
                                tableMessage.AppendLine();
                                tableMessage.AppendLine($"We hope this message finds you well. We are thrilled to inform you that your application for {model.ProjectName} Project has been Approved.");
                                tableMessage.AppendLine("We were impressed by your profile and we believe that you are an excellent fit for this Project.");
                                tableMessage.AppendLine();
                                tableMessage.AppendLine("Please reply to this email to confirm your acceptance of this position. " +
                                    "Once we receive your confirmation, we will provide you with further details regarding your responsibilities, project timelines, and other relevant information.");
                                tableMessage.AppendLine();
                                tableMessage.AppendLine("Should you have any questions or need further clarification, please do not hesitate to get in touch.");
                                tableMessage.AppendLine();
                                tableMessage.AppendLine("Looking forward to your positive response.");
                                tableMessage.AppendLine();
                                tableMessage.AppendLine("Best Regards,");
                                tableMessage.AppendLine();
                                tableMessage.AppendLine("Lukurare.");

                                var plainTextMessage = tableMessage.ToString();
                                var htmlMessage = plainTextMessage.Replace("\n", "<br>");

                                var Domain = ContextConnectionService._configuration["AppConstants:Domain"];
                                var FromEmail = ContextConnectionService._configuration["AppConstants:FromEmail"];
                                var request = new MailRequest
                                {
                                    ToEmail = model.CandidateEmail,
                                    Subject = $"Your Application for {model.ProjectName} Project",
                                    Body = htmlMessage,

                                    From = FromEmail
                                };

                                var uri = new Uri(Domain);
                                var client = new RestClient(uri);
                                client.AddDefaultHeader("Content-Type", "application/json");
                                client.AddDefaultHeader("Accept", "application/json"); //ensure the application goes with the application/json header for accept
                                                                                       //client.AddDefaultHeader("Authorization", $"Basic {bearerToken}");
                                var reques = new RestRequest("/Email/Mail/SendMail", Method.POST); //create an object to send this request just compliments the RestClient

                                reques.AddJsonBody(request);

                                var restClientResult = await client.ExecuteAsync<MailRequest>(reques);

                                await context.SaveChangesAsync();

                                var project = await context.CompanyProjects
                                              .Where(ac => ac.Id == model.ProjectId)
                                              .FirstOrDefaultAsync();
                                if (project != null)
                                {
                                    project.OpenPosition--;
                                    await context.SaveChangesAsync();
                                }
                            }

                        }
                        else
                        {
                            executionResult.IsSuccessful = true;
                            executionResult.Message = "No open positions available for this project";
                        }
                    }
                    else
                    {
                        executionResult.IsSuccessful = true;
                        executionResult.Message = "This application has already been accepted";
                    }

                    executionResult.IsSuccessful = true;
                    executionResult.Message = "This application has been Accepted Successfully";
                    executionResult.Result = model;

                    return executionResult;
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here, such as logging the exception details or throwing a custom exception.
                // You can also consider returning a specific error result instead.

                // Example: Log the exception
                Console.WriteLine($"An exception occurred: {ex}");

                // Example: Throw a custom exception
                throw new Exception("An error occurred while checking application details", ex);
            }
        }

        public async Task<ExecutionResult<AcceptApplication>> RejectApplication(AcceptApplication model)
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<AcceptApplication>(true);

                    // Check if the application has already been accepted
                    var applicationStatus = DefaultConfiguration.ProjectApplicationStatus.rejectedTypeName;
                    var isAlreadyRejected = await context.ProjectApplications
                                            .AnyAsync(p => p.Id == model.ApplicationId && p.ProjectApplicationStatus.ApplicationStatus == applicationStatus);

                    if (!isAlreadyRejected)
                    {
                        // Check if there are open positions available
                        var openPositions = await context.CompanyProjects
                                        .Where(p => p.OpenPosition > 0 && p.Id == model.ProjectId)
                                        .FirstOrDefaultAsync();

                        if (openPositions != null)
                        {
                            // Update project application status to Rejected
                            var application = await context.ProjectApplications
                                              .Join(context.ProjectApplicationStatuses, b => b.ProjectApplicationStatusId, a => a.Id, (b, a) => new { b, a })
                                              .Where(c => c.b.Id == model.ApplicationId && c.a.ApplicationStatus == DefaultConfiguration.ProjectApplicationStatus.notReviewedTypeName)
                                              .FirstOrDefaultAsync();
                            if (application != null)
                            {
                                var statusType = await context.ProjectApplicationStatuses.FirstOrDefaultAsync(
                                                 r => r.ApplicationStatus == DefaultConfiguration.ProjectApplicationStatus.rejectedTypeName);

                                application.b.ProjectApplicationStatusId = statusType.Id;

                                var tableMessage = new StringBuilder();
                                tableMessage.AppendLine($"Dear {model.CandidateName},");
                                tableMessage.AppendLine();
                                tableMessage.AppendLine($"We hope this message finds you well. We are writing to inform you that after careful consideration, We have decided not to move forward with your application for the {model.ProjectName} Project at this time.");
                                tableMessage.AppendLine("We received applications from many qualified candidates and it was a difficult decision to make. Please know that this decision does not reflect on your qualifications or your potential to succeed in this field.");
                                tableMessage.AppendLine();
                                tableMessage.AppendLine($"We appreciate the time and effort you put into your application and we encourage you to apply for future openings at our website that match your skills and interests.");
                                tableMessage.AppendLine();
                                tableMessage.AppendLine("Should you have any questions or need further clarification, please do not hesitate to get in touch.");
                                tableMessage.AppendLine();
                                tableMessage.AppendLine("Thank you for considering Lukurare as your place of opportunities. We wish you all the best in your future endeavors.");
                                tableMessage.AppendLine();
                                tableMessage.AppendLine("Best Regards,");
                                tableMessage.AppendLine();
                                tableMessage.AppendLine("Lukurare.");

                                var plainTextMessage = tableMessage.ToString();
                                var htmlMessage = plainTextMessage.Replace("\n", "<br>");

                                var Domain = ContextConnectionService._configuration["AppConstants:Domain"];
                                var FromEmail = ContextConnectionService._configuration["AppConstants:FromEmail"];
                                var request = new MailRequest
                                {
                                    ToEmail = model.CandidateEmail,
                                    Subject = $"Your Application for {model.ProjectName} Project",
                                    Body = htmlMessage,

                                    From = FromEmail
                                };

                                var uri = new Uri(Domain);
                                var client = new RestClient(uri);
                                client.AddDefaultHeader("Content-Type", "application/json");
                                client.AddDefaultHeader("Accept", "application/json"); //ensure the application goes with the application/json header for accept
                                                                                       //client.AddDefaultHeader("Authorization", $"Basic {bearerToken}");
                                var reques = new RestRequest("/Email/Mail/SendMail", Method.POST); //create an object to send this request just compliments the RestClient

                                reques.AddJsonBody(request);

                                var restClientResult = await client.ExecuteAsync<MailRequest>(reques);

                                await context.SaveChangesAsync();

                            }

                        }
                        else
                        {
                            executionResult.IsSuccessful = true;
                            executionResult.Message = "No open positions available for this project";
                        }
                    }
                    else
                    {
                        executionResult.IsSuccessful = true;
                        executionResult.Message = "This application has already been rejected";
                    }

                    executionResult.IsSuccessful = true;
                    executionResult.Message = "This application has been Rejected Successfully";
                    executionResult.Result = model;

                    return executionResult;
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here, such as logging the exception details or throwing a custom exception.
                // You can also consider returning a specific error result instead.

                // Example: Log the exception
                Console.WriteLine($"An exception occurred: {ex}");

                // Example: Throw a custom exception
                throw new Exception("An error occurred while rejected application", ex);
            }
        }
    }
}
