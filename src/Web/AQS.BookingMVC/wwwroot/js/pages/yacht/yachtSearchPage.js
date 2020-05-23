var yachtComponent = yachtComponent || (function (window, $) {
    "use strict";
    //varible

    let urlObj = {
        SEARCH_Yacht: "/Yacht/YachtSearch/YachtSearchResult",
        SEARCH_Yacht_Similar: "/Yacht/YachtSearch/YachtSearchSimilarResult",
        GET_LOCATION: "/Yacht/YachtLocation/GetListDistrictPostalByCountry",
        YACHT_SEARCH: "/yacht/search"
    }
    let ddlLocation = $("#ddlLocation");
    let idYachtDateRange = "#txtYachtDateRange";
    let ddlSearchYachtType = $("#ddlSearchYachtType");
    let txtSearchYachtName = $("#txtSearchYachtName");
    let txtPassengers = $("#txtPassengers");
    let hTotalRecord = $("#hTotalRecord");
    let $YachtPagination = null;
    let currentPageNumber = 1;
    let totalPages = 0;
    let isBusy = false;
    let defaultParams = null;
    function initPaging(newTotalPages) {
        var defaultOpts = {
            totalPages: newTotalPages
        };
        if (!$YachtPagination) {
            $YachtPagination = $('#divPaging');
            $YachtPagination.twbsPagination(defaultOpts);
        }
        else {
            let currentPage = $YachtPagination.twbsPagination('getCurrentPage');
            $YachtPagination.twbsPagination('destroy');
            $YachtPagination.twbsPagination($.extend({}, defaultOpts, {
                startPage: currentPage,
                totalPages: newTotalPages
            }));
        }

    }


    function checkViewMore(totalItems) {
        if (totalItems === 0
            || totalPages === 0
            || currentPageNumber === totalPages)
            $("#btnViewMore").hide();
        else {
            $("#btnViewMore").show();
        }
    }

    function requestYachtData(params, onRequestSuccess) {
        if (isBusy) {
            console.log("isBusy");
            return;
        }
        aqs.ajaxCall.setParams(urlObj.SEARCH_Yacht, "GET", params)
            .set({
                beforeSend: function () {
                    aqs.loadingHelper.show();
                    isBusy = true;

                },
                complete: function () {
                    aqs.loadingHelper.hide();
                    isBusy = false;

                },
                error: function () {
                    isBusy = false;
                }
            })
            .execute((response) => {
                window.history.pushState({}, null, response.url);
                onRequestSuccess(response.model);
            })
    }
    function loadLocation() {

        let country = defaultParams.country.toUpperCase();
        function processLocationResponse(response) {
            ddlLocation.append($("<option/>", { selected: true }).text("All " + country).val(" "));
            $.each(response.listCity, function (index, city) {
                ddlLocation.append($("<option />", { selected: defaultParams.city === city.value }).text(city.text).val(city.text));
                let listPort = response.portLocations.filter(x => x.cityName === city.text);
                listPort.map(x => {
                    ddlLocation.append($("<option />", { city: city.value, selected: defaultParams.portFID === x.id }).text(city.text + " - " + x.pickupPointName).val(x.id));
                });
            })
        }
        aqs.ajaxCall
            .get(urlObj.GET_LOCATION, { country: country })
            .execute(function (response) {
                processLocationResponse(response)
            });

    }
    function processResponse(response) {
        totalPages = response.totalPages;
        checkViewMore(response.totalItems);

        hTotalRecord.html(response.totalItems + " offer(s)");
        if (response.data.length > 0) {
            let html = "";
            for (let i = 0; i < response.data.length; i++) {
                html += renderYachtItem(response.data[i]);
            }
            $("#div-list-data").append(html);
        }


    }

    function renderYachtItem(item) {
        return ` <div class="col-12 col-md-4">
            <a href=${item.customProperties.DetailLink}>
              <div class="article-product-wrap">
                <div class="article-product">
                  <div class="img-block">
                    <img src="${item.customProperties.FileThumbUrl}" alt="${item.name}">
                    <h3 class="title">${item.name}</h3>
                  </div>
                  <div class="content">
                    <div class="left"> 
                      <p class="type">${item.engineGenerators || ""}</p>
                      <p class="text">${item.brandName || ""}</p>
                      <ul class="info-wrap">
                        <li class="info"><img src="/images/icons/feet.png" alt="icon">${item.lengthMeters} Metter,</li>
                        <li class="info"><img src="/images/icons/guests.png" alt="icon">${item.maxPassenger} guests,</li>
                        <li class="info"><img src="/images/icons/cabins.png" alt="icon"> ${item.cabins} cabins,</li>
                        <li class="info"><img src="/images/icons/heads.png" alt="icon">3 heads,</li>
                        <li class="info"><img src="/images/icons/captained.png" alt="icon">Captained</li>
                      </ul>
                    </div>
                    <div class="right"><span>from</span>
                      <p class="price">${item.customProperties.PriceFromText || "Contact us"}</p><span>${item.customProperties.PricingType}</span>
                    </div><a target='_blank' class="btn-more" href="${item.customProperties.DetailLink}"><span>More details</span><svg width="36" height="25" viewBox="0 0 36 25" fill="none" xmlns="http://www.w3.org/2000/svg">
<path d="M35.7804 12.3587L24.5377 1.1161C24.3225 0.902344 24.0026 0.837207 23.7208 0.953568C23.4405 1.06923 23.2583 1.34299 23.2583 1.64598V3.14497C23.2583 3.34404 23.3373 3.53439 23.4779 3.67492L31.1926 11.3896H0.772971C0.358679 11.3897 0.0234375 11.7249 0.0234375 12.1392V13.6382C0.0234375 14.0525 0.358679 14.3877 0.772971 14.3877H31.1926L23.4779 22.1024C23.3373 22.2429 23.2583 22.4332 23.2583 22.6324V24.1314C23.2583 24.4344 23.4406 24.7081 23.7208 24.8238C23.8138 24.8625 23.9111 24.8809 24.0077 24.8809C24.2025 24.8809 24.3942 24.8048 24.5377 24.6613L35.7804 13.4186C36.0732 13.1258 36.0732 12.6515 35.7804 12.3587Z" fill="white"/>
</svg></a>
                  </div>
                </div>
              </div>
</a>
            </div>`;
    }
    function searchYacht(data) {
        if (data.inDetailPage) {
            let queryParams = $.param(data);
            window.location.href = `${urlObj.YACHT_SEARCH}?${queryParams}`;
        }
        else {
            requestYachtData(data, (response) => {
                if (data.pageIndex === 1) {
                    $("#div-list-data").empty();
                    if (response.totalItems === 0) {

                        $("#div-list-data").html("<div class='h4 col-md-12 text-center'>0 offer is available in your search criteria</div>")
                    }

                }
                processResponse(response);
            });
        }

    }
    function frmSearchYachtSubmit(pageNumber = 1) {
        let selectedDate = aqs.commonFunc.getDateFromDateRange(idYachtDateRange);
        let data = $.extend({}, defaultParams, {
            yachtTypeFID: ddlSearchYachtType.val(),
            yachtName: txtSearchYachtName.val(),
            checkIn: selectedDate.minDate,
            checkOut: selectedDate.maxDate,
            passengers: txtPassengers.val(),
            pageIndex: pageNumber,
            pageSize: 15
        });
        let selectedPort = ddlLocation.find("option:selected");
        if (selectedPort.val().trim() !== "") {
            if (selectedPort.attr("city")) {
                data.portFID = selectedPort.val();
                data.city = selectedPort.attr("city");
            }
            else {
                data.city = selectedPort.val();
                data.portFID = null;
            }
        }
        else {
            data.country = defaultParams.country;
            data.city = null;
            data.portFID = null;
        }
        searchYacht(data)
    }
    function searchYachtOnLoadPage() {
        if (defaultParams.country) {
            searchYacht(defaultParams);
        }
    }
    // function
    function initSearchBox(defaultSearchParams) {
        defaultParams = defaultSearchParams || {};
        ddlLocation.select2({
            placeholder: "Select location",
            dropdownParent: $('#frmSearchYacht')
        })
        loadLocation();
        aqs.commonFunc.setDateRangePicker(idYachtDateRange, {
            minDate: new Date()
        });
        $("#frmSearchYacht").submit(function (e) {
            e.preventDefault();
            frmSearchYachtSubmit();

        })
    }
    function initSearchYacht() {
        //initPaging(1);       
        //searchYacht();
        //eveny binding
        $("#ddlSearchYachtType").change(function () {
            frmSearchYachtSubmit();
        });
        $("#txtSearchYachtName").blur(function () {
            if ($(this).val() !== "")
                frmSearchYachtSubmit();
        })
        $("#btnViewMore").click(function () {
            currentPageNumber++;
            frmSearchYachtSubmit(currentPageNumber);

        })
        //load yatch when load page
        searchYachtOnLoadPage();
    }
    function initYachtSimilar(divRender, configs) {

        let parammeters = $.extend({

        }, configs);
        aqs.ajaxCall.setParams(urlObj.SEARCH_Yacht_Similar, "GET", parammeters)
            .set({
                beforeSend: function () {
                    aqs.loadingHelper.show();
                    isBusy = true;

                },
                complete: function () {
                    aqs.loadingHelper.hide();
                    isBusy = false;

                },
                error: function () {
                    isBusy = false;
                }
            })
            .execute((response) => {
                if (response.length === 0)
                    $(".block-similar").hide();
                else {
                    let $elRender = $(divRender);
                    response.forEach((value) => {
                        $elRender.append(renderYachtItem(value));
                    })
                }
            })
    }

    // init
    return {
        initSearchBox,
        initSearchYacht,
        initYachtSimilar
    }
})(window, jQuery);
