// JavaScript source code
/// <reference path="./lib/require.js" />
/// <reference path="./lib/js.cookie.js" />
define(['../RequireConfig', 'js.cookie'], function (config, cookie) {
    return {
        getRootUrl: function () {
            return /^https?:\/\/.*?\//.exec(window.location.href);
        },

        getQueryString: function (field, url) {
            var href = url ? url : window.location.href;
            var reg = new RegExp('[?&]' + field + '=([^&#]*)', 'i');
            var string = reg.exec(href);
            return string ? string[1] : null;
        },

        getWorkIdentifier: function () {
            return document.querySelector('input#workIdentifier').getAttribute('value');
        },

        getLoggedInUser: function () {
            var rawJwt = cookie.get('BorogoveProfile');
            var regMatches = /^.+\.([A-Za-z0-9+\/=]+)\..+$/.exec(rawJwt);
            return regMatches ? JSON.parse(atob(regMatches[1])) : null;
        }
    }
});
