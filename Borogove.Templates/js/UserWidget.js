/// <reference path="./lib/require.js" />
/// <reference path="./lib/jquery-2.2.3.js" />
/// <reference path="./lib/pure.js" />
/// <reference path="./Common.js" />
requirejs(['../RequireConfig', 'jquery', 'pure', 'common'], function (config, jQuery, pure, common) {
    var informationDirective = {
        'img#userPicture@src': 'picture',
        'div#userName': function (user) {
            var displayName = user.context.nickname || user.context.name || user.context.username || 'Nameless User'; // Last one should never happen.
            if (user.context.email && user.context.email != displayName) {
                displayName += ' (' + user.context.email + ')'
            }
            else if (user.context.username && user.context.username != displayName) {
                displayName += ' (' + user.context.username + ')'
            }
            return displayName;
        }
    };

    var loggedInUser = common.getLoggedInUser();

    if (loggedInUser) {
        $('div#userWidget').attr('hidden', false);
        $('div#userInformation').render(loggedInUser, informationDirective)
        if (loggedInUser.email_verified == false) {
            $('div#emailNotVerifiedWarning').attr('hidden', false);
        }
    }
    else {
        $('div#loginLink').attr('hidden', false);
    }
});