scrilla.directives.directive('dateRangeSlider', ['$filter', '$log', 'DateRangeService', function ($filter, $log, DateRangeService) {
	return {
		restrict: 'E',
		link: function (scope, element, attrs) {
			scope.DateRangeService = DateRangeService;

			angular.element(element).dateRangeSlider({
				arrows: false,
				bounds: {
					min: scope.DateRangeService.getMin(),
					max: scope.DateRangeService.getMax()
				},
				defaultValues: {
					min: scope.DateRangeService.getFrom(),
					max: scope.DateRangeService.getTo()
				},
				valueLabels: "hide",
				formatter: function (val) {
					return $filter('date')(val, 'MMMM yyyy');
				},
				step: {
					months: 1
				},
				scales: [{
					next: function (value) {
						var next = new Date(value);
						var r = new Date(next.setMonth(value.getMonth() + 1));
						return r;
					},
					label: function (value, nextValue) {
						if (value.getMonth() == 0)
							return '\'' + $filter('date')(value, 'yy');

						return $filter('date')(value, 'MMM');
					}
				}]
			});

			angular.element(element).bind("valuesChanged", function (e, data) {
				$log.info('dateRangeSlider valuesChanges: ' + data.values.min + ',' + data.values.max);

				scope.$apply(function () {
					scope.DateRangeService.set({ from: data.values.min, to: data.values.max });
				});
			});
		}
	};
}]);
