

var MVVM = {
    blockUiPageElement: function (element) {
        return login.blockUiPageElementWithMessage(element, 'Please wait...');
    },

    blockUiPageElementWithMessage: function (element, message) {
        //element.block
        $.blockUI({
            message:
                '<div class="d-flex justify-content-center align-items-center"><p class="me-50 mb-0">' + message + '</p> <div class="spinner-grow spinner-grow-sm text-white" role="status"></div> </div>',
            css: {
                backgroundColor: 'transparent',
                color: '#fff',
                border: '0',
                zIndex: 19999
            },
            overlayCSS: {
                opacity: 0.5
            }
        });
    },

    

    unBlockUiPageElement: function (element) {
        $.unblockUI();
    },


    init: function () {

        var viewModel = function () {
            debugger;
            var self = this;
            this.hasError = ko.observable(false);
            this.errorMessage = ko.observable();
            this.Balance = ko.observable(0);
            this.PhotoLinkUrl = ko.observable();
            this.ParentPhone = ko.observable();
            this.ParentEmail = ko.observable();
            this.Brand = ko.observable();
            this.Location = ko.observable();
            this.AgentMenueVisible = ko.observable(false);
            this.AdminMenueVisible = ko.observable(false);
            this.CustomerMenueVisible = ko.observable(false);
            this.InternMenueVisible = ko.observable(false);
            this.newNotificationCount = ko.observable(0);
            this.UserNotifications = ko.observableArray([]);
            this.expanded = ko.observable();
            this.truncateMessage = ko.observable();
            this.showFullMessageFlag = ko.observable(false);
            this.selectedNotification = ko.observable(null);
            this.emailSubject = ko.observable();
            this.emailMessage = ko.observable();
            this.EmailMail = ko.observable();
            this.EmailPhoneNumber = ko.observable();
            this.chatMessages = ko.observableArray([]);
            this.appendedChatElements = [];
            this.timeStamp = ko.observable("0");
            this.newChatCount = ko.observable(0);
            this.chatName = ko.observable();
            this.selectCont = ko.observable();
            this.toastMessage = ko.observable("");
            this.toastHeader = ko.observable("");
            this.ProfileImageLink = ko.observable("");
            this.school = ko.observable("");
            this.degree = ko.observable("");
            this.year = ko.observable("");
            this.expectedInternshipPeriod = ko.observable("");
            this.expectedGraduationDate = ko.observable("");
            this.technicalSkills = ko.observable("");
            this.softSkills = ko.observable("");
            this.languageProficiency = ko.observable("");
            this.internships = ko.observable("");
            this.partTimeJobs = ko.observable("");
            this.volunteerWork = ko.observable("");
            this.relevantProjects = ko.observable("");
            this.projectDescription = ko.observable("");
            this.projectObjectives = ko.observable("");
            this.projectTechnologies = ko.observable("");
            this.projectRoles = ko.observable("");
            this.clubs = ko.observable("");
            this.leadershipRoles = ko.observable("");
            this.awards = ko.observable("");
            this.relevantCertifications = ko.observable("");
            this.trainingPrograms = ko.observable("");
            this.desiredIndustry = ko.observable();
            this.preferredRoles = ko.observable();
            this.careerGoals = ko.observable();
            this.reference1 = ko.observable("");
            this.contact1 = ko.observable("");
            this.reference2 = ko.observable("");
            this.contact2 = ko.observable("");
            this.reference3 = ko.observable("");
            this.contact3 = ko.observable("");
            this.portfolioLink1 = ko.observable('');
            this.workSample1 = ko.observable('');
            this.portfolioLink2 = ko.observable('');
            this.workSample2 = ko.observable('');
            this.portfolioLink3 = ko.observable('');
            this.workSample3 = ko.observable('');
            this.internshipAvailability = ko.observable();
            this.preferredStartDate = ko.observable();
            this.locationPreference = ko.observable();
            this.industryPreferences = ko.observable();
            this.companyPreferences = ko.observable();
            self.Candidates = ko.observableArray([]);
            this.CandidateId = ko.observable();
            this.CandidateName = ko.observable();
            this.CandidateLocation = ko.observable();
            this.CandidateProfileImage = ko.observable();
            this.EntityId = ko.observable();
            this.EntityName = ko.observable();
            this.EntityPhone = ko.observable();
            this.EntityEmail = ko.observable();
            this.EntityProfileImage = ko.observable();
            this.birthDate = ko.observable();
            this.gender = ko.observable();
            this.Companies = ko.observableArray([]);
            this.selectedCompany = ko.observable();
            self.Applications = ko.observableArray([]);
            this.ApplicationStatus = ko.observable();
            this.ApplicationCandidate = ko.observable();
            this.ApplicationCandidatePhone = ko.observable();
            this.ApplicationCandidateEmail = ko.observable();
            this.ApplicationCandidateProfile = ko.observable();
            this.ApplicationStatus = ko.observable();
            this.ApplicationId = ko.observable();
            this.ProjectId = ko.observable();
            this.ApplicationProjectName = ko.observable();
            this.ApplicationProjectCategory = ko.observable();
            this.ApplicationProjectDetails = ko.observable();
            this.ApplicationProjectWorkScope = ko.observable();
            this.ApplicationProjectRecommendation = ko.observable();
            this.ApplicationCandidateLocation = ko.observable();
            this.ApplicationProjectStart = ko.observable();
            this.ApplicationProjectEnd = ko.observable();
            this.ApplicationProjectLocation = ko.observable();
            self.currentPage = ko.observable(1);
            // Number of projects per page
            self.candidatesPerPage = ko.observable(20);
            self.applicationsPerPage = ko.observable(10);
            this.NoOfCandidates = ko.observable();
            this.NoOfApplications = ko.observable();

            self.candidatesOnCurrentPage = ko.computed(function () {
                debugger;
                /*var self = this;*/
                if (self.Candidates() && self.Candidates().length > 0) {
                    var startIndex = (self.currentPage() - 1) * self.candidatesPerPage();
                    return self.Candidates.slice(startIndex, startIndex + self.candidatesPerPage());
                } else {
                    return [];
                }
            });

            self.applicationsOnCurrentPage = ko.computed(function () {
                debugger;
                /*var self = this;*/
                if (self.Applications() && self.Applications().length > 0) {
                    var startIndex = (self.currentPage() - 1) * self.applicationsPerPage();
                    return self.Applications.slice(startIndex, startIndex + self.applicationsPerPage());
                } else {
                    return [];
                }
            });


            // Function to go to a specific page
            self.goToPage = function (page) {
                debugger;
                console.log(page);
                self.currentPage(page);
            };

            this.checkEducationDetails = function () {
                var self = this;
                this.hasError(false);

                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {

                        if (data.Result == true) {
                            $('#educationalBackgroundCard').hide(); // Hide the card if details exist
                        }
                    }
                };

                var functionFailed = function (hasError, Message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', Message, "red", null);
                    }
                    // Handle other failure cases if necessary
                };


                // Make a GET request with the form data
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Resume/Resume/CheckEducationDetails", functionDone, functionFailed);
            };

            // Call the function to check form details
            this.checkEducationDetails();


            this.checkSkills = function () {
                var self = this;
                this.hasError(false);

                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {

                        if (data.Result == true) {
                            $('#skillsCard').hide(); // Hide the card if details exist
                        }
                    }
                };

                var functionFailed = function (hasError, Message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', Message, "red", null);
                    }
                    // Handle other failure cases if necessary
                };


                // Make a GET request with the form data
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Resume/Resume/CheckSkills", functionDone, functionFailed);
            };

            // Call the function to check form details
            this.checkSkills();

            this.checkWorkExperience = function () {
                var self = this;
                this.hasError(false);

                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {

                        if (data.Result == true) {
                            $('#workExperienceCard').hide(); // Hide the card if details exist
                        }
                    }
                };

                var functionFailed = function (hasError, Message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', Message, "red", null);
                    }
                    // Handle other failure cases if necessary
                };


                // Make a GET request with the form data
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Resume/Resume/CheckWorkExperience", functionDone, functionFailed);
            };

            // Call the function to check form details
            this.checkWorkExperience();

            this.checkProjects = function () {
                var self = this;
                this.hasError(false);

                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {

                        if (data.Result == true) {
                            $('#projectsCard').hide(); // Hide the card if details exist
                        }
                    }
                };

                var functionFailed = function (hasError, Message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', Message, "red", null);
                    }
                    // Handle other failure cases if necessary
                };


                // Make a GET request with the form data
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Resume/Resume/CheckProjects", functionDone, functionFailed);
            };

            // Call the function to check form details
            this.checkProjects();

            this.checkExtraCurricular = function () {
                var self = this;
                this.hasError(false);

                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {

                        if (data.Result == true) {
                            $('#extraCurricularCard').hide(); // Hide the card if details exist
                        }
                    }
                };

                var functionFailed = function (hasError, Message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', Message, "red", null);
                    }
                    // Handle other failure cases if necessary
                };


                // Make a GET request with the form data
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Resume/Resume/CheckExtraCurricular", functionDone, functionFailed);
            };

            // Call the function to check form details
            this.checkExtraCurricular();

            this.checkCertifications = function () {
                var self = this;
                this.hasError(false);

                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {

                        if (data.Result == true) {
                            $('#certificationCard').hide(); // Hide the card if details exist
                        }
                    }
                };

                var functionFailed = function (hasError, Message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', Message, "red", null);
                    }
                    // Handle other failure cases if necessary
                };


                // Make a GET request with the form data
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Resume/Resume/CheckCertification", functionDone, functionFailed);
            };

            // Call the function to check form details
            this.checkCertifications();

            this.checkCareer = function () {
                var self = this;
                this.hasError(false);

                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {

                        if (data.Result == true) {
                            $('#careerCard').hide(); // Hide the card if details exist
                        }
                    }
                };

                var functionFailed = function (hasError, Message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', Message, "red", null);
                    }
                    // Handle other failure cases if necessary
                };


                // Make a GET request with the form data
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Resume/Resume/CheckCareer", functionDone, functionFailed);
            };

            // Call the function to check form details
            this.checkCareer();

            this.checkReference = function () {
                var self = this;
                this.hasError(false);

                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {

                        if (data.Result == true) {
                            $('#referenceCard').hide(); // Hide the card if details exist
                        }
                    }
                };

                var functionFailed = function (hasError, Message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', Message, "red", null);
                    }
                    // Handle other failure cases if necessary
                };


                // Make a GET request with the form data
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Resume/Resume/CheckReference", functionDone, functionFailed);
            };

            // Call the function to check form details
            this.checkReference();

            this.checkPortfolio = function () {
                var self = this;
                this.hasError(false);

                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {

                        if (data.Result == true) {
                            $('#portfolioCard').hide(); // Hide the card if details exist
                        }
                    }
                };

                var functionFailed = function (hasError, Message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', Message, "red", null);
                    }
                    // Handle other failure cases if necessary
                };


                // Make a GET request with the form data
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Resume/Resume/CheckPortfolio", functionDone, functionFailed);
            };

            // Call the function to check form details
            this.checkPortfolio();

            this.checkAvailability = function () {
                var self = this;
                this.hasError(false);

                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {

                        if (data.Result == true) {
                            $('#availabilityCard').hide(); // Hide the card if details exist
                        }
                    }
                };

                var functionFailed = function (hasError, Message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', Message, "red", null);
                    }
                    // Handle other failure cases if necessary
                };


                // Make a GET request with the form data
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Resume/Resume/CheckAvailability", functionDone, functionFailed);
            };

            // Call the function to check form details
            this.checkAvailability();

            // Initially hide the success card
            $('#successMessage').hide();

            this.checkPreferences = function () {
                var self = this;
                this.hasError(false);

                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {

                        if (data.Result == true) {
                            $('#preferenceCard').hide(); // Hide the card if details exist
                            $('#successMessage').show(); // Show the success card
                        }
                    }
                };

                var functionFailed = function (hasError, Message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', Message, "red", null);
                    }
                    // Handle other failure cases if necessary
                };


                // Make a GET request with the form data
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Resume/Resume/CheckPreference", functionDone, functionFailed);
            };

            // Call the function to check form details
            this.checkPreferences();


            this.educationBackground = function () {
                debugger;
                var self = this;
                // Function to be executed on successful AJAX request
                var functionDone = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else if (data.IsOkay == false) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed', data.Message, "red", null);
                        self.clearForm();
                    } else {
                        if (data.Message === 'Education Background added Successfully.') {
                            jqueryConfirmGenerics.showOkAlertBox('Education Background added Successfully.', data.Message, "green", null);
                            window.location.reload();
                        }
                    }
                };

                // Function to be executed on failed AJAX request
                var functionFailed = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };

                var data = {
                    School: self.school(),
                    Degree: self.degree(),
                    YearOfStudy: self.year(),
                    ExpectedInternshipPeriod: self.expectedInternshipPeriod(),
                    ExpectedGraduationDate: convertToUTC(self.expectedGraduationDate())
                };

                // Make AJAX request
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Resume/Resume/EducationalBackground", data, functionDone, functionFailed);
            };

            function convertToUTC(localDateTime) {
                // Create a Date object
                var date = new Date(localDateTime);

                // Convert to UTC string
                var utcDateTime = date.toISOString();

                // Remove milliseconds
                utcDateTime = utcDateTime.slice(0, -5) + 'Z';

                return utcDateTime;
            }
            this.submitSkills = function () {
                debugger;
                var self = this;

                // Function to be executed on successful AJAX request
                var functionDone = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else if (data.IsOkay == false) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed', data.Message, "red", null);
                        self.clearForm();
                    } else {
                        if (data.Message === 'Skills added successfully.') {
                            jqueryConfirmGenerics.showOkAlertBox('Skills Added!', data.Message, "green", null);
                            window.location.reload();
                        } else {
                            jqueryConfirmGenerics.showOkAlertBox('Skills Not Added!', data.Message, "red", null);
                            self.clearForm();
                        }
                    }
                };

                // Function to be executed on failed AJAX request
                var functionFailed = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };

                var data = {
                    TechnicalSkills: self.technicalSkills(),
                    SoftSkills: self.softSkills(),
                    LanguageProficiency: self.languageProficiency()

                };

                // Make AJAX request
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Resume/Resume/Skills", data, functionDone, functionFailed);
            };
            this.submitWorkExperience = function () {
                debugger;
                var self = this;


                // Function to be executed on successful AJAX request
                var functionDone = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else if (data.IsOkay == false) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed', data.Message, "red", null);
                    } else {                       
                        if (data.Message === 'Work experience added successfully.') {
                            jqueryConfirmGenerics.showOkAlertBox('Work Experience Added!', data.Message, "green", null);
                            window.location.reload();
                        }
                    }
                };

                // Function to be executed on failed AJAX request
                var functionFailed = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };

                var data = {
                    Internships: self.internships(),
                    PartTimeJobs: self.partTimeJobs(),
                    VolunteerWork: self.volunteerWork(),
                    RelevantProjects: self.relevantProjects()
                };

                // Make AJAX request
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Resume/Resume/WorkExperience", data, functionDone, functionFailed);
            };
            this.submitProjects = function () {
                debugger;
                var self = this;
                // Function to be executed on successful AJAX request
                var functionDone = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else if (data.IsOkay == false) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed', data.Message, "red", null);
                        self.clearForm();
                    } else {
                        if (data.IsSuccessful == true) {
                            jqueryConfirmGenerics.showOkAlertBox('Projects Added!', data.Message, "green", null);
                            window.location.reload();
                        }
                    }
                };

                // Function to be executed on failed AJAX request
                var functionFailed = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };

                var data = {
                    ProjectDescription: self.projectDescription(),
                    ProjectObjectives: self.projectObjectives(),
                    ProjectTechnologies: self.projectTechnologies(),
                    ProjectRoles: self.projectRoles()
                };

                // Make AJAX request
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Resume/Resume/Projects", data, functionDone, functionFailed);
            };
            this.submitExtracurricular = function () {
                debugger;
                var self = this;

                // Function to be executed on successful AJAX request
                var functionDone = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else if (data.IsOkay == false) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed', data.Message, "red", null);
                        self.clearForm();
                    } else {
                        if (data.Message === 'Extracurricular activities added successfully') {
                            jqueryConfirmGenerics.showOkAlertBox('Extracurricular Activities Added!', data.Message, "green", null);
                            window.location.reload();
                        } else {
                            jqueryConfirmGenerics.showOkAlertBox('Extracurricular Activities Not Added!', data.Message, "red", null);

                        }
                    }
                };

                // Function to be executed on failed AJAX request
                var functionFailed = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };

                var data = {
                    Clubs: self.clubs(),
                    LeadershipRoles: self.leadershipRoles(),
                    Awards: self.awards()
                };

                // Make AJAX request
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Resume/Resume/Extracurricular", data, functionDone, functionFailed);
            };
            this.submitCertificationsTraining = function () {
                debugger;
                var self = this;

                // Function to be executed on successful AJAX request
                var functionDone = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else if (data.IsOkay == false) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed', data.Message, "red", null);
                        self.clearForm();
                    } else {
                        if (data.Message === 'Certifications and training added successfully.') {
                            jqueryConfirmGenerics.showOkAlertBox('Certifications and Training Added!', data.Message, "green", null);
                            window.location.reload();
                        } else {
                            jqueryConfirmGenerics.showOkAlertBox('Certifications and Training Not Added!', data.Message, "red", null);
                        }
                    }
                };

                // Function to be executed on failed AJAX request
                var functionFailed = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };

                var data = {
                    RelevantCertifications: self.relevantCertifications(),
                    TrainingPrograms: self.trainingPrograms()
                };

                // Make AJAX request
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Resume/Resume/CertificationsTraining", data, functionDone, functionFailed);
            };
            this.submitCareerInterest = function () {
                debugger;
                var self = this;


                // Function to be executed on successful AJAX request
                var functionDone = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else if (data.IsOkay == false) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed', data.Message, "red", null);
                        self.clearForm();
                    } else {
                        // Adjust this part according to your response data
                        // Example: If the response indicates success
                        if (data.Message === 'Career interests added successfully.') {
                            jqueryConfirmGenerics.showOkAlertBox('Career Interests Added!', data.Message, "green", null);
                            window.location.reload();
                        } else {
                            jqueryConfirmGenerics.showOkAlertBox('Career Interests Not Added!', data.Message, "red", null);

                        }
                    }
                };

                // Function to be executed on failed AJAX request
                var functionFailed = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };

                var data = {
                    DesiredIndustry: self.desiredIndustry(),
                    PreferredRoles: self.preferredRoles(),
                    CareerGoals: self.careerGoals()
                    // Add more data properties as needed
                };

                // Make AJAX request
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Resume/Resume/CareerInterest", data, functionDone, functionFailed);
            };
            this.submitReferences = function () {
                debugger;
                var self = this;
                // Function to be executed on successful AJAX request
                var functionDone = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else if (data.IsOkay == false) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed', data.Message, "red", null);
                        self.clearForm();
                    } else {
                        // Adjust this part according to your response data
                        // Example: If the response indicates success
                        if (data.Message === 'References added successfully.') {
                            jqueryConfirmGenerics.showOkAlertBox('References Added!', data.Message, "green", null);
                            window.location.reload();
                        } else {
                            jqueryConfirmGenerics.showOkAlertBox('References Not Added!', data.Message, "red", null);
                            self.clearForm();
                        }
                    }
                };

                // Function to be executed on failed AJAX request
                var functionFailed = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };

                var data = {
                    Reference1: self.reference1(),
                    ContactDetails1: self.contact1(),
                    Reference2: self.reference2(),
                    ContactDetails2: self.contact2(),
                    Reference3: self.reference3(),
                    ContactDetails3: self.contact3()
                    // Add more data properties as needed
                };

                // Make AJAX request
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Resume/Resume/References", data, functionDone, functionFailed);
            };
            this.submitPortfolio = function () {
                debugger;
                var self = this;
                // Function to be executed on successful AJAX request
                var functionDone = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else if (data.IsOkay == false) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed', data.Message, "red", null);
                        self.clearForm();
                    } else {
                        // Adjust this part according to your response data
                        // Example: If the response indicates success
                        if (data.Message === 'Portfolio added successfully.') {
                            jqueryConfirmGenerics.showOkAlertBox('Portfolio Added!', data.Message, "green", null);
                            window.location.reload();
                        } else {
                            jqueryConfirmGenerics.showOkAlertBox('Portfolio Not Added!', data.Message, "red", null);
                        }
                    }
                };

                // Function to be executed on failed AJAX request
                var functionFailed = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };

                var data = {
                    PortfolioLink1: self.portfolioLink1(),
                    WorkSample1: self.workSample1(),
                    PortfolioLink2: self.portfolioLink2(),
                    WorkSample2: self.workSample2(),
                    PortfolioLink3: self.portfolioLink3(),
                    WorkSample3: self.workSample3()
                };

                // Make AJAX request
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Resume/Resume/Portfolio", data, functionDone, functionFailed);
            };
            this.submitAvailability = function () {
                debugger;
                var self = this;
                // Function to be executed on successful AJAX request
                var functionDone = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else if (data.IsOkay == false) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed', data.Message, "red", null);
                        self.clearForm();
                    } else {
                        if (data.Message === 'Availability added successfully.') {
                            jqueryConfirmGenerics.showOkAlertBox('Availability Added!', data.Message, "green", null);
                            window.location.reload();
                        } else {
                            jqueryConfirmGenerics.showOkAlertBox('Availability Not Added!', data.Message, "red", null);
                        }
                    }
                };

                // Function to be executed on failed AJAX request
                var functionFailed = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };
                debugger;
                var data = {
                    InternshipAvailability: self.internshipAvailability(),
                    PreferredStartDate: convertToUTC(self.preferredStartDate())
                };

                // Make AJAX request
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Resume/Resume/Availability", data, functionDone, functionFailed);
            };

            // Function to submit preferences
            self.submitPreferences = function () {
                var self = this;
                debugger;
                // Function to be executed on successful AJAX request
                var functionDone = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else if (data.IsOkay == false) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed', data.Message, "red", null);
                        self.clearForm();
                    } else {
                        if (data.Message === 'Preferences added successfully.') {
                            jqueryConfirmGenerics.showOkAlertBox('Preferences Added!', data.Message, "green", null);
                            window.location.reload();
                        } else {
                            jqueryConfirmGenerics.showOkAlertBox('Preferences Not Added!', data.Message, "red", null);
                        }
                    }
                };

                // Function to be executed on failed AJAX request
                var functionFailed = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };

                var data = {
                    LocationPreference: self.locationPreference(),
                    IndustryPreferences: self.industryPreferences(),
                    CompanyPreferences: self.companyPreferences()
                };

                // Make AJAX request
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Resume/Resume/Preferences", data, functionDone, functionFailed);
            };


            this.showErrorMessage = function (title, message) {
                var self = this;
                debugger;
                try {
                    this.hasError(true);
                    this.errorMessage(title + ': ' + message);


                }
                catch (err) {
                    jqueryConfirmGenerics.showOkAlertBox(title, err.message, "red", null);
                }
            }

            this.setUpGlobalVariables = function () {
                debugger;
                var self = this;
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        debugger;

                        //self.ParentName(data.Result.EntityName);

                        //self.PhotoLinkUrl(data.Result.ProfileImageUrl);



                        self.ParentPhone(data.Phone1);


                        self.ParentEmail(data.Email);

                    }
                    //data;///data contains the result

                };
                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }

                    //self.ParentPhone(data.Phone1);

                    self.ParentEmail(data.Email);

                    //jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };
                debugger;
                data = {

                };
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Accounts/AccountProfiles/GetParentDetails",
                    functionDone, functionFailed);

            }
            //this.setUpGlobalVariables();

            function readStringUntilSpace(inputString) {
                let result = '';
                for (let i = 0; i < inputString.length; i++) {
                    if (inputString[i] !== ' ') {
                        result += inputString[i];
                    } else {
                        break; // Exit the loop when a space is found
                    }
                }
                return result;
            }

            this.checkProfile = function () {
                debugger;
                var self = this;
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        debugger;
                        //self.Documents(data.Result);

                        self.Brand(readStringUntilSpace(data.Result.Result[0].EntityName));
                        self.PhotoLinkUrl(data.Result.Result[0].ProfileImageUrl);
                        self.Location(data.Result.Result[0].PhysicalAddress);


                    }
                    //data;///data contains the result

                };
                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }

                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };
                debugger;
                data = {

                };
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Accounts/AccountProfiles/GetKendoGridFiltered", data,
                    functionDone, functionFailed);

            }

            this.checkProfile();

            this.getAllCompanies = function () {
                debugger;
                var self = this;
                self.hasError(false);
                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        self.Companies(data.Result);
                    }
                };
                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Candidates/Candidates/GetAllCompanies",
                    functionDone, functionFailed);

            }.bind(this);
            this.getAllCompanies();

            this.getAllCandidates = function () {
                debugger;
                var self = this;
                self.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        debugger;
                        var CandidatesNo = data.Result.length
                        self.NoOfCandidates(CandidatesNo);
                        var candidates = []
                        data.Result.forEach(function (item){
                            var id = item.Id;
                            var name = item.EntityName;
                            var location = item.PhysicalAddress;
                            var profileImage = item.ProfileImageUrl;

                            var candidatesData = {
                                CandidateId: id,
                                CandidateName: name,
                                CandidateLocation: location,
                                CandidateProfileImage: profileImage,
                            }
                            candidates.push(candidatesData);
                        });
                        self.Candidates(candidates)

                       
                    }
                };
                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Candidates/Candidates/GetAllCandidates",
                    functionDone, functionFailed);

            }
            this.getAllCandidates();

            this.getAllApplications = function () {
                debugger;
                var self = this;
                self.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        debugger;
                        var ApplicationsNo = data.Result.length
                        self.NoOfApplications(ApplicationsNo);
                        var applications = []
                        data.Result.forEach(function (item) {
                            var id = item.Id;
                            var name = item.AccountEntity.EntityName;
                            var profileImage = item.AccountEntity.ProfileImageUrl;
                            var projectName = item.Project.ProjectName;
                            var applicationStatus = item.ProjectApplicationStatus.ApplicationStatus;
                            var candidateLocation = item.AccountEntity.PhysicalAddress;

                            var applicationsData = {
                                ApplicationsId: id,
                                ApplicationCandidate: name,
                                ApplicationCandidateProfile: profileImage,
                                ApplicationStatus: applicationStatus,
                                ApplicationProjectName: projectName,
                                ApplicationCandidateLocation: candidateLocation
                            }
                            applications.push(applicationsData);
                        });
                        self.Applications(applications)


                    }
                };
                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Candidates/Candidates/GetApplications",
                    functionDone, functionFailed);

            }.bind(this);
            this.getAllApplications();

            this.matchCandidate = function () {

                debugger;
                var self = this;
                var functionDone = function (hasError, message, data) {
                    debugger;
                    var alertArea = $('#alertArea');
                    if (hasError) {
                        alertArea.addClass('alert-danger').text('Failed! ' + Message).show();
                    } else if (data.IsOkay == false) {
                        alertArea.addClass('alert-danger').text('Failed! ' + data.Message).show();
                        self.clearForm();
                    }
                    else {
                        if (data.Message === 'Candidate Matched Successfully.') {
                            alertArea.addClass('alert-success').text('Candidate has been Matched Successfully').show();                                                       
                            //window.location.reload();
                        } else {
                            alertArea.addClass('alert-danger').text('Candidate has already been Matched').show();
                            /*self.clearForm();*/
                        }                       

                    }

                };
                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }

                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };
                debugger;

                debugger;
                var data = {
                    AccountEntity: self.selectedCompany(),
                    CandidateId: self.EntityId()
                }
                
                debugger;

                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Candidates/Candidates/MatchCandidate", data,
                    functionDone, functionFailed);

            }.bind(this);

            //this.getMatch = (id) => {
            //    // Call getCandidateDetails function passing the id
            //    this.matchCandidate(id);
            //};
            // Assuming this is the function triggered when you click "viewProfile"
            //this.viewProfile = (data, index) => {
          

            this.getUserDetails = function () {
                debugger;
                var self = this;
                self.hasError(false);
                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        
                        if (data.Result) {
                            data.Result.forEach(function (item) {
                                debugger;
                                var keyName = item.TypeAdditionalDetails.KeyName;
                                var value = item.Value;                                

                                debugger;
                                switch (keyName) {
                                    case "Current School/University":
                                        self.school(value);
                                        /*localStorage.setItem('school', value);*/
                                        break;
                                    case "Degree Program/Diploma/Certificate":
                                        self.degree(value);
                                        /*localStorage.setItem('degree', value);*/
                                        break;
                                    case "Year of Study":
                                        self.year(value);
                                        /*localStorage.setItem('year', value);*/
                                        break;
                                    case "Expected Internship Period":
                                        self.expectedInternshipPeriod(value);
                                        /*localStorage.setItem('expectedInternshipPeriod', value);*/
                                        break;
                                    case "Expected Graduation Date":
                                        self.expectedGraduationDate(value);
                                        /*localStorage.setItem('expectedGraduationDate', value);*/
                                        break;
                                    case "Internships Work Experience":
                                        self.internships(value);
                                        /*localStorage.setItem('internships', value);*/
                                        break;
                                    case "Part-time Jobs Work Experience":
                                        self.partTimeJobs(value);
                                        /*localStorage.setItem('partTimeJobs', value);*/
                                        break;
                                    case "Volunteer Work Work Experience":
                                        self.volunteerWork(value);
                                        /*localStorage.setItem('volunteerWork', value);*/
                                        break;
                                    case "Relevant Projects or Research Experience":
                                        self.relevantProjects(value);
                                        /*localStorage.setItem('relevantProjects', value);*/
                                        break;
                                    case "Description of Projects Completed":
                                        self.projectDescription(value);
                                        /*localStorage.setItem('projectDescription', value);*/
                                        break;
                                    case "Objectives and Outcomes Achieved":
                                        self.projectObjectives(value);
                                        /*localStorage.setItem('projectObjectives', value);*/
                                        break;
                                    case "Technologies or Tools Used":
                                        self.projectTechnologies(value);
                                        /*localStorage.setItem('projectTechnologies', value);*/
                                        break;
                                    case "Roles and Responsibilities":
                                        self.projectRoles(value);
                                        /*localStorage.setItem('projectRoles', value);*/
                                        break;
                                    case "Clubs or Organizations":
                                        self.clubs(value);
                                        /*localStorage.setItem('clubs', value);*/
                                        break;
                                    case "Leadership Roles":
                                        self.leadershipRoles(value);
                                        /*localStorage.setItem('leadershipRoles', value);*/
                                        break;
                                    case "Awards or Achievements":
                                        self.awards(value);
                                        /*localStorage.setItem('awards', value);*/
                                        break;
                                    case "Career Goals and Aspirations":
                                        self.careerGoals(value);
                                        /*localStorage.setItem('careerGoals', value);*/
                                        break;
                                    case "Desired Industry or Field":
                                        self.desiredIndustry(value);
                                        /*localStorage.setItem('desiredIndustry', value);*/
                                        break;
                                    case "Preferred Job Functions or Roles":
                                        self.preferredRoles(value);
                                        /*localStorage.setItem('preferredRoles', value);*/
                                        break;
                                    case "Reference 1":
                                        self.reference1(value);
                                        /*localStorage.setItem('reference1', value);*/
                                        break;
                                    case "Contact Details for Reference 1":
                                        self.contact1(value);
                                        /*localStorage.setItem('contact1', value);*/
                                        break;
                                    case "Reference 2":
                                        self.reference2(value);
                                        /*localStorage.setItem('reference2', value);*/
                                        break;
                                    case "Contact Details for Reference 2":
                                        self.contact2(value);
                                        /*localStorage.setItem('contact2', value);*/
                                        break;
                                    case "Reference 3":
                                        self.reference3(value);
                                        /*localStorage.setItem('reference3', value);*/
                                        break;
                                    case "Contact Details for Reference 3":
                                        self.contact3(value);
                                        /*localStorage.setItem('contact3', value);*/
                                        break;
                                    case "Portfolio Link 1":
                                        self.portfolioLink1(value);
                                        /*localStorage.setItem('portfolioLink1', value);*/
                                        break;
                                    case "Work Sample 1":
                                        self.workSample1(value);
                                        /*localStorage.setItem('workSample1', value);*/
                                        break;
                                    case "Portfolio Link 2":
                                        self.portfolioLink2(value);
                                        /*localStorage.setItem('portfolioLink2', value);*/
                                        break;
                                    case "Work Sample 2":
                                        self.workSample2(value);
                                        /*localStorage.setItem('workSample2', value);*/
                                        break;
                                    case "Portfolio Link 3":
                                        self.portfolioLink3(value);
                                        /*localStorage.setItem('portfolioLink3', value);*/
                                        break;
                                    case "Work Sample 3":
                                        self.workSample3(value);
                                        /*localStorage.setItem('workSample3', value);*/
                                        break;
                                    case "Technical Skills":
                                        self.technicalSkills(value);
                                        /*localStorage.setItem('technicalSkills', value);*/
                                        break;
                                    case "Soft Skills":
                                        self.softSkills(value);
                                        /*localStorage.setItem('softSkills', value);*/
                                        break;
                                    case "Language Proficiency":
                                        self.languageProficiency(value);
                                        /*localStorage.setItem('languageProficiency', value);*/
                                        break;
                                    case "Relevant Certifications":
                                        self.relevantCertifications(value);
                                        /*localStorage.setItem('relevantCertifications', value);*/
                                        break;
                                    case "Training Programs Attended":
                                        self.trainingPrograms(value);
                                        /*localStorage.setItem('trainingPrograms', value);*/
                                        break;
                                    case "Availability for Internships":
                                        self.internshipAvailability(value);
                                        /*localStorage.setItem('internshipAvailability', value)*/;
                                        break;
                                    case "Preferred Start Date":
                                        self.preferredStartDate(value);
                                        /*localStorage.setItem('preferredStartDate', value)*/;
                                        break;
                                    case "Location Preference for Internships":
                                        self.locationPreference(value);
                                        /*localStorage.setItem('locationPreference', value);*/
                                        break;
                                    case "Industry Preferences":
                                        self.industryPreferences(value);
                                        /*localStorage.setItem('industryPreferences', value);*/
                                        break;
                                    case "Company Size or Type Preferences":
                                        self.companyPreferences(value);
                                        /*localStorage.setItem('companyPreferences', value);*/
                                        break;
                                    default:
                                        // Handle default case if needed
                                        break;
                                }


                            });
                            debugger;
                            //// Navigation to the CandidatesDetails page
                            //window.location.href = "/Admin/CandidatesDetails";
                        } else {
                            var alertArea = $('#alertArea');
                            alertArea.addClass('alert-danger').text('Failed' + ':' + data.Message).show();
                        }
                    }
                };
                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };

                /*var data = id;*/
                debugger;
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Candidates/Candidates/GetUserDetails",
                    functionDone, functionFailed);

            }

            this.getUserDetails();

            this.viewProfile = (data, index) => {
                var id = data.CandidateId;
                // Call getCandidateDetails function passing the id
                this.getCandidateDetails(id, index);
            };

            this.viewApplication = (data) => {
                var id = data.ApplicationsId;
                // Call getCandidateDetails function passing the id
                this.getApplicationDetails(id);
            };

            this.getApplicationDetails = function (id) {
                debugger;
                var self = this;
                self.hasError(false);
                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        if (data.Result) {
                            data.Result.forEach(function (item) {
                                debugger;
                                var candidateName = item.AccountEntity.EntityName;
                                var candidatePhone = item.AccountEntity.Phone1;
                                var candidateEmail = item.AccountEntity.Email;
                                var candidateProfileImage = item.AccountEntity.ProfileImageUrl;
                                var candidateLocation = item.AccountEntity.PhysicalAddress;
                                var projectRecommendation = item.Project.ProjectRecommendation;
                                var workScope = item.Project.WorkScope;
                                var projectDetails = item.Project.ProjectDetails;
                                var projectCategory = item.Project.ProjectCategory;
                                var projectName = item.Project.ProjectName;
                                var projectStart = new Date(item.Project.ProjectStart).toLocaleDateString();
                                var projectEnd = new Date(item.Project.ProjectEnd).toLocaleDateString();
                                var projectLocation = item.Project.ProjectLocation;
                                var applicationId = item.Id;
                                var projectId = item.Project.Id;

                                self.ApplicationCandidate(candidateName);
                                self.ApplicationCandidateEmail(candidateEmail);
                                self.ApplicationCandidateLocation(candidateLocation);
                                self.ApplicationCandidatePhone(candidatePhone);
                                self.ApplicationCandidateProfile(candidateProfileImage);
                                self.ApplicationProjectName(projectName);
                                self.ApplicationProjectDetails(projectDetails);
                                self.ApplicationProjectCategory(projectCategory);
                                self.ApplicationProjectRecommendation(projectRecommendation);
                               /* self.ApplicationProjectWorkScope(workScope);*/
                                self.ApplicationProjectStart(projectStart);
                                self.ApplicationProjectEnd(projectEnd);
                                self.ApplicationProjectLocation(projectLocation);
                                self.ApplicationId(applicationId);
                                self.ProjectId(projectId);

                                var formattedWork = workScope.replace(/\n/g, "<br>");
                                self.ApplicationProjectWorkScope(formattedWork);

                                // Store items in local storage
                                localStorage.setItem('ApplicationCandidate', self.ApplicationCandidate());
                                localStorage.setItem('ApplicationCandidateEmail', self.ApplicationCandidateEmail());
                                localStorage.setItem('ApplicationCandidateLocation', self.ApplicationCandidateLocation());
                                localStorage.setItem('ApplicationCandidatePhone', self.ApplicationCandidatePhone());
                                localStorage.setItem('ApplicationCandidateProfile', self.ApplicationCandidateProfile());
                                localStorage.setItem('ApplicationProjectName', self.ApplicationProjectName());
                                localStorage.setItem('ApplicationProjectDetails', self.ApplicationProjectDetails());
                                localStorage.setItem('ApplicationProjectRecommendation', self.ApplicationProjectRecommendation());
                                localStorage.setItem('ApplicationProjectStart', self.ApplicationProjectStart());
                                localStorage.setItem('ApplicationProjectWorkScope', self.ApplicationProjectWorkScope());
                                localStorage.setItem('ApplicationProjectEnd', self.ApplicationProjectEnd());
                                localStorage.setItem('ApplicationProjectLocation', self.ApplicationProjectLocation());
                                localStorage.setItem('ApplicationProjectCategory', self.ApplicationProjectCategory());
                                localStorage.setItem('ApplicationId', self.ApplicationId());
                                localStorage.setItem('ProjectId', self.ProjectId());



                            });
                            debugger;
                            // Navigation to the CandidatesDetails page
                            window.location.href = "/Admin/ApplicationDetails";
                        } else {
                            debugger;                           
                                debugger;
                                var alertArea = $('#alertArea');
                                alertArea.addClass('alert-danger').text('Failed' + ':' + data.Message).show();                            
                        }
                    }
                };
                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };

                var data = id;
                debugger;
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Candidates/Candidates/GetApplicationDetails?id=" + data,
                    functionDone, functionFailed);

            }



            this.getCandidateDetails = function (id, index) {
                debugger;
                var self = this;
                self.hasError(false);
                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        if (data.Result) {
                            data.Result.forEach(function (item) {
                                debugger;
                                var keyName = item.TypeAdditionalDetails.KeyName;
                                var value = item.Value;
                                var name = item.AccountEntity.EntityName;
                                var email = item.AccountEntity.Email;
                                var phone = item.AccountEntity.Phone1;
                                var profileImage = item.AccountEntity.ProfileImageUrl;
                                var location = item.AccountEntity.PhysicalAddress;
                                var dob = item.AccountEntity.DateOfBirth;
                                var genderName = item.AccountEntity.Gender.GenderName;
                                var id = item.AccountEntity.Id;

                                self.EntityName(name);
                                self.EntityEmail(email);
                                self.EntityPhone(phone);
                                self.EntityProfileImage(profileImage);
                                self.Location(location);
                                self.birthDate(dob);
                                self.gender(genderName);
                                self.EntityId(id);

                                // Store items in local storage
                                localStorage.setItem('EntityName', self.EntityName());
                                localStorage.setItem('EntityEmail', self.EntityEmail());
                                localStorage.setItem('EntityPhone', self.EntityPhone());
                                localStorage.setItem('EntityProfileImage', self.EntityProfileImage());
                                localStorage.setItem('Location', self.Location());
                                localStorage.setItem('birthDate', self.birthDate());
                                localStorage.setItem('gender', self.gender());
                                localStorage.setItem('EntityId', self.EntityId());

                                debugger;
                                switch (keyName) {
                                    case "Current School/University":
                                        self.school(value);
                                        localStorage.setItem('school', value);
                                        break;
                                    case "Degree Program/Diploma/Certificate":
                                        self.degree(value);
                                        localStorage.setItem('degree', value);
                                        break;
                                    case "Year of Study":
                                        self.year(value);
                                        localStorage.setItem('year', value);
                                        break;
                                    case "Expected Internship Period":
                                        self.expectedInternshipPeriod(value);
                                        localStorage.setItem('expectedInternshipPeriod', value);
                                        break;
                                    case "Expected Graduation Date":
                                        self.expectedGraduationDate(value);
                                        localStorage.setItem('expectedGraduationDate', value);
                                        break;
                                    case "Internships Work Experience":
                                        self.internships(value);
                                        localStorage.setItem('internships', value);
                                        break;
                                    case "Part-time Jobs Work Experience":
                                        self.partTimeJobs(value);
                                        localStorage.setItem('partTimeJobs', value);
                                        break;
                                    case "Volunteer Work Work Experience":
                                        self.volunteerWork(value);
                                        localStorage.setItem('volunteerWork', value);
                                        break;
                                    case "Relevant Projects or Research Experience":
                                        self.relevantProjects(value);
                                        localStorage.setItem('relevantProjects', value);
                                        break;
                                    case "Description of Projects Completed":
                                        self.projectDescription(value);
                                        localStorage.setItem('projectDescription', value);
                                        break;
                                    case "Objectives and Outcomes Achieved":
                                        self.projectObjectives(value);
                                        localStorage.setItem('projectObjectives', value);
                                        break;
                                    case "Technologies or Tools Used":
                                        self.projectTechnologies(value);
                                        localStorage.setItem('projectTechnologies', value);
                                        break;
                                    case "Roles and Responsibilities":
                                        self.projectRoles(value);
                                        localStorage.setItem('projectRoles', value);
                                        break;
                                    case "Clubs or Organizations":
                                        self.clubs(value);
                                        localStorage.setItem('clubs', value);
                                        break;
                                    case "Leadership Roles":
                                        self.leadershipRoles(value);
                                        localStorage.setItem('leadershipRoles', value);
                                        break;
                                    case "Awards or Achievements":
                                        self.awards(value);
                                        localStorage.setItem('awards', value);
                                        break;
                                    case "Career Goals and Aspirations":
                                        self.careerGoals(value);
                                        localStorage.setItem('careerGoals', value);
                                        break;
                                    case "Desired Industry or Field":
                                        self.desiredIndustry(value);
                                        localStorage.setItem('desiredIndustry', value);
                                        break;
                                    case "Preferred Job Functions or Roles":
                                        self.preferredRoles(value);
                                        localStorage.setItem('preferredRoles', value);
                                        break;
                                    case "Reference 1":
                                        self.reference1(value);
                                        localStorage.setItem('reference1', value);
                                        break;
                                    case "Contact Details for Reference 1":
                                        self.contact1(value);
                                        localStorage.setItem('contact1', value);
                                        break;
                                    case "Reference 2":
                                        self.reference2(value);
                                        localStorage.setItem('reference2', value);
                                        break;
                                    case "Contact Details for Reference 2":
                                        self.contact2(value);
                                        localStorage.setItem('contact2', value);
                                        break;
                                    case "Reference 3":
                                        self.reference3(value);
                                        localStorage.setItem('reference3', value);
                                        break;
                                    case "Contact Details for Reference 3":
                                        self.contact3(value);
                                        localStorage.setItem('contact3', value);
                                        break;
                                    case "Portfolio Link 1":
                                        self.portfolioLink1(value);
                                        localStorage.setItem('portfolioLink1', value);
                                        break;
                                    case "Work Sample 1":
                                        self.workSample1(value);
                                        localStorage.setItem('workSample1', value);
                                        break;
                                    case "Portfolio Link 2":
                                        self.portfolioLink2(value);
                                        localStorage.setItem('portfolioLink2', value);
                                        break;
                                    case "Work Sample 2":
                                        self.workSample2(value);
                                        localStorage.setItem('workSample2', value);
                                        break;
                                    case "Portfolio Link 3":
                                        self.portfolioLink3(value);
                                        localStorage.setItem('portfolioLink3', value);
                                        break;
                                    case "Work Sample 3":
                                        self.workSample3(value);
                                        localStorage.setItem('workSample3', value);
                                        break;
                                    case "Technical Skills":
                                        self.technicalSkills(value);
                                        localStorage.setItem('technicalSkills', value);
                                        break;
                                    case "Soft Skills":
                                        self.softSkills(value);
                                        localStorage.setItem('softSkills', value);
                                        break;
                                    case "Language Proficiency":
                                        self.languageProficiency(value);
                                        localStorage.setItem('languageProficiency', value);
                                        break;
                                    case "Relevant Certifications":
                                        self.relevantCertifications(value);
                                        localStorage.setItem('relevantCertifications', value);
                                        break;
                                    case "Training Programs Attended":
                                        self.trainingPrograms(value);
                                        localStorage.setItem('trainingPrograms', value);
                                        break;
                                    case "Availability for Internships":
                                        self.internshipAvailability(value);
                                        localStorage.setItem('internshipAvailability', value);
                                        break;
                                    case "Preferred Start Date":
                                        self.preferredStartDate(value);
                                        localStorage.setItem('preferredStartDate', value);
                                        break;
                                    case "Location Preference for Internships":
                                        self.locationPreference(value);
                                        localStorage.setItem('locationPreference', value);
                                        break;
                                    case "Industry Preferences":
                                        self.industryPreferences(value);
                                        localStorage.setItem('industryPreferences', value);
                                        break;
                                    case "Company Size or Type Preferences":
                                        self.companyPreferences(value);
                                        localStorage.setItem('companyPreferences', value);
                                        break;
                                    default:
                                        // Handle default case if needed
                                        break;
                                }


                            });
                            debugger;
                            // Navigation to the CandidatesDetails page
                            window.location.href = "/Admin/CandidatesDetails";
                        } else {
                            debugger;
                            if (index !== null) {
                                debugger;
                                var alertArea = $('#alertArea' + index.toString());
                                alertArea.addClass('alert-danger').text('Failed' + ':' + data.Message).show();
                            }                           
                        }
                    }
                };
                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };

                var data = id;
                debugger;
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Candidates/Candidates/GetCandidatesDetails?id=" + data,
                    functionDone, functionFailed);

            }

            // Function to retrieve items from local storage and apply them to observables
            this.retrieveDataFromLocalStorage = function () {
                var self = this;
                // Iterate over each keyName and retrieve the corresponding value from local storage
                if (window.location.pathname === "/Admin/CandidatesDetails") {
                    Object.keys(localStorage).forEach(function (keyName) {
                        var value = localStorage.getItem(keyName);
                        // Apply the value to the respective observable based on the keyName
                        switch (keyName) {
                            case "school":
                                self.school(value);
                                break;
                            case "degree":
                                self.degree(value);
                                break;
                            case "year":
                                self.year(value);
                                break;
                            case "expectedInternshipPeriod":
                                self.expectedInternshipPeriod(value);
                                break;
                            case "expectedGraduationDate":
                                self.expectedGraduationDate(value);
                                break;
                            case "internships":
                                self.internships(value);
                                break;
                            case "partTimeJobs":
                                self.partTimeJobs(value);
                                break;
                            case "volunteerWork":
                                self.volunteerWork(value);
                                break;
                            case "relevantProjects":
                                self.relevantProjects(value);
                                break;
                            case "projectDescription":
                                self.projectDescription(value);
                                break;
                            case "projectObjectives":
                                self.projectObjectives(value);
                                break;
                            case "projectTechnologies":
                                self.projectTechnologies(value);
                                break;
                            case "projectRoles":
                                self.projectRoles(value);
                                break;
                            case "clubs":
                                self.clubs(value);
                                break;
                            case "leadershipRoles":
                                self.leadershipRoles(value);
                                break;
                            case "awards":
                                self.awards(value);
                                break;
                            case "careerGoals":
                                self.careerGoals(value);
                                break;
                            case "desiredIndustry":
                                self.desiredIndustry(value);
                                break;
                            case "preferredRoles":
                                self.preferredRoles(value);
                                break;
                            case "reference1":
                                self.reference1(value);
                                break;
                            case "contact1":
                                self.contact1(value);
                                break;
                            case "reference2":
                                self.reference2(value);
                                break;
                            case "contact2":
                                self.contact2(value);
                                break;
                            case "reference3":
                                self.reference3(value);
                                break;
                            case "contact3":
                                self.contact3(value);
                                break;
                            case "portfolioLink1":
                                self.portfolioLink1(value);
                                break;
                            case "workSample1":
                                self.workSample1(value);
                                break;
                            case "portfolioLink2":
                                self.portfolioLink2(value);
                                break;
                            case "workSample2":
                                self.workSample2(value);
                                break;
                            case "portfolioLink3":
                                self.portfolioLink3(value);
                                break;
                            case "workSample3":
                                self.workSample3(value);
                                break;
                            case "technicalSkills":
                                self.technicalSkills(value);
                                break;
                            case "softSkills":
                                self.softSkills(value);
                                break;
                            case "languageProficiency":
                                self.languageProficiency(value);
                                break;
                            case "relevantCertifications":
                                self.relevantCertifications(value);
                                break;
                            case "trainingPrograms":
                                self.trainingPrograms(value);
                                break;
                            case "internshipAvailability":
                                self.internshipAvailability(value);
                                break;
                            case "preferredStartDate":
                                self.preferredStartDate(value);
                                break;
                            case "locationPreference":
                                self.locationPreference(value);
                                break;
                            case "industryPreferences":
                                self.industryPreferences(value);
                                break;
                            case "companyPreferences":
                                self.companyPreferences(value);
                                break;
                            default:
                                // Handle default case if needed
                                break;
                        }
                    });

                    // Retrieve observables not stored under switch statement
                    self.EntityName(localStorage.getItem('EntityName'));
                    self.EntityEmail(localStorage.getItem('EntityEmail'));
                    self.EntityPhone(localStorage.getItem('EntityPhone'));
                    self.EntityProfileImage(localStorage.getItem('EntityProfileImage'));
                    self.Location(localStorage.getItem('Location'));
                    self.birthDate(localStorage.getItem('birthDate'));
                    self.gender(localStorage.getItem('gender'));
                    self.EntityId(localStorage.getItem('EntityId'));
                }

            }.bind(this);

            this.retrieveDataFromLocalStorage();

            this.retrieveApplicationDetailsDataFromLocalStorage = function () {
                var self = this;
                // Iterate over each keyName and retrieve the corresponding value from local storage
                if (window.location.pathname === "/Admin/ApplicationDetails") {
                  
                    // Retrieve observables not stored under switch statement
                    self.ApplicationCandidate(localStorage.getItem('ApplicationCandidate'));
                    self.ApplicationCandidateEmail(localStorage.getItem('ApplicationCandidateEmail'));
                    self.ApplicationCandidateLocation(localStorage.getItem('ApplicationCandidateLocation'));
                    self.ApplicationCandidatePhone(localStorage.getItem('ApplicationCandidatePhone'));
                    self.ApplicationCandidateProfile(localStorage.getItem('ApplicationCandidateProfile'));
                    self.ApplicationProjectName(localStorage.getItem('ApplicationProjectName'));
                    self.ApplicationProjectDetails(localStorage.getItem('ApplicationProjectDetails'));
                    self.ApplicationProjectCategory(localStorage.getItem('ApplicationProjectCategory'));
                    self.ApplicationProjectRecommendation(localStorage.getItem('ApplicationProjectRecommendation'));
                    self.ApplicationProjectWorkScope(localStorage.getItem('ApplicationProjectWorkScope'));
                    self.ApplicationProjectStart(localStorage.getItem('ApplicationProjectStart'));
                    self.ApplicationProjectEnd(localStorage.getItem('ApplicationProjectEnd'));
                    self.ApplicationProjectLocation(localStorage.getItem('ApplicationProjectLocation'));
                    self.ApplicationId(localStorage.getItem('ApplicationId'));
                    self.ProjectId(localStorage.getItem('ProjectId'));



                }

            }.bind(this);

            this.retrieveApplicationDetailsDataFromLocalStorage();

            this.acceptApplication = function () {
                debugger;
                var self = this;               
                // Function to be executed on successful AJAX request
                var functionDone = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else if (data.IsOkay == false) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed', data.Message, "red", null);
                        //self.clearForm();
                    } else {
                        if (data.IsSuccessful == true) {
                            // Set the alert message
                            $('#alertModalBody').text(data.Message);

                            // Show the modal
                            $('#alertModal').modal('show');
                        } 
                    }
                };

                // Function to be executed on failed AJAX request
                var functionFailed = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };
                debugger;
                var data = {
                    ProjectId: localStorage.getItem('ProjectId'),
                    ApplicationId: localStorage.getItem('ApplicationId'),
                    CandidateName: localStorage.getItem('ApplicationCandidate'),
                    CandidateEmail: localStorage.getItem('ApplicationCandidateEmail'),
                    ProjectName: localStorage.getItem('ApplicationProjectName')
                };

                // Make AJAX request
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("PUT", "/Candidates/Candidates/AcceptApplication", data, functionDone, functionFailed);
            };

            this.rejectApplication = function () {
                debugger;
                var self = this;
                // Function to be executed on successful AJAX request
                var functionDone = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else if (data.IsOkay == false) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed', data.Message, "red", null);
                        //self.clearForm();
                    } else {
                        if (data.IsSuccessful == true) {
                            // Set the alert message
                            $('#alertModalBody').text(data.Message);

                            // Show the modal
                            $('#alertModal').modal('show');
                        }
                    }
                };

                // Function to be executed on failed AJAX request
                var functionFailed = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };
                debugger;
                var data = {
                    ProjectId: localStorage.getItem('ProjectId'),
                    ApplicationId: localStorage.getItem('ApplicationId'),
                    CandidateName: localStorage.getItem('ApplicationCandidate'),
                    CandidateEmail: localStorage.getItem('ApplicationCandidateEmail'),
                    ProjectName: localStorage.getItem('ApplicationProjectName')
                };

                // Make AJAX request
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("PUT", "/Candidates/Candidates/RejectApplication", data, functionDone, functionFailed);
            };

            this.validate = function () {
                debugger;
                var self = this;
                if (!self.Agents()) {
                    jqueryConfirmGenerics.showOkAlertBox('Validation Failed!', "Agent is required", "red", null);
                    return false;
                }


                return true;
            }

            this.confirmDetails = function () {
                window.location.reload();
            };



            this.OnImport = function () {
                debugger;

                var self = this;
                MVVM.blockUiPageElementWithMessage($('body'), 'Uploading your File .... Please wait');
                var formData = new FormData();
                var totalFiles = document.getElementById(self.SelectedDocument()).files.length;
                for (var i = 0; i < totalFiles; i++) {
                    var file = document.getElementById(self.SelectedDocument()).files[i];
                    formData.append(self.SelectedDocument(), file, self.SelectedDocument());
                }

                if (file && file.type === 'application/pdf') {

                    $.ajax({
                        type: 'POST',
                        url: '/DashBoard/Dashboard/UploadAccounts',
                        dataType: 'json',
                        processData: false,
                        contentType: false,
                        data: formData,
                        success: function (data) {

                            jqueryConfirmGenerics.showOkAlertBox('Message', data.Message, "green", null);
                            window.location.reload();
                        },
                        error: function (err) {
                            jqueryConfirmGenerics.showOkAlertBox('Failed', err.Message, "red", null);

                        }

                    });

                } else {
                    jqueryConfirmGenerics.showOkAlertBox('Failed', "Please upload a PDF file.", "red", null);
                    return;
                }

            }

            this.checkRequiredMenu = function () {
                debugger;
                var self = this;

                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        debugger;
                        if (data.Result.TypeName == "Customer") {
                            self.CustomerMenueVisible(true);
                        }
                        if (data.Result.TypeName == "Agent") {
                            self.AgentMenueVisible(true);
                        }
                        if (data.Result.TypeName == "Admin") {
                            self.AdminMenueVisible(true);
                        }
                    }
                };
                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }

                };
                debugger;

                jqueryAjaxGenerics.createJSONAjaxGETRequest("/DashBoard/Dashboard/GetUserType",

                    functionDone, functionFailed);

            }
            this.checkRequiredMenu();

            this.truncateMessage = function (message, maxLength) {
                if (message.length > maxLength) {
                    return message.substring(0, maxLength) + "...";
                }
                return message;
            }

            this.showFullMessage = function () {
                var self = this;
                self.showFullMessageFlag(!self.showFullMessageFlag());
            }.bind(this);

            this.GetMessage = function () {
                debugger;
                var self = this;
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, 'red', null);
                    } else {
                        var notification = [];

                        data.Result.forEach(function (item) {
                            debugger;
                            var notificationId = item.Id;
                            var isRead = item.BatchJobId;
                            var senderName = item.Entity.EntityName;
                            var senderPhone = item.Entity.Phone1;
                            var receiverPhone = item.PhoneNumber;
                            var SenderId = item.SenderId;
                            var Message = item.Message;
                            debugger;
                            var SentTime = moment(item.SentTime).format('MMM D, YYYY, h:mm A') + ' (' + moment(item.SentTime).fromNow() + ')';
                            var Email = item.Entity.Email;


                            var notificationData = {
                                Id: notificationId,
                                BatchJobId: isRead,
                                EntityName: senderName,
                                Phone1: senderPhone,
                                SenderId: SenderId,
                                PhoneNumber: receiverPhone,
                                Message: Message,
                                SentTime: SentTime,
                                Email: Email,
                                IsExpanded: ko.observable(false), // Add observable for expanded state
                                IsRead: ko.observable(false) // Add observable for read state
                            };
                            debugger;
                            notification.push(notificationData);
                        });

                        self.UserNotifications(notification);

                        // Update the newNotificationCount based on the fetched notifications
                        self.newNotificationCount(self.UserNotifications().length);
                    }
                };

                var functionFailed = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, 'red', null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, 'red', null);
                };

                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Hubs/Notification/GetNotifications",
                    functionDone, functionFailed);
            }
            this.toggleMessage = function (notification) {

                debugger;
                var self = this;
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, 'red', null);
                    } else {
                        debugger;
                        if (!notification.IsRead() && !notification.IsExpanded()) {
                            // Mark notification as read and expanded
                            notification.IsRead(true);
                            notification.IsExpanded(true);

                            // Calculate the new notification count based on remaining unread messages
                            var remainingUnreadCount = ko.utils.arrayFilter(self.UserNotifications(), function (item) {
                                return !item.IsRead();
                            }).length;
                            self.newNotificationCount(remainingUnreadCount);

                        } else if (!notification.IsExpanded()) {
                            // Mark notification as expanded without affecting the count
                            notification.IsExpanded(true);
                        } else {
                            // Collapse the expanded message
                            notification.IsExpanded(false);
                        }

                    }
                }

                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, 'red', null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, 'red', null);
                };
                var data = {

                    Id: notification.Id,
                    BatchJobId: notification.BatchJobId,
                    PhoneNumber: notification.PhoneNumber,
                    SenderId: notification.SenderId,
                    Message: notification.Message,
                };
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Hubs/Notification/UpdateNotifications", data,
                    functionDone, functionFailed);
            }.bind(this);

            /*this.GetMessage();*/

            this.selectNotification = function (notification) {
                debugger;
                var self = this;
                self.selectedNotification(notification);
                debugger;
            };

            this.SendMessageNotification = function () {//for replying the message
                debugger;
                $('#ReplyMessage').modal('hide');
                MVVM.blockUiPageElementWithMessage($('body'), 'Sending message.. Please wait');
                var self = this;
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        jqueryConfirmGenerics.showOkAlertBox('Success!', data.Message, "green", null);
                        /*$('.datatables-basic').DataTable().ajax.reload();*/
                        MVVM.unBlockUiPageElement($('body'));

                    }
                };
                var functionFailed = function (hasError, message, data) {
                    debugger;
                    MVVM.unBlockUiPageElement($('body'));
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);

                };
                debugger;
                var selectedNotification = self.selectedNotification();
                var senderEmail = selectedNotification.Email;
                var senderPhone = selectedNotification.Phone1;
                var data = {
                    SenderNo: null,
                    ReceiverEmail: senderEmail,
                    ReceiverNo: senderPhone,
                    Title: self.emailSubject(),
                    Message: self.emailMessage()
                };
                debugger;
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Hubs/Notification/Send", data,
                    functionDone, functionFailed);

            }
            this.SendNewMessage = function () {
                debugger;
                $('#NewMessage').modal('hide');
                MVVM.blockUiPageElementWithMessage($('body'), 'Sending message.. Please wait');
                var self = this;
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        jqueryConfirmGenerics.showOkAlertBox('Success!', data.Message, "green", null);
                        /*$('.datatables-basic').DataTable().ajax.reload();*/

                        MVVM.unBlockUiPageElement($('body'));
                    }
                };
                var functionFailed = function (hasError, message, data) {
                    debugger;
                    MVVM.unBlockUiPageElement($('body'));
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);

                };
                debugger;
                var data = {
                    SenderNo: null,
                    ReceiverEmail: self.EmailMail(),
                    ReceiverNo: self.EmailPhoneNumber(),
                    Title: self.emailSubject(),
                    Message: self.emailMessage()
                };
                debugger;
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Hubs/Notification/Send", data,
                    functionDone, functionFailed);

            }
            this.displayTopupMessage = function () {
                var self = this;
                jqueryConfirmGenerics.showOkAlertBox("Information", "As we work on automating account topup, kindly contact " + self.ParentEmail() + " or " + self.ParentPhone() + " to load your wallet", "green", null);
            }



        }
        var myModel = new viewModel();
        ko.applyBindings(myModel);



    }
};

$(document).ready(function () {
    debugger;
    MVVM.init();
    //MVVM.initCharts();

});



