namespace medcin.Models
{
    public class RegisterModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public DateTime BirthDate { get; set; }
    }
}