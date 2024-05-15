


/**
 * DataTables Basic
 */
var MVVM = {
    init: function () {
        var viewModel = function () {
            debugger;

            this.hasError = ko.observable(false);
            this.errorMessage = ko.observable();
            this.EntityUserName = ko.observable();
            this.EntityName = ko.observable();
            //this.EntityNo= ko.observable();
            this.EntityUserName = ko.observable();
            this.DateOfBirth= ko.observable();
            this.Phone1 = ko.observable();
            this.Phone2 = ko.observable();
            this.Email = ko.observable();
            this.Balance = ko.observable();
            this.PhysicalAddress = ko.observable();
            this.PostalAddress = ko.observable();
            this.IdentificationDocumentNo = ko.observable();
            this.PhotoLinkUrl = ko.observable();
            this.selectedDocument= ko.observable();
            this.Documents= ko.observableArray([]);
            this.color = ko.observable("red");
            this.Id = ko.observable();
            this.WhiteListedDomain = ko.observable();
            this.avatar = ko.observable("Profile");
            this.Brand = ko.observable();
            this.ParentPhone = ko.observable();
            this.ParentEmail = ko.observable();
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

            this.checkBalance = function () {
                debugger;
                var self = this;
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message,"red",null);
                    } else {
                        self.Balance(data.balance);
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

            this.checkBalance();
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
            this.setUpGlobalVariables();
           

            this.displayTopupMessage = function () {
                var self = this;
                jqueryConfirmGenerics.showOkAlertBox("Information", "As we work on automating account topup, kindly contact " + self.ParentEmail() + " or " + self.ParentPhone() + " to load your wallet", "green", null);
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

            this.checkEntity = function () {
                debugger;
                var self = this;
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message,"red",null);
                    } else {
                        debugger;
                        //self.Documents(data.Result);

                        self.Brand(readStringUntilSpace(data.Result.Result[0].EntityName));
                        self.DateOfBirth(data.Result.Result[0].DateOfBirth);
                        self.EntityName(data.Result.Result[0].EntityName);
                        self.EntityUserName(data.Result.Result[0].EntityUserName);
                        self.PhysicalAddress(data.Result.Result[0].PhysicalAddress);
                        self.PostalAddress(data.Result.Result[0].PostalAddress);
                        self.IdentificationDocumentNo(data.Result.Result[0].IdentificationDocumentNumber);
                        self.PhotoLinkUrl(data.Result.Result[0].ProfileImageUrl);
                        self.WhiteListedDomain(data.Result.Result[0].WhiteListedDomain);

                        self.Phone1(data.Result.Result[0].Phone1);
                        self.Phone2(data.Result.Result[0].Phone2);
                        self.Email(data.Result.Result[0].Email);
                        self.Id(data.Result.Result[0].Id);
                        //self.EntityNo(data.Result.Result[0].EntityNo);

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
                data = {

                };
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Accounts/AccountProfiles/GetKendoGridFiltered", data,
                    functionDone, functionFailed);

            }

            this.checkEntity();

           

            this.addEntity = function () {
                debugger;
                var self = this;
                self.color("red");
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message,"red",null);
                    } else {
                        
                        self.color("green");
                       jqueryConfirmGenerics.showOkAlertBox('Success!', data.Message,"green",null);
                        //alert("Done");
                       
                      
                       
                       
                       
                        
                    
                    
                        
                       
         
                      
                       

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


                /*  *//* if (selected == undefined) {*/
                var data = {
                    Id:self.Id(),
                    //EntityNo: self.EntityNo(),
                    EntityUserName: self.EntityUserName(),
                    EntityName: self.EntityName(),
                    Phone1: self.Phone1(),
                    Phone2: self.Phone2(),
                    ProfileImageUrl:self.PhotoLinkUrl(),
                    Email: self.Email(),
                    PhysicalAddress: self.PhysicalAddress(),
                    PostalAddress: self.PostalAddress(),
                 WhiteListedDomain:self.WhiteListedDomain(),
                    IdentificationDocumentNumber: self.IdentificationDocumentNo()
                };
                debugger;
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("PUT", "/Accounts/AccountProfiles/Update", data,
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
           
            this.OnImport = function () {
                var self = this;
                debugger;
                var formData = new FormData();
                var totalFiles = document.getElementById("account-upload").files.length;
                for (var i = 0; i < totalFiles; i++) {
                    var file = document.getElementById("account-upload").files[i];
                    formData.append("account-upload", file);
                }

                $.ajax({
                    type: 'POST',
                    url: '/Accounts/AccountProfiles/UploadAccounts',
                    dataType: 'json',
                    processData: false,
                    contentType: false,
                    data: formData,
                    success: function (data) {
                        //jqueryConfirmGenerics.showOkAlertBox('success', data.Message, null);
                        if (data.Message) {
                            debugger;
                            self.PhotoLinkUrl(data.Message)
                            //$('.datatables-basic').DataTable().ajax.reload();
                            $.alert({ title: 'Success', icon: 'icon-exclamation', content: data.Message, type: 'green', typeAnimated: true, });
                            /* $("#accountEntityGroupContactsGridDivId").data("kendoGrid").dataSource.read();*/
                           /* kendo.ui.progress($("#upload_Section"), false);*/
                        }
                        else {
                            /*kendo.ui.progress($("#upload_section"), false);*/
                            $.alert({ title: 'Failed', icon: 'icon-exclamation', content: data.Message, type: 'red', typeAnimated: true, });

                        }
                    },
                    error: function (err) {

                        //jqueryConfirmGenerics.showOkAlertBox('Done!', err.message, null);
                        debugger;
                       /* kendo.ui.progress($("#upload_section"), false);*/

                        $.alert({ title: 'Failed', icon: 'icon-exclamation', content: err.message, type: 'red', typeAnimated: true, });
                    }

                });


            }

        }
        var myModel = new viewModel();
        ko.applyBindings(myModel);
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

            // jQuery Validation
            // --------------------------------------------------------------------
            if (pageResetForm.length) {
                pageResetForm.validate({
                    /*
                    * ? To enable validation onkeyup
                    onkeyup: function (element) {
                      $(element).valid();
                    },*/
                    /*
                    * ? To enable validation on focusout
                    onfocusout: function (element) {
                      $(element).valid();
                    }, */
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

            // multi-steps registration
            // --------------------------------------------------------------------

            // Horizontal Wizard
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

            // credit card

            // Credit Card
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

       
     /* 'use strict';

        var dt_basic_table = $('.datatables-basic'),
            dt_date_table = $('.dt-date'),
            dt_complex_header_table = $('.dt-complex-header'),
            dt_row_grouping_table = $('.dt-row-grouping'),
            dt_multilingual_table = $('.dt-multilingual'),
            assetPath = '/app-assets/';

        if ($('body').attr('data-framework') === 'laravel') {
            assetPath = $('body').attr('data-asset-path');
        }

        // DataTable with buttons
        // --------------------------------------------------------------------


        if (dt_basic_table.length) {
            var dt_basic = dt_basic_table.DataTable({

                ajax: function (data, callback, settings) {
                    debugger;


                    var functionDone = function (hasError, message, data) {
                        debugger;
                        var data = data.Result.Result;

                            var objectData = {
                                data: data
                            };
                            callback(objectData);


                    };
                    var functionFailed = function (hasError, message, data) {
                        debugger;
                        //jqueryConfirmGenerics.unBlockUiPageElement($('basic-datatable'));
                        // Swal.fire('Server Error', message, 'info');
                        var objectData = {
                            data: []
                        };
                        callback(objectData);
                    };
                    debugger;


                        var modelToSend = "";



                        jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Accounts/Users/GetKendoGridFiltered", modelToSend,
                            functionDone, functionFailed);




                        //var urlName = "/Contacts/AirtimeGroupContacts/GetKendoGridFilteredFromGroupId/" + myModel.selectedGroup().Id

                        //jqueryAjaxGenerics.createJSONAjaxGETRequest(urlName, functionDone, functionFailed);



                },

                columns: [

                    { data: 'Id' },
                    { data: 'Id' },
                    { data: 'Id' },

                    // used for sorting so will hide this column
                    { data: 'EntityUserName' },
                    { data: 'Phone1' },
                    { data: 'Email' },
                    { data: 'IsActive' },

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
                        // For Checkboxes
                        targets: 1,
                        orderable: false,
                        responsivePriority: 3,
                        render: function (data, type, full, meta) {
                            return (
                                '<div class="form-check"> <input class="form-check-input dt-checkboxes" type="checkbox" value="" id="checkbox' +
                                data +
                                '" /><label class="form-check-label" for="checkbox' +
                                data +
                                '"></label></div>'
                            );
                        },
                        checkboxes: {
                            selectAllRender:
                                '<div class="form-check"> <input class="form-check-input" type="checkbox" value="" id="checkboxSelectAll" /><label class="form-check-label" for="checkboxSelectAll"></label></div>'
                        }
                    },
                    {
                        targets: 2,
                        visible: false
                    },
                    {
                        // Avatar image/badge, Name and post
                        targets: 3,
                        responsivePriority: 4,
                        render: function (data, type, full, meta) {
                            var $user_img = full['avatar'],
                                $name = full['EntityUserName'],
                                $post = full['Phone1'];
                            if ($user_img) {
                                // For Avatar image
                                var $output =
                                    '<img src="' + assetPath + 'images/avatars/' + $user_img + '" alt="Avatar" width="32" height="32">';
                            } else {
                                // For Avatar badge
                                // var stateNum = full['Id'];
                                var states = ['success', 'danger', 'warning', 'info', 'dark', 'primary', 'secondary'];
                                var $state = states[0],
                                    $name = full['EntityUserName'],
                                    $initials = $name.match(/\b\w/g) || [];
                                $initials = (($initials.shift() || '') + ($initials.pop() || '')).toUpperCase();
                                $output = '<span class="avatar-content">' + $initials + '</span>';
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
                                '<span class="emp_name text-truncate fw-bold">' +
                                $name +
                                '</span>' +
                                '<small class="emp_post text-truncate text-muted">' +
                                $post +
                                '</small>' +
                                '</div>' +
                                '</div>';
                            return $row_output;
                        }
                    },
                    {
                        responsivePriority: 1,
                        targets: 4
                    },
                    {

                        targets: 5
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

                                //'<a href="javascript:;" class="dropdown-item delete-record">' +
                                //feather.icons['trash-2'].toSvg({ class: 'font-small-4 me-50' }) +
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
                order: [[2, 'desc']],
                dom: '<"card-header border-bottom p-1"<"head-label"><"dt-action-buttons text-end"B>><"d-flex justify-content-between align-items-center mx-0 row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>t<"d-flex justify-content-between mx-0 row"<"col-sm-12 col-md-6"i><"col-sm-12 col-md-6"p>>',
                displayLength: 10,
                lengthMenu: [7, 10, 25, 50, 75, 100],
                buttons: [
                 
                    {
                        text: feather.icons['plus'].toSvg({ class: 'me-50 font-small-4' }) + 'Add New User',
                        className: 'create-new btn btn-primary',
                        attr: {
                            'data-bs-toggle': 'modal',
                            'data-bs-target': '#modals-slide-in'
                        },
                        init: function (api, node, config) {
                            $(node).removeClass('btn-secondary');
                        }
                    }

                ],
                responsive: {
                    details: {
                        display: $.fn.dataTable.Responsive.display.modal({
                            header: function (row) {
                                var data = row.data();
                                return 'Details of ' + data['ContactName'];
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
            $('div.head-label').html('<h5>App Users</h5>');

        }

      

       
      $('.datatables-basic tbody').on('click', '.item-edit', function () {

            debugger;
            var data = dt_basic.row($(this).parents('tr')).data();
            myModel.EntityUserName2(data.EntityUserName);
            myModel.Email2(data.Email);
            myModel.Phone12(data.Phone1);
            myModel.Id(data.Id);
            myModel.selectedOption(data.IsActive);

            $("#edit-modal").modal('show');


        });
        // Delete Record
        $('.datatables-basic tbody').on('click', '.delete-record', function () {
            debugger;
            var data = dt_basic.row($(this).parents('tr')).data();
            var functionDone = function (hasError, message, data) {
                debugger;
                if (hasError) {
                    myModel.showErrorMessage("failed", message);
                } else {
                    myModel.showErrorMessage("success", data.Message);
                    $('.datatables-basic').DataTable().ajax.reload();
                }
                //data;///data contains the result

            };
            var functionFailed = function (hasError, message, data) {
                debugger;
                if (hasError) {
                    myModel.showErrorMessage("failed", message);
                }


            };
            debugger;


            jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Contacts/AccountEntityContacts/Delete", data,
                functionDone, functionFailed);

        });

   **/

      

        
       

        
    }
};

$(document).ready(function () {
    debugger;

    //Contacts();
    MVVM.init();
});
