
function mainMenuController() {
    this.menuEntries = [
        { text: "Devices", link: "/Devices/Index", materialIcon: "important_devices" },
        { text: "User settings", link: "/Account/Login", materialIcon: "face" }];
}

angular.module('javithalion.IoT.webSite')
.component('mainMenu', {

    templateUrl: 'js/app/components/mainMenu/mainMenuTemplate.html',
    controller: mainMenuController,

});