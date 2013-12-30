scrilla.controllers.controller('AccountController', ['$scope', '$log', 'DateRangeService', 'AccountService', function ($scope, $log, DateRangeService, AccountService) {
	$scope.isNaN = isNaN;
	$scope.DateRangeService = DateRangeService;
	$scope.model = {};

	$scope.$watch('DateRangeService', function(newValue, oldValue, scope) {
		$log.info('AccountController watched DateRangeService');

		UpdateModel();
	}, true);

	var UpdateModel = function () {
		var dateRange = $scope.DateRangeService.getDateRange();
		$log.info('AccountController updating model: ' + dateRange)

		var p = AccountService.balances({
			from: $scope.DateRangeService.toUrlFormat(dateRange.from),
			to: $scope.DateRangeService.toUrlFormat(dateRange.to)
		});

		p.$promise.then(function (result) {
			$log.info('account balances received');

			$scope.model = result;
			$scope.model.summary = "Accounts from " + result.dateRange.rangeSummary;
		});

	};
}]);