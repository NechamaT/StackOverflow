
$(() => {

    let id = $("#like-question").data("question-id");
    
    $("#like-question").on('click', function () {
        console.log(id);
        $.post("/question/Update", { id }, function (id) {
            $("#like-question").attr('class', "oi oi-heart text-danger");
        });
    });

    setInterval(() => {
        $.get("/question/GetLikes", { id }, function (likes) {
            $("#likes-count").text(likes);
        })
    }, 500);
});