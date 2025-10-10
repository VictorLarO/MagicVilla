using static MagicVilla_Utilidad.DS;
namespace MagicVilla_Web.Models
{
    public class APIRequest
    {
        public APITipo APITipo { get; set; }
        public string Url { get; set; }
        public object Datos { get; set; }
    }
}
        