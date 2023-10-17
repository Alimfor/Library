using Library.DTO;
using Library.Models;
using Library.Services;
using Library.Utils;
using Microsoft.AspNetCore.Http;
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
        private const string PUT_UPDATE_AUTHOR = "edit";
        private const string DELETE_AUTHOR_BY_ID = "delete";
        public AuthorController(AuthorService authorService)
        {
            _authorService = authorService;
        }

        [Route(GET_ALL_AUTHORS)]
        public List<AuthorDTO> GetAllAuthors()
        {
            var authors = _authorService.GetAllAuthors();
            return authors.Select(FromAuthorToAuthorDto).ToList();
        }

        [Route(GET_AUTHOR_BY_ID)]
        public IActionResult GetAuthorById(int id)
        {
            Author author = _authorService.GetAuthorById(id);
            AuthorDTO authorDto = FromAuthorToAuthorDto(author);
            
            return authorDto == null
                ? BadRequest("Sent id is wrong")
                : Ok(authorDto);
        }

        [Route(GET_AUTHOR_LIST_SELECT)]
        public IActionResult AuthorSelect()
        {
            return Ok(_authorService.AuthorSelect());
        }

        [HttpPost, Route(POST_SAVE_AUTHOR)]
        public IActionResult AddAuthor(AuthorDTO authorDto)
        {
            Result result = _authorService.SaveAuthor(FromAuthorDtoToAuthor(authorDto));
            return result.code == 200
                ? Ok()
                : BadRequest(result.error);
        }

        [HttpPut, Route(PUT_UPDATE_AUTHOR)]
        public IActionResult UpdateAuthor(AuthorDTO authorDto)
        {
            Result result = _authorService.UpdateAuthor(FromAuthorDtoToAuthor(authorDto));
            return result.code == 200
                ? Ok()
                : BadRequest(result.error);
        }

        [HttpDelete, Route(DELETE_AUTHOR_BY_ID)]
        public IActionResult DeleteAuthorById(int id)
        {
            Result result = _authorService.DeleteAuthorById(id);
            return result.code == 200
                ? Ok()
                : BadRequest(result.error);
        }

        private Author FromAuthorDtoToAuthor(AuthorDTO authorDto)
        {
            if (authorDto == null)
                return null;

            return new Author()
            {
                firstName = authorDto.firstName,
                lastName = authorDto.lastName
            };
        }
        
        private AuthorDTO FromAuthorToAuthorDto(Author author)
        {
            if (author == null)
                return null;
	        
            return new AuthorDTO()
            {
                firstName = author.firstName,
                lastName = author.lastName
            };
        }
    }
}
