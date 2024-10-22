using System.Net;

namespace ApiPeliculas.Models
{
    public class ApiResponses
    {
        public ApiResponses()
        {
            ErrorMessages = new List<string>();
        }

        public HttpStatusCode StatusCode { get; set; }

        public bool IsSucces { get; set; } = true;

        public List<string> ErrorMessages { get; set; }

        public object Result { get; set; }
    }
}
