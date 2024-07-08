using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using proiect_practica; 

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

        // GET all
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produs>>> GetProducts()
        {
            return await _context.Produse.ToListAsync();
        }

        // GET dupa ID
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

        // POST
        [HttpPost]
        public async Task<ActionResult<Produs>> PostProduct(Produs product)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.produs ON");

                    _context.Produse.Add(product);
                    await _context.SaveChangesAsync();

                    await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.produs OFF");

                    await transaction.CommitAsync();

                    return CreatedAtAction(nameof(GetProduct), new { id = product.ID }, product);
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        // PUT
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

        // DELETE dupa ID
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
