using ApiPeliculas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Data {  
public class ApplicationDbContext :DbContext
        {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        }

        // Pasar todos los modelos
        public DbSet<Category> Categories {  get; set; }
    }

}