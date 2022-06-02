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
    $("#rental-items-list .item-record:not([status=Closed])").each(function (i, tr) {
        key = $(tr).data("itemid");
        selfitems.push(String(key));
    });
    $("#rental-items-list .item-record[status=Draft],[status=Scheduled]").each(function (i, tr) {
        key = $(tr).data("itemid");
        var o = { overlap: false, status: -1 };
        if (key in availability) {
            o = overlap(availability[key], s, e);
        }
        $(this)
            .toggleClass("unavailable", o.overlap)
            .toggleClass("active", o.overlap && (o.status == 2 || o.status == 6))
            .toggleClass("reserved", o.overlap && (o.status == 1 || o.status == 5));
    });
    Object.keys(availability).forEach(key => {    
        const o = overlap(availability[key], s, e);
        $("label[for='chk-add-" + key + "']")
            .toggleClass("btn-outline-success", !o.overlap)
            .toggleClass("btn-outline-danger", o.overlap && (o.status == 2 || o.status == 6))
            .toggleClass("btn-outline-warning", o.overlap && (o.status == 1 || o.status == 5));
    });
    selfitems.forEach( key => {
        $("#chk-add-" + key).prop("checked", true);
    });
}

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
            filtered = filtered && ($(this).children("td[data-field='" + field + "']").text().trim() == filter[field].trim());
        }
        $(this).toggle(filtered, "slow");
    });
});

async function add_item(id) {
    var recid = -1;
    try {
        const result = await $.get(url_rental_additem, { ItemID: id });        
        $("#rental-items-list").append(result);
        recid = $(result).attr("data-id");
        updateprices($("#rental-items-list tr.item-record[data-id='"+recid+"']"));
    } catch (err) {
        console.log(err);
    }
}


function delete_item(id) {
    $("#rental-items-list tr.item-record[data-itemid=" + id + "][status='Draft'],[data-itemid=" + id + "][status='Scheduled']").remove();
    $("#chk-add-" + id).prop("checked", false);
}

$(document).on("change", "#rental-items-chooser .chk-add", async function () {
    var itemid = $(this).data("itemid");
    if ($(this).is(":checked")) {
        await add_item(itemid);
    } else {
        delete_item(itemid);
    }
    change_action();  
});

$(document).on("click", "#rental-items-list .btn-item-delete", function () {
    var itemid = $(this).data("itemid");
    delete_item(itemid);
    change_action();
});


$(document).on("click", "#btn-getbackall", function () {
    var action = "Closed";
    $("#rental-items-list tr[status=Active] .chk-givenout:checked").prop("checked", false);
    $("#rental-items-list tr[status=Active] .chk-getback").prop("checked", true);
    $("#rental-items-list tr[status=Active] .status").val(action);
    change_action();
});

$(document).on("click", "#rental-items-list .chk-item-action", function () {
    var irecid = $(this).data("irecid").toString();
    var action = $(this).val();
    $("#rental-items-list .item-record[data-id=" + irecid + "] .status").val(action);
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
    $("#rental-items-chooser").toggle("slow");
});

$("#btn-rental-info").on("click", function () {
    $("#rental-info").toggle("slow");
});

$("#btn-rental-items").on("click", function () {
    $("#rental-items").toggle("slow");
});

function itemsCount() {
    return $("#rental-items-list tr").length;
}
function scheduledCount() {
    return $("#rental-items-list tr[status=Scheduled]").length;
}

function giveoutCount() {
    return $("#rental-items-list tr:not([status=Active]):not([status=Closed])").length;
}
function givenoutCount() {
    return $("#rental-items-list tr[status=Active] .chk-givenout:checked").length;
}
function serviceCount() {
    return $("#rental-items-list tr[status=Active] .chk-service:checked").length;
}
function getbackCount() {
    return $("#rental-items-list tr[status=Active] .chk-getback:checked").length;
}

