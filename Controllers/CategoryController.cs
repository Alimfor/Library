using Library.DTO;
using Library.Models;
using Library.Services;
using Library.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers;

    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        private const string GET_ALL_CATEGORIES = "all";
        private const string GET_CATEGORY_BY_ID = "{id}";
        private const string GET_CATEGORY_LIST_SELECT = "categories_select";
        private const string POST_SAVE_CATEGORY = "new";
        private const string PUT_UPDATE_CATEGORY = "{id}/edit";
        private const string DELETE_CATEGORY_BY_ID = "{id}/delete";
        
        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Route(GET_ALL_CATEGORIES)]
        public IActionResult GetAllCategories()
        {
            var operationResult = _categoryService.GetAllCategories();
            var result = operationResult.result;
            var categories = operationResult.data;

            if (!categories.Any() && result.code == 200)
                return Ok(Enumerable.Empty<Category>());

            var categoriesDto = categories.Select(category => category.ToCategoryDto());
            return ResultState(result,categoriesDto);
        }

        [Route(GET_CATEGORY_BY_ID)]
        public IActionResult GetCategoryById(int id)
        {
            var operationResult = _categoryService.GetCategoryById(id);
            var result = operationResult.result;
            if (result.code != 200)
                return StatusCode(result.code, result.message);
            
            var categoryDto = operationResult.data.ToCategoryDto();
            return ResultState(result,categoryDto);
        }

        [Route(GET_CATEGORY_LIST_SELECT)]
        public IActionResult CategorySelect()
        {
            var operationResult = _categoryService.CategorySelect();
            var result = operationResult.result;
            var categorySelects = operationResult.data;
            
            return ResultState(result,categorySelects);
        }

        [HttpPost, Route(POST_SAVE_CATEGORY)]
        public IActionResult AddCategory(CategoryDTO categoryDto)
        {
            var result = _categoryService.SaveCategory(categoryDto.ToCategory());
            
            return ResultState<object>(result, null);
    }

        [HttpPut, Route(PUT_UPDATE_CATEGORY)]
        public IActionResult UpdateCategory(CategoryDTO categoryDto,int id)
        {
            var result = _categoryService.UpdateCategory(categoryDto.ToCategory(), id);
            
            return ResultState<object>(result, null);
    }

        [HttpDelete, Route(DELETE_CATEGORY_BY_ID)]
        public IActionResult DeleteCategoryById(int id)
        {
            var result = _categoryService.DeleteCategoryById(id);

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