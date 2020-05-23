
const DEFAULT_DATE_FORMAT = "MM/DD/YYYY";
const SERVER_DATE_FORMAT = "YYYY-MM-D";
var aqs = aqs || {};

aqs.commonFunc = aqs.commonFunc || (function (window, $) {
    return {
        setDateRangePicker: function (idInput, options, onSelectedDate = null) {
            options = options || {}
            options.locale = {
                "format": DEFAULT_DATE_FORMAT,
            };
            options.autoUpdateInput = true;
            options.autoApply = true;

            $(idInput).daterangepicker(options, onSelectedDate);
        },
        getDateFromDateRange: function (idInput) {
            let dateRangePicker = $(idInput).data("daterangepicker");
            if (!dateRangePicker)
                return {};
            return {
                minDate: dateRangePicker.startDate.format(SERVER_DATE_FORMAT),
                maxDate: dateRangePicker.endDate.format(SERVER_DATE_FORMAT),
            }
        },
        datePickerRange: function (idInput) {
            $(idInput).daterangepicker({
                parentEl: "#calendar-modal-center",
                locale: { format: 'DD MMM YYYY' },
                autoUpdateInput: true,
                startDate: moment().startOf('days').add(1, 'days'),
                endDate: moment().startOf('hour').add(2, 'days'),
                minDate: moment().startOf('days').add(1, 'days'),
            });
        },
        datePickerRangeCustomize: function (idInput, startDate, endDate) {
            $(idInput).daterangepicker({
                parentE1: "#calender-modal-center",
                locale: { format: 'DD MMM YYYY' },
                autoUpdateInput: true,
                startDate: startDate,
                endDate: endDate,
                minDate: moment().startOf('days').add(1, 'days'),
            });           
        },
        formatCurrency: function (val) {
            if (!val || val === "")
                return "";
            return val.toLocaleString("en_US");
        },
        changLanguage: function (lang, langCode) {
            if (this.checkCookie()) {
                this.setCookie("lang", lang, 365);
                this.setCookie('langCode', langCode, 365);

            }
            else {
                $('.check-cookie-popup').css('display', '');
            }
            let newUrl =window.location.href.split("?")[0];           
            window.location.href= newUrl.replace("/" + BASE_VAR.langCode.toLowerCase() + "/", "/" + langCode.toLowerCase() + "/") + "?lang_id=" + lang;
            return false;
        },
        checkCookie:function(){
            var cookieEnabled = navigator.cookieEnabled;
            if (!cookieEnabled) {
                document.cookie = "testcookie";
                cookieEnabled = document.cookie.indexOf("testcookie") != -1;
            }
            if (!cookieEnabled)
                console.log("Cookie disabled");
            return cookieEnabled;
        },

        setCookie: function (name, value, exdays) {
            if (navigator.cookieEnabled) {
                var d = new Date();
                d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
                var expires = "expires=" + d.toUTCString();
                document.cookie = name + "=" + value + ";" + expires + ";path=/";
            }
        },
        getCookie: function (cname) {
            if (navigator.cookieEnabled) {
                var name = cname + "=";
                var decodedCookie = decodeURIComponent(document.cookie);
                var ca = decodedCookie.split(';');
                for (var i = 0; i < ca.length; i++) {
                    var c = ca[i];
                    while (c.charAt(0) == ' ') {
                        c = c.substring(1);
                    }
                    if (c.indexOf(name) == 0) {
                        return c.substring(name.length, c.length);
                    }
                }
            }
            return "";
        },
        closePopUpCookieWarning: function () {
            $('.check-cookie-popup').css('display', 'none');
        }
    }
})(this, jQuery);

