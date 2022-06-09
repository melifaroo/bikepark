
var validator;

$(function () {
    console.log(prices);
    console.log(arcprices);
    highlight();
    calculatePrice();
});


$(document).on("change", "#itemrecords-list .pricing", function () {
    showprice(this);
    calculatePrice();
});

$(document).on("change", "#itemrecords-list .start", function () {
    var id = $(this).data("id");
    var start = new Date($(this).val());
    var enddate = $("#itemrecords-list tr.item-record[data-id=" + id + "]").find(".end").val();
    var end = new Date(enddate);
    if (start.getTime() > end.getTime()) {
        alert("Укажите начало ремонта не позже планируемой даты его окончания");
        $(this).val(enddate);
    }
});

$(document).on("change", "#itemrecords-list .end", function () {
    var id = $(this).data("id");
    
    var end = new Date($(this).val());
    var startdate = $("#itemrecords-list tr.item-record[data-id=" + id + "]").find(".start").val();
    var start = new Date(startdate);
    if (start.getTime() > end.getTime()) {
        alert("Укажите плаинруемую дату завершения ремонта не ранее даты его начала");
        $(this).val(startdate);
    }

});

$(document).on("click", "#itemrecords-list .btn-remove", function () {
    var id = $(this).data("id");
    var recid = $("#itemrecords-list tr.item-record[data-id=" + id + "]").data("recid")
    if (recid) {
        $("#itemrecords-list tr.item-record[data-id=" + id + "]").toggleClass("alert alert-secondary");
        $("#itemrecords-list tr.item-record[data-id=" + id + "]").find(".itemid").val(null);
        $("#itemrecords-list tr.item-record[data-id=" + id + "] input").prop("readonly", true);
        $("#itemrecords-list tr.item-record[data-id=" + id + "] select").prop("disabled", true);
        $("#itemrecords-list tr.item-record[data-id=" + id + "] .btn-remove").toggle(false);
        $("#itemrecords-list tr.item-record[data-id=" + id + "] .btn-add").toggle(false);
        $("#itemrecords-list tr.item-record[data-id=" + id + "] .btn-restore").toggle(true);
    } else {
        $("#itemrecords-list tr.item-record[data-id=" + id + "]").remove();
    }
    calculatePrice();
});

$(document).on("click", "#itemrecords-list .btn-restore", function () {
    var id = $(this).data("id");
    var recid = $("#itemrecords-list tr.item-record[data-id=" + id + "]").data("recid")
    var itemid = $("#itemrecords-list tr.item-record[data-id=" + id + "]").data("itemid")
   
    $("#itemrecords-list tr.item-record[data-id=" + id + "]").toggleClass("alert alert-secondary");
    $("#itemrecords-list tr.item-record[data-id=" + id + "]").find(".itemid").val( itemid );
    $("#itemrecords-list tr.item-record[data-id=" + id + "] input").prop("readonly", false);
    $("#itemrecords-list tr.item-record[data-id=" + id + "] select").prop("disabled", false);
    $("#itemrecords-list tr.item-record[data-id=" + id + "] .btn-remove").toggle(true);
    $("#itemrecords-list tr.item-record[data-id=" + id + "] .btn-add").toggle(true);
    $("#itemrecords-list tr.item-record[data-id=" + id + "] .btn-restore").toggle(false);
    calculatePrice();
});

$(document).on("click", "#itemrecords-list .btn-add", async function () { 
    var id = $(this).data("id");
    var itemid = $("#itemrecords-list tr.item-record[data-id=" + id + "]").find(".itemid").val();
    var recordid = $("#itemrecords-list tr.item-record[data-id=" + id + "]").find(".recordid").val();
    var start = $("#itemrecords-list tr.item-record[data-id=" + id + "]").find(".start").val();
    var end = $("#itemrecords-list tr.item-record[data-id=" + id + "]").find(".end").val();
    try {
        const result = await $.get(url_rental_addservicerecord, { ItemID: itemid, Start: start, End: end, RecordID: recordid });
        $("#itemrecords-list").append(result);
        calculatePrice();
    } catch (err) {
        console.log(err);
    }

});


function highlight() {
    $("#itemrecords-list tr").each(function () {
            var id = $(this).data("id");
            var status = $(this).find(".status").val();
            if (status == "Fixed") {
                $("#itemrecords-list tr.item-record[data-id=" + id + "]").toggleClass("alert alert-success");
                $("#itemrecords-list tr.item-record[data-id=" + id + "] input").prop("readonly", true);
                $("#itemrecords-list tr.item-record[data-id=" + id + "] select").prop("disabled", true);
                $("#itemrecords-list tr.item-record[data-id=" + id + "] .btn-remove").toggle(false);
                $("#itemrecords-list tr.item-record[data-id=" + id + "] .btn-restore").toggle(false);
                $("#itemrecords-list tr.item-record[data-id=" + id + "] .btn-add").toggle(true);
            }
        }
    );

}

function showprice(pricing) {
    var pricingID = $(pricing).val();
    var price = prices.filter(p => p.PricingID == pricingID)[0];
    $(pricing).parent().parent().find(".pricing-info").text(price.Price);
}

function calculatePrice() {
    var price_total = 0;
    $("#itemrecords-list tr").each(function () {
            var itemid = $(this).find(".itemid").val();
            if (itemid) {
                var pricingID = $(this).find(".pricing").val();
                if (pricingID) {
                    var pricing = arcprices.filter(p => p.PricingID == pricingID)[0];
                    price_total += pricing.Price;
                }
            }
        }
    );
    $("#price").html(price_total);

}

