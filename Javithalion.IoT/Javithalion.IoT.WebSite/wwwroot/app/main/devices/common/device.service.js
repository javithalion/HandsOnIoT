(function () {
    'use strict';

    angular
        .module('app.devices')
        .service('DeviceService', DeviceService);

    /** @ngInject */
    function DeviceService($http) {

        var svc = this;

        //Data
        svc.devicesApiUrl = 'http://localhost:4311/api/devices';
        svc.deviceEventsApiUrl = '';

        // Methods
        svc.getAllDevices = function () {
            return $http.get(svc.devicesApiUrl);
        };

        svc.getDevice = function (id) {
            return $http.get(svc.devicesApiUrl + '/' + id);
        };

        svc.createDevice = function (device) {
            return $http.post(svc.devicesApiUrl, JSON.stringify(device));
        };

        svc.editDevice = function (device) {
            return $http.put(svc.devicesApiUrl, JSON.stringify(device));
        };

        svc.deleteDevice = function (idToDelete) {
            return $http.delete(svc.devicesApiUrl, {
                data: { "id": idToDelete },
                headers: { 'Content-Type': 'application / json' }
            })
        };

        svc.getAllDeviceEvents = function (device) {
            return $http.get(svc.devicesApiUrl, { deviceId: device.id });
        }
    }
})();