
$(function () {
    checkAvailability();
    resizeStorage();
});

$(window).resize(function () {
    resizeStorage();
});

function resizeStorage(){
    var height = window.innerHeight - 180 - $("header").height() - $("footer").height() - $("#storage thead").height() - $("#prepared-title").height();
    height = Math.max(100, height);
    $("#storage tbody").height(height);
}

function checkAvailability() {
    var prepared = [];
    $("#items-list .item").each(function (i, tr) {
        key = $(tr).data("itemid");
        prepared.push(key);
    });
    $("#storage .number-check").each(function (i, number) {
        key = $(number).data("itemid");
        $(number).prop("checked", prepared.includes(key)).toggleClass("prepared", prepared.includes(key));
    });
}

$(document).on("change", ".chooser-filter", function () {
    var filter = {};
    $("#storage table thead .chooser-filter").each(function () {
        var field = $(this).data("field");
        var value = $(this).val();
        if (value!="All")
            filter[field] = value;
    });
    $("#storage table tbody tr").each(function () {
        var filtered = true;
        for (var field in filter) {
            filtered = filtered && ($(this).children("td[data-field='" + field + "']").text().trim() == filter[field].trim());
        }
        $(this).toggle(filtered, "slow");
    });
});

async function add_item(id) {
    var recid = -1;
    try {
        const result = await $.get(url_rental_addprepareditem, { ItemID: id });        
        $("#items-list").append(result);
    } catch (err) {
        console.log(err);
    }
}

function delete_item(id) {
    $("#items-list tr.item[data-itemid=" + id + "]").remove();
    $("#number-check-" + id).prop("checked", false);
}

$(document).on("change", "#storage .number-check", async function () {
    var itemid = $(this).data("itemid");
    if ($(this).is(":checked")) {
        await add_item(itemid);
    } else {
        delete_item(itemid);
    }
    checkAvailability();  
});

$(document).on("click", "#items-list .btn-item-delete", function () {
    var itemid = $(this).data("itemid");
    delete_item(itemid);
    checkAvailability(); 
});

$("#btn-toggle-filters").on("click", function () {
    $(".chooser-filter-optional").toggle();
});

$("#btn-clear").on("click", function () {
    $("#items-list .item").each(function (i, tr) {
        key = $(tr).data("itemid");
        delete_item(key);
    });
    checkAvailability(); 
});

