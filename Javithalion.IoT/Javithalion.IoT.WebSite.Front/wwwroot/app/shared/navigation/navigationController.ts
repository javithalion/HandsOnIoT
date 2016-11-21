module app.shared {

    export class NavigationCtrl {

        static $inject = ['$scope', '$http'];

        public MenuEntries: any[];


        constructor(protected $scope: ng.IScope, protected $http: ng.IHttpService) {
            this.getAllMenuEntries();
        }

        private getAllMenuEntries() {

            this.$http
                .get('<%=ResolveUrl("API/Menu/")%>')
                .then((response: ng.IHttpPromiseCallbackArg<any>) => {
                    this.MenuEntries = response.data;
                });
        }
    }

    angular.module("app").controller('navigationCtrl', NavigationCtrl);
}

