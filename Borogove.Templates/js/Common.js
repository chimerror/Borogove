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

        decodeUtf8Base64String: function (base64String) {
            var length = base64String.length;
            var paddingNeeded = 0
            if ((base64String[length - 1] != '=') && (length % 4 > 0)) {
                paddingNeeded = 4 - (length % 4);
            }

            var paddedString = base64String;
            for (i = 0; i < paddingNeeded; i++) {
                paddedString += '=';
            }

            var base64Bytes = b64.toByteArray(paddedString);
            var decoder = new tel.TextDecoderLite('utf-8');
            return decoder.decode(base64Bytes);
        }
    }
});
