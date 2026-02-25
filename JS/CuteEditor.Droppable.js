$(document).ready(function () {
    $('#%= txtVideos.ClientID %>').droppable({
        //accept: ".draggable-item",
        tolerance: "pointer",
        greedy: true,
        drop: function (event, ui) {
            //$(this).insertAtCaret(ui.draggable.text());
            //$(this).insertAtCaret(ui.draggable.context.innerHTML);
            $(this).insertAtCaret(ui.draggable.context.lastChild.outerHTML);
            //console.log(ui.draggable);
            //console.log(ui.draggable.context.innerHTML);
            //alert('Dropped!!!');
        }
    });
});