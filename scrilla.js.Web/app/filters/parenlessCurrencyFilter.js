scrilla.filters.filter('parenlessCurrency', ['currencyFilter', function (currencyFilter) {
	return function (amount) {
		var currency = currencyFilter(amount);
		if (amount >= 0)
			return currency;

		return currency.replace('(', '-').replace(')', '');
	};
}]);
