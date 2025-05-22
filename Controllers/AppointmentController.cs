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
    public class AppointmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/appointments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .ToListAsync();
        }

        // GET: api/appointments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointment(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
                return NotFound();

            return appointment;
        }

        // POST: api/appointments
        [HttpPost]
        public async Task<ActionResult<Appointment>> CreateAppointment(Appointment appointment)
        {
            var doctor = await _context.Doctors
                .Include(d => d.Appointments)
                .FirstOrDefaultAsync(d => d.Id == appointment.DoctorId);

            var patientExists = await _context.Patients.AnyAsync(p => p.Id == appointment.PatientId);

            if (doctor == null)
                return BadRequest("Médecin introuvable.");

            if (!patientExists)
                return BadRequest("Patient introuvable.");

            // Ajouter explicitement la réservation à la collection du médecin
            doctor.Appointments.Add(appointment);

            // Ajouter l'appointment au contexte
            _context.Appointments.Add(appointment);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, appointment);
        }


        // PUT: api/appointments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, Appointment updatedAppointment)
        {
            if (id != updatedAppointment.Id)
                return BadRequest("L'identifiant ne correspond pas.");

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
                return NotFound();

            appointment.Date = updatedAppointment.Date;
            appointment.DoctorId = updatedAppointment.DoctorId;
            appointment.PatientId = updatedAppointment.PatientId;

            await _context.SaveChangesAsync();
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
                return NotFound();

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
