using System;
using System.ComponentModel.DataAnnotations;

namespace BE.DAL.Models
{
    public class User
    {
        public Guid Id { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string PasswordHash { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Role { get; set; } // Admin, Manager, Technician, Customer
        
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }

    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
