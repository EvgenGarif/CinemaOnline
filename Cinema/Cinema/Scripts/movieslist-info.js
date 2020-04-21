$(document).ready(
    function () {
        var termInput = $(".js-search-film-term")[0];
        if (termInput === null || termInput === 'undefined') {
            return;
        }
        var searchResultTemplateContainer = $(".js-search-result-item")[0].innerHTML;
        var resultTemplate = Handlebars.compile(searchResultTemplateContainer);

        Handlebars.registerHelper('for', function (from, to, incr, block) {
            var accum = '';
            for (var i = from; i <= to; i += incr)
                accum += block.fn(i);
            return accum;
        });

        $(".js-search-film-submit").on('click',
            function () {
                searchFilm(termInput.value);
            });

        function searchFilm(currentTerm) {
            var reqestModel = {
                term: currentTerm
            };

            $.ajax({
                url: "/tickets/SearchFilms",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json;charset=utf-8',
                data: JSON.stringify(reqestModel)
            }).done(function (result) {
                var resultHtml = resultTemplate(result);
                $(".js-search-result-container").html(resultHtml);
            }).fail(function () {
                alert("Search request processing failed. Please, contact system administrator.");
            });
        }

        searchFilm("");
    })