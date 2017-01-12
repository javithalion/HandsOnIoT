(function () {
    'use strict';

    angular
        .module('app.devices')
        .controller('PredictionsController', PredictionsController);

    /** @ngInject */
    function PredictionsController($mdToast, $state, PredictionsService) {
        var vm = this;
        vm.contentPlaceHolder = angular.element('#content');

        // Data		
        vm.availablePredictions = [
            //{ id: "DvcOn", description: "Devices on for a given date and hour (future)" },            
            //{ id: "DvcSu", description: "For a given device get his next startup" },
            //{ id: "DvcSd", description: "For a given device get his next teardown" },
            { id: "DvcNmbr", description: "Forecast of daily's connected devices number (hourly)" }
        ];

        vm.devices = [];

        // Methods
        vm.searchTextChange = function (searchText) {
            PredictionsService.getAllDevices(searchText)
            .then(function (response) {
                vm.devices = response.data;

            }, function (rejection) {

                $mdToast.show(
                    $mdToast.simple().textContent('Error on getting devices: ' + JSON.stringify(rejection.data))
                    .parent(vm.contentPlaceHolder)
                    .position('top right')
                    .toastClass('toast-error'));
            });
        }

        vm.submitStepper = function () {
            if (vm.selectedPrediction) {
                $state.go('app.predictions.devicesOnDuringDay', { "date": vm.selectedDate.toISOString().substring(0, 10) }); //YYYY-MM-DD
            }
            else {
                return false;
            }
        }
        //////////
    }
})();