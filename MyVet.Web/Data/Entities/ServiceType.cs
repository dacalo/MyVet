namespace MyVet.Web.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ServiceType
    {
        public int Id { get; set; }

        [Display(Name = "Tipo de Servicio")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede ser mayor a {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        public ICollection<History> Histories { get; set; }
    }
}
