// JavaScript source code
/// <reference path="./lib/require.js" />
/// <reference path="./lib/js.cookie.js" />
/// <reference path="./lib/TextEncoderLite-1.0.0.js" />
/// <reference path="./lib/base64-js-1.1.2.js" />
define(['../RequireConfig', 'js.cookie', 'textencoderlite', 'base64'], function (config, cookie, tel, b64) {
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
            var base64Bytes = regMatches ? b64.toByteArray(regMatches[1]) : null;
            var decoder = new tel.TextDecoderLite('utf-8');
            var jsonString = base64Bytes ? decoder.decode(base64Bytes) : null;
            return jsonString ? JSON.parse(jsonString) : null;
        }
    }
});
