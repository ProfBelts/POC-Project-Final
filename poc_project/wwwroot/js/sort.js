$(document).ready(function () {
    var sortOrder = 'asc'; // Default sort order

    $('#relevanceScoreHeader').click(function () {
        var rows = $('#responsesTable tbody tr').get();

        rows.sort(function (a, b) {
            var keyA = parseFloat($(a).find('.score-value').text());
            var keyB = parseFloat($(b).find('.score-value').text());

            if (sortOrder === 'asc') {
                return keyA < keyB ? -1 : keyA > keyB ? 1 : 0;
            } else {
                return keyA > keyB ? -1 : keyA < keyB ? 1 : 0;
            }
        });

        // Reverse the sort order for the next click
        sortOrder = (sortOrder === 'asc') ? 'desc' : 'asc';

        // Update the sort indicator (arrow up/down)
        var header = $(this);
        header.html("Relevance Score " + (sortOrder === 'asc' ? "&#9650;" : "&#9660;"));

        // Append the sorted rows back into the table
        $.each(rows, function (index, row) {
            $('#responsesTable').children('tbody').append(row);
        });
    });
});