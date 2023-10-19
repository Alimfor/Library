using Library.DTO;
using Library.Models;
using Library.Services;
using Library.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Route("api/book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookService _bookService;
        private readonly AuthorService _authorService;
        private readonly CategoryService _categoryService;

		private const string GET_ALL_BOOKS = "all";
		private const string GET_BOOK_BY_ID = "book/{id}";
		private const string POST_SAVE_BOOK = "new";
		private const string PUT_UPDATE_BOOK = "edit";
		private const string DELETE_BOOK_BY_BOOK_DETAILS = "delete";

        public BookController(BookService bookService, CategoryService categoryService, AuthorService authorService)
        {
	        _bookService = bookService;
	        _categoryService = categoryService;
	        _authorService = authorService;
        }

        [Route(GET_ALL_BOOKS)]
        public IActionResult GetAllBooks()
        {
	        var operationResult = _bookService.GetAllBooks();
	        var result = operationResult.result;
	        var books = operationResult.data;

	        if (!books.Any() && result.code == 200)
	        {
		        return Ok(Enumerable.Empty<Book>());
	        }

	        var operationResults  = books.Select(book => book
		        .ToBookDto(_authorService, _categoryService)
	        );

	        var results = operationResults.Select(operateResult => operateResult.result);
	        var bookDtos = operationResults.Select(operateResult => operateResult.data);

	        var hasNoOne200Code = results.Any(res => res.code != 200);

	        return hasNoOne200Code 
		        ? StatusCode(500, "Internal Server Error") 
		        : ResultState(result,bookDtos);
        }

		[Route(GET_BOOK_BY_ID)]
		public IActionResult GetBookById(int id)
        {
			var bookOperationResult = _bookService.GetBookById(id);
			var result = bookOperationResult.result;
			var book = bookOperationResult.data;

			if (result.code == 400)
				return StatusCode(result.code, result.message);

			var operationResult = book.ToBookDto(_authorService, _categoryService);

			var results = operationResult.result;
			var bookDto = operationResult.data;

			var hasNoOne200Code = results.code != 200;

			return hasNoOne200Code 
				? StatusCode(500, "Internal Server Error") 
				: ResultState(result,bookDto);
        }

        [HttpPost,Route(POST_SAVE_BOOK)]
        public IActionResult AddBook(BookDTO? bookDto)
        {
	        if (bookDto == null)
		        return StatusCode(400, "wrong request!");
	        
	        var authorOperationResult =  _authorService.GetAuthorId(bookDto.author.firstName, bookDto.author.lastName);
	        var categoryOperationResult = _categoryService.GetCategoryIdByName(bookDto.category.name);
	        var authorResult = authorOperationResult.result;
	        var categoryResult = categoryOperationResult.result;

	        if (authorResult.code != 200)
		        return StatusCode(authorResult.code, authorResult.message);
	        if (categoryResult.code != 200)
		        return StatusCode(categoryResult.code, categoryResult.message);
	        
	        var authorId = authorOperationResult.data;
	        var categoryId = categoryOperationResult.data;
	        
	        var result = _bookService.SaveBook(bookDto.ToBook(authorId,categoryId));

	        return ResultState<>(result,null);
        }

        [HttpPut,Route(PUT_UPDATE_BOOK)]
        public IActionResult UpdateBook(BookDTO? bookDto)
        {
	        if (bookDto == null)
		        return StatusCode(400, "wrong request!");
	        
	        var authorOperationResult =  _authorService.GetAuthorId(bookDto.author.firstName, bookDto.author.lastName);
	        var categoryOperationResult = _categoryService.GetCategoryIdByName(bookDto.category.name);
	        var authorResult = authorOperationResult.result;
	        var categoryResult = categoryOperationResult.result;

	        if (authorResult.code != 200)
		        return StatusCode(authorResult.code, authorResult.message);
	        if (categoryResult.code != 200)
		        return StatusCode(categoryResult.code, categoryResult.message);
	        
	        var authorId = authorOperationResult.data;
	        var categoryId = categoryOperationResult.data;
	        
	        var result = _bookService.UpdateBook(bookDto.ToBook(authorId,categoryId));
	        
	        return ResultState<>(result,null);
        }

        [HttpDelete,Route(DELETE_BOOK_BY_BOOK_DETAILS)]
        public IActionResult DeleteBook(BookDetails bookDetails)
        {
	        var result = _bookService.DeleteBook(
		        bookDetails.bookName, bookDetails.authorName, bookDetails.categoryName
		        );
	        
	        return ResultState<>(result,null);
        }
        
        private IActionResult ResultState<T>(Result result,T data)
        {
	        return result.code switch
	        {
		        200 => Ok(data == null ? result.message : data),
		        400 => BadRequest(result.message),
		        500 => StatusCode(500, result.message),
		        _ => StatusCode(result.code, result.message)
	        };
        }
    }
}

