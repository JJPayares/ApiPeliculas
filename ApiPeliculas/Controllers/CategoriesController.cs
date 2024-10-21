﻿using ApiPeliculas.Models;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetCategories() 
        { 
            var categoriesList = _categoryRepository.GetCategories();
            var categoriesListDto = new List<CategoryDto>();

            foreach (var category in categoriesList) { 
                categoriesListDto.Add(_mapper.Map<CategoryDto>(category));
            }
            return Ok(categoriesListDto);
        }


    
        [HttpGet("{categoryId:int}", Name ="GetCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategory( int categoryId)
        {

            var categoryItem = _categoryRepository.GetCategory(categoryId);

            if (categoryItem == null) { return NotFound(); }

            var categoryItemDto = _mapper.Map<CategoryDto>(categoryItem);

            return Ok(categoryItemDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            if (!ModelState.IsValid || createCategoryDto == null) {
                return BadRequest(ModelState);
            }

            if (_categoryRepository.ExistsCategory(createCategoryDto.Name)) {
                ModelState.AddModelError("", "Category already exists.");
                return StatusCode(404, ModelState);
            }

            var category = _mapper.Map<Category>(createCategoryDto);

            if (!_categoryRepository.CreateCategory(category)) 
            {
                ModelState.AddModelError("",$"Something went wrong saving the record {category.Name}");
                return StatusCode(404, ModelState);
            }

            return CreatedAtRoute("GetCategory", new {categoryId = category.Id}, category);

        }
    }
}
