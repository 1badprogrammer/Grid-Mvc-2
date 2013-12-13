(function ($) {
    var plugin = GridMvc.prototype;

    // store copies of the original plugin functions before overwriting
    var functions = {};
    for (var i in plugin) {
        if (typeof (plugin[i]) === 'function') {
            functions[i] = plugin[i];
        }
    }
    

    // extend existing functionality of the gridmvc plugin
    $.extend(true, plugin, {
        applyFilterValues: function (initialUrl, columnName, values, skip) {
            var self = this;
            self.gridColumnFilters = null;
            var filters = this.jqContainer.find(".grid-filter");
            if (initialUrl.length > 0)
                initialUrl += "&";

            var url = "";
            if (!skip) {
                url += this.getFilterQueryData(columnName, values);
            }

            if (this.options.multiplefilters) { //multiple filters enabled
                for (var i = 0; i < filters.length; i++) {
                    if ($(filters[i]).attr("data-name") != columnName) {
                        var filterData = this.parseFilterValues($(filters[i]).attr("data-filterdata"));
                        if (filterData.length == 0) continue;
                        if (url.length > 0) url += "&";
                        url += this.getFilterQueryData($(filters[i]).attr("data-name"), filterData);
                    } else {
                        continue;
                    }
                }
            }
            
            var fullSearch = url;
            if (fullSearch.indexOf("?") == -1) {
                fullSearch = "?" + fullSearch;
            }

            self.gridColumnFilters = fullSearch;

            self.currentPage = 1;
            
            if (self.gridFilterForm) {
                var formButton = $("#" + self.gridFilterForm.attr('id') + " input[type=submit],button[type=submit]")[0];
                var l = Ladda.create(formButton);
                l.start();
            }
          
            self.updateGrid(fullSearch, function () {
                self.SetupGridHeaderEvents();
                self.initFilters();

                if (l) {
                    l.stop();
                }
            });
        },
        ajaxify: function (options) {
            var self = this;
            self.currentPage = 1;
            self.loadPagedDataAction = options.getPagedData;
            self.loadDataAction = options.getData;
            self.gridFilterForm = options.gridFilterForm;
            self.gridSort = $("div.sorted a").attr('href');

            if (self.gridSort) {
                if (self.gridSort.indexOf("grid-dir=0") != -1) {
                    self.gridSort = self.gridSort.replace("grid-dir=0", "grid-dir=1");
                } else {
                    self.gridSort = self.gridSort.replace("grid-dir=1", "grid-dir=0");
                }
            }

            self.updateGrid = function (search, callback) {
                var gridQuery = "";
                
                if (self.gridFilterForm) {
                    $("#" + self.gridFilterForm.attr("id") + " input,select").each(function (index, item) {
                        if ($(item).attr('id')) {
                            var queryVal = $(item).attr('id') + "=" + $(item).val();
                            if (gridQuery !== "") {
                                gridQuery += "&" + queryVal;
                            } else {
                                gridQuery += queryVal;
                            }
                        }
                    });
                }

                if (search) {
                    gridQuery = search + "&" + gridQuery;
                } else {
                    gridQuery = "?" + gridQuery;
                }

                if (self.gridSort) {
                    if (self.gridSort.indexOf("?") != -1) {
                        self.gridSort = self.gridSort.substr(1);
                    }

                    gridQuery += "&" + self.gridSort;
                }

                var gridUrl = self.loadDataAction + gridQuery;
                $.ajax({
                    url: gridUrl,
                    type: 'get',
                    data: {},
                    contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                    async: true,
                    cache: false,
                    timeout: 10000
                }).done(function(response) {
                    $(".grid-mvc").html(response.Html);

                    if (callback) {
                        callback();
                    }

                    self.notifyOnGridLoaded(response, $.Event("GridLoaded"));
                });
            };

            self.SetupGridHeaderEvents = function () {
                $(".grid-header-title > a").on('click', function (e) {
                    self.gridSort = '';
                    e.preventDefault();
                    self.currentPage = 1;
                    
                    if (self.gridFilterForm) {
                        var formButton = $("#" + self.gridFilterForm.attr('id') + " input[type=submit],button[type=submit]")[0];
                        var l = Ladda.create(formButton);
                        l.start();
                    }

                    // remove grid sort arrows
                    $(".grid-header-title").removeClass("sorted-asc");
                    $(".grid-header-title").removeClass("sorted-desc");

                    var mySearch = $(this).attr('href');
                    var isAscending = mySearch.indexOf("grid-dir=1") !== -1;
                    self.updateGrid(mySearch, function() {
                        if (l) {
                            l.stop();
                        }
                        
                        self.SetupGridHeaderEvents();
                        self.initFilters();
                    });

                    // update link to sort in opposite direction
                    if (isAscending) {
                        $(this).attr('href', mySearch.replace("grid-dir=1", "grid-dir=0"));
                    } else {
                        $(this).attr('href', mySearch.replace("grid-dir=0", "grid-dir=1"));
                    }

                    // add new grid sort arrow
                    var newSortClass = isAscending ? "sorted-desc" : "sorted-asc";
                    $(this).parent(".grid-header-title").addClass(newSortClass);
                    $(this).parent(".grid-header-title").children("span").remove();
                    $(this).parent(".grid-header-title").append($("<span/>").addClass("grid-sort-arrow"));
                    self.gridSort = mySearch.substr(mySearch.match(/grid-column=\w+/).index);
                });
            };

            if (self.gridFilterForm) {
                self.gridFilterForm.on('submit', function (e) {
                    e.preventDefault();
                    self.currentPage = 1;
                    var formButton = $("#" + self.gridFilterForm.attr('id') + " input[type=submit],button[type=submit]")[0];
                    var l = Ladda.create(formButton);
                    l.start();
                    self.updateGrid(location.search, function () {
                        self.SetupGridHeaderEvents();
                        self.initFilters();
                        l.stop();
                    });
                });
            }

            $(this)[0].jqContainer.on("click", ".grid-next-page", function (e) {
                e.preventDefault();
                self.currentPage++;
                self.loadPage();
                $(".pagination li.active").removeClass("active").children("a").attr('href', '#');
                $("a[data-page=" + self.currentPage + "]").parent("li").addClass("active");
            });

            $(this)[0].jqContainer.on("click", ".grid-prev-page", function (e) {
                e.preventDefault();
                self.currentPage--;
                self.loadPage();
                $(".pagination li.active").removeClass("active").children("a").attr('href', '#');
                $("a[data-page=" + self.currentPage + "]").parent("li").addClass("active");
            });

            $(this)[0].jqContainer.on("click", ".grid-page-link", function (e) {
                e.preventDefault();
                var pageNumber = $(this).attr('data-page');
                self.currentPage = pageNumber;
                self.loadPage();
                $(".pagination li.active").removeClass("active").children("a").attr('href', '#');
                $(this).parent("li").addClass("active");
            });

            this.loadPage = function () {
                var gridTableBody = $(".grid-footer").closest(".grid-wrap").find("tbody");
                var nextPageLink = $(".grid-next-page");
                var prevPageLink = $(".grid-prev-page");

                var gridQuery = "";
                
                if (self.gridFilterForm) {
                    $("#" + self.gridFilterForm.attr("id") + " input,select").each(function (index, item) {
                        if ($(item).attr('id')) {
                            gridQuery += "&" + $(item).attr('id') + "=" + $(item).val();
                        }
                    });
                }

                if (self.gridSort) {
                    gridQuery += "&" + self.gridSort;
                }
                
                if (self.gridColumnFilters) {
                    gridQuery += "&" + self.gridColumnFilters.replace("?","");
                }

                $.get(self.loadPagedDataAction + self.pad(location.search) + self.currentPage + gridQuery)
                    .done(function (response) {
                        gridTableBody.html("");
                        gridTableBody.append(response.Html);
                        if (!response.HasItems) {
                            nextPageLink.hide();
                        } else {
                            nextPageLink.show();
                        }

                        if (self.currentPage == 1) {
                            prevPageLink.hide();
                        } else {
                            prevPageLink.show();
                        }
                        
                        self.notifyOnGridLoaded(response, $.Event("GridLoaded"));
                    })
                    .fail(function () {
                        alert("cannot load items");
                    });
            };

            this.pad = function (query) {
                if (query.length == 0) return "?page=";
                return query + "&page=";
            };

            self.SetupGridHeaderEvents();
        },
        onGridLoaded: function (func) {
            this.events.push({ name: "onGridLoaded", callback: func });
        },
        notifyOnGridLoaded : function (data, e) {
            e.data = data;
            this.notifyEvent("onGridLoaded", e);
        },
        refreshFullGrid: function () {
            var self = this;
            self.currentPage = 1;
            self.updateGrid(location.search, function () {
                self.SetupGridHeaderEvents();
                self.initFilters();
            });
        }
    });
})(jQuery);