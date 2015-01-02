angular.module('scrilla').factory('TransactionDataService', ['$http', function ($http) {
	var service = {};

	service.getTransactions = function () {
		return $http.get('/api/transactions', { params: { from: '2012-01-01', to: '2012-02-01' } });
	};

	return service;
}]);