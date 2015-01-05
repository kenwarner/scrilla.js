angular.module('scrilla', ['ui.router', 'ui.grid', 'ui.grid.selection', 'ui.grid.cellNav', 'ui.grid.autoResize', 'angular-loading-bar', 'ngAnimate', 'ngSanitize', 'ui.date', 'ui.select', 'ui.unique']);

angular.module('scrilla').config(['$stateProvider', function ($stateProvider) {
	$stateProvider
		.state('scrilla', {
			url: '?from&to&accountId&vendorId&categoryId',
			template: '<ui-view></ui-view>'
		})
		.state('scrilla.transactions', {
			url: '/transactions',
			templateUrl: 'app/views/transactions.html'
		});
}]);

angular.module('scrilla').run(['$rootScope', '$state', '$stateParams', function ($rootScope, $state, $stateParams) {
	$rootScope.$state = $state;
	$rootScope.$stateParams = $stateParams;
}]);

angular.module('scrilla').config(function(uiSelectConfig) {
  uiSelectConfig.theme = 'bootstrap';
});

// provide success() and error() for $q
// http://stackoverflow.com/a/17889426/55948
angular.module('scrilla').config(function ($provide) {
	$provide.decorator('$q', function ($delegate) {
		var defer = $delegate.defer;
		$delegate.defer = function () {
			var deferred = defer();
			deferred.promise.success = function (fn) {
				deferred.promise.then(fn);
				return deferred.promise;
			};
			deferred.promise.error = function (fn) {
				deferred.promise.then(null, fn);
				return deferred.promise;
			};
			return deferred;
		};
		return $delegate;
	});

});
