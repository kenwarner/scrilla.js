scrilla.filters.filter('duringMonths', function () {
	return function (balances, months) {
		return _.map(months, function (month) {
			return _.find(this, function (balance) {
				return balance.month == month;
			});
		}, balances);
	};
});
