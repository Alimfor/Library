using Library.DTO;
using Library.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Route("api/author")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly AuthorService _authorService;

        public AuthorController(AuthorService authorService)
        {
            _authorService = authorService;
        }

        //[HttpPost]
        //public string CreateAuthor(AuthorDTO authorDTO)
        //{
        //    Result result = _authorService.
        //}
    }
}
