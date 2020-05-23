(function (window, $, postCategoryConfig) {
    "use strict"
    //region Field
    let btnCreateNew = "#btnCreateNew";
    let btnSave = "#btnSavePostCate";
    let frmAddUpdatePostCate = "#frmAddUpdatePostCate";
    let ddlLang = "#LanguageFid";
    let modalCreateUpdate = "#modal-postcategory";
    let panelLstPostCate = $("#panelLstPostCate");
    //endregion

    //Function
    function LoadPostCategory() {
        aqs.ajaxCall.get(postCategoryConfig.urlObj.GET_LIST_CATE, null)
            .execute((response) => {
                panelLstPostCate.html(response);
            });
    };
    function showModalCreateUpdate(id = 0, langFId = 0) {
        aqs.ajaxCall.get(postCategoryConfig.urlObj.GET_MODAL_CATE, { id: id, langFId: langFId })
            .execute((response) => {
                $(modalCreateUpdate).html(response);
                $(modalCreateUpdate).modal("show");
            });
    };
    function deleteCategory(id) {
        aqs.alertHelper.showConfirm("Are you sure detete this category?", "Yes sure", "Cancel", function () {
            aqs.ajaxCall.post(postCategoryConfig.urlObj.DELTE_CATE, { id: id })
                .execute((response) => {
                    if (!response)
                        aqs.notifyHelper.showError("Deteted category failed! Please try again later");
                    else {
                        aqs.notifyHelper.showSuccess("Deteted category successfuly");
                        LoadPostCategory();
                    }
                });
        });
    };

    function checkPostCategoryDuplicate() {
        var frmObj = $(frmAddUpdatePostCate).serializeArray();
        var model = {};
        $.map(frmObj, (i, v) => {
            model[i['name']] = i['value'];
        });
        let isActived = $("#IsActivated").prop("checked");
        model.IsActivated = isActived;
        model.ParentFid = $("#ParentFid").val() !== "" ? $("#ParentFid").val() : 0;
        model.PostCateDetailId = $("#PostCateDetailId").val();
        if (model.PostCateDetailId != 0) SavePostCategory(model);
        else {
            checkPostCateDuplicate(model, function (response) {
                if (!response) {
                    SavePostCategory(model);
                }
                else
                    aqs.notifyHelper.showError("Can not add duplicate post category detail ");
            });
        }
    };

    function SavePostCategory(model) {
        aqs.ajaxCall.post(postCategoryConfig.urlObj.POST_CATE, { model: model })
            .execute((response) => {
                if (response > 0) {
                    aqs.notifyHelper.showSuccess("Saved category successfuly");
                    $(modalCreateUpdate).modal("hide");
                    if (postCategoryConfig.onSaveSuccess) {
                        postCategoryConfig.onSaveSuccess(response);
                    }
                    else
                        LoadPostCategory();
                }
                else
                    aqs.notifyHelper.showError("Saved category failed! Please try again later");
            });
    }


    function checkPostCateDuplicate(model, onCheckSuccess) {
        aqs.ajaxCall.post(postCategoryConfig.urlObj.CHECK_DUPLICATE, { model: model }) //PostCateDetailId
            .execute((response) => {
                onCheckSuccess(response);
            });
    }
    function renderPagination(currentPage, totalPages) {
        $('#divPagging').twbsPagination({
            startPage: currentPage,
            totalPages: totalPages,
            initiateStartPageClick: false,
            onPageClick: function (event, page) {
                searchPost(page);
            }
        });
    };
    function selectBoxChanged(id = 0, langFId = 0) {
        $("#PostCateDetailId").val("");
        $("#Name").val("");

        aqs.ajaxCall.get(postCategoryConfig.urlObj.LOAD_CHANGED_LANGFID_CATE, { id: id, langFId: langFId })
            .execute((response) => {
                if (response !== "") {
                    var str = response.split('-');
                    $("#Name").val(str[0]);
                    $("#DefaultName").val(str[1]);
                    $("#PostCateDetailId").val(str[2]);
                }
            });
    }
    function initPostCategory() {
        $(document).on("click", btnCreateNew, function () {
            showModalCreateUpdate();
        });
        $(document).on("click", "button.btn-edit-category", function () {
            let id = $(this).attr("data-id");
            let langFId = $(this).attr("data-langFId");
            showModalCreateUpdate(id, langFId);
        });
        $(document).on("click", "button.btn-delete-category", function () {
            let id = $(this).attr("data-id");
            deleteCategory(id);
        });
        $(document).on("click", btnSave, function (e) {
            e.preventDefault();
            var resultValidFrm = $(frmAddUpdatePostCate).valid();
            if (!resultValidFrm) return;
            checkPostCategoryDuplicate();
        });
        $(document).on("change", ddlLang, function (e) {
            e.preventDefault();
            let id = $("#Id").val();
            let langFId = $(ddlLang).val();
            if (id > 0)
                selectBoxChanged(id, langFId);
        });
    };
    $(document).ready(() => {
        initPostCategory();
        if (!postCategoryConfig.isPostPage)
            LoadPostCategory();
    });
})(this, jQuery, postCategoryConfig)