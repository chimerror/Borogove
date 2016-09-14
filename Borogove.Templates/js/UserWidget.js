/// <reference path="./lib/require.js" />
/// <reference path="./lib/jquery-2.2.3.js" />
/// <reference path="./lib/pure.js" />
/// <reference path="./Common.js" />
requirejs(['../RequireConfig', 'jquery', 'q', 'o', 'pure', 'common'], function (config, jQuery, q, o, pure, common) {
    var informationDirective = {
        'img#userPicture@src': 'Picture',
        'div#userName': function (user) {
            var displayName = user.context.NickName || user.context.FullName || user.context.UserName || 'Nameless User'; // Last one should never happen.
            if (user.context.Email && user.context.Email != displayName) {
                displayName += ' (' + user.context.Email + ')'
            }
            else if (user.context.UserName && user.context.UserName != displayName) {
                displayName += ' (' + user.context.UserName + ')'
            }
            return displayName;
        }
    };

    var updateUserWidget = function (data) {
        if (jQuery.isPlainObject(data)) { // o.js returns an empty array for 204 NoContent
            $('div#userWidget').attr('hidden', false);
            $('div#userInformation').render(data, informationDirective)
            if (data.EmailVerified == false) {
                $('div#emailNotVerifiedWarning').attr('hidden', false);
            }
        }
        else {
            $('div#loginLink').attr('hidden', false);
        }
    }

    o(common.getRootUrl() + "api/Users/LoggedInUser").get(updateUserWidget);
});