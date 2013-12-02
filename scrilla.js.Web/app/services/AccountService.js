scrilla.services.factory('AccountService', ['$resource',
	function ($resource) {
		return $resource('api/accounts/balances', {}, {
			balances: { method: 'GET' }
		});
	}]);