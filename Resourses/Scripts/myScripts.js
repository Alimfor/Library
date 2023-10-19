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
        // $.ajax({
        //     type: "GET",
        //     url: "http://localhost:5180/api/author/authors_select",
        //     success: function (data) {
        //         $("#author_select").empty();

        //         for (var i = 0; i < data.length; i++) {
        //             $("#author_select").append('<option value=' + data[i].id + '>' + data[i].name + '</option>');
        //         }
        //     },
        //     error: function (data) {
        //         console.log("error");
        //     }
        // });

        // $.ajax({
        //     type: "GET",
        //     url: "http://localhost:5180/api/book/category_select",
        //     success: function (data) {
        //         $("#category_select").empty();

        //         for (var i = 0; i < data.length; i++) {
        //             $("#category_select").append('<option value=' + data[i].id + '>' + data[i].name + '</option>');
        //         }
        //     },
        //     error: function (data) {
        //         console.log("error");
        //     }
        // });
$(document).ready(function () {
    $("#filling").click(function () {
       
        var htmlCode = `
        <table class="table table-bordered">
            <tr>
                <td>
                    <button type="submit" class="btn btn-outline-primary" name="send" id="search">Send</button>
                    <br>
                    <div class="form-group">
                        <label for="for-id">Input id:</label>
                        <input type="text" class="form-control" name="for-id" id="for-id">
                    </div>
                </td>
                <td>
                    <div class="form-group">
                        <label for="title">Title:</label>
                        <input type="text" class="form-control" name="title" id="title">
                    </div>
                    <div class="form-group">
                        <label for="year">Year:</label>
                        <input type="number" class="form-control" name="year" id="year">
                    </div>
                    <div class="form-group">
                        <label for="author-first-name">Author first name:</label>
                        <input type="text" class="form-control" name="author-first-name" id="author-first-name">
                    </div>
                    <div class="form-group">
                        <label for="author-last-name">Author last name:</label>
                        <input type="text" class="form-control" name="author-last-name" id="author-last-name">
                    </div>
                    <div class="form-group">
                        <label for="category-name">Category name:</label>
                        <input type="text" class="form-control" name="category-name" id="category-name">
                    </div>
                </td>
                <td>
                    <button type="submit" class="btn btn-primary" name="send" id="report">Download report</button>
                </td>
            </tr>

        `;

//         <tr>
//         <td colspan="3" rowspan="3" class="empty-space"></td>
//     </tr>
// </table>

        $.ajax({
            url: 'http://localhost:5180/api/book/all',
            method: 'GET',
            success: function(data) {
                $.each(data, (i,item) => {
                    htmlCode += `
                        <tr>
                            <td>` + item.title + `</td>` +
                            `<td>` + item.year + `</td>` +
                            `<td>` + item.author.firstName + " " + item.author.firstName `</td>`
                            `<td>`+ item.category.name + `</td>
                        </tr>`;

                });
                htmlCode += `</table>`;
            },
        
            error: function(error) {
                console.error('Произошла ошибка:', error);
            }
        });

        $("#tableBook").html(htmlCode);
    });
});

        