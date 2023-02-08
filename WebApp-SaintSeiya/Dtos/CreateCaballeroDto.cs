using System.ComponentModel.DataAnnotations;

namespace WebApp_SaintSeiya.Dtos
{   
    // Con model binding le asigno un poco de lógica
    public class CreateCaballeroDto
    {
        [Required (ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; }

        [Required (ErrorMessage = "Se debe especificar la armadura")]
        public string Armadura { get; set; }

        [Required(ErrorMessage = "Se debe especificar la categoría")]
        public string Categoria { get; set; }

        public string Grupo { get; set; }
    }
}
