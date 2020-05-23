(($) => {
    "use strict";

    /**
     * Global letiables
     */
    let pages = {
        homePage: 'homePage',
        newsPage: 'newsPage',
        newsPageDetail: 'newsPageDetail',
        yatchListPage: 'yatchListPage',
        yatchDetailPage: 'yatchDetailPage',
        diningListPage: 'diningListPage',
        diningDetailPage: 'diningDetailPage',
        checkoutPage: 'checkoutPage',
        comingsoonPage: 'comingsoonPage',
    };
    let currentPage = document.querySelector('.aq-main-content') && document.querySelector('.aq-main-content').getAttribute('id');
    /**
     * DOM Event Listener
     */
    $(document).ready(() => {
        appInit(currentPage);
    });

    /**
     * App Init
     */
    let appInit = (id) => {
        switch (id) {
            case pages.homePage:
                homeActions.init();
                break;
            case pages.newsPage:
                newsActions.init();
                break;
            case pages.newsPageDetail:
                newsPageDetailActions.init();
                break;
            case pages.yatchListPage:
                yatchListActions.init();
                break;
            case pages.yatchDetailPage:
                yatchDetailActions.init();
                break;
            case pages.diningListPage:
                diningListActions.init();
                break;
            case pages.diningDetailPage:
                diningDetailActions.init();
                break;
            case pages.checkoutPage:
                checkoutPageActions.init();
                break;
            case pages.comingsoonPage:
                comingsoonPageActions.init();
                break;
        }
    }

    /**
     * Function Declarations
     */

    // ************************************************* ALL PAGES *************************************************
    AOS.init({
        once: true,
    });

    let locationPicker = (select) => {
        $(select).select2();
    }

    let clickScrollTo = (btn, block) => {
        $(btn).click(() => {
            $('html, body').animate({
                scrollTop: $(block).offset().top - 100
            }, 1000)
        })
    }

    let clickShowMenuDes = () => {
        if ($(window).width() <= 768) {
            $("#res-menu-icon").click(() => {
                $('header').addClass('show-menu');
            })
            $(".aq-main-content").click(() => {
                $('header').removeClass('show-menu');
            })
        }
    }

    $("header").addClass('active');
    let scrollMenu = () => {
        // $("header").removeClass('active');
        let scrollTop = $("header").offset().top;
        if (scrollTop !== 0) {
            $("header").addClass('active');
        } else {
            $("header").removeClass('active');
        }
        $(window).on('scroll', () => {
            let scrollTop = $("header").offset().top;
            if (scrollTop !== 0) {
                $("header").addClass('active');
            } else {
                $("header").removeClass('active');
            }
        })
    }

    let formRangePicker = (name) => {
        let nameInput = $("input[name=" + name + "]");
        $(nameInput).daterangepicker({
            opens: 'right'
        }, (start, end) => {
            console.log('startTime: ' + start.format('MM/DD/YYYY') + ", endTime: " + end.format('MM/DD/YYYY'))
        });
    }

    let formDatePicker = (name) => {
        let nameInput = $("input[name=" + name + "]");
        $(nameInput).daterangepicker({
            singleDatePicker: true,
            showDropdowns: true,
            minYear: 1901,
            maxYear: parseInt(moment().format('YYYY'), 10)
        }, function (start) {
            // console.log(start)
        });
    }

    let clickSearch = () => {
        $('#btn-search-header').click(() => {
            $('#input-search-top').focus()
            $('html, body').animate({
                scrollTop: $("#input-search-top").offset().top - 300
            }, 1000)
        })
        $('#input-search-top').focus(() => {
            $("#search-expand-top").addClass("active");
        });
        $('#input-search-top').focusout(() => {
            $("#search-expand-top").removeClass("active");
        });
    }

    let activeForm = (btn, userForm) => {
        $(btn).click(() => {
            // e.stopPropagation();
            $(".aq-main-content").addClass("has-overflow");
            $(userForm).addClass("active");
        });
        $(".aq-main-content").click(() => {
            $(".aq-main-content").removeClass("has-overflow");
            $(userForm).removeClass("active");
        });
        $(".btn-close").click(() => {
            $(".aq-main-content").removeClass("has-overflow");
            $(userForm).removeClass("active");
        });
    }

    let activeFormCheckout = (btnConfirm, btnClose, form) => {
        $(btnConfirm).click(() => {
            $(form).addClass("active");
        });
        $(btnClose).click(() => {
            $(form).removeClass("active");
        });
    }
    // ************************************************* HOME *************************************************
    let homeActions = {
        init: () => {
            scrollMenu();
            sliderHome($(".home-block-3-slide-img"));
            clickSearch();
            activeForm("#btn-login-menu", "#user-login-form");
            activeForm("#btn-register-menu", "#user-register-form");
        }
    };
    let sliderHome = (slider) => {
        slider.slick({
            dots: false,
            infinite: false,
            slidesToShow: 3,
            slidesToScroll: 1,
            responsive: [
                {
                    breakpoint: 1024,
                    settings: {
                        slidesToShow: 3,
                        slidesToScroll: 3,
                        infinite: true,
                        dots: true,
                    }
                },
                {
                    breakpoint: 992,
                    settings: {
                        slidesToShow: 2,
                        slidesToScroll: 2
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
        $('.btn-next').click(function () {
            slider.slick('slickNext');
        });
        $('.btn-prev').click(function () {
            slider.slick('slickPrev');
        });
    }
    // ************************************************* NEWS *************************************************
    let newsActions = {
        init: () => {
            scrollMenu();
            clickShowMenuDes();
        }
    };
    // ************************************************* NEWS *************************************************
    let newsPageDetailActions = {
        init: () => {
            handleSubscribeDetail();
            scrollMenu();
            clickShowMenuDes();
            handleLanguageButton();
        }
    };
    // ************************************************* YATCH LIST *************************************************
    let yatchListActions = {
        init: () => {
            formRangePicker("dateRangeYatch");
            locationPicker($('#select-location-yatch'));
        }
    };
    // ************************************************* YATCH DETAIL *************************************************
    let yatchDetailActions = {
        init: () => {
            formRangePicker("dateRangeYatch");
            locationPicker($('#select-location-yatch'));
            sliderDetail($(".slider-yatch-detail"));
            formDatePicker("formDatePicker");

            clickScrollTo($('.btn-overview'), $('.block-overview'));
            clickScrollTo($('.btn-details'), $('.block-details'));
            clickScrollTo($('.btn-amenities'), $('.block-amenities'));
            clickScrollTo($('.btn-travel'), $('.block-travel'));
        }
    };


    let sliderDetail = (slider) => {
        slider.slick({
            dots: false,
            infinite: false,
            slidesToShow: 1
        });
        $('.btn-next').click(function () {
            slider.slick('slickNext');
        });
        $('.btn-prev').click(function () {
            slider.slick('slickPrev');
        });
    }

    // ************************************************* DINING LIST *************************************************
    let diningListActions = {
        init: () => {
            formRangePicker("dateRangeDining");
            formRangePicker("dateRangeDiningBooking");
            locationPicker($('#select-location-dining'));
        }
    };
    // ************************************************* DINING DETAIL *************************************************
    let diningDetailActions = {
        init: () => {
            formRangePicker("dateRangeDining");
            locationPicker($('#select-location-dining'));
            sliderDetail($(".slider-dining-detail"));
            formDatePicker("fromdatepickerdining");

            clickScrollTo($('.btn-overview-dining'), $('.block-overview'));
            clickScrollTo($('.btn-location-dining'), $('.block-location-dining'));
            clickScrollTo($('.btn-menu-dining'), $('.block-menu-dining'));
            clickScrollTo($('.btn-review-dining'), $('.block-reviews-dining'));
        }
    };

    // ************************************************* CHECKOUT *************************************************

    let checkoutPageActions = {
        init: () => {
            formRangePicker("dateCheckinCheckOut");
            activeFormCheckout($('.btn-submit'), $('#btn-close-form-complete'), $('.wrap-form-complete'));
        }
    };

    // ************************************************* COMING SOON *************************************************

    let comingsoonPageActions = {
        init: () => {
            scrollMenu();
            handleSubscribeComingsoon();
            clickScrollTo($('#btn-subscribe-header'), $('.form-comingsoon'));
            clickShowMenuDes();
            handleLanguageButton();
        }
    };

    let handleSubscribeComingsoon = () => {
        let message = $(".btn-subscribe-header").attr('name');
        let btnSub = `<a id="btn-subscribe-header" href="javascript:void(0)">${message}</a>`
        $(".btn-subscribe-header").append(btnSub)
    }

    let handleSubscribeDetail = () => {
        let message = $(".btn-subscribe-header").attr('name');
        let btnSub = `<a id="btn-subscribe-header" href="/">${message}</a>`
        $(".btn-subscribe-header").append(btnSub);
    }

    let handleLanguageButton = () => {
        let currentLanguage = aqs.commonFunc.getCookie('lang');
        if (currentLanguage === null || currentLanguage === '') {
            aqs.commonFunc.changLanguage(1);
        }

        let value = $('#lang_' + currentLanguage).text();
        $('#lang_' + currentLanguage).addClass('active');
        $('#dropdownMenuButton').text(value);
    }
})(jQuery);