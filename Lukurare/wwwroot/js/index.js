 var MVVM = {
    setNetworksChart: function () {

        /* var $textMutedColor = '#b9b9c3';*/
        debugger;
        var reportChartOptions = {
           
            series: [
                {
                    name: "Water",
                    data: []
                },
                {
                    name: "Rates",
                    data: []
                },
                //{
                //    name: "Telkom",
                //    data: [0, 0, 0, 0, 0, 0, 0]
                //}
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
            colors: [window.colors.solid.primary, window.colors.solid.warning/*'#b9b9c3', '#ee1b24', '#fff100'*/],//#77B6EA#545454
            dataLabels: {
                enabled: true,
            },
            stroke: {
                curve: 'smooth'
            },
            title: {
                text: 'Last one week Collection',
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
                /* categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul'],*/
                title: {
                    text: 'Dates'
                }
            },
            yaxis: {
                title: {
                    text: 'Amount(Ksh)'
                }
            },
            legend: {
                position: 'top',
                horizontalAlign: 'right',
                floating: true,
                offsetY: -25,
                offsetX: -5
            }

        };
        debugger;
        var chart = new ApexCharts(document.querySelector("#networks-chart"), reportChartOptions);
        chart.render();

        var functionDone = function (hasError, message, data) {
            debugger;
            if (hasError) {
                $.alert({ title: 'Failed', icon: 'icon-exclamation', content: message, type: 'red', typeAnimated: true, });
            } else {
                debugger;
                data = data.Result;
                for (i = 0; i < data.length; i++) {
                    for (j = 0; j < data[i].length; j++) {
                        var date = new Date(data[i][j].x);
                        data[i][j].x = moment(date).format('MMM-DD');

                    }


                }
                chart.updateSeries([{
                    name: 'Water',

                    data: data[0]


                },
                {
                    name: 'Rent',

                    data: data[1]


                },
                    //{
                    //    name: 'Telkom',

                    //    data: data[2]


                    //}



                ])
            }
            //data;///data contains the result

        };
        var functionFailed = function (hasError, message, data) {
            debugger;
            if (hasError) {
                jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
            }
            //jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);


        };
        debugger;
        //var data = {
        //    StartDate: '',
        //    EndDate: ''
        //};
        jqueryAjaxGenerics.createJSONAjaxGETRequest(/*"POST",*/ "/DashBoard/DashBoard/GetCharts",/* data,*/
            functionDone, functionFailed);
    },

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
                border: '0',
                zIndex: 19999
            },
            overlayCSS: {
                opacity: 0.5
            }
        });
    },

    unBlockUiPageElement: function (element) {
        $.unblockUI();
    },

    init: function () {
        var viewModel = function () {

            this.ProductName = ko.observable('');
            this.ProductPrice = ko.observable('');
            this.path = ko.observable('');
            this.cart = ko.observableArray([]);
            this.products = ko.observableArray([]);

            this.productInfo = function () {
                var self = this;
                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        var product = data.Result;
                        if (product.length == 0) {
                            return;
                        }
                        self.ProductName(product[1]?.Value || 'null');
                        self.ProductPrice(product[0]?.Value || 'null');

                        var productData = {
                            type: 'productType', // Replace with actual type if available
                            title: self.ProductName(),
                            primaryImage: self.path(), // Assuming path observable is set somewhere
                            hoverImage: self.path(),
                            price: 'Ksh' + self.ProductPrice(),
                            oldPrice: '0',
                            rating: 5
                        };

                        self.products.push(productData);
                    }
                };

                var functionFailed = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                    }
                };

                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Accounts/SelectAgent/CustomerInfo", functionDone, functionFailed);
            };

            this.productInfo();

            this.uploadedDocuments = function () {
                var self = this;
                var functionDone = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        var documents = [];
                        data.Result.forEach(function (item) {
                            var documentData = {
                                Id: item.Id,
                                DocumentName: item.RequiredDocuments.IdentificationDocumentType.DocumentName,
                                Path: item.Path,
                                Validated: item.Validated,
                                Status: item.Validated ? 'Approved' : 'Not Approved'
                            };
                            documents.push(documentData);
                        });
                        self.Documents(documents);
                        $('#documentTable').DataTable();
                    }
                };

                var functionFailed = function (hasError, message, data) {
                    if (hasError) {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
                    } else {
                        jqueryConfirmGenerics.showOkAlertBox('Failed!', data.Message, "red", null);
                    }
                };

                jqueryAjaxGenerics.createJSONAjaxGETRequest("/Accounts/SelectAgent/UploadedDocuments", functionDone, functionFailed);
            };

            this.uploadedDocuments();

            this.sortedImages = ko.computed(function () {
                var self = this;
                var images = [];
                var productTypes = ['wig', 'shirt', 'trouser', 'shoe'];

                productTypes.forEach(function (type) {
                    var product = self.cart().find(function (item) { return item.type === type; });
                    if (product) {
                        images.push({
                            src: product.primaryImage,
                            alt: product.title,
                            title: product.title
                        });
                    }
                });

                return images;
            });

            this.addToCart = function (product) {
                var self = this;
                self.cart.remove(function (item) { return item.type === product.type; });
                self.cart.push(product);
            };
        };

        var myModel = new viewModel();
        ko.applyBindings({ viewModel: myModel });
    }
};

$(document).ready(function () {
    MVVM.init();
});
