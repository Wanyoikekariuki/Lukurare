
var registerObject = {
    blockUiPageElement: function (element) {
        return registerObject.blockUiPageElementWithMessage(element, 'Please wait...');
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

            var viewModel = function () {

                this.hasError = ko.observable(false);
                this.errorMessage = ko.observable();
                this.FullName = ko.observable();
                this.Phone = ko.observable();
                this.Email = ko.observable();
                this.PhysicalAddress = ko.observable();
                this.Username = ko.observable();
                this.PostalAddress = ko.observable();
                this.NationalID = ko.observable();
                this.AdminMenueVisible = ko.observable();
                this.EntityTypes= ko.observableArray([]);
                this.selectedEntityType = ko.observable();


                this.checkEntityTypes = function () {
                    debugger;
                    var self = this;
                    //self.hasError(false);
                    var functionDone = function (hasError, message, data) {
                        debugger;
                        if (hasError) {
                            jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                        } else {
                            self.EntityTypes(data.Result);
                            self.selectedEntityType(data.Result[1]);
                           
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

                    jqueryAjaxGenerics.createJSONAjaxGETRequest("/Register/GetEntityType",
                        functionDone, functionFailed);

                }
                this.checkEntityTypes();
                this.resetModel= function () {
                    var self = this;
                    self.FullName("");
                    self.Phone("");
                    self.Email("");
                    self.PhysicalAddress("");
                    self.Username("");
                    self.PostalAddress("");
                    self.NationalID("");
                }

               

                

             

                this.showErrorMessage= function (title, message) {
                    try {
                        this.hasError(true);
                        //this.errorMessage(title + ':' + message);
                        var alertArea = $('#alertArea');
                        /*jqueryConfirmGenerics.showOkAlertBox(title, message, "red", null);*/
                        alertArea.addClass('alert-danger').text(title + ': ' + message).show();
                        
                    }
                    catch (err) {
                        var alertArea = $('#alertArea');
                        /*jqueryConfirmGenerics.showOkAlertBox(title, err.message,"red",null);*/
                        alertArea.addClass('alert-danger').text(title + ': ' + err.message).show();
                    }
                }

            this.validateEmail = function () {
                    var self = this;
                    var email = self.Email();
                    var isValid = /\S+@\S+\.\S+/.test(email); // Basic email format validation

                    if (!isValid) {
                        self.showErrorMessage("Validation Failed", "Invalid email format");
                        self.Email('');
                    }
                    return false;
                };


                this.validate= function () {
                    var self = this;
                    if (!self.Username()) {
                        this.showErrorMessage("Validation Failed", "Username can not be empty");
                        return false;
                    }
                    else if (!self.Email() && self.Email()) {
                        this.showErrorMessage("Validation Failed", "Email can not be empty");
                        return false;
                    }
                    else if (!self.Phone()) {
                        this.showErrorMessage("Validation Failed", "Phone can not be empty");
                        return false;
                    }
                    else if (!self.PhysicalAddress()) {
                        this.showErrorMessage("Validation Failed", "Location Preference can not be empty");
                        return false;
                    }
                    else if (!self.PostalAddress()) {
                        this.showErrorMessage("Validation Failed", "County Residence can not be empty");
                        return false;
                    }
                    else if (!self.FullName()) {
                        this.showErrorMessage("Validation Failed", "Full Name can not be empty");
                        return false;
                    }
                    //else if (!self.NationalID()) {
                    //    this.showErrorMessage("Validation Failed", "National ID/Company Reg No. can not be empty");
                    //    return false;
                    //}
                    //else if (!self.selectedEntityType()) {
                    //    this.showErrorMessage("Validation Failed", "User category must be specified");
                    //    return false;
                    //}
                    else if (self.Phone().length<12) {
                        this.showErrorMessage("Validation Failed", "Include country code in phone number e.g 2547..");
                        return false;
                    }                 

                    return true;
                }

                this.registerReseller = function () {
                    registerObject.blockUiPageElementWithMessage($('body'), 'Processing.. Please wait');
                    var self = this;

                    if (self.validate() == false) {
                        registerObject.unBlockUiPageElement($('body'));
                        return;
                    }                

                   
                    self.hasError(false);

                    self.Model = {

                        FullName: self.FullName(),
                        Phone: self.Phone(),
                        Email: self.Email(),
                        PhysicalAddress: self.PhysicalAddress(),
                        Username: self.Username(),
                        PostalAddress: self.PostalAddress(),
                        EntityTypeName: self.selectedEntityType().TypeName,
                        NationalID: self.Phone()
                      
                    };

                    debugger;
                    $.ajax({
                        type: "POST",
                        url: "/Register/Register",
                        dataType: "json",
                        contentType: "application/json",
                        data: JSON.stringify(self.Model),
                    }).done(function (data, status, jqxhr) {
                        registerObject.unBlockUiPageElement($('body'));
                        debugger;
                        try {
                            self.resetModel();
                            var alertArea = $('#alertArea');
                            var succesFunction = function () {
                                authenticationHelper.navigateToPath("/Authentication/Login");
                            };
                            /*jqueryConfirmGenerics.showOkAlertBox('success', data.Message,"green", succesFunction);*/
                            alertArea.addClass('alert-success').text('Success' + ':' + data.Message).show();

                            setTimeout(succesFunction, 6000);
                        }
                        catch (err) {
                            self.showErrorMessage('Error', err.message);
                         
                           
                        }

                    }).fail(function (jqxhr) {
                        registerObject.unBlockUiPageElement($('body'));
                        debugger;
                      
                        try {
                            var alertArea = $('#alertArea');
                            self.hasError(true);
                            debugger;
                            var data = jqueryAjaxGenerics.parseJSON(jqxhr.responseText);
                            //self.showErrorMessage('Failed', data.Message);
                            /*jqueryConfirmGenerics.showOkAlertBox('Failed', data.Message, "red",null);*/
                            alertArea.addClass('alert-danger').text('Failed' + ': ' + data.Message).show();
                        }
                        catch (err) {
                            self.showErrorMessage('Failed', err.message);
                        }
                       
                    });
                }



               
            }

            debugger;
            var obj = ko.applyBindings(new viewModel());
            registerObject.myModelUI = obj;

        }
        catch (err) {
            var alertArea = $('#alertArea');
            registerObject.unBlockUiPageElement($('body'));
            /*jqueryConfirmGenerics.showOkAlertBox('Error Init Register', err.message, "red", null);*/
            alertArea.addClass('alert-danger').text('Error Init Register' + ': ' + err.message).show();

        }
    }
}

$(document).ready(function () {
    debugger
    registerObject.init();
});