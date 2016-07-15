/// <reference path="./lib/require.js" />
/// <reference path="./lib/lock-9.2.1.min.js" />
/// <reference path="./Common.js" />
requirejs(['../RequireConfig', 'auth0-lock', 'common'], function (config, auth0Lock, common) {
    document.querySelector('div#loginNoScript').hidden = true;

    var clientId = document.querySelector('input#authorizationClientId').getAttribute('value');
    var domain = document.querySelector('input#authorizationDomain').getAttribute('value');

    if (clientId && domain) {
        var lock = new auth0Lock(clientId, domain);

        var getState = function () {
            var returnUrl = common.getQueryString('ReturnUrl');
            return returnUrl ? "ru=" + returnUrl : null;
        }

        lock.show({
            container: 'loginContainer',
            callbackURL: common.getRootUrl() + 'LoginCallback.ashx',
            responseType: 'code',
            authParams: {
                scope: 'openid email name username nickname picture',
                state: getState()
            }
        });
    }
});
