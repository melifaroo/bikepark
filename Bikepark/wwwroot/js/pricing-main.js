
$(function () {
    $("#daysofweek").toggle($("#PricingType").val() != 2);
    $("#holiday").toggle($("#PricingType").val() != 2);
    $("#reduced").toggle($("#PricingType").val() != 2);
    $("#duration").toggle($("#PricingType").val() != 2);
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

$(document).on("change", "#PricingType", function () {
    $("#daysofweek").toggle($(this).val() != 2);
    $("#holiday").toggle($(this).val() != 2);
    $("#reduced").toggle($(this).val() != 2);
    $("#duration").toggle($(this).val() != 2);
});