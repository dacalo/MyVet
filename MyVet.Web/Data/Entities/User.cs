namespace MyVet.Web.Data.Entities
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        [Display(Name = "RFC:")]
        [MaxLength(13, ErrorMessage = "El campo {0} no puede ser mayor a {1} caracteres.")]
        [MinLength(12, ErrorMessage = "El campo {0}")]
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

        [Display(Name= "Dirección")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede ser mayor a {1} caracteres.")]
        public string Address { get; set; }

        [Display(Name = "Latitud")]
        [DisplayFormat(DataFormatString = "{0:N6}")]
        public double Latitude { get; set; }

        [Display(Name = "Longitud")]
        [DisplayFormat(DataFormatString = "{0:N6}")]
        public double Longitude { get; set; }

        [Display(Name = "Nombre completo")]
        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "Nombre completo con RFC")]
        public string FullNameWithDocument => $"{FirstName} {LastName} - {RFC}";
    }
}
