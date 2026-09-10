using System;

namespace NandiniSareesAPIs.Features.Users
{
    public record UserDto(
        int Id,
        string FirstName,
        string? LastName,
        string Email,
        string? Phone,
        bool IsActive,
        DateTime CreatedAt
    );

    public class CreateUserRequest
    {
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public string Email { get; set; } = null!;
        public string? PasswordHash { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateUserRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public bool? IsActive { get; set; }
        public string? PasswordHash { get; set; }
    }
}
