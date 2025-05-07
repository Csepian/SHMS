namespace SHMS.DTO
{
    public class UserDTO
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; } // Dropdown value: "User" or "Manager"
        public string? ContactNumber { get; set; }
    }
}