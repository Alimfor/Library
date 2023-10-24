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

    let requestFieldsTable = `
        <table class="table table-bordered" id="request-fileds">
            <tr>
                <td>
                    <button class="btn btn-outline-primary" name="search" id="search-btn">Send</button>
                    <br>
                    <div class="form-group">
                        <label for="for-id">Input id:</label>
                        <input type="number" class="form-control" name="id-field" id="id-field">
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
                    <button type="submit" class="btn btn-outline-primary" name="send" id="report">Download report</button>
                </td>
            </tr>
        </table>
    `;

    $("#request-container").html(requestFieldsTable);
    
    let resonseTable = `
        <table class="table table-bordered" id="response-table">
            <tr>
                <td>Id</td>
                <td>Title</td>
                <td>Year</td>
                <td>Author full name</td>
                <td>Category</td>
                <td>Edit</td>
                <td>Delete</td>
            </tr>
    `;


    function findAllBooks() {
            let list = resonseTable;
        
            $.ajax({
                url: 'http://localhost:5180/api/book/all',
                method: 'GET',
                success: function(data) {
                    $.each(data, (i,item) => {
                        let bookData = {
                            bookName: item.title,
                            authorName: item.author.firstName,
                            categoryName: item.category.name
                        };

                        let edit  = "<button class='btn btn-primary' onclick='edit(" + 5 + ")'>Edit</button>";
                        let del = "<button class='btn btn-primary' onclick='del(" + JSON.stringify(bookData) + ")'>Delete</button>";
    
                        list += `
                            <tr>
                                 <td>` + (i + 1) + `</td>` +
                                `<td>` + item.title + `</td>` +
                                `<td>` + item.year + `</td>` +
                                `<td>` + item.author.firstName + " " + item.author.lastName + `</td>` +
                                `<td>`+ item.category.name + `</td>` +
                                `<td>`+ edit + `</td>` +
                                `<td>`+ del + `</td>` +
                            `</tr>`;
    
                    });
                    list += `</table>`;
                    $("#response-container").html(list);
                },
            
                error: function(error) {
                    console.error('Произошла ошибка:', error);
                }
            });
    }

    function findBookById(id) {
            let book = resonseTable;
    
            let getBookById = 'http://localhost:5180/api/book/';

            if (id == null) {
                id = $("#id-field").val();
            }
    
            console.log(id);
            return new Promise(function(resolve, reject) {
                $.ajax({
                    url: getBookById + id,
                    method: 'GET',
                    success: (data) => {
                        let bookData = {
                            title: data.title,
                            authorName: data.author.firstName,
                            categoryName: data.category.name
                        }

                        let edit  = "<button class='btn btn-primary' onclick='edit(" + id + ")'>Edit</button>";
                        let del = "<button class='btn btn-primary delete-button' data-book='" + JSON.stringify(bookData) + "'>Delete</button>";

                        book += `
                                <tr>
                                     <td>` +  1 + `</td>` +
                                    `<td>` + data.title + `</td>` +
                                    `<td>` + data.year + `</td>` +
                                    `<td>` + data.author.firstName + " " + data.author.lastName + `</td>` +
                                    `<td>`+ data.category.name + `</td>`  +
                                    `<td>`+ edit + `</td>` +
                                    `<td>`+ del + `</td>` +
                                `</tr>`;
                        book += `</table>`;
                        $("#response-container").html(book);

                    },
                
                    error: (error) => {
                        console.error('Произошла ошибка:', error);
                    }
                });
            });
    }



    $("#filling").click( () => {
        findAllBooks();
    });

    $("#search-btn").click( () => {
        findBookById();
    }); 
});

function edit(id) {    
    findBookById(id)
        .then(function(data) {
            $("#title").val(data.title);
            $("#year").val(data.year);
            $("#author-first-name").val(data.author.firstName);
            $("#author-last-name").val(data.author.lastName);
            $("#category-name").val(data.category.name);
    
            $("#exampleModal").modal("show");
        })
        .catch(function(error) {
            console.error('Произошла ошибка:', error);
        });
}


function del(bookData) {
    $.confirm({
        title: 'Are you sure to delete book!',
        content: 'Deletion!',
        buttons: {
            confirm: function () {
                deleteBookByDetails(bookData);
            },
            cancel: function () {
                //$.alert('cancel delete!');
            }            
        }
    });
}
 


function deleteBookByDetails(bookData) {
    $.ajax({
        url: 'http://localhost:5180/api/book/delete',
        method: 'DELETE',
        data: JSON.stringify(bookData),
        contentType: 'application/json',
        success: data => {
            var message, modalClass;
            message = "Операция выполнена успешно!";
            modalClass = "modal-success";
            $("#messageText").text(message);
            $("#messageModal").addClass(modalClass);
    
            $("#messageModal").modal("show");
        },
        error: error => {
            var message, modalClass;
            message = error.responseText;
            modalClass = "modal-error";
            $("#messageText").text(message);
            $("#messageModal").addClass(modalClass);
    
            $("#messageModal").modal("show");
        }
    });
}

    // $("#showModal").click(function () {
    //     var isSuccess = true; 

    //     var message, modalClass;
    //     if (isSuccess) {
    //         message = "Операция выполнена успешно!";
    //         modalClass = "modal-success";
    //     } else {
    //         message = "Произошла ошибка при выполнении операции.";
    //         modalClass = "modal-error";
    //     }

    //     // Установите сообщение и класс модального окна.
    //     $("#messageText").text(message);
    //     $("#messageModal").addClass(modalClass);

    //     // Покажите модальное окно.
    //     $("#messageModal").modal("show");
    // });