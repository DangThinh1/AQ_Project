//FIELD


//FUNCTION
//$(document).ready(function () {
//    LoadTheme();
//});

$(function () {
    LoadBanner();
    LoadTheme();

});


function LoadTheme() {
    var searchModel = {
        MerchantId: $("#ThemeParamId").val(),
        PageIndex: 1,
        PageSize: 10,
    }
    $.post(YachtLandingURL.ChangeThemeURL, { searchModel: searchModel }, (data, status, xhr) => {
        if (data !== null)
            $("#dvDisplayData").html(data);
    });
}

function LoadBanner() {
    let merchant = $("#MerchantName").val();
    let code = $("#Code").val();


    $.get(YachtLandingURL.LoadBannerLanding, { merchant: merchant, code: code }, (data, status, xhr) => {
        if (data !== null)
            $("#dvBannerDisplay").html(data);
    });
}