using Library.DTO;
using Library.Models;
using Library.Services;
using Library.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Route("api/author")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly AuthorService _authorService;

        private const string GET_ALL_AUTHORS = "all";
        private const string GET_AUTHOR_BY_ID = "{id}";
        private const string GET_AUTHOR_LIST_SELECT = "authors_select";
        private const string POST_SAVE_AUTHOR = "new";
        private const string PUT_UPDATE_AUTHOR = "{id}/edit";
        private const string DELETE_AUTHOR_BY_ID = "{id}/delete";
        
        public AuthorController(AuthorService authorService)
        {
            _authorService = authorService;
        }

        [Route(GET_ALL_AUTHORS)]
        public IActionResult GetAllAuthors()
        {
            var operationResult = _authorService.GetAllAuthors();
            var result = operationResult.result;
            var authors = operationResult.data;

            if (!authors.Any() && result.code == 200)
                return Ok(Enumerable.Empty<Author>());

            var authorsDto = authors.Select(author => author.ToAuthorDto());
            return ResultState(result,authorsDto);
        }

        [Route(GET_AUTHOR_BY_ID)]
        public IActionResult GetAuthorById(int id)
        {
            var operationResult = _authorService.GetAuthorById(id);
            var result = operationResult.result;
            if (result.code != 200)
                return StatusCode(result.code, result.message);
            
            var authorDto = operationResult.data.ToAuthorDto();
            return ResultState(result,authorDto);
        }

        [Route(GET_AUTHOR_LIST_SELECT)]
        public IActionResult AuthorSelect()
        {
            var operationResult = _authorService.AuthorSelect();
            var result = operationResult.result;
            var authorSelects = operationResult.data;
            
            return ResultState(result,authorSelects);
        }

        [HttpPost, Route(POST_SAVE_AUTHOR)]
        public IActionResult AddAuthor(AuthorDTO authorDto)
        {
            var result = _authorService.SaveAuthor(authorDto.ToAuthor());
            
            return ResultState<object>(result, null);
        }

        [HttpPut, Route(PUT_UPDATE_AUTHOR)]
        public IActionResult UpdateAuthor(AuthorDTO authorDto,int id)
        {
            var result = _authorService.UpdateAuthor(authorDto.ToAuthor(), id);
            
            return ResultState<object>(result, null);
        }

        [HttpDelete, Route(DELETE_AUTHOR_BY_ID)]
        public IActionResult DeleteAuthorById(int id)
        {
            var result = _authorService.DeleteAuthorById(id);

            return ResultState<object>(result, null);
        }

        private IActionResult ResultState<T>(Result result,T? data)
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
