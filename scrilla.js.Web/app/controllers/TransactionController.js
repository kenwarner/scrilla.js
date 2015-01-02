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
				bindScrollVertical: false,
				//infiniteScroll: 5,//50 / data.length * 100,
				columnDefs: [
					{ name: 'Date', field: 'timestamp', cellFilter: 'date:"yyyy-MM-dd"', maxWidth: 1 },
					{ name: 'Account', field: 'accountName', enableCellEdit: false },
					{ name: 'Vendor', field: 'vendorName' },
					{ name: 'Category', field: 'categoryName' },
					{ name: 'Amount', field: 'amount', cellFilter: 'parenlessCurrency', maxWidth: 1, enableCellEdit: false }
				]
			};
		});
	};

	// initialize Controller
	vm.getTransactions();
}]);
