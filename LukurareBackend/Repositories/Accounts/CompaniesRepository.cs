using EFDatabaseModel.Contexts;
using EFDatabaseModel.DbModel;
using LukurareBackend.Repositories.Entities;
using Microsoft.EntityFrameworkCore;
using ProjectBase.Repository.Configuration;
using ProjectBase.Repository.Transaction;
using LukurareBackend.Models.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LukurareBackend.Repositories.Accounts
{
   public class CompaniesRepository : PatnerAccountEntityRepository
    {
        public CompaniesRepository(string currentModule)
          : base(currentModule)
        { }

        public async Task<ExecutionResult<AllCompanies.CompanyOverview>> CompanyOverview(AllCompanies.CompanyOverview model)
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {
                        var accountEntityId = UserPrinciple.User.AccountEntityId;

                        var executionResult = new ExecutionResult<AllCompanies.CompanyOverview>(true);

                        // Mapping between model properties and database entries
                        var propertyMappings = new Dictionary<string, string>
                    {
                        { "MissionStatement", "Mission Statement" },
                        { "Vision", "Vision" },
                        { "CoreValues", "Core Values" },
                        { "HistoryBackground", "History and Background" }
                        
                    };

                        foreach (var property in propertyMappings)
                        {
                            var educationalType = context.AccountEntityTypeAdditionalDetails
                                .FirstOrDefault(t => t.KeyName == property.Value);

                            if (educationalType == null)
                            {
                                throw new Exception($"Company Overview type '{property.Value}' not found in database.");
                            }

                            // Create a new instance of AccountEntityTypeAdditionalDetailsValue
                            var companyOverviewValue = new AccountEntityTypeAdditionalDetailsValue
                            {
                                AccountEntityId = accountEntityId,
                                TypeAdditionalDetailsId = educationalType.Id,
                                Value = model.GetType().GetProperty(property.Key).GetValue(model).ToString()
                            };

                            // Add the newly created educational background value to the context
                            context.AccountEntityTypeAdditionalDetailsValues.Add(companyOverviewValue);
                        }

                        executionResult.Message = "Company Overview added Successfully.";
                        executionResult.Result = model;
                        // Save changes to the database
                        await context.SaveChangesAsync();

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
                    throw new Exception("An error occurred while submiting Company Overview", ex);
                }
            }

        }

        public async Task<ExecutionResult<AllCompanies.CompanyOverview>> UpdateCompanyOverview(AllCompanies.CompanyOverview model)
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {
                        var accountEntityId = UserPrinciple.User.AccountEntityId;

                        var executionResult = new ExecutionResult<AllCompanies.CompanyOverview>(true);

                        // Mapping between model properties and database entries
                        var propertyMappings = new Dictionary<string, string>
                    {
                        { "MissionStatement", "Mission Statement" },
                        { "Vision", "Vision" },
                        { "CoreValues", "Core Values" },
                        { "HistoryBackground", "History and Background" }

                    };

                        foreach (var property in propertyMappings)
                        {
                            var educationalType = context.AccountEntityTypeAdditionalDetails
                                .FirstOrDefault(t => t.KeyName == property.Value);

                            if (educationalType == null)
                            {
                                throw new Exception($"Company Overview type '{property.Value}' not found in database.");
                            }

                            // Find existing educational background value based on property name
                            var existingValue = await context.AccountEntityTypeAdditionalDetailsValues
                                .FirstOrDefaultAsync(v => v.AccountEntityId == accountEntityId && v.TypeAdditionalDetailsId == educationalType.Id);

                            if (existingValue != null)
                            {
                                // Update existing educational background value
                                var propertyValue = model.GetType().GetProperty(property.Key).GetValue(model).ToString();
                                if (existingValue.Value != propertyValue)
                                {
                                    existingValue.Value = propertyValue;
                                    context.Entry(existingValue).State = EntityState.Modified;
                                }
                            }
                            else
                            {
                                throw new Exception($"No existing record found for '{property.Value}'.");
                            }
                        }
                        executionResult.IsSuccessful = true;
                        executionResult.Message = "Company Overview updated Successfully.";
                        executionResult.Result = model;

                        // Save changes to the database
                        await context.SaveChangesAsync();

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
                    throw new Exception("An error occurred while updating Company Overview", ex);
                }
            }

        }

        public async Task<ExecutionResult<AllCompanies.Projects>> UpdateProjects(AllCompanies.Projects model)
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {
                        var accountEntityId = UserPrinciple.User.AccountEntityId;

                        var executionResult = new ExecutionResult<AllCompanies.Projects>(true);

                        // Mapping between model properties and database entries
                        var propertyMappings = new Dictionary<string, string>
                    {
                        { "OngoingProjects", "Ongoing Projects" },
                        { "PastProjects", "Past Projects" },
                        { "ProjectTechnologies", "Technologies or Tools Used" }

                    };

                        foreach (var property in propertyMappings)
                        {
                            var educationalType = context.AccountEntityTypeAdditionalDetails
                                .FirstOrDefault(t => t.KeyName == property.Value);

                            if (educationalType == null)
                            {
                                throw new Exception($"Projects type '{property.Value}' not found in database.");
                            }

                            // Find existing educational background value based on property name
                            var existingValue = await context.AccountEntityTypeAdditionalDetailsValues
                                .FirstOrDefaultAsync(v => v.AccountEntityId == accountEntityId && v.TypeAdditionalDetailsId == educationalType.Id);

                            if (existingValue != null)
                            {
                                // Update existing educational background value
                                var propertyValue = model.GetType().GetProperty(property.Key).GetValue(model).ToString();
                                if (existingValue.Value != propertyValue)
                                {
                                    existingValue.Value = propertyValue;
                                    context.Entry(existingValue).State = EntityState.Modified;
                                }
                            }
                            else
                            {
                                throw new Exception($"No existing record found for '{property.Value}'.");
                            }
                        }
                        executionResult.IsSuccessful = true;
                        executionResult.Message = "Projects updated Successfully.";
                        executionResult.Result = model;

                        // Save changes to the database
                        await context.SaveChangesAsync();

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
                    throw new Exception("An error occurred while updating Projects", ex);
                }
            }

        }

        public async Task<ExecutionResult<AllCompanies.InternshipType>> UpdateInternship(AllCompanies.InternshipType model)
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {
                        var accountEntityId = UserPrinciple.User.AccountEntityId;

                        var executionResult = new ExecutionResult<AllCompanies.InternshipType>(true);

                        // Mapping between model properties and database entries
                        var propertyMappings = new Dictionary<string, string>
                    {
                        { "Internships", "Internship Type" }

                    };

                        foreach (var property in propertyMappings)
                        {
                            var educationalType = context.AccountEntityTypeAdditionalDetails
                                .FirstOrDefault(t => t.KeyName == property.Value);

                            if (educationalType == null)
                            {
                                throw new Exception($"Internship type '{property.Value}' not found in database.");
                            }

                            // Find existing educational background value based on property name
                            var existingValue = await context.AccountEntityTypeAdditionalDetailsValues
                                .FirstOrDefaultAsync(v => v.AccountEntityId == accountEntityId && v.TypeAdditionalDetailsId == educationalType.Id);

                            if (existingValue != null)
                            {
                                // Update existing educational background value
                                var propertyValue = model.GetType().GetProperty(property.Key).GetValue(model).ToString();
                                if (existingValue.Value != propertyValue)
                                {
                                    existingValue.Value = propertyValue;
                                    context.Entry(existingValue).State = EntityState.Modified;
                                }
                            }
                            else
                            {
                                throw new Exception($"No existing record found for '{property.Value}'.");
                            }
                        }
                        executionResult.IsSuccessful = true;
                        executionResult.Message = "Internships updated Successfully.";
                        executionResult.Result = model;

                        // Save changes to the database
                        await context.SaveChangesAsync();

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
                    throw new Exception("An error occurred while updating Internship", ex);
                }
            }

        }

        public async Task<ExecutionResult<AllCompanies.CoreCompetencies>> UpdateCompetencies(AllCompanies.CoreCompetencies model)
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {
                        var accountEntityId = UserPrinciple.User.AccountEntityId;

                        var executionResult = new ExecutionResult<AllCompanies.CoreCompetencies>(true);

                        // Mapping between model properties and database entries
                        var propertyMappings = new Dictionary<string, string>
                    {
                        { "AreaofExpertise", "Area of Expertise" },
                        { "Specializations", "Specializations" }

                    };

                        foreach (var property in propertyMappings)
                        {
                            var educationalType = context.AccountEntityTypeAdditionalDetails
                                .FirstOrDefault(t => t.KeyName == property.Value);

                            if (educationalType == null)
                            {
                                throw new Exception($"Competencies type '{property.Value}' not found in database.");
                            }

                            // Find existing educational background value based on property name
                            var existingValue = await context.AccountEntityTypeAdditionalDetailsValues
                                .FirstOrDefaultAsync(v => v.AccountEntityId == accountEntityId && v.TypeAdditionalDetailsId == educationalType.Id);

                            if (existingValue != null)
                            {
                                // Update existing educational background value
                                var propertyValue = model.GetType().GetProperty(property.Key).GetValue(model).ToString();
                                if (existingValue.Value != propertyValue)
                                {
                                    existingValue.Value = propertyValue;
                                    context.Entry(existingValue).State = EntityState.Modified;
                                }
                            }
                            else
                            {
                                throw new Exception($"No existing record found for '{property.Value}'.");
                            }
                        }
                        executionResult.IsSuccessful = true;
                        executionResult.Message = "Competencies updated Successfully.";
                        executionResult.Result = model;

                        // Save changes to the database
                        await context.SaveChangesAsync();

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
                    throw new Exception("An error occurred while updating Competencies", ex);
                }
            }

        }

        public async Task<ExecutionResult<AllCompanies.CompanyCulture>> UpdateCompanyCulture(AllCompanies.CompanyCulture model)
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {
                        var accountEntityId = UserPrinciple.User.AccountEntityId;

                        var executionResult = new ExecutionResult<AllCompanies.CompanyCulture>(true);

                        // Mapping between model properties and database entries
                        var propertyMappings = new Dictionary<string, string>
                    {
                        { "WorkEnvironment", "Work Environment" },
                        { "CompanyValues", "Company Values and Culture" }

                    };

                        foreach (var property in propertyMappings)
                        {
                            var educationalType = context.AccountEntityTypeAdditionalDetails
                                .FirstOrDefault(t => t.KeyName == property.Value);

                            if (educationalType == null)
                            {
                                throw new Exception($"Company Culture type '{property.Value}' not found in database.");
                            }

                            // Find existing educational background value based on property name
                            var existingValue = await context.AccountEntityTypeAdditionalDetailsValues
                                .FirstOrDefaultAsync(v => v.AccountEntityId == accountEntityId && v.TypeAdditionalDetailsId == educationalType.Id);

                            if (existingValue != null)
                            {
                                // Update existing educational background value
                                var propertyValue = model.GetType().GetProperty(property.Key).GetValue(model).ToString();
                                if (existingValue.Value != propertyValue)
                                {
                                    existingValue.Value = propertyValue;
                                    context.Entry(existingValue).State = EntityState.Modified;
                                }
                            }
                            else
                            {
                                throw new Exception($"No existing record found for '{property.Value}'.");
                            }
                        }
                        executionResult.IsSuccessful = true;
                        executionResult.Message = "Company Culture updated Successfully.";
                        executionResult.Result = model;

                        // Save changes to the database
                        await context.SaveChangesAsync();

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
                    throw new Exception("An error occurred while updating Company Culture", ex);
                }
            }

        }

        public async Task<ExecutionResult<AllCompanies.TechnologicalCapabilities>> UpdateTechnology(AllCompanies.TechnologicalCapabilities model)
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {
                        var accountEntityId = UserPrinciple.User.AccountEntityId;

                        var executionResult = new ExecutionResult<AllCompanies.TechnologicalCapabilities>(true);

                        // Mapping between model properties and database entries
                        var propertyMappings = new Dictionary<string, string>
                    {
                        { "TechnologiesUsed", "Technologies Used" },
                        { "InnovationInitiatives", "Innovation Initiatives" }

                    };

                        foreach (var property in propertyMappings)
                        {
                            var educationalType = context.AccountEntityTypeAdditionalDetails
                                .FirstOrDefault(t => t.KeyName == property.Value);

                            if (educationalType == null)
                            {
                                throw new Exception($"Technology type '{property.Value}' not found in database.");
                            }

                            // Find existing educational background value based on property name
                            var existingValue = await context.AccountEntityTypeAdditionalDetailsValues
                                .FirstOrDefaultAsync(v => v.AccountEntityId == accountEntityId && v.TypeAdditionalDetailsId == educationalType.Id);

                            if (existingValue != null)
                            {
                                // Update existing educational background value
                                var propertyValue = model.GetType().GetProperty(property.Key).GetValue(model).ToString();
                                if (existingValue.Value != propertyValue)
                                {
                                    existingValue.Value = propertyValue;
                                    context.Entry(existingValue).State = EntityState.Modified;
                                }
                            }
                            else
                            {
                                throw new Exception($"No existing record found for '{property.Value}'.");
                            }
                        }
                        executionResult.IsSuccessful = true;
                        executionResult.Message = "Technology Capabilities updated Successfully.";
                        executionResult.Result = model;

                        // Save changes to the database
                        await context.SaveChangesAsync();

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
                    throw new Exception("An error occurred while updating Technology Capabilities", ex);
                }
            }

        }

        public async Task<ExecutionResult<AllCompanies.SocialResponsibility>> UpdateSocialResponsibility(AllCompanies.SocialResponsibility model)
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {
                        var accountEntityId = UserPrinciple.User.AccountEntityId;

                        var executionResult = new ExecutionResult<AllCompanies.SocialResponsibility>(true);

                        // Mapping between model properties and database entries
                        var propertyMappings = new Dictionary<string, string>
                    {
                        { "Programs", "CSR Initiatives or Programs" },
                        { "EnvironmentalEfforts", "Environmental Sustainability Efforts" }

                    };

                        foreach (var property in propertyMappings)
                        {
                            var educationalType = context.AccountEntityTypeAdditionalDetails
                                .FirstOrDefault(t => t.KeyName == property.Value);

                            if (educationalType == null)
                            {
                                throw new Exception($"Social Responsibility type '{property.Value}' not found in database.");
                            }

                            // Find existing educational background value based on property name
                            var existingValue = await context.AccountEntityTypeAdditionalDetailsValues
                                .FirstOrDefaultAsync(v => v.AccountEntityId == accountEntityId && v.TypeAdditionalDetailsId == educationalType.Id);

                            if (existingValue != null)
                            {
                                // Update existing educational background value
                                var propertyValue = model.GetType().GetProperty(property.Key).GetValue(model).ToString();
                                if (existingValue.Value != propertyValue)
                                {
                                    existingValue.Value = propertyValue;
                                    context.Entry(existingValue).State = EntityState.Modified;
                                }
                            }
                            else
                            {
                                throw new Exception($"No existing record found for '{property.Value}'.");
                            }
                        }
                        executionResult.IsSuccessful = true;
                        executionResult.Message = "Social Responsibility updated Successfully.";
                        executionResult.Result = model;

                        // Save changes to the database
                        await context.SaveChangesAsync();

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
                    throw new Exception("An error occurred while updating Social Responsibility", ex);
                }
            }

        }

        public async Task<ExecutionResult<AllCompanies.Awards>> UpdateAwards(AllCompanies.Awards model)
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {
                        var accountEntityId = UserPrinciple.User.AccountEntityId;

                        var executionResult = new ExecutionResult<AllCompanies.Awards>(true);

                        // Mapping between model properties and database entries
                        var propertyMappings = new Dictionary<string, string>
                    {
                        { "IndustryAwards", "Industry Awards" },
                        { "Certifications", "Certifications" }

                    };

                        foreach (var property in propertyMappings)
                        {
                            var educationalType = context.AccountEntityTypeAdditionalDetails
                                .FirstOrDefault(t => t.KeyName == property.Value);

                            if (educationalType == null)
                            {
                                throw new Exception($"Awards type '{property.Value}' not found in database.");
                            }

                            // Find existing educational background value based on property name
                            var existingValue = await context.AccountEntityTypeAdditionalDetailsValues
                                .FirstOrDefaultAsync(v => v.AccountEntityId == accountEntityId && v.TypeAdditionalDetailsId == educationalType.Id);

                            if (existingValue != null)
                            {
                                // Update existing educational background value
                                var propertyValue = model.GetType().GetProperty(property.Key).GetValue(model).ToString();
                                if (existingValue.Value != propertyValue)
                                {
                                    existingValue.Value = propertyValue;
                                    context.Entry(existingValue).State = EntityState.Modified;
                                }
                            }
                            else
                            {
                                throw new Exception($"No existing record found for '{property.Value}'.");
                            }
                        }
                        executionResult.IsSuccessful = true;
                        executionResult.Message = "Awards updated Successfully.";
                        executionResult.Result = model;

                        // Save changes to the database
                        await context.SaveChangesAsync();

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
                    throw new Exception("An error occurred while updating Awards", ex);
                }
            }

        }

        public async Task<ExecutionResult<AllCompanies.CareerOpportunities>> UpdateCareer(AllCompanies.CareerOpportunities model)
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {
                        var accountEntityId = UserPrinciple.User.AccountEntityId;

                        var executionResult = new ExecutionResult<AllCompanies.CareerOpportunities>(true);

                        // Mapping between model properties and database entries
                        var propertyMappings = new Dictionary<string, string>
                    {
                        { "InternshipPrograms", "Internship Programs" },
                        { "JobOpenings", "Job Openings" },
                        { "CareerGrowth", "Opportunities for Career Growth and Development" }

                    };

                        foreach (var property in propertyMappings)
                        {
                            var educationalType = context.AccountEntityTypeAdditionalDetails
                                .FirstOrDefault(t => t.KeyName == property.Value);

                            if (educationalType == null)
                            {
                                throw new Exception($"Career type '{property.Value}' not found in database.");
                            }

                            // Find existing educational background value based on property name
                            var existingValue = await context.AccountEntityTypeAdditionalDetailsValues
                                .FirstOrDefaultAsync(v => v.AccountEntityId == accountEntityId && v.TypeAdditionalDetailsId == educationalType.Id);

                            if (existingValue != null)
                            {
                                // Update existing educational background value
                                var propertyValue = model.GetType().GetProperty(property.Key).GetValue(model).ToString();
                                if (existingValue.Value != propertyValue)
                                {
                                    existingValue.Value = propertyValue;
                                    context.Entry(existingValue).State = EntityState.Modified;
                                }
                            }
                            else
                            {
                                throw new Exception($"No existing record found for '{property.Value}'.");
                            }
                        }
                        executionResult.IsSuccessful = true;
                        executionResult.Message = "Career Opportunities updated Successfully.";
                        executionResult.Result = model;

                        // Save changes to the database
                        await context.SaveChangesAsync();

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
                    throw new Exception("An error occurred while updating Career", ex);
                }
            }

        }

        public async Task<ExecutionResult<AllCompanies.InternProfile>> UpdatePreferredIntern(AllCompanies.InternProfile model)
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {
                        var accountEntityId = UserPrinciple.User.AccountEntityId;

                        var executionResult = new ExecutionResult<AllCompanies.InternProfile>(true);

                        // Mapping between model properties and database entries
                        var propertyMappings = new Dictionary<string, string>
                    {
                        { "DesiredSkills", "Desired Skills and Qualifications in Interns" },
                        { "InternTraits", "Characteristics or Traits sought in Interns" }

                    };

                        foreach (var property in propertyMappings)
                        {
                            var educationalType = context.AccountEntityTypeAdditionalDetails
                                .FirstOrDefault(t => t.KeyName == property.Value);

                            if (educationalType == null)
                            {
                                throw new Exception($"Intern Profile type '{property.Value}' not found in database.");
                            }

                            // Find existing educational background value based on property name
                            var existingValue = await context.AccountEntityTypeAdditionalDetailsValues
                                .FirstOrDefaultAsync(v => v.AccountEntityId == accountEntityId && v.TypeAdditionalDetailsId == educationalType.Id);

                            if (existingValue != null)
                            {
                                // Update existing educational background value
                                var propertyValue = model.GetType().GetProperty(property.Key).GetValue(model).ToString();
                                if (existingValue.Value != propertyValue)
                                {
                                    existingValue.Value = propertyValue;
                                    context.Entry(existingValue).State = EntityState.Modified;
                                }
                            }
                            else
                            {
                                throw new Exception($"No existing record found for '{property.Value}'.");
                            }
                        }
                        executionResult.IsSuccessful = true;
                        executionResult.Message = "Intern Profile updated Successfully.";
                        executionResult.Result = model;

                        // Save changes to the database
                        await context.SaveChangesAsync();

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
                    throw new Exception("An error occurred while updating Intern Profile", ex);
                }
            }

        }

        public async Task<ExecutionResult<AllCompanies.Projects>> Projects(AllCompanies.Projects model)
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {
                        var accountEntityId = UserPrinciple.User.AccountEntityId;

                        var executionResult = new ExecutionResult<AllCompanies.Projects>(true);

                        // Mapping between model properties and database entries
                        var propertyMappings = new Dictionary<string, string>
                    {
                        { "OngoingProjects", "Ongoing Projects" },
                        { "PastProjects", "Past Projects" },
                        { "ProjectTechnologies", "Technologies or Tools Used" }                      

                    };

                        foreach (var property in propertyMappings)
                        {
                            var educationalType = context.AccountEntityTypeAdditionalDetails
                                .FirstOrDefault(t => t.KeyName == property.Value);

                            if (educationalType == null)
                            {
                                throw new Exception($"Project type '{property.Value}' not found in database.");
                            }

                            // Create a new instance of AccountEntityTypeAdditionalDetailsValue
                            var projectValue = new AccountEntityTypeAdditionalDetailsValue
                            {
                                AccountEntityId = accountEntityId,
                                TypeAdditionalDetailsId = educationalType.Id,
                                Value = model.GetType().GetProperty(property.Key).GetValue(model).ToString()
                            };

                            // Add the newly created educational background value to the context
                            context.AccountEntityTypeAdditionalDetailsValues.Add(projectValue);
                        }

                        executionResult.Message = "Projects added Successfully.";
                        executionResult.Result = model;
                        // Save changes to the database
                        await context.SaveChangesAsync();

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
                    throw new Exception("An error occurred while submiting Projects", ex);
                }
            }

        }

        public async Task<ExecutionResult<AllCompanies.InternshipType>> InternshipType(AllCompanies.InternshipType model)
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {
                        var accountEntityId = UserPrinciple.User.AccountEntityId;

                        var executionResult = new ExecutionResult<AllCompanies.InternshipType>(true);

                        // Mapping between model properties and database entries
                        var propertyMappings = new Dictionary<string, string>
                    {
                        { "Internships", "Internship Type" }
                       
                    };

                        foreach (var property in propertyMappings)
                        {
                            var educationalType = context.AccountEntityTypeAdditionalDetails
                                .FirstOrDefault(t => t.KeyName == property.Value);

                            if (educationalType == null)
                            {
                                throw new Exception($"Internship type '{property.Value}' not found in database.");
                            }

                            // Create a new instance of AccountEntityTypeAdditionalDetailsValue
                            var internshipValue = new AccountEntityTypeAdditionalDetailsValue
                            {
                                AccountEntityId = accountEntityId,
                                TypeAdditionalDetailsId = educationalType.Id,
                                Value = model.GetType().GetProperty(property.Key).GetValue(model).ToString()
                            };

                            // Add the newly created educational background value to the context
                            context.AccountEntityTypeAdditionalDetailsValues.Add(internshipValue);
                        }

                        executionResult.IsSuccessful = true;
                        executionResult.Message = "InternshipType added Successfully.";
                        executionResult.Result = model;
                        // Save changes to the database
                        await context.SaveChangesAsync();

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
                    throw new Exception("An error occurred while submiting Internship Type", ex);
                }
            }

        }

        public async Task<ExecutionResult<AllCompanies.CoreCompetencies>> Competencies(AllCompanies.CoreCompetencies model)
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {
                        var accountEntityId = UserPrinciple.User.AccountEntityId;

                        var executionResult = new ExecutionResult<AllCompanies.CoreCompetencies>(true);

                        // Mapping between model properties and database entries
                        var propertyMappings = new Dictionary<string, string>
                    {
                        { "AreaofExpertise", "Area of Expertise" },
                        { "Specializations", "Specializations" }
                    };

                        foreach (var property in propertyMappings)
                        {
                            var educationalType = context.AccountEntityTypeAdditionalDetails
                                .FirstOrDefault(t => t.KeyName == property.Value);

                            if (educationalType == null)
                            {
                                throw new Exception($"Core Competencies type '{property.Value}' not found in database.");
                            }

                            // Create a new instance of AccountEntityTypeAdditionalDetailsValue
                            var competenciesValue = new AccountEntityTypeAdditionalDetailsValue
                            {
                                AccountEntityId = accountEntityId,
                                TypeAdditionalDetailsId = educationalType.Id,
                                Value = model.GetType().GetProperty(property.Key).GetValue(model).ToString()
                            };

                            // Add the newly created educational background value to the context
                            context.AccountEntityTypeAdditionalDetailsValues.Add(competenciesValue);
                        }

                        executionResult.Message = "Core Competencies added Successfully.";
                        executionResult.Result = model;
                        // Save changes to the database
                        await context.SaveChangesAsync();

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
                    throw new Exception("An error occurred while submiting Internship Type", ex);
                }
            }

        }

        public async Task<ExecutionResult<AllCompanies.CompanyCulture>> CompanyCulture(AllCompanies.CompanyCulture model)
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {
                        var accountEntityId = UserPrinciple.User.AccountEntityId;

                        var executionResult = new ExecutionResult<AllCompanies.CompanyCulture>(true);

                        // Mapping between model properties and database entries
                        var propertyMappings = new Dictionary<string, string>
                    {
                        { "WorkEnvironment", "Work Environment" },
                        { "CompanyValues", "Company Values and Culture" }
                    };

                        foreach (var property in propertyMappings)
                        {
                            var educationalType = context.AccountEntityTypeAdditionalDetails
                                .FirstOrDefault(t => t.KeyName == property.Value);

                            if (educationalType == null)
                            {
                                throw new Exception($"Company Culture type '{property.Value}' not found in database.");
                            }

                            // Create a new instance of AccountEntityTypeAdditionalDetailsValue
                            var competenciesValue = new AccountEntityTypeAdditionalDetailsValue
                            {
                                AccountEntityId = accountEntityId,
                                TypeAdditionalDetailsId = educationalType.Id,
                                Value = model.GetType().GetProperty(property.Key).GetValue(model).ToString()
                            };

                            // Add the newly created educational background value to the context
                            context.AccountEntityTypeAdditionalDetailsValues.Add(competenciesValue);
                        }

                        executionResult.Message = "Company Culture added Successfully.";
                        executionResult.Result = model;
                        // Save changes to the database
                        await context.SaveChangesAsync();

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
                    throw new Exception("An error occurred while submiting Company Culture", ex);
                }
            }

        }

        public async Task<ExecutionResult<AllCompanies.TechnologicalCapabilities>> Technology(AllCompanies.TechnologicalCapabilities model)
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {
                        var accountEntityId = UserPrinciple.User.AccountEntityId;

                        var executionResult = new ExecutionResult<AllCompanies.TechnologicalCapabilities>(true);

                        // Mapping between model properties and database entries
                        var propertyMappings = new Dictionary<string, string>
                    {
                        { "TechnologiesUsed", "Technologies Used" },
                        { "InnovationInitiatives", "Innovation Initiatives" }
                    };

                        foreach (var property in propertyMappings)
                        {
                            var educationalType = context.AccountEntityTypeAdditionalDetails
                                .FirstOrDefault(t => t.KeyName == property.Value);

                            if (educationalType == null)
                            {
                                throw new Exception($"Technology type '{property.Value}' not found in database.");
                            }

                            // Create a new instance of AccountEntityTypeAdditionalDetailsValue
                            var competenciesValue = new AccountEntityTypeAdditionalDetailsValue
                            {
                                AccountEntityId = accountEntityId,
                                TypeAdditionalDetailsId = educationalType.Id,
                                Value = model.GetType().GetProperty(property.Key).GetValue(model).ToString()
                            };

                            // Add the newly created educational background value to the context
                            context.AccountEntityTypeAdditionalDetailsValues.Add(competenciesValue);
                        }

                        executionResult.Message = "Technology Capabilities added Successfully.";
                        executionResult.Result = model;
                        // Save changes to the database
                        await context.SaveChangesAsync();

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
                    throw new Exception("An error occurred while submiting Technology Capabilities", ex);
                }
            }

        }

        public async Task<ExecutionResult<AllCompanies.SocialResponsibility>> SocialResponsibility(AllCompanies.SocialResponsibility model)
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {
                        var accountEntityId = UserPrinciple.User.AccountEntityId;

                        var executionResult = new ExecutionResult<AllCompanies.SocialResponsibility>(true);

                        // Mapping between model properties and database entries
                        var propertyMappings = new Dictionary<string, string>
                    {
                        { "Programs", "CSR Initiatives or Programs" },
                        { "EnvironmentalEfforts", "Environmental Sustainability Efforts" }
                    };

                        foreach (var property in propertyMappings)
                        {
                            var educationalType = context.AccountEntityTypeAdditionalDetails
                                .FirstOrDefault(t => t.KeyName == property.Value);

                            if (educationalType == null)
                            {
                                throw new Exception($"Social Responsibility type '{property.Value}' not found in database.");
                            }

                            // Create a new instance of AccountEntityTypeAdditionalDetailsValue
                            var competenciesValue = new AccountEntityTypeAdditionalDetailsValue
                            {
                                AccountEntityId = accountEntityId,
                                TypeAdditionalDetailsId = educationalType.Id,
                                Value = model.GetType().GetProperty(property.Key).GetValue(model).ToString()
                            };

                            // Add the newly created educational background value to the context
                            context.AccountEntityTypeAdditionalDetailsValues.Add(competenciesValue);
                        }

                        executionResult.Message = "Social Responsibility added Successfully.";
                        executionResult.Result = model;
                        // Save changes to the database
                        await context.SaveChangesAsync();

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
                    throw new Exception("An error occurred while submiting Technology Capabilities", ex);
                }
            }

        }

        public async Task<ExecutionResult<AllCompanies.Awards>> Awards(AllCompanies.Awards model)
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {
                        var accountEntityId = UserPrinciple.User.AccountEntityId;

                        var executionResult = new ExecutionResult<AllCompanies.Awards>(true);

                        // Mapping between model properties and database entries
                        var propertyMappings = new Dictionary<string, string>
                    {
                        { "IndustryAwards", "Industry Awards" },
                        { "Certifications", "Certifications" }
                    };

                        foreach (var property in propertyMappings)
                        {
                            var educationalType = context.AccountEntityTypeAdditionalDetails
                                .FirstOrDefault(t => t.KeyName == property.Value);

                            if (educationalType == null)
                            {
                                throw new Exception($"Awards type '{property.Value}' not found in database.");
                            }

                            // Create a new instance of AccountEntityTypeAdditionalDetailsValue
                            var competenciesValue = new AccountEntityTypeAdditionalDetailsValue
                            {
                                AccountEntityId = accountEntityId,
                                TypeAdditionalDetailsId = educationalType.Id,
                                Value = model.GetType().GetProperty(property.Key).GetValue(model).ToString()
                            };

                            // Add the newly created educational background value to the context
                            context.AccountEntityTypeAdditionalDetailsValues.Add(competenciesValue);
                        }

                        executionResult.Message = "Awards and Recognition added Successfully.";
                        executionResult.Result = model;
                        // Save changes to the database
                        await context.SaveChangesAsync();

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
                    throw new Exception("An error occurred while submiting Awards and Recognition", ex);
                }
            }

        }

        public async Task<ExecutionResult<AllCompanies.CareerOpportunities>> Career(AllCompanies.CareerOpportunities model)
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {
                        var accountEntityId = UserPrinciple.User.AccountEntityId;

                        var executionResult = new ExecutionResult<AllCompanies.CareerOpportunities>(true);

                        // Mapping between model properties and database entries
                        var propertyMappings = new Dictionary<string, string>
                    {
                        { "InternshipPrograms", "Internship Programs" },
                        { "JobOpenings", "Job Openings" },
                        { "CareerGrowth", "Opportunities for Career Growth and Development" }
                    };

                        foreach (var property in propertyMappings)
                        {
                            var educationalType = context.AccountEntityTypeAdditionalDetails
                                .FirstOrDefault(t => t.KeyName == property.Value);

                            if (educationalType == null)
                            {
                                throw new Exception($"Career type '{property.Value}' not found in database.");
                            }

                            // Create a new instance of AccountEntityTypeAdditionalDetailsValue
                            var competenciesValue = new AccountEntityTypeAdditionalDetailsValue
                            {
                                AccountEntityId = accountEntityId,
                                TypeAdditionalDetailsId = educationalType.Id,
                                Value = model.GetType().GetProperty(property.Key).GetValue(model).ToString()
                            };

                            // Add the newly created educational background value to the context
                            context.AccountEntityTypeAdditionalDetailsValues.Add(competenciesValue);
                        }

                        executionResult.Message = "Career Opportunities added Successfully.";
                        executionResult.Result = model;
                        // Save changes to the database
                        await context.SaveChangesAsync();

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
                    throw new Exception("An error occurred while submiting Career Opportunities", ex);
                }
            }

        }

        public async Task<ExecutionResult<AllCompanies.InternProfile>> PreferredIntern(AllCompanies.InternProfile model)
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {
                        var accountEntityId = UserPrinciple.User.AccountEntityId;

                        var executionResult = new ExecutionResult<AllCompanies.InternProfile>(true);

                        // Mapping between model properties and database entries
                        var propertyMappings = new Dictionary<string, string>
                    {
                        { "DesiredSkills", "Desired Skills and Qualifications in Interns" },
                        { "InternTraits", "Characteristics or Traits sought in Interns" }                        
                    };

                        foreach (var property in propertyMappings)
                        {
                            var educationalType = context.AccountEntityTypeAdditionalDetails
                                .FirstOrDefault(t => t.KeyName == property.Value);

                            if (educationalType == null)
                            {
                                throw new Exception($"Intern type '{property.Value}' not found in database.");
                            }

                            // Create a new instance of AccountEntityTypeAdditionalDetailsValue
                            var competenciesValue = new AccountEntityTypeAdditionalDetailsValue
                            {
                                AccountEntityId = accountEntityId,
                                TypeAdditionalDetailsId = educationalType.Id,
                                Value = model.GetType().GetProperty(property.Key).GetValue(model).ToString()
                            };

                            // Add the newly created educational background value to the context
                            context.AccountEntityTypeAdditionalDetailsValues.Add(competenciesValue);
                        }

                        executionResult.Message = "Preferred Intern added Successfully.";
                        executionResult.Result = model;
                        // Save changes to the database
                        await context.SaveChangesAsync();

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
                    throw new Exception("An error occurred while submiting Preferred Intern", ex);
                }
            }

        }

        public async Task<ExecutionResult<List<CompanyProject>>> SearchProjects(string industry, string location)
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {                      

                        var executionResult = new ExecutionResult<List<CompanyProject>>(true);

                        // Set industry and location to null if they are equal to "Industry..." and "Location..." respectively
                        if (industry == "Industry...")
                        {
                            industry = null;
                        }
                        if (location == "Location...")
                        {
                            location = null;
                        }

                        // Query projects based on industry and location
                        var projectsQuery = context.CompanyProjects
                            .Join(context.AccountEntities, a => a.AccountEntityId, b => b.Id, (a, b) => new { a, b })
                            .Select(k => new CompanyProject
                            {
                                Id = k.a.Id,
                                ProjectName = k.a.ProjectName,
                                DatePosted = k.a.DatePosted,
                                ProjectLocation = k.a.ProjectLocation,
                                ProjectCategory = k.a.ProjectCategory,
                                AccountEntity = new AccountEntity
                                {
                                    EntityName = k.b.EntityName,
                                    ProfileImageUrl = k.b.ProfileImageUrl,
                                    PhysicalAddress = k.b.PhysicalAddress
                                }
                            });
                            

                        if (!string.IsNullOrEmpty(industry))
                        {
                            projectsQuery = projectsQuery.Where(p => p.ProjectCategory == industry);
                        }

                        if (!string.IsNullOrEmpty(location))
                        {
                            projectsQuery = projectsQuery.Where(p => p.ProjectLocation == location);
                        }

                        // Execute the query
                        var projects = await projectsQuery.ToListAsync();

                        if (projects.Count == 0)
                        {
                            executionResult.IsSuccessful = true;
                            executionResult.Message = "No Project Found";
                        }
                                               
                        executionResult.Result = projects;


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
                    throw new Exception("An error occurred while Searching Project", ex);
                }
            }

        }
        public async Task<ExecutionResult<CompanyProject>> CompanyProject(CompanyProject model)
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {
                        long accountEntityId = 0; 

                        if (model.AccountEntityId == 0)
                        {
                            accountEntityId = UserPrinciple.User.AccountEntityId;
                        }
                        if (model.AccountEntityId != 0)
                        {
                             accountEntityId = model.AccountEntityId;
                        }

                        var executionResult = new ExecutionResult<CompanyProject>(true);

                        var newProject = new CompanyProject
                        {
                            AccountEntityId = accountEntityId,
                            ProjectName = model.ProjectName,
                            ProjectCategory = model.ProjectCategory,
                            ProjectDetails = model.ProjectDetails,
                            ProjectEnd = model.ProjectEnd,
                            ProjectStart = model.ProjectStart,
                            OpenPosition = model.OpenPosition,
                            WorkScope = model.WorkScope,
                            ProjectRecommendation = model.ProjectRecommendation,
                            PostAs = model.PostAs,
                            ProjectLocation = model.ProjectLocation,
                            DatePosted = DateTime.UtcNow,

                        };

                        context.CompanyProjects.Add(newProject);

                        await context.SaveChangesAsync();
                       

                        executionResult.Message = "Project added Successfully.";
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
                    throw new Exception("An error occurred while adding Project", ex);
                }
            }

        }

        public async Task<ExecutionResult<CompanyProject>> GetProjectDetails(int id)
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var projectId = id;

                    var executionResult = new ExecutionResult<CompanyProject>(true);

                    var projectDetails = await context.CompanyProjects                        
                        .Join(context.AccountEntities, u => u.AccountEntityId, v => v.Id, (u, v) => new { u, v })                        
                        .Where(t => t.u.Id == projectId)                            
                        .Select(k => new CompanyProject
                        {
                            Id = k.u.Id,
                            ProjectName = k.u.ProjectName,
                            ProjectStart = k.u.ProjectStart,
                            ProjectEnd = k.u.ProjectEnd,
                            ProjectCategory = k.u.ProjectCategory,
                            ProjectDetails = k.u.ProjectDetails,
                            ProjectRecommendation = k.u.ProjectRecommendation,
                            OpenPosition = k.u.OpenPosition,
                            PostAs = k.u.PostAs,
                            WorkScope = k.u.WorkScope,
                            DatePosted = k.u.DatePosted,
                            ProjectLocation = k.u.ProjectLocation,
                            AccountEntity = new AccountEntity
                            {
                                EntityName = k.v.EntityName,
                                Phone1 = k.v.Phone1,
                                PhysicalAddress = k.v.PhysicalAddress,
                                Email = k.v.Email,
                                ProfileImageUrl = k.v.ProfileImageUrl
                            }
                        })
                        .FirstOrDefaultAsync();

                    if (projectDetails == null)
                    {
                        executionResult.Message = "The Project has no details.";
                    }
                    else
                    {
                        executionResult.IsSuccessful = true;
                        executionResult.Result = projectDetails;
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
                throw new Exception("An error occurred while checking Projects details", ex);
            }
        }

        public async Task<ExecutionResult<bool>> CheckCompany()
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<bool>(true);
                    // Check if any documents match the provided form data
                    var exists = await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Mission Statement" && !string.IsNullOrEmpty(c.a.Value))
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Vision" && !string.IsNullOrEmpty(c.a.Value))
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Core Values" && !string.IsNullOrEmpty(c.a.Value))
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "History and Background" && !string.IsNullOrEmpty(c.a.Value));
                        

                    executionResult.Result = exists;

                    return executionResult;
                }
            }
            catch (Exception ex)
            {
                // Log or handle any exceptions
                throw;
            }
        }

        public async Task<ExecutionResult<bool>> CheckProjects()
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<bool>(true);
                    // Check if any documents match the provided form data
                    var exists = await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Ongoing Projects" && !string.IsNullOrEmpty(c.a.Value))
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Past Projects" && !string.IsNullOrEmpty(c.a.Value))
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Technologies or Tools Used" && !string.IsNullOrEmpty(c.a.Value));
                            
                    executionResult.Result = exists;

                    return executionResult;
                }
            }
            catch (Exception ex)
            {
                // Log or handle any exceptions
                throw;
            }
        }
        public async Task<ExecutionResult<bool>> CheckInternships()
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<bool>(true);
                    // Check if any documents match the provided form data
                    var exists = await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Internship Type" && !string.IsNullOrEmpty(c.a.Value));
                       
                    executionResult.Result = exists;

                    return executionResult;
                }
            }
            catch (Exception ex)
            {
                // Log or handle any exceptions
                throw;
            }
        }

        public async Task<ExecutionResult<bool>> CheckCompetence()
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<bool>(true);
                    // Check if any documents match the provided form data
                    var exists = await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Area of Expertise" && !string.IsNullOrEmpty(c.a.Value))
                        
                        ;

                    executionResult.Result = exists;

                    return executionResult;
                }
            }
            catch (Exception ex)
            {
                // Log or handle any exceptions
                throw;
            }
        }

        public async Task<ExecutionResult<bool>> CheckCulture()
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<bool>(true);
                    // Check if any documents match the provided form data
                    var exists = await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Work Environment" && !string.IsNullOrEmpty(c.a.Value))
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Company Values and Culture" && !string.IsNullOrEmpty(c.a.Value));

                    executionResult.Result = exists;

                    return executionResult;
                }
            }
            catch (Exception ex)
            {
                // Log or handle any exceptions
                throw;
            }
        }

        public async Task<ExecutionResult<bool>> CheckTechnology()
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<bool>(true);
                    // Check if any documents match the provided form data
                    var exists = await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Technologies Used" && !string.IsNullOrEmpty(c.a.Value))
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Innovation Initiatives" && !string.IsNullOrEmpty(c.a.Value));

                    executionResult.Result = exists;

                    return executionResult;
                }
            }
            catch (Exception ex)
            {
                // Log or handle any exceptions
                throw;
            }
        }

        public async Task<ExecutionResult<bool>> CheckSocial()
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<bool>(true);
                    // Check if any documents match the provided form data
                    var exists = await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "CSR Initiatives or Programs" && !string.IsNullOrEmpty(c.a.Value))
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Environmental Sustainability Efforts" && !string.IsNullOrEmpty(c.a.Value));

                    executionResult.Result = exists;

                    return executionResult;
                }
            }
            catch (Exception ex)
            {
                // Log or handle any exceptions
                throw;
            }
        }

        public async Task<ExecutionResult<bool>> CheckAward()
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<bool>(true);
                    // Check if any documents match the provided form data
                    var exists = await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Industry Awards" && !string.IsNullOrEmpty(c.a.Value))
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Certifications" && !string.IsNullOrEmpty(c.a.Value));

                    executionResult.Result = exists;

                    return executionResult;
                }
            }
            catch (Exception ex)
            {
                // Log or handle any exceptions
                throw;
            }
        }

        public async Task<ExecutionResult<bool>> CheckCareer()
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<bool>(true);
                    // Check if any documents match the provided form data
                    var exists = await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Internship Programs" && !string.IsNullOrEmpty(c.a.Value))
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Job Openings" && !string.IsNullOrEmpty(c.a.Value)) 
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Opportunities for Career Growth and Development" && !string.IsNullOrEmpty(c.a.Value));

                    executionResult.Result = exists;

                    return executionResult;
                }
            }
            catch (Exception ex)
            {
                // Log or handle any exceptions
                throw;
            }
        }

        public async Task<ExecutionResult<bool>> CheckIntern()
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<bool>(true);
                    // Check if any documents match the provided form data
                    var exists = await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Desired Skills and Qualifications in Interns" && !string.IsNullOrEmpty(c.a.Value))
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Characteristics or Traits sought in Interns" && !string.IsNullOrEmpty(c.a.Value));


                    executionResult.Result = exists;

                    return executionResult;
                }
            }
            catch (Exception ex)
            {
                // Log or handle any exceptions
                throw;
            }
        }

        public async Task<ExecutionResult<List<AccountEntity>>> GetCompanyCandidates()
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    //var accountEntityId = id;

                    var executionResult = new ExecutionResult<List<AccountEntity>>(true);

                    var entity = await context.AccountEntities.FirstOrDefaultAsync(
                       r => r.Id == UserPrinciple.User.AccountEntityId
                   );
                    var branchi = await context.SubAccountBranches.FirstOrDefaultAsync(
                        r => r.Id == entity.SubAccountBranchId
                    );
                    var subacc = await context.SubAccounts.FirstOrDefaultAsync(
                        r => r.Id == branchi.SubAccountId
                    );

                    var allCandidates = await context.AccountEntities
                        .Join(context.SubAccountBranches, ae => ae.SubAccountBranchId, t => t.Id, (ae, t) => new { ae, t })
                        .Join(context.SubAccounts, a => a.t.SubAccountId, b => b.Id, (a,b) => new {a,b})
                        .Join(context.AccountEntityTypes, ax => ax.a.ae.AccountEntityTypeId, cd => cd.Id, (ax,cd) => new {ax,cd})
                        .Where(ts => ts.ax.b.ParentId == subacc.Id && ts.cd.TypeName == DefaultConfiguration.AccountEntityType.InternTypeName)
                        .Select(k => new AccountEntity
                        {
                            Id = k.ax.a.ae.Id,
                            EntityName = k.ax.a.ae.EntityName,
                            PhysicalAddress = k.ax.a.ae.PhysicalAddress,
                            ProfileImageUrl = k.ax.a.ae.ProfileImageUrl

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

        public async Task<ExecutionResult<List<CompanyProject>>> GetAllProjects()
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    //var accountEntityId = id;

                    var executionResult = new ExecutionResult<List<CompanyProject>>(true);

                   // var entity = await context.AccountEntities.FirstOrDefaultAsync(
                   //    r => r.Id == UserPrinciple.User.AccountEntityId
                   //);
                   // var branchi = await context.SubAccountBranches.FirstOrDefaultAsync(
                   //     r => r.Id == entity.SubAccountBranchId
                   // );
                   // var subacc = await context.SubAccounts.FirstOrDefaultAsync(
                   //     r => r.Id == branchi.SubAccountId
                   // );

                    var allProjects = await context.CompanyProjects
                        .Join(context.AccountEntities, ae => ae.AccountEntityId, t => t.Id, (ae, t) => new { ae, t })                                                
                        .Select(k => new CompanyProject
                        {
                            Id = k.ae.Id,
                            ProjectName = k.ae.ProjectName,
                            DatePosted = k.ae.DatePosted,
                            ProjectLocation = k.ae.ProjectLocation,
                            AccountEntity = new AccountEntity
                            {
                                EntityName = k.t.EntityName,
                                ProfileImageUrl = k.t.ProfileImageUrl,
                                PhysicalAddress = k.t.PhysicalAddress
                            }
                           

                        })
                        .ToListAsync();

                    if (allProjects.Count == 0)
                    {
                        executionResult.IsSuccessful = true;
                        executionResult.Message = "No Projects Available at the Moment";
                    }

                    executionResult.Result = allProjects;

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

        public async Task<ExecutionResult<List<CompanyProject>>> GetSpecificProjects()
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<List<CompanyProject>>(true);

                    // var entity = await context.AccountEntities.FirstOrDefaultAsync(
                    //    r => r.Id == UserPrinciple.User.AccountEntityId
                    //);
                    // var branchi = await context.SubAccountBranches.FirstOrDefaultAsync(
                    //     r => r.Id == entity.SubAccountBranchId
                    // );
                    // var subacc = await context.SubAccounts.FirstOrDefaultAsync(
                    //     r => r.Id == branchi.SubAccountId
                    // );

                    var allProjects = await context.CompanyProjects
                        .Join(context.AccountEntities, ae => ae.AccountEntityId, t => t.Id, (ae, t) => new { ae, t })
                        .Where(tx => tx.ae.AccountEntityId == accountEntityId)
                        .Select(k => new CompanyProject
                        {
                            Id = k.ae.Id,
                            ProjectName = k.ae.ProjectName,
                            DatePosted = k.ae.DatePosted,
                            ProjectLocation = k.ae.ProjectLocation,
                            AccountEntity = new AccountEntity
                            {
                                EntityName = k.t.EntityName,
                                ProfileImageUrl = k.t.ProfileImageUrl,
                                PhysicalAddress = k.t.PhysicalAddress
                            }


                        })
                        .ToListAsync();

                    if (allProjects.Count == 0)
                    {
                        executionResult.IsSuccessful = true;
                        executionResult.Message = "No Projects Available at the Moment";
                    }

                    executionResult.Result = allProjects;

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
    }
}

