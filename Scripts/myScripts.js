$(document).ready(function () {
    $("#temp").click(function () {
        let str = $("#p1").text();

        $("#p1").text($("#in").val());
        $("#in").val(str);

        $("#div2").text($("#div1").text());
        $("#div1").text("");
    });
});