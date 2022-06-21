
$(document).ready(function () {
    $('#page-selection').bootpag({
        total: totalPages,
        page: 1,
        maxVisible: 4,
        leaps: true,
        firstLastUse: true,
        first: '←',
        last: '→',
        wrapClass: 'pagination',
        activeClass: 'active',
        disabledClass: 'disabled',
        nextClass: 'next',
        prevClass: 'prev',
        lastClass: 'last',
        firstClass: 'first'
    }).on("page", async function (event, /* page number here */ num) {
        try {
            const result = await $.get(urlPageSelect, { statuses: statuses, from: from, to: to, pageSize: pageSize, page: num });
            $("#content").html(result);
        } catch (err) {
            console.log(err);
        }
    });
    $(".pagination li").toggleClass("page-item");
    $(".pagination a").toggleClass("page-link");
});
