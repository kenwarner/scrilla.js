angular.module('scrilla').directive('transactionCriteriaEditor', ['$state', '$stateParams', function ($state, $stateParams) {
	return {
		restrict: 'E',
		replace: true,
		templateUrl: 'app/directives/transactionCriteriaEditor/transactionCriteriaEditor.html',
		link: function (scope) {
			scope.$watchGroup(['$stateParams.from', '$stateParams.to', '$stateParams.vendorId'], function (newValue, oldValue) {
				if (newValue !== oldValue && !$state.current.abstract) {
					$state.transitionTo($state.current, $stateParams, { notify: false });
				}
			});

			scope.animals = [
				{
					name: 'cat',
					email: 'test'
				},
				{
					name: 'dog',
					email: 'test2'
				}
			];
		}
	};
}]);
