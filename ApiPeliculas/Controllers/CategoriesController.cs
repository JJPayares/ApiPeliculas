using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategories() 
        { 
            var categoriesList = _categoryRepository.GetCategories();
            var categoriesListDto = new List<CategoryDto>();

            foreach (var category in categoriesList) { 
                categoriesListDto.Add(_mapper.Map<CategoryDto>(category));
            }
            return Ok(categoriesListDto);
        }


        [HttpGet]
        [HttpGet("{categoryId:int}", Name ="GetCategory")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategory( int categoryId)
        {
            var categoryItem = _categoryRepository.GetCategory(categoryId);

            if (categoryItem == null) { return NotFound(); }

            var categoryItemDto = _mapper.Map<CategoryDto>(categoryItem);

            return Ok(categoryItemDto);
        }
    }
}
