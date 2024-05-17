var switchLoginView = function (toViewId) {

    $("#" + toViewId).attr('style', 'display: block');
    var loginViewIds = login.getLoginIdSections();
    for (var i = 0; i < loginViewIds.length; i++) {
        var currentElem = loginViewIds[i];
        if (currentElem !== toViewId) {
            //$("#" + currentElem).fadeOut("fast", function () {
            //    //remove the element just incase it on UI
            //});
            $("#" + currentElem).attr('style', 'display: none');
        }
        //else if (currentElem === toViewId) {
        //    $("#" + toViewId).fadeIn("slow", function () {
        //        //display the selected item on the UI
        //    });

        //}
    }

};
var login = {
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

    getLoginIdSections: function () {
        return [login.login_section, login.forgot_password_section, login.reset_password_section];
    },

    login_section: "login_section",
    forgot_password_section: "forgot_password_section",
    reset_password_section: "reset_password_section",
    /*init_renew_sectionId: "init_renew_section",*/

    loginSectionValidator: null,

    init: function () {

        try {
            debugger;
            var viewModel = function () {

                this.hasError = ko.observable(true);
                this.errorMessage = ko.observable();
                //this.serverResultModel =ko.observable();
                this.Username = ko.observable();
                this.Password = ko.observable();


                this.IsOkay = ko.observable();
                this.Message = ko.observable();

                this.EmailAddress = ko.observable();
                this.Id = ko.observable();
                this.Phone = ko.observable();
                //this.Token= ko.observable();
                this.color = ko.observable("red");

                this.Username2 = ko.observable();
                this.Token = ko.observable();
                this.Password2 = ko.observable();
                this.Password12 = ko.observable();
                this.OtpCode = ko.observable();
                this.PasswordVisible = ko.observable(false);
                this.togglePasswordVisibility = function () {
                    this.PasswordVisible(!this.PasswordVisible());
                };

                this.sendOtpRenewText = ko.observable("Send Code to Email");
                

                this.clearPassword = function () {
                    debugger;
                    this.Password(null);
                };

                this.clearAll = function () {
                    this.clearPassword();

                    this.Username(null);
                };

                this.showErrorMessage = function (title, message) {
                    debugger;
                    try {
                        this.hasError(true);
                        //this.errorMessage(title + ':' + message);
                        jqueryConfirmGenerics.showOkAlertBox(title, message, "red", null);

                    }
                    catch (err) {
                        jqueryConfirmGenerics.showOkAlertBox(title, err.message,"red",null);
                    }
                };

                this.validate = function () {
                    debugger;
                    if (this.Username() === null) {
                       
                        this.showErrorMessage("Validation Failed", "Username can not be empty");
                        
                        return false
                    } else if (this.Password() === null) {
                       
                        this.showErrorMessage("Validation Failed", "Password can not be empty");
                        return false;
                       
                    }
                    return true;
                };

                this.loginGeneric = function (url, elementProgress) {
                    var self = this;
                    debugger;
                    if (this.validate() === false) {
                        login.unBlockUiPageElement($('body'));
                        return;
                    }
                       
                        
                    if (this.Username2() != null) {
                        this.Username(this.Username2())
                    }
                    self.authenticationRequest = {
                        Username: this.Username(),
                        Password: this.Password()
                    };
                   // kendo.ui.progress(elementProgress, true);
                    self.hasError(false);
                    debugger;
                    $.ajax({
                        type: "POST",
                        url: url,
                        dataType: "json",
                        contentType: "application/json",
                        data: ko.toJSON(self.authenticationRequest)
                        // data: JSON.stringify(self.authenticationRequest),
                    }).done(function (data, status, jqxhr) {
                        login.unBlockUiPageElement($('body'));
                        debugger;
                        try {
                            if (data && data.Result && data.Result.Token) {
                                login.unBlockUiPageElement($('body'));
                                self.Message(data.Message);
                                self.IsOkay(data.IsOkay);
                                self.Id(data.Result.Id);
                                self.Phone(data.Result.Phone);
                                self.Token(data.Result.Token);
                                self.EmailAddress(data.Result.EmailAddress);
                                //self.serverResultModel(data);
                                self.loginUserToSystem(data);

                            }
                            else {
                                self.showErrorMessage('Error', err.message,"red",null);
                                self.clearPassword();
                                
                            }
                           
                        }
                        catch (err) {
                            login.unBlockUiPageElement($('body'));
                            self.showErrorMessage('Error', err.message,"red",null);
                            self.clearPassword();
                           
                        }
                        login.unBlockUiPageElement($('body'));
                    }).fail(function (jqxhr) {
                        debugger;
                        login.unBlockUiPageElement($('body'));
                        try {
                            self.hasError(true);
                            var data = jqueryAjaxGenerics.parseJSON(jqxhr.responseText);
                            self.showErrorMessage('Failed', data.Message);
                            
                        }
                        catch (err) {
                            self.showErrorMessage('Failed', data.Message);
                           
                        }
                        self.clearPassword();
                    });
                };

                this.initResetPassword = function () {
                    debugger;
                    var self = this;
                    self.Password("N");
                    

                   self.loginGeneric('/Authentication/RenewPasswordInitiate', $("#" + login.forgot_password_section));
                    debugger;
                   
                };

                this.login = function () {
                    login.blockUiPageElementWithMessage($('body'), 'Processing.. Please wait');
                    this.loginGeneric('/Authentication/Authenticate', $("#" + login.login_section));
                   
                };


                this.sendRenewOTPCode = function () {
                    var self = this;
                    login.blockUiPageElementWithMessage($('body'), 'Processing.. Please wait');
                    var elementProgress = $("#" + login.forgot_password_section);
                    self.serverResultModel = {
                        User: {
                            Id: self.Id(),
                            Phone: self.Phone(),
                            EmailAddress: self.EmailAddress()
                           
                        },
                        Token: self.Token()
                       
                    };
                    debugger;
                    self.hasError(false);
                    self.color("red");
                    debugger;
                    $.ajax({
                        type: "POST",
                        url: '/Authentication/SendRenewPasswordOTP',
                        dataType: "json",
                        contentType: "application/json; charset=UTF-8",
                        data: ko.toJSON(self.serverResultModel),
                    }).done(function (data, status, jqxhr) {
                        login.unBlockUiPageElement($('body'));
                        debugger;
                        try {
                            if (data && data.Result && data.Result.Token) {
                                self.Message(data.Message);
                                self.IsOkay(data.IsOkay);
                                self.Id(data.Result.Id);
                                self.Phone(data.Result.Phone);
                                self.Token(data.Result.Token);
                                self.EmailAddress(data.Result.EmailAddress);
                                self.color("green");
                                //self.showErrorMessage('Success', data.Message);
                               // self.serverResultModel(data);
                                //alert("Success:" + data.Message);
                                //jqueryConfirmGenerics.showOkAlertBox('Success', data.Message);
                                jqueryConfirmGenerics.showOkAlertBox('Success', data.Message, "green", null);
                            }
                            else {
                                self.showErrorMessage('Error', err.message,"red",null);
                            }
                        }
                        catch (err) {
                            self.showErrorMessage('Error', err.message,"red",null);
                        }

                    }).fail(function (jqxhr) {
                        debugger;
                        login.unBlockUiPageElement($('body'));
                        try {
                            self.hasError(true);
                            var data = jqueryAjaxGenerics.parseJSON(jqxhr.responseText);
                            self.showErrorMessage('Failed', data.Message);
                        }
                        catch (err) {
                            self.showErrorMessage('Failed', err.Message);
                        }
                    }).always(function () {
                        self.sendOtpRenewText("Resend OTP");
                    });
                };
                this.onResetPasswordSubmit = function () {
                    var self = this;
                    login.blockUiPageElementWithMessage($('body'), 'Processing.. Please wait');
                    var elementProgress = $("#" + login.reset_password_section);
                    //kendo.ui.progress(elementProgress, true);
                    self.hasError(false);// Username: self.Username2(),Password: self.Password2(),
                    
                    self.renewPasswordModel = {
                        Username: self.Username2(),
                        Password: self.Password2(),
                        Password1: self.Password12(),
                        OtpCode: self.OtpCode(),
                        Token: self.Token()
                    };
                    debugger;
                    $.ajax({
                        type: "POST",
                        url: '/Authentication/RenewPassword',
                        dataType: "json",
                        contentType: "application/json; charset=UTF-8",
                        data: ko.toJSON(self.renewPasswordModel)
                    }).done(function (data, status, jqxhr) {
                        login.unBlockUiPageElement($('body'));
                        debugger;
                        try {
                            if (data && data.Result && data.Result.Token) {
                                self.Message(data.Message);
                                self.IsOkay(data.IsOkay);
                                self.Id(data.Result.Id);
                                self.Phone(data.Result.Phone);
                                self.Token(data.Result.Token);
                                self.EmailAddress(data.Result.EmailAddress);
                                //self.serverResultModel(data);
                                jqueryConfirmGenerics.showOkAlertBox('Success', data.Message,"green",null);
                                self.backToLoginView();
                            }
                            else {
                                jqueryConfirmGenerics.showOkAlertBox('Error',data.Message,"red",null);
                            }
                        }
                        catch (err) {
                            jqueryConfirmGenerics.showOkAlertBox('Error', err.message,"red",null);
                        }

                    }).fail(function (jqxhr) {
                        debugger;
                        login.unBlockUiPageElement($('body'));
                        try {
                            self.hasError(true);
                            var data = jqueryAjaxGenerics.parseJSON(jqxhr.responseText);
                            self.showErrorMessage('Failed', data.Message);
                        }
                        catch (err) {
                            self.showErrorMessage('Failed', data.Message);
                        }
                    }).always(function () {

                         });
                };

                this.loginUserToSystem = function (data) {
                    //for this system we use cookie based Authentication
                    if (data.Message === "authenticated") {
                        authenticationHelper.displayIndexPage();
                    }
                    else if (data.Message === "otp_code_required") {
                        debugger;
                        this.initOTPCodeSupply(data);
                    }
                    else if (data.Message === "renew_password_required") {
                        debugger;
                        this.displayResetPasswordSection(data);
                    }
                    else {
                        //just display the login page
                        switchLoginView(login.login_section);
                    }
                };

                this.initOTPCodeSupply = function (data) {
                    debugger;
                    switchLoginView(login.forgot_password_section);
                };

                this.backToLoginView = function () {
                    switchLoginView(login.login_section);
                    this.clearAll();
                };

                this.showInitResetPassword = function () {
                    switchLoginView(login.forgot_password_section);
                };

                this.displayResetPasswordSection = function (data) {
                    debugger;
                    var self = this;
                    self.Token(data.Result.Token);
                    self.EmailAddress(data.Result.EmailAddress);
                    self.Id(data.Result.Id);
                    self.Phone(data.Result.Phone);
                    self.Username2(self.Username());
                    debugger;
                    self.sendRenewOTPCode();
                    switchLoginView(login.reset_password_section);
                    //self.sendRenewOTPCode();
                };
            };


            //ko.applyBindings(new viewModel());
            var obj = ko.applyBindings(new viewModel());
            login.myModelUI = obj;
            

        }
        catch (err) {
            login.unBlockUiPageElement($('body'));
            jqueryConfirmGenerics.showOkAlertBox('Error Init Login', err.message,"red",null);
        }
    }
}

$(document).ready(function () {
    debugger;
    login.init();
});


