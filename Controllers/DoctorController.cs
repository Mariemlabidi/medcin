using medcin.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using medcin.Data;

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

        // GET: api/doctors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctors()
        {
            return await _context.Doctors
                .Include(d => d.Availabilities) 
                .Include(d => d.Appointments)
                .ToListAsync();
        }

        // GET: api/doctors/5
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

        // POST: api/doctors
        [HttpPost]
        public async Task<ActionResult<Doctor>> AddDoctor(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDoctor), new { id = doctor.Id }, doctor);
        }

        // PUT: api/doctors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, Doctor updatedDoctor)
        {
            if (id != updatedDoctor.Id)
                return BadRequest("L'identifiant ne correspond pas.");

            var existingDoctor = await _context.Doctors.FindAsync(id);
            if (existingDoctor == null)
                return NotFound();

            // Mettre à jour les champs
            existingDoctor.FullName = updatedDoctor.FullName;
            existingDoctor.Speciality = updatedDoctor.Speciality;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/doctors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors
                .Include(d => d.Appointments)
                .Include(d => d.Availabilities)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (doctor == null)
                return NotFound();

            // Supprimer les rendez-vous et disponibilités liés si nécessaire
            _context.Appointments.RemoveRange(doctor.Appointments);
            _context.Availabilities.RemoveRange(doctor.Availabilities);
            _context.Doctors.Remove(doctor);

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
