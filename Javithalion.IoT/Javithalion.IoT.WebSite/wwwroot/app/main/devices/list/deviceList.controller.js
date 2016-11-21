(function () {
    'use strict';

    angular
        .module('app.devices')
        .controller('DeviceListController', DeviceListController);

    /** @ngInject */
    function DeviceListController($scope, $compile, $mdDialog, $mdToast, $q, DeviceService, DTOptionsBuilder, DTColumnBuilder) {
        var vm = this;

        // Data    
        vm.dtInstance = {}       
        vm.contentPlaceHolder = angular.element('#content');

        vm.dtOptions = DTOptionsBuilder.fromFnPromise(loadData)
        .withPaginationType('full_numbers')
        .withDOM('<"top" fi><t><"bottom" lp>')
        .withOption('createdRow', drawComplete);

        vm.dtColumns = [
            DTColumnBuilder.newColumn('id').withTitle('Id'),
            DTColumnBuilder.newColumn('name').withTitle('Name'),
            DTColumnBuilder.newColumn(null)
                .withTitle('Actions')
                .notSortable()
                .renderWith(function (data, type, full) {
                    var renderResult = '<div layout="row">';

                    renderResult += '<md-button ui-sref="app.devices.detail({\'id\' : \'' + data.id + '\'})" class="md-fab md-mini md-primary icon icon-pencil"  aria-label="Edit"></md-button>'; //edit button                   
                    renderResult += '<md-button ng-click="vm.showConfirm($event,\'' + data.id + '\')" class="md-fab md-mini md-warn icon icon-delete"  aria-label="Delete"></md-button>'; //delete button

                    renderResult += '</div>';
                    return renderResult;
                })
        ];

        // Methods                
        vm.showConfirm = function (ev, id) {
            var confirm = $mdDialog.confirm()
            .title('Are you sure to delete this device?')
            .textContent('All the recorded events will be kept')
            .ariaLabel('Delete device confirmation')
            .targetEvent(ev)
            .ok('Yes, delete it')
            .cancel('No, keep that device');

            $mdDialog.show(confirm).then(function () {
                vm.delete(id);
            }, function () {
                //cancelled action
            });
        };

        vm.delete = function (idToDelete) {
            DeviceService.deleteDevice(idToDelete)
            .then(function (response) {
                $mdToast.show(
                    $mdToast.simple().textContent('Device with id ' + idToDelete + ' was deleted')
                    .parent(vm.contentPlaceHolder)
                    .position('top right')
                    .toastClass('toast-success'));

                vm.dtInstance.reloadData();
            }, function (rejection) {
                $mdToast.show(
                    $mdToast.simple().textContent('Error on action: ' + rejection.statusText)
                    .parent(vm.contentPlaceHolder)
                    .position('top right')
                    .toastClass('toast-error'));
            });
        };        

        function loadData() {
            var defer = $q.defer();

            DeviceService.getAllDevices().success(function (devices) {
                defer.resolve(devices);
            });

            return defer.promise;
        }

        function drawComplete(row, data, dataIndex) {
            // Recompiling so we can bind Angular directive to the DT
            $compile(angular.element(row).contents())($scope);
        }
        //////////        
    }
})();