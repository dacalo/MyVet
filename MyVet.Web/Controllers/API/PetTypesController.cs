using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyVet.Web.Data;
using MyVet.Web.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyVet.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PetTypesController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public PetTypesController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // GET: api/PetTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PetType>>> GetPetTypes()
        {
            return await _dataContext.PetTypes.ToListAsync();
        }

    }
}
