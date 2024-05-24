
var MVVM = {

    setNetworksChart: function () {

        /* var $textMutedColor = '#b9b9c3';*/

        var reportChartOptions = {
            series: [
                {
                    name: "Safaricom",
                    data: [0, 0, 0, 0, 0, 0, 0]
                },
                {
                    name: "Airtel",
                    data: [0, 0, 0, 0, 0, 0, 0]
                },
                {
                    name: "Telkom",
                    data: [0, 0, 0, 0, 0, 0, 0]
                }
            ],
            chart: {
                height: 350,
                type: 'line',
                dropShadow: {
                    enabled: true,
                    color: '#000',
                    top: 18,
                    left: 7,
                    blur: 10,
                    opacity: 0.2
                },
                toolbar: {
                    show: false
                }
            },
            colors: ['#52b44b', '#ee1b24', '#fff100'],//#77B6EA#545454
            dataLabels: {
                enabled: true,
            },
            stroke: {
                curve: 'smooth'
            },
            title: {
                text: 'Totals per Network',
                align: 'left'
            },
            grid: {
                borderColor: '#e7e7e7',
                row: {
                    colors: ['#f3f3f3', 'transparent'], // takes an array which will be repeated on columns
                    opacity: 0.5
                },
            },
            markers: {
                size: 1
            },
            xaxis: {
                categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul'],
                title: {
                    text: 'Dates'
                }
            },
            yaxis: {
                title: {
                    text: 'Total Allocations per Network'
                }
                //min: 5,
                //max: 40
            },
            legend: {
                position: 'top',
                horizontalAlign: 'right',
                floating: true,
                offsetY: -25,
                offsetX: -5
            }
            //chart: {
            //    height: 230,
            //    stacked: true,
            //    type: 'line',
            //    toolbar: { show: false }
            //},
            //plotOptions: {
            //    line: {
            //        columnWidth: '17%',
            //        endingShape: 'rounded'
            //    },
            //    distributed: true
            //},
            //colors: [window.colors.solid.primary, window.colors.solid.warning],
            //series: [
            //    //{
            //    //    name: 'Guides',
            //    //    data: [95, 177, 284, 256, 105, 63, 168, 218, 72]
            //    //}
            //    //},
            //    //{
            //    //    name: 'Expense',
            //    //    data: [-145, -80, -60, -180, -100, -60, -85, -75, -100]
            //    //}
            //],
            //dataLabels: {
            //    enabled: false
            //},
            //legend: {
            //    show: false
            //},
            //grid: {
            //    padding: {
            //        top: -20,
            //        bottom: -10
            //    },
            //    yaxis: {
            //        lines: { show: false }
            //    }
            //},
            //xaxis: {
            //    /* categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep'],*/
            //    labels: {
            //        style: {
            //            colors: $textMutedColor,
            //            fontSize: '0.86rem'
            //        }
            //    },
            //    axisTicks: {
            //        show: false
            //    },
            //    axisBorder: {
            //        show: false
            //    }
            //},
            //yaxis: {
            //    labels: {
            //        style: {
            //            colors: $textMutedColor,
            //            fontSize: '0.86rem'
            //        }
            //    }
            //}
        };
        var chart = new ApexCharts(document.querySelector("#networks-chart"), reportChartOptions);
        chart.render();
        //this.$lessonGuideSentReportChart = document.querySelector('#networks-chart');
        //var reportChart = new ApexCharts(this.$NetworksReportChart, reportChartOptions);
        //reportChart.render();
        var functionDone = function (hasError, message, data) {
            debugger;
            if (hasError) {
                $.alert({ title: 'Failed', icon: 'icon-exclamation', content: message, type: 'red', typeAnimated: true, });
            } else {
                for (i = 0; i < data.length; i++) {
                    for (j = 0; j < data[i].length; j++) {
                        var date = new Date(data[i][j].x);
                        data[i][j].x = moment(date).format('MMM-DD');
                        //data[j].x = kendo.toString(kendo.parseDate(data[i].x), 'MMM-dd');
                    }
                    //data[i].x = data[i].x.split(' ')[0];
                    //data[i].xlabel=

                }
                chart.updateSeries([{
                    name: 'Safaricom',

                    data: data[0]


                },
                {
                    name: 'Airtel',

                    data: data[1]


                },
                {
                    name: 'Telkom',

                    data: data[2]


                }



                ])
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
        var data = {
            StartDate: '',
            EndDate: ''
        };
        jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/DashBoard/Dashboard/GetNetworks", data,
            functionDone, functionFailed);

    },
    initCharts: function () {
        debugger;

    },
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

            //this.checkBalance();
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

                        self.ParentPhone(data.Phone1);

                        self.ParentEmail(data.Email);

                    }

                };
                var functionFailed = function (hasError, message, data) {
                    debugger;
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    }

                    self.ParentPhone(data.Phone1);

                    self.ParentEmail(data.Email);

                };
                debugger;
                data = {

                };
                jqueryAjaxGenerics.createJSONAjaxNonGETRequest("GET", "/Accounts/AccountProfiles/GetParentDetails",
                    functionDone, functionFailed);

            }
            //this.setUpGlobalVariables();
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

                        self.Brand(data.Result.Result[0].EntityName);
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
                            data[i].Id = data[i].Id;
                            data[i].AmountInCredit = data[i].AmountInCredit;
                            data[i].AmountOutDebit = data[i].AmountOutDebit;
                            data[i].TransactionDate = data[i].TransactionDate;
                            data[i].Narration = data[i].Narration;
                            data[i].AccountNumber = data[i].MfsEntityAccount.AccountNumber;
                            data[i].EntityName = data[i].MfsEntityAccount.AccountEntity.EntityName;
                            data[i].EntityUserName = data[i].MfsEntityAccount.AccountEntity.EntityUserName;
                            data[i].TransactionNo = data[i].MfsSystemTransactionReceipt.ReceiptNo;//MfsEntityAccount.SystemTransaction.TransactionNo

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
                    var modelToSend = {

                        AccountTypeName: "Rent Account"

                    };


                    jqueryAjaxGenerics.createJSONAjaxNonGETRequest("POST", "/Reports/CreditAllocationReport/GetReport", modelToSend,
                        functionDone, functionFailed);


                }, // JSON file to add data
                autoWidth: false,
                columns: [
                    // columns according to JSON
                    { data: 'Id' },

                    { data: 'EntityName' },//
                    { data: 'AccountNumber' },//
                    { data: 'TransactionNo' },
                    { data: 'AmountInCredit' },//AccountEntityContact.BillReferenceServiceType.ServiceName
                    { data: 'AmountOutDebit' },//SystemTransaction.TransactionDate, { data: '' },
                    { data: 'TransactionDate' },//Success
                    { data: 'Narration' }

                ],
                columnDefs: [
                    {
                        // For Responsive
                        className: 'control',
                        responsivePriority: 2,
                        visible: false,
                        targets: 0
                    },

                    {
                        // Client name and Service  width: '100px',
                        targets: 1,
                        responsivePriority: 4,

                        render: function (data, type, full, meta) {
                            var $name = full['EntityName'],
                                $username = full['EntityUserName'],
                                $image = full['avatar'],
                                stateNum = Math.floor(Math.random() * 6),
                                states = ['success', 'danger', 'warning', 'info', 'primary', 'secondary'],
                                $state = states[stateNum],
                                $name = full['EntityName'],
                                $initials = $name.match(/\b\w/g) || [];
                            $initials = (($initials.shift() || '') + ($initials.pop() || '')).toUpperCase();
                            if ($image) {
                                // For Avatar image
                                var $output =
                                    '<img  src="' + assetPath + 'images/avatars/' + $image + '" alt="Avatar" width="32" height="32">';
                            } else {
                                // For Avatar badge
                                $output = '<div class="avatar-content">' + $initials + '</div>';
                            }
                            // Creates full output for row
                            var colorClass = $image === '' ? ' bg-light-' + $state + ' ' : ' ';

                            var $rowOutput =
                                '<div class="d-flex justify-content-left align-items-center">' +
                                '<div class="avatar-wrapper">' +
                                '<div class="avatar' +
                                colorClass +
                                'me-50">' +
                                $output +
                                '</div>' +
                                '</div>' +
                                '<div class="d-flex flex-column">' +
                                '<h6 class="user-name text-truncate mb-0">' +
                                $name +
                                '</h6>' +
                                '<small class="text-truncate text-muted">' +
                                $username +
                                '</small>' +
                                '</div>' +
                                '</div>';
                            return $rowOutput;
                        }
                    },
                    {
                        // phone width: '73px',
                        targets: 2,

                        render: function (data, type, full, meta) {
                            var $total = full['AccountNumber'];
                            return $total;
                        }
                    },
                    {
                        //  Amount width: '73px',
                        targets: 3,

                        render: function (data, type, full, meta) {
                            var $total = full['TransactionNo'];
                            return $total;
                        }
                    },
                    {
                        // network width: '73px',
                        targets: 4,

                        render: function (data, type, full, meta) {
                            var $total = full['AmountInCredit'];
                            return $total;
                        }
                    },
                    {
                        // network width: '73px',
                        targets: 5,

                        render: function (data, type, full, meta) {
                            var $total = full['AmountOutDebit'];
                            return $total;
                        }
                    },

                    {
                        // Due Date width: '130px',
                        targets: 6,

                        render: function (data, type, full, meta) {
                            var $dueDate = new Date(full['TransactionDate']);
                            // Creates full output for row
                            var $rowOutput =

                                moment($dueDate).format('DD MMM YYYY HH:mm:ss');
                            /*$dueDate;*/
                            return $rowOutput;
                        }
                    },
                    {
                        // network width: '73px',
                        targets: 7,

                        render: function (data, type, full, meta) {
                            var $total = full['Narration'];
                            return $total;
                        }
                    },

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
                buttons: [{
                    extend: 'collection',
                    className: 'btn btn-outline-secondary dropdown-toggle me-2',
                    text: feather.icons['external-link'].toSvg({ class: 'font-small-4 me-50' }) + 'Export',
                    buttons: [
                        {
                            extend: 'print',
                            text: feather.icons['printer'].toSvg({ class: 'font-small-4 me-50' }) + 'Print',
                            className: 'dropdown-item',
                            exportOptions: { columns: [1, 2, 3, 4, 5, 6, 7] }
                        },
                        {
                            extend: 'csv',
                            text: feather.icons['file-text'].toSvg({ class: 'font-small-4 me-50' }) + 'Csv',
                            className: 'dropdown-item',
                            exportOptions: { columns: [1, 2, 3, 4, 5, 6, 7] }
                        },
                        {
                            extend: 'excel',
                            text: feather.icons['file'].toSvg({ class: 'font-small-4 me-50' }) + 'Excel',
                            className: 'dropdown-item',
                            exportOptions: { columns: [1, 2, 3, 4, 5, 6, 7] }
                        },
                        {
                            extend: 'pdf',
                            text: feather.icons['clipboard'].toSvg({ class: 'font-small-4 me-50' }) + 'Pdf',
                            className: 'dropdown-item',
                            exportOptions: { columns: [1, 2, 3, 4, 5, 6, 7] }
                        }
                    ],
                    init: function (api, node, config) {
                        $(node).removeClass('btn-secondary');
                        $(node).parent().removeClass('btn-group');
                        setTimeout(function () {
                            $(node).closest('.dt-buttons').removeClass('btn-group').addClass('d-inline-flex mt-50');
                        }, 50);
                    }
                }],
                responsive: {
                    details: {
                        display: $.fn.dataTable.Responsive.display.modal({
                            header: function (row) {
                                var data = row.data();
                                return 'Details of ' + data['EntityName'];
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
                    this.api()
                        .columns(6)
                        .every(function () {
                            var column = this;
                            var select = $(
                                '<select id="UserRole" class="form-select ms-50 text-capitalize"><option value="">Filter By</option></select>'
                            )
                                .appendTo('.invoice_status')
                                .on('change', function () {
                                    //var val = $.fn.dataTable.util.escapeRegex($(this).val());
                                    //column.search(val ? '^' + val + '$' : '', true, false).draw();
                                    if (select.val() == "Date") {
                                        $('#DescModal').modal("show");
                                        $('#sendDateFilter').on('click', function () {
                                            debugger;
                                            $.fn.dataTable.ext.search.push(

                                                function (settings, data, dataIndex) {
                                                    debugger;
                                                    var min = new Date(moment($('#from-date-time').val()).format('DD MMM YYYY HH:mm:ss'));
                                                    var max = new Date(moment($('#to-date-time').val()).format('DD MMM YYYY HH:mm:ss'));
                                                    /*var date = new Date(data[6]);*/
                                                    var date = new Date(data[6]);
                                                    if (

                                                        (min <= date && date <= max)
                                                    ) {
                                                        return true;

                                                        date = "";

                                                    }
                                                    return false;
                                                    date = "";
                                                }
                                            );

                                            dtInvoiceTable.DataTable().draw();
                                        });
                                        $('#reset').on('click', function () {
                                            $('#DescModal').modal("hide");
                                        }
                                        );
                                    } else {

                                    }

                                });
                            select.append('<option value="Date" class="text-capitalize">Date Range</option>')
                        });

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

    MVVM.init();
});