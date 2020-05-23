var main = main || (function (window, $) {
    "use strict";

    let isBusy = false;
    let currentPageIndex = 1;
    let totalPages = 0;

    function LoadMore() {
        aqs.ajaxCall.setParams("TravelBlog/TravelBlog/GetSearchCondition", "GET").execute((response) => {
            var data = JSON.parse(response);
            data.PageIndex = currentPageIndex;
            let parammeters = $.param(data);
            aqs.ajaxCall.setParams("TravelBlog/TravelBlog/GetTravelBlog", "GET", parammeters)
                .set({
                    beforeSend: function () {
                        aqs.loadingHelper.show();
                        isBusy = true;
                    },
                    complete: function () {
                        aqs.loadingHelper.hide();
                        isBusy = false;
                    },
                    error: function () {
                        isBusy = false;
                    }
                })
                .execute((response) => {
                    RenderTravelblog(response);
                });
        });
    }

    function RenderTravelblog(response) {
        if (response.data.lenght === 0) {
            return;
        }
        let listdvCol = [$('#dvCol1'), $('#dvCol2'), $('#dvCol3')];
        let index = 0;
        response.data.forEach((value) => {
            let renderItem = CreateRenderItem(value);
            if (window.innerWidth < 768) {
                $('#dvCol3').append(renderItem);
            }
            else {
                listdvCol[index].append(renderItem);
            }
            index++;

            if (index === 3) index = 0;
        });
    }

    function CreateRenderItem(item) {
        let renderItem =
            `<div class= "article">
            <a title="${item.title}" class="link-overlay" href="${item.customProperties.DetailUrl}"></a><img src="${item.customProperties.ImageUrl}" alt="${item.title}">
            <div class="content">
                <div class="top">
                    <h4 class="sub-title">${item.postCategoryName}</h4><span class="time">${item.timeToRead} min read</span>
                </div>
                <h3 class="title">${item.title}</h3>
                <p class="writer">${item.customProperties.Author}</p>
                <p class="text">${item.shortDescription}</p>
            </div>
        </div>`;

        return renderItem;
    }

    function InitSubscribe(msgObj) {
        $('#divFormSubscribe form').on('submit', function (e) {
            e.preventDefault();
            if (isBusy) {
                console.log("isBusy");
                return;
            }
            if ($(".g-recaptcha").is(":visible")) {
                let isCheckCaptcha = grecaptcha.getResponse();
                if (isCheckCaptcha === "") {
                    aqs.notifyHelper.showWarning("Please verify the captcha");
                    return;
                }
            }
            if (IsEmail($('#txtEmail').val()) == false) {
                aqs.notifyHelper.showWarning(msgObj.existEmail);
                return;
            }

            let data = $(this).serialize();

            aqs.ajaxCall.post("TravelBlog/Subscribe", data)
                .set({
                    beforeSend: function () {
                        aqs.loadingHelper.show();
                        isBusy = true;
                    },
                    complete: function () {
                        aqs.loadingHelper.hide();
                        isBusy = false;
                    },
                    error: function () {
                        isBusy = false;
                    }
                })
                .execute((response) => {
                    if (response === -1) {
                        //aqs.notifyHelper.showWarning("You have submited too much request in short time, please verify the captcha");
                        aqs.notifyHelper.showWarning(msgObj.warning);
                        $(".g-recaptcha").show();
                    }
                    else if (response === 1) {
                        //aqs.notifyHelper.showSuccess("Thanks for you registered email");
                        aqs.notifyHelper.showWarning(msgObj.success);
                        $('#divFormSubscribe input').val('');

                    }
                    else {
                        //aqs.notifyHelper.showSuccess("Your email already has been registered");
                        aqs.notifyHelper.showSuccess(msgObj.existEmail);
                        $('#divFormSubscribe input').val('');
                    }
                    grecaptcha.reset();
                })

            return false;
        });
    }

    function IsEmail(email) {
        var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        var result = regex.test(email);

        return result;
    }

    function checkViewMore() {
        if (totalPages === 0 || currentPageIndex === totalPages)
            $("#btnViewMore").hide();
        else {
            $("#btnViewMore").show();
        }
    }

    function InitLoadMore(total) {
        totalPages = total;
        checkViewMore();
        $("#btnViewMore").click(function () {
            currentPageIndex++;
            LoadMore();
            checkViewMore();
        });
    }

    function InitLanguage(languageCode) {
        //if (languageCode === null || languageCode === '') {
        //    //aqs.commonFunc.changLanguage(1, 'en-US');
        //}
    }

    function SortItem(div1, div2) {
        let div1_height = 0;
        $(div1).find('.article').each(function (index) {
            div1_height = div1_height + $(this).outerHeight();
        });

        let div2_height = 0;
        $(div2).find('.article').each(function (index) {
            div2_height = div2_height + $(this).outerHeight();
        });

        return (div1_height > div2_height)
    }

    return {
        InitLoadMore,
        InitSubscribe,
        InitLanguage
    }

})(window, jQuery);