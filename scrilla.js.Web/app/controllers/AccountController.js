scrilla.controllers.controller('AccountController', ['$scope', '$location', '$document', '$filter', 'AccountService', function ($scope, $location, $document, $filter, AccountService) {
	$scope.isNaN = isNaN;
	$scope.from = $location.search().from;
	$scope.to = $location.search().to;

	$scope.model = AccountService.balances({ from: $scope.from, to: $scope.to });
	$scope.model.$promise.then(function (result) {
		$scope.model = result;
		$scope.summary = "Accounts from " + $scope.model.dateRange.rangeSummary;
		$document.prop('title', $scope.summary);
	});
}]);