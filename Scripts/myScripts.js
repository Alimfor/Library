//$(document).ready(function () {
//    $("#temp").click(function () {
//        let str = $("#p1").text();

//        $("#p1").text($("#in").val());
//        $("#in").val(str);

//        $("#div2").text($("#div1").text());
//        $("#div1").text("");
//    });
//});

//let arr = [
//    {
//        "id" : "1",
//        "name" : "Almaty"
//    },
//    {
//        "id": "2",
//        "name": "Astana"
//    }
//];

//$(document).ready(function () {
//    $("#temp").click(() => {
//        $('#sel1').empty();

//        for (var i = 0; i < arr.length; i++) {
//            $("#sel1").append('<option value=' + arr[i].id + '>' + arr[i].name + '</option>');
//        };
//    });

//});

$(document).ready(function () {
    $("#filling").click(function () {
        $.ajax({
            type: "GET",
            url: "http://localhost:5180/api/author/authors_select",
            success: function (data) {
                $("#author_select").empty();

                for (var i = 0; i < data.length; i++) {
                    $("#author_select").append('<option value=' + data[i].id + '>' + data[i].name + '</option>');
                }
            },
            error: function (data) {
                console.log("error");
            }
        });

        $.ajax({
            type: "GET",
            url: "http://localhost:5180/api/book/category_select",
            success: function (data) {
                $("#category_select").empty();

                for (var i = 0; i < data.length; i++) {
                    $("#category_select").append('<option value=' + data[i].id + '>' + data[i].name + '</option>');
                }
            },
            error: function (data) {
                console.log("error");
            }
        });

        let html;



        $("#tableBook").html(html);
    });
});
