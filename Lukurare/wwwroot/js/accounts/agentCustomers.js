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

    //myModelUI: null,

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
        var viewModel = function () {
            debugger;

            this.hasError = ko.observable(false);
            this.errorMessage = ko.observable();
            this.selection = ko.observable(true);
            this.noSelection = ko.observable(true);
            this.EntityUserName = ko.observable();
            this.Phone1 = ko.observable();
            this.Email = ko.observable();
            this.Balance = ko.observable();
            this.Id = ko.observable(0);
            this.selectedOption = ko.observable(true);
            this.PhotoLinkUrl = ko.observable();
            this.EntityUserName2 = ko.observable();
            this.Documents = ko.observableArray([]);
            this.Details = ko.observableArray([]);
            this.AdditionalDets = ko.observableArray([]);
            this.Email2 = ko.observable();
            this.color = ko.observable("red");
            this.Brand = ko.observable();
            this.ParentPhone = ko.observable();
            this.ParentEmail = ko.observable();
            this.MeterNumber = ko.observable();
            this.EntityName = ko.observable();
            this.EntityId = ko.observable();
            this.PhysicalAddress = ko.observable("");
            this.ParentPhysicalAddress = ko.observable("");
            this.AccountTypes = ko.observableArray([]);
            this.SelectedAccountType = ko.observable();
            this.DocumentComment = ko.observable();
            this.CustomerData = ko.observable();
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
            this.selectedQuoteCurrency = ko.observable();
            this.selectedBaseCurrency = ko.observable();
            this.Currencies = ko.observableArray([]);
            this.Paymode = ko.observableArray([]);
            this.selectedPaymode = ko.observable();
            this.PaymentPurpose = ko.observable();
            this.ReferenceNumber = ko.observable();
            this.Amount = ko.observable();
            this.rowData = ko.observable(null);
            this.validatedDoc = ko.observable(false);
            this.DocData = ko.observable();
            this.color = ko.observable("red");
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
            this.loggedInEntity = ko.observable(false);
            this.AccountTypes = ko.observableArray([]);
            this.SelectedAccountType = ko.observable(1);
            this.RetainerFeeView = ko.observable(false);
            this.RetainerFeeStatusView = ko.observable(false);
            this.RetainerFeeBalance = ko.observable();
            this.PaymentPurposeView = ko.observable(false);
            this.RetainerFeeStatus = ko.observable();
            this.RetainerFeeOverpaid = ko.observable(false);
            this.ApplicationFeeBalanceView = ko.observable(false);
            this.ApplicationFeeBalance = ko.observable();
            this.ApplicationFeeStatus = ko.observable();
            this.accountName = ko.observable();

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

            this.clearForm = function () {
                debugger;
                var self = this;
                //self.SelectedAccountType('');
                self.PaymentPurpose('');
                self.Amount('');
                self.ReferenceNumber('');
            }

            this.checkBalance = function () {
                debugger;
                var self = this;
                //self.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        self.Balance(data.balance.toString());
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

                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Accounts/AccountProfiles/GetBalance",
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

            this.additionalDetails = function (id) {
                debugger;
                var self = this;
                self.hasError(false);
                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        var additionaldet = [];
                        data.Result.forEach(function (item) {
                            // Accessing the KeyName and Value properties from the nested object
                            var name = item.TypeAdditionalDetails.KeyName;
                            var value = item.Value;
                            // Creating an object to hold both KeyName and Value properties
                            var additionalData = {
                                KeyName: name,
                                Value: value,
                            };
                            additionaldet.push(additionalData);
                        });
                        debugger;
                        self.AdditionalDets(additionaldet);

                    }
                };
                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Accounts/SelectAgent/AdditionalDetails?id=" + id,
                    functionDone, functionFailed);

            }


            this.checkCurrencies = function () {

                var self = this;
                //self.hasError(false);
                var functionDone = function (hasError, message, data) {

                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        self.Currencies(data.Result);
                        self.selectedBaseCurrency(data.Result[0]);
                        self.selectedQuoteCurrency(data.Result[2]);
                    }

                };
                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);

                };


                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Accounts/MeterAccount/GetCurrency",
                    functionDone, functionFailed);

            }
            this.checkCurrencies();

            this.checkPaymode = function () {

                var self = this;
                //self.hasError(false);
                var functionDone = function (hasError, message, data) {

                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        self.Paymode(data.Result);
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

                debugger;
                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Accounts/MeterAccount/GetPaymode",
                    functionDone, functionFailed);

            }
            this.checkPaymode();

            this.validate = function () {
                debugger;
                var self = this;
                if (!self.Amount() || self.Amount() === 0) {
                    jqueryConfirmGenerics.showOkAlertBox('Validation Failed!', "Amount figure is required", "red", null);
                    return false;
                }
                if (!self.ReferenceNumber()) {
                    jqueryConfirmGenerics.showOkAlertBox('Validation Failed!', "Reference Number is required", "red", null);
                    return false;
                }

                return true;
            }

            this.submitPaymentDetails = function () {

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
                        var newValue = self.SelectedAccountType();
                        self.AccountBalances(newValue);

                        jqueryConfirmGenerics.showOkAlertBox('Success!', data.Message, "green", null);
                        self.clearForm();

                    }
                    setTimeout(() => {

                        location.reload();
                    }, 2000);
                };
                var functionFailed = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }

                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                    self.clearForm();
                };
                debugger;

                debugger;
                var data = {
                    AccountName: self.SelectedAccountType(),
                    PaymentPurpose: self.PaymentPurpose(),
                    Amount: self.Amount(),
                    ReferenceNumber: self.ReferenceNumber(),
                    BaseCurrency: self.selectedBaseCurrency().CurrencyCode,
                    PaymodeId: self.selectedPaymode().Id,
                    Id: self.rowData().Id // Access rowData properties

                };
                debugger;

                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Accounts/Payments/Add", data,
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
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Hubs/Notification/Send", data,
                    functionDone, functionFailed);

            }.bind(this);

            /*Chat js */
            const connection = new signalR.HubConnectionBuilder()
                .withUrl("/ChatHub")
                .withAutomaticReconnect()
                //.configureLogging(signalR.LogLevel.Information)
                .withHubProtocol(new signalR.JsonHubProtocol())
                .build();


            connection.serverTimeoutInMilliseconds = 60000; // 60 seconds  

            // Start the connection
            connection.start().then(() => {
                console.log("Connected to the SignalR hub.");
            }).catch(err => console.error(err.toString()));


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

            /*END of chat js */

            this.approveDocument = function (id) {
                debugger;
                var self = this;
                self.color("red");
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else if (data.Message === 'The document has been Approved Successfully.') {
                        self.color("green");
                        jqueryConfirmGenerics.showOkAlertBox('Success!', data.Message, "green", null);
                        $('.datatables-basic').DataTable().ajax.reload();
                        window.location.reload();
                    }
                    else {
                        self.color("red");
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                        $('.datatables-basic').DataTable().ajax.reload();
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

                //var data = {
                //    Id: self.Documents().Id,
                //    Validated: self.Documents().Validated,
                //    //PaymentPurpose: self.PaymentPurpose(),
                //    //    Amount: self.Amount(),
                //    //        ReferenceNumber: self.ReferenceNumber(),
                //    //            BaseCurrency: self.selectedBaseCurrency().CurrencyCode,
                //    //                PaymodeId: self.selectedPaymode().Id,
                //    //                    Id: 0

                //};
                var data = id;
                debugger;
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Accounts/SelectAgent/ConfirmDocument", data,
                    functionDone, functionFailed);

            }.bind(this);

            this.declineDocument = function (id) {
                debugger;
                var self = this;
                self.color("red");
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else if (data.Message === 'The document has been Declined Successfully.') {
                        self.color("green");
                        jqueryConfirmGenerics.showOkAlertBox('Success!', data.Message, "green", null);
                        $('.datatables-basic').DataTable().ajax.reload();
                        window.location.reload();
                    }
                    else {
                        self.color("red");
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                        $('.datatables-basic').DataTable().ajax.reload();
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

                var data = id;
                debugger;
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("DELETE", "/Accounts/SelectAgent/DeclineDocument", data,
                    functionDone, functionFailed);

            }.bind(this);


            this.toggleMessage = function (notification) {
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

            this.checkBalance = function () {
                debugger;
                var self = this;
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        self.Balance(data.balance.toString());
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

                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Accounts/AccountProfiles/GetBalance",
                    functionDone, functionFailed);
            }

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
                //was done because on loading the rowData is null. rowData is populated after payment modal is loaded
                if (self.rowData() != null) {
                    var modelToSend =
                    {
                        AccountEntityId: self.rowData().Id,
                        MfsAccountType:
                        {
                            AccountTypeName: self.SelectedAccountType()
                        }
                    };

                    jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Accounts/SelectAgent/GetCustomerBalance", modelToSend, functionDone, functionFailed);
                }
            }
            this.SelectedAccountType.subscribe(function (newValue) {
                debugger;
                var self = this;
                this.AccountBalances(newValue);
            }, this);

            //this.checkAccountTypes = function () {
            //    debugger;
            //    var self = this;
            //    //self.hasError(false);
            //    var functionDone = function (hasError, message, data) {
            //        debugger;
            //        if (hasError) {
            //            jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
            //        } else {
            //            self.AccountTypes(data.Result);
            //            self.SelectedAccountType(data.Result[2]);

            //        }

            //    };
            //    var functionFailed = function (hasError, message, data) {
            //        debugger;
            //        if (hasError) {
            //            jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
            //        }
            //        jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);

            //    };
            //    debugger;

            //    jqueryAjaxGenerics.createJSONAjaxGETRequest("/Accounts/SelectAgent/GetAccountType",
            //        functionDone, functionFailed);

            //}
            //this.checkAccountTypes(); 
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

            this.addUser = function () {
                debugger;
                var self = this;
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        jqueryConfirmGenerics.showOkAlertBox('Success!', data.Message, "green", null);
                        $('.datatables-basic').DataTable().ajax.reload();
                        self.EntityUserName("");
                        self.Phone1("");
                        self.Email("");
                        self.selectedOption(true);
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



                /*  *//* if (selected == undefined) {*/
                var data = {
                    Id: self.Id(),
                    EntityUserName: self.EntityUserName(),
                    Phone1: self.Phone1(),
                    Email: self.Email(),
                    IsActive: self.selectedOption()
                };
                debugger;
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Accounts/SelectAgent/Add", data,
                    functionDone, functionFailed);

            }
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
                        $('.datatables-basic').DataTable().ajax.reload();
                        self.EntityUserName2("");
                        self.Phone1("");
                        self.Email2("");
                        self.MeterNumber("");
                        self.selectedOption(true);


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
                    Id: self.Id(),
                    EntityUserName: self.EntityUserName2(),
                    Phone1: self.Phone1(),
                    Email: self.Email2(),
                    IsActive: self.selectedOption()
                };
                debugger;
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("PUT", "/Accounts/SelectAgent/Update", data,
                    functionDone, functionFailed);


            }
            this.addMeter = function () {
                debugger;
                var self = this;
                if (!self.MeterNumber()) {
                    jqueryConfirmGenerics.showOkAlertBox('Validation Failed!', "Meter Number is required", "red", null);

                    return;
                }
                self.color("red");
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        self.color("green");
                        jqueryConfirmGenerics.showOkAlertBox('Success!', data.Message, "green", null);
                        $('.datatables-basic').DataTable().ajax.reload();
                        self.MeterNumber("");
                    }
                }
                //end of function done

                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);

                }
                debugger;
                var data = {

                    MfsAccountTypeId: self.SelectedAccountType().Id,
                    AccountNumber: self.MeterNumber(),
                    AccountName: self.EntityName(),

                    AccountEntity: {

                        EntityName: self.EntityName(),
                        Phone1: self.Phone1(),

                        PhysicalAddress: self.ParentPhysicalAddress(),

                    }
                };
                debugger;
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Accounts/MeterAccount/Add", data,
                    functionDone, functionFailed);

            }

            this.submitComment = function (item) {
                debugger;
                var self = this;
                self.hasError(false);
                // Retrieve the phone number from myModel.CustomerData()
                var customerData = myModel.CustomerData(); // Retrieve the observable
                var receiverNo = customerData && customerData.Phone1 ? customerData.Phone1 : null;
                var receiverEmail = customerData && customerData.Email ? customerData.Email : null;

                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        self.clearForm();
                        jqueryConfirmGenerics.showOkAlertBox('Message sent Successfully!', data.Message, "green", null);
                    }
                };
                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };

                var data = {
                    SenderNo: '',
                    ReceiverNo: receiverNo,
                    Title: "Document Clarification",
                    Message: item.DocumentComment,
                    ReceiverEmail: receiverEmail,
                };

                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Hubs/Notification/Send", data,
                    functionDone, functionFailed);

            }.bind(this);


            this.OnImport = function () {
                debugger;
                var formData = new FormData();
                var totalFiles = document.getElementById("fileToUpload").files.length;
                for (var i = 0; i < totalFiles; i++) {
                    var file = document.getElementById("fileToUpload").files[i];
                    formData.append("fileToUpload", file);
                }

                $.ajax({
                    type: 'POST',
                    url: '/Contacts/AirtimeGroupContacts/UploadContacts',
                    dataType: 'json',
                    processData: false,
                    contentType: false,
                    data: formData,
                    success: function (data) {
                        MVVM.unBlockUiPageElement($('body'));
                        if (data.IsOkay) {
                            $('.datatables-basic').DataTable().ajax.reload();
                            jqueryConfirmGenerics.showOkAlertBox('Success', data.Message, "green", null);

                        }
                        else {
                            jqueryConfirmGenerics.showOkAlertBox('Failed', data.Message, "red", null);

                        }
                    },
                    error: function (err) {
                        MVVM.unBlockUiPageElement($('body'));
                        jqueryConfirmGenerics.showOkAlertBox('Failed', err.message, "red", null);
                        debugger;

                    }

                });

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
                    self.ParentPhysicalAddress(data.PhysicalAddress);
                    self.ParentEmail(data.Email);

                    //jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
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
            //this.claimCommision = function (id, entityname, username, phone, address, email, status, balance) {
            //    debugger;
            //    var self = this;
            //    MVVM.blockUiPageElementWithMessage($('body'), 'Sending your claim to the admin.. Please wait');
            //    this.hasError(false);

            //    var functionDone = function (hasError, message, data) {
            //        debugger;
            //        if (hasError) {
            //            jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
            //        } else {
            //            jqueryConfirmGenerics.showOkAlertBox('Success!', data.Message, "green", null);

            //            /*$('.datatables-basic').DataTable().ajax.reload();*/

            //            MVVM.unBlockUiPageElement($('body'));
            //        }
            //    };
            //    var functionFailed = function (hasError, message, data) {
            //        debugger;
            //        MVVM.unBlockUiPageElement($('body'));
            //        if (hasError) {
            //            jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
            //        }
            //        jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
            //    };

            //    var data = {
            //        Id: id,
            //        EntityName: entityname,
            //        Phone: phone,
            //        //Status: status,
            //        Email:email,
            //        Address: address,
            //        UserName: username,
            //        AccountName: "",
            //        Balance: { "Retainer": balance },
            //        Status: { "Retainer": status }
            //    };
            //    jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Hubs/Notification/SendClaim", data,functionDone, functionFailed);
            //}

            this.claimCommision = function (item) {
                debugger;
                var self = this;
                MVVM.blockUiPageElementWithMessage($('body'), 'Sending your claim to the admin.. Please wait');
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

                var data = {
                    Id: item.Id,
                    EntityName: item.EntityName,
                    Email: item.Email,
                    Phone1: item.Phone1,
                    Address: item.PhysicalAddress,
                    EntityNo: item.EntityNo,
                    Phone2: item.Phone2
                };
                debugger;
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Accounts/SelectAgent/SendClaim", data, functionDone, functionFailed);

            }

            this.reverseTransaction = function (item) {
                debugger;
                var self = this;
                MVVM.blockUiPageElementWithMessage($('body'), 'Sending your Reversal to the admin.. Please wait');
                this.hasError(false);

                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        jqueryConfirmGenerics.showOkAlertBox('Success!', data.Message, "green", null);

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

                var data = {
                    Id: item.Id,
                    EntityName: item.EntityName,
                    Email: item.Email,
                    Phone1: item.Phone1,
                    Address: item.PhysicalAddress,
                    EntityNo: item.EntityNo,
                    Phone2: item.Phone2
                };
                debugger;
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Accounts/SelectAgent/SendReversal", data, functionDone, functionFailed);

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
                debugger;
                data = {

                };
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Accounts/AccountProfiles/GetKendoGridFiltered", data,
                    functionDone, functionFailed);

            }

            this.checkProfile();

            this.OnImport = function () {
                debugger;
                var formData = new FormData();
                var totalFiles = document.getElementById("fileToUpload").files.length;
                for (var i = 0; i < totalFiles; i++) {
                    var file = document.getElementById("fileToUpload").files[i];
                    formData.append("fileToUpload", file);
                }

                $.ajax({
                    type: 'POST',
                    url: '/Accounts/Resellers/UploadAccounts',
                    dataType: 'json',
                    processData: false,
                    contentType: false,
                    data: formData,
                    success: function (data) {

                        if (data.IsOkay) {
                            $('.datatables-basic').DataTable().ajax.reload();
                            $.alert({ title: 'Success', icon: 'icon-exclamation', content: data.Message, type: 'green', typeAnimated: true, });

                        }
                        else {

                            $.alert({ title: 'Failed', icon: 'icon-exclamation', content: data.Message, type: 'red', typeAnimated: true, });

                        }
                    },
                    error: function (err) {

                        debugger;

                        $.alert({ title: 'Failed', icon: 'icon-exclamation', content: err.message, type: 'red', typeAnimated: true, });
                    }

                });
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
                            self.loggedInEntity(true);
                            self.AgentMenueVisible(true);
                        }
                        if (data.Result.TypeName == "Admin") {
                            self.loggedInEntity(true);
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
        }
        var myModel = new viewModel();
        ko.applyBindings(myModel);

        connection.on("ReceiveMessage", function (senderId, message) {
            debugger;

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

        debugger;
        'use strict';


        var dt_basic_table = $('.datatables-basic'),
            dt_date_table = $('.dt-date'),
            dt_complex_header_table = $('.dt-complex-header'),
            dt_row_grouping_table = $('.dt-row-grouping'),
            dt_multilingual_table = $('.dt-multilingual'),
            assetPath = '/app-assets/';

        if ($('body').attr('data-framework') === 'laravel') {
            assetPath = $('body').attr('data-asset-path');
        }

        if (dt_basic_table.length) {
            var dt_basic = dt_basic_table.DataTable({

                ajax: function (data, callback, settings) {
                    debugger;

                    var functionDone = function (hasError, message, data) {
                        debugger;
                        var data = data.Result.Result;//.Result

                        var objectData = {
                            data: data
                        };
                        callback(objectData);
                        
                    };
                    var functionFailed = function (hasError, message, data) {
                        debugger;

                        var objectData = {
                            data: []
                        };
                        callback(objectData);
                    };
                    debugger;

                    var modelToSend = {};

                    jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Accounts/SelectAgent/GetKendoGridFiltered", modelToSend,
                        functionDone, functionFailed);

                },

                columns: [

                     { data: 'Id' },
                    { data: 'Id' },

                    { data: 'EntityName' },
                    { data: 'PhysicalAddress' },
                    { data: 'Phone1' },
                    { data: 'Email' },
                    //{ data: '' },
                    //{ data: '' },
                    { data: '' }

                ],
                columnDefs: [
                    {
                        // For Responsive
                        className: 'control',
                        orderable: false,
                        responsivePriority: 2,
                        targets: 0

                    },
                    {
                        // Avatar image/badge, Name and post
                        targets: 1,
                        visible: false
                    },

                    {

                        targets: 2,
                        responsivePriority: 4,
                        render: function (data, type, full, meta) {
                            var $user_img = full['avatar'],
                                $name = full['EntityName'],
                                $post = full['EntityUserName'];
                            if ($user_img) {

                                var $output =
                                    '<img src="' + assetPath + 'images/avatars/' + $user_img + '" alt="Avatar" width="32" height="32">';
                            } else {

                                var states = ['success', 'danger', 'warning', 'info', 'dark', 'primary', 'secondary'];
                                var $state = states[0],
                                    $name = full['EntityName'];
                                $initials = [];
                                if ($name != null)
                                    $initials = $name.match(/\b\w/g) || [];

                                $initials = (($initials.shift() || '') + ($initials.pop() || '')).toUpperCase();
                                $output = '<a href="#"><span class="avatar-content">' + $initials + '</span></a>';
                            }

                            var colorClass = $user_img === '' ? ' bg-light-' + $state + ' ' : '';
                            // Creates full output for row
                            var $row_output =
                                '<div class="d-flex justify-content-left align-items-center">' +
                                '<div class="avatar ' +
                                colorClass +
                                ' me-1">' +
                                $output +
                                '</div>' +
                                '<div class="d-flex flex-column">' +
                                '<a href="#"><span class="emp_name text-truncate fw-bold">' +
                                $name +
                                '</span></a>' +
                                '<small class="emp_post text-truncate text-muted">' +
                                $post +
                                '</small>' +
                                '</div>' +
                                '</div>';
                            return $row_output;
                        }
                    },

                    {
                        targets: 3,
                        visible: true
                    },
                    {
                        targets: 4,
                        visible: true
                    },
                    {
                        targets: 5,
                        visible: true
                    },
                    //{
                    //    // Actions
                    //    targets: 5,//fifth place from the data table
                    //    responsive: true,
                    //    title: 'Status',
                    //    orderable: false,
                    //    render: function (data, type, full, meta) {
                    //        debugger;
                    //        $stat = full['Phone2']
                    //        return ('<span class="badge ' + 'badge-light-primary' + '">' + $stat + '</span>');
                    //    }
                    //},
                    //{
                    //    targets: 6,//sixth place from the data table
                    //    responsive: true,
                    //    title: 'Fee Paid',
                    //    orderable: false,
                    //    render: function (data, type, full, meta) {
                    //        debugger;
                    //        $bal = full['EntityNo'];
                    //        return ('<span class="badge ' + 'badge-light-primary' + '">' + $bal + '</span>');
                    //    }
                    //},
                    //{
                    //    targets: 7,//seventh place from the data table
                    //    responsive: true,
                    //    title: 'claim',
                    //    orderable: false,
                    //    render: function (data, type, full, meta) {
                    //        var $Id = full['Id'],
                    //            $name = full['EntityName'],
                    //            $post = full['EntityUserName'],
                    //            $email = full['Email'],
                    //            $address = full['PhysicalAddress'],
                    //            $phone = full['Phone1'],
                    //            $username = full['EntityUserName'],
                    //            $status = full['Phone2'];


                    //        if ($status == "OverPaid") {
                    //            var buttonId = 'reverseButton' + meta.row; // Generate a unique button ID
                    //            return `<button 
                    //                    id="${buttonId}"
                    //                    type="button" 
                    //                    class="btn-sm btn-light"
                    //                    >Reverse</button>`;
                    //        }
                    //        //if ($status == "Partially Paid")
                    //        //{
                    //        //    return `<button 
                    //        //        type="button" 
                    //        //        class="btn-sm btn-secondary" 
                    //        //        >Claim</button>`;
                    //        //}
                    //        var buttonId = 'claimButton' + meta.row; // Generate a unique button ID
                    //        return `<button 
                    //                    id="${buttonId}" 
                    //                    type="button" 
                    //                    class="btn-sm btn-success" 
                    //                    data-id="${$Id}" 
                    //                    data-name="${$name}" 
                    //                    data-post="${$post}"
                    //                    data-email="${$email}" 
                    //                    data-address="${$address}" 
                    //                    data-phone="${$phone}"
                    //                    data-username="${$username}"
                    //                    >Claim</button>`;
                    //    }

                    //},
                    {
                        // Actions
                        targets: 6,
                        responsive: true,
                        title: 'Actions',
                        orderable: false,
                        render: function (data, type, full, meta) {

                            console.log('self.loggedInEntity:', self.loggedInEntity);

                            /* return (*/

                            console.log("Rendering function called for entity: " + self.loggedInEntity);


                            var actionsHtml = '<div class="d-inline-flex">' +
                                '<a class="pe-1 dropdown-toggle hide-arrow text-primary" data-bs-toggle="dropdown">' +
                                '<button type="button" class="btn-sm btn-primary">' +
                                'More Details' +
                                '</button>' +
                                '<div class="dropdown-menu dropdown-menu-end">';

                            
                            actionsHtml += '<a href="javascript:;" class="dropdown-item additional-view">' +
                                feather.icons['file-text'].toSvg({ class: 'me-50 font-small-4' }) +
                                ' Additional details</a>' +
                                '<a href="javascript:;" class="dropdown-item view-documents">' +
                                feather.icons['eye'].toSvg({ class: 'font-small-4' }) +
                                ' View Documents</a>';
                            

                            actionsHtml += '<a href="javascript:;" class="dropdown-item make-payments">' +
                                feather.icons['dollar-sign'].toSvg({ class: 'font-small-4' }) +
                                ' Add Payments details</a>';
                           
                            actionsHtml += '</div>' +
                                '</div>' +
                                '<a href="javascript:;" class="item-edit">' +
                                '</a>';

                            return actionsHtml;

                            //var actionsHtml = '<div class="d-inline-flex">' +
                            //    '<a class="pe-1 dropdown-toggle hide-arrow text-primary" data-bs-toggle="dropdown">' +
                            //    '<button type="button" class="btn-sm btn-primary">' +
                            //    'More Details' +
                            //    '</button>' +

                            //    '<div class="dropdown-menu dropdown-menu-end">';
                            //        actionsHtml += '<a href="javascript:;" class="dropdown-item make-payments">' +
                            //            feather.icons['dollar-sign'].toSvg({ class: 'font-small-4' }) +
                            //            ' Add Payments details</a>';
                            //        /* }*/
                            //        actionsHtml += '</div>' +
                            //            '</div>' +
                            //            '<a href="javascript:;" class="item-edit">' +

                            //            '</a>';

                            //return actionsHtml;
                        }

                    },

                ],
                order: [[0, 'desc']],
                dom: '<"card-header border-bottom p-1"<"head-label"><"dt-action-buttons text-end"B>><"d-flex justify-content-between align-items-center mx-0 row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>t<"d-flex justify-content-between mx-0 row"<"col-sm-12 col-md-6"i><"col-sm-12 col-md-6"p>>',
                displayLength: 10,
                lengthMenu: [7, 10, 25, 50, 75, 100],
                buttons: [

                ],
                responsive: {
                    details: {
                        display: $.fn.dataTable.Responsive.display.modal({
                            header: function (row) {
                                var data = row.data();
                                return 'Details of ' + data['EntityName'];
                            }
                        }),
                        type: 'column',
                        renderer: function (api, rowIdx, columns) {
                            var data = $.map(columns, function (col, i) {
                                return col.title !== '' // ? Do not show row in modal popup if title is blank (for check box)
                                    ? '<tr data-dt-row="' +
                                    col.rowIdx +
                                    '" data-dt-column="' +
                                    col.columnIndex +
                                    '">' +
                                    '<td>' +
                                    col.title +
                                    ':' +
                                    '</td> ' +
                                    '<td>' +
                                    col.data +
                                    '</td>' +
                                    '</tr>'
                                    : '';
                            }).join('');

                            return data ? $('<table class="table"/>').append('<tbody>' + data + '</tbody>') : false;
                        }
                    }
                },
                language: {
                    paginate: {
                        // remove previous & next text from pagination
                        previous: '&nbsp;',
                        next: '&nbsp;'
                    }
                }
            });
            $('div.head-label').html('<h5>Members</h5>');

        }

        // Flat Date picker
        if (dt_date_table.length) {
            dt_date_table.flatpickr({
                monthSelectorType: 'static',
                dateFormat: 'm/d/Y'
            });
        }

        // $('.datatables-basic tbody').on('click', 'button[id^="claimButton"]', function () {
        //
        //     var $Id = $(this).data('id');
        //     var $name = $(this).data('name');
        //     var $post = $(this).data('post');
        //     var $phone = $(this).data('phone');
        //     var $address = $(this).data('address');
        //     var $email = $(this).data('email');
        //     var $status = $(this).data('status');
        //     var $balance = $(this).data('balance');
        //
        //     debugger;
        //     myModel.claimCommision($Id, $name, $post, $phone, $address, $email, $status, $balance);
        //
        // });
        $('.datatables-basic tbody').on('click', 'button[id^="claimButton"]', function () {
            var rowData = dt_basic.row($(this).closest('tr')).data();

            myModel.claimCommision(rowData);

        });
        $('.datatables-basic tbody').on('click', 'button[id^="reverseButton"]', function () {
            var rowData = dt_basic.row($(this).closest('tr')).data();

            myModel.reverseTransaction(rowData);

        });

        $('.datatables-basic tbody').on('click', '.item-edit', function () {

            debugger;
            var data = dt_basic.row($(this).parents('tr')).data();
            myModel.EntityUserName2(data.EntityUserName);
            myModel.Email2(data.Email);
            myModel.Phone1(data.Phone1);
            myModel.Id(data.Id);
            myModel.selectedOption(data.IsActive);

            $("#edit-modal").modal('show');

        });
        dt_basic_table.on('click', '.view-documents', function () {
            // Get the row data associated with the clicked row
            var rowData = dt_basic.row($(this).closest('tr')).data();
            myModel.CustomerData(
                {
                    Id: rowData.Id,
                    Phone1: rowData.Phone1,
                    Email: rowData.Email

                }
            );

            debugger;
            myModel.uploadedDocuments(rowData.Id);
            $("#view-modal").modal('show');
        });

        dt_basic_table.on('click', '.additional-view', function () {
            // Get the row data associated with the clicked row
            var rowData = dt_basic.row($(this).closest('tr')).data();
            myModel.CustomerData(
                {
                    Id: rowData.Id,
                    Phone1: rowData.Phone1,
                    Email: rowData.Email

                }
            );

            debugger;
            myModel.additionalDetails(rowData.Id);
            $("#more-modal").modal('show');
        });

        dt_basic_table.on('click', '.make-payments', function () {
            // Get the row data associated with the clicked row
            var rowData = dt_basic.row($(this).closest('tr')).data();
            myModel.rowData(rowData); // Set the observable with rowData
            debugger;
            $("#payment-modal").modal('show');

        });


        var count = 101;
        $('.data-submit').on('click', function () {

        });


        if (dt_complex_header_table.length) {
            var dt_complex = dt_complex_header_table.DataTable({
                ajax: assetPath + 'data/table-datatable.json',
                columns: [
                    { data: 'full_name' },
                    { data: 'email' },
                    { data: 'city' },
                    { data: 'post' },
                    { data: 'salary' },
                    { data: 'status' },
                    { data: '' }
                ],
                columnDefs: [
                    {
                        // Label
                        targets: -2,
                        render: function (data, type, full, meta) {
                            var $status_number = full['status'];
                            var $status = {
                                1: { title: 'Current', class: 'badge-light-primary' },
                                2: { title: 'Professional', class: ' badge-light-success' },
                                3: { title: 'Rejected', class: ' badge-light-danger' },
                                4: { title: 'Resigned', class: ' badge-light-warning' },
                                5: { title: 'Applied', class: ' badge-light-info' }
                            };
                            if (typeof $status[$status_number] === 'undefined') {
                                return data;
                            }
                            return (
                                '<span class="badge rounded-pill ' +
                                $status[$status_number].class +
                                '">' +
                                $status[$status_number].title +
                                '</span>'
                            );
                        }
                    },
                    {
                        // Actions
                        targets: -1,
                        title: 'Actions',
                        orderable: false,
                        render: function (data, type, full, meta) {
                            return (
                                '<div class="d-inline-flex">' +
                                '<a class="pe-1 dropdown-toggle hide-arrow text-primary" data-bs-toggle="dropdown">' +
                                feather.icons['more-vertical'].toSvg({ class: 'font-small-4' }) +
                                '</a>' +
                                '<div class="dropdown-menu dropdown-menu-end">' +
                                '<a href="javascript:;" class="dropdown-item">' +
                                feather.icons['file-text'].toSvg({ class: 'me-50 font-small-4' }) +
                                'Details</a>' +
                                '<a href="javascript:;" class="dropdown-item">' +
                                feather.icons['archive'].toSvg({ class: 'me-50 font-small-4' }) +
                                'Archive</a>' +
                                //'<a href="javascript:;" class="dropdown-item delete-record">' +
                                //feather.icons['trash-2'].toSvg({ class: 'me-50 font-small-4' }) +
                                //'Delete</a>' +
                                '</div>' +
                                '</div>' +
                                '<a href="javascript:;" class="item-edit">' +
                                feather.icons['edit'].toSvg({ class: 'font-small-4' }) +
                                '</a>'
                            );
                        }
                    }
                ],
                dom: '<"d-flex justify-content-between align-items-center mx-0 row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>t<"d-flex justify-content-between mx-0 row"<"col-sm-12 col-md-6"i><"col-sm-12 col-md-6"p>>',
                displayLength: 7,
                lengthMenu: [7, 10, 25, 50, 75, 100],
                language: {
                    paginate: {
                        // remove previous & next text from pagination
                        previous: '&nbsp;',
                        next: '&nbsp;'
                    }
                }
            });
        }

        // Row Grouping
        // --------------------------------------------------------------------

        var groupColumn = 2;
        if (dt_row_grouping_table.length) {
            var groupingTable = dt_row_grouping_table.DataTable({
                ajax: assetPath + 'data/table-datatable.json',
                columns: [
                    { data: 'responsive_id' },
                    { data: 'full_name' },
                    { data: 'post' },
                    { data: 'email' },
                    { data: 'city' },
                    { data: 'start_date' },
                    { data: 'salary' },
                    { data: 'status' },
                    { data: '' }
                ],
                columnDefs: [
                    {
                        // For Responsive
                        className: 'control',
                        orderable: false,
                        targets: 0
                    },
                    { visible: false, targets: groupColumn },
                    {
                        // Label
                        targets: -2,
                        render: function (data, type, full, meta) {
                            var $status_number = full['status'];
                            var $status = {
                                1: { title: 'Current', class: 'badge-light-primary' },
                                2: { title: 'Professional', class: ' badge-light-success' },
                                3: { title: 'Rejected', class: ' badge-light-danger' },
                                4: { title: 'Resigned', class: ' badge-light-warning' },
                                5: { title: 'Applied', class: ' badge-light-info' }
                            };
                            if (typeof $status[$status_number] === 'undefined') {
                                return data;
                            }
                            return (
                                '<span class="badge rounded-pill ' +
                                $status[$status_number].class +
                                '">' +
                                $status[$status_number].title +
                                '</span>'
                            );
                        }
                    },
                    {
                        // Actions
                        targets: -1,
                        title: 'Actions',
                        orderable: false,
                        render: function (data, type, full, meta) {
                            return (
                                '<div class="d-inline-flex">' +
                                '<a class="pe-1 dropdown-toggle hide-arrow text-primary" data-bs-toggle="dropdown">' +
                                feather.icons['more-vertical'].toSvg({ class: 'font-small-4' }) +
                                '</a>' +
                                '<div class="dropdown-menu dropdown-menu-end">' +
                                '<a href="javascript:;" class="dropdown-item">' +
                                feather.icons['file-text'].toSvg({ class: 'me-50 font-small-4' }) +
                                'Details</a>' +
                                '<a href="javascript:;" class="dropdown-item">' +
                                feather.icons['archive'].toSvg({ class: 'me-50 font-small-4' }) +
                                'Archive</a>' +
                                '<a href="javascript:;" class="dropdown-item delete-record">' +
                                feather.icons['trash-2'].toSvg({ class: 'me-50 font-small-4' }) +
                                'Delete</a>' +
                                '</div>' +
                                '</div>' +
                                '<a href="javascript:;" class="item-edit">' +
                                feather.icons['edit'].toSvg({ class: 'font-small-4' }) +
                                '</a>'
                            );
                        }
                    }
                ],
                order: [[groupColumn, 'asc']],
                dom: '<"d-flex justify-content-between align-items-center mx-0 row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>t<"d-flex justify-content-between mx-0 row"<"col-sm-12 col-md-6"i><"col-sm-12 col-md-6"p>>',
                displayLength: 7,
                lengthMenu: [7, 10, 25, 50, 75, 100],
                drawCallback: function (settings) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;

                    api
                        .column(groupColumn, { page: 'current' })
                        .data()
                        .each(function (group, i) {
                            if (last !== group) {
                                $(rows)
                                    .eq(i)
                                    .before('<tr class="group"><td colspan="8">' + group + '</td></tr>');

                                last = group;
                            }
                        });
                },
                responsive: {
                    details: {
                        display: $.fn.dataTable.Responsive.display.modal({
                            header: function (row) {
                                var data = row.data();
                                return 'Details of ' + data['full_name'];
                            }
                        }),
                        type: 'column',
                        renderer: function (api, rowIdx, columns) {
                            var data = $.map(columns, function (col, i) {
                                return col.title !== '' // ? Do not show row in modal popup if title is blank (for check box)
                                    ? '<tr data-dt-row="' +
                                    col.rowIdx +
                                    '" data-dt-column="' +
                                    col.columnIndex +
                                    '">' +
                                    '<td>' +
                                    col.title +
                                    ':' +
                                    '</td> ' +
                                    '<td>' +
                                    col.data +
                                    '</td>' +
                                    '</tr>'
                                    : '';
                            }).join('');

                            return data ? $('<table class="table"/>').append('<tbody>' + data + '</tbody>') : false;
                        }
                    }
                },
                language: {
                    paginate: {
                        // remove previous & next text from pagination
                        previous: '&nbsp;',
                        next: '&nbsp;'
                    }
                }
            });

            // Order by the grouping
            $('.dt-row-grouping tbody').on('click', 'tr.group', function () {
                var currentOrder = table.order()[0];
                if (currentOrder[0] === groupColumn && currentOrder[1] === 'asc') {
                    groupingTable.order([groupColumn, 'desc']).draw();
                } else {
                    groupingTable.order([groupColumn, 'asc']).draw();
                }
            });
        }

        var lang = 'German';
        if (dt_multilingual_table.length) {
            var table_language = dt_multilingual_table.DataTable({
                ajax: assetPath + 'data/table-datatable.json',
                columns: [
                    { data: 'responsive_id' },
                    { data: 'full_name' },
                    { data: 'post' },
                    { data: 'email' },
                    { data: 'start_date' },
                    { data: 'salary' },
                    { data: 'status' },
                    { data: '' }
                ],
                columnDefs: [
                    {
                        // For Responsive
                        className: 'control',
                        orderable: false,
                        targets: 0
                    },
                    {
                        // Label
                        targets: -2,
                        render: function (data, type, full, meta) {
                            var $status_number = full['status'];
                            var $status = {
                                1: { title: 'Current', class: 'badge-light-primary' },
                                2: { title: 'Professional', class: ' badge-light-success' },
                                3: { title: 'Rejected', class: ' badge-light-danger' },
                                4: { title: 'Resigned', class: ' badge-light-warning' },
                                5: { title: 'Applied', class: ' badge-light-info' }
                            };
                            if (typeof $status[$status_number] === 'undefined') {
                                return data;
                            }
                            return (
                                '<span class="badge rounded-pill ' +
                                $status[$status_number].class +
                                '">' +
                                $status[$status_number].title +
                                '</span>'
                            );
                        }
                    },
                    {
                        // Actions
                        targets: -1,
                        title: 'Actions',
                        orderable: false,
                        render: function (data, type, full, meta) {

                            return (
                                '<div class="d-inline-flex">' +
                                '<a class="pe-1 dropdown-toggle hide-arrow text-primary" data-bs-toggle="dropdown">' +
                                feather.icons['more-vertical'].toSvg({ class: 'font-small-4' }) +
                                '</a>' +
                                '<div class="dropdown-menu dropdown-menu-end">' +
                                '<a href="javascript:;" class="dropdown-item">' +
                                feather.icons['file-text'].toSvg({ class: 'me-50 font-small-4' }) +
                                'Details</a>' +
                                '<a href="javascript:;" class="dropdown-item">' +
                                feather.icons['archive'].toSvg({ class: 'me-50 font-small-4' }) +
                                'Archive</a>' +
                                '<a href="javascript:;" class="dropdown-item delete-record">' +
                                feather.icons['trash-2'].toSvg({ class: 'me-50 font-small-4' }) +
                                'Delete</a>' +
                                '</div>' +
                                '</div>' +
                                '<a href="javascript:;" class="item-edit">' +
                                feather.icons['edit'].toSvg({ class: 'font-small-4' }) +
                                '</a>'
                            );
                        }
                    }
                ],
                language: {
                    url: '//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/' + lang + '.json',
                    paginate: {
                        // remove previous & next text from pagination
                        previous: '&nbsp;',
                        next: '&nbsp;'
                    }
                },
                dom: '<"d-flex justify-content-between align-items-center mx-0 row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>t<"d-flex justify-content-between mx-0 row"<"col-sm-12 col-md-6"i><"col-sm-12 col-md-6"p>>',
                displayLength: 7,
                lengthMenu: [7, 10, 25, 50, 75, 100],
                responsive: {
                    details: {
                        display: $.fn.dataTable.Responsive.display.modal({
                            header: function (row) {
                                var data = row.data();
                                return 'Details of ' + data['full_name'];
                            }
                        }),
                        type: 'column',
                        renderer: function (api, rowIdx, columns) {
                            var data = $.map(columns, function (col, i) {
                                return col.title !== '' // ? Do not show row in modal popup if title is blank (for check box)
                                    ? '<tr data-dt-row="' +
                                    col.rowIdx +
                                    '" data-dt-column="' +
                                    col.columnIndex +
                                    '">' +
                                    '<td>' +
                                    col.title +
                                    ':' +
                                    '</td> ' +
                                    '<td>' +
                                    col.data +
                                    '</td>' +
                                    '</tr>'
                                    : '';
                            }).join('');

                            return data ? $('<table class="table"/>').append('<tbody>' + data + '</tbody>') : false;
                        }
                    }
                }
            });
        }


    }
};


$(document).ready(function () {
    debugger;

    //Contacts();
    MVVM.init();
});

