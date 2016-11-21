/// <reference path="../hapi/hapi.d.ts" />
/// <reference path="hapi-decorators.d.ts" />
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import * as hapi from 'hapi';
import { controller, get, post, put, cache, config, route, validate } from 'hapi-decorators';
let TestController = class TestController {
    getHandler(request, reply) {
        reply({ success: true });
    }
    postHandler(request, reply) {
        reply({ success: true });
    }
    putHandler(request, reply) {
        reply({ success: true });
    }
    deleteHandler(request, reply) {
        reply({ success: true });
    }
};
__decorate([
    get('/'),
    config({
        auth: false
    }),
    cache({
        expiresIn: 42000
    }),
    validate({
        payload: false
    })
], TestController.prototype, "getHandler", null);
__decorate([
    post('/')
], TestController.prototype, "postHandler", null);
__decorate([
    put('/{id}')
], TestController.prototype, "putHandler", null);
__decorate([
    route('delete', '/{id}')
], TestController.prototype, "deleteHandler", null);
TestController = __decorate([
    controller('/test')
], TestController);
const server = new hapi.Server();
server.route(new TestController().routes());
