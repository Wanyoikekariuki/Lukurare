/**
 * DataTables Basic
 */
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
            //console.log("Connected to the SignalR hub.");
        }).catch(err => console.error(err.toString()));
        var viewModel = function () {
            debugger;

            this.hasError = ko.observable(false);
            this.errorMessage = ko.observable();
            this.EntityUserName = ko.observable();
            this.EntityName = ko.observable();
            this.EntityUserName = ko.observable();
            this.DateOfBirth = ko.observable();
            this.Phone1 = ko.observable();
            this.Phone2 = ko.observable("n/a");
            this.Email = ko.observable();
            this.Balance = ko.observable();
            this.PhysicalAddress = ko.observable("");
            this.PostalAddress = ko.observable("");
            this.IdentificationDocumentNo = ko.observable();
            this.PhotoLinkUrl = ko.observable();
            this.selectedDocument = ko.observable();
            this.Documents = ko.observableArray([]);
            this.color = ko.observable("red");
            this.PhotoLinkUrl = ko.observable();
            this.Brand = ko.observable();
            this.ParentPhone = ko.observable();
            this.ParentEmail = ko.observable();
            this.AccountNumber = ko.observable();
            this.NationalID = ko.observable();
            this.HouseNumber = ko.observable();
            this.MeterNumber = ko.observable();
            this.AccountTypes = ko.observableArray([]);
            this.SelectedAccountType1 = ko.observable();
            this.SelectedAccountType2 = ko.observable();
            this.AgentName = ko.observable();
            this.DocumentTypeId = ko.observableArray([]);
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
            this.UserContactList = ko.observable([]);




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
            this.addEntity = function () {
                debugger;
                var self = this;
                if (!self.AgentName()) {
                    jqueryConfirmGenerics.showOkAlertBox('Validation Failed!', "Agent Name required", "red", null);
                    return;
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                }

                this.sendRequest();

            }
            
            this.checkAccountTypes = function () {
                debugger;
                var self = this;
                //self.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        self.AccountTypes(data.Result);
                        self.SelectedAccountType1(data.Result[2]);
                        self.SelectedAccountType2(data.Result[1]);

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

                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Accounts/SelectAgent/GetAccountType",
                    functionDone, functionFailed);

            }
            this.checkAccountTypes();
            this.sendRequest = function () {
                var self = this;
                MVVM.blockUiPageElementWithMessage($('body'), 'Processing.. Please wait');
                debugger;

                self.color("red");
                self.hasError(false);
                var functionDone = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        self.color("green");
                        self.hasError(true);
                        jqueryConfirmGenerics.showOkAlertBox('Success!', data.Message, "green", null);

                        self.DateOfBirth("");
                        self.EntityName("");
                        self.EntityUserName("");

                        self.IdentificationDocumentNo("");
                        self.PhotoLinkUrl("");

                        self.Phone1("");
                        self.Phone2("");
                        self.Email("");
                    }

                };
                var functionFailed = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {

                        jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);

                };
                debugger;


                var data = {
                    Email: self.Email(),
                    PhysicalAddress: self.PhysicalAddress(),
                    PostalAddress: self.PostalAddress(),
                    NationalID: self.NationalID(),
                    IdentificationDocumentNumber: self.NationalID(),
                    EntityName: self.EntityName(),
                    Phone1: self.Phone1(),
                    EntityUserName: self.EntityUserName(),
                    EntityTypeName: self.SelectedAccountType2(),               
                   
                    

                };

            

                debugger;
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Accounts/SelectAgent/BulkAdd", data,
                    functionDone, functionFailed);

            }                      
           
            this.editUser = function () {
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
                        self.EntityUserName2("");
                        self.Phone12("");
                        self.Email2("");
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

                var data = {
                    Id: self.Id(),
                    EntityUserName: self.EntityUserName2(),
                    Phone1: self.Phone12(),
                    Email: self.Email2(),
                    IsActive: self.selectedOption()
                };
                debugger;
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("PUT", "/Accounts/Users/Update", data,
                    functionDone, functionFailed);

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

                        self.AgentName(data.EntityName);

                    }

                };
                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    self.AgentName(data.EntityName);
                    self.Brand(data.EntityName);

                };
                debugger;
                data = {

                };
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("GET", "/Accounts/AccountProfiles/GetParentDetails",
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
                        self.AgentName(data.Result.Result[0].EntityName);
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
                    container.scrollTop = container.scrollHeight;
                });
                Contact.messages([]);
            }   

            //this.SendOnEnter = function (event) {
            //    var self = this;
            //    if (event.keyCode === 13) {
            //        debugger;
            //        // Call your sendChat function or perform any desired action here
            //        self.sendChat(); // Make sure your sendChat function is defined and handles the message sending
            //    }
            //}
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

            this.displayTopupMessage = function () {
                var self = this;
                jqueryConfirmGenerics.showOkAlertBox("Information", "As we work on automating account topup, kindly contact " + self.ParentEmail() + " or " + self.ParentPhone() + " to load your wallet", "green", null);
            }
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
                    url: '/Accounts/MeterAccount/UploadAccounts',
                    dataType: 'json',
                    processData: false,
                    contentType: false,
                    data: formData,
                    beforeSend: jqueryAjaxGenerics.beforeSend,
                    success: function (data) {

                        if (data.IsOkay) {

                            jqueryConfirmGenerics.showOkAlertBox('Message', data.Message, null);
                        }
                        else {
                            jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, null);

                        }
                    },
                    error: function (err) {

                        jqueryConfirmGenerics.showOkAlertBox('Error!!', err.message, null);
                        debugger;
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
        }
        var myModel = new viewModel();
        //ko.applyBindings(myModel);
        var obj = ko.applyBindings(myModel);

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

        MVVM.myModelUI = obj;
        $(function () {
            ('use strict');

            var assetsPath = '/app-assets/',
                registerMultiStepsWizard = document.querySelector('.register-multi-steps-wizard'),
                pageResetForm = $('.auth-register-form'),
                select = $('.select2'),
                creditCard = $('.credit-card-mask'),
                expiryDateMask = $('.expiry-date-mask'),
                cvvMask = $('.cvv-code-mask'),
                mobileNumberMask = $('.mobile-number-mask'),
                pinCodeMask = $('.pin-code-mask');

            if ($('body').attr('data-framework') === 'laravel') {
                assetsPath = $('body').attr('data-asset-path');
            }

            if (pageResetForm.length) {
                pageResetForm.validate({

                    rules: {
                        'register-username': {
                            required: true
                        },
                        'register-email': {
                            required: true,
                            email: true
                        },
                        'register-password': {
                            required: true
                        }
                    }
                });
            }

            if (typeof registerMultiStepsWizard !== undefined && registerMultiStepsWizard !== null) {
                var numberedStepper = new Stepper(registerMultiStepsWizard),
                    $form = $(registerMultiStepsWizard).find('form');
                $form.each(function () {
                    var $this = $(this);
                    $this.validate({
                        rules: {
                            username: {
                                required: true
                            },
                            email: {
                                required: true
                            },
                            password: {
                                required: true,
                                minlength: 8
                            },
                            'confirm-password': {
                                required: true,
                                minlength: 8,
                                equalTo: '#password'
                            },
                            'first-name': {
                                required: true
                            },
                            'home-address': {
                                required: true
                            },
                            addCard: {
                                required: true
                            }
                        },
                        messages: {
                            password: {
                                required: 'Enter new password',
                                minlength: 'Enter at least 8 characters'
                            },
                            'confirm-password': {
                                required: 'Please confirm new password',
                                minlength: 'Enter at least 8 characters',
                                equalTo: 'The password and its confirm are not the same'
                            }
                        }
                    });
                });

                $(registerMultiStepsWizard)
                    .find('.btn-next')
                    .each(function () {
                        $(this).on('click', function (e) {
                            var isValid = $(this).parent().siblings('form').valid();
                            if (isValid) {
                                numberedStepper.next();
                            } else {
                                e.preventDefault();
                            }
                        });
                    });

                $(registerMultiStepsWizard)
                    .find('.btn-prev')
                    .on('click', function () {
                        numberedStepper.previous();
                    });

                $(registerMultiStepsWizard)
                    .find('.btn-submit')
                    .on('click', function () {
                        var isValid = $(this).parent().siblings('form').valid();
                        if (isValid) {
                            alert('Submitted..!!');
                        }
                    });
            }

            // select2
            select.each(function () {
                var $this = $(this);
                $this.wrap('<div class="position-relative"></div>');
                $this.select2({
                    // the following code is used to disable x-scrollbar when click in select input and
                    // take 100% width in responsive also
                    dropdownAutoWidth: true,
                    width: '100%',
                    dropdownParent: $this.parent()
                });
            });


            if (creditCard.length) {
                creditCard.each(function () {
                    new Cleave($(this), {
                        creditCard: true,
                        onCreditCardTypeChanged: function (type) {
                            const elementNodeList = document.querySelectorAll('.card-type');
                            if (type != '' && type != 'unknown') {
                                //! we accept this approach for multiple credit card masking
                                for (let i = 0; i < elementNodeList.length; i++) {
                                    elementNodeList[i].innerHTML =
                                        '<img src="' + assetsPath + 'images/icons/payments/' + type + '-cc.png" height="24"/>';
                                }
                            } else {
                                for (let i = 0; i < elementNodeList.length; i++) {
                                    elementNodeList[i].innerHTML = '';
                                }
                            }
                        }
                    });
                });
            }

            // Expiry Date Mask
            if (expiryDateMask.length) {
                new Cleave(expiryDateMask, {
                    date: true,
                    delimiter: '/',
                    datePattern: ['m', 'y']
                });
            }

            // CVV
            if (cvvMask.length) {
                new Cleave(cvvMask, {
                    numeral: true,
                    numeralPositiveOnly: true
                });
            }

            // phone number mask
            if (mobileNumberMask.length) {
                new Cleave(mobileNumberMask, {
                    phone: true,
                    phoneRegionCode: 'KE'
                });
            }

            // Pincode
            if (pinCodeMask.length) {
                new Cleave(pinCodeMask, {
                    delimiter: '',
                    numeral: true
                });
            }

            // multi-steps registration
            // --------------------------------------------------------------------
        });

    }
};

$(document).ready(function () {
    debugger;

    MVVM.init();
});
