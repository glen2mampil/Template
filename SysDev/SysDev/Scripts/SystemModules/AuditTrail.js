$(document).ready(function () {
    var table = $("#audittrail-table").DataTable({
        ajax: {
            url: "/api/audittrails",
            dataSrc: ""
        },
        order: [[0, "desc"]],
        columns: [
            {
                data: "DateCreated",
                render: function (data, type, full) {
                    var date = new Date(full.DateCreated);
                    var options = {
                        //weekday: "long",
                        year: "numeric",
                        month: "short",
                        day: "numeric"
                        //hour: "2-digit",
                        //minute: "2-digit"
                    };
                    return "<small class=''>" + date.toLocaleString("en-us", options) + "</small>";
                }
            },
            {
                render: function (data, type, full, meta) {
                    var fullName = full.UserProfile.FirstName + " " + full.UserProfile.LastName;
                    if ($("#filterby_commit option[value='" + fullName + "']").length < 1) {
                        $("#filterby_commit")
                            .append("<option value='" + fullName + "'>" + fullName + "</option>");
                    }
                    return fullName;
                }
            }, {
                render: function (info, type, data, meta) {
                    var moduleName = data.Module.Name;
                    return moduleName;
                }

            },
            {
                render: function (info, type, data, meta) {
                    var action = data.Action;
                    return action;
                }
            },
            {
                data: "Description"
            }
        ]
    });
    $('#filterr').on('change', function () {
        table.columns(0).search(this.value).draw();
    });
    $('#filterby_commit').on('change', function () {
        table.columns(1).search(this.value).draw();
    });
    $('#filterby_module').on('change', function () {
        table.columns(2).search(this.value).draw();
    });
    $('#filterby_action').on('change', function () {
        table.columns(3).search(this.value).draw();
    });
    // ---------------------- [ Date Picker for filter]-----
    $("#date_from").datepicker({
        autoclose: true
    });
    $("#date_to").datepicker({
        autoclose: true
    });
    var date_from = "";
    var date_to = "";
    $.fn.dataTable.ext.search.push(
        function (settings, data, dataIndex) {
            var min = date_from;
            var max = new Date(date_to);
            var startDate = new Date(data[0]);

            if (date_to === "") {
                max = new Date(Date.now());
            }
            if (date_from !== "") {
                min = new Date(date_from);
            }
            if (min === null && max === null) { return false; }
            if (min === null && startDate <= max) { return true; }
            if (max === null && startDate >= min) { return true; }
            if (startDate <= max && startDate >= min) { return true; }
            return false;
        }
    );
    $("#date_from").change(function () {
        date_from = this.value;
        table.draw();
    });
    $("#date_to").change(function () {
        date_to = this.value;
        table.draw();
    });

});