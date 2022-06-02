
$(function () {
    console.log("dddddd");
});



$(document).on("change", "#daysofweek .chk-dayofweek", function () {
    var idx = 0;
    $("#daysofweek .idx-dayofweek").remove();
    $("#daysofweek .chk-dayofweek").each(function () {
        $(this).attr("name", $(this).is(":checked") ? "DaysOfWeek[" + idx + "]" : null);
        if ($(this).is(":checked"))
            $("<input type='hidden' name='DaysOfWeek.index' autocomplete='off' value=" + idx++ + " class='idx-dayofweek'/>").insertBefore($(this));
    })
});