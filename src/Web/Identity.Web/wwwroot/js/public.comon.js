//SELECT CUSTOMIZE
$(".customize-select").each(function() {
    var classes = $(this).attr("class"),
        id = $(this).attr("id"),
        name = $(this).attr("name");
    var template = '<div class="' + classes + '">';
    template +=
        '<span class="customize-select-trigger">' +
        this.options[this.selectedIndex].text +
        "</span>";
    template += '<div class="customize-options">';
    $(this)
        .find("option")
        .each(function() {
            template +=
                '<span onclick="changLanguage(' +
                $(this).attr("value") +
                ')" class="' +
                id +
                " customize-option " +
                $(this).attr("class") +
                '" data-value="' +
                $(this).attr("value") +
                '">' +
                $(this).html() +
                "</span>";
        });
    template += "</div></div>";

    $(this).wrap(
        '<div class="customize-select-wrapper" id="' + id + '"></div>'
    );
    $(this).hide();
    $(this).after(template);
});

$(".customize-option:first-of-type").hover(
    function() {
        $(this)
            .parents(".customize-options")
            .addClass("option-hover");
    },
    function() {
        $(this)
            .parents(".customize-options")
            .removeClass("option-hover");
    }
);

$(".customize-select-trigger").on("click", function() {
    $("html").one("click", function() {
        $(".customize-select").removeClass("opened");
    });
    $(this)
        .parents(".customize-select")
        .toggleClass("opened");
    event.stopPropagation();
});

$(".customize-option").on("click", function() {
    $(this)
        .parents(".customize-select-wrapper")
        .find("select")
        .val($(this).data("value"));
    $(this)
        .parents(".customize-options")
        .find(".customize-option")
        .removeClass("selection");
    $(this).addClass("selection");
    $(this)
        .parents(".customize-select")
        .removeClass("opened");
    $(this)
        .parents(".customize-select")
        .find(".customize-select-trigger")
        .text($(this).text());
});
//END SELECT CUSTOMIZE

(function($) {
    "use strict";

    function InputNumber(element) {
        this.$el = $(element);
        this.$input = this.$el.find("[type=text]");
        this.$inc = this.$el.find("[data-increment]");
        this.$dec = this.$el.find("[data-decrement]");
        this.min = this.$el.attr("min") || false;
        this.max = this.$el.attr("max") || false;
        this.init();
    }

    InputNumber.prototype = {
        init: function() {
            this.$dec.on("click", $.proxy(this.decrement, this));
            this.$inc.on("click", $.proxy(this.increment, this));
            this.$input.on("input", $.proxy(this.write_step, this));
        },

        write_step: function() {
            $("#step").html(this.step());
        },

        step: function() {
            var value = this.$input[0].value;

            if (value <= 999) {
                return 1;
            } else {
                var length = value.toString().length;
                return (
                    Math.floor(value / Math.pow(10, length - 1)) *
                    Math.pow(10, length - 2)
                );
            }
        },

        increment: function(e) {
            var value = Number(this.$input[0].value);
            this.$input[0].value = value + this.step();
            this.write_step();
        },

        decrement: function(e) {
            var value = this.$input[0].value;
            var new_value = value - this.step();

            if (new_value < this.min) {
                new_value = 0;
            }

            this.$input[0].value = new_value;
            this.write_step();
        }
    };

    $.fn.inputNumber = function(option) {
        return this.each(function() {
            var $this = $(this),
                data = $this.data("inputNumber");

            if (!data) {
                $this.data("inputNumber", (data = new InputNumber(this)));
            }
        });
    };

    $.fn.inputNumber.Constructor = InputNumber;
})(jQuery);

$(".input-number-2").inputNumber();

//PAGINATION SCRIPT
$(document).on("click", ".page-link", function(e) {
    e.preventDefault();
    var pageNumber = $(this).data("page");
    var model = $(this).data("model");
    var url = $(this).data("url");
    var targetId = "divSearchResult";
    var targetIdTemp = $(this).attr("update-target-id");
    var method = $(this).attr("method");
    if (targetIdTemp !== "") {
        targetId = targetIdTemp;
    }
    model.PageIndex = pageNumber;
    aesSearch(url, model, targetId, method);
});

function aesSearch(url, model, updateTargetId, method) {
    $.ajax({
        url: url,
        type: "POST",
        data: { searchModel: model },
        success: function(res) {
            $(`#${updateTargetId}`).html(res);
        }
    });
}
//END PAGINATION SCRIPT

//SHOW SWEET ALERT
function showInfo(msg) {
    swal({
        title: msg,
        text: "",
        icon: "info",
        button: "Ok"
    });
}
function showSuccess(msg, msgButton) {
    swal({
        title: msg,
        text: "",
        icon: "success",
        button: msgButton
    });
}
function showError(msg, msgButton) {
    swal({
        title: msg,
        text: "",
        icon: "warning",
        button: msgButton
    });
}
function showConfirm(title, confirmmsg, cancelmsg, callback) {
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
    }).then(function(isConfirm) {
        if (isConfirm) {
            callback();
        }
    });
}
function showSuccessRedirect(title, messOK, url) {
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
    }).then(function(isConfirm) {
        if (isConfirm) {
            window.location.href = url;
        }
    });
}
//SHOW BOOTSTRAP NOTIFY
function showNotify(message, type) {
    $.notify(
        {
            message: message
        },
        {
            placement: {
                from: "bottom",
                align: "center"
            },
            type: type
        }
    );
}

//CREATE HTML TAG FOR SUBMIT FORM
function addFormControlParameter(
    frmName,
    controlType,
    controlName,
    controlValue
) {
    var input = $("<input>")
        .attr("type", controlType)
        .attr("name", controlName)
        .val(controlValue);
    $("#" + frmName).append(input);
}

function subMenuToggle() {
    $(".main-menu .sub-toggle").on("click", function(e) {
        e.preventDefault();
        var current = $(this).parent(".nav-item-has-children");
        $(this).toggleClass("active");
        current
            .siblings()
            .find(".sub-toggle")
            .removeClass("active");
        current.children(".nav-list-subitem").slideToggle('fast');
        current
            .siblings('.nav-item-has-children')
            .find(".nav-list-subitem")
            .slideUp('fast');
    });
}

$(function() {
    subMenuToggle();
});
