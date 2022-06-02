var validator;

$(document).ready(function () {
    setupValidator()
    if (error) {
        var $validator = $("#item-form").validate();
        /* Show errors on the form */
        $validator.showErrors({ ItemNumber: error });
    }
});

function setupValidator() {

    validator = $("item-form").validate(
        {
            ignore: [],
            rules: {
                ItemNumber: {
                    required: true
                },
            },
            messages: {
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