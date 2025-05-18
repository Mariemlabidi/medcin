using System.Text.Json.Serialization;

namespace medcin.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        [JsonIgnore]
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();
    }

}
