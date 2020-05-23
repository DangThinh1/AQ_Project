
$(document).ready(function () {
    // $("#navbarSupportedContent ul").find("li").eq(1).addClass("active");
    //SetHeightBanner();

    $("#hdprice").ionRangeSlider({
        type: "double",
        min: 0,
        max: 300,
        from: 0,
        to: 170,
        prefix: "$",
        drag_interval: true,
        min_interval: null,
        max_interval: null,
        extra_classes: "aq-ion"
    });

    $("#hdlength").ionRangeSlider({
        type: "double",
        min: 0,
        max: 150,
        from: 0,
        to: 150,
        postfix: " m",
        drag_interval: true,
        min_interval: null,
        max_interval: null,
        extra_classes: "aq-ion"
    });

    

});

// function SetHeightBanner() {
//     var screenw = $(window).width();
//     var screenh = $(window).height();
//     $(".bg-banner").css("height", (screenw / (1920 / 450)) + "px");
// }

function SlideMenuYachtDetailRelated() {
    //Menu Fixed
    var menuItems = $("#MenuScroll").find("a");
    var heightMenu = $(".header-menu").height();
    // so we can get a fancy scroll animation
    menuItems.click(function (e) {
        var href = $(this).attr("href"),
            offsetTop = href === "#" ? 0 : $(href).offset().top - heightMenu + 1;
        $('html, body').stop().animate({
            scrollTop: offsetTop
        }, 500);
        e.preventDefault();
    });

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
                    infinite: true,
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

    $('.slider-for').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        arrows: true,
        asNavFor: '.slider-nav'
    });

    $('.slider-nav').slick({
        slidesToShow: 5,
        slidesToScroll: 0,
        asNavFor: '.slider-for',
        dots: false,
        centerMode: true,
        focusOnSelect: true
    });
}