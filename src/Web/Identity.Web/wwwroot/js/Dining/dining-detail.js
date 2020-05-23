//FIELD



//FUNCTIONAL
$(document).ready(function () {
    LoadDiningRelated();
});

function BookMenufn() {
    let restaurantId = $("#RestaurantId").val();
    let txtQuantity = $("input[name='txtQuantity']");
    let arrMenuCart = {};
    let MenuCartlst = [];
    let valReservationFee = $("#PriceReservationFee").data("reserfee");
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
    if (valReservationFee !== null && valReservationFee > 0 && MenuCartlst.length == 0) {
        MenuCartlst.push({
            Name: "Reservation Fee",
            Quantity: 1,
            Price: valReservationFee,
        })
        arrMenuCart = {
            MenuCartlst: MenuCartlst,
            RestaurantID: restaurantId,
        };
    }
    if (!jQuery.isEmptyObject(arrMenuCart)) {
        $.post(DinningURLAction.CheckingMenuCartURL, { arrQuantity: arrMenuCart }, (data) => {
            if (data !== null)
                if (data.success !== false)
                    showConfirm(messNotify.messTitle, messNotify.messOK, messNotify.messCancel, function () {
                        $.post(DinningURLAction.BookMenuURL, { arrQuantity: arrMenuCart }, (data) => {
                            if (data !== null) {
                                showSuccessRedirect(messNotify.messBookSuccess, messNotify.messOK, "DiningDish");
                            }
                        });
                    });
                else
                    $.post(DinningURLAction.BookMenuURL, { arrQuantity: arrMenuCart }, (data) => {
                        if (data !== null) {
                            showSuccessRedirect(messNotify.messBookSuccess, messNotify.messOK, "DiningDish");
                        }
                    });
        });
    }
    else
        showNotify(messNotify.messBookFailed, 'danger');        
}

function LoadDiningRelated() {
    let restaurantId = $("#RestaurantId").val();
    $.get(DinningURLAction.LoadDiningRelatedURL, { restaurantId: restaurantId }, (data) => {
        if (data !== null)
            $("#lstDiningRelated").append(data);
        LoadQuantityJS();
    });
}

