using ApiPeliculas.Models;

namespace ApiPeliculas.Repository.IRepository
{
    public interface IMovieRepository
    {
        ICollection<Movie> GetMovies();
        ICollection<Movie> GetMoviesByCategory(int categoryId);
        IEnumerable<Movie> FindMovies(String movieName);

        Movie GetMovie(int movieId);

        bool ExistsMovie(int movieId);
        bool ExistsMovie(String movieName);


        bool CreateMovie(Movie movie);
        bool UpdateMovie(Movie movie);
        bool DeleteMovie(Movie movie);

        bool Save();
    }
}
