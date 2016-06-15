/// <reference path="./lib/require.js" />
/// <reference path="./Common.js" />
/// <reference path="https://cdn.auth0.com/js/lock-9.1.min.js" />
document.querySelector('div#loginNoScript').hidden = true;
requirejs.config({
    baseUrl: '/js/lib',
    paths: {
        'common': '../Common'
    }
});
requirejs(['common'], function (common) {
    var clientId = document.querySelector('input#authorizationClientId').getAttribute('value');
    var domain = document.querySelector('input#authorizationDomain').getAttribute('value');

    if (clientId && domain) {
        var lock = new Auth0Lock(clientId, domain);

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
    }
});
