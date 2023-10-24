using Library.DTO;
using Library.Models;
using Library.Services;

namespace Library.Utils;

public static class BookExtension
{
    public static OperationResult<BookDTO> ToBookDto(this Book book,AuthorService authorService, CategoryService categoryService)
    {
        var authorOperationResult = authorService.GetAuthorById(book.authorId);
        var authorResult = authorOperationResult.result;
        
        var categoryOperationResult = categoryService.GetCategoryById(book.categoryId);
        var categoryResult = categoryOperationResult.result;

        if (authorResult.code != 200)
        {
            return CreateErrorResult(authorOperationResult.result.message, 
                authorOperationResult.result.code, authorOperationResult.result.status);
        }

        if (categoryResult.code != 200)
        {
            return CreateErrorResult(categoryOperationResult.result.message, 
                categoryOperationResult.result.code, 
                categoryOperationResult.result.status);
        }
        
        var authorDto = authorOperationResult.data.ToAuthorDto();
        var categoryDto = categoryOperationResult.data.ToCategoryDto();

        var bookDto = new BookDTO
        {
            bookId = book.bookId,
            title = book.title,
            year = book.year,
            author = authorDto,
            category = categoryDto
        };

        return CreateSuccessResult(bookDto);
    }

    public static Book ToBook(this BookDTO bookDto, int authorId,int categoryId)
    {
        return new Book()
        {
            bookId = bookDto.bookId,
            title = bookDto.title,
            year = bookDto.year,
            authorId = authorId,
            categoryId = categoryId
        };
    }
    
    private static OperationResult<BookDTO> CreateErrorResult(string message, int code, Status status)
    {
        return new OperationResult<BookDTO>
        {
            data = new BookDTO(),
            result = new Result
            {
                code = code,
                message = message,
                status = status
            }
        };
    }

    private static OperationResult<BookDTO> CreateSuccessResult(BookDTO bookDto)
    {
        return new OperationResult<BookDTO>
        {
            data = bookDto,
            result = new Result
            {
                code = 200,
                message = "done",
                status = Status.SUCCESSFUL
            }
        };
    }
}