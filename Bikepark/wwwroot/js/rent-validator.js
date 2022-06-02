
function setupValidator() {

    jQuery.validator.addMethod("ItemsAvailable",
        function (value, element, params) {

            var BreakException = {};
            try {
                $(params).each(function (i, item) {
                    if ($(item).hasClass("unavailable"))
                        throw BreakException;
                });
            } catch (e) {
                if (e !== BreakException) throw e;
                if (e == BreakException) return false;
            }

            return true;

        }, " Некоторые позиции на это время недоступны ");

    jQuery.validator.addMethod("dateGreaterThan",
        function (value, element, params) {

            if (!/Invalid|NaN/.test(new Date(value))) {
                return new Date(value) > new Date($(params).val());
            }

            return isNaN(value) && isNaN($(params).val())
                || (Number(value) > Number($(params).val()));
        }, 'Время возврата должно быть позже выдачи.'
    );

    jQuery.validator.addMethod("isOneDay",
        function (value, element, params) {

            if (!/Invalid|NaN/.test(new Date(value))) {
                var d1 = new Date(value);
                d1.setHours(0, 0, 0, 0);
                var d2 = new Date($(params).val());
                d2.setHours(0, 0, 0, 0);
                return d1.getTime() == d2.getTime()
            }

            return isNaN(value) && isNaN(params);
        }, 'Возврат должен быть в тот же день.'
    );

    var s = new Date(workingHoursStart);
    var e = new Date(workingHoursEnd);
    var workingHoursMessage = 'Рабочее время велоцентра с ' + zeroPadded(s.getHours()) + ":" + zeroPadded(s.getMinutes()) + ' до ' + zeroPadded(e.getHours()) + ":" + zeroPadded(e.getMinutes());
    jQuery.validator.addMethod("onWorkingHours",
        function (value, element, params) {

            if (!/Invalid|NaN/.test(new Date(value))) {
                var d = new Date(value);
                var s = new Date(params[0]);
                var e = new Date(params[1]);
                return d.getHours() * 60 + d.getMinutes() > s.getHours() * 60 + s.getMinutes() && d.getHours() * 60 + d.getMinutes() < e.getHours() * 60 + e.getMinutes();
            }

            return isNaN(value) && isNaN(params)
                || (Number(value) > Number(params));
        }, workingHoursMessage
    );

    validator = $("#rental-control").validate(
        {
            ignore: [],
            rules: {
                "Customer.CustomerContactNumber": {
                    required: true
                },
                End: {
                    dateGreaterThan: "#time-start",
                    isOneDay: "#time-start"
                },
                //End: { onWorkingHours: [workingHoursStart, workingHoursEnd] },
                Start: { onWorkingHours: [workingHoursStart, workingHoursEnd] },
                RecordID: { ItemsAvailable: '#rental-items-list .item-record' }
            },
            messages: {
                "Customer.CustomerContactNumber": {
                    required: "Укажите номер телефона"
                },
            },
            errorElement: 'span',
            errorClass: 'text-danger',
            errorPlacement: function (error, element) {
                if (element.parent('.input-group').length) {
                    error.insertAfter(element.parent());
                } else {
                    error.insertAfter(element);
                }
            }
        }
    );
}