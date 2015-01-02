angular.module('scrilla').controller('TransactionController', ['TransactionDataService', function (TransactionDataService) {
    var vm = this;

    vm.getTransactions = function () {
        TransactionDataService.getTransactions().success(function (data) {
            vm.gridOptions = {
                data: data,
                minRowsToShow: data.length,
                enableVerticalScrollbar: false,
                enableHorizontalScrollbar: false,
                bindScrollHorizontal: false,
                bindScrollVertical: false
            };
        });
    };

    // initialize Controller
    vm.getTransactions();
}]);
