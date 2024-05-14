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
            var self = this; // Define 'self' here
            this.hasError = ko.observable(false);
            this.errorMessage = ko.observable();
            this.Balance = ko.observable(0);
            this.PhotoLinkUrl = ko.observable();
            this.ParentPhone = ko.observable();
            this.ParentEmail = ko.observable();
            this.Brand = ko.observable();
            this.Location = ko.observable();            
            this.AdminMenueVisible = ko.observable(false);            
            this.InternMenueVisible = ko.observable(false);
            this.CompanyMenueVisible = ko.observable(false);
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
            this.mission = ko.observable("");
            this.vision = ko.observable("");
            this.coreValues = ko.observable("");
            this.history = ko.observable("");
            this.ongoingProjects = ko.observable("");
            this.pastProjects = ko.observable("");
            this.expertise = ko.observable("");
            this.specialization = ko.observable("");
            this.internships = ko.observable("");
            this.workEnvironment = ko.observable("");
            this.companyValues = ko.observable("");
            this.usedTechnologies = ko.observable("");
            this.innovation = ko.observable("");
            this.programs = ko.observable("");
            this.projectTechnologies = ko.observable("");
            this.environmentalEfforts = ko.observable("");
            this.internshipPrograms = ko.observable("");
            this.jobs = ko.observable("");
            this.awards = ko.observable("");
            this.certifications = ko.observable("");
            this.internsTraits = ko.observable("");
            this.desiredSkill = ko.observable();            
            this.careerOpportunities = ko.observable();
            this.CompanyId = ko.observable();
            this.CompanyName = ko.observable();
            this.CompanyLocation = ko.observable();
            this.CompanyProfileImage = ko.observable();
            self.Companies = ko.observableArray([]);
            this.EntityId = ko.observable();
            this.EntityName = ko.observable();
            this.EntityPhone = ko.observable();
            this.EntityEmail = ko.observable();
            this.EntityProfileImage = ko.observable();
            this.CompanySector = ko.observable();
            self.Candidates = ko.observableArray([]);
            this.CandidateId = ko.observable();
            this.CandidateName = ko.observable();
            this.CandidateLocation = ko.observable();
            this.CandidateProfileImage = ko.observable();
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
            this.projectCategory = ko.observable();
            this.projectDetails = ko.observable();
            this.projectName = ko.observable();
            this.projectStart = ko.observable();
            this.projectEnd = ko.observable();
            this.projectLocation = ko.observable();
            this.openPosition = ko.observable();
            this.projectRecommendation = ko.observable();
            this.workScope = ko.observable();
            this.postAs = ko.observable();
            self.AllProjects = ko.observableArray([]);
            self.AllCandidates = ko.observableArray([]);
            this.ProjectId = ko.observable();
            this.postDate = ko.observable();
            this.ProjectName = ko.observable();
            this.NoOfProjects = ko.observable();
            this.NoOfCandidates = ko.observable();
            this.NoOfCompanies = ko.observable();
            // Current page number, starts from 1
            self.currentPage = ko.observable(1);
            // Number of projects per page
            self.candidatesPerPage = ko.observable(15);
            self.companiesPerPage = ko.observable(15);
            self.projectsPerPage = ko.observable(15);
            self.currentSortOrder = ko.observable('Newest Project');
            this.selectedIndustry = ko.observable();
            this.selectedLocation = ko.observable();
            self.searchProjectsCalled = false;
            self.isEditing = ko.observable(false);
            this.whenPosted = ko.observable();

            this.Card = function () {
                var self = this;
                self.hasError = ko.observable(false);
                self.errorMessage = ko.observable();
                self.mission = ko.observable("");
                self.vision = ko.observable("");
                self.coreValues = ko.observable("");
                self.history = ko.observable("");
                self.ongoingProjects = ko.observable("");
                self.pastProjects = ko.observable("");
                self.expertise = ko.observable("");
                self.specialization = ko.observable("");
                self.internships = ko.observable("");
                self.workEnvironment = ko.observable("");
                self.companyValues = ko.observable("");
                self.usedTechnologies = ko.observable("");
                self.innovation = ko.observable("");
                self.programs = ko.observable("");
                self.projectTechnologies = ko.observable("");
                self.environmentalEfforts = ko.observable("");
                self.internshipPrograms = ko.observable("");
                self.jobs = ko.observable("");
                self.awards = ko.observable("");
                self.certifications = ko.observable("");
                self.internsTraits = ko.observable("");
                self.desiredSkill = ko.observable();
                self.careerOpportunities = ko.observable();
                self.isEditing = ko.observable(false);

                self.edit = function () {
                    self.isEditing(true);
                };

                self.editCompanyOverview = function () {
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
                        MissionStatement: self.mission(),
                        Vision: self.vision(),
                        CoreValues: self.coreValues(),
                        HistoryBackground: self.history()
                    };
                    debugger;
                    jqueryAjaxGenerics.createJSONAjaxNonGETRequest("PUT", "/Companies/Companies/UpdateCompanyOverview", data,
                        functionDone, functionFailed);
                };

                self.editCompanyProjects = function () {
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
                        OngoingProjects: self.ongoingProjects(),
                        PastProjects: self.pastProjects(),
                        ProjectTechnologies: self.projectTechnologies()
                    };
                    debugger;
                    jqueryAjaxGenerics.createJSONAjaxNonGETRequest("PUT", "/Companies/Companies/UpdateProjects", data,
                        functionDone, functionFailed);
                };

                self.editInternshipType = function () {
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
                        Internships: self.internships()
                    };
                    debugger;
                    jqueryAjaxGenerics.createJSONAjaxNonGETRequest("PUT", "/Companies/Companies/UpdateInternship", data,
                        functionDone, functionFailed);
                };

                self.editCoreCompetencies = function () {
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
                        AreaofExpertise: self.expertise(),
                        Specializations: self.specialization()
                    };
                    debugger;
                    jqueryAjaxGenerics.createJSONAjaxNonGETRequest("PUT", "/Companies/Companies/UpdateCompetencies", data,
                        functionDone, functionFailed);
                };

                self.editCompanyCulture = function () {
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
                        WorkEnvironment: self.workEnvironment(),
                        CompanyValues: self.companyValues()
                    };
                    debugger;
                    jqueryAjaxGenerics.createJSONAjaxNonGETRequest("PUT", "/Companies/Companies/UpdateCompanyCulture", data,
                        functionDone, functionFailed);
                };

                self.editTechnology = function () {
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
                        TechnologiesUsed: self.usedTechnologies(),
                        InnovationInitiatives: self.innovation()
                    };
                    debugger;
                    jqueryAjaxGenerics.createJSONAjaxNonGETRequest("PUT", "/Companies/Companies/UpdateTechnology", data,
                        functionDone, functionFailed);
                };

                self.editSocial = function () {
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
                        Programs: self.programs(),
                        EnvironmentalEfforts: self.environmentalEfforts()
                    };
                    debugger;
                    jqueryAjaxGenerics.createJSONAjaxNonGETRequest("PUT", "/Companies/Companies/UpdateSocialResponsibility", data,
                        functionDone, functionFailed);
                };

                self.editIndustryAwards = function () {
                    debugger;
                    /*var self = this;*/
                    this.hasError(false);
                    var functionDone = function (hasError, message, data) {
                        var alertArea = $('#alertArea8');
                        debugger;
                        if (hasError) {
                            alertArea.addClass('alert-danger').text('Failed! ' + data.Message).show();
                            setTimeout(function () {
                                alertArea.hide();
                            }, 10000); // Delay of 10 second
                        } else {
                            if (data.IsSuccessful == true) {
                                alertArea.addClass('alert-success').text('Success!  ' + data.Message).show();
                                setTimeout(function () {
                                    alertArea.hide();
                                }, 5000); // Delay of 5 second
                            }
                            /*self.isEditing(false);*/
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
                        IndustryAwards: self.awards(),
                        Certifications: self.certifications()
                    };
                    debugger;
                    jqueryAjaxGenerics.createJSONAjaxNonGETRequest("PUT", "/Companies/Companies/UpdateAwards", data,
                        functionDone, functionFailed);
                };

                self.editCareer = function () {
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
                        InternshipPrograms: self.internshipPrograms(),
                        JobOpenings: self.jobs(),
                        CareerGrowth: self.careerOpportunities()
                    };
                    debugger;
                    jqueryAjaxGenerics.createJSONAjaxNonGETRequest("PUT", "/Companies/Companies/UpdateCareer", data,
                        functionDone, functionFailed);
                };

                self.editInternProfile = function () {
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
                        DesiredSkills: self.desiredSkill(),
                        InternTraits: self.internsTraits()
                    };
                    debugger;
                    jqueryAjaxGenerics.createJSONAjaxNonGETRequest("PUT", "/Companies/Companies/UpdatePreferredIntern", data,
                        functionDone, functionFailed);
                };

                self.getUserDetails = function () {
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
                                        case "Mission Statement":
                                            self.mission(value);                                            
                                            break;
                                        case "Vision":
                                            self.vision(value);                                            
                                            break;
                                        case "Core Values":
                                            self.coreValues(value);                                            
                                            break;
                                        case "History and Background":
                                            self.history(value);                                            
                                            break;
                                        case "Ongoing Projects":
                                            self.ongoingProjects(value);                                            
                                            break;
                                        case "Past Projects":
                                            self.pastProjects(value);                                            
                                            break;
                                        case "Technologies or Tools Used":
                                            self.projectTechnologies(value);                                            
                                            break;
                                        case "Internship Type":
                                            self.internships(value);                                           
                                            break;
                                        case "Area of Expertise":
                                            self.expertise(value);                                            
                                            break;
                                        case "Specializations":
                                            self.specialization(value);                                            
                                            break;
                                        case "Work Environment":
                                            self.workEnvironment(value);                                            
                                            break;
                                        case "Company Values and Culture":
                                            self.companyValues(value);                                            
                                            break;
                                        case "Technologies Used":
                                            self.usedTechnologies(value);                                            
                                            break;
                                        case "Innovation Initiatives":
                                            self.innovation(value);                                            
                                            break;
                                        case "CSR Initiatives or Programs":
                                            self.programs(value);                                            
                                            break;
                                        case "Environmental Sustainability Efforts":
                                            self.environmentalEfforts(value);                                            
                                            break;
                                        case "Industry Awards":
                                            self.awards(value);                                            
                                            break;
                                        case "Certifications":
                                            self.certifications(value);                                            
                                            break;
                                        case "Internship Programs":
                                            self.internshipPrograms(value);                                            
                                            break;
                                        case "Job Openings":
                                            self.jobs(value);                                            
                                            break;
                                        case "Opportunities for Career Growth and Development":
                                            self.careerOpportunities(value);                                            
                                            break;
                                        case "Desired Skills and Qualifications in Interns":
                                            self.desiredSkill(value);                                            
                                            break;
                                        case "Characteristics or Traits sought in Interns":
                                            self.internsTraits(value);                                            
                                            break;
                                        default:
                                            // Handle default case if needed
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
                    jqueryAjaxGenerics.createJSONAjaxGETRequest("/Candidates/Candidates/GetUserDetails",
                        functionDone, functionFailed);

                }

                self.getUserDetails();
            };

            self.sortedProjects = ko.computed(function () {
                debugger;
                var projects = self.AllProjects().slice(); // Create a copy of the array
                var currentDate = new Date(); // Get the current date
                if (self.currentSortOrder() === 'Newest Project') {
                    projects.sort(function (a, b) {
                        return new Date(b.postDate) - currentDate; // Sort by how recently the project was posted
                    });
                } else if (self.currentSortOrder() === 'Oldest Project') {
                    projects.sort(function (a, b) {
                        return currentDate - new Date(a.postDate); // Sort by how long ago the project was posted
                    });
                }
                return projects;
            });
            self.projectsOnCurrentPage = ko.computed(function () {
                debugger;
                /*var self = this;*/
                if (self.AllProjects() && self.AllProjects().length > 0) {
                    var startIndex = (self.currentPage() - 1) * self.projectsPerPage();
                    return self.AllProjects.slice(startIndex, startIndex + self.projectsPerPage());
                } else {
                    return [];
                }
            });

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

            self.companiesOnCurrentPage = ko.computed(function () {
                debugger;
                /*var self = this;*/
                if (self.Companies() && self.Companies().length > 0) {
                    var startIndex = (self.currentPage() - 1) * self.companiesPerPage();
                    return self.Companies.slice(startIndex, startIndex + self.companiesPerPage());
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

            this.checkCompanyOverview = function () {
                var self = this;
                this.hasError(false);

                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {

                        if (data.Result == true) {
                            $('#companyOverviewCard').hide(); // Hide the card if details exist
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
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Companies/Companies/CheckCompany", functionDone, functionFailed);
            };

            // Call the function to check form details
            this.checkCompanyOverview();


           this.checkProjects = function () {
                var self = this;
                this.hasError(false);

                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {

                        if (data.Result == true) {
                            $('#initiativeCard').hide(); // Hide the card if details exist
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
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Companies/Companies/CheckProjects", functionDone, functionFailed);
            };

            // Call the function to check form details
            this.checkProjects();

            this.checkInternship = function () {
                var self = this;
                this.hasError(false);

                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {

                        if (data.Result == true) {
                            $('#internshipCard').hide(); // Hide the card if details exist
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
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Companies/Companies/CheckInternship", functionDone, functionFailed);
            };

            // Call the function to check form details
            this.checkInternship();

            this.checkCompetence = function () {
                var self = this;
                this.hasError(false);

                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {

                        if (data.Result == true) {
                            $('#coreCard').hide(); // Hide the card if details exist
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
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Companies/Companies/CheckCompetence", functionDone, functionFailed);
            };

            // Call the function to check form details
            this.checkCompetence();

            this.checkCareer = function () {
                var self = this;
                this.hasError(false);

                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {

                        if (data.Result == true) {
                            $('#opportunityCard').hide(); // Hide the card if details exist
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
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Companies/Companies/CheckCareer", functionDone, functionFailed);
            };

            // Call the function to check form details
            this.checkCareer();

            this.checkCulture = function () {
                var self = this;
                this.hasError(false);

                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {

                        if (data.Result == true) {
                            $('#cultureCard').hide(); // Hide the card if details exist
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
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Companies/Companies/CheckCulture", functionDone, functionFailed);
            };

            // Call the function to check form details
            this.checkCulture();

            this.checkTechnology = function () {
                var self = this;
                this.hasError(false);

                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {

                        if (data.Result == true) {
                            $('#technologyCard').hide(); // Hide the card if details exist
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
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Companies/Companies/CheckTechnology", functionDone, functionFailed);
            };

            // Call the function to check form details
            this.checkTechnology();

            this.checkSocial = function () {
                var self = this;
                this.hasError(false);

                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {

                        if (data.Result == true) {
                            $('#socialCard').hide(); // Hide the card if details exist
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
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Companies/Companies/CheckSocial", functionDone, functionFailed);
            };

            // Call the function to check form details
            this.checkSocial();

            this.checkAward = function () {
                var self = this;
                this.hasError(false);

                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {

                        if (data.Result == true) {
                            $('#awardCard').hide(); // Hide the card if details exist
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
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Companies/Companies/CheckAward", functionDone, functionFailed);
            };

            // Call the function to check form details
            this.checkAward();

            // Initially hide the success card
            $('#successMessage').hide();

           /* $('#noProfile').hide();*/

            this.CheckIntern= function () {
                var self = this;
                this.hasError(false);

                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {

                        if (data.Result == true) {
                            $('#internCard').hide(); // Hide the card if details exist
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
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Companies/Companies/CheckIntern", functionDone, functionFailed);
            };

            // Call the function to check form details
            this.CheckIntern();


            this.companyOverview = function () {
                debugger;
                var self = this;
                // Function to be executed on successful AJAX request
                var functionDone = function (hasError, message, data) {
                    var alertArea = $('#alertArea');
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        alertArea.addClass('alert-danger').text('Failed! ' + Message).show();
                    } else if (data.IsOkay == false) {
                        alertArea.addClass('alert-danger').text('Failed! ' + data.Message).show();
                        self.clearForm();
                    } else {
                        if (data.Message === 'Company Overview added Successfully.') {
                            alertArea.addClass('alert-success').text('Company Overview added Successfully').show();
                            window.location.reload();
                        }
                    }
                };

                // Function to be executed on failed AJAX request
                var functionFailed = function (hasError, message, data) {
                    var alertArea = $('#alertArea');
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        alertArea.addClass('alert-danger').text('Failed! ' + message).show();
                    }
                    alertArea.addClass('alert-danger').text('Failed! ' + data.Message).show();
                };

                var data = {
                    MissionStatement: self.mission(),
                    Vision: self.vision(),
                    CoreValues: self.coreValues(),
                    HistoryBackground: self.history()                    
                };

                // Make AJAX request
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Companies/Companies/CompanyOverview", data, functionDone, functionFailed);
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
            this.companyProjects = function () {
                debugger;
                var self = this;

                // Function to be executed on successful AJAX request
                var functionDone = function (hasError, message, data) {
                    var alertArea = $('#alertArea');
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        alertArea.addClass('alert-danger').text('Failed! ' + Message).show();
                    } else if (data.IsOkay == false) {
                        alertArea.addClass('alert-danger').text('Failed! ' + data.Message).show();
                        self.clearForm();
                    } else {
                        if (data.Message === 'Projects added Successfully.') {
                            alertArea.addClass('alert-success').text('Projects and Initiatives added successfully').show();
                            window.location.reload();
                        } else {
                            alertArea.addClass('alert-danger').text('Projects and Initiatives Not Added').show();
                            self.clearForm();
                        }
                    }
                };

                // Function to be executed on failed AJAX request
                var functionFailed = function (hasError, message, data) {
                    var alertArea = $('#alertArea');
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        alertArea.addClass('alert-danger').text('Failed! ' + message).show();
                    }
                    alertArea.addClass('alert-danger').text('Failed! ' + message).show();
                };

                var data = {
                    OngoingProjects: self.ongoingProjects(),
                    PastProjects: self.pastProjects(),
                    ProjectTechnologies: self.projectTechnologies()

                };

                // Make AJAX request
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST","/Companies/Companies/Projects", data, functionDone, functionFailed);
            };

            this.internshipType = function () {
                debugger;
                var self = this;

                // Function to be executed on successful AJAX request
                var functionDone = function (hasError, message, data) {
                    var alertArea = $('#alertArea');
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        alertArea.addClass('alert-danger').text('Failed! ' + Message).show();
                    } else if (data.IsOkay == false) {
                        alertArea.addClass('alert-danger').text('Failed! ' + data.Message).show();                        
                    } else {
                        if (data.IsSuccessful == true) {
                            alertArea.addClass('alert-success').text('Internship Type added Successfully.').show();
                            window.location.reload();
                        } else {
                            alertArea.addClass('alert-danger').text('Internship Type Not Added.').show();                            
                        }
                    }
                };

                // Function to be executed on failed AJAX request
                var functionFailed = function (hasError, message, data) {
                    var alertArea = $('#alertArea');
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        alertArea.addClass('alert-danger').text('Failed! ' + message).show();
                    }
                    alertArea.addClass('alert-danger').text('Failed! ' + message).show();
                };

                var data = {
                    Internships: self.internships()
                };

                // Make AJAX request
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Companies/Companies/Internship", data, functionDone, functionFailed);
            };

            this.coreCompetencies = function () {
                debugger;
                var self = this;

                // Function to be executed on successful AJAX request
                var functionDone = function (hasError, message, data) {
                    var alertArea = $('#alertArea');
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        alertArea.addClass('alert-danger').text('Failed! ' + Message).show();
                    } else if (data.IsOkay == false) {
                        alertArea.addClass('alert-danger').text('Failed! ' + data.Message).show();                        
                    } else {
                        if (data.Message === 'Core Competencies added Successfully.') {
                            alertArea.addClass('alert-success').text('Core Competencies added Successfully.').show();
                            window.location.reload();
                        } else {
                            alertArea.addClass('alert-danger').text('Core Competencies Not Added.').show();
                        }
                    }
                };

                // Function to be executed on failed AJAX request
                var functionFailed = function (hasError, message, data) {
                    var alertArea = $('#alertArea');
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        alertArea.addClass('alert-danger').text('Failed! ' + message).show();
                    }
                    alertArea.addClass('alert-danger').text('Failed! ' + message).show();
                };

                var data = {
                    AreaofExpertise: self.expertise(),
                    Specializations: self.specialization()
                };

                // Make AJAX request
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Companies/Companies/Competencies", data, functionDone, functionFailed);
            };

            this.companyCulture = function () {
                debugger;
                var self = this;

                // Function to be executed on successful AJAX request
                var functionDone = function (hasError, message, data) {
                    var alertArea = $('#alertArea');
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        alertArea.addClass('alert-danger').text('Failed! ' + Message).show();
                    } else if (data.IsOkay == false) {
                        alertArea.addClass('alert-danger').text('Failed! ' + data.Message).show();                        
                    } else {
                        if (data.Message === 'Company Culture added Successfully.') {
                            alertArea.addClass('alert-success').text('Company Culture added Successfully.').show();
                            window.location.reload();
                        } else {
                            alertArea.addClass('alert-danger').text('Company Culture Not Added').show();                            
                        }
                    }
                };

                // Function to be executed on failed AJAX request
                var functionFailed = function (hasError, message, data) {
                    var alertArea = $('#alertArea');
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        alertArea.addClass('alert-danger').text('Failed! ' + message).show();
                    }
                    alertArea.addClass('alert-danger').text('Failed! ' + message).show();
                };

                var data = {
                    WorkEnvironment: self.workEnvironment(),
                    CompanyValues: self.companyValues()
                };

                // Make AJAX request
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Companies/Companies/CompanyCulture", data, functionDone, functionFailed);
            };

            this.technology = function () {
                debugger;
                var self = this;

                // Function to be executed on successful AJAX request
                var functionDone = function (hasError, message, data) {
                    var alertArea = $('#alertArea');
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        alertArea.addClass('alert-danger').text('Failed! ' + Message).show();
                    } else if (data.IsOkay == false) {
                        alertArea.addClass('alert-danger').text('Failed! ' + data.Message).show();                       
                    } else {
                        if (data.Message === 'Technology Capabilities added Successfully.') {
                            alertArea.addClass('alert-success').text('Technology Capabilities added Successfully.').show();
                            window.location.reload();
                        } else {
                            alertArea.addClass('alert-danger').text('Technology Capabilities Not Added').show();
                        }
                    }
                };

                // Function to be executed on failed AJAX request
                var functionFailed = function (hasError, message, data) {
                    var alertArea = $('#alertArea');
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        alertArea.addClass('alert-danger').text('Failed! ' + message).show();
                    }
                    alertArea.addClass('alert-danger').text('Failed! ' + message).show();
                };

                var data = {
                    TechnologiesUsed: self.usedTechnologies(),
                    InnovationInitiatives: self.innovation()
                };

                // Make AJAX request
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Companies/Companies/Technology", data, functionDone, functionFailed);
            };

            this.social = function () {
                debugger;
                var self = this;

                // Function to be executed on successful AJAX request
                var functionDone = function (hasError, message, data) {
                    var alertArea = $('#alertArea');
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        alertArea.addClass('alert-danger').text('Failed! ' + Message).show();
                    } else if (data.IsOkay == false) {
                        alertArea.addClass('alert-danger').text('Failed! ' + data.Message).show();
                        self.clearForm();
                    } else {
                        if (data.Message === 'Social Responsibility added Successfully.') {
                            alertArea.addClass('alert-success').text('Corporate Social Responsibility (CSR) Added Successfully').show();
                            window.location.reload();
                        } else {
                            alertArea.addClass('alert-danger').text('Corporate Social Responsibility (CSR) Not Added').show();
                        }
                    }
                };

                // Function to be executed on failed AJAX request
                var functionFailed = function (hasError, message, data) {
                    var alertArea = $('#alertArea');
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        alertArea.addClass('alert-danger').text('Failed! ' + message).show();
                    }
                    alertArea.addClass('alert-danger').text('Failed! ' + message).show();
                };

                var data = {
                    Programs: self.programs(),
                    EnvironmentalEfforts: self.environmentalEfforts()
                };

                // Make AJAX request
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Companies/Companies/SocialResponsibility", data, functionDone, functionFailed);
            };

            this.industryAwards = function () {
                debugger;
                var self = this;

                // Function to be executed on successful AJAX request
                var functionDone = function (hasError, message, data) {
                    var alertArea = $('#alertArea');
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        alertArea.addClass('alert-danger').text('Failed! ' + Message).show();
                    } else if (data.IsOkay == false) {
                        alertArea.addClass('alert-danger').text('Failed! ' + data.Message).show();
                        self.clearForm();
                    } else {
                        if (data.Message === 'Awards and Recognition added Successfully.') {
                            alertArea.addClass('alert-success').text('Awards and Recognition added Successfully.').show();
                            window.location.reload();
                        } else {
                            alertArea.addClass('alert-danger').text('Awards and Recognition Not Added').show();
                        }
                    }
                };

                // Function to be executed on failed AJAX request
                var functionFailed = function (hasError, message, data) {
                    var alertArea = $('#alertArea');
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        alertArea.addClass('alert-danger').text('Failed! ' + message).show();
                    }
                    alertArea.addClass('alert-danger').text('Failed! ' + message).show();
                };

                var data = {
                    IndustryAwards: self.awards(),
                    Certifications: self.certifications()
                };

                // Make AJAX request
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Companies/Companies/Awards", data, functionDone, functionFailed);
            };

            this.career = function () {
                debugger;
                var self = this;

                // Function to be executed on successful AJAX request
                var functionDone = function (hasError, message, data) {
                    var alertArea = $('#alertArea');
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        alertArea.addClass('alert-danger').text('Failed! ' + Message).show();
                    } else if (data.IsOkay == false) {
                        alertArea.addClass('alert-danger').text('Failed! ' + data.Message).show();
                        self.clearForm();
                    } else {
                        if (data.Message === 'Career Opportunities added Successfully.') {
                            alertArea.addClass('alert-success').text('Career Opportunities added Successfully.').show();
                            window.location.reload();
                        } else {
                            alertArea.addClass('alert-danger').text('Career Opportunities Not Added').show();
                        }
                    }
                };

                // Function to be executed on failed AJAX request
                var functionFailed = function (hasError, message, data) {
                    var alertArea = $('#alertArea');
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        alertArea.addClass('alert-danger').text('Failed! ' + message).show();
                    }
                    alertArea.addClass('alert-danger').text('Failed! ' + message).show();
                };

                var data = {
                    InternshipPrograms: self.internshipPrograms(),
                    JobOpenings: self.jobs(),
                    CareerGrowth: self.careerOpportunities()
                };

                // Make AJAX request
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Companies/Companies/Career", data, functionDone, functionFailed);
            };
             
            this.internProfile = function () {
                debugger;
                var self = this;

                // Function to be executed on successful AJAX request
                var functionDone = function (hasError, message, data) {
                    var alertArea = $('#alertArea');
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        alertArea.addClass('alert-danger').text('Failed! ' + Message).show();
                    } else if (data.IsOkay == false) {
                        alertArea.addClass('alert-danger').text('Failed! ' + data.Message).show();
                        self.clearForm();
                    } else {
                        if (data.Message === 'Preferred Intern added Successfully.') {
                            alertArea.addClass('alert-success').text('Preferred Intern Profile Added Successfully.').show();
                            window.location.reload();
                        } else {
                            alertArea.addClass('alert-danger').text('Preferred Intern Profile Not Added').show();
                        }
                    }  
                };

                // Function to be executed on failed AJAX request
                var functionFailed = function (hasError, message, data) {
                    var alertArea = $('#alertArea');
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        alertArea.addClass('alert-danger').text('Failed! ' + message).show();
                    }
                    alertArea.addClass('alert-danger').text('Failed! ' + message).show();
                };

                var data = {
                    DesiredSkills: self.desiredSkill(),
                    InternTraits: self.internsTraits()
                   
                };

                // Make AJAX request
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Companies/Companies/PreferredIntern", data, functionDone, functionFailed);
            };

            this.submitPostProject = function () {
                debugger;
                var self = this;

                // Function to be executed on successful AJAX request
                var functionDone = function (hasError, message, data) {
                    var alertArea = $('#alertArea');
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        alertArea.addClass('alert-danger').text('Failed! ' + Message).show();
                    } else if (data.IsOkay == false) {
                        alertArea.addClass('alert-danger').text('Failed! ' + data.Message).show();
                        self.clearForm();
                    } else {
                        if (data.Message === 'Project added Successfully.') {
                            alertArea.addClass('alert-success').text('Project Added Successfully.').show();
                            setTimeout(function () {
                                window.location.reload();
                            }, 3000); // Delay of 3 second
                        } else {
                            alertArea.addClass('alert-danger').text('Project Was Not Added').show();
                        }
                    }
                };

                // Function to be executed on failed AJAX request
                var functionFailed = function (hasError, message, data) {
                    var alertArea = $('#alertArea');
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        alertArea.addClass('alert-danger').text('Failed! ' + message).show();
                    }
                    alertArea.addClass('alert-danger').text('Failed! ' + message).show();
                };
                debugger;
                var data = {
                    ProjectCategory: self.projectCategory(),
                    ProjectDetails: self.projectDetails(),
                    ProjectStart: convertToUTC(self.projectStart()),
                    ProjectName: self.projectName(),
                    ProjectEnd: convertToUTC(self.projectEnd()),
                    OpenPosition: self.openPosition(),
                    ProjectRecommendation: self.projectRecommendation(),
                    WorkScope: self.workScope(),
                    PostAs: self.postAs(),
                    AccountEntityId: localStorage.getItem('EntityId'),
                };

                // Make AJAX request
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Companies/Companies/CompanyProject", data, functionDone, functionFailed);
            };

            this.submitCompanyProject = function () {
                debugger;
                var self = this;

                // Function to be executed on successful AJAX request
                var functionDone = function (hasError, message, data) {
                    var alertArea = $('#alertArea');
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        alertArea.addClass('alert-danger').text('Failed! ' + Message).show();
                    } else if (data.IsOkay == false) {
                        alertArea.addClass('alert-danger').text('Failed! ' + data.Message).show();
                        self.clearForm();
                    } else {
                        if (data.Message === 'Project added Successfully.') {
                            alertArea.addClass('alert-success').text('Project Added Successfully.').show();
                            setTimeout(function () {
                                window.location.reload();
                            }, 3000); // Delay of 3 second
                        } else {
                            alertArea.addClass('alert-danger').text('Project Was Not Added').show();
                        }
                    }
                };

                // Function to be executed on failed AJAX request
                var functionFailed = function (hasError, message, data) {
                    var alertArea = $('#alertArea');
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        alertArea.addClass('alert-danger').text('Failed! ' + message).show();
                    }
                    alertArea.addClass('alert-danger').text('Failed! ' + message).show();
                };

                var data = {
                    ProjectCategory: self.projectCategory(),
                    ProjectDetails: self.projectDetails(),
                    ProjectStart: convertToUTC(self.projectStart()),
                    ProjectName: self.projectName(),
                    ProjectEnd: convertToUTC(self.projectEnd()),
                    OpenPosition: self.openPosition(),
                    ProjectRecommendation: self.projectRecommendation(),
                    WorkScope: self.workScope(),
                    PostAs: self.postAs(),
                    ProjectLocation: self.projectLocation()
                };

                // Make AJAX request
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Companies/Companies/CompanyProject", data, functionDone, functionFailed);
            };

            this.getAllCompanies = function () {
                debugger;
                var self = this;
                self.hasError(false);
                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        debugger;
                        var CompaniesNo = data.Result.length
                        self.NoOfCompanies(CompaniesNo);
                        var companies = [];
                        data.Result.forEach(function (item) {
                            var id = item.Id;
                            var name = item.EntityName;
                            var location = item.PhysicalAddress;
                            var profileImage = item.ProfileImageUrl;
                            var industrySector = item.PostalAddress;

                            var companiesData = {
                                CompanyId: id,
                                CompanyName: name,
                                CompanyLocation: location,
                                CompanyProfileImage: profileImage,
                                CompanySector: industrySector,
                            }
                            companies.push(companiesData);
                        });
                        self.Companies(companies)
                    }
                };
                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Candidates/Candidates/GetMainCompanies",
                    functionDone, functionFailed);

            }.bind(this);
            this.getAllCompanies();

            this.getSpecificProjects = function () {
                debugger;
                var self = this;
                self.hasError(false);
                var functionDone = function (hasError, message, data) {
                    var alertArea = $('#alertArea');
                    if (hasError) {
                        alertArea.addClass('alert-danger').text('Failed! ' + message).show();
                        /*jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);*/
                    } else {
                        debugger;
                        //if (data.IsSuccessful == true) {
                        //    var alertAreaP = $('#alertAreaProjects');
                        //    alertAreaP.addClass('alert-danger').text(data.Message).show();
                        //    setTimeout(function () {
                        //        window.location.reload();
                        //    }, 5000);
                        //}
                        var ProjectsNo = data.Result.length
                        self.NoOfProjects(ProjectsNo);
                        var projects = []
                        data.Result.forEach(function (item) {
                            var id = item.Id;
                            var name = item.ProjectName;
                            var location = item.ProjectLocation;
                            var profileImage = item.AccountEntity.ProfileImageUrl;
                            var industrySector = item.ProjectCategory;
                            var datePosted = item.DatePosted;

                            var projectData = {
                                ProjectId: id,
                                ProjectName: name,
                                CompanyLocation: location,
                                CompanyProfileImage: profileImage,
                                CompanySector: industrySector,
                                postDate: datePosted,
                            }
                            projects.push(projectData);
                        });
                        self.AllProjects(projects);
                    }
                };
                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Companies/Companies/GetSpecificProjects",
                    functionDone, functionFailed);

            }.bind(this);

            if (window.location.pathname === "/Companies/MyProjects") {
                debugger;
                this.getSpecificProjects();
            }
          
            this.getAllProjects = function () {
                debugger;
                var self = this;
                self.hasError(false);
                var functionDone = function (hasError, message, data) {
                    var alertArea = $('#alertArea');
                    if (hasError) {
                        alertArea.addClass('alert-danger').text('Failed! ' + message).show();
                        /*jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);*/
                    } else {
                        debugger;
                        //if (data.IsSuccessful == true) {
                        //    var alertAreaP = $('#alertAreaProjects');
                        //    alertAreaP.addClass('alert-danger').text(data.Message).show();
                        //    setTimeout(function () {
                        //        window.location.reload();
                        //    }, 5000);
                        //}
                        var ProjectsNo = data.Result.length
                        self.NoOfProjects(ProjectsNo);
                        var projects = []
                        data.Result.forEach(function (item) {
                            var id = item.Id;
                            var name = item.ProjectName;
                            var location = item.ProjectLocation;
                            var profileImage = item.AccountEntity.ProfileImageUrl;
                            var industrySector = item.ProjectCategory;
                            var datePosted = item.DatePosted;

                            var projectData = {
                                ProjectId: id,
                                ProjectName: name,
                                CompanyLocation: location,
                                CompanyProfileImage: profileImage,
                                CompanySector: industrySector,
                                postDate: datePosted,
                            }
                            projects.push(projectData);
                        });
                        self.AllProjects(projects);
                    }
                };
                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Companies/Companies/GetAllProjects",
                    functionDone, functionFailed);

            }.bind(this);
            // Check the current URL before calling the function
            if (window.location.pathname !== '/Companies/MyProjects') {
                debugger;
                self.getAllProjects();
            }

            // Function to handle search button click
            this.indexSearch = function () {
                debugger;
                // Get selected industry and location
                var industry = self.selectedIndustry();
                var location = self.selectedLocation();

                // Check if the current page is 'Projects'
                var cPage = window.location.pathname;
                if (cPage == '/User/AllProjects') {                 
                    // Make sure to replace 'searchFunction' with your actual search function
                    self.searchProjects(industry, location);
                }
            };

            self.getUrlParams = function () {
                debugger;
                var params = {};
                var queryString = window.location.search.substring(1);
                var pairs = queryString.split('&');
                for (var i = 0; i < pairs.length; i++) {
                    var pair = pairs[i].split('=');
                    var key = decodeURIComponent(pair[0]);
                    var value = decodeURIComponent(pair[1]);
                    params[key] = value;
                }
                return params;
            }     
       
            this.searchProjects = function (industry, location) {
                debugger;
                var self = this;
                self.searchProjectsCalled = true;
                self.hasError(false);
                var functionDone = function (hasError, message, data) {
                    var alertArea = $('#alertArea');
                    if (hasError) {
                        alertArea.addClass('alert-danger').text('Failed! ' + message).show();
                    } else {
                        if (data.IsSuccessful == true) {
                            var alertAreaP = $('#alertAreaProjects');
                            alertAreaP.addClass('alert-danger').text(data.Message).show();
                        }
                        var ProjectsNo = data.Result.length
                        self.NoOfProjects(ProjectsNo);
                        var projects = []
                        data.Result.forEach(function (item) {
                            var id = item.Id;
                            var name = item.ProjectName;
                            var location = item.ProjectLocation;
                            var profileImage = item.AccountEntity.ProfileImageUrl;
                            var industrySector = item.ProjectCategory;
                            var datePosted = item.DatePosted;

                            var projectData = {
                                ProjectId: id,
                                ProjectName: name,
                                CompanyLocation: location,
                                CompanyProfileImage: profileImage,
                                CompanySector: industrySector,
                                postDate: datePosted,
                            }
                            projects.push(projectData);
                        });
                        self.AllProjects(projects);

                        //// Redirect to the Projects page with selected industry and location
                        //window.location.href = '/Projects?industry=' + industry + '&location=' + location;
                    }
                };
                var functionFailed = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };

                var url = "/Companies/Companies/SearchProjects?industry=" + industry + "&location=" + location;
                jqueryAjaxGenerics.createJSONAjaxGETRequest(url, functionDone, functionFailed);
            };

            // Retrieve search parameters from URL
            var searchParams = self.getUrlParams();
            var industry = searchParams['industry'];
            var location = searchParams['location'];

            // Check if the searchProjects function has already been called
            if (!self.searchProjectsCalled && industry && location) {
                self.searchProjects(industry, location);
            }

            this.getCompanyCandidates = function () {
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
                        var candidates = [];
                        data.Result.forEach(function (item) {
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
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Companies/Companies/GetCompanyCandidates",
                    functionDone, functionFailed);

            }
            this.getCompanyCandidates();

            this.viewProfile = (data, index) => {
                var id = data.CandidateId;
                // Call getCandidateDetails function passing the id
                this.getCandidateDetails(id, index);
            };
            //this.submitSpecificProject = (data) => {
            //    debugger;
            //    var id = data.EntityId.Symbol;
            //    // Call getCandidateDetails function passing the id
            //    this.submitPostProject(id);
            //};

            this.getCandidateDetails = function (id, index) {
                debugger;
                var self = this;
                self.hasError(false);
                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        debugger;
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
                                //var dob = item.AccountEntity.DateOfBirth;
                                //var genderName = item.AccountEntity.Gender.GenderName;
                                var id = item.AccountEntity.Id;

                                self.EntityName(name);
                                self.EntityEmail(email);
                                self.EntityPhone(phone);
                                self.EntityProfileImage(profileImage);
                                self.Location(location);
                                //self.birthDate(dob);
                                //self.gender(genderName);
                                self.EntityId(id);

                                // Store items in local storage
                                localStorage.setItem('EntityName', self.EntityName());
                                localStorage.setItem('EntityEmail', self.EntityEmail());
                                localStorage.setItem('EntityPhone', self.EntityPhone());
                                localStorage.setItem('EntityProfileImage', self.EntityProfileImage());
                                localStorage.setItem('Location', self.Location());
                                //localStorage.setItem('birthDate', self.birthDate());
                                //localStorage.setItem('gender', self.gender());
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
                            window.location.href = "/Companies/CompanyCandidatesDetails";
                        } else {
                            var alertArea = $('#alertArea' + index.toString());
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
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Candidates/Candidates/GetCandidatesDetails?id=" + data,
                    functionDone, functionFailed);

            }

            this.viewCompany = (data, index) => {
                debugger;
                var id = data.CompanyId;
                // Call getCandidateDetails function passing the id
                this.getCompanyDetails(id, index);
            };

            this.viewProject = (data, index) => {
                debugger;
                var id = data.ProjectId;
                // Call getCandidateDetails function passing the id
                this.getProjectDetails(id, index);
            };

            this.viewPostProject = (data, index) => {
                debugger;
                var id = data.ProjectId;
                // Call getCandidateDetails function passing the id
                this.getPostProjectDetails(id, index);
            };

            self.submitApplication = function () {
                debugger;
                /*var self = this;*/
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    var alertArea = $('#alertArea8');
                    debugger;
                    if (hasError) {
                        alertArea.addClass('alert-danger').text('Failed!  ' + data.Message).show();
                        setTimeout(function () {
                            alertArea.hide();
                        }, 10000); // Delay of 10 second
                    } else {
                        if (data.IsSuccessful == true) {
                            alertArea.addClass('alert-success').text('Success!  ' + data.Message).show();
                            setTimeout(function () {
                                alertArea.hide();
                            }, 5000); // Delay of 5 second
                        } else {
                            alertArea.addClass('alert-danger').text('Failed!  ' + data.Message).show();
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
                        alertArea.addClass('alert-danger').text('Failed!  ' + data.Message).show();
                        setTimeout(function () {
                            alertArea.hide();
                        }, 10000); // Delay of 10 second
                    }
                    alertArea.addClass('alert-danger').text('Failed!  ' + data.Message).show();
                    setTimeout(function () {
                        alertArea.hide();
                    }, 10000); // Delay of 10 second
                };
                debugger;

                var projectId = localStorage.getItem('ProjectId');

                var data = {
                    CompanyProjectId: projectId,
                    ApplicationStatus: "Not Reviewed"                   
                };
                debugger;
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Candidates/Candidates/ApplyProject", data,
                    functionDone, functionFailed);
            };

            this.getPostProjectDetails = function (id, index) {
                debugger;
                var self = this;
                self.hasError(false);
                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        var item = data.Result;
                        if (item) {
                            var name = item.ProjectName;
                            var category = item.ProjectCategory;
                            var start = new Date(item.ProjectStart).toLocaleDateString();
                            var end = new Date(item.ProjectEnd).toLocaleDateString();
                            var details = item.ProjectDetails;
                            var open = item.OpenPosition;
                            var work = item.WorkScope;
                            var post = item.PostAs;
                            var recommendation = item.ProjectRecommendation;
                            var companyName = item.AccountEntity.EntityName;
                            var companyEmail = item.AccountEntity.Email;
                            var companyPhone = item.AccountEntity.Phone1
                            var companyLocation = item.ProjectLocation;
                            var companyProfile = item.AccountEntity.ProfileImageUrl;
                            var id = item.Id;
                            var whenPosted = item.DatePosted;
                            var now = new Date();

                            // Calculate the difference in milliseconds
                            var diff = now - whenPosted;

                            // Convert the difference from milliseconds to various units
                            var seconds = Math.floor(diff / 1000);
                            var minutes = Math.floor(seconds / 60);
                            var hours = Math.floor(minutes / 60);
                            var days = Math.floor(hours / 24);
                            var weeks = Math.floor(days / 7);
                            var months = Math.floor(days / 30);
                            var years = Math.floor(days / 365);

                            var timeDiff;

                            // Determine the unit to display based on the magnitude of the difference
                            if (seconds <= 60) {
                                timeDiff = seconds + " seconds";
                            } else if (minutes <= 60) {
                                timeDiff = minutes + " minutes";
                            } else if (hours <= 24) {
                                timeDiff = hours + " hours";
                            } else if (days <= 7) {
                                timeDiff = days + " days";
                            } else if (weeks <= 4) {
                                timeDiff = weeks + " weeks";
                            } else if (months <= 12) {
                                timeDiff = months + " months";
                            } else {
                                timeDiff = years + " years";
                            }

                            // Assign the difference to the observable
                            self.whenPosted(timeDiff);
                                                    
                            var formattedWork = work.replace(/\n/g, "<br>");
                            self.workScope(formattedWork);


                            self.projectName(name);
                            self.projectCategory(category);
                            self.projectDetails(details);
                            self.projectStart(start);
                            self.projectEnd(end);
                            self.openPosition(open);
                            /*self.workScope(work);*/
                            self.projectRecommendation(recommendation);
                            self.postAs(post);
                            self.EntityName(companyName);
                            self.EntityPhone(companyPhone);
                            self.EntityProfileImage(companyProfile);
                            self.CompanyLocation(companyLocation);
                            self.EntityEmail(companyEmail);
                            self.ProjectId(id);

                            // Store items in local storage
                            localStorage.setItem('projectName', self.projectName());
                            localStorage.setItem('projectCategory', self.projectCategory());
                            localStorage.setItem('projectDetails', self.projectDetails());
                            localStorage.setItem('projectStart', self.projectStart());
                            localStorage.setItem('projectEnd', self.projectEnd());
                            localStorage.setItem('openPosition', self.openPosition());
                            localStorage.setItem('workScope', self.workScope());
                            localStorage.setItem('projectRecommendation', self.projectRecommendation());
                            localStorage.setItem('postAs', self.postAs());
                            localStorage.setItem('EntityName', self.EntityName());
                            localStorage.setItem('EntityPhone', self.EntityPhone());
                            localStorage.setItem('EntityProfileImage', self.EntityProfileImage());
                            localStorage.setItem('CompanyLocation', self.CompanyLocation());
                            localStorage.setItem('EntityEmail', self.EntityEmail());
                            localStorage.setItem('ProjectId', self.ProjectId());


                            debugger;
                            if (window.location.pathname === "/Companies/MyProjects") {
                                window.location.href = "/Companies/MyProjectDetails";
                            } else {
                                // Navigation to the CandidatesDetails page
                                window.location.href = "/User/ProjectDetails";
                            }
                            //// Navigation to the CandidatesDetails page
                            //window.location.href = "/User/ProjectDetails";
                        }

                        else {
                            var alertArea = $('#alertArea' + index.toString());
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
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Companies/Companies/GetProjectDetails?id=" + data,
                    functionDone, functionFailed);

            }

            this.getProjectDetails = function (id, index) {
                debugger;
                var self = this;
                self.hasError(false);
                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        var item = data.Result;
                        if (item) {
                            var name = item.ProjectName;
                            var category = item.ProjectCategory;
                            var start = new Date(item.ProjectStart).toLocaleDateString();
                            var end = new Date(item.ProjectEnd).toLocaleDateString();
                            var details = item.ProjectDetails;
                            var open = item.OpenPosition;
                            var work = item.WorkScope;
                            var post = item.PostAs;
                            var recommendation = item.ProjectRecommendation;
                            var companyName = item.AccountEntity.EntityName;
                            var companyEmail = item.AccountEntity.Email;
                            var companyPhone = item.AccountEntity.Phone1
                            var companyLocation = item.AccountEntity.PhysicalAddress;
                            var companyProfile = item.AccountEntity.ProfileImageUrl;
                            var id = item.Id;

                            self.projectName(name);
                            self.projectCategory(category);
                            self.projectDetails(details);
                            self.projectStart(start);
                            self.projectEnd(end);
                            self.openPosition(open);
                            self.workScope(work);
                            self.projectRecommendation(recommendation);
                            self.postAs(post);
                            self.EntityName(companyName);
                            self.EntityPhone(companyPhone);
                            self.EntityProfileImage(companyProfile);
                            self.CompanyLocation(companyLocation);
                            self.EntityEmail(companyEmail);
                            self.ProjectId(id);

                            // Store items in local storage
                            localStorage.setItem('projectName', self.projectName());
                            localStorage.setItem('projectCategory', self.projectCategory());
                            localStorage.setItem('projectDetails', self.projectDetails());
                            localStorage.setItem('projectStart', self.projectStart());
                            localStorage.setItem('projectEnd', self.projectEnd());
                            localStorage.setItem('openPosition', self.openPosition());
                            localStorage.setItem('workScope', self.workScope());
                            localStorage.setItem('projectRecommendation', self.projectRecommendation());
                            localStorage.setItem('postAs', self.postAs());
                            localStorage.setItem('EntityName', self.EntityName());
                            localStorage.setItem('EntityPhone', self.EntityPhone());
                            localStorage.setItem('EntityProfileImage', self.EntityProfileImage());
                            localStorage.setItem('CompanyLocation', self.CompanyLocation());
                            localStorage.setItem('EntityEmail', self.EntityEmail());
                            localStorage.setItem('ProjectId', self.ProjectId());

                            
                            debugger;
                            // Navigation to the CandidatesDetails page
                            window.location.href = "/Admin/ProjectDetails";
                        }

                        else {
                            var alertArea = $('#alertArea' + index.toString());
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
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Companies/Companies/GetProjectDetails?id=" + data,
                    functionDone, functionFailed);

            }

            this.getCompanyDetails = function (id, index) {
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
                                var id = item.AccountEntity.Id;
                                var industrySector = item.AccountEntity.PostalAddress;

                                self.EntityName(name);
                                self.EntityEmail(email);
                                self.EntityPhone(phone);
                                self.EntityProfileImage(profileImage);
                                self.Location(location);
                                self.EntityId(id);
                                self.CompanySector(industrySector);

                                // Store items in local storage
                                localStorage.setItem('EntityName', self.EntityName());
                                localStorage.setItem('EntityEmail', self.EntityEmail());
                                localStorage.setItem('EntityPhone', self.EntityPhone());
                                localStorage.setItem('EntityProfileImage', self.EntityProfileImage());
                                localStorage.setItem('Location', self.Location());
                                localStorage.setItem('EntityId', self.EntityId());
                                localStorage.setItem('CompanySector', self.CompanySector());

                                debugger;
                                switch (keyName) {
                                    case "Mission Statement":
                                        self.mission(value);
                                        localStorage.setItem('mission', value);
                                        break;
                                    case "Vision":
                                        self.vision(value);
                                        localStorage.setItem('vision', value);
                                        break;
                                    case "Core Values":
                                        self.coreValues(value);
                                        localStorage.setItem('coreValues', value);
                                        break;
                                    case "History and Background":
                                        self.history(value);
                                        localStorage.setItem('history', value);
                                        break;
                                    case "Ongoing Projects":
                                        self.ongoingProjects(value);
                                        localStorage.setItem('ongoingProjects', value);
                                        break;
                                    case "Past Projects":
                                        self.pastProjects(value);
                                        localStorage.setItem('pastProjects', value);
                                        break;
                                    case "Technologies or Tools Used":
                                        self.projectTechnologies(value);
                                        localStorage.setItem('projectTechnologies', value);
                                        break;
                                    case "Internship Type":
                                        self.internships(value);
                                        localStorage.setItem('internships', value);
                                        break;
                                    case "Area of Expertise":
                                        self.expertise(value);
                                        localStorage.setItem('expertise', value);
                                        break;
                                    case "Specializations":
                                        self.specialization(value);
                                        localStorage.setItem('specialization', value);
                                        break;
                                    case "Work Environment":
                                        self.workEnvironment(value);
                                        localStorage.setItem('workEnvironment', value);
                                        break;
                                    case "Company Values and Culture":
                                        self.companyValues(value);
                                        localStorage.setItem('companyValues', value);
                                        break;
                                    case "Technologies Used":
                                        self.usedTechnologies(value);
                                        localStorage.setItem('usedTechnologies', value);
                                        break;
                                    case "Innovation Initiatives":
                                        self.innovation(value);
                                        localStorage.setItem('innovation', value);
                                        break;
                                    case "CSR Initiatives or Programs":
                                        self.programs(value);
                                        localStorage.setItem('programs', value);
                                        break;
                                    case "Environmental Sustainability Efforts":
                                        self.environmentalEfforts(value);
                                        localStorage.setItem('environmentalEfforts', value);
                                        break;
                                    case "Industry Awards":
                                        self.awards(value);
                                        localStorage.setItem('awards', value);
                                        break;
                                    case "Certifications":
                                        self.certifications(value);
                                        localStorage.setItem('certifications', value);
                                        break;
                                    case "Internship Programs":
                                        self.internshipPrograms(value);
                                        localStorage.setItem('internshipPrograms', value);
                                        break;
                                    case "Job Openings":
                                        self.jobs(value);
                                        localStorage.setItem('jobs', value);
                                        break;
                                    case "Opportunities for Career Growth and Development":
                                        self.careerOpportunities(value);
                                        localStorage.setItem('careerOpportunities', value);
                                        break;
                                    case "Desired Skills and Qualifications in Interns":
                                        self.desiredSkill(value);
                                        localStorage.setItem('desiredSkill', value);
                                        break;
                                    case "Characteristics or Traits sought in Interns":
                                        self.internsTraits(value);
                                        localStorage.setItem('internsTraits', value);
                                        break;
                                    default:
                                        // Handle default case if needed
                                        break;
                                }

                            });
                            debugger;
                            // Navigation to the CandidatesDetails page
                            window.location.href = "/Admin/CompanyDetails";
                        } else {
                            var alertArea = $('#alertArea' + index.toString());
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
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Candidates/Candidates/GetCompanyDetails?id=" + data,
                    functionDone, functionFailed);

            }

            // Function to retrieve items from local storage and apply them to observables
            this.retrieveProjectDetailsFromLocalStorage = function () {
                var self = this;
                // Iterate over each keyName and retrieve the corresponding value from local storage
                if (window.location.pathname === "/Admin/ProjectDetails" || window.location.pathname === "/User/ProjectDetails" || window.location.pathname === "/Companies/MyProjectDetails") {

                    // Retrieve observables not stored under switch statement
                    self.projectName(localStorage.getItem('projectName'));
                    self.projectCategory(localStorage.getItem('projectCategory'));
                    self.projectDetails(localStorage.getItem('projectDetails'));
                    self.projectStart(localStorage.getItem('projectStart'));
                    self.projectEnd(localStorage.getItem('projectEnd'));
                    self.openPosition(localStorage.getItem('openPosition'));
                    self.workScope(localStorage.getItem('workScope'));
                    self.projectRecommendation(localStorage.getItem('projectRecommendation'));
                    self.postAs(localStorage.getItem('postAs'));
                    self.EntityName(localStorage.getItem('EntityName'));
                    self.EntityPhone(localStorage.getItem('EntityPhone'));
                    self.EntityProfileImage(localStorage.getItem('EntityProfileImage'));
                    self.CompanyLocation(localStorage.getItem('CompanyLocation'));
                    self.EntityEmail(localStorage.getItem('EntityEmail'));

                }

            }.bind(this);

            this.retrieveProjectDetailsFromLocalStorage();

            // Function to retrieve items from local storage and apply them to observables
            this.retrieveCompanyDataFromLocalStorage = function () {
                var self = this;
                // Iterate over each keyName and retrieve the corresponding value from local storage
                if (window.location.pathname === "/Admin/CompanyDetails") {
                    Object.keys(localStorage).forEach(function (keyName) {
                        var value = localStorage.getItem(keyName);
                        // Apply the value to the respective observable based on the keyName
                        switch (keyName) {
                            case "mission":
                                self.mission(value);
                                break;
                            case "vision":
                                self.vision(value);
                                break;
                            case "coreValues":
                                self.coreValues(value);
                                break;
                            case "history":
                                self.history(value);
                                break;
                            case "ongoingProjects":
                                self.ongoingProjects(value);
                                break;
                            case "pastProjects":
                                self.pastProjects(value);
                                break;
                            case "projectTechnologies":
                                self.projectTechnologies(value);
                                break;
                            case "internships":
                                self.internships(value);
                                break;
                            case "expertise":
                                self.expertise(value);
                                break;
                            case "specialization":
                                self.specialization(value);
                                break;
                            case "workEnvironment":
                                self.workEnvironment(value);
                                break;
                            case "companyValues":
                                self.companyValues(value);
                                break;
                            case "usedTechnologies":
                                self.usedTechnologies(value);
                                break;
                            case "innovation":
                                self.innovation(value);
                                break;
                            case "programs":
                                self.programs(value);
                                break;
                            case "environmentalEfforts":
                                self.environmentalEfforts(value);
                                break;
                            case "awards":
                                self.awards(value);
                                break;
                            case "certifications":
                                self.certifications(value);
                                break;
                            case "internshipPrograms":
                                self.internshipPrograms(value);
                                break;
                            case "jobs":
                                self.jobs(value);
                                break;
                            case "careerOpportunities":
                                self.careerOpportunities(value);
                                break;
                            case "desiredSkill":
                                self.desiredSkill(value);
                                break;
                            case "internsTraits":
                                self.internsTraits(value);
                                break;
                            default:
                                // Handle default case if needed
                                break;
                        }
                    });

                    // Retrieve observables not stored under switch statement
                    /*self.EntityId(localStorage.getItem('EntityId'));*/
                    self.EntityName(localStorage.getItem('EntityName'));
                    self.EntityEmail(localStorage.getItem('EntityEmail'));
                    self.EntityPhone(localStorage.getItem('EntityPhone'));
                    self.EntityProfileImage(localStorage.getItem('EntityProfileImage'));
                    self.Location(localStorage.getItem('Location'));
                    self.EntityId(localStorage.getItem('EntityId'));
                    self.CompanySector(localStorage.getItem('CompanySector'));
                }

            }.bind(this);

            this.retrieveCompanyDataFromLocalStorage();

            // Function to retrieve items from local storage and apply them to observables
            this.retrieveCandidateDataFromLocalStorage = function () {
                var self = this;
                // Iterate over each keyName and retrieve the corresponding value from local storage
                if (window.location.pathname === "/Companies/CompanyCandidatesDetails") {
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
                    //self.birthDate(localStorage.getItem('birthDate'));
                    //self.gender(localStorage.getItem('gender'));
                    self.EntityId(localStorage.getItem('EntityId'));
                }

            }.bind(this);

            this.retrieveCandidateDataFromLocalStorage();

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
                        if (data.Result.TypeName == "Admin") {
                            self.AdminMenueVisible(true);
                        }
                        if (data.Result.TypeName == "Intern") {
                            self.InternMenueVisible(true);
                        }
                        if (data.Result.TypeName == "Company") {
                            self.CompanyMenueVisible(true);
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



