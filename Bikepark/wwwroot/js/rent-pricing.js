$(document).on("change", "#itemrecords-list .pricing", function () {
    showprice(this)
    change_action();
});

function showprice(pricing) {
    var pricingID = $(pricing).val();
    var price = prices.filter(p => p.PricingID == pricingID)[0];
    $(pricing).parent().parent().find(".pricing-info").text(price.Price + " - " + pricingTypes[price.PricingType]);
}

function updateprices(row) {
    var pricingID = $(row).find(".pricing").val();
    var pricingCategoryID = $(row).find("input.pricingcategoryid").val();
    var date = new Date($("#time-start").val());
    date.setHours(0, 0, 0, 0);
    var dayOfWeek = date.getDay();//
    var isHoliday = false;

    var BreakException = {};
    try {
        holidays.forEach(day => {
            var holi = new Date(day.Year + "-" + zeroPadded(day.Month) + "-" + zeroPadded(day.Day));
            holi.setHours(0, 0, 0, 0);
            isHoliday = date.getTime() == holi.getTime();
            if (isHoliday) throw BreakException;
        });
    } catch (e) {
        if (e !== BreakException) throw e;
    }

    d1 = new Date($("#time-start").val());
    d2 = new Date($("#time-end").val());
    var duration = Math.ceil(durationHours(d1, d2));
    $(row).find("select.pricing").empty();
    const actualprices = prices.filter(p =>
        ((p.PricingCategoryID==null || !pricingCategoryID)?true:p.PricingCategoryID == pricingCategoryID) &&
        p.DaysOfWeek.filter(d => d == dayOfWeek).length > 0 &&
        p.MinDuration <= duration &&
        p.PricingType < 2 && (!p.IsHoliday || isHoliday));
    actualprices.forEach(price => {
        $(row).find(".pricing").append($("<option />").val(price.PricingID).text(price.PricingName));
    });
    if (pricingID != null) { 
        var price = actualprices.filter(p => p.PricingID == pricingID)[0];
        if (price) {
            $(row).find(".pricing").val(pricingID);
        } else {
            pricingID = $(row).find(".pricing").val();
            price = actualprices.filter(p => p.PricingID == pricingID)[0];
        }
        $(row).find(".pricing-info").text(price.Price + " - " + pricingTypes[price.PricingType]);
    }
}

function priceChanged() {
    var changed = false;
    var BreakException = {};
    try {
        $("#itemrecords-list tr").each(function () {
            changed = ($(this).find(".pricing").val() != $(this).find(".pricing0").val())
            if (changed) throw BreakException;
        });
    } catch (e) {
        if (e !== BreakException) throw e;
    }
    return changed;
}


function calculatePrice() {
    var s1 = new Date($("#time-start").val());
    var a1 = new Date($("#time-action").val());
    var e1 = new Date($("#time-end").val());
    var n = new Date();

    var price_account = parseFloat( $("#price-account").val()?$("#price-account").val():"0" );
    var price_total = record.Status > 2 ? price_account : 0;
    if (record.Status <= 2)
        $("#itemrecords-list tr").each(function () {
            var s = (record.Status == 2) ? (($(this).attr('status') == "Draft") ? a1 : new Date($(this).find(".start").val())) : s1;
            var e = (record.Status == 2) ? (($(this).attr('status') == "Closed") ? new Date($(this).find(".end").val()) : (e1.getTime() < n.getTime())?n:e1 ) : e1;;
            var duration = durationHours(s, e);
            console.log(duration);
            var pricingID = $(this).find(".pricing").val();
            if (pricingID) {
                var pricing = arcprices.filter(p => p.PricingID == pricingID)[0];
                price_total += pricing.Price * (pricing.PricingType == 1 ? duration:1);
            }
        });
    var price_change = price_total - price_account;

    return [price_total, price_account, price_change];
}