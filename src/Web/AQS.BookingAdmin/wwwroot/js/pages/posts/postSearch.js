(function (window, $, postSearchConfig) {
    "use strict"

    function searchPost(pageIndex) {
        let params = $("#frmSearchBlog").serializeJSON();
        params.PageIndex = pageIndex;
        params.PageSize = DEFAULT_PAGE_SIZE;
        params.IsDeleted = $('#IsDeleted').prop('checked');
        aqs.ajaxCall
            .get(postSearchConfig.urlObj.SearchPostUrl, params)
            .execute((response) => {
                $("#divPostTable").html(response);              
                ShowPages(true);
            });
    }
    function deletePost(id) {
        aqs.alertHelper.showConfirm("Are you sure delete this post?", "Ok", "Cancel", function () {
            aqs.ajaxCall.delete(postSearchConfig.urlObj.DeletePostUrl, {id})
                .execute((response) => {
                    if (response.id > 0) {
                        aqs.notifyHelper.showSuccess("Post has been deleted!");
                        searchPost();
                    }
                    else {
                        aqs.notifyHelper.showError("Post deleted was not successful");
                    }
                });
        })
    }

    function restorePost(id) {
        aqs.alertHelper.showConfirm("Are you sure restore this post?", "Ok", "Cancel", function () {
            aqs.ajaxCall.put(postSearchConfig.urlObj.RestorePostUrl, { id })
                .execute((response) => {
                    if (response.id > 0) {
                        aqs.notifyHelper.showSuccess("Post has been restore!");
                        searchPost();
                    }
                    else {
                        aqs.notifyHelper.showError("Post restore was not successful");
                    }
                });
        })
    }

    function initPostSearch() {
        searchPost();
        $("#frmSearchBlog").submit(function (e) {
            e.preventDefault();
            searchPost();
        })
        $(document).on("click", ".btn-delete-post", function () {
            let id = $(this).attr("data-post-id");
            deletePost(id);
        })
        $(document).on("click", ".btn-restore-post", function () {
            let id = $(this).attr("data-post-id");
            restorePost(id);
        })
    }

    function ShowPages(update) {
        let totalPage = $("#hdTotalPage").val();
        if (update) {
            var $pagination = $('#divPagging');
            var currentPage = $pagination.twbsPagination('getCurrentPage');
            $('#divPage').html('');
            $('#divPage').html('<div id="divPagging"></div>');
            renderPagination(currentPage, totalPage)
        }
        else {
            renderPagination(1, totalPage)
        }

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
    }

    $(document).ready(() => {
        $('#IsDeleted').bootstrapToggle('off');
        initPostSearch();
    })

})(this, jQuery, postSearchConfig)