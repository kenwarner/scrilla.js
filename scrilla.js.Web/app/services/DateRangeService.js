scrilla.services.factory('DateRangeService', ['$location', '$filter', '$document', '$log', function ($location, $filter, $document, $log) {
	var from, to;
	var now = new Date();
	var min = new Date(2012, 2, 1);
	var max = new Date(now.getFullYear(), now.getMonth() + 5, 1, 0, 0, 1);

	var defaultFrom = new Date(now.getFullYear(), now.getMonth() - 4, 1);
	var defaultTo = new Date(now.getFullYear(), now.getMonth() + 1, 1);

	var UrlToDate = function (dateString) {
		var dateArray = dateString.match(/(\d{4})-(\d{2})-(\d{2})/);
		return new Date(dateArray[1], dateArray[2] - 1, dateArray[3]);
	};

	return {
		init: function () {
			$log.info('initializing DateRangeService');

			var locationFrom = $location.search().from;
			var locationTo = $location.search().to;

			// TODO all the $location syncing should probably be offloaded to another service
			this.setFrom(locationFrom ? UrlToDate(locationFrom) : defaultFrom);
			this.setTo(locationTo ? UrlToDate(locationTo) : defaultTo);

			if ($location.search().from != this.toUrlFormat(from)) {
				$log.info('resetting ?from = ' + $location.search().from + " -> " + this.toUrlFormat(from));
				$location.search('from', this.toUrlFormat(from));
			}

			if ($location.search().to != this.toUrlFormat(to)) {
				$log.info('resetting ?to = ' + $location.search().to + " -> " + this.toUrlFormat(to));
				$location.search('to', this.toUrlFormat(to));
			}

			$log.info('DateRangeService initialized with: ' + this.toUrlFormat(from) + ' - ' + this.toUrlFormat(to));
		},

		setFrom: function (val) {
			if (!from || from.getTime() != val.getTime()) {
				$log.info('setting from = ' + val);
				from = val;

				if ($location.search().from != this.toUrlFormat(from)) {
					$log.info('setting ?from = ' + $location.search().from + " -> " + this.toUrlFormat(from));
					$location.search('from', this.toUrlFormat(from));
				}
			}
		},

		setTo: function (val) {
			if (!to || to.getTime() != val.getTime()) {
				$log.info('setting to = ' + val);
				to = val;

				if ($location.search().to != this.toUrlFormat(to)) {
					$log.info('setting ?to = ' + $location.search().to + " -> " + this.toUrlFormat(to));
					$location.search('to', this.toUrlFormat(to));
				}
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
		},

		lastDayOfMonth: function (strDate) {
			var date = UrlToDate(strDate);
			return new Date(date.getFullYear(), date.getMonth() + 1, 0);
		},

		isLastDayOfTheMonth: function (date) {
			var test = new Date(date.getTime());
			test.setDate(test.getDate() + 1);
			return test.getDate() === 1;
		},

		isInSameMonth: function (date1, date2) {
			return (date1.getFullYear() === date2.getFullYear()) && (date1.getMonth() === date2.getMonth());
		}

	};
}]);