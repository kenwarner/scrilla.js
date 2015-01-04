angular.module('scrilla').directive('stateDebugger', ['$state', '$stateParams', function ($state, $stateParams) {
	return {
		restrict: 'E',
		replace: true,
		templateUrl: 'app/directives/stateDebugger/stateDebugger.html'
	};
}]);
