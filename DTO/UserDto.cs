namespace smart_hotel_management.DTO
{
    public class UserDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; } // Dropdown value: "User" or "Manager"
        public string? ContactNumber { get; set; }
    
}
}
