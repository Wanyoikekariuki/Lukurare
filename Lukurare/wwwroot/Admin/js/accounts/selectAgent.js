    var MVVM = {
        blockUiPageElement: function (element) {
            return MVVM.blockUiPageElementWithMessage(element, 'Please wait...');
        },

        blockUiPageElementWithMessage: function (element, message) {
        //element.block
        $.blockUI({
            message:
                '<div class="d-flex justify-content-center align-items-center"><p class="me-50 mb-0">' + message + '</p> <div class="spinner-grow spinner-grow-sm text-white" role="status"></div> </div>',
            css: {
                backgroundColor: 'transparent',
                color: '#fff',
                border: '0'
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
            const connection = new signalR.HubConnectionBuilder()
                .withUrl("/ChatHub")
                .withAutomaticReconnect()
                //.configureLogging(signalR.LogLevel.Information)
                .withHubProtocol(new signalR.JsonHubProtocol())
                .build();


            connection.serverTimeoutInMilliseconds = 60000; // 60 seconds  

            // Start the connection
            connection.start().then(() => {
                //console.log("Connected to the SignalR hub.");
            }).catch(err => console.error(err.toString()));
        var viewModel = function () {


            this.hasError = ko.observable(false);
            this.selectedAgent = ko.observable();
            this.selectedCountryOrigin = ko.observable();
            this.countryCitizenship = ko.observable();
            this.selectedCurrentCountry = ko.observable();
            this.selectedCountryTravelling = ko.observable();
            this.Agents = ko.observableArray([]);
            this.Countries = ko.observableArray([]);
            this.errorMessage = ko.observable();
            this.EntityUserName = ko.observable();
            this.Phone1 = ko.observable();
            this.Email = ko.observable();
            this.Id = ko.observable(0);
            this.selectedOption = ko.observable(true);
            this.PhotoLinkUrl = ko.observable();
            this.EntityUserName2 = ko.observable();
            this.Phone12 = ko.observable();
            this.Email2 = ko.observable();
            this.color = ko.observable("red");
            this.Brand = ko.observable();
            this.ParentPhone = ko.observable();
            this.ParentEmail = ko.observable();
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
            this.AgentName = ko.observable();
            this.AgentEmail = ko.observable();
            this.AgentPhone = ko.observable();
            this.AgentLocation = ko.observable();
            this.Citizenship = ko.observable();
            this.OriginCountry = ko.observable();
            this.CountryCurrent = ko.observable();
            this.TravelCountry = ko.observable();
            this.YourPhone = ko.observable();
            this.YourEmail = ko.observable();
            this.YourLocation = ko.observable();
            this.YourNationalIDNumber = ko.observable();
            this.YourName = ko.observable();
            this.CurrentId = ko.observable();
            this.Documents = ko.observableArray([]);
            this.Status = ko.observable('');
            this.UserContactList = ko.observable([]);
            this.selectedContact = ko.observable(null);
            this.tempMessagesStorage = ko.observable({});
            this.chatMessages = ko.observableArray([]);
            var appendedChatElements = [];
            this.timeStamp = ko.observable("0");
            this.newChatCount = ko.observable(0);
            this.chatName = ko.observable();
            this.selectCont = ko.observable();
            this.toastMessage = ko.observable("");
            this.toastHeader = ko.observable("");
            this.ProfileImageLink = ko.observable("");
            this.onlineStatus = ko.observable(false);
            this.Age = ko.observable();
            this.HighestLevelOfEducation = ko.observable();

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

            this.makeSelectionInvisible = function (obj, event) {
                debugger;


                if (event.originalEvent) { //user changed
                    debugger;
                    this.selection(false);
                } else { // program changed
                    debugger;
                }

            }
            this.makeNoSelectionInvisible = function (obj, event) {
                debugger;


                if (event.originalEvent) { //user changed
                    debugger;
                    this.noSelection(false);
                } else { // program changed
                    debugger;
                }

            }

            this.clearForm = function () {
                debugger;
                this.selectedAgent();
            } 
                    
            this.chooseAgent = function () {
                debugger;
                var self = this;
                self.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        debugger;
                        self.Agents(data.Result);

                    }
                };
                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Accounts/SelectAgent/SelectAgent",
                    functionDone, functionFailed);

            }
            this.chooseAgent();

            this.agentDetails = function () {
                debugger;
                var self = this;
                self.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        debugger;
                        var agent = data.Result;

                        self.AgentName(agent.SubAccount.AccountName);
                        self.AgentEmail(agent.SubAccount.Email);
                        self.AgentPhone(agent.SubAccount.Phone);
                        self.AgentLocation(agent.BuildingAddress);
                    }
                       
                   
                };
                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Accounts/SelectAgent/AgentDetails",
                    functionDone, functionFailed);

            }
            this.agentDetails();

            this.customerInfo = function () {
                debugger;
                var self = this;
                self.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        debugger;
                        var customer = data.Result
                        if (customer.length == 0) {                            
                            return;
                        }
                        self.Citizenship(customer[1]?.Value || 'null');                        
                        self.OriginCountry(customer[0]?.Value || 'null');
                        self.CountryCurrent(customer[3]?.Value || 'null');
                        self.TravelCountry(customer[2]?.Value || 'null');
                        self.Age(customer[4]?.Value || 'null');
                        self.HighestLevelOfEducation(customer[5]?.Value || 'null');
                        self.YourPhone(customer[1]?.AccountEntity.Phone1 || 'null')
                        self.YourEmail(customer[1]?.AccountEntity.Email || 'null')
                        self.YourName(customer[1]?.AccountEntity.EntityName || 'null')
                        self.CurrentId(customer[1]?.AccountEntity.Id || 'null')
                        self.YourLocation(customer[1]?.AccountEntity.PhysicalAddress || 'null')
                        self.YourNationalIDNumber(customer[1]?.AccountEntity.IdentificationDocumentNumber || 'null')
                      
                    }


                };
                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Accounts/SelectAgent/CustomerInfo",
                    functionDone, functionFailed);

            }
            this.customerInfo();

            this.validate = function () {
                debugger;
                var self = this;
                if (!self.Agents()) {
                    jqueryConfirmGenerics.showOkAlertBox('Validation Failed!', "Agent is required", "red", null);
                    return false;
                }
               

                return true;
            }

            this.submitAgentDetails = function () {

                debugger;
                var self = this;

                var result = self.validate();
                if (result === false) {
                    MVVM.unBlockUiPageElement($('body'));
                    return;
                }
                MVVM.blockUiPageElementWithMessage($('body'), 'Processing.. Please wait');


                var functionDone = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else if (data.IsOkay == false) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed', data.Message, "red", null);
                        self.clearForm();
                    }
                    else {
                        if (data.Message === 'Selected agent Successfully.') {
                            jqueryConfirmGenerics.showOkAlertBox('You have Successfully choosen your Agent!', data.Message, "green", null);
                        } else {
                            jqueryConfirmGenerics.showOkAlertBox('You have already chosen an agent!', data.Message, "red", null);
                            self.clearForm();
                        }

                        //jqueryConfirmGenerics.showOkAlertBox('You have Successfully choosen your Agent!', data.Message, "green", null);
                        //self.clearForm();

                    }

                };
                var functionFailed = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }

                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };
                debugger;

                debugger;
                var data = self.selectedAgent();

                debugger;

                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Accounts/SelectAgent/SubmitAgent", data,
                    functionDone, functionFailed);

            }

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

                var self = this;
                this.hasError(false);
                var functionDone = function (hasError, message, data) {

                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {

                        //self.Documents(data.Result);

                        self.Brand(readStringUntilSpace(data.Result.Result[0].EntityName));
                        self.PhotoLinkUrl(data.Result.Result[0].ProfileImageUrl);


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

                data = {

                };
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Accounts/AccountProfiles/GetKendoGridFiltered", data,
                    functionDone, functionFailed);

            }

            this.checkProfile();

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



            this.chooseCountry = function () {
                debugger;
                var self = this;
                self.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        var checkCountry = data.Result;
                        self.Countries(data.Result);
                    }
                };
                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Accounts/SelectAgent/SelectCountry",
                    functionDone, functionFailed);

            }
            this.chooseCountry();


            this.CheckCountrySuplied = function () {
                debugger;
                var self = this;
                if (!self.countryCitizenship() || self.countryCitizenship() === 0) {
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', "Enter Your Citizenship", "red", null);
                    return false;
                }
                if (!self.selectedCountryOrigin() || self.selectedCountryOrigin() === 0) {
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', "Enter Your Country of Origin", "red", null);
                    return false;
                }
                if (!self.selectedCurrentCountry() || self.selectedCurrentCountry() === 0) {
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', "Enter Your Current Country", "red", null);
                    return false;
                }
                if (!self.selectedCountryTravelling() || self.selectedCountryTravelling() === 0) {
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', "Enter the Country You are Travelling To", "red", null);
                    return false;
                }
                return true;
            }                     

            this.uploadedDocuments = function (id) {
                debugger;
                var self = this;
                self.hasError(false);
                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        var documents = [];
                        data.Result.forEach(function (item) {
                            // Accessing the DocumentName and Path properties from the nested object
                            var id = item.Id;
                            var validated = item.Validated;
                            var documentName = item.RequiredDocuments.IdentificationDocumentType.DocumentName;
                            var path = item.Path;
                            var status = validated ? 'Approved' : 'Not Approved';
                            // Creating an object to hold both DocumentName and Path properties
                            var documentData = {
                                Id: id,
                                DocumentName: documentName,
                                Path: path,
                                Validated: validated,
                                Status: status

                                /*docComment: ko.observable("")*/
                            };
                            documents.push(documentData);
                            $('#documentTable').DataTable();
                        });
                        debugger;
                        self.Documents(documents);
                    }
                };
                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Accounts/SelectAgent/UploadedDocuments?id=" + id,
                    functionDone, functionFailed);

            }
            this.uploadedDocuments();

            this.editUser = function () {
                debugger;

                var self = this;
                self.color("red");
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        self.color("green");
                        jqueryConfirmGenerics.showOkAlertBox('Success!', data.Message, "green", null);
                        
                        window.location.reload();

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

                var data = {
                    Id: self.CurrentId(),
                    EntityName : self.YourName(),
                    Phone1: self.YourPhone(),
                    Email: self.YourEmail(),
                    PhysicalAddress: self.YourLocation(),
                    IdentificationDocumentNumber: self.YourNationalIDNumber(),
                    IsActive : true

                };
                debugger;
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("PUT", "/Accounts/SelectAgent/Update", data,
                    functionDone, functionFailed);


            }

            this.OnSubmit = function () {
                debugger;
                var self = this;
                var result = self.CheckCountrySuplied();

                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        jqueryConfirmGenerics.showOkAlertBox('Success!', data.Message, "green", null);
                       // $('.datatables-basic').DataTable().ajax.reload();

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
                var data = 
                {
                    CountryOrigin: self.selectedCountryOrigin().CountryName,
                    CountryCitizenship: self.countryCitizenship(),
                    CountryTravelling: self.selectedCountryTravelling().CountryName,
                    CurrentCountry: self.selectedCurrentCountry().CountryName
                };

               
                debugger;
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/DashBoard/Dashboard/SubmitUserUploadDetails", data,
                    functionDone, functionFailed);

            }



            this.truncateMessage = function (message, maxLength) {
                if (message.length > maxLength) {
                    return message.substring(0, maxLength) + "...";
                }
                return message;
            };

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
                            ;

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
            };
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
                };

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

            this.SendMessageNotification = function () {
                debugger;
                var self = this;
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        jqueryConfirmGenerics.showOkAlertBox('Success!', data.Message, "green", null);
                        /*$('.datatables-basic').DataTable().ajax.reload();*/

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
                var self = this;
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        jqueryConfirmGenerics.showOkAlertBox('Success!', data.Message, "green", null);
                        /*$('.datatables-basic').DataTable().ajax.reload();*/

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

            this.uploadedDocuments = function (id) {
                debugger;
                var self = this;
                self.hasError(false);
                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        var documents = [];
                        data.Result.forEach(function (item) {
                            // Accessing the DocumentName and Path properties from the nested object
                            var id = item.Id;
                            var validated = item.Validated;
                            var documentName = item.RequiredDocuments.IdentificationDocumentType.DocumentName;
                            var path = item.Path;
                            var status = validated ? 'Approved' : 'Not Approved';
                            // Creating an object to hold both DocumentName and Path properties
                            var documentData = {
                                Id: id,
                                DocumentName: documentName,
                                Path: path,
                                Validated: validated,
                                Status: status
                                
                            };
                            documents.push(documentData);
                            $('#documentTable').DataTable();
                        });
                        debugger;
                        self.Documents(documents);
                    }
                };
                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Accounts/SelectAgent/UploadedDocuments?id=" + id,
                    functionDone, functionFailed);

            }
            this.uploadedDocuments();

            this.OnSubmit = function () {
                debugger;
                var self = this;
                var result = self.CheckCountrySuplied();

                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        jqueryConfirmGenerics.showOkAlertBox('Success!', data.Message, "green", null);
                       // $('.datatables-basic').DataTable().ajax.reload();

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
                var data = 
                {
                    CountryOrigin: self.selectedCountryOrigin().CountryName,
                    CountryCitizenship: self.countryCitizenship(),
                    CountryTravelling: self.selectedCountryTravelling().CountryName,
                    CurrentCountry: self.selectedCurrentCountry().CountryName
                };

               
                debugger;
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/DashBoard/Dashboard/SubmitUserUploadDetails", data,
                    functionDone, functionFailed);

            }

            /*Chat js */
            


            this.populateContactList = function () {
                debugger;
                var self = this;
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, 'red', null);
                    } else {
                        var contactList = [];
                        if (data.Result != null && data.Result.length >= 1) {
                            data.Result.forEach(function (item) {
                                debugger;
                                var userName = item.Username;
                                var userId = item.Id;
                                var username = item.Username;
                                var contactName = item.AccountEntity.EntityUserName;
                                var contactPhone = item.AccountEntity.Phone1;
                                var contactId = item.AccountEntity.Id;
                                var profileImage = item.AccountEntity.ProfileImageUrl;

                                var contactData = {
                                    Name: userName,
                                    Id: userId,
                                    UserName: username,
                                    EntityName: contactName,
                                    Phone1: contactPhone,
                                    ContactId: contactId,
                                    ProfileImage: profileImage,
                                    messages: ko.observableArray([]),
                                    chatCount: ko.observable(0),
                                    isOnline: ko.observable(false)
                                    //IsExpanded: ko.observable(false), // Add observable for expanded state
                                    //IsRead: ko.observable(false) // Add observable for read state
                                };
                                debugger;
                                contactList.push(contactData);
                            });
                        }
                            self.UserContactList(contactList);
                        
                        // Update the newNotificationCount based on the fetched notifications
                        // self.newNotificationCount(self.UserNotifications().length);
                    }
                }
                //this.populateContactList();
                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, 'red', null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, 'red', null);
                };

                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Hubs/Notification/GetContactList",
                    functionDone, functionFailed);
            };
            this.populateContactList();
            this.selectContact = function (selectedContact) {
                // debugger;
                var self = this;

                this.checkOnlineStatus(selectedContact.Id);
                self.selectCont = selectedContact;
                sessionStorage.setItem("ContactSelected", selectedContact.Id);
                self.ProfileImageLink(selectedContact.ProfileImage);
                document.getElementById("ChatBrand").innerHTML = selectedContact.Name;
                self.refreshChat(selectedContact);
            }.bind(this);



            this.populateChatMessages = function (senderId, message) {
                debugger;

                var self = this;



                //let selectedContact = parseInt(sessionStorage.getItem("ContactSelected"));
                var contacts = self.UserContactList();
                var Contact = null;

                //from the user contact list it matches the contact and the sender so as to append the incoming messages
                for (var i = 0; i < contacts.length; i++) {
                    if (contacts[i].Id === senderId) {
                        Contact = contacts[i];
                        break; // Stop iterating once a match is found
                    }
                }

                var messagesArray = Contact.messages() || [];
                //var temp = Contact.tempMessages() || [];
                messagesArray.push(message);
                // Contact.tempMessages(messagesArray);
                Contact.messages(messagesArray);

                debugger;

                if (!sessionStorage.getItem("ContactSelected")) {
                    //do something if no contact is selected
                    self.toastHeader(Contact.Name);
                    self.toastMessage(message);
                    $('#mytoast').toast('show');
                    //this.newChatCount(this.newChatCount() + 1);
                    Contact.chatCount(Contact.chatCount() + 1);
                    return;
                }
                var a = sessionStorage.getItem("ContactSelected");
                var selectedContact = parseInt(a);
                if (senderId !== selectedContact) {
                    //do something if no the selected contact is not the one active
                    self.toastHeader(Contact.Name);
                    self.toastMessage(message);
                    $('#mytoast').toast('show');
                    //this.newChatCount(this.newChatCount() + 1);
                    Contact.chatCount(Contact.chatCount() + 1);
                    return;
                }
                else {
                    this.checkOnlineStatus(senderId);
                    var a = sessionStorage.getItem("ContactSelected");
                    var selectedContact = parseInt(a);
                    if (senderId === selectedContact) {
                        var messagesArray = Contact.messages() || [];
                        /*messageArray.push(message);*/
                        /* messageArray.push({ text: message })*/
                        debugger;
                        messagesArray.forEach(function (message) {
                            // Create a new chat message element
                            var chatDiv = document.createElement("div");
                            chatDiv.className = "chat";
                            var chatBody = document.createElement("div");
                            chatBody.className = "chat-body";
                            var chatContent = document.createElement("div");
                            chatContent.className = "chat-content";
                            var messageParagraph = document.createElement("p");
                            messageParagraph.textContent = message;
                            chatContent.appendChild(messageParagraph);
                            chatBody.appendChild(chatContent);
                            chatDiv.appendChild(chatBody);

                            // Append the chat message to the messagesContainer
                            var container = document.getElementById("messagesContainer");
                            container.appendChild(chatDiv);

                            // Tracking the appended chat element
                            appendedChatElements.push(chatDiv);

                            // You may also want to scroll to the bottom of the chat container to show the latest message
                            container.scrollTop = container.scrollHeight;
                        });
                    }
                    Contact.messages([]);
                }
                debugger;
            }
            this.OnlineBadge = function () {
                debugger;
                var self = this;
                var contacts = self.UserContactList();
                debugger;
                //from the user contact list it matches the contact and the sender so as to append the incoming messages
                for (var i = 0; i < contacts.length; i++) {
                    Contact = contacts[i];
                    self.checkOnlineStatus(Contact.Id);
                    debugger;
                }
            }

            this.sendChat = function () {
                debugger;
                // Get the selected contact


                var a = sessionStorage.getItem("ContactSelected");

                let selectedContact = parseInt(a);

                this.checkOnlineStatus(selectedContact);

                debugger;
                // Get the message from the input field
                var message = document.getElementById("messageInput").value;



                if (message.trim() !== "") {
                    messageInput.value = "";
                    // Invoke the SendMessageToUser method on the SignalR connection
                    connection.invoke("SendMessageToUser", selectedContact, message)
                        .then(function () {
                            // Message sent successfully
                            console.log("message sent");
                            // Create a new chat message element
                            var chatDiv = document.createElement("div");
                            chatDiv.className = "chat-left";
                            var chatBody = document.createElement("div");
                            chatBody.className = "chat-body";
                            var chatContent = document.createElement("div");
                            chatContent.className = "chat-content";
                            var messageParagraph = document.createElement("p");
                            messageParagraph.textContent = message;
                            chatContent.appendChild(messageParagraph);
                            chatBody.appendChild(chatContent);
                            chatDiv.appendChild(chatBody);

                            // Append the chat message to the messagesContainer
                            var container = document.getElementById("messagesContainer");
                            container.appendChild(chatDiv);

                            appendedChatElements.push(chatDiv);
                            // Scroll to the bottom of the chat container to show the latest message
                            var userChatsContainer = document.getElementById("user-chats");
                            if (userChatsContainer) {
                                userChatsContainer.scrollTop = userChatsContainer.scrollHeight;
                            }
                        })

                        .catch(function (err) {
                            return console.error(err.toString());
                        });

                }

            }.bind(this);

            debugger;
            // Listen for error messages
            connection.on("ReceiveErrorMessage", function (errorMessage) {
                // Display the error message to the user (you can replace this with your UI logic)
                alert("Error: " + errorMessage);
            });

            this.checkOnlineStatus = function (senderId) {
                debugger;

                connection.invoke("CheckOnlineStatus", senderId);
            }

            this.offline = function (message, senderId) {
                debugger;
                var self = this;
                document.getElementById("messageInput").disabled = true;
                document.getElementById("sendchatbtn").disabled = true;

                var formInputDiv = document.getElementById("formInput");
                formInputDiv.style.display = "none";

                var warningElement = document.getElementById("warningMessage");
                warningElement.style.display = "block";
                warningElement.textContent = message;

                this.onlineStatus(false);


                var self = this;
                var contacts = self.UserContactList();
                var Contact = null;
                debugger;
                //from the user contact list it matches the contact and the sender so as to append the incoming messages
                for (var i = 0; i < contacts.length; i++) {
                    if (contacts[i].Id == senderId) {
                        Contact = contacts[i];
                        Contact.isOnline(false);
                    }
                    debugger;
                }
            }
            this.online = function (senderId) {
                debugger;
                document.getElementById("warningMessage").disabled = true;
                document.getElementById("messageInput").disabled = false;
                document.getElementById("sendchatbtn").disabled = false;

                var formInputDiv = document.getElementById("formInput");
                formInputDiv.style.display = "flex";

                var warningElement = document.getElementById("warningMessage");
                warningElement.style.display = "none";

                this.onlineStatus(true);

                var self = this;
                var contacts = self.UserContactList();
                var Contact = null;
                debugger;
                //from the user contact list it matches the contact and the sender so as to append the incoming messages
                for (var i = 0; i < contacts.length; i++) {
                    if (contacts[i].Id == senderId) {
                        Contact = contacts[i];
                        Contact.isOnline(true);
                    }
                    debugger;
                }
            }

            this.removeLastChatElement = function () {
                debugger;
                var self = this;
                var a = sessionStorage.getItem("ContactSelected");
                var selectedContact = parseInt(a);
                self.checkOnlineStatus(selectedContact);
                sessionStorage.removeItem("ContactSelected");
                self.newChatCount(0);
                var container = document.getElementById("messagesContainer");
                $('#chats').modal('hide');
                // Check if there are any elements to remove
                while (appendedChatElements.length > 0) {
                    var lastChatElement = appendedChatElements.pop();
                    container.removeChild(lastChatElement);
                }
            }
            /**END of chat js*/

        }
        var myModel = new viewModel();
        ko.applyBindings(myModel);

        connection.on("ReceiveMessage", function (senderId, message) {
            debugger;


            myModel.populateChatMessages(senderId, message);
           
        });

        connection.on("UserOffline", function (message) {
            debugger;
            myModel.offline(message);

        });



        connection.on("UserOnline", function () {
            debugger;
            myModel.online();


        });
       
      
    }
};


$(document).ready(function () {

    $('#documentTable').DataTable({
        "lengthMenu": [10, 25, 50, 75, 100],  // Number of entries per page options
        "pageLength": 25,  // Default number of entries per page
        "searching": true,  // Enable search feature
        "ordering": true,   // Enable column sorting
        "responsive": true, // Enable responsive feature
    });
    //Contacts();
    MVVM.init();
});
