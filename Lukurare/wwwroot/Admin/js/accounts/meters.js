






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
            this.Phone1 = ko.observable();
            this.PhysicalAddress = ko.observable();
            this.Email = ko.observable();
            this.Balance = ko.observable();
            this.Id = ko.observable(0);
            this.selectedOption = ko.observable(true);
            this.PhotoLinkUrl = ko.observable();
            this.EntityUserName2 = ko.observable();
            this.Phone12 = ko.observable();
            this.Email2 = ko.observable();
            this.color = ko.observable("red");
            this.Brand = ko.observable();
            this.ParentPhone = ko.observable();
            this.ParentEmail = ko.observable();
            this.MeterNumber = ko.observable();
            this.EntityName = ko.observable();
            this.EntityId = ko.observable();
            this.CurrentMeterReading = ko.observable();
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

                var data = {
                    Id: self.Id(),
                    EntityUserName: self.EntityUserName(),
                    Phone1: self.Phone1(),
                    Email: self.Email(),
                    IsActive: self.selectedOption()
                };
                debugger;
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Accounts/Resellers/Add", data,
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
                        self.Phone12("");
                        self.Email2("");
                        self.MeterNumber("");
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
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("PUT", "/Accounts/Resellers/Update", data,
                    functionDone, functionFailed);

            }
            this.addMeter = function () {
                debugger;
                var self = this;
                self.color("red");
                this.hasError(false);
                var functionDone = function () {
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

                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }
                    jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);

                }
                debugger;
                var data = {
                    Id: 0,
                    MfsAccountTypeId: 0,
                    DateCreated: new Date(),
                    AccountNumber: self.MeterNumber(),
                    AccountName: self.EntityName(),
                    AccountEntity: {
                        EntityName: self.EntityName(),
                        Phone1: self.Phone1(),
                        PhysicalAddress: self.PhysicalAddress()
                    },
                    MfsCurrencyTypeId: 0,
                    IsActive: true,
                    SystemTransactionId: 0
                };
                debugger;
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Accounts/MeterAccount/Add", data,
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

                        self.Brand(data.EntityName);

                        self.PhotoLinkUrl(data.ProfileImageUrl);

                    }

                };
                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }

                    self.Brand(data.EntityName);

                    self.PhotoLinkUrl(data.ProfileImageUrl);

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
        }
        var myModel = new viewModel();
        ko.applyBindings(myModel);

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
                        var data = data.Result;

                        for (var i = 0; i < data.length; i++) {
                            data[i].AccountNumber = data[i].MfsEntityAccount.AccountNumber;
                            data[i].EntityUserName = data[i].MfsEntityAccount.AccountEntity.EntityUserName;
                            data[i].EntityName = data[i].MfsEntityAccount.AccountEntity.EntityName;
                            data[i].CurrentMeterReading = data[i].MfsEntityAccount.AccountName;
                            data[i].PhysicalAddress = data[i].MfsEntityAccount.AccountEntity.PhysicalAddress;
                            if (data[i].MeterStatus) {
                                data[i].MeterStatus = "Open"
                            } else {
                                data[i].MeterStatus = "Closed"
                            }

                        }

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

                    jqueryAjaxGenerics.createJSONAjaxGETRequest("/Accounts/MeterAccount/GetMeterState",
                        functionDone, functionFailed);

                },

                columns: [
                   { data: 'Id' },
                    { data: 'Id' },
                    { data: 'EntityName' },
                    { data: 'PhysicalAddress' },
                    { data: 'AccountNumber' },
                    { data: 'CurrentMeterReading' },
                    { data: 'MeterStatus' },
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
                    {
                        targets: 6,
                        visible: true
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

                                '</div>' +
                                '</div>'

                            );
                        }
                    }
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
                                return 'Details of ' + data['AccountNumber'];
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
            $('div.head-label').html('<h5>Water Meters</h5>');

        }

        // Flat Date picker
        if (dt_date_table.length) {
            dt_date_table.flatpickr({
                monthSelectorType: 'static',
                dateFormat: 'm/d/Y'
            });
        }


        //edit record
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
