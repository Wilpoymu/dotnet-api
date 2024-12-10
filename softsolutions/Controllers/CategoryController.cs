using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using softsolutions.Data;
using softsolutions.Models;

namespace softsolutions.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private bool CategoryExists(AppDbContext context, int id)
        {
            return context.Category.Any(e => e.Id == id);
        }
        
        [HttpGet]
        public IEnumerable<Category> GetAll(AppDbContext context)
        {
            return context.Category
                .Include(x => x.Products);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetById(AppDbContext context, int id)
        {
            var category = await context.Category
                .Include(x => x.Products)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return category;
        }
        
        [HttpPost]
        public async Task<ActionResult<Category>> Create(AppDbContext context, Category category)
        {
            context.Category.Add(category);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(AppDbContext context, int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }
            context.Entry(category).State = EntityState.Modified;
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(context, id))
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
            var category = await context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            context.Category.Remove(category);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}