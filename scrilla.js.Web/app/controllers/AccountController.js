scrilla.controllers.controller('AccountController', ['$scope', '$log', 'DateRangeService', 'AccountService', function ($scope, $log, DateRangeService, AccountService) {
	$scope.isNaN = isNaN;
	$scope.DateRangeService = DateRangeService;
	$scope.model = {};
	$scope.model.summary = $scope.DateRangeService.summary();

	$scope.$watch('DateRangeService', function (newValue, oldValue, scope) {
		$log.info('AccountController watched DateRangeService');
		UpdateModel();
	});

	var UpdateModel = function () {
		$log.info('AccountController updating model');

		var dateRange = $scope.DateRangeService.getDateRange();
		var p = AccountService.balances({
			from: $scope.DateRangeService.toUrlFormat(dateRange.from),
			to: $scope.DateRangeService.toUrlFormat(dateRange.to)
		});

		p.$promise.then(function (result) {
			$log.info('account balances received\n');
			$scope.model.data = result;
		});

	};
}]);