var logOutUser = function (e) {
    authenticationHelper.endSessionCurrent();
    if (e)
        e.preventDefault();
};
//var functionSetSelectedItem = function () {

//    var previosClickedLink = localStorage.getItem("selectedMenu");
//    if (previosClickedLink) {
//        var children = [];


//        $('a').each(function (e) {
//            children.push($(this));
//        });

//        for (var i = 0; i < children.length; i++) {
//            var currentChild = children[i];
//            var href = currentChild.attr('href');
//            if (href && href === previosClickedLink) {
//                currentChild.parent().addClass("active");
//            }
//            else {
//                currentChild.parent().removeClass("active");
//            }
//        }
//    }
//};
//$(window).on('load', function () {

//    if (feather) {
//        feather.replace({
//            width: 14,
//            height: 14
//        });
//    }
//    $("a").click(function (e) {
//        debugger;
//        var link = $(this).attr('href');
//        localStorage.setItem("selectedMenu", link);
//        $(this).parent().addClass("active");
//    });
//    functionSetSelectedItem();
//});



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



