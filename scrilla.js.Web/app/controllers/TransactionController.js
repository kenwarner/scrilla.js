scrilla.controllers.controller('TransactionController', ['$scope', '$log', '$location', 'DateRangeService', 'TransactionService', function ($scope, $log, $location, DateRangeService, TransactionService) {
	$scope.DateRangeService = DateRangeService;

	$scope.$watch('DateRangeService', function (newValue, oldValue, scope) {
		$log.info('TransactionController watched DateRangeService');
		UpdateModel();
	});

	var UpdateModel = function () {
		$log.info('TransactionController updating model');

		var dateRange = $scope.DateRangeService.getDateRange();
		var p = TransactionService.list({
			accountId: $location.search().accountId,
			from: $scope.DateRangeService.toUrlFormat(dateRange.from),
			to: $scope.DateRangeService.toUrlFormat(dateRange.to)
		});

		p.$promise.then(function (result) {
			$log.info('transactions received\n');
			$scope.transactions = result;
		});

	};

	$scope.gridOptions = {
		data: 'transactions',
		plugins: [new ngGridFlexibleHeightPlugin()],
		columnDefs: [
			{ field: 'isReconciled', displayName: '' },
			{ field: 'timestamp', displayName: 'Date', cellFilter: "date:'yyyy-MM-dd'" },
			{ field: 'vendorId', displayName: 'Vendor' },
			{ field: 'amount', displayName: 'Amount', cellFilter: 'parenlessCurrency' }
		]
	};
}]);
