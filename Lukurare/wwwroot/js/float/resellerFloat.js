


/**
 * DataTables Basic
 */
var MVVM = {
    init: function () {
        var viewModel = function () {
            debugger;

            this.hasError = ko.observable(false);
            this.errorMessage = ko.observable();
            this.AccountName = ko.observable();
            this.AccountNumber = ko.observable();
            this.AccountName2 = ko.observable();
            this.AccountNumber2 = ko.observable();
            this.selectedActive= ko.observable();
            this.Currencies = ko.observableArray([]);
            this.AccountTypes = ko.observableArray([]);
            this.selectedCurrency = ko.observable(0);
            this.selectedAccountType = ko.observable(0);
          
            this.Balance = ko.observable();
            this.Id= ko.observable(0);
            this.PhotoLinkUrl = ko.observable();
            this.color = ko.observable("red");
            this.showErrorMessage = function (title, message) {
                var self = this;
                debugger;
                try {
                    this.hasError(true);
                    this.errorMessage(title + ':' + message);


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

            //this.checkBalance();
            this.checkAccounts= function () {
                debugger;
                var self = this;
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message,"red",null);
                    } else {
                        self.AccountTypes(data.Result);
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

                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Accounts/ResellerFloatAccounts/GetAccountTypes",
                    functionDone, functionFailed);

            }

            this.checkAccounts();
            this.checkCurrencies = function () {
                debugger;
                var self = this;
                this.hasError(false);
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

                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Accounts/ResellerFloatAccounts/GetCurrencies",
                  functionDone, functionFailed);

            }

            this.checkCurrencies();
           

           

            this.addAccount = function () {
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
                        $('.datatables-basic').DataTable().ajax.reload();
                        self.AccountName("");
                        self.AccountNumber("");
                     
                        
                       

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
                    AccountName: self.AccountName(),
                    AccountNumber: self.AccountNumber(),
                    MfsAccountType: self.selectedAccountType(),
                    MfsCurrencyType:self.selectedCurrency()
                   
                };
                debugger;
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Accounts/ResellerFloatAccounts/Add", data,
                    functionDone, functionFailed);
                   
                
             


            }
            this.editAccount = function () {
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
                        $('.datatables-basic').DataTable().ajax.reload();
                        //self.EntityUserName2("");
                        //self.Phone12("");
                        //self.Email2("");
                        //self.selectedOption(true);
                        

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
                    AccountName: self.AccountName2(),
                    AccountNumber: self.AccountNumber2(),
                    MfsAccountType: self.selectedAccountType(),
                    MfsCurrencyType: self.selectedCurrency(),
                    IsActive:self.selectedActive()
                };
                debugger;
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("PUT", "/Accounts/ResellerFloatAccounts/Update", data,
                    functionDone, functionFailed);


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
                    url: '/Accounts/Users/UploadAccounts',
                    dataType: 'json',
                    processData: false,
                    contentType: false,
                    data: formData,
                    success: function (data) {
                        //jqueryConfirmGenerics.showOkAlertBox('success', data.Message, null);
                        if (data.IsOkay) {
                            $('.datatables-basic').DataTable().ajax.reload();
                            
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
                        var data = data.Result.Result;
                        //for (var i = 0; i <= data.length; i++) {
                        //    if (data[i].Username == null) {
                        //        data[i].Username= false;
                        //    }
                        //}

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



                    jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Accounts/ResellerFloatAccounts/GetKendoGridFiltered", modelToSend,
                            functionDone, functionFailed);



                    ///GetKendoGridFiltered
                        //var urlName = "/Contacts/AirtimeGroupContacts/GetKendoGridFilteredFromGroupId/" + myModel.selectedGroup().Id

                        //jqueryAjaxGenerics.createJSONAjaxGETRequest(urlName, functionDone, functionFailed);



                },

                columns: [

                    { data: 'Id' },
                    { data: 'Id' },
                    { data: 'Id' },

                    // used for sorting so will hide this column
                    { data: 'AccountName' },
                    { data: 'AccountNumber' },
                    { data: 'IsActive' },
                    /*{ data: 'IsActive' },*/
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
                        text: feather.icons['plus'].toSvg({ class: 'me-50 font-small-4' }) + 'Add New Account',
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
            $('div.head-label').html('<h5>Reseller Float Accounts</h5>');

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
            var data = dt_basic.row($(this).parents('tr')).data();
          myModel.AccountName2(data.AccountName);
          myModel.AccountNumber2(data.AccountNumber);
          myModel.selectedActive(data.IsActive);
            myModel.Id(data.Id);
            //myModel.selectedOption(data.IsActive);

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


            jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Accounts/AccountTypes/Delete", data,
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
