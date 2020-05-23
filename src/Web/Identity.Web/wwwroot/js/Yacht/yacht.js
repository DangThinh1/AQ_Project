//FIELD

//FUNCTION
$(document).ready(function () {
    //SlideMenuYachtDetailRelated();
    //LoadYacht();
    SlideMenuYachtDetailRelated();
});

function LoadYacht() {
    $.get(YachtObjectURL.YachtURL, null, (data) => {
        if (data !== null){
            $("#dvLoadYachtDetail").append(data);
            SlideMenuYachtDetailRelated();
        }
    });
}

$(document).on('click', '#btnViewYachtDetail', function () {
    let param = $(this).data("id");
    LoadYachtDetail('N140RQCBB47P');
});

function LoadYachtDetail(paramId) {
    $.get(YachtObjectURL.YachtDetailURL, { uniqueid: paramId }, (data) => {
        if (data !== null)
            $("#dvLoadYachtDetail").html(data);
    });
}

function SlideMenuYachtDetailRelated() {
    //Menu Fixed
    $('[data-slick-yacht]').slick({
        slidesToShow: 4,
        slidesToScroll: 4,
        infinite: false,
        responsive: [
            {
                breakpoint: 1025,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 2,
                }
            },
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            }
        ]
    });   
}