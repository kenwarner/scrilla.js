angular.module('scrilla').controller('TransactionController', ['$scope', '$state', '$stateParams', 'uiGridConstants', 'TransactionDataService', function ($scope, $state, $stateParams, uiGridConstants, TransactionDataService) {
	var allTransactions;

	var vm = this;
	vm.gridOptions = {
		enableFiltering: true,
		enableVerticalScrollbar: false,
		enableHorizontalScrollbar: false,
		bindScrollHorizontal: false,
		bindScrollVertical: false,
		enableRowSelection: true,
		enableColumnMenu: false,
		selectionRowHeaderWidth: 35,

		//minRowsToShow: data.length,
		//data: data,

		//infiniteScroll: 5,//50 / data.length * 100,
		columnDefs: [
			{
				name: 'Date',
				field: 'timestamp',
				enableColumnMenu: false,
				enableFiltering: false,
				filters: [
					{
						condition: uiGridConstants.filter.GREATER_THAN_OR_EQUAL,
						term: new Date(Date.parse($stateParams.from))
					},
					{
						condition: uiGridConstants.filter.LESS_THAN_OR_EQUAL,
						term: new Date(Date.parse($stateParams.to))
					}
				],
				cellFilter: 'date:"yyyy-MM-dd"',
				maxWidth: 1
			},
			{
				name: 'Account',
				field: 'accountName',
				enableColumnMenu: false,
				enableSorting: false,
				enableCellEdit: false,
				enableFiltering: false,
				filter: {
					condition: uiGridConstants.filter.CONTAINS,
					term: $stateParams.accountId
				}
			},
			{
				name: 'Vendor',
				field: 'vendorName',
				enableColumnMenu: false,
				enableSorting: false,
				enableFiltering: false,
				filter: {
					condition: uiGridConstants.filter.CONTAINS,
					term: $stateParams.vendorId
				}
			},
			{
				name: 'Category',
				field: 'categoryName',
				enableColumnMenu: false,
				enableSorting: false,
				enableFiltering: false,
				filter: {
					condition: uiGridConstants.filter.CONTAINS,
					term: $stateParams.categoryId
				}
			},
			{
				field: 'amount',
				cellFilter: 'parenlessCurrency',
				enableColumnMenu: false,
				enableFiltering: false,
				enableCellEdit: false,
				maxWidth: 1
			}
		],

		onRegisterApi: function (gridApi) {
			vm.gridApi = gridApi;
		}
	};

	var setGridData = function (data) {
		vm.gridOptions.minRowsToShow = data.length;
		vm.gridOptions.data = data;

		//if (angular.isDefined(vm.gridApi)) {
		//	vm.gridApi.core.refresh();
		//}
	};

	vm.getTransactions = function () {
		TransactionDataService.getTransactions().success(function (data) {
			setGridData(data);
		});
	};

	vm.getRecentTransactions = function () {
		TransactionDataService.getRecentTransactions().success(function (data) {
			setGridData(data);
		});
	};

	vm.getAllTransactions = function () {
		TransactionDataService.getAllTransactions().success(function (data) {
			allTransactions = data;
			setGridData(data);
		});
	};

	$scope.$watchGroup(['$stateParams.from', '$stateParams.to'], function (newValue, oldValue) {
		if (newValue !== oldValue && !$state.current.abstract) {
			vm.gridOptions.columnDefs[0].filters[0].term = new Date(Date.parse($stateParams.from));
			vm.gridOptions.columnDefs[0].filters[1].term = new Date(Date.parse($stateParams.to));
			vm.getTransactions();
		}
	});

	$scope.$watch('$stateParams.accountId', function (newValue, oldValue) {
		if (newValue !== oldValue && !$state.current.abstract) {
			vm.gridOptions.columnDefs[1].filter.term = $stateParams.accountId;
			if (angular.isDefined(vm.gridApi)) {
				vm.gridApi.core.refresh();
			}
		}
	});

	$scope.$watch('$stateParams.vendorId', function (newValue, oldValue) {
		if (newValue !== oldValue && !$state.current.abstract) {
			vm.gridOptions.columnDefs[2].filter.term = $stateParams.vendorId;
			if (angular.isDefined(vm.gridApi)) {
				vm.gridApi.core.refresh();
			}
		}
	});

	$scope.$watch('$stateParams.categoryId', function (newValue, oldValue) {
		if (newValue !== oldValue && !$state.current.abstract) {
			vm.gridOptions.columnDefs[3].filter.term = $stateParams.categoryId;
			if (angular.isDefined(vm.gridApi)) {
				vm.gridApi.core.refresh();
			}
		}
	});

	// initialize Controller
	vm.getTransactions();
	//vm.getRecentTransactions();
	//vm.getAllTransactions();
}]);
