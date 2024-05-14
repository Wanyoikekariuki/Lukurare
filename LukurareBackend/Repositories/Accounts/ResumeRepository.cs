using EFDatabaseModel.Contexts;
using EFDatabaseModel.DbModel;
using EFDatabaseModel.Repository;
using LukurareBackend.Repositories.Entities;
using Microsoft.EntityFrameworkCore;
using ProjectBase.Repository.Transaction;
using LukurareBackend.Models.Resume;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukurareBackend.Repositories.Accounts
{
   public class ResumeRepository : EFDatabaseModelBaseRepository<UserResume>
    {
        public ResumeRepository(string currentModule)
           : base(currentModule)
        { }

        public override Task<ExecutionResult<UserResume>> Add(UserResume model, EFDatabaseModelDatabaseContext context = null)
        {
            throw new NotImplementedException();
        }

        public override UserResume CloneModel(UserResume model)
        {
            throw new NotImplementedException();
        }

        public async Task<ExecutionResult<UserResume.Educational_Background>> EducationalBackground(UserResume.Educational_Background model)
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {
                        var accountEntityId = UserPrinciple.User.AccountEntityId;

                        var executionResult = new ExecutionResult<UserResume.Educational_Background>(true);

                        // Mapping between model properties and database entries
                        var propertyMappings = new Dictionary<string, string>
                    {
                        { "School", "Current School/University" },
                        { "Degree", "Degree Program/Diploma/Certificate" },
                        { "YearOfStudy", "Year of Study" },
                        { "ExpectedInternshipPeriod", "Expected Internship Period" },
                        { "ExpectedGraduationDate", "Expected Graduation Date" }
                    };

                        foreach (var property in propertyMappings)
                        {
                            var educationalType = context.AccountEntityTypeAdditionalDetails
                                .FirstOrDefault(t => t.KeyName == property.Value);

                            if (educationalType == null)
                            {
                                throw new Exception($"Education background type '{property.Value}' not found in database.");
                            }

                            // Create a new instance of AccountEntityTypeAdditionalDetailsValue
                            var educationalBackgroundValue = new AccountEntityTypeAdditionalDetailsValue
                            {
                                AccountEntityId = accountEntityId,
                                TypeAdditionalDetailsId = educationalType.Id,
                                Value = model.GetType().GetProperty(property.Key).GetValue(model).ToString()
                            };

                            // Add the newly created educational background value to the context
                            context.AccountEntityTypeAdditionalDetailsValues.Add(educationalBackgroundValue);
                        }

                        executionResult.Message = "Education Background added Successfully.";
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
                    throw new Exception("An error occurred while submiting an Education Background", ex);
                }
            }

        }

        public async Task<ExecutionResult<UserResume.Educational_Background>> UpdateEducationalBackground(UserResume.Educational_Background model)
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<UserResume.Educational_Background>(true);

                    // Mapping between model properties and database entries
                    var propertyMappings = new Dictionary<string, string>
                    {
                        { "School", "Current School/University" },
                        { "Degree", "Degree Program/Diploma/Certificate" },
                        { "YearOfStudy", "Year of Study" },
                        { "ExpectedInternshipPeriod", "Expected Internship Period" },
                        { "ExpectedGraduationDate", "Expected Graduation Date" }
                   };

                    foreach (var property in propertyMappings)
                    {
                        var educationalType = context.AccountEntityTypeAdditionalDetails
                            .FirstOrDefault(t => t.KeyName == property.Value);

                        if (educationalType == null)
                        {
                            throw new Exception($"Education background type '{property.Value}' not found in database.");
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
                    executionResult.Message = "Education Background updated Successfully.";
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
                throw new Exception("An error occurred while updating Education Background", ex);
            }
        }

        public async Task<ExecutionResult<UserResume.Skills>> UpdateSkills(UserResume.Skills model)
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<UserResume.Skills>(true);

                    // Mapping between model properties and database entries
                    var propertyMappings = new Dictionary<string, string>
                    {
                        { "TechnicalSkills", "Technical Skills" },
                        { "SoftSkills", "Soft Skills" },
                        { "LanguageProficiency", "Language Proficiency" }
                   };

                    foreach (var property in propertyMappings)
                    {
                        var educationalType = context.AccountEntityTypeAdditionalDetails
                            .FirstOrDefault(t => t.KeyName == property.Value);

                        if (educationalType == null)
                        {
                            throw new Exception($"Skills type '{property.Value}' not found in database.");
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
                    executionResult.Message = "Skills updated Successfully.";
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
                throw new Exception("An error occurred while updating Skills", ex);
            }
        }

        public async Task<ExecutionResult<UserResume.Extracurricular>> UpdateExtracurricular(UserResume.Extracurricular model)
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<UserResume.Extracurricular>(true);

                    // Mapping between model properties and database entries
                    var propertyMappings = new Dictionary<string, string>
                    {
                        { "Clubs", "Clubs or Organizations" },
                      { "LeadershipRoles", "Leadership Roles" },
                      { "Awards", "Awards or Achievements" }
                   };

                    foreach (var property in propertyMappings)
                    {
                        var educationalType = context.AccountEntityTypeAdditionalDetails
                            .FirstOrDefault(t => t.KeyName == property.Value);

                        if (educationalType == null)
                        {
                            throw new Exception($"Extracurricular type '{property.Value}' not found in database.");
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
                    executionResult.Message = "Extracurricular activities updated Successfully.";
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
                throw new Exception("An error occurred while updating Skills", ex);
            }
        }

        public async Task<ExecutionResult<UserResume.CareerInterest>> UpdateCareer(UserResume.CareerInterest model)
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<UserResume.CareerInterest>(true);

                    // Mapping between model properties and database entries
                    var propertyMappings = new Dictionary<string, string>
                    {
                       { "DesiredIndustry", "Desired Industry or Field" },
                       { "PreferredRoles", "Preferred Job Functions or Roles" },
                       { "CareerGoals", "Career Goals and Aspirations" }
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
                    executionResult.Message = "Career Interest updated Successfully.";
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
                throw new Exception("An error occurred while updating Skills", ex);
            }
        }

        public async Task<ExecutionResult<UserResume.Availability>> UpdateAvailability(UserResume.Availability model)
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<UserResume.Availability>(true);

                    // Mapping between model properties and database entries
                    var propertyMappings = new Dictionary<string, string>
                    {
                        { "InternshipAvailability", "Availability for Internships" },              
                        { "PreferredStartDate", "Preferred Start Date" }
                   };

                    foreach (var property in propertyMappings)
                    {
                        var educationalType = context.AccountEntityTypeAdditionalDetails
                            .FirstOrDefault(t => t.KeyName == property.Value);

                        if (educationalType == null)
                        {
                            throw new Exception($"Availability type '{property.Value}' not found in database.");
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
                    executionResult.Message = "Availability updated Successfully.";
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
                throw new Exception("An error occurred while updating Skills", ex);
            }
        }

        public async Task<ExecutionResult<UserResume.Certifications>> UpdateCertification(UserResume.Certifications model)
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<UserResume.Certifications>(true);

                    // Mapping between model properties and database entries
                    var propertyMappings = new Dictionary<string, string>
                    {
                        { "RelevantCertifications", "Relevant Certifications" },
                        { "TrainingPrograms", "Training Programs Attended" }
                   };

                    foreach (var property in propertyMappings)
                    {
                        var educationalType = context.AccountEntityTypeAdditionalDetails
                            .FirstOrDefault(t => t.KeyName == property.Value);

                        if (educationalType == null)
                        {
                            throw new Exception($"Certifications type '{property.Value}' not found in database.");
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
                    executionResult.Message = "Certifications updated Successfully.";
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
                throw new Exception("An error occurred while updating Skills", ex);
            }
        }

        public async Task<ExecutionResult<UserResume.WorkExperience>> UpdateWork(UserResume.WorkExperience model)
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<UserResume.WorkExperience>(true);

                    // Mapping between model properties and database entries
                    var propertyMappings = new Dictionary<string, string>
                    {
                        { "Internships", "Internships Work Experience" },
                        { "PartTimeJobs", "Part-time Jobs Work Experience" },
                        { "VolunteerWork", "Volunteer Work Work Experience" },
                        { "RelevantProjects", "Relevant Projects or Research Experience" }
                   };

                    foreach (var property in propertyMappings)
                    {
                        var educationalType = context.AccountEntityTypeAdditionalDetails
                            .FirstOrDefault(t => t.KeyName == property.Value);

                        if (educationalType == null)
                        {
                            throw new Exception($"Work Experience type '{property.Value}' not found in database.");
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
                    executionResult.Message = "Work Experience updated Successfully.";
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
                throw new Exception("An error occurred while updating Skills", ex);
            }
        }

        public async Task<ExecutionResult<UserResume.ProjectsDone>> UpdateProject(UserResume.ProjectsDone model)
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<UserResume.ProjectsDone>(true);

                    // Mapping between model properties and database entries
                    var propertyMappings = new Dictionary<string, string>
                    {
                        { "ProjectDescription", "Description of Projects Completed" },
                       { "ProjectObjectives", "Objectives and Outcomes Achieved" },
                       { "ProjectTechnologies", "Technologies or Tools Used" },
                       { "ProjectRoles", "Roles and Responsibilities" }
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

        public async Task<ExecutionResult<UserResume.References>> UpdateReferences(UserResume.References model)
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<UserResume.References>(true);

                    // Mapping between model properties and database entries
                    var propertyMappings = new Dictionary<string, string>
                    {
                        { "Reference1", "Reference 1" },
                        { "ContactDetails1", "Contact Details for Reference 1" },
                        { "Reference2", "Reference 2" },
                        { "ContactDetails2", "Contact Details for Reference 2" },
                        { "Reference3", "Reference 3" },
                        { "ContactDetails3", "Contact Details for Reference 3" }
                   };

                    foreach (var property in propertyMappings)
                    {
                        var educationalType = context.AccountEntityTypeAdditionalDetails
                            .FirstOrDefault(t => t.KeyName == property.Value);

                        if (educationalType == null)
                        {
                            throw new Exception($"References type '{property.Value}' not found in database.");
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
                    executionResult.Message = "References updated Successfully.";
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
                throw new Exception("An error occurred while updating References", ex);
            }
        }

        public async Task<ExecutionResult<UserResume.Portfolio>> UpdatePortfolio(UserResume.Portfolio model)
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<UserResume.Portfolio>(true);

                    // Mapping between model properties and database entries
                    var propertyMappings = new Dictionary<string, string>
                    {
                         { "PortfolioLink1", "Portfolio Link 1" },
                         { "WorkSample1", "Work Sample 1" },
                         { "PortfolioLink2", "Portfolio Link 2" },
                         { "WorkSample2", "Work Sample 2" },
                         { "PortfolioLink3", "Portfolio Link 3" },
                         { "WorkSample3", "Work Sample 3" }
                   };

                    foreach (var property in propertyMappings)
                    {
                        var educationalType = context.AccountEntityTypeAdditionalDetails
                            .FirstOrDefault(t => t.KeyName == property.Value);

                        if (educationalType == null)
                        {
                            throw new Exception($"Portfolio type '{property.Value}' not found in database.");
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
                    executionResult.Message = "Portfolio updated Successfully.";
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
                throw new Exception("An error occurred while updating References", ex);
            }
        }

        public async Task<ExecutionResult<bool>> CheckEducationalDetails()
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
                            && c.b.KeyName == "Current School/University" && !string.IsNullOrEmpty(c.a.Value))
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Degree Program/Diploma/Certificate" && !string.IsNullOrEmpty(c.a.Value))
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Year of Study" && !string.IsNullOrEmpty(c.a.Value))
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Expected Internship Period" &&  !string.IsNullOrEmpty(c.a.Value))
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Expected Graduation Date" &&  !string.IsNullOrEmpty(c.a.Value));

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

        public async Task<ExecutionResult<bool>> CheckSkills()
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
                            && c.b.KeyName == "Technical Skills" && !string.IsNullOrEmpty(c.a.Value))
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Soft Skills" && !string.IsNullOrEmpty(c.a.Value))
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Language Proficiency" && !string.IsNullOrEmpty(c.a.Value));
                      

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

        public async Task<ExecutionResult<bool>> CheckWorkExperience()
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
                            && c.b.KeyName == "Internships Work Experience" && !string.IsNullOrEmpty(c.a.Value))
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Part-time Jobs Work Experience" && !string.IsNullOrEmpty(c.a.Value))
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Volunteer Work Work Experience" && !string.IsNullOrEmpty(c.a.Value))
                             && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Relevant Projects or Research Experience" && !string.IsNullOrEmpty(c.a.Value));
                            


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
                            && c.b.KeyName == "Description of Projects Completed" && !string.IsNullOrEmpty(c.a.Value))
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Objectives and Outcomes Achieved" && !string.IsNullOrEmpty(c.a.Value))
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Technologies or Tools Used" && !string.IsNullOrEmpty(c.a.Value))
                             && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Roles and Responsibilities" && !string.IsNullOrEmpty(c.a.Value));



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

        public async Task<ExecutionResult<bool>> CheckExtraCurricular()
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
                            && c.b.KeyName == "Clubs or Organizations" && !string.IsNullOrEmpty(c.a.Value))
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Leadership Roles" && !string.IsNullOrEmpty(c.a.Value))
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Awards or Achievements" && !string.IsNullOrEmpty(c.a.Value));
                            



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
        public async Task<ExecutionResult<bool>> CheckCertification()
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
                            && c.b.KeyName == "Relevant Certifications" && !string.IsNullOrEmpty(c.a.Value))
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Training Programs Attended" && !string.IsNullOrEmpty(c.a.Value));
                     

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
                            && c.b.KeyName == "Desired Industry or Field" && !string.IsNullOrEmpty(c.a.Value))
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Preferred Job Functions or Roles" && !string.IsNullOrEmpty(c.a.Value))
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Career Goals and Aspirations" && !string.IsNullOrEmpty(c.a.Value));



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

        public async Task<ExecutionResult<bool>> CheckReference()
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
                            && c.b.KeyName == "Reference 1" && !string.IsNullOrEmpty(c.a.Value))
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Contact Details for Reference 1" && !string.IsNullOrEmpty(c.a.Value))
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Reference 2" && !string.IsNullOrEmpty(c.a.Value))
                         && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Contact Details for Reference 2" && !string.IsNullOrEmpty(c.a.Value))
                         && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Reference 3" && !string.IsNullOrEmpty(c.a.Value))
                         && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Contact Details for Reference 3" && !string.IsNullOrEmpty(c.a.Value));

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

        public async Task<ExecutionResult<bool>> CheckPortfolio()
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
                            && c.b.KeyName == "Portfolio Link 1" && !string.IsNullOrEmpty(c.a.Value))
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Work Sample 1" && !string.IsNullOrEmpty(c.a.Value))
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Portfolio Link 2" && !string.IsNullOrEmpty(c.a.Value))
                         && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Work Sample 2" && !string.IsNullOrEmpty(c.a.Value))
                         && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Portfolio Link 3" && !string.IsNullOrEmpty(c.a.Value))
                         && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Work Sample 3" && !string.IsNullOrEmpty(c.a.Value));

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

        public async Task<ExecutionResult<bool>> CheckAvailability()
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
                            && c.b.KeyName == "Availability for Internships" && !string.IsNullOrEmpty(c.a.Value))
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Preferred Start Date" && !string.IsNullOrEmpty(c.a.Value));

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

        public async Task<ExecutionResult<bool>> CheckPreference()
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
                            && c.b.KeyName == "Location Preference for Internships" && !string.IsNullOrEmpty(c.a.Value))
                        && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Industry Preferences" && !string.IsNullOrEmpty(c.a.Value))
                         && await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, a => a.TypeAdditionalDetailsId, b => b.Id, (a, b) => new { a, b })
                        .AnyAsync(c => c.a.AccountEntityId == accountEntityId
                            && c.b.KeyName == "Company Size or Type Preferences" && !string.IsNullOrEmpty(c.a.Value));

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

        public async Task<ExecutionResult<UserResume.Skills>> Skills(UserResume.Skills model)
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<UserResume.Skills>(true);

                    // Mapping between model properties and database entries
                    var propertyMappings = new Dictionary<string, string>
                    {
                        { "TechnicalSkills", "Technical Skills" },
                        { "SoftSkills", "Soft Skills" },
                        { "LanguageProficiency", "Language Proficiency" }                
            };

                    foreach (var property in propertyMappings)
                    {
                        var skillType = context.AccountEntityTypeAdditionalDetails
                            .FirstOrDefault(t => t.KeyName == property.Value);

                        if (skillType == null)
                        {
                            throw new Exception($"Skill type '{property.Value}' not found in the database.");
                        }

                        // Create a new instance of AccountEntityTypeAdditionalDetailsValue
                        var skillValue = new AccountEntityTypeAdditionalDetailsValue
                        {
                            AccountEntityId = accountEntityId,
                            TypeAdditionalDetailsId = skillType.Id,
                            Value = model.GetType().GetProperty(property.Key).GetValue(model).ToString()
                        };

                        // Add the newly created skill value to the context
                        context.AccountEntityTypeAdditionalDetailsValues.Add(skillValue);
                    }

                    executionResult.Message = "Skills added successfully.";
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
                throw new Exception("An error occurred while submitting skills data", ex);
            }
        }

        public async Task<ExecutionResult<UserResume.WorkExperience>> WorkExperience(UserResume.WorkExperience model)
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<UserResume.WorkExperience>(true);

                    // Mapping between model properties and database entries
                    var propertyMappings = new Dictionary<string, string>
            {
                { "Internships", "Internships Work Experience" },
                { "PartTimeJobs", "Part-time Jobs Work Experience" },
                { "VolunteerWork", "Volunteer Work Work Experience" },
                { "RelevantProjects", "Relevant Projects or Research Experience" }
                // Add more mappings as needed
            };

                    foreach (var property in propertyMappings)
                    {
                        var experienceType = context.AccountEntityTypeAdditionalDetails
                            .FirstOrDefault(t => t.KeyName == property.Value);

                        if (experienceType == null)
                        {
                            throw new Exception($"Experience type '{property.Value}' not found in the database.");
                        }

                        // Create a new instance of AccountEntityTypeAdditionalDetailsValue
                        var experienceValue = new AccountEntityTypeAdditionalDetailsValue
                        {
                            AccountEntityId = accountEntityId,
                            TypeAdditionalDetailsId = experienceType.Id,
                            Value = model.GetType().GetProperty(property.Key).GetValue(model).ToString()
                        };

                        // Add the newly created experience value to the context
                        context.AccountEntityTypeAdditionalDetailsValues.Add(experienceValue);
                    }

                    executionResult.Message = "Work experience added successfully.";
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
                throw new Exception("An error occurred while submitting work experience data", ex);
            }
        }

        public async Task<ExecutionResult<UserResume.ProjectsDone>> Projects(UserResume.ProjectsDone model)
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<UserResume.ProjectsDone>(true);

                    // Mapping between model properties and database entries
                    var propertyMappings = new Dictionary<string, string>
                    {
                       { "ProjectDescription", "Description of Projects Completed" },
                       { "ProjectObjectives", "Objectives and Outcomes Achieved" },
                       { "ProjectTechnologies", "Technologies or Tools Used" },
                       { "ProjectRoles", "Roles and Responsibilities" }
                // Add more mappings as needed
            };

                    foreach (var property in propertyMappings)
                    {
                        var projectType = context.AccountEntityTypeAdditionalDetails
                            .FirstOrDefault(t => t.KeyName == property.Value);

                        if (projectType == null)
                        {
                            throw new Exception($"Project type '{property.Value}' not found in the database.");
                        }

                        // Create a new instance of AccountEntityTypeAdditionalDetailsValue
                        var projectValue = new AccountEntityTypeAdditionalDetailsValue
                        {
                            AccountEntityId = accountEntityId,
                            TypeAdditionalDetailsId = projectType.Id,
                            Value = model.GetType().GetProperty(property.Key).GetValue(model).ToString()
                        };

                        // Add the newly created project value to the context
                        context.AccountEntityTypeAdditionalDetailsValues.Add(projectValue);
                    }
                    executionResult.IsSuccessful = true;
                    executionResult.Message = "Projects added successfully";
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
                throw new Exception("An error occurred while submitting projects done data", ex);
            }
        }

        public async Task<ExecutionResult<UserResume.Extracurricular>> Extracurricular(UserResume.Extracurricular model)
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<UserResume.Extracurricular>(true);

                    // Mapping between model properties and database entries
                    var propertyMappings = new Dictionary<string, string>
                    {
                      { "Clubs", "Clubs or Organizations" },
                      { "LeadershipRoles", "Leadership Roles" },
                      { "Awards", "Awards or Achievements" }
                // Add more mappings as needed
                    };

                    foreach (var property in propertyMappings)
                    {
                        var extracurricularType = context.AccountEntityTypeAdditionalDetails
                            .FirstOrDefault(t => t.KeyName == property.Value);

                        if (extracurricularType == null)
                        {
                            throw new Exception($"Extracurricular type '{property.Value}' not found in the database.");
                        }

                        // Create a new instance of AccountEntityTypeAdditionalDetailsValue
                        var extracurricularValue = new AccountEntityTypeAdditionalDetailsValue
                        {
                            AccountEntityId = accountEntityId,
                            TypeAdditionalDetailsId = extracurricularType.Id,
                            Value = model.GetType().GetProperty(property.Key).GetValue(model).ToString()
                        };

                        // Add the newly created extracurricular value to the context
                        context.AccountEntityTypeAdditionalDetailsValues.Add(extracurricularValue);
                    }
                    executionResult.IsSuccessful = true;
                    executionResult.Message = "Extracurricular activities added successfully";
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
                throw new Exception("An error occurred while submitting extracurricular activities data", ex);
            }
        }
        public async Task<ExecutionResult<UserResume.Certifications>> CertificationsTraining(UserResume.Certifications model)
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<UserResume.Certifications>(true);

                    // Mapping between model properties and database entries
                    var propertyMappings = new Dictionary<string, string>
                    {
                      { "RelevantCertifications", "Relevant Certifications" },
                      { "TrainingPrograms", "Training Programs Attended" }
                // Add more mappings as needed
            };

                    foreach (var property in propertyMappings)
                    {
                        var certificationType = context.AccountEntityTypeAdditionalDetails
                            .FirstOrDefault(t => t.KeyName == property.Value);

                        if (certificationType == null)
                        {
                            throw new Exception($"Certification type '{property.Value}' not found in the database.");
                        }

                        // Create a new instance of AccountEntityTypeAdditionalDetailsValue
                        var certificationValue = new AccountEntityTypeAdditionalDetailsValue
                        {
                            AccountEntityId = accountEntityId,
                            TypeAdditionalDetailsId = certificationType.Id,
                            Value = model.GetType().GetProperty(property.Key).GetValue(model).ToString()
                        };

                        // Add the newly created certification value to the context
                        context.AccountEntityTypeAdditionalDetailsValues.Add(certificationValue);
                    }

                    executionResult.Message = "Certifications and training added successfully.";
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
                throw new Exception("An error occurred while submitting certifications and training data", ex);
            }
        }

        public async Task<ExecutionResult<UserResume.CareerInterest>> CareerInterest(UserResume.CareerInterest model)
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<UserResume.CareerInterest>(true);

                    // Mapping between model properties and database entries
                    var propertyMappings = new Dictionary<string, string>
            {
                { "DesiredIndustry", "Desired Industry or Field" },
                { "PreferredRoles", "Preferred Job Functions or Roles" },
                { "CareerGoals", "Career Goals and Aspirations" }
                // Add more mappings as needed
            };

                    foreach (var property in propertyMappings)
                    {
                        var interestType = context.AccountEntityTypeAdditionalDetails
                            .FirstOrDefault(t => t.KeyName == property.Value);

                        if (interestType == null)
                        {
                            throw new Exception($"Interest type '{property.Value}' not found in the database.");
                        }

                        // Create a new instance of AccountEntityTypeAdditionalDetailsValue
                        var interestValue = new AccountEntityTypeAdditionalDetailsValue
                        {
                            AccountEntityId = accountEntityId,
                            TypeAdditionalDetailsId = interestType.Id,
                            Value = model.GetType().GetProperty(property.Key).GetValue(model).ToString()
                        };

                        // Add the newly created interest value to the context
                        context.AccountEntityTypeAdditionalDetailsValues.Add(interestValue);
                    }

                    executionResult.Message = "Career interests added successfully.";
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
                throw new Exception("An error occurred while submitting career interest data", ex);
            }
        }

        public async Task<ExecutionResult<UserResume.References>> References(UserResume.References model)
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<UserResume.References>(true);

                    // Mapping between model properties and database entries
                    var propertyMappings = new Dictionary<string, string>
            {
                { "Reference1", "Reference 1" },
                { "ContactDetails1", "Contact Details for Reference 1" },
                { "Reference2", "Reference 2" },
                { "ContactDetails2", "Contact Details for Reference 2" },
                { "Reference3", "Reference 3" },
                { "ContactDetails3", "Contact Details for Reference 3" }                
            };

                    foreach (var property in propertyMappings)
                    {
                        var referenceType = context.AccountEntityTypeAdditionalDetails
                            .FirstOrDefault(t => t.KeyName == property.Value);

                        if (referenceType == null)
                        {
                            throw new Exception($"Reference type '{property.Value}' not found in the database.");
                        }

                        // Create a new instance of AccountEntityTypeAdditionalDetailsValue
                        var referenceValue = new AccountEntityTypeAdditionalDetailsValue
                        {
                            AccountEntityId = accountEntityId,
                            TypeAdditionalDetailsId = referenceType.Id,
                            Value = model.GetType().GetProperty(property.Key).GetValue(model).ToString()
                        };

                        // Add the newly created reference value to the context
                        context.AccountEntityTypeAdditionalDetailsValues.Add(referenceValue);
                    }

                    executionResult.Message = "References added successfully.";
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
                throw new Exception("An error occurred while submitting reference data", ex);
            }
        }

        public async Task<ExecutionResult<UserResume.Portfolio>> Portfolio(UserResume.Portfolio model)
        {
            try
            {
                // Assuming EFDatabaseModelDatabaseContext is your EF database context
                using (var context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<UserResume.Portfolio>(true);

                    // Mapping between model properties and database entries
                    var propertyMappings = new Dictionary<string, string>
                {
                    { "PortfolioLink1", "Portfolio Link 1" },
                    { "WorkSample1", "Work Sample 1" },
                    { "PortfolioLink2", "Portfolio Link 2" },
                    { "WorkSample2", "Work Sample 2" },
                    { "PortfolioLink3", "Portfolio Link 3" },
                    { "WorkSample3", "Work Sample 3" }
                    // Add more mappings as needed
                };

                    foreach (var property in propertyMappings)
                    {
                        var portfolioType = context.AccountEntityTypeAdditionalDetails
                            .FirstOrDefault(t => t.KeyName == property.Value);

                        if (portfolioType == null)
                        {
                            throw new Exception($"Portfolio type '{property.Value}' not found in the database.");
                        }

                        // Create a new instance of AccountEntityTypeAdditionalDetailsValue
                        var portfolioValue = new AccountEntityTypeAdditionalDetailsValue
                        {
                            AccountEntityId = accountEntityId,
                            TypeAdditionalDetailsId = portfolioType.Id,
                            Value = (string)model.GetType().GetProperty(property.Key).GetValue(model)
                        };

                        // Add the newly created portfolio value to the context
                        context.AccountEntityTypeAdditionalDetailsValues.Add(portfolioValue);
                    }

                    executionResult.Message = "Portfolio added successfully.";
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
                throw new Exception("An error occurred while submitting portfolio data", ex);
            }
        }

        public async Task<ExecutionResult<UserResume.Availability>> Availability(UserResume.Availability model)
        {
            try
            {
                using (var context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<UserResume.Availability>(true);

                    // Mapping between model properties and database entries
                    var propertyMappings = new Dictionary<string, string>
            {
                { "InternshipAvailability", "Availability for Internships" },
                // Assuming EFDatabaseModelDatabaseContext.AccountEntityTypeAdditionalDetails represents the entity set for additional details
                // You need to adjust this based on your database schema
                { "PreferredStartDate", "Preferred Start Date" }
                // Add more mappings as needed
            };

                    foreach (var property in propertyMappings)
                    {
                        var availabilityType = context.AccountEntityTypeAdditionalDetails
                            .FirstOrDefault(t => t.KeyName == property.Value);

                        if (availabilityType == null)
                        {
                            throw new Exception($"Availability type '{property.Value}' not found in the database.");
                        }

                        // Create a new instance of AccountEntityTypeAdditionalDetailsValue
                        var availabilityValue = new AccountEntityTypeAdditionalDetailsValue
                        {
                            AccountEntityId = accountEntityId,
                            TypeAdditionalDetailsId = availabilityType.Id,
                            Value = model.GetType().GetProperty(property.Key).GetValue(model)?.ToString()
                        };

                        // Add the newly created availability value to the context
                        context.AccountEntityTypeAdditionalDetailsValues.Add(availabilityValue);
                    }

                    executionResult.Message = "Career interests added successfully.";
                    executionResult.IsSuccessful = true;
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
                throw new Exception("An error occurred while submitting availability data", ex);
            }
        }

        public async Task<ExecutionResult<UserResume.Preferences>> Preferences(UserResume.Preferences model)
        {
            try
            {
                using (var context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<UserResume.Preferences>(true);

                    // Mapping between model properties and database entries
                    var propertyMappings = new Dictionary<string, string>
            {
                { "LocationPreference", "Location Preference for Internships" },
                { "IndustryPreferences", "Industry Preferences" },
                { "CompanyPreferences", "Company Size or Type Preferences" }
                // Add more mappings as needed
            };

                    foreach (var property in propertyMappings)
                    {
                        var preferenceType = context.AccountEntityTypeAdditionalDetails
                            .FirstOrDefault(t => t.KeyName == property.Value);

                        if (preferenceType == null)
                        {
                            throw new Exception($"Preference type '{property.Value}' not found in the database.");
                        }

                        // Create a new instance of AccountEntityTypeAdditionalDetailsValue
                        var preferenceValue = new AccountEntityTypeAdditionalDetailsValue
                        {
                            AccountEntityId = accountEntityId,
                            TypeAdditionalDetailsId = preferenceType.Id,
                            Value = model.GetType().GetProperty(property.Key).GetValue(model)?.ToString()
                        };

                        // Add the newly created preference value to the context
                        context.AccountEntityTypeAdditionalDetailsValues.Add(preferenceValue);
                    }

                    executionResult.Message = "Preferences added successfully.";
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
                throw new Exception("An error occurred while submitting preferences data", ex);
            }
        }


        public override Task<ExecutionResult<UserResume>> GetItem(int id, EFDatabaseModelDatabaseContext context = null)
        {
            throw new NotImplementedException();
        }

        

        public override Task<ExecutionResult<UserResume>> Update(UserResume model, EFDatabaseModelDatabaseContext context = null)
        {
            throw new NotImplementedException();
        }
    }
}
