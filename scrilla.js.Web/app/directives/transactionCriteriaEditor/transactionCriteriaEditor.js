angular.module('scrilla').directive('transactionCriteriaEditor', ['$state', '$stateParams', function ($state, $stateParams) {
	return {
		restrict: 'E',
		replace: true,
		templateUrl: 'app/directives/transactionCriteriaEditor/transactionCriteriaEditor.html',
		link: function (scope) {
			scope.selectedAccounts = {};
			scope.selectedVendors = {};
			scope.selectedCategories = {};

			scope.$watch('selectedAccounts.selected', function (newValue, oldValue) {
				if (newValue !== oldValue) {
					var accountIds = newValue.map(function (account) {
						return account.accountId;
					});

					$stateParams.accountId = accountIds;
				}
			});

			scope.$watch('selectedVendors.selected', function (newValue, oldValue) {
				if (newValue !== oldValue) {
					var vendorIds = newValue.map(function (vendor) {
						return vendor.vendorId;
					});

					$stateParams.vendorId = vendorIds;
				}
			});

			scope.$watch('selectedCategories.selected', function (newValue, oldValue) {
				if (newValue !== oldValue) {
					var categoryIds = newValue.map(function (category) {
						return category.categoryId;
					});

					$stateParams.categoryId = categoryIds;
				}
			});

			scope.$watchGroup(['$stateParams.from', '$stateParams.to', '$stateParams.accountId', '$stateParams.vendorId', '$stateParams.categoryId'], function (newValue, oldValue) {
				if (newValue !== oldValue && !$state.current.abstract) {
					$state.transitionTo($state.current, $stateParams, { notify: false });
				}
			});

		}
	};
}]);
