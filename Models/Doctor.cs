using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace medcin.Models
{
    public class Doctor
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;  

        public string Speciality { get; set; } = string.Empty;  

        public List<Appointment> Appointments { get; set; } = new();  

        public List<Availability> Availabilities { get; set; } = new();  
    }
}
