var main = main || (function (window, $) {
    "use strict";

    function GetPostNagivation(id) {
        let data = {
            postId: id
        };
        let parameter = $.param(data);
        aqs.ajaxCall.setParams("TravelBlog/GetPostNagivation", "GET", parameter)
            .execute((response) => {
                if (response.isSuccessStatusCode == false) {
                    console.log("Error when get post nagivation post id:" + id);
                }
                else {
                    RenderPostNagivation(response);
                }
            })
    }

    function RenderPostNagivation(data) {
        let rawPreHtml = ``;
        if (data.isExsistPre) {
            rawPreHtml = `<a title="${data.previousPostTitle}" class="btn-blog left" href="${data.previousPostUrl}">
                          <img src="${data.previousPostImageUrl}"><svg width="49" height="24" viewBox="0 0 49 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                          <path d="M11.9639 0.988465C12.3701 0.980653 12.7412 1.22284 12.9053 1.59393C13.0654 1.96893 12.9834 2.40253 12.6982 2.69159L4.72168 11.0002H47.9756C48.335 10.9963 48.6709 11.1838 48.8506 11.4963C49.0342 11.8088 49.0342 12.1916 48.8506 12.5041C48.6709 12.8166 48.335 13.0041 47.9756 13.0002H4.72168L12.6982 21.3088C13.0811 21.7072 13.0654 22.34 12.667 22.7228C12.2686 23.1057 11.6357 23.0939 11.2529 22.6916L0.987305 12.0002L11.2529 1.30878C11.4365 1.10956 11.6943 0.996278 11.9639 0.988465Z" fill="black" />
                            </svg>${data.previousPostContent}
                        </a>`;
        }
        else {
            rawPreHtml = `<a class="btn-blog left"></a>`
        }

        let rawNextHtml = ``;
        if (data.isExistNext) {
            rawNextHtml = `<a title="${data.nextPostTitle}" class="btn-blog right" href="${data.nextPostUrl}">
                            ${data.nextPostContent}
                            <svg width="49" height="24" viewBox="0 0 49 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                            <path d="M37.0361 0.988465C36.6299 0.980653 36.2588 1.22284 36.0947 1.59393C35.9346 1.96893 36.0166 2.40253 36.3018 2.69159L44.2783 11.0002H1.02441C0.665039 10.9963 0.329101 11.1838 0.149414 11.4963C-0.0341797 11.8088 -0.0341797 12.1916 0.149414 12.5041C0.329101 12.8166 0.665039 13.0041 1.02441 13.0002H44.2783L36.3018 21.3088C35.9189 21.7072 35.9346 22.34 36.333 22.7228C36.7314 23.1057 37.3643 23.0939 37.7471 22.6916L48.0127 12.0002L37.7471 1.30878C37.5635 1.10956 37.3057 0.996278 37.0361 0.988465Z" fill="black" />
                            </svg>
                            <img src="${data.nextPostImageUrl}">
                        </a>`;
        }
        else {
            rawNextHtml = `<a style="display: none;" class="btn-blog right"></a>`
        }

        let rawHtml = rawPreHtml + rawNextHtml;
        $('#divNavigation').append(rawHtml);
    }

    return {
        GetPostNagivation
    }

})(window, jQuery);