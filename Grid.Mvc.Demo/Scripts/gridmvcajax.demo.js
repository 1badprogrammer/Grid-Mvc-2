GridMvcAjax = {};
GridMvcAjax.demo = (function (my, $) {

    var constructorSpec = {
        updateGridAction: ''
    };

    my.init = function (options) {
        $(function () {
            constructorSpec = options;

            $.ajaxSetup({
                cache: false
            });

            pageGrids.carsGrid.ajaxify({
                getPagedData: constructorSpec.updateGridAction,
                getData: constructorSpec.updateGridAction
            });

            pageGrids.grid2.ajaxify({
                getPagedData: constructorSpec.grid2Action,
                getData: constructorSpec.grid2Action
            });
        });
    };

    return my;
}(GridMvcAjax.demo || {}, jQuery));