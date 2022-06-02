

$(document).ready(async function () {
    await previewNumbers();
    $("#item-numbers-preview .btn-remove-label").toggle(false);
});

async function  previewNumbers() {
    var typeid = $("#typeid").val();
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
