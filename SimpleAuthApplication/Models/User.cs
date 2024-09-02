namespace SimpleAuthApplication.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string JobPosition { get; set; }
        public string EmploymentType { get; set; }

        
        public Auth Auth { get; set; }
    }
}
