
if (postConfig.PostId === 0) {
    window.onbeforeunload = function (e) {
        e = e || window.event;
        
        // For IE and Firefox prior to version 4
        if (e) {
            e.returnValue = 'Sure?';
        }
        // For Safari
        return 'Sure?';
    };
}
(function (window, $, postConfig) {
    "use strict";
    var fontByLanguageIds= {
        "1": "Avenir LT Std", //english
        "2": "ZCOOL XiaoWei",//chinese
        "3": "Sawarabi Mincho",//japanese
        "4": "Noto Sans Thai",//thai
        "5": "Montserrat",//vietnam
    }
    var $window = window;   

    $window.postblog = {};

    var $this = $window.postblog;
    var isBusy = false;
   
    function createUpdatePost(isPublished) {

        if (isBusy) {
            aqs.notifyHelper.showWarning("Saving post please wait a bit");
            return;
        }
        var resultValidFrm = $("#frmCreatePost").valid();
        if (!resultValidFrm)
            return;
        if ($("#PostDetail_FileStreamFid").val() === "0" &&
            $("#ThumbImgName").val() === ""
        ) {
            aqs.notifyHelper.showWarning("Please upload the thumbnail image");
            return;
        }
        var params = $("#frmCreatePost").serializeJSON();
        let postUrl = postConfig.PostId > 0
            ? postConfig.urlObj.UpdatePost
            : postConfig.urlObj.CreatePost;
        params["PostInfo.IsActivated"] = true;
        params["PostDetail.IsActivated"] = isPublished;
        params["PostDetail.FileDescriptionIds"] = $this.uploadedPostDetailImages.getIds();
        aqs.ajaxCall
            .set({
                beforeSend: function () {
                    isBusy = true;
                    window.onbeforeunload = function () {}
                },
                complete: function () {
                    isBusy = false;
                },
                error: function () {
                    isBusy = false;
                }
            })
            .post(postUrl, params)
            .execute((response) => {
                if (response.id > 0) {
                    aqs.notifyHelper.showSuccess(postConfig.PostId > 0 ? "Updated post successful" : "Created new post successful");    
                    setTimeout(function () {
                        window.location.href = postConfig.urlObj.CreateUpdatePost + response.id + "?languageId=" + params.LanguageId;
                    }, 1200)
                  
                }
                else {
                    aqs.notifyHelper.showError(postConfig.PostId > 0 ? "Updated post failed" : "Created new post failed");
                }
            }, (error) => {
                aqs.notifyHelper.showError(error.responseText);
            });
    }
    function getPostDetail(languageId) {
        if (postConfig.PostId > 0) {
            aqs.ajaxCall.get(postConfig.urlObj.GetPostDetail, { postId: postConfig.PostId, languageId: languageId })
                .execute((response) => {
                    $("#divMutipleLanguage").html(response);
                    initPostDetail();
                    
                })
        }
    }
    function loadCategory(id = 0) {
        aqs.ajaxCall.get(postConfig.urlObj.GetCategoryList)
            .execute((response) => {
                let ddlCategoryId = $("#PostInfo_PostCategoryFid");
                ddlCategoryId.html("<option value=''>Please select</option>");
                let selectedId = postConfig.CategoryId;
                if (id > 0)
                    selectedId = id;
                $.each(response, function (index, el) {
                    ddlCategoryId.append($("<option/>", { selected: selectedId.toString() === el.value }).text(el.text).val(el.value));
                })
            })
    }
    function initPostDetail() {
        aqs.commonFunc.initSummerNote("#txtDescription", {
            height: 500,
            callbacks: {
                onImageUpload: function (files) {
                    handleOnImageUploadSummerNode(files, handleOnUploadPostDetailImage);
                },
                onDialogShown: function (x) {
                    generateGalleryElement();
                }
            }
        })
        let languageId = $("#ddlLanguageId").val();
        let fontLanguage = fontByLanguageIds[languageId];
        if (fontLanguage) {
            $('#txtDescription').summernote('fontName', fontLanguage);
        }

    }
    function validFile(file) {
        if (!aqs.fileHelper.isImage(file.name)) {
            aqs.notifyHelper.showWarning("File is not valid image extensions JPG,PNG,JPEG,GIF");
            false;
        }
        if (!aqs.fileHelper.checkFileSize(file, 5)) {
            aqs.notifyHelper.showWarning("File size maximum is 5Mb allowed");
            return false;
        }
        return true;

    }
    function openPreview() {
        let postId = postConfig.PostId;
        let languageId = $("#ddlLanguageId").val();
        let url = postConfig.urlObj.PreviewUrl + "?id=" + postId + "&languageId=" + languageId;
        window.open(url, "_blank");
    }

    function initCreatePostPage() {
        initPostDetail();
        loadCategory();
        //$("#frmCreatePost").submit(function (e) {
        //    e.preventDefault();
        //    createUpdatePost();
        //})
        $(".btn-save-draft").click(function () {
            createUpdatePost(false);
        })
        $(".btn-save-publish").click(function () {
            createUpdatePost(true);
        })
        $("#ddlLanguageId").change(function () {
            var languageId = $(this).val();
            getPostDetail(languageId);
        })
        $(document).on("click", "#btnUploadImg", function () {
            $("#flUploadImage").click();
        })
        $(document).on("change", "#flUploadImage", function () {
            let files = $(this)[0].files;
            if (files.length > 0) {
                let file = files[0];
                if (validFile(file)) {
                    aqs.commonFunc.uploadImageTemp(file, function (imgObj) {
                        $("#ThumbImgName").val(imgObj.fileName);
                        $("#imgThumb").attr("src", imgObj.fileUrl);
                        $("#imgThumb").parent().attr("href", imgObj.fileUrl);
                    })
                }
            }
        })
        $(document).on('click', '[data-toggle="lightbox"]', function (event) {
            event.preventDefault();
            $(this).ekkoLightbox({
                alwaysShowClose: true
            });
        });
        $(".btn-preview").click(function () {
            openPreview();
        })
       
        
    }

    $(document).ready(() => {
        initCreatePostPage();
        postCategoryConfig.onSaveSuccess = function (id) {
            loadCategory(id);
        };
        $this.uploadedPostDetailImages = new UploadedImages();
    });

    var UploadedImages = function () { this.images = []; };
    UploadedImages.prototype.addImage = function (image) {
        this.images.push(image);
    }

    UploadedImages.prototype.getIds = function () {
        return this.images.map(x => x.id);
    }

    UploadedImages.prototype.getImages = function () {
        return this.images;
    }

    function handleOnImageUploadSummerNode(files, handleOnUploadImage) {
        for (var i = files.length - 1; i >= 0; i--) {
            if (aqs.fileHelper.checkFileSize(files[i], 10)) {
                aqs.commonFunc.uploadImage(files[i], handleOnUploadImage);
            }
        }
    }

    function handleOnUploadPostDetailImage(image) {
        $this.uploadedPostDetailImages.addImage(image);

        var uploadedImageNode = document.createElement('img');
        uploadedImageNode.src = image.url;
        uploadedImageNode.ondrag = function (e) { e.preventDefault(); }
        $('#txtDescription').summernote('insertNode', uploadedImageNode);
    }

    function generateGalleryElement() {
        //set size of modal
        $(".modal[aria-label='Insert Image'] .modal-dialog").addClass("modal-lg")
        var insertImageSummerNoteContent = $('.form-group.note-group-image-url')[0];
        if (insertImageSummerNoteContent) {
            var galleryNode = document.getElementById('existingPostDetailImages');
            if (!galleryNode) {
                galleryNode = document.createElement('div');
                galleryNode.id = 'existingPostDetailImages';
                galleryNode.style.marginTop = '5px';
                galleryNode.className = 'form-group';

                var titleNode = document.createElement('label');
                titleNode.innerText = 'Existing Images';
                titleNode.className = 'note-form-label';
                titleNode.style.marginTop = '15px';

                insertImageSummerNoteContent.appendChild(titleNode);
                insertImageSummerNoteContent.appendChild(galleryNode);
            }

            if (postConfig && postConfig.PostId > 0) {
                $.when(getExistingImagesOfPostDetail()).then(response => {
                    var images = [];
                    for (var res of response) {
                        var image = { id: res.item1, url: res.item2 };
                        images.push(image);
                    }
                    addImageToGallery(galleryNode, images);
                });
            }

            var images = $this.uploadedPostDetailImages.getImages();
            if (images) {
                addImageToGallery(galleryNode, images);
            }
        }
    }

    function getExistingImagesOfPostDetail() {
        return aqs.ajaxCall.get(postConfig.urlObj.GetFileStreamOfPostDetail, { postDetailId: postConfig.PostId })
            .executePromise();
    }

    function addImageToGallery(galleryNode, images) {
        images.forEach(i => {
            var aNode = document.getElementById(i.id);
            if (!aNode) {
                aNode = document.createElement('a');
                aNode.href = i.url;
                aNode.setAttribute('data-fancybox', 'images');

                var wrapNode = document.createElement('div');
                wrapNode.className = 'form-group';
                wrapNode.className = 'float-left';
                wrapNode.style.marginRight = '10px';

                var imageNode = document.createElement('img');
                imageNode.id = i.id;
                imageNode.src = i.url;
                imageNode.className = 'rounded';
                imageNode.style.height = '112px';
                var lazyInst = new LazyLoad({}, imageNode);
                wrapNode.appendChild(imageNode);

                var selectLnkNode = document.getElementById('s_' + i.id);
                if (!selectLnkNode) {
                    selectLnkNode = document.createElement('a');
                    selectLnkNode.id = 's_' + i.id;
                    selectLnkNode.href = i.url;
                    selectLnkNode.innerText = "Select";
                    selectLnkNode.onclick = function (e) {
                        e.preventDefault();
                        var urlNode = $('.note-image-url.form-control.note-form-control.note-input')[0];

                        if (urlNode) {
                            console.log(e.target);
                            urlNode.value = e.target.href;
                            var insertImageBtnNode = $('.btn.btn-primary.note-btn.note-btn-primary.note-image-btn')[0];
                            insertImageBtnNode.removeAttribute('disabled');
                            insertImageBtnNode.classList.remove('disabled');
                        }
                    }

                    wrapNode.appendChild(document.createElement('br'));
                    wrapNode.appendChild(selectLnkNode);
                }
                aNode.appendChild(wrapNode);
                galleryNode.appendChild(aNode);

                setTimeout(function () { initGallery(); }, 1000);
            }
        });
    }

    function initGallery() {
        $('[data-fancybox="images"]').fancybox({
            toolbar: false,
            loop: false,
            infobar: true,
            arrows: false
        });
    }
})(this, jQuery, postConfig);

