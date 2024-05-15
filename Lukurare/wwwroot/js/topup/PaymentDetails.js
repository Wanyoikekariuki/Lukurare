
var topUp = {
    blockUiPageElement: function (element) {
        return topUp.blockUiPageElementWithMessage(element, 'Please wait...');
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

    myModelUI: null,
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
           // console.log("Connected to the SignalR hub.");
        }).catch(err => console.error(err.toString()));

        try {
          
            var viewModel = function () {
                this.hasError = ko.observable(false);
                this.selection = ko.observable(true);
                this.noSelection= ko.observable(true);
                this.errorMessage = ko.observable();
                this.Amount = ko.observable();
                this.PhoneNumber = ko.observable();
                this.PhotoLinkUrl = ko.observable();
                this.Services = ko.observableArray([]);
                this.Contacts = ko.observableArray([]);
                this.selectedContact = ko.observable();
                this.selectedService = ko.observable();
                this.Balance = ko.observable();
                this.color = ko.observable("red");
                this.Brand = ko.observable();
                this.ParentPhone = ko.observable();
                this.ParentEmail = ko.observable();
                //this.AccountNumber = ko.observable();
                this.selectedQuoteCurrency = ko.observable();
                this.selectedBaseCurrency = ko.observable();
                this.Currencies = ko.observableArray();
                this.Paymode = ko.observableArray([]);
                this.selectedPaymode = ko.observable();
                this.PaymentPurpose = ko.observable();
                this.ReferenceNumber = ko.observable();
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
                this.AccountTypes = ko.observableArray([]);
                this.SelectedAccountType = ko.observable(1);
                this.RetainerFeeView = ko.observable(false);
                this.RetainerFeeStatusView = ko.observable(false);
                this.RetainerFeeBalance = ko.observable();
                this.RetainerFeeStatus = ko.observable();
                this.PaymentPurposeView = ko.observable(false);
                this.RetainerFeeOverpaid = ko.observable(false);
                this.ApplicationFeeBalanceView = ko.observable(false);
                this.ApplicationFeeBalance = ko.observable();

                this.isApplicationFeeSelected = ko.pureComputed(function () {
                    return this.SelectedAccountType === 5;
                })
                this.accountName = ko.observable();
                this.ApplicationFeeStatus = ko.observable();
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
                    var self = this;
                    //self.SelectedAccountType("");
                    self.PaymentPurpose('');
                    self.Amount('');
                    self.ReferenceNumber('');
                }


                this.showErrorMessage = function (title, message) {
                    debugger;
                    try {
                        //this.hasError(true);
                        this.errorMessage(title + ':' + message);


                    }
                    catch (err) {
                        jqueryConfirmGenerics.showOkAlertBox(title, err.message,"red",null);
                    }
                }
                this.checkBalance = function () {
                    debugger;
                    var self = this;
                    //self.hasError(false);
                    var functionDone = function (hasError, message, data) {
                        debugger;
                        if (hasError) {
                            jqueryConfirmGenerics.showOkAlertBox('Failed!', message,"red",null);
                        } else {
                            self.Balance(data.balance.toString());
                        }
                        //data;///data contains the result

                    };
                    var functionFailed = function (hasError, message, data) {
                        debugger;
                        if (hasError) {
                            jqueryConfirmGenerics.showOkAlertBox('Failed!', message,"red",null);
                        }
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);

                    };
                    debugger;

                    jqueryAjaxGenerics.createJSONAjaxGETRequest("/Accounts/AccountProfiles/GetBalance",
                        functionDone, functionFailed);

                }

                this.checkCurrencies = function () {
                   debugger;
                    var self = this;
                    //self.hasError(false);
                    var functionDone = function (hasError, message, data) {
                        debugger;
                        if (hasError) {
                            jqueryConfirmGenerics.showOkAlertBox('Failed!', message,"red",null);
                        } else {
                            self.Currencies(data.Result);
                            self.selectedBaseCurrency(data.Result[0]);
                            self.selectedQuoteCurrency(data.Result[2]);
                        }

                    };
                    var functionFailed = function (hasError, message, data) {
                        debugger;
                        if (hasError) {
                            jqueryConfirmGenerics.showOkAlertBox('Failed!', message,"red",null);
                        }
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);

                    };
                  

                    jqueryAjaxGenerics.createJSONAjaxGETRequest("/Accounts/MeterAccount/GetCurrency",
                        functionDone, functionFailed);

                }
                this.checkCurrencies();

                this.checkPaymode = function () {
                    debugger;
                    var self = this;
                    //self.hasError(false);
                    var functionDone = function (hasError, message, data) {
                        debugger;
                        if (hasError) {
                            jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                        } else {
                            self.Paymode(data.Result);
                            self.selectedPaymode(data.Result[0]);
                            //self.selectedBaseCurrency(data.Result[0]);
                            //self.selectedQuoteCurrency(data.Result[2]);
                        }

                    };
                    var functionFailed = function (hasError, message, data) {
                        debugger;
                        if (hasError) {
                            jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                        }
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);

                    };


                    jqueryAjaxGenerics.createJSONAjaxGETRequest("/Accounts/MeterAccount/GetPaymode",
                        functionDone, functionFailed);

                }

                this.checkPaymode();

                this.checkAccountTypes = function () {
                    debugger;
                    var self = this;
                    //self.hasError(false);
                    var functionDone = function (hasError, message, data) {
                        debugger;

                        if (hasError) {
                            jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                        }
                        else {
                            data.Result.forEach(function (item) {

                                if (item.AccountTypeName == "Float") {
                                    return item.AccountTypeName = "Others";
                                }
                            })  

                            self.AccountTypes(data.Result);
                            //self.SelectedAccountType();


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

                    jqueryAjaxGenerics.createJSONAjaxGETRequest("/Accounts/MeterAccount/GetAccountType",
                        functionDone, functionFailed);

                }
                this.checkAccountTypes();


                this.AccountBalances = function (newValue) {

                    debugger;
                    var self = this;
                    if (newValue === "Retainer Fee") {//retainer
                        this.PaymentPurposeView(false);
                        this.ApplicationFeeBalanceView(false);
                        this.RetainerFeeStatusView(true);
                        this.RetainerFeeOverpaid(false);
                        //this.RetainerFeeView(true);
                        debugger;
                    }
                    if (newValue === "Application Fee") {//application
                        this.PaymentPurposeView(false);
                        this.ApplicationFeeBalanceView(true)
                        this.RetainerFeeStatusView(false);
                        this.RetainerFeeOverpaid(false);
                        debugger;
                    }
                    if (newValue === "Others") {//other
                        this.PaymentPurposeView(true);
                        this.ApplicationFeeBalanceView(false)
                        this.RetainerFeeStatusView(false);
                        this.RetainerFeeOverpaid(false);
                        debugger;
                    }

                    var functionDone = function (hasError, message, data) {
                        debugger;
                        if (hasError) {
                            jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                        }
                        else {
                            if (data.Result.Balance > 0) {
                                self.RetainerFeeOverpaid(true);
                                self.RetainerFeeStatusView(false);
                            }
                            var retainerBalance = Math.abs(data.Result.Balance);
                            self.RetainerFeeBalance(retainerBalance);
                            var retainerStatus = data.Result.Status;
                            self.RetainerFeeStatus(retainerStatus);
                            var applicationBalance = Math.abs(data.Result.Balance);
                            self.ApplicationFeeBalance(applicationBalance);
                            var applicationStatus = data.Result.Status;
                            self.ApplicationFeeStatus(applicationStatus);
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
                    var modelToSend =
                    {
                        AccountEntityId: 0,
                        MfsAccountType:
                        {
                            AccountTypeName: newValue
                        }
                    };
                    jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Accounts/SelectAgent/GetCustomerBalance", modelToSend, functionDone, functionFailed);

                }
                this.SelectedAccountType.subscribe(function (newValue) {
                    debugger;
                    var self = this;
                    this.AccountBalances(newValue)
                }, this);

                this.checkContacts = function () {
                    debugger;
                    var self = this;
                    //self.hasError(false);
                    var functionDone = function (hasError, message, data) {
                        debugger;
                        if (hasError) {
                            jqueryConfirmGenerics.showOkAlertBox('Failed!', message,"red",null);
                        } else {
                            self.Currencies(data.Result);
                        }
                        //data;///data contains the result

                    };
                    var functionFailed = function (hasError, message, data) {
                        debugger;
                        if (hasError) {
                            jqueryConfirmGenerics.showOkAlertBox('Failed!', message,"red",null);
                        }
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);

                    };
                    debugger;

                    jqueryAjaxGenerics.createJSONAjaxGETRequest("Accounts/MeterAccount/GetCurrency",
                        functionDone, functionFailed);

                }
                //this.checkContacts();
                this.validate = function () {
                    debugger;
                    var self = this;
                    if (!self.Amount()  || self.Amount() === 0) {
                        jqueryConfirmGenerics.showOkAlertBox('Validation Failed!', "Amount figure is required","red",null);
                        return false;
                    }
                    if (!self.ReferenceNumber() ) {
                        jqueryConfirmGenerics.showOkAlertBox('Validation Failed!', "Reference Number is required", "red", null);
                        return false;
                    }

                    return true;
                }
                this.submitTopupRequest = function () {
                    
                    debugger;
                    var self = this;

                    var result = self.validate();
                    if (result === false) {
                        topUp.unBlockUiPageElement($('body'));
                        return;
                    }
                    topUp.blockUiPageElementWithMessage($('body'), 'Processing.. Please wait');

                       
                    var functionDone = function (hasError, message, data) {
                        topUp.unBlockUiPageElement($('body'));
                        debugger;
                        if (hasError) {
                            jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                        } else if (data.IsOkay == false) {
                            jqueryConfirmGenerics.showOkAlertBox('Failed', data.Message, "red", null);
                            self.clearForm();
                            
                        }
                    else
                        {
                            var newValue = self.SelectedAccountType();
                            self.AccountBalances(newValue);//On success to update the balance
                           jqueryConfirmGenerics.showOkAlertBox('Success!', data.Message,"green",null);
                            self.clearForm();
                           

                        }

                    };
                    var functionFailed = function (hasError, message, data) {
                        topUp.unBlockUiPageElement($('body'));
                        debugger;
                        if (hasError) {
                            jqueryConfirmGenerics.showOkAlertBox('Failed!', message,"red",null);
                        }

                        jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                    };
                    debugger;
      
                  
                    var data = {
                        AccountName: self.SelectedAccountType(),
                        PaymentPurpose: self.PaymentPurpose(),
                        Amount:self.Amount(),
                        ReferenceNumber: self.ReferenceNumber(),
                        BaseCurrency: self.selectedBaseCurrency().CurrencyCode,
                        PaymodeId: self.selectedPaymode().Id,
                        Id: 0
    
                    };
                    debugger;
                   
                    jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Accounts/Payments/Add", data,
                        functionDone, functionFailed);
                }
                
                this.setUpGlobalVariables = function () {
                    debugger;
                    var self = this;
                   // this.hasError(false);
                    var functionDone = function (hasError, message, data) {
                        debugger;
                        if (hasError) {
                            jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                        } else {
                            debugger;

                            self.ParentPhone(data.Phone1);

                            self.ParentEmail(data.Email);

                        }
 
                    };
                    var functionFailed = function (hasError, message, data) {
                        debugger;
                        if (hasError) {
                            jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                        }

                        self.ParentPhone(data.Phone1);

                        self.ParentEmail(data.Email);

                    };
                    debugger;
                    data = {

                    };
                    jqueryAjaxGenerics.createJSONAjaxNonGETRequest("GET", "/Accounts/AccountProfiles/GetParentDetails",
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

                    var functionDone = function (hasError, message, data) {
                      
                        if (hasError) {
                            jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                        } else {
                           
 
                            self.Brand(readStringUntilSpace(data.Result.Result[0].EntityName));
                            self.PhotoLinkUrl(data.Result.Result[0].ProfileImageUrl);

                        }

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

                this.displayTopupMessage = function () {
                    var self = this;
                    jqueryConfirmGenerics.showOkAlertBox("Information", "As we work on automating account topup, kindly contact " + self.ParentEmail() + " or " + self.ParentPhone() + " to load your wallet", "green", null);
                }

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
                    self.ProfileImageLink(selectedContact.ProfileImage);
                    sessionStorage.setItem("ContactSelected", selectedContact.Id);
                    document.getElementById("ChatBrand").innerHTML = selectedContact.Name;
                    self.refreshChat(selectedContact);
                }.bind(this);


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
                                var userChatsContainer = document.getElementById("user-chats");
                                if (userChatsContainer) {
                                    userChatsContainer.scrollTop = userChatsContainer.scrollHeight;
                                }
                            });
                        }
                        Contact.messages([]);
                    }
                    debugger;
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

               
                connection.on("ReceiveErrorMessage", function (errorMessage) {
                    // Display the error message to the user (you can replace this with your UI logic)
                    alert("Error: " + errorMessage);
                });

                this.checkOnlineStatus = function (senderId) {
                    debugger;

                    connection.invoke("CheckOnlineStatus", senderId);
                }

                this.online = function () {
                    debugger;
                    document.getElementById("warningMessage").disabled = true;
                    document.getElementById("messageInput").disabled = false;
                    document.getElementById("sendchatbtn").disabled = false;

                    var formInputDiv = document.getElementById("formInput");
                    formInputDiv.style.display = "flex";

                    var warningElement = document.getElementById("warningMessage");
                    warningElement.style.display = "none";

                    this.onlineStatus(true);

                }
                this.offline = function (message) {
                    debugger;
                    document.getElementById("messageInput").disabled = true;
                    document.getElementById("sendchatbtn").disabled = true;

                    var formInputDiv = document.getElementById("formInput");
                    formInputDiv.style.display = "none";

                    var warningElement = document.getElementById("warningMessage");
                    warningElement.style.display = "block";
                    warningElement.textContent = message;

                    this.onlineStatus(false);
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
            }

            var myModel = new viewModel();
            var obj=ko.applyBindings(myModel);

            topUp.myModelUI = obj;

            connection.on("ReceiveMessage", function (senderId, message) {
                debugger;

                myModel.populateChatMessages(senderId, message);
            });

            connection.on("UserOffline", function (message,senderId) {
                debugger;
                myModel.offline(message,senderId);

            });



            connection.on("UserOnline", function (senderId) {
                debugger;
                myModel.online(senderId);


            });

        }
        catch (err) {
            topUp.unBlockUiPageElement($('body'));
            jqueryConfirmGenerics.showOkAlertBox('Error Init Single topup', err.message,"red",null);
        }
    }
};

$(document).ready(function () {
   
    topUp.init();
});



