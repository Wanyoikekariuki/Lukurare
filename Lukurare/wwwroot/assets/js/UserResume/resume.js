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

            this.hasError = ko.observable(false);
            this.errorMessage = ko.observable();
            this.Balance = ko.observable(0);
            this.PhotoLinkUrl = ko.observable();
            this.ParentPhone = ko.observable();
            this.ParentEmail = ko.observable();
            this.Brand = ko.observable();
            this.Location= ko.observable();
            this.AgentMenueVisible = ko.observable(false);
            this.AdminMenueVisible = ko.observable(false);
            this.CustomerMenueVisible = ko.observable(false);
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
                this.isEditing = ko.observable(false);
               
            //    this.edit = function () {
            //    var self = this;
            //    self.isEditing(true);
            //};

            this.Card = function () {
                var self = this;
                self.hasError = ko.observable(false);
                self.errorMessage = ko.observable();
                self.school = ko.observable("");
                self.degree = ko.observable("");
                self.year = ko.observable("");
                self.expectedInternshipPeriod = ko.observable("");
                self.expectedGraduationDate = ko.observable("");
                self.technicalSkills = ko.observable("");
                self.softSkills = ko.observable("");
                self.languageProficiency = ko.observable("");
                self.internships = ko.observable("");
                self.partTimeJobs = ko.observable("");
                self.volunteerWork = ko.observable("");
                self.relevantProjects = ko.observable("");
                self.projectDescription = ko.observable("");
                self.projectObjectives = ko.observable("");
                self.projectTechnologies = ko.observable("");
                self.projectRoles = ko.observable("");
                self.clubs = ko.observable("");
                self.leadershipRoles = ko.observable("");
                self.awards = ko.observable("");
                self.relevantCertifications = ko.observable("");
                self.trainingPrograms = ko.observable("");
                self.desiredIndustry = ko.observable();
                self.preferredRoles = ko.observable();
                self.careerGoals = ko.observable();
                self.reference1 = ko.observable("");
                self.contact1 = ko.observable("");
                self.reference2 = ko.observable("");
                self.contact2 = ko.observable("");
                self.reference3 = ko.observable("");
                self.contact3 = ko.observable("");
                self.portfolioLink1 = ko.observable('');
                self.workSample1 = ko.observable('');
                self.portfolioLink2 = ko.observable('');
                self.workSample2 = ko.observable('');
                self.portfolioLink3 = ko.observable('');
                self.workSample3 = ko.observable('');
                self.internshipAvailability = ko.observable();
                self.preferredStartDate = ko.observable();
                self.locationPreference = ko.observable();
                self.industryPreferences = ko.observable();
                self.companyPreferences = ko.observable();
                self.isEditing = ko.observable(false);

                self.edit = function () {
                    self.isEditing(true);
                };

                self.submitEditEducation = function () {
                    debugger;
                    /*var self = this;*/
                    this.hasError(false);
                    var functionDone = function (hasError, message, data) {
                        var alertArea = $('#alertArea1');
                        debugger;
                        if (hasError) {
                            alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                        } else {
                            if (data.IsSuccessful == true) {
                                alertArea.addClass('alert-success').text('Success!  ' + data.Message).show();
                                setTimeout(function () {
                                    alertArea.hide();
                                }, 5000); // Delay of 5 second
                            }
                            self.isEditing(false);
                        }
                    };
                    var functionFailed = function (hasError, message, data) {
                        var alertArea = $('#alertArea1');
                        debugger;
                        if (hasError) {
                            alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                        }
                        alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                    };
                    debugger;

                    var data = {
                        School: self.school(),
                        Degree: self.degree(),
                        YearOfStudy: self.year(),
                        ExpectedInternshipPeriod: self.expectedInternshipPeriod(),
                        ExpectedGraduationDate: self.expectedGraduationDate()
                    };
                    debugger;
                    jqueryAjaxGenerics.createJSONAjaxNonGETRequest("PUT", "/Resume/Resume/UpdateEducation", data,
                        functionDone, functionFailed);
                };

                self.submitEditSkills = function () {
                    debugger;
                    /*var self = this;*/
                    this.hasError(false);
                    var functionDone = function (hasError, message, data) {
                        var alertArea = $('#alertArea2');
                        debugger;
                        if (hasError) {
                            alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                        } else {
                            if (data.IsSuccessful == true) {
                                alertArea.addClass('alert-success').text('Success!  ' + data.Message).show();
                                setTimeout(function () {
                                    alertArea.hide();
                                }, 5000); // Delay of 5 second
                            }
                            self.isEditing(false);
                        }
                    };
                    var functionFailed = function (hasError, message, data) {
                        var alertArea = $('#alertArea2');
                        debugger;
                        if (hasError) {
                            alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                        }
                        alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                    };
                    debugger;

                    var data = {
                        TechnicalSkills: self.technicalSkills(),
                        SoftSkills: self.softSkills(),
                        LanguageProficiency: self.languageProficiency()
                    };
                    debugger;
                    jqueryAjaxGenerics.createJSONAjaxNonGETRequest("PUT", "/Resume/Resume/UpdateSkills", data,
                        functionDone, functionFailed);
                };

                self.submitEditExtraCurricular = function () {
                    debugger;
                    /*var self = this;*/
                    this.hasError(false);
                    var functionDone = function (hasError, message, data) {
                        var alertArea = $('#alertArea3');
                        debugger;
                        if (hasError) {
                            alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                        } else {
                            if (data.IsSuccessful == true) {
                                alertArea.addClass('alert-success').text('Success!  ' + data.Message).show();
                                setTimeout(function () {
                                    alertArea.hide();
                                }, 5000); // Delay of 5 second
                            }
                            self.isEditing(false);
                        }
                    };
                    var functionFailed = function (hasError, message, data) {
                        var alertArea = $('#alertArea3');
                        debugger;
                        if (hasError) {
                            alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                        }
                        alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                    };
                    debugger;

                    var data = {
                        Clubs: self.clubs(),
                        LeadershipRoles: self.leadershipRoles(),
                        Awards: self.awards()
                    };
                    debugger;
                    jqueryAjaxGenerics.createJSONAjaxNonGETRequest("PUT", "/Resume/Resume/UpdateExtracurricular", data,
                        functionDone, functionFailed);
                };

                self.submitEditCareer = function () {
                    debugger;
                    /*var self = this;*/
                    this.hasError(false);
                    var functionDone = function (hasError, message, data) {
                        var alertArea = $('#alertArea4');
                        debugger;
                        if (hasError) {
                            alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                        } else {
                            if (data.IsSuccessful == true) {
                                alertArea.addClass('alert-success').text('Success!  ' + data.Message).show();
                                setTimeout(function () {
                                    alertArea.hide();
                                }, 5000); // Delay of 5 second
                            }
                            self.isEditing(false);
                        }
                    };
                    var functionFailed = function (hasError, message, data) {
                        var alertArea = $('#alertArea4');
                        debugger;
                        if (hasError) {
                            alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                        }
                        alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                    };
                    debugger;

                    var data = {
                        DesiredIndustry: self.desiredIndustry(),
                        PreferredRoles: self.preferredRoles(),
                        CareerGoals: self.careerGoals()
                    };
                    debugger;
                    jqueryAjaxGenerics.createJSONAjaxNonGETRequest("PUT", "/Resume/Resume/UpdateCareer", data,
                        functionDone, functionFailed);
                };

                self.submitEditAvailability = function () {
                    debugger;
                    /*var self = this;*/
                    this.hasError(false);
                    var functionDone = function (hasError, message, data) {
                        var alertArea = $('#alertArea5');
                        debugger;
                        if (hasError) {
                            alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                        } else {
                            if (data.IsSuccessful == true) {
                                alertArea.addClass('alert-success').text('Success!  ' + data.Message).show();
                                setTimeout(function () {
                                    alertArea.hide();
                                }, 5000); // Delay of 5 second
                            }
                            self.isEditing(false);
                        }
                    };
                    var functionFailed = function (hasError, message, data) {
                        var alertArea = $('#alertArea5');
                        debugger;
                        if (hasError) {
                            alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                        }
                        alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                    };
                    debugger;

                    var data = {
                        InternshipAvailability: self.internshipAvailability(),
                        PreferredStartDate: self.preferredStartDate()
                    };
                    debugger;
                    jqueryAjaxGenerics.createJSONAjaxNonGETRequest("PUT", "/Resume/Resume/UpdateAvailability", data,
                        functionDone, functionFailed);
                };

                self.submitEditCertifications = function () {
                    debugger;
                    /*var self = this;*/
                    this.hasError(false);
                    var functionDone = function (hasError, message, data) {
                        var alertArea = $('#alertArea6');
                        debugger;
                        if (hasError) {
                            alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                        } else {
                            if (data.IsSuccessful == true) {
                                alertArea.addClass('alert-success').text('Success!  ' + data.Message).show();
                                setTimeout(function () {
                                    alertArea.hide();
                                }, 5000); // Delay of 5 second
                            }
                            self.isEditing(false);
                        }
                    };
                    var functionFailed = function (hasError, message, data) {
                        var alertArea = $('#alertArea6');
                        debugger;
                        if (hasError) {
                            alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                        }
                        alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                    };
                    debugger;

                    var data = {
                        RelevantCertifications: self.relevantCertifications(),
                        TrainingPrograms: self.trainingPrograms()
                    };
                    debugger;
                    jqueryAjaxGenerics.createJSONAjaxNonGETRequest("PUT", "/Resume/Resume/UpdateCertifications", data,
                        functionDone, functionFailed);
                };

                self.submitEditWork = function () {
                    debugger;
                    /*var self = this;*/
                    this.hasError(false);
                    var functionDone = function (hasError, message, data) {
                        var alertArea = $('#alertArea7');
                        debugger;
                        if (hasError) {
                            alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                        } else {
                            if (data.IsSuccessful == true) {
                                alertArea.addClass('alert-success').text('Success!  ' + data.Message).show();
                                setTimeout(function () {
                                    alertArea.hide();
                                }, 5000); // Delay of 5 second
                            }
                            self.isEditing(false);
                        }
                    };
                    var functionFailed = function (hasError, message, data) {
                        var alertArea = $('#alertArea7');
                        debugger;
                        if (hasError) {
                            alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                        }
                        alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                    };
                    debugger;

                    var data = {
                        Internships: self.internships(),
                        PartTimeJobs: self.partTimeJobs(),
                        VolunteerWork: self.volunteerWork(),
                        RelevantProjects: self.relevantProjects()
                    };
                    debugger;
                    jqueryAjaxGenerics.createJSONAjaxNonGETRequest("PUT", "/Resume/Resume/UpdateWork", data,
                        functionDone, functionFailed);
                };

                self.submitEditProject = function () {
                    debugger;
                    /*var self = this;*/
                    this.hasError(false);
                    var functionDone = function (hasError, message, data) {
                        var alertArea = $('#alertArea8');
                        debugger;
                        if (hasError) {
                            alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                        } else {
                            if (data.IsSuccessful == true) {
                                alertArea.addClass('alert-success').text('Success!  ' + data.Message).show();
                                setTimeout(function () {
                                    alertArea.hide();
                                }, 5000); // Delay of 5 second
                            }
                            self.isEditing(false);
                        }
                    };
                    var functionFailed = function (hasError, message, data) {
                        var alertArea = $('#alertArea8');
                        debugger;
                        if (hasError) {
                            alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                        }
                        alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                    };
                    debugger;

                    var data = {
                        ProjectDescription: self.projectDescription(),
                        ProjectObjectives: self.projectObjectives(),
                        ProjectTechnologies: self.projectTechnologies(),
                        ProjectRoles: self.projectRoles()
                    };
                    debugger;
                    jqueryAjaxGenerics.createJSONAjaxNonGETRequest("PUT", "/Resume/Resume/UpdateProject", data,
                        functionDone, functionFailed);
                };

                self.submitEditReferences = function () {
                    debugger;
                    /*var self = this;*/
                    this.hasError(false);
                    var functionDone = function (hasError, message, data) {
                        var alertArea = $('#alertArea9');
                        debugger;
                        if (hasError) {
                            alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                        } else {
                            if (data.IsSuccessful == true) {
                                alertArea.addClass('alert-success').text('Success!  ' + data.Message).show();
                                setTimeout(function () {
                                    alertArea.hide();
                                }, 5000); // Delay of 5 second
                            }
                            self.isEditing(false);
                        }
                    };
                    var functionFailed = function (hasError, message, data) {
                        var alertArea = $('#alertArea9');
                        debugger;
                        if (hasError) {
                            alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                        }
                        alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                    };
                    debugger;

                    var data = {
                        Reference1: self.reference1(),
                        ContactDetails1: self.contact1(),
                        Reference2: self.reference2(),
                        ContactDetails2: self.contact2(),
                        Reference3: self.reference3(),
                        ContactDetails3: self.contact3()
                    };
                    debugger;
                    jqueryAjaxGenerics.createJSONAjaxNonGETRequest("PUT", "/Resume/Resume/UpdateReferences", data,
                        functionDone, functionFailed);
                };

                self.submitEditPortfolio = function () {
                    debugger;
                    /*var self = this;*/
                    this.hasError(false);
                    var functionDone = function (hasError, message, data) {
                        var alertArea = $('#alertArea10');
                        debugger;
                        if (hasError) {
                            alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                        } else {
                            if (data.IsSuccessful == true) {
                                alertArea.addClass('alert-success').text('Success!  ' + data.Message).show();
                                setTimeout(function () {
                                    alertArea.hide();
                                }, 5000); // Delay of 5 second
                            }
                            self.isEditing(false);
                        }
                    };
                    var functionFailed = function (hasError, message, data) {
                        var alertArea = $('#alertArea10');
                        debugger;
                        if (hasError) {
                            alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                        }
                        alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                    };
                    debugger;

                    var data = {
                        PortfolioLink1: self.portfolioLink1(),
                        WorkSample1: self.workSample1(),
                        PortfolioLink2: self.portfolioLink2(),
                        WorkSample2: self.workSample2(),
                        PortfolioLink3: self.portfolioLink3(),
                        WorkSample3: self.workSample3()
                    };
                    debugger;
                    jqueryAjaxGenerics.createJSONAjaxNonGETRequest("PUT", "/Resume/Resume/UpdatePortfolio", data,
                        functionDone, functionFailed);
                };

                self.getCompanyDetails = function () {
                    debugger;
                    /*var self = this;*/
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
                                            break;
                                        case "Degree Program/Diploma/Certificate":
                                            self.degree(value);                                            
                                            break;
                                        case "Year of Study":
                                            self.year(value);                                            
                                            break;
                                        case "Expected Internship Period":
                                            self.expectedInternshipPeriod(value);                                            
                                            break;
                                        case "Expected Graduation Date":
                                            self.expectedGraduationDate(value);                                            
                                            break;
                                        case "Internships Work Experience":
                                            self.internships(value);                                            
                                            break;
                                        case "Part-time Jobs Work Experience":
                                            self.partTimeJobs(value);                                            
                                            break;
                                        case "Volunteer Work Work Experience":
                                            self.volunteerWork(value);                                           
                                            break;
                                        case "Relevant Projects or Research Experience":
                                            self.relevantProjects(value);                                            
                                            break;
                                        case "Description of Projects Completed":
                                            self.projectDescription(value);                                            
                                            break;
                                        case "Objectives and Outcomes Achieved":
                                            self.projectObjectives(value);                                            
                                            break;
                                        case "Technologies or Tools Used":
                                            self.projectTechnologies(value);                                            
                                            break;
                                        case "Roles and Responsibilities":
                                            self.projectRoles(value);                                           
                                            break;
                                        case "Clubs or Organizations":
                                            self.clubs(value);                                            
                                            break;
                                        case "Leadership Roles":
                                            self.leadershipRoles(value);                                            
                                            break;
                                        case "Awards or Achievements":
                                            self.awards(value);                                            
                                            break;
                                        case "Career Goals and Aspirations":
                                            self.careerGoals(value);                                            
                                            break;
                                        case "Desired Industry or Field":
                                            self.desiredIndustry(value);                                            
                                            break;
                                        case "Preferred Job Functions or Roles":
                                            self.preferredRoles(value);                                            
                                            break;
                                        case "Reference 1":
                                            self.reference1(value);                                            
                                            break;
                                        case "Contact Details for Reference 1":
                                            self.contact1(value);                                           
                                            break;
                                        case "Reference 2":
                                            self.reference2(value);                                            
                                            break;
                                        case "Contact Details for Reference 2":
                                            self.contact2(value);                                            
                                            break;
                                        case "Reference 3":
                                            self.reference3(value);                                           
                                            break;
                                        case "Contact Details for Reference 3":
                                            self.contact3(value);                                            
                                            break;
                                        case "Portfolio Link 1":
                                            self.portfolioLink1(value);                                            
                                            break;
                                        case "Work Sample 1":
                                            self.workSample1(value);                                            
                                            break;
                                        case "Portfolio Link 2":
                                            self.portfolioLink2(value);                                            
                                            break;
                                        case "Work Sample 2":
                                            self.workSample2(value);                                            
                                            break;
                                        case "Portfolio Link 3":
                                            self.portfolioLink3(value);                                            
                                            break;
                                        case "Work Sample 3":
                                            self.workSample3(value);                                            
                                            break;
                                        case "Technical Skills":
                                            self.technicalSkills(value);                                            
                                            break;
                                        case "Soft Skills":
                                            self.softSkills(value);                                            
                                            break;
                                        case "Language Proficiency":
                                            self.languageProficiency(value);                                            
                                            break;
                                        case "Relevant Certifications":
                                            self.relevantCertifications(value);                                           
                                            break;
                                        case "Training Programs Attended":
                                            self.trainingPrograms(value);                                            
                                            break;
                                        case "Availability for Internships":
                                            self.internshipAvailability(value);                                        
                                            break;
                                        case "Preferred Start Date":
                                            self.preferredStartDate(value);                                        
                                            break;
                                        case "Location Preference for Internships":
                                            self.locationPreference(value);                                            
                                            break;
                                        case "Industry Preferences":
                                            self.industryPreferences(value);                                           
                                            break;
                                        case "Company Size or Type Preferences":
                                            self.companyPreferences(value);                                            
                                            break;
                                        default:                                            
                                            break;
                                    }


                                });
                                debugger;                                
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
                    debugger;
                    jqueryAjaxGenerics.createJSONAjaxGETRequest("/Candidates/Candidates/GetLoggedCompanyDetails",
                        functionDone, functionFailed);

                }

                self.getCompanyDetails();
            };

            this.submitEditCareer = function () {
                debugger;
                var self = this;
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    var alertArea = $('#alertArea');
                    debugger;
                    if (hasError) {
                        alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                    } else {
                        if (data.IsSuccessful == true) {
                            alertArea.addClass('alert-success').text('Success!  ' + data.Message).show();
                            setTimeout(function () {
                                alertArea.hide();
                            }, 5000); // Delay of 5 second
                        }
                        self.isEditing(false);
                    }
                };
                var functionFailed = function (hasError, message, data) {
                    var alertArea = $('#alertArea');
                    debugger;
                    if (hasError) {
                        alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                    }
                    alertArea.addClass('alert-danger').text('Failed!' + data.Message).show();
                };
                debugger;

                var data = {
                    DesiredIndustry: self.desiredIndustry(),
                    PreferredRoles: self.preferredRoles(),
                    CareerGoals: self.careerGoals()
                };
                debugger;
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("PUT", "/Resume/Resume/UpdateCareer", data,
                    functionDone, functionFailed);
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

            // Initially hide the success card
            $('#successMessage').hide();

            this.checkPortfolio = function () {
                var self = this;
                this.hasError(false);

                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {

                        if (data.Result == true) {
                            $('#portfolioCard').hide();
                            $('#successMessage').show();
                            // Hide the card if details exist
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
                            $('#availabilityCard').hide();                           
                            // Hide the card if details exist
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

            //// Initially hide the success card
            //$('#successMessage').hide();

            //this.checkPreferences = function () {
            //    var self = this;
            //    this.hasError(false);

            //    var functionDone = function (hasError, message, data) {
            //        if (hasError) {
            //            jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
            //        } else {

            //            if (data.Result == true) {
            //                $('#preferenceCard').hide(); // Hide the card if details exist
            //                $('#successMessage').show(); // Show the success card
            //            }
            //        }
            //    };

            //    var functionFailed = function (hasError, Message, data) {
            //        if (hasError) {
            //            jqueryConfirmGenerics.showOkAlertBox('Failed!', Message, "red", null);
            //        }
            //        // Handle other failure cases if necessary
            //    };


            //    // Make a GET request with the form data
            //    jqueryAjaxGenerics.createJSONAjaxGETRequest("/Resume/Resume/CheckPreference", functionDone, functionFailed);
            //};

            //// Call the function to check form details
            //this.checkPreferences();
                      

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
                            // Adjust this part according to your response data
                            // Example: If the response indicates success
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
                            if (data.IsSuccessful == true) {
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
                            if (data.IsSuccessful == true) {
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
                            if (data.IsSuccessful == true) {
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

            this.GetMessage();

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



