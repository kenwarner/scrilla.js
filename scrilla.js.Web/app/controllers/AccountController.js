scrilla.controllers.controller('AccountController', ['$scope', '$location', 'AccountService', function ($scope, $location, AccountService) {
	$scope.isNaN = isNaN;
	$scope.from = $location.search().from;
	$scope.to = $location.search().to;

	$scope.model = AccountService.balances({ from: $scope.from, to: $scope.to });
}]);