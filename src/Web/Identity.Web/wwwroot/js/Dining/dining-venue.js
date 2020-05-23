//FIELD


//FUNCTIONAL
$(document).ready(function () {
    LoadDiningVenue();
});


function LoadDiningVenue() {
    var model = {
        RestaurantName: $("#ddlRestaurant").val(),
        BusinessDay: "",
        City: "",
        ZoneDistrict: "",
        Time: $("#txtCheckInDate").val(),
        Adults: $("#ddlAdult").val(),
    }

    $.post(VenueURl.SearchVenue, { searchModel: model }, (data) => {
        if (data != null)
            $("#divVenueLstDisplay").append(data);
    })
}


$(document).on("click", "#btnSearchVenue", function () {

});