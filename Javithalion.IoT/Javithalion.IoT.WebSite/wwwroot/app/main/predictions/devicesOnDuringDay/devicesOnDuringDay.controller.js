(function () {
    'use strict';

    angular
        .module('app.devices')
        .controller('DevicesOnDuringDayController', DevicesOnDuringDayController);

    /** @ngInject */
    function DevicesOnDuringDayController($stateParams, DevicesOnDuringDayService) {
        var vm = this;
        vm.predictionChart = null;


        // Data		
        //vm.requestedDate = Date.parse($stateParams.date);
        vm.requestedDate = $stateParams.date;

        vm.datacolumns = [{ "id": "devices-on", "type": "spline", "name": "Devices on", "color": "green" }];
        vm.xAxis = { "id": "x" };
        vm.predictionHourlyValues = [];

        // Methods
        vm.loadHourlyPredictionByDate = function () {
            DevicesOnDuringDayService.getHourlyPredictionByDate(vm.requestedDate)
                                 .then(function (forecast) {

                                     angular.forEach(forecast.values, function (value, key) {
                                         vm.predictionHourlyValues.push({ "x": key, "devices-on": value });
                                     });

                                     vm.requestedDate = forecast.date.toDateString();
                                 });
        };


        //////////   
        vm.loadHourlyPredictionByDate();
    }
})();