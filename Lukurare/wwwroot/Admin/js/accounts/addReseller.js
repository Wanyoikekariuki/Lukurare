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
        var viewModel = function () {
            debugger;

            this.hasError = ko.observable(false);
            this.errorMessage = ko.observable();
            this.EntityUserName = ko.observable();
            this.EntityName = ko.observable();
            this.EntityUserName = ko.observable();
            this.DateOfBirth= ko.observable();
            this.Phone1 = ko.observable();
            this.Phone2 = ko.observable("n/a");
            this.Email = ko.observable();
            this.Balance = ko.observable();
            this.PhysicalAddress = ko.observable("");
            this.PostalAddress = ko.observable("");
            this.IdentificationDocumentNo = ko.observable();
            this.PhotoLinkUrl = ko.observable();
            this.selectedDocument= ko.observable();
            this.Documents= ko.observableArray([]);
            this.color = ko.observable("red");
            this.PhotoLinkUrl = ko.observable();
            this.Brand = ko.observable();
            this.ParentPhone = ko.observable(); 
            this.ParentEmail = ko.observable();
            this.AccountNumber = ko.observable();
            //this.IdentificationNumber = ko.observable();
            this.HouseNumber = ko.observable();
            this.MeterNumber = ko.observable();
            this.AccountTypes = ko.observableArray([]);
            this.SelectedAccountType1 = ko.observable();
            this.SelectedAccountType2 = ko.observable();

            this.showErrorMessage = function (title, message) {
                var self = this;
                debugger;
                try {
                    this.hasError(true);
                    this.errorMessage(title + ': ' + message);


                }
                catch (err) {
                    jqueryConfirmGenerics.showOkAlertBox(title, err.message,"red",null);
                }
            }
                 this.addEntity = function () {
                debugger;
                var self = this;
                if (!self.PhysicalAddress()) {
                    jqueryConfirmGenerics.showOkAlertBox('Validation Failed!', "Physical Address required", "red", null);
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

                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Accounts/MeterAccount/GetAccountType",
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

                
                var data = [];

                //var IdentificationNumber = {
                //    Id: 0,
                //    AccountEntityType: self.SelectedAccountType1().Id,
                //    AccountNumber: self.IdentificationNumber(),
                //    AccountName: self.EntityName(),
                //    AccountEntity: {

                //        EntityName: self.EntityName(),
                //        Phone1: self.Phone1(),

                //        PhysicalAddress: self.PhysicalAddress(),

                //    }
                //};
                var HouseNumber = {
                    Id: 0,
                    MfsAccountTypeId: self.SelectedAccountType1().Id,
                    AccountNumber: self.HouseNumber(),
                    AccountName: self.EntityName(),
                    AccountEntity: {

                        EntityName: self.EntityName(),
                        Phone1: self.Phone1(),

                        PhysicalAddress: self.PhysicalAddress(),

                    }
                };

                  var MeterNumber =   {
                        Id:0,
                        MfsAccountTypeId: self.SelectedAccountType2().Id,
                        AccountNumber: self.MeterNumber(),
                        AccountName: self.EntityName(), 
                        AccountEntity: {

                            EntityName: self.EntityName(),
                            Phone1: self.Phone1(),

                            PhysicalAddress: self.PhysicalAddress(),

                        }
                };

                if (self.HouseNumber()) {
                    data.push(HouseNumber);
                }
                if (self.MeterNumber()) {
                    data.push(MeterNumber);
                }

                debugger;
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Accounts/MeterAccount/BulkAdd", data,
                    functionDone, functionFailed);

            }
            this.editUser = function () {
                debugger;

                var self = this;
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message,"red",null);
                    } else {
                       jqueryConfirmGenerics.showOkAlertBox('Success!', data.Message,"green",null);
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
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message,"red",null);
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

                        self.PhysicalAddress(data.EntityName);

                    }

                };
                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    self.PhysicalAddress(data.EntityName);
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
                        self.PhysicalAddress(data.Result.Result[0].EntityName);
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
        }
        var myModel = new viewModel();
        //ko.applyBindings(myModel);
        var obj = ko.applyBindings(myModel);

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