aqs.ajaxCall = aqs.ajaxCall || (function ($, window, document, err) {
    "use strict";
    var setting = {
        url: "",
        type: "POST",
        data: {},
        err: "undefind"
    }

    return {
        set: function (option) {
            setting = $.extend({}, setting, option);
            return this;
        },
        post: function (url, data) {
            setting.method = "post";
            setting.data = data;
            setting.url = url;
            return this;
        },
        put: function (url, data) {
            setting.method = "PUT";
            setting.data = data;
            setting.url = url;
            return this;
        },
        delete: function (url, data) {
            setting.method = "DELETE";
            setting.data = data;
            setting.url = url;
            return this;
        },
        get: function (url, data) {
            setting.method = "get";
            setting.data = data;
            setting.url = url;
            return this;
        },
        setParams: function (url, type, data = null) {
            setting.data = data;
            setting.url = url;
            setting.type = type;
            return this;
        },
        execute: function (onSuccess, onError = null) {
            $.ajax(setting).then((response) => {
                onSuccess(response)
            }).catch((error) => {
                if (onError) {
                    console.log(error);
                    onError();
                }
            });
        }
    }
}(jQuery, window, document))

//swal alert 
aqs.alertHelper = aqs.alertHelper || (function ($, window, document, err) {
    "use strict";
    return {
        showInfo: function (msg) {
            swal({
                title: msg,
                text: "",
                icon: "info",
                button: "Ok"
            });
        },
        showSuccess: function (msg, msgButton) {
            swal({
                title: msg,
                text: "",
                icon: "success",
                button: msgButton
            });
        },
        showError: function (msg, msgButton) {
            swal({
                title: msg,
                text: "",
                icon: "warning",
                button: msgButton
            });
        },
        showConfirm: function (title, confirmmsg, cancelmsg, callback) {
            swal({
                title: title,
                text: "",
                icon: "warning",
                buttons: {
                    cancel: {
                        text: cancelmsg,
                        value: false,
                        visible: true,
                        closeModal: true
                    },
                    confirm: {
                        text: confirmmsg,
                        value: true,
                        visible: true,
                        className: "btn-blue",
                        closeModal: true
                    }
                }
            }).then(function (isConfirm) {
                if (isConfirm) {
                    callback();
                }
            });
        },
        showSuccessRedirect: function (title, messOK, url) {
            swal({
                title: title,
                text: "",
                icon: "success",
                buttons: {
                    cancel: false,
                    confirm: {
                        text: messOK,
                        value: true,
                        visible: true,
                        className: "btn-blue",
                        closeModal: true
                    }
                }
            }).then(function (isConfirm) {
                if (isConfirm) {
                    window.location.href = url;
                }
            });
        },
        //SHOW BOOTSTRAP NOTIFY

    }
}(jQuery, window, document))

//Notification
aqs.loadingHelper = aqs.loadingHelper || (function ($) {
    "use strict";
    return {
        show: function () {
            $("#ajax-loading").show();
        },
        hide: function () {
            $("#ajax-loading").hide();
        },
    }
}(jQuery))

aqs.notifyHelper = aqs.notifyHelper || (function ($) {
    "use strict";
    return {
        showSuccess: function (message) {
            this.show(message, "success");
        },
        showWarning: function (message) {
            this.show(message, "warning");
        },
        showError: function (message) {
            this.show(message, "danger");
        },
        show: function (message, type) {
            $.notify(
                {
                    message: message
                },
                {
                    placement: {
                        from: "top",
                        align: "right"
                    },
                    type: type
                }
            );
        }
    }
}(jQuery))

aqs.paging = aqs.paging || (function ($) {
    "use strict";
    function checkTotalPages($pagination, totalPages) {

        if (totalPages <= 1) {
            $pagination.hide();
        }
        else {
            $pagination.show();
        }
    }
    return {
        init: function (options) {
            this.options = options;
            this.options.totalPages = this.options.totalPages === 0 ? 1 : this.options.totalPages;
            let $pagination = $(this.options.divRenderId);
            $(options.divRenderId).twbsPagination({
                totalPages: options.totalPages,
                visiblePages: 7,
                onPageClick: function (event, page) {
                    options.onPageSelected(page);
                }
            });
            checkTotalPages($pagination, this.options.totalPages);
            return this;

        },
        setTotalPages: function (newTotalPages) {
            let defaultOpts = { totalPages: 1 };
            let $pagination = $(this.options.divRenderId);
            let currentPage = $pagination.twbsPagination('getCurrentPage');
            $pagination.twbsPagination('destroy');
            $pagination.twbsPagination($.extend({}, defaultOpts, {
                startPage: currentPage,
                totalPages: newTotalPages
            }));
            checkTotalPages($pagination, this.options.totalPages)
        }
    }
}(jQuery))
