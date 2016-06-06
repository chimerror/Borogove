/// <reference path="./lib/require.js" />
/// <reference path="./lib/o.js" />
/// <reference path="./lib/pure.js" />
document.querySelector('div#searchNoScript').hidden = true;
requirejs.config({
    baseUrl: 'js/lib'
});
requirejs(['q', 'o', 'pure'], function (q, o, pure) {

    var getRootUrl = function () {
        var reg = new RegExp(/^https?:\/\/.*?\//);
        return reg.exec(window.location.href);
    }

    var getQueryString = function (field, url) {
        var href = url ? url : window.location.href;
        var reg = new RegExp('[?&]' + field + '=([^&#]*)', 'i');
        var string = reg.exec(href);
        return string ? string[1] : null;
    };

    var searchResultDirective = {
        'section.searchResult': {
            'searchResult<-': {
                'a.searchResultTitle@href': 'searchResult.Path',
                'a': 'searchResult.Title',
                'div.searchResultDescription': 'searchResult.Description'
            }
        }
    };

    var updateSearchResults = function (data) {
        document.querySelector('section#searchSpinner').hidden = true;
        if (data.length == 0) {
            document.querySelector('div#searchNoResults').hidden = false;
        }
        else {
            $p('article#searchResults').render(data, searchResultDirective);
        }
    };

    document.querySelector('form#searchForm').removeAttribute('action');
    var searchInput = getQueryString('searchInput');
    if (searchInput != null) {
        document.querySelector('input#searchInput').value = searchInput;
        o(getRootUrl() + "api/Works/Search(input='" + searchInput + "')").get(updateSearchResults);
        document.querySelector('section#searchSpinner').hidden = false;
    }
});
