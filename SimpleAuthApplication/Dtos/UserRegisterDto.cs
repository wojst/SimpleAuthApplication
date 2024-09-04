namespace SimpleAuthApplication.Dtos
{
    public class UserRegisterDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string JobPosition { get; set; }
        public string EmploymentType { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
