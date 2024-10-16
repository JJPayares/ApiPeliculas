using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Repository.IRepository;

namespace ApiPeliculas.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool CreateCategory(Category category)
        {
            category.CreationDate = DateTime.Now;
            _db.Categories.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _db.Categories.Remove(category);
            return Save();
        }

        public bool ExistsCategory(int categoryId)
        {
            return _db.Categories.Any(c => c.Id == categoryId);
        }

        public bool ExistsCategory(string categoryName)
        {
            return _db.Categories.Any(
                c => c.Name.ToLower().Trim() == categoryName.ToLower().Trim());

        }

        public ICollection<Category> GetCategories()
        {
            return _db.Categories.OrderBy(c => c.Id).ToList();
        }

        public Category GetCategory(int categoryId)
        {
            return _db.Categories.FirstOrDefault(c => c.Id == categoryId);
                }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            category.CreationDate = DateTime.Now;
            _db.Categories.Update(category);
            return Save();
        }
    }
}
