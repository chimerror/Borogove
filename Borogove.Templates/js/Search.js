/// <reference path="./lib/require.js" />
/// <reference path="./lib/o.js" />
/// <reference path="./lib/pure.js" />
/// <reference path="./Common.js" />
document.querySelector('div#searchNoScript').hidden = true;
requirejs.config({
    baseUrl: '/js/lib',
    paths: {
        'common': '../Common'
    },
});
requirejs(['q', 'o', 'pure', 'common'], function (q, o, pure, common) {
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
    var searchInput = common.getQueryString('searchInput');
    if (searchInput != null) {
        document.querySelector('input#searchInput').value = searchInput;
        o(common.getRootUrl() + "api/Works/Search(input='" + searchInput + "')").get(updateSearchResults);
        document.querySelector('section#searchSpinner').hidden = false;
    }
});
