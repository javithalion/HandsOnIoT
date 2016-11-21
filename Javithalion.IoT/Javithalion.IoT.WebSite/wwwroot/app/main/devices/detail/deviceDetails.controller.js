(function () {
    'use strict';

    angular
        .module('app.devices')
        .controller('DeviceDetailsController', DeviceDetailsController);

    /** @ngInject */
    function DeviceDetailsController($stateParams, $http, $mdToast, $state, $scope, DeviceService) {
        var vm = this;

        // Data       
        vm.contentPlaceHolder = angular.element('#content');

        vm.operativeSystems = [{ id: 1, name: 'Winows' }, { id: 2, name: 'Windows IoT' }, { id: 3, name: 'Android' }, { id: 4, name: 'iOS' }]

        vm.deviceId = $stateParams.id;


        vm.create = true;
        if (vm.deviceId)
            vm.create = vm.deviceId.toLowerCase() === 'new';

        // Methods
        vm.get = function (id) {

            DeviceService.getDevice(id)
            .then(function (response) {
                vm.device = response.data;
            }, function (rejection) {
                var message = 'Error on action: ' + JSON.stringify(rejection.data);
                $scope.$emit('$generalErrorNotification', message);
                $state.go('app.devices');
            });

            DeviceService.getAllDeviceEvents(id)
             .then(function (response) {
                 vm.device.events = response.data;
             }, function (rejection) {
                 var message = 'Error on action: ' + JSON.stringify(rejection.data);
                 $scope.$emit('$generalErrorNotification', message);
                 $state.go('app.devices');
             });

            console.log(vm.deviceId);
        }

        function submitForEdit() {

            DeviceService.editDevice(vm.device)
            .then(function (response) {
                $mdToast.show(
                    $mdToast.simple().textContent('Device modifications were saved')
                    .parent(vm.contentPlaceHolder)
                    .position('top right')
                    .toastClass('toast-success'));

            }, function (rejection) {

                $mdToast.show(
                    $mdToast.simple().textContent('Error on saving: ' + JSON.stringify(rejection.data))
                    .parent(vm.contentPlaceHolder)
                    .position('top right')
                    .toastClass('toast-error'));
            });
        }

        function submitForCreate() {

            DeviceService.createDevice(vm.device)
            .then(function (response) {

                var newId = response.data.id;

                var message = 'Device was created';
                $scope.$emit('$generalSuccessNotification', message);
                $state.go('app.devices.detail', { id: newId });

            }, function (rejection) {

                $mdToast.show(
                    $mdToast.simple().textContent('Error on saving: ' + JSON.stringify(rejection.data))
                    .parent(vm.contentPlaceHolder)
                    .position('top right')
                    .toastClass('toast-error'));
            });
        }

        //////////   
        vm.submit = vm.create ? submitForCreate : submitForEdit;

        if (!vm.create)
            vm.get(vm.deviceId);
    }
})();