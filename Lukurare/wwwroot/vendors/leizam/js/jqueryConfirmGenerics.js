var jqueryConfirmGenerics = {

    showYesNoSaveDialog: function (title, message, onOkCallbackMethod, onNoCallbackMethod) {
        $.confirm({
            icon: 'icon-save',
            type: 'green',
            useBootstrap: false,
            width: 'auto',
            typeAnimated: true,
            title: title,
            content: message,
            buttons: {
                confirm: {
                    btnClass: 'btn-green',
                    text: 'Yes',//yes button
                    action: function () {
                        onOkCallbackMethod();
                    }
                },
                cancel: {
                    btnClass: 'btn-red',
                    text: 'No',//no button
                    action: function () {
                        onNoCallbackMethod();
                    }
                },
            }
        });
    },

    showOkAlertBox: function (title, message,color, onOkCallbackMethod) {
        $.confirm({
            icon: 'icon-exclamation',
            type: color,
            useBootstrap: false,
            width: 'auto',
          
            typeAnimated: true,
            title: title,
            content: message,
            buttons: {
                confirm: {
                    btnClass: 'btn-green',
                    text: 'Ok',//yes button
                    action: function () {
                        if (onOkCallbackMethod)
                            onOkCallbackMethod();
                    }
                },
            }
        });
    }

};