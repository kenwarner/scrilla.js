var scrilla = scrilla || {};
scrilla.directives = angular.module('scrilla.directives', []);
scrilla.filters = angular.module('scrilla.filters', []);
scrilla.controllers = angular.module('scrilla.controllers', ['ngRoute']);
scrilla.services = angular.module('scrilla.services', ['ngResource']);
scrilla.app = angular.module('scrillaApp', ['scrilla.directives', 'scrilla.filters', 'scrilla.controllers', 'scrilla.services']);

scrilla.app.config(['$locationProvider', '$routeProvider',
	function ($locationProvider, $routeProvider) {
		$locationProvider.html5Mode(true);

		$routeProvider
			.when('/', {
				controller: 'AccountController',
				templateUrl: '/app/views/accounts.html',
			})
			.when('/transactions', {
				controller: 'TransactionController',
				templateUrl: '/app/views/transactions.html',
			})
			.when('/vendors', {
				controller: 'VendorController',
				templateUrl: '/app/views/vendors.html',
			});
	}]);

scrilla.app.run(['DateRangeService',
	function (DateRangeService) {
		DateRangeService.init();
	}
]);