module app.shared {

    export class NavigationComponent implements ng.IComponentOptions {

        public bindings: any;
        public controller: any;
        public templateUrl: string;
         
        constructor() {
            this.bindings = {
            };
            this.controller = NavigationCtrl;
            this.templateUrl = 'app/shared/navigation/navigationLayout.html';
        }
    }

    angular.module("app").component('navigation', NavigationComponent);
}