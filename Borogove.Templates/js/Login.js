/// <reference path="./lib/require.js" />
/// <reference path="./Common.js" />
/// <reference path="https://cdn.auth0.com/js/lock-9.1.min.js" />
requirejs.config({
    baseUrl: '/js/lib',
    paths: {
        'common': '../Common'
    }
});
requirejs(['common'], function (common) {
    var lock = new Auth0Lock('bZmoX9XKDg68wfSAqbZK4Km8v76ctkO3', 'chimerror.auth0.com');

    var getState = function () {
        var returnUrl = common.getQueryString('ReturnUrl');
        return returnUrl ? "ru=" + returnUrl : null;
    }

    lock.show({
        container: 'loginContainer',
        callbackURL: common.getRootUrl() + 'LoginCallback.ashx',
        responseType: 'code',
        authParams: {
            scope: 'openid email name nickname picture',
            state: getState()
        }
    });
});
