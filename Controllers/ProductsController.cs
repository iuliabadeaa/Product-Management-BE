using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using proiect_practica; // Ensure to use the correct namespace where your DbContext and Produs class are defined

namespace proiect_practica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produs>>> GetProducts()
        {
            return await _context.Produse.ToListAsync();
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Produs>> GetProduct(int id)
        {
            var product = await _context.Produse.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Produs>> PostProduct(Produs product)
        {
            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.produs ON");
            _context.Produse.Add(product);
            await _context.SaveChangesAsync();
            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.produs OFF");


            return CreatedAtAction(nameof(GetProduct), new { id = product.ID }, product);
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Produs product)
        {
            if (id != product.ID)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Produse.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Produse.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Produse.Any(e => e.ID == id);
        }
    }
}
