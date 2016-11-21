(function ()
{
    'use strict';

    /**
     * Main module of the Fuse
     */
   var app = angular
        .module('fuse', [

            // Core
            'app.core',

            // Navigation
            'app.navigation',

            // Toolbar
            'app.toolbar',

            // Quick Panel
            'app.quick-panel',

            // Sample
            'app.sample',

            // devices
            'app.devices',

            'app.predictions'
        ]);
})();