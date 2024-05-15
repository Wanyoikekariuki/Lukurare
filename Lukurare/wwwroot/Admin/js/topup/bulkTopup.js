



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
            this.PhotoLinkUrl = ko.observable();
            this.Amount = ko.observable();
            this.Amount2 = ko.observable();
            this.Index = ko.observable();
            this.hasError = ko.observable(false);
            this.errorMessage = ko.observable();
            this.ContactName = ko.observable();
            this.ContactPhone = ko.observable();
            this.ContactEmail = ko.observable();
            this.AccountEntityId = ko.observable();
            this.Countries = ko.observableArray([]);
            this.Groups = ko.observableArray([]);
            this.selectedCountry = ko.observable(0);
            this.selectedGroup = ko.observable();
            this.Id = ko.observable(0);
            this.Networks = ko.observableArray([]);
            this.selectedNetwork = ko.observable(0);
            this.GroupName = ko.observable();
            this.ContactName2 = ko.observable();
            this.ContactPhone2 = ko.observable();
            this.ContactEmail2 = ko.observable();
            this.Balance = ko.observable();
            this.color = ko.observable("red");
            this.sendRequestNow = ko.observable(false);
            this.Brand = ko.observable();
            this.ParentPhone = ko.observable();
            this.ParentEmail = ko.observable();
            this.clearAmount = function () {
                debugger;
                this.Amount(0);
            }
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

            this.checkBalance();

            this.checkCountries = function () {
                debugger;
                var self = this;
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message,"red",null);
                    } else {
                        self.Countries(data.Result);
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

                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Contacts/AirtimeGroupContacts/GetCountryCode",
                    functionDone, functionFailed);

            }
            this.checkCountries();
         
            this.checkGroups = function () {
                debugger;
                var self = this;
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message,"red",null);
                    } else {
                        self.Groups(data.Result);
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

                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Contacts/AirtimeGroupContacts/GetGroups",
                    functionDone, functionFailed);

            }
            this.checkGroups();

            this.addContact = function () {
                MVVM.blockUiPageElementWithMessage($('body'), 'Processing.. Please wait');
                debugger;
                var self = this;
                self.color("red");
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message,"red",null);
                    } else {
                        this.hasError(true);
                        self.color("green");
                       jqueryConfirmGenerics.showOkAlertBox('Success!', data.Message,"green",null);
                        self.ContactName("");
                        self.ContactPhone("");
                        self.ContactEmail("");
                        self.Id(0);
                        $('.datatables-basic').DataTable().ajax.reload();

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
                    Id: self.Id(),
                    ContactName: self.ContactName(),
                    ContactPhone: self.ContactPhone(),
                    ContactEmail: self.ContactEmail(),
                    AccountEntityId: 0,
                    Country: self.selectedCountry(),
                     BillReferenceServiceType: self.selectedNetwork()
                };
                debugger;
                var selected = self.selectedGroup();

                if (!selected) {
                    if (data.Id === 0) {
                        jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Contacts/AccountEntityContacts/Add", data,
                            functionDone, functionFailed);
                    } else {
                        jqueryAjaxGenerics.createJSONAjaxNonGETRequest("PUT", "/Contacts/AccountEntityContacts/Update", data,
                            functionDone, functionFailed);
                    }
                } else {
                    var data2 = {
                        Id: 0,
                        AccountEntityContact: data,
                        AccountEntityGroup: selected
                    };

                    jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Contacts/AirtimeGroupContacts/Add", data2,
                        functionDone, functionFailed);

                }


            }
            this.editRecord = function () {
                //MVVM.blockUiPageElementWithMessage($('body'), 'Processing.. Please wait');
                debugger;
                
                var datas = dt_basic.row(this.Index()).data();
                datas.Amount = this.Amount2();
                dt_basic.row(this.Index()).data(datas).draw();
                this.Index(0);
                jqueryConfirmGenerics.showOkAlertBox('Success!', "Amount Edited successfully", "green", null);
                
                $("#edit-modal").modal('hide');
            }
            this.createGroup = function () {
                MVVM.blockUiPageElementWithMessage($('body'), 'Processing.. Please wait');
                debugger;
                var self = this;
                self.color("red");
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message,"red",null);
                    } else {
                        this.hasError(true);
                        self.color("green");
                       jqueryConfirmGenerics.showOkAlertBox('Success!', data.Message,"green",null);
                        self.checkGroups();
                        self.GroupName = ko.observable();

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
                    Id: 0,
                    GroupName: self.GroupName(),

                    AccountEntityId: 0

                };
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Contacts/AccountEntityGroups/Add", data,
                    functionDone, functionFailed);

            }
            this.permissionChanged = function (obj, event) {
                debugger;


                if (event.originalEvent) { //user changed
                    debugger;
                    $('.datatables-basic').DataTable().ajax.reload();
                } else { // program changed
                    debugger;
                }

            }
            this.updateAmount = function (obj, event) {
                debugger;


                if (event.originalEvent) { //user changed
                    debugger;
                    $('.datatables-basic').DataTable().ajax.reload();
                   
                } else { // program changed
                    debugger;
                }

            }
            this.checkExistingContact = function (obj, event) {
                debugger;
                var self = this;
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (data && data.Result) {
                        self.color("green");
                        //self.showErrorMessage("success", "contact already exists,you can edit though");
                        jqueryConfirmGenerics.showOkAlertBox(' Succcess!', "contact already exists,You can edit it though", "green", null);

                        var data2 = data.Result;
                        self.Id(data2.Id);
                        self.ContactPhone(data2.ContactPhone);
                        self.ContactEmail(data2.ContactEmail);
                        self.ContactName(data2.ContactName);
                        //$("#edit-modal").modal('show');
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
                    Id: 0,
                    ContactName: "",
                    ContactPhone: self.ContactPhone(),
                    ContactEmail: "",
                    AccountEntityId: 0,
                    Country: self.selectedCountry()
                };
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Contacts/AccountEntityContacts/CheckExistingContact", data,
                    functionDone, functionFailed);


            }
            this.validate = function () {
                debugger;
                var self = this;
               
                if (!self.sendRequestNow()) {
                   
                    jqueryConfirmGenerics.showOkAlertBox('Request Failed!', "Please tick Confirm box before Submit", "red",null);
                    return false;
                }
                if (self.Amount() === null || self.Amount() === 0) {
                  
                   jqueryConfirmGenerics.showOkAlertBox('Validation Failed!', "Amount figure is required","red",null);
                    return false;
                }
               
                return true;
            }
            this.submitTopupRequest = function () {
                MVVM.blockUiPageElementWithMessage($('body'), 'Processing.. Please wait');
                debugger;
                var self = this;
                this.hasError(false);
                var result=self.validate();
                if (result === false) {
                    MVVM.unBlockUiPageElement($('body'));

                    return;
                }
                  
               //self.updateAmount();
                var functionDone = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message,"red",null);
                    } else {
                        self.color("green");
                       jqueryConfirmGenerics.showOkAlertBox('Success!', data.Message,"green",null);
                        self.clearAmount();
                        self.checkBalance();

                    }


                };
                var functionFailed = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message,"red",null);
                    }

                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                };
                debugger;
                var alldata = dt_basic.rows().data();
                var data = [];
                var total = 0;
                for (let i = 0; i < alldata.length; i++) {
                 
                    var model = {
                        Id: 0,
                        Success: false,
                        Processed: false,
                        Narration: "bulk top up",
                        Amount: alldata[i].Amount,
                        AccountEntityContact: {
                            Id: alldata[i].Id,
                            BillReferenceServiceTypeId: alldata[i].BillReferenceServiceTypeId
                        },
                        SystemTransactionId: 0
                    };
                    data[i] = model;
                    total = total + parseFloat(alldata[i].Amount);
                }
                

                
                debugger;

                var balance = parseFloat(self.Balance());
               
                if (total > balance) {
                    MVVM.unBlockUiPageElement($('body'));
                    jqueryConfirmGenerics.showOkAlertBox('Request Failed!', "Insufficient float balance to complete request","red",null);
                    return;
                }
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Topup/BillReferenceServiceRequest/BulkTopup", data,
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
        var myModel = new viewModel();
        var obj= ko.applyBindings(myModel);
       
        MVVM.myModelUI =obj;

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

        // DataTable with buttons
        // --------------------------------------------------------------------

        if (dt_basic_table.length) {
            var dt_basic = dt_basic_table.DataTable({

                ajax: function (data, callback, settings) {
                    debugger;


                    var functionDone = function (hasError, message, data) {
                        debugger;
                        if (myModel.Amount() == null) {
                            myModel.Amount(0);
                        }
                        
                        var data = data.Result.Result;
                        if (data.length > 0) {
                            if (data[0].hasOwnProperty("AccountEntityContact")) {
                                for (i = 0; i < data.length; i++) {
                                    //data[i].x = data[i].x.split(' ')[0];
                                    //data[i].xlabel=
                                    data[i] = data[i].AccountEntityContact
                                }
                                for (i = 0; i < data.length; i++) {
                                   
                                    data[i].Amount = myModel.Amount();
                                    data[i].Service = data[i].BillReferenceServiceType.ServiceName;
                                    data[i].BillReferenceServiceTypeId = data[i].BillReferenceServiceType.Id;
                                }

                                var objectData = {
                                    data: data
                                };
                                callback(objectData);
                            } else {
                                for (i = 0; i < data.length; i++) {

                                    data[i].Amount = myModel.Amount();
                                    data[i].Service = data[i].BillReferenceServiceType.ServiceName;
                                    data[i].BillReferenceServiceTypeId = data[i].BillReferenceServiceType.Id;
                                }
                                var objectData = {
                                    data: data
                                };
                                callback(objectData);
                            }
                        } else {
                            var objectData = {
                                data: data
                            };
                            callback(objectData);
                        }

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
                    var selected = myModel.selectedGroup();
                    if (!selected) {

                        var modelToSend = "";



                        jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Contacts/AccountEntityContacts/GetKendoGridFiltered", modelToSend,
                            functionDone, functionFailed);
                    } else {



                        var urlName = "/Contacts/AirtimeGroupContacts/GetKendoGridFilteredFromGroupId/" + myModel.selectedGroup().Id

                        jqueryAjaxGenerics.createJSONAjaxGETRequest(urlName, functionDone, functionFailed);

                    }

                },

                columns: [

                   
                    { data: 'Id' },
                    { data: 'BillReferenceServiceTypeId' },
                   
                    { data: 'ContactName' },
                    { data: 'ContactPhone' },
                    { data: 'Amount' },
                    { data: 'Service' },
                    { data: '' },

                ],
                columnDefs: [
                  
                    {
                        targets: 0,
                        visible: false
                    },
                    {
                        targets: 1,
                        visible: false
                    },
                    {
                        // Avatar image/badge, Name and post
                        targets: 2,
                        responsivePriority: 4,
                        render: function (data, type, full, meta) {
                            var $user_img = full['avatar'],
                                $name = full['ContactName'],
                                $post = full['ContactEmail'];
                            if ($user_img) {
                                // For Avatar image
                                var $output =
                                    '<img src="' + assetPath + 'images/avatars/' + $user_img + '" alt="Avatar" width="32" height="32">';
                            } else {
                                // For Avatar badge
                                // var stateNum = full['Id'];
                                var states = ['success', 'danger', 'warning', 'info', 'dark', 'primary', 'secondary'];
                                var $state = states[0],
                                    $name = full['ContactName'],
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
                        targets: 3
                    },
                    {

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
                            debugger;
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
                order: [[2, 'asc']],
                dom: '<"card-header border-bottom p-1"<"head-label"><"dt-action-buttons text-end"B>><"d-flex justify-content-between align-items-center mx-0 row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>t<"d-flex justify-content-between mx-0 row"<"col-sm-12 col-md-6"i><"col-sm-12 col-md-6"p>>',
                displayLength: 10,
                lengthMenu: [7, 10, 25, 50, 75, 100],
                buttons: [
                    //{
                    //    extend: 'collection',
                    //    className: 'btn btn-outline-secondary dropdown-toggle me-2',
                    //    text: 'Select Group',
                    //    buttons: [


                    //    ],
                    //    init: function (api, node, config) {
                    //        return (
                    //            '<div class="mb-1">'
                    //            + '<label class="form-label" for="basicSelect">Basic Select</label>' +
                    //            '<select class="form-select" id="basicSelect">' +
                    //            '<option>IT</option>' +
                    //            '<option>Blade Runner</option>' +
                    //            '<option>Thor Ragnarok</option>' +
                    //            '</select>' +
                    //            '</div>'

                    //        );
                    //    }
                    //},
                    //{
                    //    text: feather.icons['plus'].toSvg({ class: 'me-50 font-small-4' }) + 'Add New Group',
                    //    className: 'create-new btn btn-primary',
                    //    attr: {
                    //        'data-bs-toggle': 'modal',
                    //        'data-bs-target': '#modals-group'
                    //    },
                    //    init: function (api, node, config) {
                    //        $(node).removeClass('btn-secondary');
                    //    }
                    //},
                    //{
                    //    text: feather.icons['plus'].toSvg({ class: 'me-50 font-small-4' }) + 'Add New Entry',
                    //    className: 'create-new btn btn-primary',
                    //    attr: {
                    //        'data-bs-toggle': 'modal',
                    //        'data-bs-target': '#modals-slide-in'
                    //    },
                    //    init: function (api, node, config) {
                    //        $(node).removeClass('btn-secondary');
                    //    }
                    //}

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
            $('div.head-label').html('<h5>Bulk Topup</h5>');

        }

        // Flat Date picker
        if (dt_date_table.length) {
            dt_date_table.flatpickr({
                monthSelectorType: 'static',
                dateFormat: 'm/d/Y'
            });
        }

        // Add New record
        // ? Remove/Update this code as per your requirements ?
        //var count = 101;
        //$('.data-submit').on('click', function () {
        //    debugger;
        //    var $new_name = $('.add-new-record .dt-full-name').val(),
        //        $new_post = $('.add-new-record .dt-post').val(),
        //        $new_salary = $('.add-new-record .dt-salary').val();

        //    if ($new_post != '') {
        //        dt_basic.row
        //            .add({
                        
        //                Id:0,
        //                COntactName: $new_name,
        //                ContactPhone: $new_post,
        //                Amount: $new_salary
                       
        //            })
        //            .draw();
        //        count++;
        //        $('.modal').modal('hide');
        //    }
        //});


        //edit record
        $('.datatables-basic tbody').on('click', '.item-edit', function () {

            debugger;
            var index = dt_basic.row($(this).parents('tr')).index();
                //dt_basic.row(this).index();
            /*myModel.ContactName2(data.ContactName);*/
            //myModel.ContactEmail2(data.ContactEmail);
            //myModel.ContactPhone2(data.ContactPhone);
            //myModel.Id(data.Id);
            //myModel.selectedCountry(data.Country);
            myModel.Index(index);
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
                jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);


            };
            debugger;


            jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Contacts/AccountEntityContacts/Delete", data,
                functionDone, functionFailed);

        });

        var count = 101;
        $('.data-submit').on('click', function () {
          
        });

        // Delete Record
        //$('.datatables-basic tbody').on('click', '.delete-record', function () {
        //    dt_basic.row($(this).parents('tr')).remove().draw();
        //});

        // Complex Header DataTable
        // --------------------------------------------------------------------

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

        // Multilingual DataTable
        // --------------------------------------------------------------------

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

