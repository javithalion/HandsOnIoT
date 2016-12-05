(function () {
    'use strict';

    angular
        .module('app.predictions', ['gridshore.c3js.chart'])
        .config(config);

    /** @ngInject */
    function config($stateProvider, $translatePartialLoaderProvider, msApiProvider, msNavigationServiceProvider) {

        $stateProvider
            .state('app.predictions', {
                url: '/Predictions',
                views: {
                    'content@app': {
                        templateUrl: 'app/main/predictions/_wizard/wizard.stepper.html',
                        controller: 'PredictionsController as vm'
                    }
                }
            })
        .state('app.predictions.devicesOnDuringDay', {
            url: '/DevicesOnDuringDay/:date',
            views: {
                'content@app': {
                    templateUrl: 'app/main/predictions/devicesOnDuringDay/devicesOnDuringDay.html',
                    controller: 'DevicesOnDuringDayController as vm'
                }
            }
        });

        

        //Trasnlation
        $translatePartialLoaderProvider.addPart('app/main/predictions/_wizard');
        $translatePartialLoaderProvider.addPart('app/main/predictions/devicesOnDuringDay');


        msNavigationServiceProvider.saveItem('Devices.Predictions', {
            title: 'Predictions',
            icon: 'icon-chart-areaspline',
            state: 'app.predictions',
            translate: 'PREDICTIONS.MENU.LABEL',
            weight: 2
        });

    }
})();