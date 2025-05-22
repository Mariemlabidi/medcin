using medcin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using medcin.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace medcin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DoctorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctors()
        {
            return await _context.Doctors
                .Include(d => d.Availabilities)
                .Include(d => d.Appointments)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> GetDoctor(int id)
        {
            var doctor = await _context.Doctors
                .Include(d => d.Availabilities)
                .Include(d => d.Appointments)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (doctor == null)
                return NotFound();

            return doctor;
        }

       
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Doctor>> AddDoctor(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDoctor), new { id = doctor.Id }, doctor);
        }

        
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> UpdateDoctor(int id, Doctor updatedDoctor)
        {
            if (id != updatedDoctor.Id)
                return BadRequest("L'identifiant ne correspond pas.");

            var existingDoctor = await _context.Doctors.FindAsync(id);
            if (existingDoctor == null)
                return NotFound();

            
            if (User.IsInRole("Doctor"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var doctorUser = await _context.Users.FindAsync(userId);
                if (doctorUser == null || doctorUser.UserName != existingDoctor.FullName)
                    return Forbid();
            }

            existingDoctor.FullName = updatedDoctor.FullName;
            existingDoctor.Speciality = updatedDoctor.Speciality;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors
                .Include(d => d.Appointments)
                .Include(d => d.Availabilities)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (doctor == null)
                return NotFound();

            _context.Appointments.RemoveRange(doctor.Appointments);
            _context.Availabilities.RemoveRange(doctor.Availabilities);
            _context.Doctors.Remove(doctor);

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
