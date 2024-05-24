


/**
 * DataTables Basic
 */
var MVVM = {
    init: function () {
        var viewModel = function () {
            debugger;

            this.hasError = ko.observable(false);
            this.errorMessage = ko.observable();
            this.Balance = ko.observable(0);
            this.PhotoLinkUrl = ko.observable();
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
                        debugger;
                        self.Balance(data.balance);
                        //$.confirm({
                        //    title: 'Confirm!',
                        //    content: 'Simple confirm!',
                        //    buttons: {
                        //        confirm: function () {
                        //            $.alert('Confirmed!');
                        //        },
                        //        cancel: function () {
                        //            $.alert('Canceled!');
                        //        },
                        //        somethingElse: {
                        //            text: 'Something else',
                        //            btnClass: 'btn-blue',
                        //            keys: ['enter', 'shift'],
                        //            action: function () {
                        //                $.alert('Something else?');
                        //            }
                        //        }
                        //    }
                        //});

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

                        self.Brand(data.Result.Result[0].EntityName);
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
        ko.applyBindings(myModel);

       
            'use strict';

            var dtInvoiceTable = $('.invoice-list-table'),
                assetPath = '/app-assets/',
                invoicePreview = 'app-invoice-preview.html',
                invoiceAdd = 'app-invoice-add.html',
                invoiceEdit = 'app-invoice-edit.html';

        if ($('body').attr('data-framework') === 'laravel') {
            assetPath = $('body').attr('data-asset-path');
            invoicePreview = assetPath + 'app/invoice/preview';
            invoiceAdd = assetPath + 'app/invoice/add';
            invoiceEdit = assetPath + 'app/invoice/edit';
        }
            // datatable
            if (dtInvoiceTable.length) {
                var dtInvoice = dtInvoiceTable.DataTable({
                    ajax: function (data, callback, settings) {
                        debugger;


                        var functionDone = function (hasError, message, data) {
                            debugger;
                            if (hasError) {
                                //myModel.showErrorMessage("failed", message);
                            }
                            var data = data.Result.Result;
                            for (var i = 0; i < data.length; i++) {
                               
                                    data[i].ReceiptNo = data[i].MfsSystemTransactionReceipt.ReceiptNo;

                                    data[i].Amount = data[i].AmountInCredit
                                
                              



                            }
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
                        
                     

                            jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Accounts/AccountProfiles/GetFloatList", modelToSend,
                       functionDone, functionFailed);
                       

                    }, // JSON file to add data
                    autoWidth: false,
                    columns: [
                        // columns according to JSON
                        { data: 'Id' },
                        { data: 'ReceiptNo' },//SystemTransaction.TransactionNo
                        { data: 'Amount' },
                        { data: 'TransactionDate' },//SystemTransaction.TransactionDate, { data: '' },
                        { data: 'Narration' }
                    
                      
                    ],
                    columnDefs: [
                        {
                            // For Responsive
                            className: 'control',
                            responsivePriority: 2,
                            visible:true,
                            targets: 0
                        },
                        {
                            // Invoice ID
                            targets: 1,
                            /*width: '46px',*/
                            //render: function (data, type, full, meta) {
                            //    var $invoiceId = full['ReceiptNo'];
                            //    // Creates full output for row
                            //    var $rowOutput = '<a class="fw-bold" href="' + invoicePreview + '">' + $invoiceId + '</a>';
                            //    return $rowOutput;
                            //}
                        },
                       
                      
                        {
                            //  Amount
                            targets: 2,
                           /* width: '73px',*/
                            render: function (data, type, full, meta) {
                                var $total = full['Amount'];
                                return $total;
                            }
                        },
                       
                        {
                            // Due Date
                            targets: 3,
                           /* width: '130px',*/
                            render: function (data, type, full, meta) {
                                var $dueDate = new Date(full['TransactionDate']);
                                // Creates full output for row
                                var $rowOutput =
                                    '<span class="d-none">' +
                                    moment($dueDate).format('YYYYMMDD') +
                                    '</span>' +
                                    moment($dueDate).format('DD MMM YYYY HH:mm:ss');
                                $dueDate;
                                return $rowOutput;
                            }
                        },
                        
                        {
                            targets: 4,
                          
                        },
                        //{
                        //    // Actions
                        //    targets: -1,
                        //    title: 'Actions',
                        //    width: '80px',
                        //    orderable: false,
                        //    render: function (data, type, full, meta) {
                        //        return (
                        //            '<div class="d-flex align-items-center col-actions">' +
                        //            '<a class="me-1" href="#" data-bs-toggle="tooltip" data-bs-placement="top" title="Send Mail">' +
                        //            feather.icons['send'].toSvg({ class: 'font-medium-2 text-body' }) +
                        //            '</a>' +
                        //            '<a class="me-25" href="' +
                        //            invoicePreview +
                        //            '" data-bs-toggle="tooltip" data-bs-placement="top" title="Preview Invoice">' +
                        //            feather.icons['eye'].toSvg({ class: 'font-medium-2 text-body' }) +
                        //            '</a>' +
                        //            '<div class="dropdown">' +
                        //            '<a class="btn btn-sm btn-icon dropdown-toggle hide-arrow" data-bs-toggle="dropdown">' +
                        //            feather.icons['more-vertical'].toSvg({ class: 'font-medium-2 text-body' }) +
                        //            '</a>' +
                        //            '<div class="dropdown-menu dropdown-menu-end">' +
                        //            '<a href="#" class="dropdown-item">' +
                        //            feather.icons['download'].toSvg({ class: 'font-small-4 me-50' }) +
                        //            'Download</a>' +
                        //            '<a href="' +
                        //            invoiceEdit +
                        //            '" class="dropdown-item">' +
                        //            feather.icons['edit'].toSvg({ class: 'font-small-4 me-50' }) +
                        //            'Edit</a>' +
                        //            '<a href="#" class="dropdown-item">' +
                        //            feather.icons['trash'].toSvg({ class: 'font-small-4 me-50' }) +
                        //            'Delete</a>' +
                        //            '<a href="#" class="dropdown-item">' +
                        //            feather.icons['copy'].toSvg({ class: 'font-small-4 me-50' }) +
                        //            'Duplicate</a>' +
                        //            '</div>' +
                        //            '</div>' +
                        //            '</div>'
                        //        );
                        //    }
                        //}
                    ],
                    order: [[0, 'desc']],
                    dom:
                        '<"row d-flex justify-content-between align-items-center m-1"' +
                        '<"col-lg-6 d-flex align-items-center"l<"dt-action-buttons text-xl-end text-lg-start text-lg-end text-start "B>>' +
                        '<"col-lg-6 d-flex align-items-center justify-content-lg-end flex-lg-nowrap flex-wrap pe-lg-1 p-0"f<"invoice_status ms-sm-2">>' +
                        '>t' +
                        '<"d-flex justify-content-between mx-2 row"' +
                        '<"col-sm-12 col-md-6"i>' +
                        '<"col-sm-12 col-md-6"p>' +
                        '>',
                    language: {
                        sLengthMenu: 'Show _MENU_',
                        search: 'Search',
                        searchPlaceholder: 'search anything',
                        paginate: {
                            // remove previous & next text from pagination
                            previous: '&nbsp;',
                            next: '&nbsp;'
                        }
                    },
                    // Buttons with Dropdown
                    buttons: [
                        //{
                        //    //text: 'Add Record',
                        //    //className: 'btn btn-primary btn-add-record ms-2',
                        //    //action: function (e, dt, button, config) {
                        //    //    window.location = invoiceAdd;
                        //    //}
                        //}
                    ],
                    // For responsive popup
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
                                    return col.columnIndex !== 2 // ? Do not show row in modal popup if title is blank (for check box)
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
                    initComplete: function () {
                        $(document).find('[data-bs-toggle="tooltip"]').tooltip();
                        // Adding role filter once table initialized
                        //this.api()
                        //    .columns(6)
                        //    .every(function () {
                        //        var column = this;
                        //        var select = $(
                        //            '<select id="UserRole" class="form-select ms-50 text-capitalize"><option value="">Filter By Date</option></select>'
                        //        )
                        //            .appendTo('.invoice_status')
                        //            .on('change', function () {
                        //                var val = $.fn.dataTable.util.escapeRegex($(this).val());
                        //                column.search(val ? '^' + val + '$' : '', true, false).draw();
                        //            });

                        //        column
                        //            .data()
                        //            .unique()
                        //            .sort()
                        //            .each(function (d, j) {
                        //                select.append('<option value="' + d + '" class="text-capitalize">' + d + '</option>');
                        //            });
                        //    });
                        //this.api()
                        //    .columns(7)
                        //    .every(function () {
                        //        var column = this;
                        //        var select = $(
                        //            '<select id="UserRole" class="form-select ms-50 text-capitalize"><option value="">Filter By Group</option></select>'
                        //        )
                        //            .appendTo('.invoice_status')
                        //            .on('change', function () {
                        //                var val = $.fn.dataTable.util.escapeRegex($(this).val());
                        //                column.search(val ? '^' + val + '$' : '', true, false).draw();
                        //            });

                        //        column
                        //            .data()
                        //            .unique()
                        //            .sort()
                        //            .each(function (d, j) {
                        //                select.append('<option value="' + d + '" class="text-capitalize">' + d + '</option>');
                        //            });
                        //    });
                    },
                    drawCallback: function () {
                        $(document).find('[data-bs-toggle="tooltip"]').tooltip();
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
