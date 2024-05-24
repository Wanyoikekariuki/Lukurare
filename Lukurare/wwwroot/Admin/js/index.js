var MVVM = {
    setNetworksChart: function () {

        /* var $textMutedColor = '#b9b9c3';*/
        debugger;
        var reportChartOptions = {
           
            series: [
                {
                    name: "Water",
                    data: []
                },
                {
                    name: "Rates",
                    data: []
                },
                //{
                //    name: "Telkom",
                //    data: [0, 0, 0, 0, 0, 0, 0]
                //}
            ],
            chart: {
                height: 350,
                type: 'line',
                dropShadow: {
                    enabled: true,
                    color: '#000',
                    top: 18,
                    left: 7,
                    blur: 10,
                    opacity: 0.2
                },
                toolbar: {
                    show: false
                }
            },
            colors: [window.colors.solid.primary, window.colors.solid.warning/*'#b9b9c3', '#ee1b24', '#fff100'*/],//#77B6EA#545454
            dataLabels: {
                enabled: true,
            },
            stroke: {
                curve: 'smooth'
            },
            title: {
                text: 'Last one week Collection',
                align: 'left'
            },
            grid: {
                borderColor: '#e7e7e7',
                row: {
                    colors: ['#f3f3f3', 'transparent'], // takes an array which will be repeated on columns
                    opacity: 0.5
                },
            },
            markers: {
                size: 1
            },
            xaxis: {
                /* categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul'],*/
                title: {
                    text: 'Dates'
                }
            },
            yaxis: {
                title: {
                    text: 'Amount(Ksh)'
                }
            },
            legend: {
                position: 'top',
                horizontalAlign: 'right',
                floating: true,
                offsetY: -25,
                offsetX: -5
            }

        };
        debugger;
        var chart = new ApexCharts(document.querySelector("#networks-chart"), reportChartOptions);
        chart.render();

        var functionDone = function (hasError, message, data) {
            debugger;
            if (hasError) {
                $.alert({ title: 'Failed', icon: 'icon-exclamation', content: message, type: 'red', typeAnimated: true, });
            } else {
                debugger;
                data = data.Result;
                for (i = 0; i < data.length; i++) {
                    for (j = 0; j < data[i].length; j++) {
                        var date = new Date(data[i][j].x);
                        data[i][j].x = moment(date).format('MMM-DD');

                    }


                }
                chart.updateSeries([{
                    name: 'Water',

                    data: data[0]


                },
                {
                    name: 'Rent',

                    data: data[1]


                },
                    //{
                    //    name: 'Telkom',

                    //    data: data[2]


                    //}



                ])
            }
            //data;///data contains the result

        };
        var functionFailed = function (hasError, message, data) {
            debugger;
            if (hasError) {
                jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
            }
            //jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);


        };
        debugger;
        //var data = {
        //    StartDate: '',
        //    EndDate: ''
        //};
        jqueryAjaxGenerics.createJSONAjaxGETRequest(/*"POST",*/ "/DashBoard/DashBoard/GetCharts",/* data,*/
            functionDone, functionFailed);
    },

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

        debugger;
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("/ChatHub")
            .withAutomaticReconnect()
            .configureLogging(signalR.LogLevel.Information)
            .withHubProtocol(new signalR.JsonHubProtocol())
            .build();


        connection.serverTimeoutInMilliseconds = 60000; // 60 seconds  
        debugger;
        // Start the connection
        connection.start().then(() => {
            console.log("Connected to the SignalR hub.");
        }).catch(err => console.error(err.toString()));

        var viewModel = function () {
            debugger;

            this.hasError = ko.observable(false);
            this.errorMessage = ko.observable();
            this.Balance = ko.observable(0);
            this.PhotoLinkUrl = ko.observable();
            this.ParentPhone = ko.observable();
            this.ParentEmail = ko.observable();
            this.Brand = ko.observable();
            this.NationalIDNumber = ko.observable();
            this.ParentIDNumber = ko.observable();
            this.GoodConductNumber = ko.observable();
            this.KraPinNumber = ko.observable();
            this.PassPortNumber = ko.observable();
            this.ChildsBirthCertificateNumber = ko.observable();
            this.SpouseIDNumber = ko.observable();
            this.BirthCertificateNumber = ko.observable();

            this.MissingDocuments = ko.observableArray([]);

            this.SelectedDocument = ko.observable();
            this.NationalIDVisible = ko.observable(false);
            this.PassportVisible = ko.observable(false);
            this.ParentIDVisible = ko.observable(false);
            this.BirthCertificateVisible = ko.observable(false);
            this.PassportPhotoVisible = ko.observable(false);
            this.ChildBirthCertificateVisible = ko.observable(false);
            this.AcademicDocumentsVisible = ko.observable(false);
            this.SpouseIDVisible = ko.observable(false);
            this.GoodConductCertificateVisible = ko.observable(false);
            this.MedicalExaminationCertificateVisible = ko.observable(false);
            this.CurriculumVitaeVisible = ko.observable(false);
            this.CovidVaccineCertificateVisible = ko.observable(false);
            this.AreaChiefLetter = ko.observable(false);
            this.BankStatementVisible = ko.observable(false);
            this.LanguageTestCertificateVisible = ko.observable(false);
            this.BiometricsVisible = ko.observable(false);
            this.PaySlipVisible = ko.observable(false);
            this.RecommendationLetterVisible = ko.observable(false);
            this.KRAPinVisible = ko.observable(false);
            this.ProofofFundsVisible = ko.observable(false);
            this.ProofofLandOwnership = ko.observable(false);
            this.ProofofVehicleOwnership = ko.observable(false);
            this.ProofofBusinessOwnership = ko.observable(false);
            this.AllfilesUploadedVisible = ko.observable(false);
            this.FilesTobeUploaded = ko.observable(true);
            this.LabourExportPermitVisible = ko.observable(false);
            this.BusinessPermitVisible = ko.observable(false);
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
            this.selectedAgent = ko.observable();
            this.selectedCountryOrigin = ko.observable();
            this.countryCitizenship = ko.observable();
            this.selectedCurrentCountry = ko.observable();
            this.selectedCountryTravelling = ko.observable();
            this.Agents = ko.observableArray([]);
            this.Countries = ko.observableArray([]);
            this.SelectAgentVisible = ko.observable(false);
            this.SelectCountryVisible = ko.observable(false);
            this.AgentName = ko.observable("");
            this.AgentEmail = ko.observable("");
            this.AgentPhone = ko.observable("");
            this.AgentLocation = ko.observable("");
            this.UserContactList = ko.observable([]);
            this.selectedContact = ko.observable(null);
            this.tempMessagesStorage = ko.observable({});
            this.chatMessages = ko.observableArray([]);
            this.appendedChatElements = [];
            this.timeStamp = ko.observable("0");
            this.newChatCount = ko.observable(0);
            this.chatName = ko.observable();
            this.selectCont = ko.observable();
            this.toastMessage = ko.observable("");
            this.toastHeader = ko.observable("");
            this.ProfileImageLink = ko.observable("");
            this.onlineStatus = ko.observable(false);
            this.totalRetainerFeeCollection = ko.observable(0);
            this.totalNumberofAgents = ko.observable(0);
            this.totalNumberofCustomers = ko.observable(0);
            this.totalApplicationFeeCollection = ko.observable(0);
            this.totalOtherPaymentCollection = ko.observable(0);

            this.productNameInput = ko.observable();
            this.productPriceInput = ko.observable();

            this.setSelectedDocument = function (data, event) {
                debugger;
                var self = this;
                self.SelectedDocument(event.target.id);
            }



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


            this.checkFloatAccount = function () {
                debugger;
                var self = this;
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        debugger;
                        //self.Balance(data.balance);
                        //$.confirm({
                        //    title: 'Confirm!',
                        //    content: 'Simple confirm!',
                        //    buttons: {
                        //        confirm: function () {
                        //            $.alert('Confirmed!');
                        //        },
                        //        cancel: function () {
                        //            $.alert('Canceled!');
                        //        },
                        //        somethingElse: {
                        //            text: 'Something else',
                        //            btnClass: 'btn-blue',
                        //            keys: ['enter', 'shift'],
                        //            action: function () {
                        //                $.alert('Something else?');
                        //            }
                        //        }
                        //    }
                        //})

                    }
                    //data;///data contains the result

                };
                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    //jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);

                };
                debugger;

                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Accounts/ResellerFloatAccounts/CheckFloatAccountExists",
                    functionDone, functionFailed);

            }

            // this.checkFloatAccount();
            this.checkBalance = function () {
                debugger;
                var self = this;
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        debugger;
                        self.Balance(data.balance);

                    }
                    //data;///data contains the result

                };
                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    //jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);


                };
                debugger;

                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Accounts/AccountProfiles/GetBalance",
                    functionDone, functionFailed);

            }


            // this.checkBalance();

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

            this.totalSummary = function () {
                debugger;
                var self = this;
                //this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        debugger;
                        var summary = data.Result;
                        debugger;

                        self.totalNumberofAgents(summary.totalNumberofAgents);
                        self.totalNumberofCustomers(summary.totalNumberofCustomers);
                        self.totalApplicationFeeCollection(summary.totalApplicationFeeCollection);
                        self.totalRetainerFeeCollection(summary.totalRetainerFeeCollection);
                        self.totalOtherPaymentCollection(summary.totalOtherPaymentCollection);


                    }

                };
                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    //jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);


                };
                debugger;

                jqueryAjaxGenerics.createJSONAjaxGETRequest("/DashBoard/DashBoard/GetDashBoardSummary",
                    functionDone, functionFailed);

            }
            this.totalSummary();


            this.OnSubmit = function () {
                debugger;
                var self = this;
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    else {
                        self.onImport();
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
                    ImageDecName: self.productNameInput(),
                    ImageDecprice: self.productPriceInput()
                };
                debugger;
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/DashBoard/Dashboard/SubmitUserUploadDetails", data,
                    functionDone, functionFailed);

            }

            this.onImport = function () {
                debugger;
                var self = this;
                MVVM.blockUiPageElementWithMessage($('body'), 'Uploading your image .... Please wait');
                var formData = new FormData();
                var fileInput = document.getElementById(self.SelectedDocument());
                var file = fileInput.files[0];
                var name = self.productNameInput();

                formData.append("name", self.productNameInput());
                formData.append("file", file, self.productNameInput(), name);

                if (file && (file.type === 'image/png' || file.type === 'image/jpeg' || file.type === 'image/jpg')) {
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
                    jqueryConfirmGenerics.showOkAlertBox('Failed', "Please upload a .png, .jpg, .jpeg file.", "red", null);
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

            this.populateContactList = function () {
                debugger;
                var self = this;
                
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, 'red', null);
                    }
                    else
                    { 
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
            }
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
            
            this.refreshChat = function (selectedContact) {
                debugger;
                var self = this;

                this.checkOnlineStatus(selectedContact.Id);
                //let selectedContact = parseInt(sessionStorage.getItem("ContactSelected"));
                //var messages = self.tempMessagesStorage();
                var contacts = self.UserContactList();
                debugger;
                var Contact = null;

                for (var i = 0; i < contacts.length; i++) {
                    if (contacts[i].Id === selectedContact.Id) {
                        Contact = contacts[i];
                        Contact.chatCount(0);
                        break; // Stop iterating once a match is found
                    }
                }
                debugger;
                var messageArray = Contact.messages() || [];
                /*messageArray.push(message);*/
                /* messageArray.push({ text: message })*/
                debugger;
                messageArray.forEach(function (message) {

                    debugger;
                    // Create a new chat message element
                    var chatDiv = document.createElement("div");
                    chatDiv.className = "chat";
                    var chatBody = document.createElement("div");
                    chatBody.className = "chat-body";
                    var chatContent = document.createElement("div");
                    chatContent.className = "chat-content";
                    var messageParagraph = document.createElement("p");
                    messageParagraph.textContent = message; // Assuming message is a property in the message object
                    chatContent.appendChild(messageParagraph);
                    chatBody.appendChild(chatContent);
                    chatDiv.appendChild(chatBody);
                    debugger;
                    // Append the chat message to the messagesContainer
                    var container = document.getElementById("messagesContainer");
                    container.appendChild(chatDiv);

                    // Tracking the appended chat element
                    appendedChatElements.push(chatDiv);

                    // You may also want to scroll to the bottom of the chat container to show the latest message
                    var userChatsContainer = document.getElementById("user-chats");
                    if (userChatsContainer) {
                        userChatsContainer.scrollTop = userChatsContainer.scrollHeight;
                    }
                });
                Contact.messages([]);
            }   

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
            this.SendOnEnter = function (event) {
                var self = this;
                if (event.keyCode === 13) {
                    debugger;
                    // Call your sendChat function or perform any desired action here
                    self.sendChat(); // Make sure your sendChat function is defined and handles the message sending
                }
            }

            // Update the sendChat function to invoke SendMessageToUser
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


            debugger;
            this.checkOnlineStatus = function (senderId) {
                debugger;

                connection.invoke("CheckOnlineStatus", senderId);
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
            this.offline = function (message,senderId) {
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

            

            this.storeQuestionnaireResponse = function () {
                debugger;
                //var self = this;
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
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
                    //IDNumber: self.NationalIDNumber(),
                    //ParentIDNumber: self.ParentIDNumber(),
                    //BirthCertificateNumber: self.BirthCertificateNumber(),
                    //ChildsBirthCertificateNumber: self.ChildsBirthCertificateNumber(),
                    //SpouseIDNumber: self.SpouseIDNumber(),
                    //GoodConductNumber: self.GoodConductNumber(),
                    //KRAPinNumber: self.KraPinNumber(),
                    //PassPortNumber: self.PassPortNumber()

                    Age: this.customerAge(),
                    HighestLevelOfEducation: this.highestLevelOfEducation(),
                    HowDidYouKnowOfUs: this.howDidYouKnowOfUs()
                };
                debugger;
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/DashBoard/Dashboard/SubmitUserUploadDetails", data,
                    functionDone, functionFailed);

            };

            this.checkQuestionnaireQuizOneFilled = function () {
                debugger;
                if (this.birthDate() == null) {
                    this.warningMessageVisibility(true);
                    return false;
                }
                this.warningMessageVisibility(false);

                return true;
            };
            this.checkQuestionnaireQuizTwoFilled = function () {
                debugger;
                if (this.highestLevelOfEducation() == null) {
                    this.warningMessageVisibility(true);
                    return false;
                }

                this.warningMessageVisibility(false);
                return true;
            };
            this.checkQuestionnaireQuizThreeFilled = function () {
                debugger;
                if (this.howDidYouKnowOfUs() == null) {
                    this.warningMessageVisibility(true);
                    return false;
                }
                this.warningMessageVisibility(false);
                return true;
            };

            // Function to handle changes in the date picker and populate the observable
            this.handleDateChange = function () {
                // Access the selected date from the input element
                var selectedDate = document.getElementById("birthday").value;

                // Update the Knockout observable with the selected date
                this.birthDate(selectedDate);
            }.bind(this);

            this.toggleQuestionnaireVisibility = function (isVisibile) {
                debugger;
                this.questionnaireVisibility(isVisibile);
                this.toggleStepOneQuestionnaireVisibility(isVisibile);

                var modal = document.getElementById("myModal");
                modal.classList.remove("fade");
                modal.style.display = "block";
            };

            this.toggleStepOneQuestionnaireVisibility = function (isVisibile) {
                debugger;
                this.stepOneQuestionnaireVisibility(isVisibile);
                this.btnQuestionnaireNext1Visibility(isVisibile);
            }
            this.toggleStepTwoQuestionnaireVisibility = function (isVisibile) {
                debugger;
                this.stepTwoQuestionnaireVisibility(isVisibile);
                this.btnQuestionnaireNext2Visibility(isVisibile);
                this.btnQuestionnairePrevious2Visibility(isVisibile);
            }
            this.toggleStepThreeQuestionnaireVisibility = function (isVisibile) {
                debugger;
                this.stepThreeQuestionnaireVisibility(isVisibile);
                this.btnQuestionnaireNext3Visibility(isVisibile);
                this.btnQuestionnairePrevious3Visibility(isVisibile);
            }

            
            this.displayTopupMessage = function () {
                var self = this;
                jqueryConfirmGenerics.showOkAlertBox("Information", "As we work on automating account topup, kindly contact " + self.ParentEmail() + " or " + self.ParentPhone() + " to load your wallet", "green", null);
            }

            //The modal doesn't close before one submits information
            window.onclick = function (event) {
                if (event.target == modal) {
                    this.questionnaireVisibility(true)
                }
            }

            // Logic for stepping through the questionnaire
            this.btnQuestionnaireNext1Action = function () {
                debugger;

                if (this.checkQuestionnaireQuizOneFilled()) {
                    this.toggleStepOneQuestionnaireVisibility(false);
                    this.toggleStepTwoQuestionnaireVisibility(true);
                    this.toggleStepThreeQuestionnaireVisibility(false);
                    this.endQuestionnaireVisibility(false);
                }
            };

            this.btnQuestionnaireNext2Action = function () {
                debugger;

                if (this.checkQuestionnaireQuizTwoFilled()) {
                    this.toggleStepOneQuestionnaireVisibility(false);
                    this.toggleStepTwoQuestionnaireVisibility(false);
                    this.toggleStepThreeQuestionnaireVisibility(true);
                    this.endQuestionnaireVisibility(false);
                }
            };

            this.btnQuestionnairePrevious2Action = function () {
                debugger;

                this.toggleStepOneQuestionnaireVisibility(true);
                this.toggleStepTwoQuestionnaireVisibility(false);
                this.toggleStepThreeQuestionnaireVisibility(false);
                this.endQuestionnaireVisibility(false);
            };

            this.btnQuestionnaireNext3Action = function () {
                debugger;
                if (this.checkQuestionnaireQuizThreeFilled()) {
                    this.storeQuestionnaireResponse();

                    this.toggleStepOneQuestionnaireVisibility(false);
                    this.toggleStepTwoQuestionnaireVisibility(false);
                    this.toggleStepThreeQuestionnaireVisibility(false);

                    this.questionnaireTitle(false);
                    this.questionnaireSubtitle(false);
                    this.endQuestionnaireVisibility(true);

                    setTimeout(() => {
                        debugger;
                        this.endQuestionnaireVisibility(false);
                        this.questionnaireVisibility(false);

                        var modal = document.getElementById("myModal");
                        modal.classList.add("fade");
                        modal.style.display = "none";

                        location.reload();
                    }, 2000);
                }
            };

            this.btnQuestionnairePrevious3Action = function () {
                debugger;

                this.toggleStepOneQuestionnaireVisibility(false);
                this.toggleStepTwoQuestionnaireVisibility(true);
                this.toggleStepThreeQuestionnaireVisibility(false);
                this.endQuestionnaireVisibility(false);
            };

        }
        var myModel = new viewModel();
        ko.applyBindings(myModel);

        debugger;
        connection.on("ReceiveMessage", function (senderId, message) {
            debugger;

            //If sender comes online and receiver is still online unhide the message input box and hide the banner
            myModel.populateChatMessages(senderId, message);
   
        });


        connection.on("UserOffline", function (message, senderId) {
            debugger;
            myModel.offline(message, senderId);
        });


        
        connection.on("UserOnline", function (senderId) {
            debugger;
            myModel.online(senderId);
        });

    }
};

$(document).ready(function () {
    debugger;
    MVVM.init();
    //MVVM.initCharts();

});