(function () {
    'use strict';

    angular
        .module('app.predictions', [])
        .config(config);

    /** @ngInject */
    function config($stateProvider, $translatePartialLoaderProvider, msApiProvider, msNavigationServiceProvider) {       
        
        $stateProvider
            .state('app.predictions', {
                url: '/predictions',
                views: {
                    'content@app': {
                        templateUrl: 'app/main/predictions/wizard.step1.html',
                        controller: 'PredictionsController as vm' 
                    }                    
                }
            }); 
       
        //Trasnlation
        $translatePartialLoaderProvider.addPart('app/main/predictions');


        msNavigationServiceProvider.saveItem('Devices.Predictions', {
            title: 'Predictions',
            icon: 'icon-chart-areaspline',
            state: 'app.predictions',
            translate: 'PREDICTIONS.MENU_LABEL',
            weight: 2
        });
        
    }
})();