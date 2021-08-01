using System;

namespace Seed.Models.V1.DTOs
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string Username { get; set; }
        public string PhotoUrl { get; set; }
    }
}
