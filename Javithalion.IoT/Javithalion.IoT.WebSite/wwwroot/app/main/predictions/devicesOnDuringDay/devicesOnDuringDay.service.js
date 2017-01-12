(function () {
    'use strict';

    angular
        .module('app.devices')
        .service('DevicesOnDuringDayService', DevicesOnDuringDayService);

    /** @ngInject */
    function DevicesOnDuringDayService($http) {

        var svc = this;
        svc.devicesApiUrl = 'http://localhost:4311/api/devices';

        //Data        

        //Methods  
        svc.getHourlyPredictionByDate = function (date) {           

            return $http.get(svc.devicesApiUrl + '/SwitchedOnForecast/' + date)
            .then(function (response) {
                var predictedValues = [];

                angular.forEach(response.data.hourlyForecast, function (value, key) {
                    predictedValues.push(Number(value));                    
                });

                return {
                    date: new Date(response.data.date),
                    values : predictedValues
                }

            },
            function (httpError) {
                
                throw httpError.statusText;
            });          
        };
    }
})();