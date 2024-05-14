//kendoDetailTableHelper a class to help simplfy interaction with the kendo details table
var kendoDetailTableHelper = {

    setUpTabStripBasedDetailRow: function (callerObject, e) {

        var detailRow = e.detailRow;

        detailRow.find(".tabstrip").kendoTabStrip({
            animation: {
                open: { effects: "fadeIn" }
            }
        });

        if (!callerObject.currentDetailRows) {
            callerObject.currentDetailRows = {
                parentId: {},
                ChildObject: {},
                ChildClassName: "",
            };
        }

        return detailRow;
    },

    addDetailRowDetails: function (callerObject, parentGridID, DetailTable, ChildClassName) {
        var objectToAdd = { parentId: parentGridID, ChildObject: DetailTable, ChildClassName: ChildClassName };
        if (callerObject.currentDetailRows.length && callerObject.currentDetailRows.length > 0) {
            var existingReference = callerObject.getDetailRow(parentGridID);
            //collapse all others except this

            if (!existingReference)
                callerObject.currentDetailRows[callerObject.currentDetailRows.length] = objectToAdd;
            else {
                existingReference.ChildObject = objectToAdd.ChildObject;
                //var Parentgrid = $(parentGridID).data('kendoGrid');
                //for (var i = 0; i < callerObject.currentDetailRows.length; i++) {
                //    Parentgrid.collapseRow(callerObject.currentDetailRows[i].masterRow);
                //}
            }
        }
        else {
            //before adding any detail row check wether the parent detail row for the same open for any instance
            callerObject.currentDetailRows = new Array(objectToAdd);
        }
    },
    //return the mother row identified by parentGridID in callerObject instance
    getDetailTableRow: function (callerObject, parentGridID) {
        if (callerObject.currentDetailRows.length && callerObject.currentDetailRows.length > 0) {
            for (var i = 0; i < callerObject.currentDetailRows.length; i++) {
                if (callerObject.currentDetailRows[i].parentId === parentGridID)
                    return callerObject.currentDetailRows[i];
            }
        }
        else
            return null;
    },
    getDetailTableRowTargetByClass: function (callerObject, rootGridId, className) {
        var currentDetailRow = kendoDetailTableHelper.getDetailTableRow(callerObject, rootGridId);
        var target = currentDetailRow.ChildObject.detailRow.find(className);
        return target;
    }
};

var kendoWidgets = {
    kendoGridWidgetName: "kendoGrid",
    kendoComboBoxWidgetName: "kendoComboBox",
    treeListDataTypeName: "kendoTreeList"
};

