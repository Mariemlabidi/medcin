using System.Text.Json.Serialization;

namespace medcin.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public int DoctorId { get; set; }
        [JsonIgnore]
        public Doctor? Doctor { get; set; }

        public int PatientId { get; set; }
        [JsonIgnore]
        public Patient? Patient { get; set; }
    }

}
