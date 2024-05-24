var MVVM = {
    init: function () {
        var viewModel = function () {
            var self = this;
            debugger;
            this.chatmessages = [
                
                {
                    "firstName": "Anna",
                    "lastName": "Smith",
                    "skills": [
                        {
                            "name": "css",
                            "rating": 5
                        },
                        {
                            "name": "javascript",
                            "rating": 5
                        }
                    ]
                },
                {
                    "firstName": "Peter",
                    "lastName": "Jones",
                    "skills": [
                        {
                            "name": "html",
                            "rating": 5
                        },
                        {
                            "name": "javascript",
                            "rating": 3
                        }
                    ]
                }
            ];
            self.selectedEmployee = ko.observable(null);
            self.selectEmployee = function (employee) {
                self.selectedEmployee(employee);
            };

        }
        var myModel = new viewModel();
        ko.applyBindings(myModel);
    }

};
$(document).ready(function () {
    debugger;

    
    MVVM.init();
});