using Microsoft.AspNetCore.Mvc.Rendering;
using MyVet.Web.Data;
using System.Collections.Generic;
using System.Linq;

namespace MyVet.Web.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _dataContext;

        public CombosHelper(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IEnumerable<SelectListItem> GetComboPetTypes()
        {
            List<SelectListItem> list = _dataContext.PetTypes.Select(pt => new SelectListItem
            {
                Text = pt.Name,
                Value = $"{pt.Id}"
            })
                .OrderBy(pt => pt.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione el tipo de mascota...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboServiceTypes()
        {
            List<SelectListItem> list = _dataContext.ServiceTypes.Select(pt => new SelectListItem
            {
                Text = pt.Name,
                Value = $"{pt.Id}"
            })
                .OrderBy(pt => pt.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione el tipo de servicio...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboOwners()
        {
            List<SelectListItem> list = _dataContext.Owners.Select(p => new SelectListItem
            {
                Text = p.User.FullNameWithDocument,
                Value = p.Id.ToString()
            }).OrderBy(p => p.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Seleccione el dueño...)",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboPets(int ownerId)
        {
            List<SelectListItem> list = _dataContext.Pets.Where(p => p.Owner.Id == ownerId).Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString()
            }).OrderBy(p => p.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Seleccione la mascota...)",
                Value = "0"
            });

            return list;
        }

    }
}

