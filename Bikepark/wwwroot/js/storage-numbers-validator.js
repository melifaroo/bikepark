
function setupStorageValidator() {

    validator = $("#numbers-form").validate(
        {
            ignore: [],
            rules: {
                "#new-item-number": {
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