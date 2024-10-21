using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Models.Dtos
{
    public class MovieDto
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public int Duration { get; set; }
        public String ImageRoute { get; set; }
        public enum ClasificationType { Seven, Thirteen, Sixteen, Eigthteen }
        public ClasificationType Clasification { get; set; }
        public DateTime CreationDate { get; set; }
        public int CategoryId { get; set; }
        
    }
}
