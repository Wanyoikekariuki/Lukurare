
var MVVM = {
    
   
    init: function () {

        var viewModel = function () {
            debugger;

            this.hasError = ko.observable(false);
            this.errorMessage = ko.observable();
            this.Balance = ko.observable(0);

            this.PhotoLinkUrl = ko.observable();
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
            this.displayTopupMessage = function () {

                jqueryConfirmGenerics.showOkAlertBox("Information", "As we work on automating account topup, kindly contact ****** (email) *****(phone) to load your wallet", "green", null);
            }
            //this.displayTopupMessage();
        }
        var myModel = new viewModel();
        ko.applyBindings(myModel);





    }
};



$(document).ready(function () {

    MVVM.init();
});