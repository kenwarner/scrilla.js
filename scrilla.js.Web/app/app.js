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
				templateUrl: '/app/views/accounts.html',
			})
			.when('/transactions', {
				templateUrl: '/app/views/transactions.html',
			})
			.when('/vendors', {
				templateUrl: '/app/views/vendors.html',
			});

	}]);


scrilla.app.run(['$rootScope', '$route', '$log', 'DateRangeService',
	function ($rootScope, $route, $log, DateRangeService) {
		$log.info('starting app');
		$rootScope.DateRangeService = DateRangeService;

		DateRangeService.init();

		$rootScope.$on('$routeChangeStart', function (ev, next, current) {
			$log.info('route change: ' + next.originalPath);

			DateRangeService.init();
		});
	}
]);
