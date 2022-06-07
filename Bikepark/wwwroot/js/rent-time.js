function setupTimer() {

    var time = new Date(),
        secondsRemaining = (60 - time.getSeconds()) * 1000;
    timer();
    setTimeout(function () {
        timer();
        setInterval(timer(), 60000);
    }, secondsRemaining);
}

function timer() {
    d = new Date();
    $("#rental-control input .timer-active").val(time2str(d));
    $("#rental-control :not(input) .timer-active").html(time2text(d));
    if (record.Status < 2)
        update_endtime();
    if (record.Status == 2)
        update_duration_actual();
}

var prevScheduleStart;

function update_endtime() {
    d = new Date($("#time-start").val());
    h = $("#duration").val();
    d = timeAfterHours(d, h)
    if (!$("#time-end").is(":focus"))
        $("#time-end").val(time2str(d));
}

function update_duration() {
    d1 = new Date($("#time-start").val());
    d2 = new Date($("#time-end").val());
    $("#duration").val(Math.ceil10(durationHours(d1, d2),-1));
}
function update_duration_actual() {
    d1 = new Date($("#time-start").val());
    d2 = new Date();
    $("#duration-current").html("в прокате: " + duration2str(durationHours(d1, d2)));
}

$("#duration").on("change", function () {
    $(this).val(Math.ceil10($(this).val(),-1));
    update_endtime();
    change_action();
});

$("#time-start").on("change", function () {
    update_endtime();
    //updateallprices();
    change_action();
});

$("#time-end").on("change", function () {
    update_duration();
    change_action();
});

$("#time-action-now").on("change", function () {
    change_action();
    if ($(this).is(":checked"))
        $("#time-action").val(time2str(new Date()));
});

$("#time-start-now").on("change", function () {
    change_action();
    if ($(this).is(":checked"))
        $("#time-start").val(time2str(new Date()));
    update_endtime();
});

function duration2str(t) {
    var h = Math.floor(t)
    var m = Math.floor((t - h) * 60);
    return ((h > 0) ? (h + " час ") : "") + ((m > 0) ? (m + " мин") : "")
}

function durationHours(d1, d2) {
    var h = (d2.getTime() - d1.getTime()) / 60 / 60 / 1000;
    return h;
}

function timeAfterHours(d, h) {
    d.setTime(d.getTime() + h * 60 * 60 * 1000);
    return d;
}