function setupInterface() {

    console.log(record);
    console.log(prices);
    console.log(arcprices);


    $("#customer-details").toggle(record.Status==0);
    $("#status-action").toggle(record.Status < 2);
    $("#rental-info").toggle(record.Status < 3);
    $("#rental-info-active").toggle(record.Status == 2);
    $("#chk-schedule").prop("checked", record.Status == 1);

    $("#time-start-now-option").toggle(record.Status < 2);
    $("#time-action-now-option").toggle(record.Status == 2);

    $("#time-end").prop("readonly", record.Status == 3);
    $("#duration").prop("disabled", record.Status == 3);

    var now = new Date();
    var start = new Date(record.Start);
    var end = new Date(record.End);

    $("#time-start").val(time2str(record.Status == 0 ? now : start));
    $("#time-end").val(time2str(record.Status == 0 ? timeAfterHours(now, 1) : end ));
    $("#duration").val(record.Status == 0 ? 1 : durationHours(start, end) );

    switch (record.Status) {
        case 1://scheduled
            $("#time-start-label").val("К выдаче");
            $("#time-end-label").val("К возврату");
            $("#duration-label").val("На время [час]");
            $("#time-current-label").val("");
            $("#duration-current-label").val("");
            break;
        case 2://active
            $("#time-start-label").html("Выдан");
            $("#time-end-label").html("К возврату (продлить)");
            $("#duration-label").html("Выдан на время (продлить)");
            $("#time-action-label").html("Принять/выдать сейчас");
            $("#time-current-label").html("Текущее время");
            $("#duration-current-label").html("Время в прокате");

            $("#time-current").val(time2str(now)).addClass("timer-active");
            $("#time-action").val(time2str(now)).addClass("timer-active");
            $("#duration-current").val(  durationHours(start, now)  );
            break;
        case 3://closed
            $("#time-start-label").html("Выдан");
            $("#time-end-label").html("Принят");
            $("#duration-label").html("Продолжительность [час]");
            $("#time-current-label").html("");
            $("#duration-current-label").html("");

            $("#btn-show-items-chooser").prop("disabled", true);
            $("#rental-items-list .pricing").prop("disabled", true);
            $("#customer-details input,button").prop("disabled", true);
            break;
        case 0:
        default:
            $("#time-start-label").html("К выдаче сейчас");
            $("#time-end-label").html("К возврату");
            $("#duration-label").html("На время [час]");
            $("#time-current-label").html("");
            $("#duration-current-label").html("");

           // $("#status").val("Active");
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
    var giveout_count = giveoutCount();
    var scheduled_count = scheduledCount();
    var givenout_count = givenoutCount();
    var repeal_count = recordScheduledCount - scheduled_count;
    var schedule_count = giveout_count - scheduled_count;

    s1 = new Date($("#time-start").val());
    e1 = new Date($("#time-end").val());
    s0 = new Date(record.Start);
    e0 = new Date(record.End);
    now = new Date();
    //Math.round((d2.getTime() - d1.getTime()) / 60 / 60 / 1000)
    //var diffInDays = (date1.getTime() - date2.getTime()) / (1000 * 60 * 60 * 24);

    var overdue = durationHours(s0, now) > 0;
    var excess = durationHours(e0, now) > 0;

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

    $("#btn-giveout").toggle(giveout); 
    $("#btn-pass").toggle(pass);
    $("#btn-schedule").toggle(schedule);
    $("#btn-repeal").toggle(repeal);
    $("#btn-getback").toggle(getback); 
    $("#btn-service").toggle(service);
    $("#btn-extend").toggle(extend);
    $("#btn-reduce").toggle(reduce);
    $("#btn-price").toggle(price_change);
    $("#btn-schedule-time").toggle(record.Status != 0 && time_change);

    $("#giveout-count").html(giveout_count);
    $("#schedule-count").html(schedule_count);
    $("#givenout-count").html(givenout_count);
    $("#scheduled-count").html(scheduled_count);
    $("#repeal-count").html(repeal_count);
    $("#getback-count").html(getback_count);
    $("#service-count").html(service_count);
    $("#extend-time").html(duration2str(extend_time));
    $("#reduce-time").html(duration2str(reduce_time));

    $("#btn-update").show().prop("disabled", !(pass || getback || service || giveout || repeal || schedule || extend || reduce || price_change));
    $("#btn-draft").toggle(draft);

    $("#btn-cancel").toggle(schedule_mode && record.Status == 1).prop("disabled", !(scheduled_count > 0));
    $("#btn-getbackall").toggle(record.Status == 2).prop("disabled", !(givenout_count>0));

    $("#time-start-now-option").toggle(record.Status < 2 && giveout_mode);

    $("#time-start").prop("readonly", (giveout_mode && start_now) || record.Status > 1);
    $("#time-start").toggleClass("timer-active", (giveout_mode && start_now));
    $("#time-action-info").toggle(record.Status == 2 && (getback || service || giveout) )
    $("#time-action").prop("readonly", action_now );
    $("#time-action").toggleClass("timer-active", action_now );

    if (time_change)
        $("#rental-items-list tr.item-record").each(function () {
            updateprices(this);
        });
    if (time_change || price_change || giveout || repeal || schedule) {
    }

    [price_total, price_account, price_change] = calculatePrice();
    $("#price-total").val(price_total);
    $("#price-account").val(price_account);
    $("#price-change").val(price_change);

    checkAvailability();
   // validator.form();
}


