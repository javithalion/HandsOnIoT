(function () {
    'use strict';

    angular
        .module('app.devices', ['datatables'])
        .config(config);

    /** @ngInject */
    function config($stateProvider, $translatePartialLoaderProvider, msApiProvider, msNavigationServiceProvider) {

        // State
        $stateProvider
            .state('app.devices', {
                url: '/devices',
                views: {
                    'content@app': {
                        templateUrl: 'app/main/devices/list/list.html',
                        controller: 'DeviceListController as vm'
                    }
                }
            })
        .state('app.devices.detail', {
            url: '/:id',
            views: {
                'content@app': {
                    templateUrl: 'app/main/devices/detail/detail.html',
                    controller: 'DeviceDetailsController as vm'
                }
            }
        }).state('app.devices.create', {
            url: '/New',
            views: {
                'content@app': {
                    templateUrl: 'app/main/devices/detail/detail.html',
                    controller: 'DeviceDetailsController as vm'
                }
            }
        });

        //Translation
        $translatePartialLoaderProvider.addPart('app/main/devices/list');
        $translatePartialLoaderProvider.addPart('app/main/devices/detail');

        // Navigation
        msNavigationServiceProvider.saveItem('Devices', {
            title: 'Devices',
            group: true,
            weight: 2
        });

        msNavigationServiceProvider.saveItem('Devices.Manage', {
            title: 'Manage',
            icon: 'icon-monitor',
            state: 'app.devices',
            translate: 'DEVICES.MENU_LABEL',
            weight: 1
        });

    }
})();