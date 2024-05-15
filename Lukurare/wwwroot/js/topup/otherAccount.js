
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

        try {
            debugger;
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
                this.AccountNumber = ko.observable();
                //this.selectedQuoteCurrency = ko.observable();
                //this.selectedBaseCurrency = ko.observable();
                //this.Currencies = ko.observableArray();
                
                

                debugger;
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
                    this.Amount();
                    this.AccountNumber();
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

               //this.checkBalance();
                //this.validate = function () {
                //    var self = this;
                //    debugger;
                //    if (self.Amount() === null) {
                //       jqueryConfirmGenerics.showOkAlertBox('Validation Failed!', "Amount figure is required","red",null);
                //        return false
                //    }
                //    var balance = parseFloat(self.Balance());
                //    var amount = parseFloat(self.Amount());
                //    if (amount>balance) {
                //        jqueryConfirmGenerics.showOkAlertBox('Request Failed!', "Insufficient float balance to complete request","red",null);
                //        return false
                //    }
                //    return true;
                //}
                //this.checkCurrencies = function () {
                //    debugger;
                //    var self = this;
                //    //self.hasError(false);
                //    var functionDone = function (hasError, message, data) {
                //        debugger;
                //        if (hasError) {
                //            jqueryConfirmGenerics.showOkAlertBox('Failed!', message,"red",null);
                //        } else {
                //            self.Currencies(data.Result);
                //            self.selectedBaseCurrency(data.Result[0]);
                //            self.selectedQuoteCurrency(data.Result[2]);
                //        }
                //        //data;///data contains the result

                //    };
                //    var functionFailed = function (hasError, message, data) {
                //        debugger;
                //        if (hasError) {
                //            jqueryConfirmGenerics.showOkAlertBox('Failed!', message,"red",null);
                //        }
                //        jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);

                //    };
                //    debugger;

                //    jqueryAjaxGenerics.createJSONAjaxGETRequest("/Topup/LoadMeter/GetCurrency",
                //        functionDone, functionFailed);

                //}
                //this.checkCurrencies();

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

                    jqueryAjaxGenerics.createJSONAjaxGETRequest("Topup/LoadMeter/GetCurrency",
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
                    if (!self.AccountNumber() || self.AccountNumber() === 0) {
                        jqueryConfirmGenerics.showOkAlertBox('Validation Failed!', "Meter Number is required", "red", null);
                        return false;
                    }
                   // var balance = parseFloat(self.Balance());
                    //var amount = parseFloat(self.Amount());
                    //if (amount > balance) {
                    //    jqueryConfirmGenerics.showOkAlertBox('Request Failed!', "Insufficient float balance to complete request","red",null);
                    //    return false;
                    //} 
                    return true;
                }
                this.submitTopupRequest = function () {
                    
                    debugger;
                    var self = this;
                    //self.color("red");
                    //self.hasError(false);

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
                            //self.hasError(true);
                           // self.color("green");
                           jqueryConfirmGenerics.showOkAlertBox('Success!', data.Message,"green",null);
                            self.clearForm();
                           // self.checkBalance();
                           

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
                    //var contact = null;
                    //if (self.PhoneNumber() != null) {
                    //    debugger;
                    //    contact = {
                    //        ContactPhone: self.PhoneNumber(),
                    //        Id:0
                           
                    //    };
                    //} else {
                    //    debugger;
                    //    contact = {
                    //        ContactPhone: self.selectedContact().ContactPhone,
                    //        ContactName: self.selectedContact().ContactName,
                    //        ContactEmail: self.selectedContact().ContactEmail,

                    //        Id: self.selectedContact().Id,
                    //        BillReferenceServiceTypeId: self.selectedContact().BillReferenceServiceType.Id
                    //    };
                    //}

                  
                    debugger;
                    var data = {
                       
                        Amount:self.Amount(),
                        AccountNumber: self.AccountNumber(),
                        

     
                    };
                    debugger;
                   

                    jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Transaction/Accounts/AccountTransaction/Deposit", data,
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

                        self.ParentPhone(data.Phone1);

                        self.ParentEmail(data.Email);

                        //jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
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
                   // this.hasError(false);
                    var functionDone = function (hasError, message, data) {
                        debugger;
                        if (hasError) {
                            jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                        } else {
                            debugger;
                            //self.Documents(data.Result);

                            self.Brand(readUntilSpace(data.Result.Result[0].EntityName));
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


                this.displayTopupMessage = function () {
                    var self = this;
                    jqueryConfirmGenerics.showOkAlertBox("Information", "As we work on automating account topup, kindly contact " + self.ParentEmail() + " or " + self.ParentPhone() + " to load your wallet", "green", null);
                }
            }


            var obj=ko.applyBindings(new viewModel());
            

            topUp.myModelUI = obj;


        }
        catch (err) {
            topUp.unBlockUiPageElement($('body'));
            jqueryConfirmGenerics.showOkAlertBox('Error Init Single topup', err.message,"red",null);
        }
    }
};

$(document).ready(function () {
    debugger;
    topUp.init();
});



