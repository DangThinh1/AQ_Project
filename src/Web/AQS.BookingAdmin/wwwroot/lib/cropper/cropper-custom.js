$(document).ready(function () {
    //Fields 
    var image = document.getElementById("imageDrop");
    var Cropper = window.Cropper;
    var URL = window.URL || window.webkitURL;
    var options = {
        aspectRatio: NaN,
        zoom: function (e) {
            console.log(e.type, e.detail.ratio);
        },
        autoCropArea: 1
    };

    var cropper = new Cropper(image, options);
    var originalImageURL = image.src;
    var uploadedImageType = 'image/jpeg';
    var uploadedImageName = 'cropped.jpg';
    var uploadedImageURL;

    $("#groupCropper").hide();

    var inputImage = $("#inputImage")[0];

    //Methods
    $(document).on("change", "#inputImage", function () {
        var files = this.files;
        var file;     
        $("#groupCropper").show();

        if (cropper && files && files.length) {
            file = files[0];

            if (/^image\/\w+/.test(file.type)) {
                uploadedImageType = file.type;
                uploadedImageName = file.name;

                if (uploadedImageURL) {
                    URL.revokeObjectURL(uploadedImageURL);
                }

                //Add cropper
                image.src = uploadedImageURL = URL.createObjectURL(file);
                //cropper = new Cropper(image, options);
                $("#lblImage").trigger("click");
            } else {
                window.alert('Please choose an image file.');
            }
        }
    });
     
    $(document).on("click", "#btnCrop", function () {
        var initialAvatarURL;
        var canvas;

        if (cropper) {
            canvas = cropper.getCroppedCanvas({
                minWidth: 256,
                minHeight: 256,
                maxWidth: 2048,
                maxHeight: 2048,
                fillColor: '#fff'
            });
            initialAvatarURL = image.src;
            image.src = canvas.toDataURL('image/jpeg', 0.9);
            cropper.destroy();
        }
    });

    $(document).on("click", "#setDragMode", function () {
        options.dragMode = 'move';
        cropper.destroy();
        cropper = new Cropper(image, options);
    });

    $(document).on("change", "[name='aspectRatio']", function (e) {
        e.preventDefault();
        var value = $(this).val();
        $("#groupRatio label").removeClass();
        $("#groupRatio label").addClass("btn btn-outline-primary");
        $(this).parent().removeClass();
        $(this).parent().addClass('btn btn-primary');
        options.aspectRatio = value;
        cropper.destroy();
        cropper = new Cropper(image, options);
    });

    $("#btnCancel").click(function () {
        cropper.destroy();
    });
});