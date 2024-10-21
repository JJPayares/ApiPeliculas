using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Models.Dtos
{
    public class CreateMovieDto
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public int Duration { get; set; }
        public String ImageRoute { get; set; }
        public enum CretaeClasificationType { Seven, Thirteen, Sixteen, Eigthteen }
        public CretaeClasificationType Clasification { get; set; }
        public int CategoryId { get; set; }
        
    }
}
