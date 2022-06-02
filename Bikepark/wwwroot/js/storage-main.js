﻿

$(document).on("change", ".chooser-filter", function () {
    var filter = {};
    $("#storage thead .chooser-filter").each(function () {
        var field = $(this).data("field");
        var value = $(this).val();
        if (value != "All")
            filter[field] = value;
    });
    $("#storage tbody tr").each(function () {
        var filtered = true;
        for (var field in filter) {
            filtered = filtered && ($(this).children("td[data-field='" + field + "']").text().trim() == filter[field].trim());
        }
        $(this).toggle(filtered, "slow");
    });
});


$(document).ready(function () {
    var height = window.innerHeight - 110 - $("header").height() - $("footer").height() - $("#storage thead").height() - $("#storage-title").height();
    height = Math.max(100, height);
    $("#storage tbody").height(height);
});

$(window).resize(function () {
    var height = window.innerHeight - 110 - $("header").height() - $("footer").height() - $("#storage thead").height() - $("#storage-title").height();
    height = Math.max(100, height);
    $("#storage tbody").height(height);
});