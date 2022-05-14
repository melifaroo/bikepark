
$(document).ready(function () {
    setInterval(function () {
        d = new Date();
        $("#rental-control .timer-active").val(time2str(d.getFullYear(), d.getMonth(), d.getDate(), d.getHours(), d.getMinutes()));
        if (!active)
            update_endtime();
    }, 1000);

    d = new Date();
    if ($("#chk-action-giveout").is(":checked")) {
        $(".timer-start").val(time2str(d.getFullYear(), d.getMonth(), d.getDate(), d.getHours(), d.getMinutes()));
        update_endtime();
    }
    $(".timer-end").val(time2str(d.getFullYear(), d.getMonth(), d.getDate(), d.getHours(), d.getMinutes()));

    $(".navbar-nav #link-rental-create").addClass("active");

    checkAvailability();
});

function checkAvailability() {
    s = new Date($("#time-start").val());
    e = new Date($("#time-end").val());
    console.log(availability);
    Object.keys(availability).forEach(key => {
        var overlap = false;
        var BreakException = {};
        console.log(key);
        try {
            availability[key].forEach(period => {
                s1 = new Date(period["Item1"]);
                e1 = new Date(period["Item2"]);
                console.log(s1);
                console.log(e1);
                overlap = (s <= e1) && (e >= s1);
                console.log(overlap);
                if (overlap) throw BreakException;
            });
        } catch (e) {
            if (e !== BreakException) throw e;
        }
        $("#chk-item-add-" + key).prop("disabled", overlap);
    });
}

function zeroPadded(val) {
    if (val >= 10)
        return val;
    else
        return "0" + val;
}

$("#btn-search-customer").on("click", function () {
    var request = $("#customer-num").val();
    var url = url_rental_searchcustomer;
    $.get(url, { Request: request },
        function (result) {
            $("#сustomer-search-results").html(result);
            $("#сustomer-search-results").show("slow");
        }
    );
});

$(document).on("change", ".chooser-filter", function () {
    var filter = {};
    $("#rental-items-chooser table thead .chooser-filter").each(function () {
        var field = $(this).data("field");
        var value = $(this).val();
        if (value!="All")
            filter[field] = value;
    });
    $("#rental-items-chooser table tbody tr").each(function () {
        var filtered = true;
        for (var field in filter) {
            filtered = true && ($(this).children("td[data-field='" + field + "']").text().trim() == filter[field]);
        }
        if (filtered)
            $(this).show("slow");
        else
            $(this).hide("slow");
    });
});

$(document).on("change", "#rental-control #rental-items-chooser .chk-item-add", function () {
    var id = $(this).data("itemid");
    if ($(this).is(":checked")) {
        add_item(id);
    } else {
        delete_item(id);
    }
    //checkAvailability();
    //$(this).prop( "disabled", true );       
});

function add_item(id) {
    var url = url_rental_additem;
    $.get(url, { ItemID: id },
        function (result) {
            $("#rental-items-list tbody").append(result);
        }
    );
    $("#btn-schedule").prop("disabled", false);
    if (itemsCount() >= 0) {
        $("#btn-giveout").show("slow");
        $("#btn-permit").hide("slow");
    } else {
        $("#btn-giveout").hide("slow");
        $("#btn-permit").show("slow");
    }
}

function delete_item(id) {
    $("#rental-items-list tr#rental-item-row-" + id).remove();
    console.log("item " + id + " removed from order, items count = " + itemsCount());
    $("#btn-schedule").prop("disabled", itemsCount() == 0);
    if (itemsCount() > 0) {
        $("#btn-giveout").show("slow");
        $("#btn-permit").hide("slow");
    } else {
        $("#btn-giveout").hide("slow");
        $("#btn-permit").show("slow");
    }
}


$(document).on("click", "#rental-control #rental-items-list .btn-item-delete", function () {
    var id = $(this).data("itemid").toString();
    $("#rental-items-chooser input[data-itemid=" + id + "]").prop("checked", false);
    delete_item(id);
    checkAvailability();
});

$("#btn-show-items-chooser").on("click", function () {
    $("#rental-items-chooser").toggle("slow");
});

$("#btn-more-customer-detail").on("click", function () {
    $(".customer-more-detail").toggle("slow");
});

$("#btn-customer").on("click", function () {
    $("#customer-details").toggle("slow");
});

$("#btn-rental-info").on("click", function () {
    $("#rental-info").toggle("slow");
});

$("#btn-rental-items").on("click", function () {
    $("#rental-items").toggle("slow");
});

$(document).on("click", ".btn-cancel-customer", function () {
    $("#сustomer-search-results").hide("slow");
});

$(document).on("click", ".btn-select-customer", function () {
    var id = $(this).data("customerid").toString();
    var url = url_rental_customer;
    $.get(url, { CustomerID: id },
        function (result) {
            $("#customer-name").val(result.customerFullName);
            $("#customer-num").val(result.customerContactNumber);
            $("#customer-email").val(result.customerEMail);
            $("#customer-doc").val(result.customerPassport);
            $("#customer-id").val(result.customerID);

            $("#chk-new-customer").prop("checked", false);
            $("#chk-new-customer").prop("disabled", true);
            $("#chk-upd-customer").prop("checked", false);
            $("#chk-upd-customer").prop("disabled", true);
        }
    );
    $("#сustomer-search-results").hide("slow");
});

