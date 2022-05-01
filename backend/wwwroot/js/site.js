// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$("#reset").on("click", function () {

    $('.storage-filter-select').each(function () {
        this.value = "";
    });
    $('#storage-filter-form').submit();

});


