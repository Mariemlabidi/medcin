using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using medcin.Models;
using medcin.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace medcin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PatientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/patients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            return await _context.Patients
                .Include(p => p.Appointments)
                .ToListAsync();
        }

        // GET: api/patients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
            var patient = await _context.Patients
                .Include(p => p.Appointments)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null)
                return NotFound();

            return patient;
        }

        // POST: api/patients
        [HttpPost]
        public async Task<ActionResult<Patient>> AddPatient(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patient);
        }

        // PUT: api/patients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, Patient updatedPatient)
        {
            if (id != updatedPatient.Id)
                return BadRequest("L'identifiant ne correspond pas.");

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
                return NotFound();

            patient.FullName = updatedPatient.FullName;
            patient.BirthDate = updatedPatient.BirthDate;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/patients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patients
                .Include(p => p.Appointments)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null)
                return NotFound();

            // Supprimer les rendez-vous liés si nécessaire
            _context.Appointments.RemoveRange(patient.Appointments);
            _context.Patients.Remove(patient);

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
