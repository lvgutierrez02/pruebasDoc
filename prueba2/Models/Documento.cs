using System.ComponentModel.DataAnnotations;

namespace prueba2.Models
{
    public class Documento
    {
        [Key]
        public int DocumentoId { get; set; }
        [Required, MaxLength]
        public string Descripción { get; set; }
        [Required, MaxLength]
        public string Ruta { get; set; }

        //permite almacenar el archivo que se envía a traves de la ruta desde el api
        public IFormFile Archivo { get; set; }
    }
}
