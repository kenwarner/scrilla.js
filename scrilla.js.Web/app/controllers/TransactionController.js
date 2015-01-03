angular.module('scrilla').controller('TransactionController', ['TransactionDataService', function (TransactionDataService) {
	var allTransactions;

	var vm = this;
	vm.gridOptions = {
		enableFiltering: true,
		enableVerticalScrollbar: false,
		enableHorizontalScrollbar: false,
		bindScrollHorizontal: false,
		bindScrollVertical: false,
		enableRowSelection: true,
		selectionRowHeaderWidth: 35,

		//minRowsToShow: data.length,
		//data: data,

		//infiniteScroll: 5,//50 / data.length * 100,
		columnDefs: [
			{ name: 'Date', field: 'timestamp', cellFilter: 'date:"yyyy-MM-dd"', maxWidth: 1, enableFiltering: false },
			{ name: 'Account', field: 'accountName', enableCellEdit: false },
			{ name: 'Vendor', field: 'vendorName' },
			{ name: 'Category', field: 'categoryName' },
			{ field: 'amount', cellFilter: 'parenlessCurrency', maxWidth: 1, enableFiltering: false, enableCellEdit: false }
		],

		onRegisterApi: function(gridApi) {
			vm.gridApi = gridApi;
		}
	};

	var setGridData = function (data) {
		vm.gridOptions.minRowsToShow = data.length;
		vm.gridOptions.data = data;

		if (angular.isDefined(vm.gridApi)) {
			vm.gridApi.core.refresh();
		}
	};

	vm.getTransactions = function () {
		TransactionDataService.getTransactions().success(function (data) {
			setGridData(data);
		});
	};

	vm.getAllTransactions = function() {
		TransactionDataService.getAllTransactions().success(function (data) {
			allTransactions = data;
			setGridData(data);
		});
	};

	// initialize Controller
	vm.getTransactions();
	vm.getAllTransactions();
}]);
