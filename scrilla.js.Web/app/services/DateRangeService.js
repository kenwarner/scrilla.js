scrilla.services.factory('DateRangeService', ['$location', '$filter', '$document', '$log', function ($location, $filter, $document, $log) {
	var from, to;
	var min = new Date(2013, 0, 1);
	var max = new Date(2013, 11, 31);

	var defaultFrom = new Date(2013, 1, 1);
	var defaultTo = new Date(2013, 10, 1);

	var UrlToDate = function (dateString) {
		var dateArray = dateString.split("-");
		return new Date(dateArray[0], dateArray[1] - 1, dateArray[2]);
	};

	return {
		init: function () {
			$log.info('initializing DateRangeService');

			var locationFrom = $location.search().from;
			var locationTo = $location.search().to;

			this.setFrom(locationFrom ? UrlToDate(locationFrom) : defaultFrom);
			this.setTo(locationTo ? UrlToDate(locationTo) : defaultTo);

			$log.info('DateRangeService initialized with: ' + from + ',' + to);
		},

		setFrom: function (val) {
			if (from !== val) {
				$log.info('setting from = ' + val);

				from = val;
				$location.search('from', this.toUrlFormat(from));
			}
		},

		setTo: function (val) {
			if (to !== val) {
				$log.info('setting to = ' + val);

				to = val;
				$location.search('to', this.toUrlFormat(to));
			}
		},

		set: function(dateRange) {
			this.setFrom(dateRange.from);
			this.setTo(dateRange.to);
		},

		getFrom: function() {
			return from;
		},

		getTo: function() {
			return to;
		},

		getDateRange: function() {
			return {
				from: from,
				to: to
			};
		},

		getMin: function() {
			return min;
		},

		getMax: function () {
			return max;
		},

		toUrlFormat: function (date) {
			return $filter('date')(date, 'yyyy-MM-dd');
		}
	};
}]);