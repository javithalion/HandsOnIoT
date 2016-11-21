(function () {
    'use strict';

    angular
        .module('fuse')
        .controller('MainController', MainController);

    /** @ngInject */
    function MainController($scope, $rootScope, $mdToast) {
        // Data

        //////////

        // Remove the splash screen
        $scope.$on('$viewContentAnimationEnded', function (event) {
            if (event.targetScope.$id === $scope.$id) {
                $rootScope.$broadcast('msSplashScreen::remove');
            }
        });

        $scope.$on('$generalErrorNotification', function (event, data) {            
            showSimpleToast(data, 'toast-error');
        });

        $scope.$on('$generalSuccessNotification', function (event, data) {
            showSimpleToast(data, 'toast-success');
        });

        function showSimpleToast(message, cssClass) {
            $mdToast.show(
               $mdToast.simple().textContent(message)
               .position('top right')
               .toastClass(cssClass));
        }
    }
})();