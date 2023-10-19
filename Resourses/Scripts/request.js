$(document).ready(function () {
    $("#test-btn").click(function () {
        $.ajax({
            type: "GET",
            url: "http://localhost:5180/api/book/all",
            success: function (data) {
                console.log(data);                
            },
            error: function (data) {
                console.log("error");
            }
        });

        $.ajax({
            type: "GET",
            url: "http://localhost:5180/api/book/1",
            success: function (data) {
                console.log(data);  
            },
            error: function (data) {
                console.log("error");
            }
        });

        var bookData = {
            title: "Название книги",
            year: 2023,
            author: {
                firstName: "имя",
                lastName: "фамилие"
            },
            category: {
                name: "название категории"
            }
        };

        var jsonData = JSON.stringify(bookData);

        $.ajax({
            url: 'http://localhost:5180/api/book/new',
            method: 'POST',
            contentType: 'application/json',
            data: jsonData,
            success: function (response) {
                console.log(response);
            },
            error: function (error) {
                console.error(error);
            }
        });

        var bookData = {
            title: "название книги",
            year: 2023,
            author: {
                firstName: "имя",
                lastName: "фамилие"
            },
            category: {
                name: "название категории"
            }
        };

        $.ajax({
            url: 'http://localhost:5180/api/book/edit', 
            type: 'PUT', 
            contentType: 'application/json',
            data: JSON.stringify(bookDTO),
            success: function (data) {
                console.log(data);
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log("error");
            }
        });

        var bookDetails = {
            bookName: "Название книги",
            authorName: "Имя автора",
            categoryName: "Категория книги"
        };

        $.ajax({
            url: 'http://localhost:5180/api/book/delete', 
            type: 'DELETE',
            contentType: 'application/json',
            data: JSON.stringify(bookDetails),
            success: function (data) {
                console.log(data);
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log("error");
            }
        });
    });
});