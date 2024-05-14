var logOutUser = function (e) {
    authenticationHelper.endSessionCurrent();
    if (e)
        e.preventDefault();
};
var _layout = {
    init: function () {
        authenticationHelper.checkLoggedIn();
    }
};

$(document).ready(function () {
    _layout.init();

});

document.addEventListener('DOMContentLoaded', function () {
    debugger;
    var chooseAndUploadBtn = document.getElementById('chooseAndUploadBtn');
    var fileInput = document.getElementById('fileInput');
    chooseAndUploadBtn.addEventListener('click', function () {
        // Trigger the file input dialog when the button is clicked
        fileInput.click();
    });

    fileInput.addEventListener('change', function () {
        debugger;
        var selectedFile = fileInput.files[0];

        // Check if a file is selected
        if (selectedFile) {
            // Perform the file upload here using Ajax or any other method
            uploadFile(selectedFile);
        } else {
            jqueryConfirmGenerics.showOkAlertBox('Failed!', message, "red", null);
        }
    });

    function uploadFile(file) {
        debugger;
        var formData = new FormData();
        formData.append('file', file);

        $.ajax({
            type: 'POST',
            url: '/Accounts/AccountProfiles/UploadAccounts',
            dataType: 'json',
            processData: false,
            contentType: false,
            data: formData,
            success: function (data) {

                jqueryConfirmGenerics.showOkAlertBox('Message', data.Message, "green", null);
                window.location.reload();


            },
            error: function (err) {
                jqueryConfirmGenerics.showOkAlertBox('Failed', err.Message, "red", null);

            }

        });
    }
});



