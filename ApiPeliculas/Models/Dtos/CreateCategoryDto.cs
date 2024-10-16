using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.Dtos
{
    public class CreateCategoryDto
    {
      
        [Required(ErrorMessage ="Category name is required")]
        [MaxLength(100, ErrorMessage ="Name too long. Only use 100 characters")]
        public string Name { get; set; }

    }
}