function new_upd_customer_mode() {
    if ($("#customer-id").val() == "") {
        $("#chk-upd-customer").prop("checked", false);
        $("#chk-upd-customer").prop("disabled", true);
        $("#chk-new-customer").prop("checked", true);
        $("#chk-new-customer").prop("disabled", true);
    } else {
        $("#chk-upd-customer").prop("checked", true);
        $("#chk-upd-customer").prop("disabled", false);
        $("#chk-new-customer").prop("checked", false);
        $("#chk-new-customer").prop("disabled", false);
    }
}

$(document).on("change", "#customer-name", function () {
    new_upd_customer_mode()
});
$(document).on("change", "#customer-num", function () {
    new_upd_customer_mode()
});
$(document).on("change", "#customer-email", function () {
    new_upd_customer_mode()
});
$(document).on("change", "#customer-doc", function () {
    new_upd_customer_mode()
});
$(document).on("change", "#chk-new-customer", function () {
    if ($("#customer-id").val() != "") {
        $("#chk-upd-customer").prop("checked", !$("#chk-new-customer").is(":checked"));
    }
});
$(document).on("change", "#chk-upd-customer", function () {
    if ($("#customer-id").val() != "") {
        $("#chk-new-customer").prop("checked", !$("#chk-upd-customer").is(":checked"));
    }
});

function time2str(year, month, day, hours, minutes) {
    return year + "-" + zeroPadded(month + 1) + "-" + zeroPadded(day) + "T" + zeroPadded(hours) + ":" + zeroPadded(minutes)
}

function itemsCount() {
    return $("#rental-items-list tbody tr").length;
}

function update_endtime() {
    d = new Date($("#time-start").val());
    t = $("#input-duration").val();

    if ($("#chk-onetime").is(":checked")) {
        $("#time-end").val(time2str(d.getFullYear(), d.getMonth(), d.getDate(), 23, 59));
    } else if ($("#chk-hourly").is(":checked")) {
        d.setTime(d.getTime() + t * 60 * 60 * 1000);
        $("#time-end").val(time2str(d.getFullYear(), d.getMonth(), d.getDate(), d.getHours(), d.getMinutes()));
    } else if ($("#chk-daily").is(":checked")) {
        d.setDate(d.getDate() + t * 1);
        $("#time-end").val(time2str(d.getFullYear(), d.getMonth(), d.getDate(), d.getHours(), d.getMinutes()));
    }

    console.log("end time updated")
}

function update_duration() {
    d1 = new Date($("#time-start").val());
    d2 = new Date($("#time-end").val());
    if ($("#chk-onetime").is(":checked")) {
        $("#input-duration").val(1);
    } else if ($("#chk-hourly").is(":checked")) {
        $("#input-duration").val(Math.round((d2.getTime() - d1.getTime()) / 60 / 60 / 1000));
    } else if ($("#chk-daily").is(":checked")) {
        $("#input-duration").val(Math.round((d2.getDate() - d1.getDate()) / 1));
    }
    console.log("duration updated")
}

$("#input-duration").on("change", function () {
    update_endtime();
    checkAvailability();
});
$("#time-start").on("change", function () {
    if ($("#chk-action-schedule").is(":checked")) {
        bookingDateTime = new Date($("#time-start").val());
    }
    update_endtime();
    checkAvailability();
});
$("#time-end").on("change", function () {
    update_duration();
    checkAvailability();
});

function change_action() {
    var d;
    $("#rental-info").show("slow");
    if ($("#chk-action-giveout").is(":checked")) {
        d = new Date();
        $(".timer-start").addClass("timer-active");
        $(".timer-start").val(time2str(d.getFullYear(), d.getMonth(), d.getDate(), d.getHours(), d.getMinutes()));

        if (itemsCount() > 0) {
            $("#btn-giveout").show("slow");
            $("#btn-permit").hide("slow");
        } else {
            $("#btn-giveout").hide("slow");
            $("#btn-permit").show("slow");
        }
        $("#btn-schedule").hide("slow");
        $("#btn-cancel").hide("slow");

    } else
        if ($("#chk-action-schedule").is(":checked")) {
            d = bookingDateTime;
            $(".timer-start").removeClass("timer-active");
            $(".timer-start").val(time2str(d.getFullYear(), d.getMonth(), d.getDate(), d.getHours(), d.getMinutes()));

            $("#btn-giveout").hide("slow");
            $("#btn-permit").hide("slow");
            $("#btn-schedule").show("slow");
            if (scheduled) {
                $("#btn-cancel").show("slow");
            }
        }
    update_endtime();
    checkAvailability();
}


function change_rentaltype() {
    $("#input-duration").prop("disabled", $("#chk-onetime").is(":checked"));
    if ($("#chk-onetime").is(":checked")) {
    } else
        if ($("#chk-hourly").is(":checked")) {
        } else
            if ($("#chk-onetime").is(":checked")) {
            }
    update_endtime();
    checkAvailability();
}


$("#chk-action-schedule").on("change", function () {
    change_action()
});
$("#chk-action-giveout").on("change", function () {
    change_action()
});


$("#chk-hourly").on("change", function () {
    change_rentaltype()
});
$("#chk-daily").on("change", function () {
    change_rentaltype()
});
$("#chk-onetime").on("change", function () {
    change_rentaltype()
});