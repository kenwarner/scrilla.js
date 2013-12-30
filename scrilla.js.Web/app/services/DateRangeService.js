scrilla.services.factory('DateRangeService', ['$location', '$filter', '$document', '$log', function ($location, $filter, $document, $log) {
	var from, to;
	var now = new Date();
	var min = new Date(2012, 5, 1);
	var max = new Date(now.getFullYear(), now.getMonth() + 5, 1, 0, 0, 1);

	var defaultFrom = new Date(now.getFullYear(), now.getMonth() - 4, 1);
	var defaultTo = new Date(now.getFullYear(), now.getMonth() + 1, 1);

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

			if ($location.search().from != this.toUrlFormat(from)) {
				$log.info('resetting ?from = ' + this.toUrlFormat(from));
				$location.search('from', this.toUrlFormat(from));
			}

			if ($location.search().to != this.toUrlFormat(to)) {
				$log.info('resetting ?to = ' + this.toUrlFormat(to));
				$location.search('to', this.toUrlFormat(to));
			}

			$log.info('DateRangeService initialized with: ' + this.toUrlFormat(from) + ' - ' + this.toUrlFormat(to));
		},

		setFrom: function (val) {
			if (!from || from.valueOf() != val.valueOf()) {
				$log.info('setting from = ' + val);
				from = val;

				$log.info('setting ?from = ' + this.toUrlFormat(from));
				$location.search('from', this.toUrlFormat(from));
			}
		},

		setTo: function (val) {
			if (!to || to.valueOf() != val.valueOf()) {
				$log.info('setting to = ' + val);
				to = val;

				$log.info('setting ?to = ' + this.toUrlFormat(to));
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

		summary: function() {
			return $filter('date')(from, 'MMMM yyyy') + ' to ' + $filter('date')(to, 'MMMM yyyy');
		},

		toUrlFormat: function (date) {
			return $filter('date')(date, 'yyyy-MM-dd');
		}
	};
}]);