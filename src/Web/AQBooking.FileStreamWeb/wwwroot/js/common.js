$(document).ready(function () {

    // Show loading spinner when ajax start
    $(document).on("ajaxStart", function () {
        $("#imgLoading").css("display", "flex");
    }).on("ajaxStop", function () {
        $("#imgLoading").hide();
    }).on("ajaxComplete", (event, jqxhr, options) => {
        var jsonHeader = jqxhr.getResponseHeader("X-Responded-JSON");
        if (jsonHeader) {
            swal({
                title: "Session expiried",
                message: "Your session has been expired, The page will auto redirect to login page"
            }, () => {
                window.location.reload();
            });
        }
    }).on("ajaxError", function (event, jqxhr, settings, thrownError) {
        $("#imgLoading").hide();

        if (jqxhr.status === "401") {
            window.location.reload();
        }
        else if (jqxhr.status === "403") {
            window.location.href = "/Common/AccessDenied";
        } else {
            console.log("error")
        }

    }).on("ajaxSuccess", function () {
        $('[data-toggle="tooltip"]').tooltip({
            container: 'body'
        });
        if ($.fn.datetimepicker) {
            $(".datepicker").prop("autocomplete", "off");
            $(".datepicker").datetimepicker({
                format: 'DD-MMM-YYYY'
            });
        }
        if ($.fn.filestyle)
            $('.filestyle').filestyle();
    });
});

////Show alert
//function showAlert(type, msg) {
//    var status = "info";
//    var icon = "fa-detail";
//    if (type === "success") {
//        status = "success";
//        icon = "fa-check";
//    } else if (type === "warning") {
//        status = "warning";
//        icon = "fa-warning";
//    }
//    else if (type === "error") {
//        status = "danger";
//        icon = "fa-minus-circle";
//    }
//    $.notify({ status: status, message: "<i class='fa " + icon + "'></i> " + msg, pos: "bottom-center" });
//}

//Show confirm


//Custom validator
function customValidator(form) {

    if ($.validator && !form.data('validator')) form.validate();
    var validator = form.data('validator');
    if (validator) {
        validator.submitHandler = function (form) {
            //if (!form.valid())
            //    form.find(".error:first").focus();

        };

        validator.settings.ignore = ':hidden, [readonly=readonly]';
        validator.settings.invalidHandler = function (event, validator) {
            var errors = validator.numberOfInvalids();
            if (errors > 0)
                form.find(".error:first").focus();
        };

        validator.settings.showErrors = function (errorMap, errorList) {
            // Clean up any tooltips for valid elements
            $.each(this.validElements(), function (index, element) {
                var $element = $(element);
                $element.attr("title", "").attr("data-original-title", "") // Clear the title - there is no error associated anymore
                    .removeClass("error");
                //.tooltip("destroy");
                if ($element.hasClass("select2-hidden-accessible")) {
                    var span = $element.next().css("border", "");
                    span.data("title", "").tooltip("destroy");
                }
            });
            // Create new tooltips for invalid elements
            $.each(errorList, function (index, error) {
                if (index === 0) {
                    var $element = $(error.element);
                    $element.attr("title", ""); // Clear the title - there is no error associated anymore
                    // .tooltip("destroy");                   
                    errorMsg = error.message;
                    if (error.method === "required") {
                        errorMsg = "Please input this field";
                    }

                    if (!$element.hasClass("date")) {
                        if ($element.hasClass("select2-hidden-accessible")) {
                            var span = $element.next().css("border", "solid 1px #d41212");
                            span.attr("title", errorMsg).addClass("error").tooltip("show");
                        }
                        else
                            $element.attr("title", errorMsg).addClass("error").tooltip("show");
                    }
                    else {
                        $element.addClass("error");
                    }
                    $element.tooltip("show");
                    return;

                }
            });
        };
    }
}

//Clear form
function clearForm(form) {
    $(':input', `#${form}`)
        .not(':button, :submit, :reset, :hidden')
        .val('')
        .prop('checked', false)
        .prop('selected', false);
}

//Config datetime picker
function initDatePicker() {
    $('.datepicker').datepicker({
        orientation: 'bottom',
        format: 'dd-M-yyyy',        
        showSecond: true,
        icons: {
            time: 'fa fa-clock-o',
            date: 'fa fa-calendar',
            up: 'fa fa-chevron-up',
            down: 'fa fa-chevron-down',
            previous: 'fa fa-chevron-left',
            next: 'fa fa-chevron-right',
            today: 'fa fa-crosshairs',
            clear: 'fa fa-trash'
        }
    });
}

////Config select2 
//$('.select2').select2({
//    placeholder: 'Please select',
//    allowClear: true,
//    theme: 'bootstrap4'
//});

//Change page function
$(document).on("click", ".page-link", function (e) {
    e.preventDefault();
    var pageNumber = $(this).data("page");
    var model = $(this).data("model");
    var url = $(this).data("url");
    var targetId = "divSearchResult";
    var targetIdTemp = $(this).attr("update-target-id");
    if (targetIdTemp !== "") {
        targetId = targetIdTemp;
    }
    model.PageIndex = pageNumber;
    aesSearch(url, model, targetId);
});

function aesSearch(url, model, updateTargetId) {
    $.ajax({
        url: url,
        type: "POST",
        data: { searchModel: model },
        success: function (res) {
            $(`#${updateTargetId}`).html(res);
        }
    });
}

//#region ajax
function ajaxTypePost(url, param = "") {
    return new Promise((resolve, reject) => {
        $.post(url, param, (response) => { resolve(response); })
            .fail((xhr, status, error) => {
                console.log(error);
                writeError(error);
                reject(error);
            });
    });

}

function ajaxTypeGet(url, param = "") {
    return new Promise((resolve, reject) => {
        $.get(url, param, (response) => { resolve(response); })
            .fail(function (xhr, status, error) {
                writeError(error);
                reject(error);
            });
    });
}

removeImage= function(preview) {
    $(preview).attr('src', $("#bkimgLoading").val());
}

ShowImagePreview = function(imageUpload, previewImage) {
    if (imageUpload.files && imageUpload.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewImage).attr('src', e.target.result);
        }
        reader.readAsDataURL(imageUpload.files[0]);
    }
}

