var initializeMessageOutbox = {
    sent: false,

    replyToMessage: function () {
        alert('Not implemented');
    },
    LoadGrid: function () {

        debugger;

        var currentGridName = "LessonsReport";

        //define datasource schemas
        var schema = {
            data: "Result.Result",
            total: "Result.DataSetCount",
            model: {
                id: "Id",
                fields: {
                    Id: { type: "number", editable: false }, //SenderId: { type: "string", required: false },
                    PhoneNumber: { type: "string", required: false },
                    Message: { type: "string", required: false },
                    SentTime: { type: "date", required: false },
                    DeliveredDate: { type: "date", required: false },
                    Entity: {
                        defaultValue: {
                            Id: 0,
                            EntityName: ""
                        }
                    }
                }
            }
        };
        debugger;
        //create a kendo dataSource with minimal code...Using kendoHelper,With Read,Create,Update and Delete on its transport instance set
        var dataSource = kendoGridCRUDHelper
            .createStandardDataSource($("#smsOutboxSection"), schema, "/SchoolMS/Analytics/LessonsReport", currentGridName, initializeMessageOutbox);

        //define grid columns
        var fieldColumns = [
            { field: "Entity.EntityName", title: 'Learner Name', width: "15%" },
            { field: "PhoneNumber", title: 'Phone', width: "15%", },
            { field: "Message", title: 'Lesson content', width: "35%", },
            { field: "SentTime", title: 'Time Sent', width: "20%", format: "{0:yyyy-MM-dd HH:mm:ss}" },
            { field: "DeliveredDate", title: 'Delivered Time', width: "15%", format: "{0:yyyy-MM-dd HH:mm:ss}" },

            //{ command: { text: "Reply", click: initializeMessageOutbox.replyToMessage }, title: " ", width: "10%" }
        ];

        var beforeCreateGridObjectOperationFunction = function (rootObject) {

            delete rootObject.toolbar;
            rootObject.editable = "none";
            rootObject.filterable = false;

            return rootObject;
        };
        debugger;
        kendoGridCRUDHelper.createStandardCRUDGridWithConfigurableObject($("#smsOutboxSection"),
            currentGridName, "none", fieldColumns, dataSource, beforeCreateGridObjectOperationFunction);

    },
}

$(document).ready(function () {
    //initialise all grids in this section
    try {
        debugger;
        initializeMessageOutbox.LoadGrid();
    }
    catch (Err) {
        $.alert(err.message,"red",null);
    }
});