/*
 
 * CREATE TABLE author (
	author_id INT IDENTITY PRIMARY KEY,
	last_name NVARCHAR(20) NOT NULL,
	first_name NVARCHAR(20) NOT NULL
);

CREATE TABLE category (
	category_id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(20) NOT NULL
);

CREATE TABLE book (
	book_id INT IDENTITY PRIMARY KEY,
	title NVARCHAR(50) NOT NULL,
	year INT NOT NULL,
	author_id INT NOT NULL REFERENCES author(author_id),
	category_id INT NOT NULL REFERENCES category(category_id)
);

CREATE PROC pGetAllOrOneBook
@id INT
AS
BEGIN
	IF @id <> 0
	BEGIN
		SELECT *
		FROM book
		WHERE book_id = @id

		RETURN
	END

	SELECT *
	FROM book
END;

CREATE PROC pSaveBook
@title NVARCHAR(50),
@year INT,
@author_id INT,
@category_id INT
AS
BEGIN
	IF @title IS NULL OR
	@year IS NULL OR 
	@author_id IS NULL OR
	@category_id IS NULL
	BEGIN
		SELECT 'Wrong request'
		RETURN
	END

	INSERT INTO book
	VALUES (@title,@year,@author_id,@category_id)

	SELECT 'successful'
END;

CREATE PROC pUpdateBook
@id INT,
@title NVARCHAR(50),
@year INT,
@author_id INT,
@category_id INT
AS
BEGIN
	UPDATE book
	SET title = @title,
		year = @year,
		author_id = @author_id,
		category_id = @category_id
	WHERE book_id = @id
END;

CREATE PROC pDeleteBook
@id INT
AS
BEGIN
	IF @id IS NOT NULL
	BEGIN
		DELETE FROM book
		WHERE book_id = @id
	END
END

CREATE PROC pGetAllOrOneAuthor
@id INT
AS
BEGIN
	IF @id IS NOT NULL 
	BEGIN
		SELECT *
		FROM author
		WHERE author_id = @id

		RETURN
	END

	SELECT *
	FROM author
END;

CREATE PROC pSaveAuthor
@lastName NVARCHAR(20),
@firstName NVARCHAR(20)
AS
BEGIN
	IF @lastName IS NULL OR
		@firstName IS NULL
	BEGIN
		SELECT 'Wrong request'
		RETURN
	END

	INSERT INTO author
	VALUES (@lastName,@firstName)

	SELECT 'Successful'
END;

CREATE PROC pUpdateAuthor
@id INT,
@lastName NVARCHAR(20),
@firstName NVARCHAR(20)
AS
BEGIN
	UPDATE author
	SET last_name = @lastName,
		first_name = @firstName
	WHERE author_id = @id
END;

CREATE PROC pDeleteAuthor
@id INT
AS
BEGIN
	DELETE FROM author
	WHERE author_id = @id
END;

CREATE PROC pGetAllOrOneCategory
@id INT
AS
BEGIN
	IF @id IS NOT NULL
	BEGIN
		SELECT *
		FROM category
		WHERE category_id = @id
		
		RETURN
	END

	SELECT *
	FROM category
END;

CREATE PROC pSaveCategory
@name NVARCHAR(20)
AS
BEGIN
	IF @name IS NULL
	BEGIN
		SELECT 'Wrong request'
		RETURN
	END

	INSERT INTO category
	VALUES (@name)

	SELECT 'Successful'
END;

CREATE PROC pUpdateCategory
@id INT,
@name NVARCHAR(20)
AS
BEGIN
	UPDATE category
	SET name = @name
	WHERE category_id = @id
END;

CREATE PROC pDeleteCategory
@id INT
AS
BEGIN
	DELETE FROM category
	WHERE category_id = @id
END

CREATE PROC pGetBookId
@bookName NVARCHAR(20),
@authorName NVARCHAR(20),
@categoryName NVARCHAR(20)
AS
BEGIN
	SELECT book_id
	FROM book b JOIN author a
	ON b.author_id = a.author_id JOIN category c
	ON b.category_id = c.category_id
END

CREATE PROC pAuthorSelect
AS
BEGIN
	SELECT author_id,first_name + ' ' + last_name name
	FROM author
	ORDER BY 2
END;

CREATE PROC pCategorySelect
AS
BEGIN
	SELECT category_id, name
	FROM category
	ORDER BY 2
END
 */