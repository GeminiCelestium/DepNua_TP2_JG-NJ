using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModernRecrut.Postulations.API.Data;
using ModernRecrut.Postulations.API.Models;


namespace ModernRecrut.Postulations.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostulationsController : ControllerBase
    {
        private readonly ModernRecrutPostulationContext _context;

        public PostulationsController(ModernRecrutPostulationContext context)
        {
            _context = context;
        }

        // GET: api/Postulations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Postulation>>> GetPostulation()
        {
            if (_context.Postulation == null)
            {
                return NotFound();
            }
            return await _context.Postulation.ToListAsync();
        }

        // GET: api/Postulations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Postulation>> GetPostulation(int id)
        {
            if (_context.Postulation == null)
            {
                return NotFound();
            }
            var postulation = await _context.Postulation.FindAsync(id);

            if (postulation == null)
            {
                return NotFound();
            }

            return postulation;
        }

        // PUT: api/Postulations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPostulation(int id, Postulation postulation)
        {
            if (id != postulation.Id)
            {
                return BadRequest();
            }

            _context.Entry(postulation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostulationExists(id))
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

        // POST: api/Postulations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Postulation>> PostPostulation(Postulation postulation)
        {
            if (_context.Postulation == null)
            {
                return Problem("Entity set 'ModernRecrutPostulationContext.Postulation'  is null.");
            }
            _context.Postulation.AddAsync(postulation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPostulation", new { id = postulation.Id }, postulation);
        }

        // DELETE: api/Postulations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostulation(int id)
        {
            if (_context.Postulation == null)
            {
                return NotFound();
            }
            var postulation = await _context.Postulation.FindAsync(id);
            if (postulation == null)
            {
                return NotFound();
            }

            _context.Postulation.Remove(postulation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostulationExists(int id)
        {
            return (_context.Postulation?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
