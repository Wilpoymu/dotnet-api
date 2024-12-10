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
        private bool ProductExists(AppDbContext context, int id)
        {
            return context.Product.Any(e => e.Id == id);
        }
        
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
        
        [HttpPost]
        public async Task<ActionResult<Product>> Create(AppDbContext context, Product product)
        {
            context.Product.Add(product);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(AppDbContext context, int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }
            context.Entry(product).State = EntityState.Modified;
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(context, id))
                {
                    return NotFound();
                }
                throw;
            }
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(AppDbContext context, int id)
        {
            var product = await context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            context.Product.Remove(product);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}