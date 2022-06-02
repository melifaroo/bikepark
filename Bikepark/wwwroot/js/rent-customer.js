﻿$("#btn-search-customer").on("click", function () {
    var request = $("#customer-num").val();
    var url = url_rental_searchcustomer;
    $.get(url, { Request: request },
        function (result) {
            $("#сustomer-search-results").html(result);
            $("#сustomer-search-results").show("slow");
        }
    );
});

$("#btn-more-customer-detail").on("click", function () {
    $(".customer-more-detail").toggle("slow");
});

$("#btn-customer").on("click", function () {
    $("#customer-details").toggle("slow");
});


$(document).on("click", ".btn-cancel-customer", function () {
    $("#сustomer-search-results").hide("slow");
});

$(document).on("click", ".btn-select-customer", function () {
    var id = $(this).data("customerid").toString();
    var url = url_rental_customer;
    $.get(url, { CustomerID: id },
        function (result) {
            $("#customer-name").val(result.customerFullName);
            $("#customer-num").val(result.customerContactNumber);
            $("#customer-email").val(result.customerEMail);
            $("#customer-doc").val(result.customerPassport);
            $("#customer-id").val(result.customerID);
            $("#chk-new-customer").prop("checked", false).prop("disabled", true);
            $("#chk-upd-customer").prop("checked", false).prop("disabled", true);
        }
    );
    $("#сustomer-search-results").hide("slow");
});

function new_upd_customer_mode() {
    id = $("#customer-id").val();
    var newcust = (id == null || id == "" || id <= 0);
    $("#chk-upd-customer").prop("checked", !newcust).prop("disabled", newcust);
    $("#chk-new-customer").prop("checked", newcust).prop("disabled", newcust);
}

$(document).on("change", ".customer-input", function () {
    new_upd_customer_mode()
});

$(document).on("change", "#chk-new-customer", function () {
    id = $("input[name='Customer.CustomerID']").val();
    if (id != null && id != "" && id > 0) {
        $("#chk-upd-customer").prop("checked", !$("#chk-new-customer").is(":checked"));
    }
});

$(document).on("change", "#chk-upd-customer", function () {
    id = $("input[name='Customer.CustomerID']").val();
    if (id != null && id != "" && id > 0) {
        $("#chk-new-customer").prop("checked", !$("#chk-upd-customer").is(":checked"));
    }
});