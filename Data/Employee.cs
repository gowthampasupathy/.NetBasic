    namespace WebApplication1.Data
    {
        public class Employee
        {
            public Guid Id { get; set; }
            public required string Name { get; set; }
            public required string Email { get; set; }
        }
    }