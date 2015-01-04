angular.module('scrilla').factory('TransactionDataService', ['$window', '$q', '$http', '$stateParams', function ($window, $q, $http, $stateParams) {
	var service = {};

	var cacheDecorator = function (promise, cacheKey) {
		var deferred = $q.defer();

		// check cache
		var data = angular.fromJson($window.sessionStorage[cacheKey]);

		// if data is in cache return the promise
		if (angular.isDefined(data)) {
			deferred.resolve(data);
		} else {
			// if data is not in cache let's get it from the server
			promise().success(function (response) {
				// put data in cache
				$window.sessionStorage[cacheKey] = angular.toJson(response);
				deferred.resolve(response);
			});
		}

		return deferred.promise;
	}

	service.getTransactions = function () {
		return cacheDecorator(function () { return $http.get('/api/transactions', { params: $stateParams }); }, 'transactions-' + angular.toJson($stateParams));
	};

	service.getRecentTransactions = function () {
		return cacheDecorator(function () { return $http.get('/api/transactions/recent'); }, 'recentTransactions');
	};

	service.getAllTransactions = function () {
		return cacheDecorator(function () { return $http.get('/api/transactions'); }, 'allTransactions');
	};

	return service;
}]);
