var jqueryAjaxGenerics = {

    beforeSend: function (xhr) {
       
        var timeZoneOffset = dayjs().format("Z");
        xhr.setRequestHeader('X-DayJS-TimeZone', timeZoneOffset)
    },

    onDone(data, status, jqXHR) {
        //generic method to handle all success
        if (jqXHR.status === 403) {
            //kendo.ui.progress(currentGridName, false);
            jqueryConfirmGenerics.showOkAlertBox('Access Denied', "You dont have rights for this section of the panel,contact your administrator", "red", null);
            
            return;
        }

    },

    onFail(jqXHR, textStatus, errorThrown, fxnAllowProceed) {
        //generic method to handle all fails
        
        try {
            if (jqXHR.status === 401) {
                authenticationHelper.endSessionCurrent();
                return;
            }
            if (jqXHR.status === 403) {
                //kendo.ui.progress(currentGridName, false);
                jqueryConfirmGenerics.showOkAlertBox('Access Denied', "You dont have rights for this section of the panel,contact your administrator", "red", null);

                return;
            }
            fxnAllowProceed(jqXHR);
        }
        catch (err) {
            //if an error occurred also just terminate the session
            //authenticationHelper.endSessionCurrent();
        }
    },

    parseJSON: function (jsonString) {
        try {
            return jQuery.parseJSON(jsonString);
        }
        catch (err) {
            return null;
        }
    },

    createJSONAjaxGETRequest: function (url, doneMethod, failMethod) {
        return jqueryAjaxGenerics.createJSONAjaxNonGETRequest('GET', url, "", doneMethod, failMethod)
    },

    createJSONAjaxNonGETRequest: function (type, url, data, doneMethod, failMethod) {
        return jqueryAjaxGenerics.createAjaxRequest(type, url, "json", "application/json", data, doneMethod, failMethod)
    },

    createAjaxRequest: function (type, url, dataType, contentType,
        data, doneMethod, failMethod) {

        
        var jsonStringData = JSON.stringify(data);
        //if (!data)
        //    jsonStringData = JSON.stringify({});

        $.ajax({
            type: type,
            url: url,
            dataType: dataType,
            contentType: contentType,
            data: jsonStringData,
            beforeSend: jqueryAjaxGenerics.beforeSend
        }).done(function (data, status, jqXHR) {
            try {
                
                //below is the primitive lock mechanism

                jqueryAjaxGenerics.onDone(data, status, jqXHR);

                doneMethod(false, "", data);
            }
            catch (err) {
                doneMethod(true, err.message, data);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            
            
            var functionSuccessOnFail = function (jqXHRi) {
                debugger;
                try {

                    var data = jqueryAjaxGenerics.parseJSON(jqXHRi.responseText);

                    failMethod(false, "", data);
                }
                catch (err) {
                    debugger;
                    failMethod(true, err.message, data);
                }
            };

            jqueryAjaxGenerics.onFail(jqXHR, textStatus, errorThrown, functionSuccessOnFail);

        }).always(function () {
            //clean up code

        });
    },

    ajaxRequestGridFail: function (hasError, errorMessage, data, gridOptions, currentGridName) {

        kendo.ui.progress($("#countriesGridDivId"), false);
        if (hasError === false) {
            //show the error in data
            gridOptions.error({});
            $.alert({
                title: currentGridName + ' Error',
                icon: 'icon-exclamation',
                content: data.Message,
                type: 'red',
                typeAnimated: true,
            });
        }
        else {

            $.alert({
                title: currentGridName + ' Error',
                icon: 'icon-exclamation',
                content: ' Message ' + errorMessage,
                type: 'red',
                typeAnimated: true,
            });
            gridOptions.error({});
        }
    },

    ajaxRequestGridDone: function (hasError, callerWidgetName, errorMessage, data, gridOptions, currentGridName, beforeOptionsSuccessFxnCall) {
        debugger;
        if (hasError === false) {
            if (beforeOptionsSuccessFxnCall)
                beforeOptionsSuccessFxnCall(data);

            
            if (data.Result
                && data.Result.Result) {//this is a read operation pass data as is
                data.Result.DataSetCount = 100000;
                gridOptions.success(data);
            }
            else if (data.Result
                && (callerWidgetName === kendoWidgets.kendoGridWidgetName || callerWidgetName === kendoWidgets.treeListDataTypeName)) {
                //for the case of GRIDS CUD operations
                //https://docs.telerik.com/kendo-ui/framework/datasource/crud?&_ga=2.248265538.495157944.1628499726-514040982.1583933661#update-remote
                // If schema.data IS SET (for example to "foo"), use the following syntax instead:
                // e.success({"foo": [e.data]});
                //Above is always the case for us so we must do it
                //schema = {
                //    data: "Result.Result",
                //    total: "Result.DataSetCount"
                //},

                gridOptions.success({ Result: { Result: [data.Result] } });
            }
            else
                gridOptions.success(data);
        }
        else {
            $.alert({
                title: currentGridName + ' Error',
                icon: 'icon-exclamation',
                content: 'Message ' + errorMessage,
                type: 'red',
                typeAnimated: true,
            });
            gridOptions.error({});
        }
    }
};