/// <reference path="lib/require.js" />
/// <reference path="lib/o.js" />
/// <reference path="lib/pure.js" />
requirejs.config({
    baseUrl: 'js/lib'
});
requirejs(['q', 'o', 'pure'], function (q, o, pure) {
    var getQueryString = function (field, url) {
        var href = url ? url : window.location.href;
        var reg = new RegExp('[?&]' + field + '=([^&#]*)', 'i');
        var string = reg.exec(href);
        return string ? string[1] : null;
    };

    var searchResultDirective = {
        'section#searchResult': {
            'searchResult<-': {
                'a#title@href': 'searchResult.Path',
                'a': 'searchResult.Title',
                'div#description': 'searchResult.Description'
            }
        }
    };

    var updateSearchResults = function (data) {
        document.querySelector('section#searchSpinner').hidden = true;
        $p('article#searchResults').render(data, searchResultDirective);
    };

    var searchInput = getQueryString('searchInput');
    if (searchInput != null) {
        document.querySelector('input#searchInput').textContent = searchInput;
        o("/api/Works/Search(input='" + searchInput + "')").get(updateSearchResults);
        document.querySelector('section#searchSpinner').hidden = false;
    }
});