scrilla.directives.directive('summary', ['$log', '$filter', '$rootScope', 'DateRangeService', function ($log, $filter, $rootScope, DateRangeService) {
	return {
		restrict: 'A',
		link: function (scope, element, attrs) {
			var page = null;

			scope.DateRangeService = DateRangeService;
			scope.message = null;

			var generateMessage = function () {
				var dateRange = DateRangeService.getDateRange();
				var timeframe = null;

				// make sure page has a value
				page = page || "Scrilla";

				// same date
				if (dateRange.from.getTime() == dateRange.to.getTime())
					timeframe = ' for ' + $filter('date')(dateRange.from, 'MMMM dd, yyyy');

				// first of month
				else if (dateRange.from.getDate() === 1) {

					// through last day of the same month
					if (DateRangeService.isInSameMonth(dateRange.from, dateRange.to) && DateRangeService.isLastDayOfTheMonth(dateRange.to)) {
						timeframe = ' for ' + $filter('date')(from, 'MMMM yyyy');
					}

						// through first day of a different month
					else if (dateRange.to.getDate() === 1) {
						timeframe = ' from ' + $filter('date')(dateRange.from, 'MMMM yyyy') + ' through ' + $filter('date')(dateRange.to, 'MMMM yyyy');
					}

						// through last day of a different month
					else if (DateRangeService.isLastDayOfTheMonth(dateRange.to)) {
						timeframe = ' from ' + $filter('date')(dateRange.from, 'MMMM yyyy') + ' through ' + $filter('date')(dateRange.to, 'MMMM yyyy');
					}
				}

				timeframe = timeframe || ' from ' + $filter('date')(dateRange.from, 'MMMM dd, yyyy') + ' through ' + $filter('date')(dateRange.to, 'MMMM dd, yyyy');
				scope.message = page + timeframe;
				$log.info('Summary directive setting message = ' + scope.message);
			};

			scope.$watch('DateRangeService', function (newValue, oldValue, scope) {
				$log.info('Summary directive watched DateRangeService');

				generateMessage();
			});

			$rootScope.$on('$routeChangeStart', function (ev, next, current) {
				$log.info('summary route change: ' + next.originalPath);

				switch (next.originalPath) {
					case "/":
						page = "Accounts";
						break;
					case "/transactions":
						page = "Transactions";
						break;
					case "/vendors":
						page = "Vendors";
						break;
					default:
						break;
				}

				generateMessage();
			});

		}
	};
}]);
