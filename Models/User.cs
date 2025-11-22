using System.ComponentModel.DataAnnotations;

namespace Gayrimenkul.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Ad Soyad zorunludur")]
        [StringLength(100)]
        public string? FullName { get; set; }
        
        [Required(ErrorMessage = "Email zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir email giriniz")]
        public string? Email { get; set; }
        
        [Required(ErrorMessage = "Şifre zorunludur")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter olmalıdır")]
        public string? Password { get; set; }
        
        [Phone]
        public string? Phone { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Navigation property
        public ICollection<Property> ?Properties { get; set; }
    }
}