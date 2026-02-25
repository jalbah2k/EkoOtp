$(document).ready(function () {
    $('table.table-heading tr:first-child td').each(function () {
        $(this).replaceWith('<th>' + $(this).text() + '</th>');
    });
});
