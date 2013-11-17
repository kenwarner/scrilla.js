var dateRangeFilters = angular.module('dateRangeFilters', []);

dateRangeFilters.filter('betweenDates', function () {
	return function (balances, from, to) {
		return _.filter(balances, function (balance) {
			return balance.month >= from && balance.month <= to;
		});
	};
});

dateRangeFilters.filter('duringMonths', function () {
	return function (balances, months) {
		return _.map(months, function (month) {
			return _.find(this, function (balance) {
				return balance.month == month;
			});
		}, balances);
	};
});
