angular.module('scrilla').factory('TransactionDataService', ['$http', '$stateParams', function ($http, $stateParams) {
	var service = {};

	service.getTransactions = function () {
		return $http.get('/api/transactions', { params: $stateParams });
	};

	return service;
}]);