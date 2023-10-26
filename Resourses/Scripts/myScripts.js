$(document).ready(function () {

    let requestFieldsTable = `
        <table class="table table-bordered" id="request-fileds">
            <tr>
                <td>
                    <button class="btn btn-outline-primary" name="search" id="search-btn">Search</button>
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
                    <button class="btn btn-outline-primary" name="report" id="report-btn">Download report</button>
                </td>
            </tr>
        </table>
    `;
    
    $("#request-container").html(requestFieldsTable);

    $("#filling").click( () => {
        findAllBooks();
    });

    $("#search-btn").click( () => {
        storeBookToTable();
    }); 

    $("#report-btn").click( () => {
        downloadXmlReport();
    }); 
});
    
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
                let edit  = "<button class='btn btn-primary' onclick='edit(" + item.bookId + ")'>Edit</button>";
                let del = "<button class='btn btn-primary' onclick='del(" + item.bookId + ")'>Delete</button>";

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
    let getBookById = 'http://localhost:5180/api/book/';

    if (id == null) {
        id = $("#id-field").val();
    }

    return new Promise(function(resolve, reject) {
        $.ajax({
            url: getBookById + id,
            method: 'GET',
            success: (data) => {
                resolve(data);
            },
            error: (error) => {
                reject(error);
            }
        });
    });
}

function storeBookToTable() {
    let book = resonseTable;
    findBookById()
        .then( data => {
            let edit  = "<button class='btn btn-primary' onclick='edit(" + data.bookId + ")'>Edit</button>";
            let del = "<button class='btn btn-primary' onclick='del(" + data.bookId + ")'>Delete</button>";
        
            book += `
                    <tr>
                         <td>` + data.bookId + `</td>` +
                        `<td>` + data.title + `</td>` +
                        `<td>` + data.year + `</td>` +
                        `<td>` + data.author.firstName + " " + data.author.lastName + `</td>` +
                        `<td>`+ data.category.name + `</td>`  +
                        `<td>`+ edit + `</td>` +
                        `<td>`+ del + `</td>` +
                    `</tr>`;
            book += `</table>`;
            $("#response-container").html(book);
        })
        .catch( error => {
            let message, modalClass;
            message = error.responseText;
            modalClass = "modal-error";
            $("#messageText").text(message);
            $("#messageModal").addClass(modalClass);
    
            $("#messageModal").modal("show");
        });
}

function downloadXmlReport() {
    window.open("http://localhost:5180/api/book/report");
}

function edit(id) {    
    findBookById(id)
    .then(function(data) {
        let modalClass = "modal-edit";

        $("#edit-title").val(data.title);
        $("#edit-year").val(data.year);
        $("#edit-author-firstName").val(data.author.firstName);
        $("#edit-author-lastName").val(data.author.lastName);
        $("#edit-category-name").val(data.category.name);

        $("#editModal").addClass(modalClass);
        $("#editModal").modal("show");
    })
    .catch(function(error) {
        console.error('Произошла ошибка:', error);
    });

    $("#saveChangesButton").click( () => {
        updateBook(id);
    });
}

function updateBook(id) {
    let updatedBook = {
        bookId: id,
        title: $("#edit-title").val(),
        year: $("#edit-year").val(),
        author: {
            firstName: $("#edit-author-firstName").val(),
            lastName: $("#edit-author-lastName").val()
        },
        category: {
            name: $("#edit-category-name").val()
        }
    };

    $.ajax({
        url: 'http://localhost:5180/api/book/edit',
        method: 'PUT',
        data: JSON.stringify(updatedBook),
        contentType: 'application/json',
        success: data => {
            let message, modalClass;
            message = "Операция выполнена успешно!";
            modalClass = "modal-success";
            $("#messageText").text(message);
            $("#messageModal").addClass(modalClass);
    
            $("#messageModal").modal("show");
            findAllBooks();
        },
        error: error => {
            let message, modalClass;
            message = error.responseText;
            modalClass = "modal-error";
            $("#messageText").text(message);
            $("#messageModal").addClass(modalClass);
    
            $("#messageModal").modal("show");
        }
    });
}

function del(id) {
    $.confirm({
        title: 'Are you sure to delete book!',
        content: 'Deletion!',
        buttons: {
            confirm: function () {
                deleteBookByDetails(id);
            },
            cancel: function () {
            }            
        }
    });
}
 
function deleteBookByDetails(id) {
    $.ajax({
        url: 'http://localhost:5180/api/book/delete/' + id,
        method: 'DELETE',
        contentType: 'application/json',
        success: data => {
            let message, modalClass;
            message = "Операция выполнена успешно!";
            modalClass = "modal-success";
            $("#messageText").text(message);
            $("#messageModal").addClass(modalClass);
    
            $("#messageModal").modal("show");
        },
        error: error => {
            let message, modalClass;
            message = error.responseText;
            modalClass = "modal-error";
            $("#messageText").text(message);
            $("#messageModal").addClass(modalClass);
    
            $("#messageModal").modal("show");
        }
    });
}