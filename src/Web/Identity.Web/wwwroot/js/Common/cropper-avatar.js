$(document).ready(function () {
    var Cropper = window.Cropper;
    var URL = window.URL || window.webkitURL;
    var modal = $("#modal");
    var options = {
        aspectRatio: 1,
        zoom: function (e) {
            console.log(e.type, e.detail.ratio);
        }
    };

    var inputImage = $("#inputImage")[0];

    $(document).on("change", "#inputImage", function () {
        var files = this.files;
        var done = function (url) {
            inputImage.value = '';
            image.src = url;
            modal.modal('show');
        };

        var reader;
        var file;
        var url;

        if (files && files.length > 0) {
            file = files[0];

            if (URL) {
                done(URL.createObjectURL(file));
            } else if (FileReader) {
                reader = new FileReader();
                reader.onload = function (e) {
                    done(reader.result);
                };
                reader.readAsDataURL(file);
            }
        }
    });

    modal.on('shown.bs.modal', function () {
        cropper = new Cropper(image, options);
    }).on('hidden.bs.modal', function () {
        cropper.destroy();
        cropper = null;
    });

    $(document).on("click", "#btnCrop", function () {
        var initialAvatarURL;
        var canvas;

        modal.modal('hide');

        if (cropper) {
            canvas = cropper.getCroppedCanvas({
                minWidth: 32,
                minHeight: 32,
                maxWidth: 128,
                maxHeight: 128,
                fillColor: '#fff'
            });
            initialAvatarURL = targetImage.src;
            targetImage.src = canvas.toDataURL('image/jpeg', 0.9);
        }
    });

    $(document).on("change", "[name='aspectRatio']", function (e) {
        e.preventDefault();
        var value = $(this).val();
        options.aspectRatio = value;
        cropper.destroy();
        cropper = new Cropper(image, options);
    });
});