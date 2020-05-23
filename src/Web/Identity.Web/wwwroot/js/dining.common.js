
$(document).ready(function () {
    // $("#navbarSupportedContent ul").find("li").eq(2).addClass("active");
    //SetHeightBanner();
    if($("#smartwizard").length) {
        $('#smartwizard').smartWizard({
            useURLhash: false,
            showStepURLhash: false,
            theme: 'circles',
            selected: 0,
            lang: {
                next: 'Next steps',
                previous: '↼ Back to first step'
            },
        });

        
        $("#smartwizard").on("showStep", function(e, anchorObject, stepNumber, stepDirection) {
            if($('button.sw-btn-next').hasClass('disabled')){
                $('.sw-toolbar-bottom').addClass('last-step');
            }
        });
    }

  
});

// function SetHeightBanner() {
//     var screenw = $(window).width();
//     var screenh = $(window).height();
//     $(".bg-banner").css("height", (screenw / (1920 / 450)) + "px");
// }

jQuery('<div class="quantity-nav"><div class="quantity-button quantity-up"><i class="fa fa-caret-up" aria-hidden="true"></i></div><div class="quantity-button quantity-down"><i class="fa fa-caret-down" aria-hidden="true"></i></div></div>').insertAfter('.quantity input');
        jQuery('.quantity').each(function () {
    var spinner = jQuery(this),
        input = spinner.find('input[type="number"]'),
        btnUp = spinner.find('.quantity-up'),
        btnDown = spinner.find('.quantity-down'),
        min = input.attr('min'),
        max = input.attr('max');

    btnUp.click(function () {
        var oldValue = parseFloat(input.val());
        if (oldValue >= max) {
            var newVal = oldValue;
        } else {
            var newVal = oldValue + 1;
        }
        spinner.find("input").val(newVal);
        spinner.find("input").trigger("change");
    });

    btnDown.click(function () {
        var oldValue = parseFloat(input.val());
        if (oldValue <= min) {
            var newVal = oldValue;
        } else {
            var newVal = oldValue - 1;
        }
        spinner.find("input").val(newVal);
        spinner.find("input").trigger("change");
    });

});

function LoadQuantityJS() {
    jQuery('<div class="quantity-nav"><div class="quantity-button quantity-up"><i class="fa fa-caret-up" aria-hidden="true"></i></div><div class="quantity-button quantity-down"><i class="fa fa-caret-down" aria-hidden="true"></i></div></div>').insertAfter('.quantity input');
    jQuery('.quantity').each(function () {
        var spinner = jQuery(this),
            input = spinner.find('input[type="number"]'),
            btnUp = spinner.find('.quantity-up'),
            btnDown = spinner.find('.quantity-down'),
            min = input.attr('min'),
            max = input.attr('max');

        btnUp.click(function () {
            var oldValue = parseFloat(input.val());
            if (oldValue >= max) {
                var newVal = oldValue;
            } else {
                var newVal = oldValue + 1;
            }
            spinner.find("input").val(newVal);
            spinner.find("input").trigger("change");
        });

        btnDown.click(function () {
            var oldValue = parseFloat(input.val());
            if (oldValue <= min) {
                var newVal = oldValue;
            } else {
                var newVal = oldValue - 1;
            }
            spinner.find("input").val(newVal);
            spinner.find("input").trigger("change");
        });

    });
}