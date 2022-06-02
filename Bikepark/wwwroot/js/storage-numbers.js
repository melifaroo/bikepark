var validator;

$(document).ready(function () {
    setupStorageValidator()
    if (error) {
        var $validator = $("#numbers-form").validate();
        /* Show errors on the form */
        $validator.showErrors({ "new-item-number": error });
    }
});

async function previewNumbers() {
    var typeid = $(this).data("typeid");
    if (typeid) {
        try {
            const result = await $.get(url_itemtype_numbers, { TypeID: typeid });
            $("#item-numbers-preview").empty();
            $("#item-numbers-preview").append(result);

        } catch (err) {
            console.log(err);
        }
    }
}

$(document).on("click", "#btn-item-numbers-view", async function () {
    await previewNumbers()
});

$(document).on("click", "#remove-selected-item-numbers", async function () {
    $("#item-numbers .btn-remove:checked").closest(".item-number").remove();
});

$(document).on("change", "#item-numbers .btn-remove", async function () {

    $("#remove-selected-item-numbers").prop("disabled", (selectedNumbersCount() == 0));
});

function selectedNumbersCount() {
    return $("#item-numbers .btn-remove:checked").length;
}

function checknumber(number) {
    var exist = false;
    var BreakException = {};
    try {
        $("#item-numbers .item-number").each(function () {
            var itemnumber = $(this).data("itemnumber");
            exist = number == itemnumber;
            if (exist) throw BreakException;
        });
    } catch (e) {
        if (e !== BreakException) throw e;
    }
    return !exist;
}

function checknumbers(start, end) {
    var exist = false;
    var BreakException = {};
    try {
        $("#item-numbers .item-number").each(function () {
            var itemnumber = $(this).data("itemnumber");
            exist = itemnumber >= start && itemnumber <= end;
            if (exist) throw BreakException;
        });
    } catch (e) {
        if (e !== BreakException) throw e;
    }
    return !exist;
}

$(document).on("click", "#add-new-item-number", async function () {
    var typeid = $(this).data("typeid");
    var number = $("#new-item-number").val();
    if (number) {
        if (checknumber(number)) {
            try {
                const result = await $.get(url_add_new_item_number, { ItemNumber: number, TypeID: typeid });
                if (result.message == undefined)
                    $("#item-numbers").append(result);
                else {
                    var $validator = $("#numbers-form").validate();
                    /* Show errors on the form */
                    $validator.showErrors({ "new-item-number": result.message });
                }
            } catch (err) {
                console.log(err);
            }
        } else {
            var $validator = $("#numbers-form").validate();
            /* Show errors on the form */
            $validator.showErrors({ "new-item-number": "Такой номер уже добавлен" });
        }
    } else {
        var $validator = $("#numbers-form").validate();
        /* Show errors on the form */
        $validator.showErrors({ "new-item-number": "Введите номер" });
    }
});


$(document).on("click", "#add-new-items-number-range", async function () {
    var typeid = $(this).data("typeid");
    var number_start = $("#new-items-start-number").val();
    var number_end = $("#new-items-end-number").val();
    if (number_start && number_end) {
        if (checknumbers(number_start, number_end)) {
            try {
                const result = await $.get(url_add_new_items_number_range, { ItemNumberStart: number_start, ItemNumberEnd: number_end, TypeID: typeid });
                if (result.message == undefined)
                    $("#item-numbers").append(result);
                else {
                    var $validator = $("#numbers-form").validate();
                    /* Show errors on the form */
                    $validator.showErrors({ "new-items-range": result.message });
                }
            } catch (err) {
                console.log(err);
            }
        } else {
            var $validator = $("#numbers-form").validate();
            /* Show errors on the form */
            $validator.showErrors({ "new-items-range": "Такой номер уже добавлен" });
        }
    } else {
        var $validator = $("#numbers-form").validate();
        /* Show errors on the form */
        $validator.showErrors({ "new-items-range": "Укажите диапазон номеров" });
    }
});