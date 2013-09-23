var AccountCtrl = ['$scope', '$http', function ($scope, $http) {
	$http.get('/api/accounts/balances').success(function (data) {
		$scope.model = data;
	});
}];
