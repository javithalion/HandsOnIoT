(function () {
    'use strict';

    angular
        .module('app.devices')
        .service('PredictionsService', PredictionsService);

    /** @ngInject */
    function PredictionsService($http) {

        var svc = this;

        //Data
        svc.devicesApiUrl = 'http://localhost:4311/api/devices';

        // Methods
        svc.getAllDevices = function (searchText) {
            return $http.get(svc.devicesApiUrl, { params: { "searchText": searchText } });
        };
    }
})();