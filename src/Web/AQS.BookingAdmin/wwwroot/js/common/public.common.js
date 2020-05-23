
const DEFAULT_DATE_FORMAT = "MM/DD/YYYY";
const SERVER_DATE_FORMAT = "YYYY-MM-D";
const UPLOAD_IMAGE_URL = "/FileStream/UploadImage";
const UPLOAD_IMAGE_TEMP_URL = "/FileStream/UploadTempImage";
const DEFAULT_PAGE_SIZE = 15;
var aqs = aqs || {};

aqs.commonFunc = aqs.commonFunc || (function (window, $) {
    //convert form params to obj
    function intCommonLib() {
        $.fn.extend({
            serializeJSON: function (exclude) {
                exclude || (exclude = []);
                return this.serializeArray().reduce(function (hash, pair) {
                    pair.value && !(pair.name in exclude) && (hash[pair.name] = pair.value);
                    return hash;
                }, {});
            }
        });
        $(document).ready(function () {
            //ajax loading
            $(document)
                .ajaxStart(
                    function () {
                        Pace.restart();
                    }).ajaxComplete(function () {


                    }).ajaxError(function () {

                    });
            $('body').tooltip({ selector: '[data-toggle="tooltip"]' });
            AddMenuSidebarClassActive();
        });

        $(document).on("hover", '[data-toggle="tooltip"]', function () {
            if (!$(this).data("tooltip"))
                $(this).tooltip();
        })

        function AddMenuSidebarClassActive() {
            $('.nav-sidebar li a').on('click', function () {
                $('.nav-item li a.active').removeClass('active');
            });
            let objNav = $('.nav-sidebar li a');
            for (let i = 0; i < objNav.length; i++) {
                if (window.location.pathname === objNav[i].pathname) {
                    $("#" + objNav[i].id).addClass('active');
                }
            }
        }
    }
    intCommonLib();
    let commonFuncs = {

        setDateRangePicker: function (idInput, options, onSelectedDate = null) {
            options = options || {}
            options.locale = {
                "format": DEFAULT_DATE_FORMAT

            };
            options.autoApply = true;
            $(idInput).daterangepicker(options, onSelectedDate);
        },
        setDatePicker: function (idInput, options, onSelectedDate = null) {
            options = options || {}
            options.autoUpdateInput = false;
            options.singleDatePicker = true;
            options.locale = {
                "format": DEFAULT_DATE_FORMAT,
                cancelLabel: 'Clear'

            };
            $(idInput).daterangepicker(options, onSelectedDate);
            $(idInput).on('apply.daterangepicker', function (ev, picker) {
                $(this).val(picker.startDate.format(DEFAULT_DATE_FORMAT));
            });
            $(idInput).on('cancel.daterangepicker', function (ev, picker) {
                $(this).val('');
            });
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
        formatCurrency: function (val) {
            if (!val || val === "")
                return "";
            return val.toLocaleString("en_US");
        },
        // handleOnUpload (accept param is an object: { id, url })
        uploadImage: function (file, handleOnUpload) {
            data = new FormData();
            data.append("file", file);
            $.ajax({
                data: data,
                type: "POST",
                url: UPLOAD_IMAGE_URL,
                cache: false,
                contentType: false,
                processData: false,
                success: function (response) {
                    if (handleOnUpload) {
                        var fileResult = { id: response.item1, url: response.item2 };
                        handleOnUpload(fileResult);
                    }
                }
            });
        },
        uploadImageTemp: function (file, handleOnUpload) {
            data = new FormData();
            data.append("file", file);
            $.ajax({
                data: data,
                type: "POST",
                url: UPLOAD_IMAGE_TEMP_URL,
                cache: false,
                contentType: false,
                processData: false,
                success: function (response) {
                    if (handleOnUpload) {                      
                        handleOnUpload(response);
                    }
                },
                error: function () {
                    aqs.notifyHelper.showError("Error when upload file!");
                }
            });
        },
        initSummerNote: function (elId, options) {
            var defaultOptions = {
                height: 500,
                tabDisable: true,
                fontNames: [
                    "Arial", "Arial Black",
                    "Comic Sans MS",
                    "Courier New",
                    "Helvetica",
                    "Impact",
                    "Tahoma",
                    "Times New Roman",
                    "Verdana",
                    "Roboto",
                    "Avenir LT Std",                  
                    "BanglaMN",
                    "Noto Sans Thai",
                    "Lora",
                    "Montserrat",
                    "Sawarabi Mincho",
                    "ZCOOL XiaoWei",
                    "NotoSanSC"
                ],
                fontNamesIgnoreCheck: [
                    "Avenir LT Std",                   
                    "BanglaMN",
                    "Noto Sans Thai",
                    "Lora",
                    "Montserrat",
                    "Sawarabi Mincho",
                    "ZCOOL XiaoWei",
                    "NotoSanSC"
                ],
                toolbar:
                    [
                        [
                            "style",
                            [
                                "style"
                            ]
                        ],
                        [
                            "font",
                            [
                                "bold",
                                "italic",
                                "underline",
                                "clear"
                            ]
                        ],
                        [
                            "fontsize",
                            [
                                "fontsize"
                            ]
                        ],
                        ["fontname", ["fontname"]],
                        [
                            "height",
                            [
                                "height"
                            ]
                        ],
                        [
                            "color",
                            [
                                "color"
                            ]
                        ],
                        [
                            "para",
                            [
                                "ul",
                                "ol",
                                "paragraph"
                            ]
                        ],
                        [
                            "table",
                            [
                                "table"
                            ]
                        ],
                        [
                            "insert",
                            [
                                "link",
                                "picture",
                                "video"
                            ]
                        ],
                        [
                            "view",
                            [
                                "fullscreen",                               
                                "help"
                            ]
                        ]
                    ]
            };
            let $el = $(elId);
            if (!$.fn.summernote)
                console.error("Summernote lib not found");
            else {
                let opts = $.extend(defaultOptions, options);
                $el.summernote(opts);
            }
        }
    }
    return commonFuncs;

})(this, jQuery);
aqs.fileHelper = aqs.fileHelper || (function (window, $) {
    function getExtension(filename) {
        var parts = filename.split('.');
        return parts[parts.length - 1];
    }

    function isImage(filename) {
        var ext = getExtension(filename);
        switch (ext.toLowerCase()) {
            case 'jpg':
            case 'gif':
            case 'bmp':
            case 'png':
                //etc
                return true;
        }
        return false;
    }
    function checkFileSize(file, maxSizeMb) {
        return (file.size / 10e5) <= maxSizeMb;
    }
    return {
        getExtension,
        isImage,
        checkFileSize
    }
}(this, jQuery))
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
                    onError(error);
                }
            });
        },
        executePromise: function () {
            var defer = $.Deferred();
            $.ajax(setting).then(function (response) {
                defer.resolve(response);
            }).catch(function (error) {
                defer.reject(error);
            });
            return defer;
        }
        // usage: $.when(defer1, def2).then(function(result1, result2) {});
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
        showConfirm: function (title, confirmmsg, cancelmsg,onConfirm,onCancel=null) {
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
                    onConfirm();
                }
                else {
                    if (onCancel)
                        onCancel();
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
            $(".wrapper").Toasts('create', {
                class: 'bg-' + type,
                title: 'Notification',
                autohide: true,
                position: 'topRight',
                delay: 10000,
                body: message
            })
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
