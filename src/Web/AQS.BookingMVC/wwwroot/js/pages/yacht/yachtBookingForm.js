

var yachtBookingForm = (function () { });

//#region Event Handler
$(document).ready(function () {
    LoadYachtBookingForm();
});

$(document).on("click", "#btnYachtBooking", (e) => {
    e.preventDefault();
    BookingYacht();
});
//#endregion


//#region Function
function LoadYachtBookingForm() {
    let yachtId = $("#Details_ID").val();
    var model = {
        YachtID: yachtId,
    };    
    $.post(DetailPageURL.LoadYachtBookForm, { model: model }, (res) => {
        if (res != null)
            $("#loadBookingForm").html(res);
        aqs.commonFunc.datePickerRange('.txtgetDate');
    });
};
function BookingYacht() {
    var yachtiId = $("#Details_ID").val();
    var model = {
        YachtID: yachtiId,
        Guest: $("#Guest").val(),
        FromDate: $('.txtgetDate').data('daterangepicker').startDate.format('DD MM YYYY'),
        ToDate: $('.txtgetDate').data('daterangepicker').endDate.format('DD MM YYYY'),
        Price: $("#priceTag").text().substr(1),
    }
    $.post(DetailPageURL.BookingYacht, { model: model }, (res) => {
        window.location.href = "YachtBooking/Checkout";
    });
}
//#endregion

