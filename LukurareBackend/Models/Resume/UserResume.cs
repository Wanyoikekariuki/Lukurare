using System;
using System.Collections.Generic;
using System.Text;

namespace LukurareBackend.Models.Resume
{
    public class UserResume
    {
        public class Educational_Background
        {
            public string School { get; set; }
            public string Degree { get; set; }
            public int YearOfStudy { get; set; }
            public string ExpectedInternshipPeriod { get; set; }
            public DateTime ExpectedGraduationDate { get; set; }
        }

        public class Skills
        {
            public string TechnicalSkills { get; set; }
            public string SoftSkills { get; set; }
            public string LanguageProficiency { get; set; }
        }

        public class WorkExperience
        {
            public string Internships { get; set; }
            public string PartTimeJobs { get; set; }
            public string VolunteerWork { get; set; }
            public string RelevantProjects { get; set; }
        }
        public class ProjectsDone
        {
            public string ProjectDescription { get; set; }
            public string ProjectObjectives { get; set; }
            public string ProjectTechnologies { get; set; }
            public string ProjectRoles { get; set; }
        }
        public class Extracurricular
        {
            public string Clubs { get; set; }
            public string LeadershipRoles { get; set; }
            public string Awards { get; set; }
        }

        public class Certifications
        {
            public string RelevantCertifications { get; set; }
            public string TrainingPrograms { get; set; }
        }

        public class CareerInterest
        {
            public string DesiredIndustry { get; set; }
            public string PreferredRoles { get; set; }
            public string CareerGoals { get; set; }
        }
        public class References
        {
            public string Reference1 { get; set; }
            public string ContactDetails1 { get; set; }
            public string Reference2 { get; set; }
            public string ContactDetails2 { get; set; }
            public string Reference3 { get; set; }
            public string ContactDetails3 { get; set; }
        }
        public class Portfolio
        {
            public string PortfolioLink1 { get; set; }
            public string WorkSample1 { get; set; }
            public string PortfolioLink2 { get; set; }
            public string WorkSample2 { get; set; }
            public string PortfolioLink3 { get; set; }
            public string WorkSample3 { get; set; }
        }
        public class Availability
        {
            public string InternshipAvailability { get; set; }
            public DateTime PreferredStartDate { get; set; }
        }

        public class Preferences
        {
            public string LocationPreference { get; set; }
            public string IndustryPreferences { get; set; }
            public string CompanyPreferences { get; set; }
        }
    }
}
