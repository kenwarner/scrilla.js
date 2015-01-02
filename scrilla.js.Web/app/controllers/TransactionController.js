angular.module('scrilla').controller('TransactionController', ['TransactionDataService', function (TransactionDataService) {
	var vm = this;

	vm.getTransactions = function () {
		TransactionDataService.getTransactions().success(function (data) {
			vm.gridOptions = {
				data: data,
				minRowsToShow: data.length,
				enableFiltering: true,
				enableVerticalScrollbar: false,
				enableHorizontalScrollbar: false,
				bindScrollHorizontal: false,
				bindScrollVertical: false,
				enableRowSelection: true,
				selectionRowHeaderWidth: 35,


				//infiniteScroll: 5,//50 / data.length * 100,
				columnDefs: [
					{ name: 'Date', field: 'timestamp', cellFilter: 'date:"yyyy-MM-dd"', maxWidth: 1, enableFiltering: false },
					{ name: 'Account', field: 'accountName', enableCellEdit: false },
					{ name: 'Vendor', field: 'vendorName' },
					{ name: 'Category', field: 'categoryName' },
					{ field: 'amount', cellFilter: 'parenlessCurrency', maxWidth: 1, enableFiltering: false, enableCellEdit: false }
				]
			};
		});
	};

	// initialize Controller
	vm.getTransactions();
}]);
