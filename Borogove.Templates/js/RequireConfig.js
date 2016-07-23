/// <reference path="./lib/require.js" />
requirejs.config({
    baseUrl: '/js/lib',
    paths: {
        'jquery': 'jquery-2.2.3',
        'common': '../Common',
        'config': '../RequireConfig',
        'auth0-lock': 'lock-9.2.1.min', // Auth0 will only load if it's explicitly named auth0-lock
        'textencoderlite': 'TextEncoderLite-1.0.0',
        'base64': 'base64-js-1.1.2',
    },
    shim: {
        'pure': {
            deps: ['jquery']
        }
    }
});