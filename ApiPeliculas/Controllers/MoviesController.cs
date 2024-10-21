using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repository;
using ApiPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [Route("api/peliculas")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public MoviesController(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetMovies()
        {
            var moviesList = _movieRepository.GetMovies();
            var moviesListDto = new List<MovieDto>();

            foreach (var movie in moviesList)
            {
                moviesListDto.Add(_mapper.Map<MovieDto>(movie));
            }
            return Ok(moviesListDto);
        }


        [HttpGet("{movieId:int}", Name = "GetMovie")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetMovie(int movieId)
        {

            var movieItem = _movieRepository.GetMovie(movieId);

            if (movieItem == null) { return NotFound(); }

            var movieItemDto = _mapper.Map<Movie>(movieItem);

            return Ok(movieItemDto);
        }


        [HttpPost]
        [ProducesResponseType(201, Type = typeof(MovieDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateMovie([FromBody] CreateMovieDto createMovieDto)
        {
            if (!ModelState.IsValid || createMovieDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_movieRepository.ExistsMovie(createMovieDto.Name))
            {
                ModelState.AddModelError("", "Movie already exists.");
                return StatusCode(404, ModelState);
            }

            var movie = _mapper.Map<Movie>(createMovieDto);

            if (!_movieRepository.CreateMovie(movie))
            {
                ModelState.AddModelError("", $"Something went wrong saving the record {movie.Name}.");
                return StatusCode(404, ModelState);
            }

            return CreatedAtRoute("GetMovie", new { movieId = movie.Id }, movie);

        }



        [HttpPatch("{movieId:int}", Name = "PatchMovie")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult PatchMovie(int movieId, [FromBody] MovieDto patchMovieDto)
        {

            if (!ModelState.IsValid || patchMovieDto == null || movieId != patchMovieDto.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_movieRepository.ExistsMovie(movieId))
            {
                return NotFound($"Not Founded any movie whit id {movieId}.");
            }

            var movie = _mapper.Map<Movie>(patchMovieDto);

            if (!_movieRepository.UpdateMovie(movie))
            {
                ModelState.AddModelError("", $"Somethiogn went wrong updating the record {movie.Name}.");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }


        [HttpDelete("{movieId:int}", Name = "DeleteMovie")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteMovie(int movieId)
        {
            if (!_movieRepository.ExistsMovie(movieId))
            {
                return NotFound();
            }

            var movie = _movieRepository.GetMovie(movieId);

            if (!_movieRepository.DeleteMovie(movie))
            {
                ModelState.AddModelError("", $"Somethiogn went wrong deleting the record {movie.Name}.");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }
    }
}
