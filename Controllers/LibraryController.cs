using Library.DTO;
using Library.Models;
using Library.Services;
using Library.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Route("api/library")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly BookService _bookService;
        private readonly AuthorService _authorService;
        private readonly CategoryService _categoryService;

		private readonly string GET_ALL_BOOKS = "all";
		private readonly string GET_BOOK_BY_ID = "book";
		private readonly string GET_ALL_BOOKS = "all";
		private readonly string GET_ALL_BOOKS = "all";
		private readonly string GET_ALL_BOOKS = "all";

        public LibraryController(BookService bookService, CategoryService categoryService, AuthorService authorService)
        {
	        _bookService = bookService;
	        _categoryService = categoryService;
	        _authorService = authorService;
        }

        public List<BookDTO> GetAllBooks()
        {
	        var books = _bookService.GetAllBooks();
	        return books.Select(FromBookToBookDTO).ToList();
        }

        public BookDTO GetBookById(int id)
        {
            return FromBookToBookDTO(_bookService.GetBookById(id));
        }

        [HttpPost]
        public string AddBook(BookDTO bookDto)
        {
	        Result result = _bookService.SaveBook(FromBookDtoToBook(bookDto));

	        return result.code == 200 ? "Successful" : "Wrong request";
        }

        [HttpPatch]
        public string UpdateBook(BookDTO bookDto)
        {
	        Result result = _bookService.UpdateBook(FromBookDtoToBook(bookDto));
	        
	        return result.code == 200 ? "Successful" : "Wrong request";
        }

        [HttpDelete]
        public string DeleteBook(string bookName, string authorName, string categoryName)
        {
	        Result result = _bookService.DeleteBook(bookName, authorName, categoryName);
	        return result.code == 200 ? "Successful" : "Wrong request";
        }
        
        private BookDTO FromBookToBookDTO(Book book)
        {
	        var authorDto = FromAuthorToAthorDTO(_authorService.GetAuthorById(book.authorId));
	        var categoryDto = FromCategoryToCategoryDTO(_categoryService.GetCategoryById(book.categoryId));
	        return new BookDTO()
	        {
		        title = book.title,
		        year = book.year,
		        author = authorDto,
		        category = categoryDto
	        };
        }

        private Book FromBookDtoToBook(BookDTO bookDto)
        {
	        int authorId =
		        _authorService.GetAuthorId(bookDto.author.firstName, bookDto.author.lastName);
	        int categoryId = _categoryService.GetCategoryIdByName(bookDto.category.name);
	        return new Book()
	        {
		        title = bookDto.title,
		        year = bookDto.year,
		        authorId = authorId,
		        categoryId = categoryId
	        };
        }

        private AuthorDTO FromAuthorToAthorDTO(Author author)
        {
	        return new AuthorDTO()
	        {
		        firstName = author.firstName,
		        lastName = author.lastName
	        };
        }

        private CategoryDTO FromCategoryToCategoryDTO(Category category)
        {
	        return new CategoryDTO()
	        {
		        name = category.name
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
	IF @id IS NOT NULL
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
		SELECT 'Wrong reques'
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
 */