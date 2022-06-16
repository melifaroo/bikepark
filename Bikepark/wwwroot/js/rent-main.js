var validator;

$(function () {
    setupValidator();
    setupInterface();
    setupTimer();  
});


$("#rental-control").on("submit", function (e) {
});

$("#rental-control").on("keyup keypress", function (e) {
    var keyCode = e.keyCode || e.which;
    if (keyCode === 13 ) {
        e.preventDefault();
        $("#rental-control input").next().focus();
        return false;
    }
});

function overlap(periods, s, e) {
    var overlap = false;
    var self = false;
    var status = -1;
    var BreakException = {};
    try {
        periods.forEach(period => {
            s1 = new Date(period["Start"]);
            e1 = new Date(period["End"]);
            status = period["Status"];
            //self = period["Item4"] == record.RecordID;// $("#record-id").val();
            overlap = (s <= e1) && (e >= s1);// && (!self);
            if (overlap) throw BreakException;
        });
    } catch (e) {
        if (e !== BreakException) throw e;
    }
    return { overlap: overlap, status: status };
}

function checkAvailability() {
    s = (record.Status < 2) ? new Date($("#time-start").val()) : new Date($("#time-action").val());
    e = new Date($("#time-end").val());
    var selfitems = [];
    $("#itemrecords-list .item-record:not([status=Closed])").each(function (i, tr) {
        key = $(tr).data("itemid");
        selfitems.push(key);
    });
    $("#itemrecords-list .item-record[status=Draft],[status=Scheduled]").each(function (i, tr) {
        key = $(tr).data("itemid");
        var o = { overlap: false, status: -1 };
        if (key in availability) {
            o = overlap(availability[key], s, e);
        }
        $(this)
            .toggleClass("unavailable", o.overlap)
            .toggleClass("active", o.overlap && (o.status == 2 || o.status == 5))
            .toggleClass("reserved", o.overlap && (o.status == 1 ));
    });
    $("#storage .number").each(function (i, number) {
        key = $(number).data("itemid");
        var ovlap = false;
        var statu = 0;
        if (key in availability) {
            const o = overlap(availability[key], s, e);
            ovlap = o.overlap;
            statu = o.status;
        }
        $(number)
            .toggleClass("btn-outline-success", !ovlap)
            .toggleClass("btn-outline-danger", ovlap && (statu == 2 || statu == 5))
            .toggleClass("btn-outline-warning", ovlap && (statu == 1))
            .toggleClass("order-first",  prepared.includes(key) && !ovlap)
            .toggleClass("prepared", prepared.includes(key) && !ovlap);
    });
    $("#storage .number-check").each(function (i, number) {
        key = $(number).data("itemid");
        $(number).prop("checked", selfitems.includes(key));
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
    start = ($("#time-start").val());
    end = ($("#time-end").val());
    try {
        const result = await $.get(url_rental_additem, { ItemID: id, Start: start, End: end });        
        $("#itemrecords-list").append(result);
        recid = $(result).attr("data-id");
        updateprices($("#itemrecords-list tr.item-record[data-id='"+recid+"']"));
    } catch (err) {
        console.log(err);
    }
}


function delete_item(id) {
    $("#itemrecords-list tr.item-record[data-itemid=" + id + "][status='Draft'],[data-itemid=" + id + "][status='Scheduled']").remove();
    $("#number-check-" + id).prop("checked", false);
}

$(document).on("change", "#storage .number-check", async function () {
    var itemid = $(this).data("itemid");
    if ($(this).is(":checked")) {
        await add_item(itemid);
    } else {
        delete_item(itemid);
    }
    change_action();  
});

$(document).on("click", "#itemrecords-list .btn-item-delete", function () {
    var itemid = $(this).data("itemid");
    delete_item(itemid);
    change_action();
});


$(document).on("click", "#btn-getbackall", function () {
    var action = "Closed";
    $("#itemrecords-list tr[status=Active] .chk-givenout:checked").prop("checked", false);
    $("#itemrecords-list tr[status=Active] .chk-getback").prop("checked", true);
    $("#itemrecords-list tr[status=Active] .status").val(action);
    change_action();
});

$(document).on("click", "#itemrecords-list .chk-item-action", function () {
    var irecid = $(this).data("irecid").toString();
    var action = $(this).val();
    $("#itemrecords-list .item-record[data-id=" + irecid + "] .status").val(action);
    change_action();
});

$(document).on("click", ".chk-action", function () {
    var action = $(this).val();
    //$("#status").val(action);
    var mode = $("input[type=radio][name=StatusAction]:checked").val();
    var giveout_mode = (record.Status < 3) && mode == "Active";
    var schedule_mode = (record.Status < 3) && mode == "Scheduled";
    var draft_mode = (record.Status < 3) && mode == "Draft";

    prevScheduleStart = schedule_mode && prevScheduleStart ? prevScheduleStart : new Date($("#time-start").val()) ;
    var d = !schedule_mode ? new Date() : prevScheduleStart;
    $("#time-start").val(time2str(d));
    update_endtime();
    change_action();
});

$("#btn-show-items-chooser").on("click", function () {
    $("#storage").toggle(300);
});

$("#btn-toggle-filters").on("click", function () {
    $(".chooser-filter-optional").toggle();
});

function itemsCount() {
    return $("#itemrecords-list tr").length;
}

function scheduledCount() {
    return $("#itemrecords-list tr[status=Scheduled]").length;
}
function givenoutCount() {
    return $("#itemrecords-list tr[status=Active]").length;
}

function giveoutCount() {
    return $("#itemrecords-list tr:not([status=Active]):not([status=Closed]):not([status=Service]):not([status=OnService]):not([status=Fixed])").length;
}

function servicedCount() {
    return $("#itemrecords-list tr[status=Service],[status=OnService],[status=Fixed]").length;
}

function serviceCount() {
    return $("#itemrecords-list tr[status=Active] .chk-service:checked").length;
}
function getbackCount() {
    return $("#itemrecords-list tr[status=Active] .chk-getback:checked").length;
}

function setupInterface() {

    console.log(record);
    console.log(prices);
    console.log(arcprices);


    $("#chk-schedule").prop("checked", record.Status == 1);
    $("#status-action").toggle(record.Status < 2);

    $("#time-start-now-option").toggle(record.Status < 2);
    $("#time-action-now-option").toggle(record.Status == 2);

    $("#time-end").prop("readonly", record.Status == 3);
    $("#duration").prop("disabled", record.Status == 3);

    var now = new Date();
    var start = new Date(record.Start);
    var end = new Date(record.End);

    $("#time-start").val(time2str(record.Status == 0 ? now : start));
    $("#time-end").val(time2str(record.Status == 0 ? timeAfterHours(now, 1) : end ));
    $("#duration").val(record.Status == 0 ? defaultRentTimeHours : durationHours(start, end) );
    $("#time-action").val(time2str(now)).addClass("timer-active");
    if (record.Status == 2) {
        $("#time-current").html(time2text(now)).addClass("timer-active");
        $("#duration-current").html(durationHours(start, now));
    }
    if (record.Status == 3) {
        $("#btn-show-items-chooser").prop("disabled", true);
        $("#itemrecords-list .pricing").prop("disabled", true);
        $("#customer-details input,button").prop("disabled", true);
    }

    switch (record.Status) {
        case 1://scheduled
            $("#time-start-label").val("К выдаче");
            $("#time-end-label").val("К возврату");
            $("#duration-label").val("На время [час]");
            break;
        case 2://active
            $("#time-start-label").html("Выдан");
            $("#time-end-label").html("К возврату (продлить)");
            $("#duration-label").html("Выдан на время (продлить)");
            break;
        case 3://closed
            $("#time-start-label").html("Выдан");
            $("#time-end-label").html("Принят");
            $("#duration-label").html("Продолжительность [час]");
            break;
        case 0:
        default:
            $("#time-start-label").html("К выдаче сейчас");
            $("#time-end-label").html("К возврату");
            $("#duration-label").html("На время [час]");
    }
    change_action()
    $("#rental-control").show();
}


function change_action() {

    var mode = $("input[type=radio][name=StatusAction]:checked").val();
    var giveout_mode = (record.Status < 2) && mode == "Active";
    var schedule_mode = (record.Status < 2) && mode == "Scheduled";
    var draft_mode = (record.Status < 2) && mode == "Draft";

    var start_now = $("#time-start-now").is(":checked");
    var action_now = $("#time-action-now").is(":checked");

    var res = record.ItemRecords.filter(itemrecord => { return itemrecord.Status == 1; })
    var recordScheduledCount = res.length;

    var price_change = priceChanged();

    var getback_count = getbackCount();
    var service_count = serviceCount();
    var serviced_count = servicedCount();
    var giveout_count = giveoutCount();
    var scheduled_count = scheduledCount();
    var givenout_count = givenoutCount();
    var repeal_count = recordScheduledCount - scheduled_count;
    var schedule_count = giveout_count - scheduled_count;
    var added_count = giveout_count - scheduled_count;

    s1 = new Date($("#time-start").val());
    e1 = new Date($("#time-end").val());
    s0 = new Date(record.Start);
    e0 = new Date(record.End);
    now = new Date();

    var overdue_time = durationHours(s0, now) > 0;
    var overdue = (record.Status == 1) && overdue_time > 0;

    var excess_time = durationHours(e0, now);
    var excess = (record.Status == 2) && excess_time > 0;

    var extend_time = durationHours(e0, e1) - durationHours(s0, s1);    
    var reduce_time = durationHours(e1, e0) - durationHours(s1, s0);

    var extend = record.Status != 0 && extend_time > 0 ;
    var reduce = record.Status != 0 && reduce_time > 0 ;
    var time_change = (s1.getTime() - s0.getTime() != 0 || extend_time != 0);
    var pass = giveout_mode && itemsCount() == 0
    var giveout = (giveout_mode || record.Status == 2) && giveout_count > 0;
    var schedule = schedule_mode && schedule_count > 0;
    var getback = record.Status == 2 && getback_count > 0;
    var service = record.Status == 2 && service_count > 0;
    var repeal = record.Status == 1 && schedule_mode && repeal_count > 0;

    var draft = draft_mode;

    $("#label-action-giveout").toggle(giveout);
    $("#label-action-pass").toggle(pass);
    $("#label-action-schedule").toggle(schedule);
    $("#label-action-repeal").toggle(repeal);
    $("#label-action-getback").toggle(getback);
    $("#label-action-service").toggle(service);
    $("#label-action-extend").toggle(extend);
    $("#label-action-reduce").toggle(reduce);
    $("#label-action-price").toggle(price_change);
    $("#label-action-schedule-time").toggle(record.Status == 1 && time_change);

    $("#giveout-count").html(giveout_count);
    $("#schedule-count").html(schedule_count);
    $("#givenout-count").html(givenout_count);
    $("#scheduled-count").html(scheduled_count);
    $("#repeal-count").html(repeal_count);
    $("#getback-count").html(getback_count);
    $("#service-count").html(service_count);
    $("#serviced-count").html(serviced_count);
    $("#extend-time").html(duration2str(extend_time));
    $("#reduce-time").html(duration2str(reduce_time));

    $("#btn-update").prop("disabled", !(pass || getback || service || giveout || repeal || schedule || extend || reduce || price_change || draft));
    $("#btn-update").toggleClass("btn-secondary", draft || !(pass || getback || service || giveout || repeal || schedule || extend || reduce || price_change));
    $("#btn-update").attr('formnovalidate', draft?'formnovalidate':null);
    $("#btn-update").toggleClass("btn-primary", pass || getback || service || giveout || repeal || schedule || extend || reduce || price_change);
    $("#btn-service").toggle(serviced_count > 0 && record.Status > 1).prop("disabled", serviced_count == 0 || record.Status < 2);

    $("#btn-cancel").toggle(schedule_mode && record.Status == 1).prop("disabled", !(scheduled_count > 0));
    $("#btn-getbackall").toggle(record.Status == 2).prop("disabled", !(givenout_count>0));

    $("#time-start-now-option").toggle(record.Status < 2 && giveout_mode);

    $("#time-start").prop("readonly", (giveout_mode && start_now) || record.Status > 1);
    $("#time-start").toggleClass("timer-active", (giveout_mode && start_now));
    $("#time-action-info").toggle(record.Status == 2 && (getback || service || giveout) )
    $("#time-action").prop("readonly", action_now );
    $("#time-action").toggleClass("timer-active", action_now );

    if (time_change)
        $("#itemrecords-list tr.item-record").each(function () { updateprices(this); });
    [price_total, price_account, price_change] = calculatePrice();

    $("#price").html((record.Price ? price_account.toFixed(0) : "0"));
    $("#price-change").html(((price_change > 0) ? ("(+" + price_change.toFixed(0) + ") => " + price_total.toFixed(0)) : ""));
    $("#price-change").toggleClass("attention-text", price_change > 0);
    $("#price-change").toggleClass("alert-text", excess > 0);

    $("#items").html((record.Status == 1) ? scheduledCount : (record.Status == 2 ? givenoutCount : 0));
    $("#items-change").html(added_count > 0 ? ("(+" + added_count + ")"):"");
    $("#items-change").toggleClass("attention-text", added_count > 0);

    if (extend_time>0)
        $("#time-change").html(("(+" + duration2str(extend_time) + ")"));
    else if (reduce_time>0)
        $("#time-change").html(("(-" + duration2str(reduce_time) + ")"));
    else
        $("#time-change").html((""));
    $("#time-change").toggleClass("attention-text", extend_time != 0 );

  
    $("#time-excess").html((excess && excess_time > 0) ? ("просрочен на " + duration2str(excess_time)) : "");
    $("#time-excess").toggleClass("attention-text", excess_time > 0);
    $("#time-excess").toggleClass("alert-text", excess_time > 0);

    $("#time-overdue").html((overdue && overdue_time > 0) ? ("просрочен на " + duration2str(overdue_time)) : "");
    $("#time-overdue").toggleClass("attention-text", overdue_time > 0);
    $("#time-overdue").toggleClass("warning-text", overdue_time > 0);
  

    checkAvailability();
}


$("#btn-contract").on("click", function () {
    //if (validator.form()) {
    //    try {
    //        $.post(url_contract)
    //            .done(function (data) {

    //            })
    //            .fail(function () {
    //                alert("error");
    //            });
    //    } catch (err) {
    //        console.log(err);
    //    }
    //}

})
