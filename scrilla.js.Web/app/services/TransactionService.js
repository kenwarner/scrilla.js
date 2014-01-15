scrilla.services.factory('TransactionService', ['$resource',
	function ($resource) {
		return $resource('api/transactions', {}, {
			list: { method: 'GET', isArray: true }
		});
	}]);