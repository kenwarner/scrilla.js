angular.module('scrilla', ['ui.router', 'ui.grid', 'ui.grid.cellNav', 'ui.grid.autoResize', 'angular-loading-bar', 'ngAnimate']);

angular.module('scrilla').config(['$stateProvider', function ($stateProvider) {
	$stateProvider
		.state('scrilla', {
			url: '?accountId&vendorId&categoryId&from&to',
			template: '<ui-view></ui-view>'
		})
		.state('scrilla.transactions', {
			url: '/transactions',
			templateUrl: 'app/views/transactions.html'
		});
}]);
