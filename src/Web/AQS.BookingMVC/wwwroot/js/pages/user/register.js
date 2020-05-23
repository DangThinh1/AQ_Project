//VARIABLE FIELD






//FUNCTION
$(document).ready(() => {
});


$(document).on("click", "#registerSubmit", function (e) {
    e.preventDefault();
    submitRegister();
});


function submitRegister() {
    var objFrm = $('#frmRegister').serializeArray();
    var data = {};
    if (objFrm.length > 0) {
        $.map(objFrm, (i, v) => {
            data[i['name']] = i['value'];
        });
        $.post(HomeURLObj.registerURL, { model: data }, function (res) {
            if (!res.isSuccessStatusCode)
                alert(res.message);
            window.location.reload();
        });
        
    }
    else
        return;
}