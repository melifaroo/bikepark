$(document).on("change", "#rental-items-list .pricing", function () {
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
    var dayOfWeek = new Date($("#time-start").val()).getDay();//
    d1 = new Date($("#time-start").val());
    d2 = new Date($("#time-end").val());
    var duration = Math.ceil(durationHours(d1, d2));
    $(row).find("select.pricing").empty();
    const actualprices = prices.filter(p =>
        ((p.PricingCategoryID==null || !pricingCategoryID)?true:p.PricingCategoryID == pricingCategoryID) &&
        p.DaysOfWeek.filter(d => d == dayOfWeek).length > 0 &&
        p.MinDuration <= duration &&
        p.PricingType < 2);
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
        $("#rental-items-list tr").each(function () {
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
    var s0 = new Date(record.Start);
    var e0 = new Date(record.End);

    var duration0 = durationHours(s0, e0);

    var price_account = 0
    var price_total = 0
    $("#rental-items-list tr").each(function () {
        var s = new Date($(this).find(".start").val());
        var e = new Date($(this).find(".end").val());
        var duration = durationHours(s, e);

        var pricingID = $(this).find(".pricing").val();
        if (pricingID) {
            var pricing = arcprices.filter(p => p.PricingID == pricingID)[0];
            price_total = price_total + pricing.Price * (pricing.PricingType == 0 ? duration:1);
        }

        var pricingID0 = $(this).find(".pricing0").val();
        if (pricingID0) {
            var pricing0 = arcprices.filter(p => p.PricingID == pricingID0)[0];
            price_account = price_account + pricing0.Price * (pricing.PricingType==0?duration0:1);
        }

    });
    var price_change = price_total - price_account;

    return [price_total, price_account, price_change];
}