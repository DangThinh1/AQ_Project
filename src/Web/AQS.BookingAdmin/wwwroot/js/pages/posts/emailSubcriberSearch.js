
(function (window, $, emailSubcriberSearchConfig) {
    "use strict"
    function searchEmailSubcriber(pageIndex) {
        let params = $("#frmSearchEmailSubcriber").serializeJSON();
        params.PageIndex = pageIndex;
        params.PageSize = DEFAULT_PAGE_SIZE;
        aqs.ajaxCall
            .get(emailSubcriberSearchConfig.urlObj.Search, params)
            .execute((response) => {
                if (response !== null) {
                    $("#divTable").html(response);
                    ShowPages(true);
                    setVisibleExport();
                }
                
            });
    }

    function setVisibleExport() {
        if ($("#lstCountSub").val() > 0)
            $("#btnExport").removeAttr("disabled");
        else
            $("#btnExport").attr('disabled', true);
    }

    function initSearch() {
        aqs.commonFunc.setDatePicker("#CreatedDateFrom", {
            autoUpdateInput: false,
            singleDatePicker: true,
            showDropdowns: true,
            minYear: 2019,
            maxYear: parseInt(moment().format('YYYY'), 2)
        }, (start, end, label) => {
            $('#CreatedDateTo').data('daterangepicker').minDate = start;

        })
        aqs.commonFunc.setDatePicker("#CreatedDateTo", {
            autoUpdateInput: false,
            singleDatePicker: true,
            showDropdowns: true,
            minYear: 2019,
            maxYear: parseInt(moment().format('YYYY'), 2)
        })
        searchEmailSubcriber();
        $("#frmSearchEmailSubcriber").submit(function (e) {
            e.preventDefault();
            searchEmailSubcriber();
        })
    };

    function ShowPages(update) {
        let totalPage = $("#hdTotalPage").val();
        if (update) {
            var $pagination = $('#divPagging');
            var currentPage = $pagination.twbsPagination('getCurrentPage');
            $('#divPage').html('');
            $('#divPage').html('<div id="divPagging"></div>');
            renderPagination(currentPage, totalPage);
        }
        else {
            renderPagination(1, totalPage);
        }

    };
    function renderPagination(currentPage, totalPages) {
        $('#divPagging').twbsPagination({
            startPage: currentPage,
            totalPages: totalPages,
            initiateStartPageClick: false,
            onPageClick: function (event, page) {
                searchEmailSubcriber(page);
            }
        });
    };
    function ExportExcelFunction() {
        let DateFrom = $("#CreatedDateFrom").val();
        let DateTo = $("#CreatedDateTo").val();
        if (DateFrom !== "" && DateTo !== "") {
            let DateFromStr = moment(DateFrom, 'MM/DD/YYYY').format("YYYY-MM-DD");
            let DateToStr = moment(DateTo, 'MM/DD/YYYY').format("YYYY-MM-DD");
            $.ajax({
                type: "GET",
                url: emailSubcriberSearchConfig.urlObj.Export,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: { DateFrom: DateFromStr, DateTo: DateToStr }
            }).done(function (data) {
                //get the file name for download
                if (data.fileName != "") {
                    //use window.location.href for redirect to download action for download the file
                    //window.location.href = "@Url.RouteUrl(New With {.Controller = 'EmailSubcriber', .Action = 'DownloadFileExcel'})/fileName=" + data.fileName;
                    window.location.href = emailSubcriberSearchConfig.urlObj.Download + "?fileName=" + data.fileName + "&dateFrom=" + DateFromStr + "&dateTo=" + DateToStr;
                }
                else
                    aqs.notifyHelper.showError("No data to export excel! Please try again later.");
            });
        }
        else
            aqs.notifyHelper.showError("Please fill in From Date and To Date");
    };

    $(document).ready(() => {
        initSearch();
        $(document).on("click", "#btnExport", function () {
            ExportExcelFunction();
        });
    })

})(this, jQuery, emailSubcriberSearchConfig)