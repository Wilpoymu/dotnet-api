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
    [ApiController]
    [Route("api/[controller]")]
    public class ProviderController : ControllerBase
    {
        private bool ProviderExists(AppDbContext context, int id)
        {
            return context.Provider.Any(e => e.Id == id);
        }
        
        [HttpGet]
        public IEnumerable<Provider> GetAll(AppDbContext context)
        {
            return context.Provider
                .Include(x => x.Products);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Provider>> GetById(AppDbContext context, int id)
        {
            var provider = await context.Provider
                .Include(x => x.Products)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (provider == null)
            {
                return NotFound();
            }
            return provider;
        }
        
        [HttpPost]
        public async Task<ActionResult<Provider>> Create(AppDbContext context, Provider provider)
        {
            context.Provider.Add(provider);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = provider.Id }, provider);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(AppDbContext context, int id, Provider provider)
        {
            if (id != provider.Id)
            {
                return BadRequest();
            }
            context.Entry(provider).State = EntityState.Modified;
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProviderExists(context, id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(AppDbContext context, int id)
        {
            var provider = await context.Provider.FindAsync(id);
            if (provider == null)
            {
                return NotFound();
            }
            context.Provider.Remove(provider);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}