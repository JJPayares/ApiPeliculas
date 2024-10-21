using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _db;

        public MovieRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public ICollection<Movie> GetMovies()
        {
            return _db.Movies.OrderBy(m => m.Name).ToList();
        }

        public ICollection<Movie> GetMoviesByCategory(int categoryId)
        {
            return _db.Movies.Include(ca => ca.Category)
                    .Where(ca => ca.CategoryId == categoryId)
                        .ToList();
        }

        public IEnumerable<Movie> FindMovies(string param)
        {
            IQueryable<Movie> query = _db.Movies;
            if (!string.IsNullOrEmpty(param))
            {
                query = query.Where(m => m.Name.Contains(param) || m.Description.Contains(param));
            }
            return query.ToList();
        }

        public Movie GetMovie(int movieId)
        {
            return _db.Movies.FirstOrDefault(m => m.Id == movieId);
        }

        public bool ExistsMovie(int movieId)
        {
            return _db.Movies.Any(m => m.Id == movieId);
        }

        public bool ExistsMovie(string movieName)
        {
            return _db.Movies.Any(m => m.Name.ToLower().Trim() == movieName.ToLower().Trim());
        }

        public bool CreateMovie(Movie movie)
        {
            movie.CreationDate = DateTime.Now;
            _db.Movies.Add(movie);
            return Save();  
        }

        public bool UpdateMovie(Movie movie)
        {
            movie.CreationDate = DateTime.Now;
            var existingMovie = _db.Movies.Find(movie.Id);

            if (existingMovie != null)
            {
                _db.Entry(existingMovie).CurrentValues.SetValues(movie);
            }
            else
            {
                _db.Movies.Update(movie);
            }
            return Save();
        }

        public bool DeleteMovie(Movie movie)
        {
            _db.Movies.Remove(movie);
            return Save();
        }
    }
}
