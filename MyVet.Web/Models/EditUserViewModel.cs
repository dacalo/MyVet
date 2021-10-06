using System.ComponentModel.DataAnnotations;

namespace MyVet.Web.Models
{
    public class EditUserViewModel
    {
        public int Id { get; set; }

        [Display(Name = "RFC")]
        [StringLength(13, MinimumLength = 12, ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres.")]
        [MaxLength(20, ErrorMessage = "El campo {0} no puede ser mayor a {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string RFC { get; set; }

        [Display(Name = "Nombre")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede ser mayor a {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string FirstName { get; set; }

        [Display(Name = "Apellidos")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede ser mayor a {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string LastName { get; set; }

        [MaxLength(100, ErrorMessage = "El campo {0} no puede ser mayor a {1} caracteres.")]
        [Display(Name = "Domicilio")]
        public string Address { get; set; }

        [Display(Name = "Teléfono")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede ser mayor a {1} caracteres.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Latitud")]
        public double Latitude { get; set; }

        [Display(Name = "Longitud")]
        public double Longitude { get; set; }

    }
}
