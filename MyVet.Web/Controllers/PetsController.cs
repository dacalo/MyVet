using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyVet.Web.Data;
using MyVet.Web.Data.Entities;
using MyVet.Web.Helpers;
using MyVet.Web.Models;
using Rotativa.AspNetCore;

namespace MyVet.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PetsController : Controller
    {
        private readonly ICombosHelper _combosHelper;
        private readonly DataContext _dataContext;

        public PetsController(
            ICombosHelper combosHelper,
            DataContext dataContext)
        {
            _combosHelper = combosHelper;
            _dataContext = dataContext;
        }

        public IActionResult Index()
        {
            return View(_dataContext.Pets
                .Include(p => p.Owner)
                .ThenInclude(o => o.User)
                .Include(p => p.PetType)
                .Include(p => p.Histories));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _dataContext.Pets
                .Include(p => p.Owner)
                .ThenInclude(o => o.User)
                .Include(p => p.Histories)
                .ThenInclude(h => h.ServiceType)
                .FirstOrDefaultAsync(o => o.Id == id.Value);
            
            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _dataContext.Pets
                .Include(p => p.Owner)
                .Include(p => p.PetType)
                .FirstOrDefaultAsync(p => p.Id == id.Value);
            if (pet == null)
            {
                return NotFound();
            }

            var model = new PetViewModel
            {
                Born = pet.Born,
                Id = pet.Id,
                ImageUrl = pet.ImageUrl,
                Name = pet.Name,
                OwnerId = pet.Owner.Id,
                PetTypeId = pet.PetType.Id,
                PetTypes = _combosHelper.GetComboPetTypes(),
                Race = pet.Race,
                Remarks = pet.Remarks
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PetViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = model.ImageUrl;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.jpg";

                    path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\Pets",
                        file);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }

                    path = $"~/images/Pets/{file}";
                }

                var pet = new Pet
                {
                    Born = model.Born,
                    Id = model.Id,
                    ImageUrl = path,
                    Name = model.Name,
                    Owner = await _dataContext.Owners.FindAsync(model.OwnerId),
                    PetType = await _dataContext.PetTypes.FindAsync(model.PetTypeId),
                    Race = model.Race,
                    Remarks = model.Remarks
                };

                _dataContext.Pets.Update(pet);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _dataContext.Pets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pet == null)
            {
                return NotFound();
            }

            _dataContext.Pets.Remove(pet);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteHistory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var history = await _dataContext.Histories
                .Include(h => h.Pet)
                .FirstOrDefaultAsync(h => h.Id == id.Value);
            if (history == null)
            {
                return NotFound();
            }

            _dataContext.Histories.Remove(history);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction($"{nameof(Details)}/{history.Pet.Id}");
        }

        public async Task<IActionResult> EditHistory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var history = await _dataContext.Histories
                .Include(h => h.Pet)
                .Include(h => h.ServiceType)
                .FirstOrDefaultAsync(p => p.Id == id.Value);
            if (history == null)
            {
                return NotFound();
            }

            var model = new HistoryViewModel
            {
                Date = history.Date,
                Description = history.Description,
                Id = history.Id,
                PetId = history.Pet.Id,
                Remarks = history.Remarks,
                ServiceTypeId = history.ServiceType.Id,
                ServiceTypes = _combosHelper.GetComboServiceTypes()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditHistory(HistoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var history = new History
                {
                    Date = model.Date,
                    Description = model.Description,
                    Id = model.Id,
                    Pet = await _dataContext.Pets.FindAsync(model.PetId),
                    Remarks = model.Remarks,
                    ServiceType = await _dataContext.ServiceTypes.FindAsync(model.ServiceTypeId)
                };

                _dataContext.Histories.Update(history);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"{nameof(Details)}/{model.PetId}");
            }

            return View(model);
        }

        public async Task<IActionResult> AddHistory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _dataContext.Pets.FindAsync(id.Value);
            if (pet == null)
            {
                return NotFound();
            }

            var model = new HistoryViewModel
            {
                Date = DateTime.Now,
                PetId = pet.Id,
                ServiceTypes = _combosHelper.GetComboServiceTypes(),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddHistory(HistoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var history = new History
                {
                    Date = model.Date,
                    Description = model.Description,
                    Pet = await _dataContext.Pets.FindAsync(model.PetId),
                    Remarks = model.Remarks,
                    ServiceType = await _dataContext.ServiceTypes.FindAsync(model.ServiceTypeId)
                };

                _dataContext.Histories.Add(history);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"{nameof(Details)}/{model.PetId}");
            }

            return View(model);
        }

        public IActionResult Print()
        {
            return new ViewAsPdf(_dataContext.Pets
                .Include(p => p.Owner)
                .ThenInclude(o => o.User)
                .Include(p => p.PetType)
                .Include(p => p.Histories))
            { FileName = "Pets.pdf" };
        }

    }
}
