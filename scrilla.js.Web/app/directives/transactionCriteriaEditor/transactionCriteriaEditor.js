angular.module('scrilla').directive('transactionCriteriaEditor', ['$state', '$stateParams', function ($state, $stateParams) {
	return {
		restrict: 'E',
		replace: true,
		templateUrl: 'app/directives/transactionCriteriaEditor/transactionCriteriaEditor.html',
		link: function (scope) {
			scope.$watchGroup(['$stateParams.from', '$stateParams.to', '$stateParams.accountId', '$stateParams.vendorId', '$stateParams.categoryId'], function (newValue, oldValue) {
				if (newValue !== oldValue && !$state.current.abstract) {
					$state.transitionTo($state.current, $stateParams, { notify: false });
				}
			});


			scope.accounts = [
				{
					id: 13,
					name: 'Wells Fargo Main'
				},
				{
					id: 15,
					name: 'Ken Blow'
				}
			];

			scope.selectedAccounts = {};

			scope.$watch('selectedAccounts.selected', function(newValue, oldValue) {
				if (newValue !== oldValue) {
					var accountIds = newValue.map(function(account) {
					  return account.accountId;
					});
					
					$stateParams.accountId = accountIds;
				}
			});
		}
	};
}]);
