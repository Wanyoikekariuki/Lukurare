


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
            this.Amount = ko.observable();
            this.ReceiptNo = ko.observable();
            this.AccountTypes = ko.observableArray([]);
            this.PayModes = ko.observableArray([]);
            this.selectedPayMode = ko.observable(0);
            this.selectedAccount = ko.observable();
            this.Balance = ko.observable();
            this.PhotoLinkUrl = ko.observable();
            this.color = ko.observable("red");
            this.PercentageCommission = ko.observable();
            this.ServiceId = ko.observable();
            this.ServiceName = ko.observable();
            this.Id = ko.observable();
            this.Commission = ko.observable();
            this.hasBalance = ko.observable(false);
            this.balanceText = ko.observable();
            this.Brand = ko.observable();
            this.ParentPhone = ko.observable();
            this.ParentEmail = ko.observable();
           
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

            this.checkBalance();
            this.checkResellerBalance = function () {
                debugger;
                var self = this;
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        self.hasBalance(true);
                        self.color("green");
                        self.balanceText("Account Balance:" + data.balance.toString())
                        //self.Balance(data.balance.toString());
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

                if (self.selectedAccount()) {
                    var urlName = "/Accounts/ResellerProfiles/GetBalanceFromAccountId/" + self.selectedAccount().Id

                    jqueryAjaxGenerics.createJSONAjaxGETRequest(urlName, functionDone, functionFailed);
                } else {
                    self.hasBalance(false);
                    self.color("red");
                    self.balanceText(null);
                }

            }

            //this.checkResellerBalance();
            this.checkAccounts = function () {
                debugger;
                var self = this;
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message,"red",null);
                    } else {
                        self.AccountTypes(data.Result);
                        $('.datatables-basic').DataTable().ajax.reload();
                        $('.invoice-list-table').DataTable().ajax.reload();
                     
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

                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Accounts/ResellerProfiles/GetEntityAccounts",
                    functionDone, functionFailed);

            }
            this.checkAccounts();
            this.checkPayModes = function () {
                debugger;
                var self = this;
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message,"red",null);
                    } else {
                        self.PayModes(data.Result);
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

                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Accounts/ResellerProfiles/GetPayModes",
                    functionDone, functionFailed);

            }
            this.checkPayModes();

            this.checkTransactions = function () {
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
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Accounts/ResellerProfiles/GetKendoGridFiltered", data,
                    functionDone, functionFailed);

            }




            this.editEntity = function () {

            }

            this.purchaseFloat = function () {
                MVVM.blockUiPageElementWithMessage($('body'), 'Processing.. Please wait');
                debugger;

                var self = this;
                self.color("red");
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message,"red",null);
                    } else {
                        self.color("green");
                       jqueryConfirmGenerics.showOkAlertBox('Success!', data.Message,"green",null);

                        self.ReceiptNo("");
                        self.Amount("");


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
                if (self.Amount() == null) {
                    MVVM.unBlockUiPageElement($('body'));
                    jqueryConfirmGenerics.showOkAlertBox(' Validation failed!', "No amount input", "red", null);
                    return;
                }
                if (self.ReceiptNo() == null) {
                    MVVM.unBlockUiPageElement($('body'));
                    jqueryConfirmGenerics.showOkAlertBox(' Validation failed!', "No reference number given", "red", null);
                    return;
                }
                var balance = parseFloat(self.Balance());
                var amount = parseFloat(self.Amount());
                if (amount > balance) {
                    jqueryConfirmGenerics.showOkAlertBox('Request Failed!', "Insufficient float balance to complete request", "red", null);
                    return;
                }
                var data = {
                    MsfAccountPaymode: self.selectedPayMode(),
                    MfsEntityAccount: self.selectedAccount(),
                    AmountInCredit: self.Amount(),
                    Narration: self.ReceiptNo()


                };
                debugger;
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Accounts/ResellerProfiles/Add", data,
                    functionDone, functionFailed);


            }
            this.editCommission = function () {
                MVVM.blockUiPageElementWithMessage($('body'), 'Processing.. Please wait');
                debugger;

                var self = this;
                self.color("red");
                this.hasError(false);
                var functionDone = function (hasError, message, data) {
                    MVVM.unBlockUiPageElement($('body'));
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message,"red",null);
                    } else {
                        self.color("green");
                       jqueryConfirmGenerics.showOkAlertBox('Success!', data.Message,"green",null);

                        $('.datatables-basic').DataTable().ajax.reload();


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
            
                var data = {
                    Id: self.Id(),
                   PercentageCommission:self.PercentageCommission(),
                    MfsEntityAccount: self.selectedAccount()
             

                };
                debugger;
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("PUT", "/Accounts/Commission/Update", data,
                    functionDone, functionFailed);
            }
            this.selectionChanged = function (obj, event) {
                debugger;

                var self = this;
                if (event.originalEvent) { //user changed
                    debugger;
                    $('.datatables-basic').DataTable().ajax.reload();
                    $('.invoice-list-table').DataTable().ajax.reload();
                    debugger;
                    self.checkResellerBalance();
                } else { // program changed
                    debugger;
                }

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

                        self.Brand(readStringUntilSpace(data.Result.Result[0].EntityName));
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

                        if (data.Message) {
                            debugger;
                            self.PhotoLinkUrl(data.Message)

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
       // ko.applyBindings(myModel);
        var obj = ko.applyBindings(myModel);

        MVVM.myModelUI = obj;


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

                        var data2 = data.Result.Result;
                        for (var i = 0; i < data2.length; i++) {

                            data2[i].ReceiptNo = data2[i].MfsSystemTransactionReceipt.ReceiptNo;

                            data2[i].Amount = data2[i].AmountInCredit;



                        }

                        var objectData = {
                            data: data2
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


                    var modelToSend = "";





                    ///GetKendoGridFiltered
                    if (myModel.selectedAccount()) {
                        var urlName = "/Accounts/ResellerProfiles/GetKendoGridFilteredFromGroupId/" + myModel.selectedAccount().Id

                        jqueryAjaxGenerics.createJSONAjaxGETRequest(urlName, functionDone, functionFailed);
                    } else {

                       
                        jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Accounts/ResellerProfiles/GetKendoGridFiltered",modelToSend,
                            functionDone, functionFailed);
                    }


                },
                autoWidth: false,
                columns: [

                    { data: 'Id' },
                    { data: 'Id' },

                    { data: 'ReceiptNo' },
                    { data: 'Amount' },
                    { data: 'TransactionDate' },
                    { data: 'Narration' }


                ],
                
                columnDefs: [
                    {
                        // For Responsive
                        className: 'control',
                        responsivePriority: 2,
                        targets: 0
                    },
                    {
                        // For Responsive
                        visible: false,
                        targets: 1
                    },
                    {
                        // Invoice ID width: '46px',
                        targets: 2,
                       
                        render: function (data, type, full, meta) {
                            var $invoiceId = full['ReceiptNo'];
                            // Creates full output for row
                            var $rowOutput = '<a class="fw-bold" href="' + invoicePreview + '">' + $invoiceId + '</a>';
                            return $rowOutput;
                        }
                    },


                    {
                        // width: '73px',
                        targets: 3,
                       
                        render: function (data, type, full, meta) {
                            var $total = full['Amount'];
                            return $total;
                        }
                    },

                    {
                        // width: '130px',
                        targets: 4,
                       
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
                        targets:5
                    }
                   
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
                order: [[1, 'desc']],
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


        // --------------------------------------------------------------------


        if (dt_basic_table.length) {
            var dt_basic = dt_basic_table.DataTable({
                ajax: function (data, callback, settings) {
                    debugger;

                    var functionDone = function (hasError, message, data) {
                        debugger;
                        if (hasError) {

                        }
                        var data = data.Result.Result;
                        for (var i = 0; i < data.length; i++) {

                            data[i].ServiceId = data[i].BillReferenceServiceType.ServiceId;
                            data[i].ServiceName = data[i].BillReferenceServiceType.ServiceName;
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




                    if (myModel.selectedAccount()) {
                        var urlName = "/Accounts/Commission/GetKendoGridFilteredFromGroupId/" + myModel.selectedAccount().Id

                        jqueryAjaxGenerics.createJSONAjaxGETRequest(urlName, functionDone, functionFailed);
                    }





                },
                columns: [

                    { data: 'Id' },
                    { data: 'ServiceId' },
                    { data: 'ServiceName' },
                    { data: 'PercentageCommission' },

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
                        // Avatar image/badge, Name and post
                        targets: 1,
                        responsivePriority: 4,

                    },
                    {
                        responsivePriority: 1,
                        targets: 2
                    },

                    {

                        targets: 3
                        //render: function (data, type, full, meta) {
                        //    var $dueDate = new Date(full['TransactionDate']);
                        //    // Creates full output for row
                        //    var $rowOutput =
                        //        //'<span class="d-none">' +
                        //        //moment($dueDate).format('YYYYMMDD') +
                        //        //'</span>' +
                        //        moment($dueDate).format('DD MMM YYYY HH:mm:ss');
                        //    /*$dueDate;*/
                        //    return $rowOutput;
                        //}
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

                    //{
                    //    text: feather.icons['plus'].toSvg({ class: 'me-50 font-small-4' }) + 'Add New Account',
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
        // display: $.fn.dataTable.Responsive.display.modal({
        //    header: function (row) {
        //        var data = row.data();
        //        return 'Details of ' + data['ContactName'];
        //    }
        //}),
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
//$('div.head-label').html('<h5>Reseller Float Accounts</h5>');

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
    myModel.PercentageCommission(data.PercentageCommission);
    myModel.ServiceId(data.ServiceId);
    myModel.ServiceName(data.ServiceName);
    myModel.Id(data.Id);
    //myModel.selectedOption(data.IsActive);

    $("#commission-modal").modal('show');


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
                //display: $.fn.dataTable.Responsive.display.modal({
                //    header: function (row) {
                //        var data = row.data();
                //        return 'Details of ' + data['full_name'];
                //    }
                //}),
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
                //display: $.fn.dataTable.Responsive.display.modal({
                //    header: function (row) {
                //        var data = row.data();
                //        return 'Details of ' + data['full_name'];
                //    }
                //}),
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

    
    MVVM.init();
});
