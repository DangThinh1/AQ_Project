//FIELD


//FUNCTIONAL
$(document).ready(function () {
    LoadDiningVenueDetail();
});


function LoadDiningVenueDetail() {
    let RestaurantID = $("#RestaurantID").val()


    $.get(DiningVenueDetailURL.LoadLstVenueDetail, { resId: RestaurantID }, (data) => {
        if (data != null)
            $("#dvLoadLstVenue").append(data);
        $('[data-slide-media-package]').slick({
            slidesToShow: 5,
            slidesToScroll: 5,
            arrows: true,
            infinite: false,
            responsive: [
                {
                    breakpoint: 768,
                    settings: {
                        slidesToShow: 3,
                        slidesToScroll: 3,
                    }

                },
                {
                    breakpoint: 425,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1,
                    }

                }
            ]
        })
    })
}
