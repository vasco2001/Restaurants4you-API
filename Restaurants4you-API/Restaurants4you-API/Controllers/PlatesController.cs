using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant4you_API.Data;
using Restaurant4you_API.Models;

namespace Restaurant4you_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Plates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plate>>> GetPlates()
        {
            return await _context.Plates
                              .Include(a => a.Restaurant)
                              .OrderByDescending(a => a.Id)
                              .Select(a => new Plate
                              {
                                  Id = a.Id,
                                  Name= a.Name,
                                  Description= a.Description,
                                  RestaurantFK=a.RestaurantFK,
                                  Restaurant = a.Restaurant,
                              })
                              .ToListAsync();
        }

        // GET: api/Plates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Plate>> GetPlate(int id)
        {
            var plate = await _context.Plates
                              .Include(a => a.Restaurant)
                              .OrderByDescending(a => a.Id)
                              .Select(a => new Plate
                              {
                                  Id = a.Id,
                                  Name = a.Name,
                                  Description = a.Description,
                                  RestaurantFK = a.RestaurantFK,
                                  Restaurant = a.Restaurant,
                              })
                              .Where(a => a.Id == id)
                              .FirstOrDefaultAsync();

            if (plate == null)
            {
                return NotFound();
            }

            return plate;
        }

        // PUT: api/Plates/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlate(int id, [FromForm] Plate plate)
        {
            if (id != plate.Id)
            {
                return BadRequest();
            }

            _context.Entry(plate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlateExists(id))
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

        // POST: api/Plates
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Plate>> PostPlate([FromForm] Plate plate)
        {
            _context.Plates.Add(plate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlate", new { id = plate.Id }, plate);
        }

        // DELETE: api/Plates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlate(int id)
        {
            var plate = await _context.Plates.FindAsync(id);
            if (plate == null)
            {
                return NotFound();
            }

            _context.Plates.Remove(plate);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlateExists(int id)
        {
            return _context.Plates.Any(e => e.Id == id);
        }
    }
}
