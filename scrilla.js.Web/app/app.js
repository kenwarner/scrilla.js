var scrilla = scrilla || {};
scrilla.controllers = angular.module('scrilla.controllers', ['ngRoute']);
scrilla.services = angular.module('scrilla.services', ['ngResource']);
scrilla.directives = angular.module('scrilla.directives', []);
scrilla.app = angular.module('scrillaApp', ['scrilla.controllers', 'scrilla.services', 'scrilla.directives']);

scrilla.app.config(['$locationProvider', 
	function($locationProvider) {
		$locationProvider.html5Mode(true);
	}]);

scrilla.app.config(['$routeProvider', 
	function ($routeProvider) {
		$routeProvider
			.when('/', {
				controller: 'AccountController'
			})
			.when('/transactions', {
				controller: 'TransactionController'
			})
			.when('/vendors', {
				controller: 'VendorController'
			});
	}]);