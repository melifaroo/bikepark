
$(document).ready(async function () {

    var time = new Date(),
        secondsRemaining = (60 - time.getSeconds()) * 1000;

    await maintimer();

    $(".nav-alert:not(:empty)").show();

    setTimeout(async function () {
        maintimer();
        await setInterval( await maintimer(), 60000 );
    }, secondsRemaining);
});

async function maintimer() {
    d = new Date();
    try {
        const result = await $.get("/rental/stat");
        var activeWarning = result.activeWarning - result.activeOverdue;
        var scheduledWarning = result.scheduledWarning - result.scheduledOverdue;
        $("#active-record-count").text(result.active);
        $("#active-warning").text(activeWarning == 0 ? "" : activeWarning);
        $("#active-overdue").text(result.activeOverdue ==0 ? "" : result.activeOverdue);
        $("#scheduled-record-count").text(result.scheduled);
        $("#scheduled-warning").text(scheduledWarning == 0 ? "" : scheduledWarning);
        $("#scheduled-overdue").text(result.scheduledOverdue == 0 ? "" : result.scheduledOverdue);
        $("#service-record-count").text(result.service);
        $("#service-warning").text(result.serviceWarning == 0 ? "" : result.serviceWarning);
        $("#service-needaction").text(result.serviceNeedAction == 0 ? "" : result.serviceNeedAction);

    } catch (err) {
        console.log(err);
    }
}


(function () {
    /**
     * Adjustment of Decimal Rounding
     *
     * @param {String}  type 
     * @param {Number}  value
     * @param {Integer} exp   Exponent (decimal logarithm of the adjustment base).
     * @returns {Number} Adjusted value
     */
    function decimalAdjust(type, value, exp) {
        // If the degree is undefined or equal to zero...
        if (typeof exp === 'undefined' || +exp === 0) {
            return Math[type](value);
        }
        value = +value;
        exp = +exp;
        // If the value is not a number, or the exponent is not an integer...
        if (isNaN(value) || !(typeof exp === 'number' && exp % 1 === 0)) {
            return NaN;
        }
        // Decimal Shift
        value = value.toString().split('e');
        value = Math[type](+(value[0] + 'e' + (value[1] ? (+value[1] - exp) : -exp)));
        // Right shift
        value = value.toString().split('e');
        return +(value[0] + 'e' + (value[1] ? (+value[1] + exp) : exp));
    }

    // Decimal rounding to the nearest
    if (!Math.round10) {
        Math.round10 = function (value, exp) {
            return decimalAdjust('round', value, exp);
        };
    }
    // Round down to the nearest decimal place
    if (!Math.floor10) {
        Math.floor10 = function (value, exp) {
            return decimalAdjust('floor', value, exp);
        };
    }
    // Round up to the nearest decimal place
    if (!Math.ceil10) {
        Math.ceil10 = function (value, exp) {
            return decimalAdjust('ceil', value, exp);
        };
    }
})();


function zeroPadded(val) {
    if (val >= 10)
        return val;
    else
        return "0" + val;
}

function time2str(d) {
    return d.getFullYear() + "-" + zeroPadded(d.getMonth() + 1) + "-" + zeroPadded(d.getDate()) + "T" + zeroPadded(d.getHours()) + ":" + zeroPadded(d.getMinutes())
}
function time2text(d) {
    return d.getFullYear() + "-" + zeroPadded(d.getMonth() + 1) + "-" + zeroPadded(d.getDate()) + " " + zeroPadded(d.getHours()) + ":" + zeroPadded(d.getMinutes())
}
