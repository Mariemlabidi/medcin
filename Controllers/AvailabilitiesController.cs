using medcin.Data;
using medcin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace medcin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvailabilitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AvailabilitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/availabilities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Availability>>> GetAvailabilities()
        {
            return await _context.Availabilities
                .Include(a => a.Doctor)
                .ToListAsync();
        }

        // GET: api/availabilities/doctor/5
        [HttpGet("doctor/{doctorId}")]
        public async Task<ActionResult<IEnumerable<Availability>>> GetDoctorAvailabilities(int doctorId)
        {
            var availabilities = await _context.Availabilities
                .Where(a => a.DoctorId == doctorId)
                .ToListAsync();

            return availabilities;
        }

        // GET: api/availabilities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Availability>> GetAvailability(int id)
        {
            var availability = await _context.Availabilities.FindAsync(id);

            if (availability == null)
                return NotFound();

            return availability;
        }

        // POST: api/availabilities
        [HttpPost]
        public async Task<ActionResult<Availability>> CreateAvailability([FromBody] Availability availability)
        {
            var doctor = await _context.Doctors.FindAsync(availability.DoctorId);
            if (doctor == null)
                return BadRequest("Médecin introuvable.");

            _context.Availabilities.Add(availability);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAvailability), new { id = availability.Id }, availability);
        }


        // PUT: api/availabilities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAvailability(int id, Availability updatedAvailability)
        {
            if (id != updatedAvailability.Id)
                return BadRequest();

            var availability = await _context.Availabilities.FindAsync(id);
            if (availability == null)
                return NotFound();

            availability.Day = updatedAvailability.Day;
            availability.StartTime = updatedAvailability.StartTime;
            availability.EndTime = updatedAvailability.EndTime;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/availabilities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAvailability(int id)
        {
            var availability = await _context.Availabilities.FindAsync(id);
            if (availability == null)
                return NotFound();

            _context.Availabilities.Remove(availability);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

