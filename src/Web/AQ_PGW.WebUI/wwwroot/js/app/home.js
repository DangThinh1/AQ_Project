$(document).ready(function () {
    (function () {
        CallPage();
    })();
})

function CallPage(page) {
    $.ajax({
        url: "Home/IndexPage",
        type: 'POST',
        data: { page: page },
        cache: false,
        async: true,
        dataType: "html",
        success: function (data) {
            //called when successful
            var table = $('#partialTable');
            table.html(data);
            console.log(data);
        },
        error: function (e) {
            //called when there is an error
            console.log(e);
        }
    });
}