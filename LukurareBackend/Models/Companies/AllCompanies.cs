using System;
using System.Collections.Generic;
using System.Text;

namespace LukurareBackend.Models.Companies
{
    public class AllCompanies
    {
               public class CompanyOverview
            {
                public string MissionStatement { get; set; }
                public string Vision { get; set; }
                public string CoreValues { get; set; }
                public string HistoryBackground { get; set; }                
            }

            public class CoreCompetencies
            {
                public string AreaofExpertise { get; set; }
                public string Specializations { get; set; }                
            }

            public class InternshipType
            {
                public string Internships { get; set; }
                
            }
            public class Projects
            {
                public string OngoingProjects { get; set; }
                public string PastProjects { get; set; }
                public string ProjectTechnologies { get; set; }               
            }
            public class CompanyCulture
            {
                public string WorkEnvironment { get; set; }
                public string CompanyValues { get; set; }               
            }

            public class TechnologicalCapabilities
            {
                public string TechnologiesUsed { get; set; }
                public string InnovationInitiatives { get; set; }
            }

            public class SocialResponsibility
            {
                public string Programs { get; set; }
                public string EnvironmentalEfforts { get; set; }                
            }
            public class Awards
            {
                public string IndustryAwards { get; set; }
                public string Certifications { get; set; }               
            }
            public class CareerOpportunities
            {
                public string InternshipPrograms { get; set; }
                public string JobOpenings { get; set; }
                public string CareerGrowth { get; set; }                
            }
            public class InternProfile
            {
                public string DesiredSkills { get; set; }
                public string InternTraits { get; set; }
            }

            public class CompanyProject
            {
               public string ProjectCategory { get; set; }
               public string ProjectDetails { get; set; }
               public string ProjectStart { get; set; }
               public string ProjectName { get; set; }
               public string ProjectEnd { get; set; }
               public string OpenPositions { get; set; }
               public string ProjectRecommendation { get; set; }
               public string WorkScope { get; set; }
               public string PostAs { get; set; }
        }


    }
    }
