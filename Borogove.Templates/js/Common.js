// JavaScript source code
/// <reference path="./lib/require.js" />
define({
    getRootUrl: function () {
        var reg = new RegExp(/^https?:\/\/.*?\//);
        return reg.exec(window.location.href);
    },

    getQueryString: function (field, url) {
        var href = url ? url : window.location.href;
        var reg = new RegExp('[?&]' + field + '=([^&#]*)', 'i');
        var string = reg.exec(href);
        return string ? string[1] : null;
    },

    getWorkIdentifier: function() {
        return document.querySelector('input#workIdentifier').getAttribute('value');
    }
})
