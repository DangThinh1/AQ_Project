//FIELD


//FUNCTIONAL
$(document).ready(function () {
    LoadDiningDish();
});

$(document).on("click", "#btnDeleteMenuCart", function (e) {
    let id = $(this).data("idmenu");
    let idParams = [];
    let inputChckBox = $("input[name='chckItem']");

    if (inputChckBox.length > 0) {
        jQuery.each(inputChckBox, function (i, val) {
            if (inputChckBox[i].checked)
                idParams.push(inputChckBox[i].value);
        });
    }
    if (idParams.length == 0) {
        showNotify(messNotify.chooseDishDelete, 'info');
        return;
    }

    showConfirm(messNotify.confirm, messNotify.confirmOkay, messNotify.confirmCancel, function () {
        $.post(DinningDishURLAction.DeleteMenuDishURL, { lstIdMenu: idParams }, (data) => {
            if (data !== null) {
                showNotify(messNotify.deleteSuccess, 'success');
                LoadDiningDish();
            }
            else
                showNotify(messNotify.deleteFail, 'danger');
        });

    });
});

$(document).on("change", "#chckboxParent", function (e) {
    if ($(this).prop("checked")) $("input[name='chckItem']").prop("checked", true);
    else $("input[name='chckItem']").prop("checked", false);
});

function LoadDiningDish() {
    $.get(DinningDishURLAction.LoadDiningDishURL, null, (data) => {
        if (data !== null) {
            $("#dishcartDisplay").html();
            $("#dishcartDisplay").html(data);
            CountTotalMenu();
            LoadQuantityJS();
        }
    });
}

function CountTotalMenu() {
    const formater = new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD', minimumFractionDigits: 2 });
    let DivPrice = $("#divPrice").find("div[name='dvPriceTotal']");
    let totalCart = 0;
    if (DivPrice.length > 0) {
        DivPrice.each(function (i) {
            totalCart += parseFloat(DivPrice[i].getAttribute("data-totalcart"));
        });
    }
    $("#totalMenuCart span").html(formater.format(totalCart.toFixed(2)));
}

function BookMenufn() {
    LoadQuantityJS();
    let restaurantId = $("#RestaurantId").val();
    let txtQuantity = $("input[name='txtQuantity']");
    let arrMenuCart = {};
    let MenuCartlst = [];
    if (txtQuantity.length > 0) {
        jQuery.each(txtQuantity, function (i, val) {
            if (parseInt(val.value) > 0) {
                MenuCartlst.push({
                    IdMenu: val.getAttribute("data-itemName"),
                    Name: val.getAttribute("data-name"),
                    Quantity: val.value,
                    Price: val.getAttribute("data-price")
                });
            }
        });
        arrMenuCart = {
            MenuCartlst: MenuCartlst,
            RestaurantID: restaurantId,
        }
    }

    if (!jQuery.isEmptyObject(arrMenuCart.MenuCartlst)) {
        $.post(DinningDishURLAction.CheckingMenuCartURL, { arrQuantity: arrMenuCart }, (data) => {
            if (data.success !== false) {
                showNotify(messNotify.notifyBookingDifferentRestaurant, 'warning');
                LoadDiningDish();
            }
            else {
                $.post(DinningDishURLAction.ChangeBookMenuURL, { arrQuantity: arrMenuCart }, (data) => {
                    if (data != null) {
                        showNotify(messNotify.bookSuccess, 'success');
                        $("#dishcartDisplay").html();
                        $("#dishcartDisplay").html(data);
                        CountTotalMenu();
                        LoadQuantityJS();
                    }
                });
            }
        });
    }
    else
        showNotify(messNotify.bookFail, 'danger');    
}

$(document).on("change", "input[name='txtQuantityCart']", function () {
    ChangeMenuCart();
});

function ChangeMenuCart() {
    let restaurantId = $("#RestaurantId").val();
    let txtQuantity = $("input[name='txtQuantityCart']");
    let arrMenuCart = {};
    let MenuCartlst = [];
    if (txtQuantity.length > 0) {
        jQuery.each(txtQuantity, function (i, val) {
            if (parseInt(val.value) > 0) {
                MenuCartlst.push({
                    IdMenu: val.getAttribute("data-itemName"),
                    Name: val.getAttribute("data-name"),
                    Quantity: val.value,
                    Price: val.getAttribute("data-price")
                });
            }
        });
        arrMenuCart = {
            MenuCartlst: MenuCartlst,
            RestaurantID: restaurantId,
        }
    }

    if (!jQuery.isEmptyObject(arrMenuCart)) {
        $.post(DinningDishURLAction.ChangeBookMenuURL, { arrQuantity: arrMenuCart }, (data) => {
            if (data != null) {
                $("#dishcartDisplay").html();
                $("#dishcartDisplay").html(data);
                CountTotalMenu();
                LoadQuantityJS();
            }
        });
    }
}