//help simplify the handling of kendo CRUD operations for GRID
var kendoGridCRUDHelper = {

    setUpTabStripBasedDetailRow: function (e) {

        var detailRow = e.detailRow;

        detailRow.find(".tabstrip").kendoTabStrip({
            animation: {
                open: { effects: "fadeIn" }
            }
        });
    },
    genericReadOperation: function (rootElement, callerWidgetName, relativeUrl, options, currentGridName, beforeReadOptionsSuccess) {


        kendo.ui.progress(rootElement, true);

        var onAjaxDoneCallBackMethod = function (hasError, errorMessage, data) {
            kendo.ui.progress(rootElement, false);

            jqueryAjaxGenerics.ajaxRequestGridDone(hasError, callerWidgetName, errorMessage, data,
                options, currentGridName, beforeReadOptionsSuccess);

        };

        var onAjaxFailCallBackMethod = function (hasError, errorMessage, data) {
            kendo.ui.progress(rootElement, false);

            jqueryAjaxGenerics.ajaxRequestGridFail(hasError, callerWidgetName, errorMessage, data,
                options, currentGridName);
        };

        jqueryAjaxGenerics.createAjaxRequest("POST", relativeUrl,
            "json", "application/json; charset=UTF-8", options.data,
            onAjaxDoneCallBackMethod, onAjaxFailCallBackMethod);
    },

    genericCUDOperation: function (rootElement, elementTypeName, method, relativeUrl, options, currentGridName, parentHostObject, SaveMessage, SaveTitle) {



        if (parentHostObject.sent === true)
            return;
        parentHostObject.sent = true;

        var kendoInstance = rootElement.data(elementTypeName);
        var editContainer = rootElement;
        if (kendoInstance._editContainer)
            editContainer = kendoInstance._editContainer;

        var onYesCallMethod = function () {
            kendo.ui.progress(editContainer, true);

            var onAjaxDoneCallBackMethod = function (hasError, errorMessage, data) {
                kendo.ui.progress(editContainer, false);
                parentHostObject.sent = false;

                jqueryAjaxGenerics.ajaxRequestGridDone(hasError, elementTypeName, errorMessage, data,
                    options, currentGridName, null);

            };

            var onAjaxFailCallBackMethod = function (hasError, errorMessage, data) {
                kendo.ui.progress(editContainer, false);
                parentHostObject.sent = false;

                jqueryAjaxGenerics.ajaxRequestGridFail(hasError, errorMessage, data,
                    options, currentGridName);
            };

            jqueryAjaxGenerics.createAjaxRequest(method, relativeUrl,
                "json", "application/json; charset=UTF-8", options.data,
                onAjaxDoneCallBackMethod, onAjaxFailCallBackMethod);
        }

        var onNoCallbackMethod = function () {

            //no code for cancel
            parentHostObject.sent = false;
            options.error({
                Result: options.data
            });

        };

        jqueryConfirmGenerics.showYesNoSaveDialog(SaveMessage, SaveTitle,
            onYesCallMethod, onNoCallbackMethod);
    },

    genericCreateOperation: function (rootElement, elementTypeName, relativeUrl, options, currentGridName, parentHostObject) {
        kendoGridCRUDHelper.genericCUDOperation(rootElement, elementTypeName, "POST", relativeUrl, options, currentGridName, parentHostObject, 'Confirm', 'Are you sure you want to save?');
    },

    genericUpdateOperation: function (rootElement, elementTypeName, relativeUrl, options, currentGridName, parentHostObject) {
        kendoGridCRUDHelper.genericCUDOperation(rootElement, elementTypeName, "PUT", relativeUrl, options, currentGridName, parentHostObject, 'Confirm', 'Are you sure you want to update?');
    },

    genericDeleteOperation: function (rootElement, elementTypeName, relativeUrl, options, currentGridName, parentHostObject) {
        kendoGridCRUDHelper.genericCUDOperation(rootElement, elementTypeName, "POST", relativeUrl, options, currentGridName, parentHostObject, 'Confirm', 'Are you sure you want to delete?');
    },


    //create a kendo Grid using the default options and not customizable
    //to customize the grid use createStandardCRUDGridConfigurableObject
    createStandardCRUDGrid: function (rootElement, currentGridName, editableMode, fieldColumns, dataSource) {

        var kendoGridInstance = kendoGridCRUDHelper
            .createStandardCRUDGridWithDetailTable(rootElement, currentGridName,
                editableMode, fieldColumns, dataSource,
                null, null, null);

        return kendoGridInstance;

    },

    //create a gird without a detail table using and allows one to customize the Grid by supplying the callBack function beforeCreateGridObjectOperationFunction
    //to create a default grid with no optimization use createStandardCRUDGrid
    createStandardCRUDGridWithConfigurableObject: function (rootElement, currentGridName, editableMode, fieldColumns, dataSource, beforeCreateGridObjectOperationFunction) {

        var kendoGridInstance = kendoGridCRUDHelper
            .createStandardCRUDGridWithDetailTableConfigurable(rootElement, currentGridName,
                editableMode, fieldColumns,
                dataSource, null, null, beforeCreateGridObjectOperationFunction);

        return kendoGridInstance;

    },

    //create a CRUD grid with detail table using the default options To customize the grid use 
    //createStandardCRUDGridWithDetailTableConfigurableObject
    createStandardCRUDGridWithDetailTable: function (rootElement, currentGridName, editableMode, fieldColumns, dataSource, detailTemplateScriptElement, detailTemplateInitFunction) {

        var kendoGridInstance = kendoGridCRUDHelper
            .createStandardCRUDGridWithDetailTableConfigurable(rootElement, currentGridName,
                editableMode, fieldColumns,
                dataSource, detailTemplateScriptElement,
                detailTemplateInitFunction, null);

        return kendoGridInstance;
    },


    //creates a kendo grid with detail table options set and allows one to use beforeCreateGridObjectOperationFunction to customize the kendo grid at caller level
    //to create a default grid with no configuration use createStandardCRUDGridWithDetailTable
    createStandardCRUDGridWithDetailTableConfigurableObject: function (rootElement, currentGridName, editableMode, fieldColumns, dataSource, detailTemplateScriptElement, detailTemplateInitFunction, beforeCreateGridObjectOperationFunction) {

        var kendoGridInstance = kendoGridCRUDHelper
            .createStandardCRUDGridWithDetailTableConfigurable(rootElement, currentGridName,
                editableMode, fieldColumns,
                dataSource, detailTemplateScriptElement, detailTemplateInitFunction, beforeCreateGridObjectOperationFunction);

        return kendoGridInstance;

    },


    createStandardCRUDGridWithDetailTableConfigurable: function (rootElement, currentGridName, editableMode, fieldColumns, dataSource, detailTemplateScriptElement, detailTemplateInitFunction, beforeCreateGridObjectOperationFunction) {
        var detailInitFunctionAssignValue = null;
        if (detailTemplateInitFunction)
            detailInitFunctionAssignValue = detailTemplateInitFunction;

        var detailTemplateAssignValue = null;
        if (detailTemplateScriptElement)
            detailTemplateAssignValue = detailTemplateScriptElement;

        var detailExpandAssignValue = null;
        if (detailTemplateScriptElement && detailTemplateInitFunction) {
            detailExpandAssignValue = function (e) {
                this.collapseRow(this.tbody.find(' > tr.k-master-row').not(e.masterRow));
            };
        }

        debugger;
        //set the height to a fraction
        var parentElement = $(rootElement).parent();

        var heightParentElement = kendoGridCRUDHelper.getKendoGridHeight(parentElement, 1);

        if (parentElement.hasClass("k-content")) {
            var newHeight = heightParentElement * 1000;
            if (newHeight < 250)
                newHeight = 250;
            if (newHeight > 350)
                newHeight = 350;
            heightParentElement = newHeight;
        }

        //parentElement.height(heightWindowElement);

        var rootObject = {

            height: heightParentElement,

            detailInit: detailInitFunctionAssignValue,
            detailTemplate: detailTemplateAssignValue,
            detailExpand: detailExpandAssignValue,

            editable: {
                mode: editableMode,
                window: {
                    title: currentGridName
                }
            },
            edit: function (e) {
                if (e.model.isNew()) {
                    $("a.k-grid-update")[0].innerHTML = "<span class='k-icon k-update'></span>SAVE";
                }
                else {
                    $("a.k-grid-update")[0].innerHTML = "<span class='k-icon k-update'></span>UPDATE";
                }
            },
            cancel: function (e) {

                //e.preventDefault();
            },
            columnResizeHandleWidth: 6,
            columnMenu: {
                columns: false,
                messages: {
                    columns: "Choose columns",
                    done: "Ok",
                }
            },
            groupable: false,
            autoBind: false,
            dataSource: dataSource,
            pageSize: 10,
            filterable: {
                extra: false,
                operators: {
                    string: {
                        contains: "Contains"
                        //neq: "Is not equal to"
                    }
                }
            },
            sortable: false,
            pageable: {
                refresh: true,
                pageSizes: true,
                buttonCount: 5
            },
            toolbar: [
                { name: "create", text: "Add" }
            ],
            columns: fieldColumns,
        };

        if (beforeCreateGridObjectOperationFunction)
            rootObject = beforeCreateGridObjectOperationFunction(rootObject);



        rootElement.kendoGrid(rootObject);

        dataSource.read();

        return rootElement.data("kendoGrid");
    },
    getKendoGridHeight: function (parentElement, rationSize) {
        //take the view port into consideration for this computation
        //the larget the screen size then scale up or down appropriately
        return (parentElement.height() * rationSize);
    },
    //kendoGridDataTypeName: "kendoGrid",
    createStandardDataSourceDetailed: function (rootElement, schema, readPath, createPath, updatePath, deletePath, currentGridName, parentHostObject, beforeCreateSetFunction, filterValue) {

        var defaultSort = null;
        if (schema.model && schema.model.id) {
            defaultSort = {
                field: schema.model.id,
                dir: "desc"
            };
        }

        return new kendo.data.DataSource({
            schema: schema,
            sort: defaultSort,
            transport: {
                read: function (options) {

                    kendoGridCRUDHelper.genericReadOperation(rootElement, kendoWidgets.kendoGridWidgetName, readPath,
                        options, currentGridName, null);
                },
                create: function (options) {

                    if (beforeCreateSetFunction)
                        beforeCreateSetFunction(options);
                    kendoGridCRUDHelper.genericCreateOperation(rootElement, kendoWidgets.kendoGridWidgetName, createPath, options, currentGridName, parentHostObject)
                },
                update: function (options) {

                    kendoGridCRUDHelper.genericUpdateOperation(rootElement, kendoWidgets.kendoGridWidgetName, updatePath, options, currentGridName, parentHostObject)
                },
                destroy: function (options) {

                    kendoGridCRUDHelper.genericDeleteOperation(rootElement, kendoWidgets.kendoGridWidgetName, deletePath, options, currentGridName, parentHostObject)
                }
            },
            serverFiltering: true,
            serverPaging: true,
            serverSorting: false,
            pageSize: 10,
            filter: filterValue ? filterValue : null,
            error: function (a) {
                try {

                    rootElement.data("kendoGrid").cancelChanges();
                }
                catch (err) {
                    jqueryConfirmGenerics.showOkAlertBox("Grid " + currentGridName + "Error", "A data source error occured cancelling changes in error event handler." + err.message,"red",null);
                }
            }
        });
    },


    createStandardDetailTableDataSource: function (rootElement, schema, relativeControllerPath, currentGridName, parentHostObject, beforeCreateFunction, filterValue) {
        return kendoGridCRUDHelper.createStandardDataSourceDetailed(rootElement, schema,
            relativeControllerPath + "/GetKendoGridFiltered",
            relativeControllerPath + "/Add",
            relativeControllerPath + "/Update",
            relativeControllerPath + "/Delete",
            currentGridName, parentHostObject, beforeCreateFunction, filterValue);
    },

    createStandardDataSource: function (rootElement, schema, relativeControllerPath, currentGridName, parentHostObject) {
        return kendoGridCRUDHelper.createStandardDetailTableDataSource(rootElement, schema,
            relativeControllerPath, currentGridName, parentHostObject, null, null);
    },

    //tree list specific implementation helpers
    createStandardDetailedTreeListDataSource: function (rootElement, schema, relativeChildControllerPath, currentGridName, parentHostObject, beforeReadOptionsSuccess) {
        return new kendo.data.TreeListDataSource({
            schema: schema,
            transport: {
                read: function (options) {


                    var pathName = "/GetKendoGridFiltered";
                    var readPath = relativeChildControllerPath + pathName;
                    //if (isInitCall === true)
                    //    readPath = relativeParentControllerPath + pathName;

                    kendoGridCRUDHelper.genericReadOperation(rootElement, kendoWidgets.treeListDataTypeName, readPath,
                        options, currentGridName, beforeReadOptionsSuccess);
                },
                create: function (options) {

                    var pathName = "/Add";
                    var createPath = relativeChildControllerPath + pathName;
                    //if (isParentCheckFunction(options) === true)
                    //    createPath = relativeParentControllerPath + pathName;

                    kendoGridCRUDHelper.genericCreateOperation(rootElement, kendoWidgets.treeListDataTypeName, createPath, options, currentGridName, parentHostObject)
                },
                update: function (options) {

                    var pathName = "/Update";
                    var updatePath = relativeChildControllerPath + pathName;
                    //if (isParentCheckFunction(options) === true)
                    //    updatePath = relativeParentControllerPath + pathName;

                    kendoGridCRUDHelper.genericUpdateOperation(rootElement, kendoWidgets.treeListDataTypeName, updatePath, options, currentGridName, parentHostObject)
                },
                destroy: function (options) {

                    var pathName = "/Delete";
                    var deletePath = relativeChildControllerPath + pathName;
                    //if (isParentCheckFunction(options) === true)
                    //    deletePath = relativeParentControllerPath + pathName;

                    kendoGridCRUDHelper.genericDeleteOperation(rootElement, kendoWidgets.treeListDataTypeName, deletePath, options, currentGridName, parentHostObject)
                }
            },
            serverFiltering: true,
            serverPaging: true,
            serverSorting: true,
            pageSize: 10,

            error: function (a) {
                try {

                    rootElement.data("kendoTreeList").cancelChanges();
                }
                catch (err) {
                    jqueryConfirmGenerics.showOkAlertBox("Grid " + currentGridName + "Error", "A data source error occured cancelling changes in error event handler." + err.message,"red",null);
                }
            },
            requestEnd: function (e) {
                if (e.type !== "read") {
                    //this.read();
                }
            }
        });
    },

    createStandardKendoTreeList: function (rootElement, currentTreeListName, editableMode, fieldColumns, dataSource) {

        rootElement.kendoTreeList({

            editable: {
                mode: editableMode,
                window: {
                    title: currentTreeListName
                }
            },
            edit: function (e) {
                if (e.model.isNew()) {
                    $("button.k-grid-update")[0].innerHTML = "<span class='k-icon k-update'></span>SAVE";
                }
                else {
                    $("button.k-grid-update")[0].innerHTML = "<span class='k-icon k-update'></span>UPDATE";
                }
            },
            cancel: function (e) {

                //e.preventDefault();
            },
            //columnResizeHandleWidth: 6,
            //columnMenu: {
            //    columns: false,
            //    messages: {
            //        columns: "Choose columns",
            //        done: "Ok",
            //    }
            //},
            //groupable: false,
            //autoBind: false,
            dataSource: dataSource,
            //pageSize: 10,
            ////filterable: {
            ////    extra: false,
            ////    operators: {
            ////        string: {
            ////            contains: "Contains"
            ////            //neq: "Is not equal to"
            ////        }
            ////    }
            ////},
            ////sortable: false,
            pageable: true,
            //pageable: {
            //    pageSize: 15,
            //    pageSizes: true
            //},
            toolbar: [
                { name: "create", text: "Add" }
            ],
            columns: fieldColumns,
        });

        //dataSource.read();

        return rootElement.data("kendoTreeList");
    },
};