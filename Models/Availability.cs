using System;
using System.Text.Json.Serialization;

namespace medcin.Models
{
    public class Availability
    {
        public int Id { get; set; }

        public DayOfWeek Day { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public int DoctorId { get; set; }

        [JsonIgnore]
        public Doctor? Doctor { get; set; }   
    }
}
