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
				formatter: function (val) {
					return $filter('date')(val, 'MMMM yyyy');
				},
				step: {
					months: 1
				}
			});

			angular.element(element).bind("valuesChanged", function (e, data) {
				$log.info('dateRangeSlider valuesChanges: ' + data.values.min + ',' + data.values.max);

				scope.$apply(function () {
					scope.DateRangeService.setFrom(data.values.min);
					scope.DateRangeService.setTo(data.values.max);
				});
			});
		}
	};
}]);