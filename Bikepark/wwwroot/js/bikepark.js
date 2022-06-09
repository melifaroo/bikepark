
$(document).ready(function () {
    //$(".navbar-nav .nav-item.active").removeClass("active");
    //if(menu>"")
    //    $(".navbar-nav .nav-item." + menu).addClass("active");

    var time = new Date(),
        secondsRemaining = (60 - time.getSeconds()) * 1000;

    maintimer();
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
     * Корректировка округления десятичных дробей.
     *
     * @param {String}  type  Тип корректировки.
     * @param {Number}  value Число.
     * @param {Integer} exp   Показатель степени (десятичный логарифм основания корректировки).
     * @returns {Number} Скорректированное значение.
     */
    function decimalAdjust(type, value, exp) {
        // Если степень не определена, либо равна нулю...
        if (typeof exp === 'undefined' || +exp === 0) {
            return Math[type](value);
        }
        value = +value;
        exp = +exp;
        // Если значение не является числом, либо степень не является целым числом...
        if (isNaN(value) || !(typeof exp === 'number' && exp % 1 === 0)) {
            return NaN;
        }
        // Сдвиг разрядов
        value = value.toString().split('e');
        value = Math[type](+(value[0] + 'e' + (value[1] ? (+value[1] - exp) : -exp)));
        // Обратный сдвиг
        value = value.toString().split('e');
        return +(value[0] + 'e' + (value[1] ? (+value[1] + exp) : exp));
    }

    // Десятичное округление к ближайшему
    if (!Math.round10) {
        Math.round10 = function (value, exp) {
            return decimalAdjust('round', value, exp);
        };
    }
    // Десятичное округление вниз
    if (!Math.floor10) {
        Math.floor10 = function (value, exp) {
            return decimalAdjust('floor', value, exp);
        };
    }
    // Десятичное округление вверх
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
