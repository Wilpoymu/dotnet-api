
using Microsoft.AspNetCore.Mvc;
using softsolutions.Models;
using System.Collections.Generic;
using System.Linq;

namespace softsolutions.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProvidersController : ControllerBase
    {
        private static List<Provider> providers = new List<Provider>();

        [HttpGet]
        public ActionResult<IEnumerable<Provider>> GetProviders()
        {
            return Ok(providers);
        }

        [HttpPost]
        public ActionResult<Provider> AddProvider(Provider provider)
        {
            try
            {
                provider.Validate();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            provider.Id = providers.Count > 0 ? providers.Max(p => p.Id) + 1 : 1;
            providers.Add(provider);
            return Ok(provider);
        }
    }
}