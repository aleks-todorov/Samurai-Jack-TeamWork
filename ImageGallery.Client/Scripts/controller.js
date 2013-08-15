/// <reference path="jquery-2.0.2.js" />
/// <reference path="class.js" />
/// <reference path="persister.js" />
/// <reference path="ui.js" />

var controllers = (function () {

    var rootUrl = "http://localhost:20329/api";

    var Controller = Class.create({
       
        init: function () {
            this.persister = new persisters.get(rootUrl);
        },

        loadUI: function (selector) {
            //TODO
        }
    });

    return {
        get: function () {
            return new Controller();
        }
    };

}());