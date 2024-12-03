using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using softsolutions.Data;
using softsolutions.Models;

namespace softsolutions.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Product> GetAll(AppDbContext context)
        {
            return context.Product
                .Include(x => x.Providers)
                .Include(x => x.Categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(AppDbContext context, int id)
        {
            var product = await context.Product
                .Include(x => x.Providers)
                .Include(x => x.Categories)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }
    }
}