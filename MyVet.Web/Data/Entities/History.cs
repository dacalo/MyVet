namespace MyVet.Web.Data.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public class History
    {
        public int Id { get; set; }

        [Display(Name = "Descripción*")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede ser mayor a {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Description { get; set; }

        [Display(Name = "Fecha*")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Observaciones")]
        public string Remarks { get; set; }

        [Display(Name = "Fecha*")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime DateLocal => Date.ToLocalTime();

        public ServiceType ServiceType { get; set; }

        public Pet Pet { get; set; }
    }
}