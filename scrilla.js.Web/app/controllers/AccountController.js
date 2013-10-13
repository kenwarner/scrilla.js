var scrillaApp = angular.module('scrillaApp', []);

scrillaApp.controller('AccountCtrl', ['$scope', '$http', 
	function AccountCtrl($scope, $http) {
		$http.get('/api/accounts/balances').success(function (data) {
			$scope.model = data;
		});
	}]);
