var authenticationHelper = {
    navigateToPath: function (url) {
        if (navigator.userAgent.match(/MSIE\s(?!9.0)/)) {
            var referLink = document.createElement("a");
            referLink.href = url;
            document.body.appendChild(referLink);
            referLink.click();
        }     //all other browsers
        else {
            window.location.replace(url);
        }
    },
    //equivalent of log out function
    endSessionCurrent: function () {        
        //end the session and wether is completes or not display navigate to login page
        jqueryAjaxGenerics.createAjaxRequest("GET", "/Authentication/EndSession", "json",
            "application/json", null, function (hasError, message, data) {
                //done function
                authenticationHelper.displayLoginPage();
            },
            function (hasError, message, data) {
                //fail function
                authenticationHelper.displayLoginPage();
            });
    },
    checkLoggedIn: function () {        
        $.ajax({
            type: "GET",
            url: '/Authentication/SessionLoggedIn',
            dataType: "json",
            contentType: "application/json; charset=UTF-8",
        }).done(function (data, status, jqxhr) {
            //dont do any thing
            jqueryAjaxGenerics.onDone(data, status, jqxhr);
            if (data && data.Result) {
                try {

                    $("#userNameLayout").html(data.Result);
                    $("#userNameDashBoard").html(data.Result);
                }
                catch (err) {
                    //dont do any thing
                }
            }
        }).fail(function (jqxhr) {
            if (jqxhr.status === 401) {
                authenticationHelper.displayLoginPage();
                return;
            }
        });
    },
 
    displayLoginPage: function () {
        authenticationHelper.navigateToPath("/Companies/CompanyLogin")
    },
    displayIndexPage: function () {

        authenticationHelper.navigateToPath("/Companies/CompanyProfile")
    },
    cookieManager: {
        createCookie: function (name, value, days) {
            var expires = "";
            if (days) {
                var date = new Date();
                date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
                expires = "; expires=" + date.toGMTString();
            }
            document.cookie = name + "=" + value + expires + "; path=/";
        },
        readCookie: function (name) {
            var nameEQ = name + "=";
            var ca = document.cookie.split(';');
            for (var i = 0; i < ca.length; i++) {
                var c = ca[i];
                while (c.charAt(0) === ' ')
                    c = c.substring(1, c.length);
                if (c.indexOf(nameEQ) === 0)
                    return c.substring(nameEQ.length, c.length);
            }
            return null;
        },
        eraseCookie: function (name) {
            createCookie(name, "", -1);
        },
    }
};