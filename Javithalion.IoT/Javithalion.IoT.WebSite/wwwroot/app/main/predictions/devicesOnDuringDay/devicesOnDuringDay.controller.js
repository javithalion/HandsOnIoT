(function () {
    'use strict';

    angular
        .module('app.devices')
        .controller('DevicesOnDuringDayController', DevicesOnDuringDayController);

    /** @ngInject */
    function DevicesOnDuringDayController($stateParams, DevicesOnDuringDayService) {
        var vm = this;       

        // Data		
        vm.requestedDate = Date.parse($stateParams.date);
       
        // Methods
        
        //////////
    }
})();