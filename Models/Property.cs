using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Gayrimenkul.Models
{
    public class Property
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Başlık zorunludur")]
        [StringLength(200)]
        public string? Title { get; set; }
        
        [Required(ErrorMessage = "Açıklama zorunludur")]
        [StringLength(2000)]
        public string? Description { get; set; }
        
        [Required(ErrorMessage = "Fiyat zorunludur")]
        [Range(0, double.MaxValue, ErrorMessage = "Fiyat 0'dan büyük olmalıdır")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        
        [Required(ErrorMessage = "Şehir zorunludur")]
        [StringLength(100)]
        public string? City { get; set; }
        
        [Required(ErrorMessage = "İlçe zorunludur")]
        [StringLength(100)]
        public string? District { get; set; }
        
        [StringLength(500)]
        public string? Address { get; set; }
        
        [Required(ErrorMessage = "Metrekare zorunludur")]
        [Range(1, int.MaxValue, ErrorMessage = "Metrekare 0'dan büyük olmalıdır")]
        public int SquareMeters { get; set; }
        
        [Required(ErrorMessage = "Oda sayısı zorunludur")]
        [Range(1, 20, ErrorMessage = "Oda sayısı 1-20 arası olmalıdır")]
        public int Rooms { get; set; }
        
        [Range(0, 20, ErrorMessage = "Banyo sayısı 0-20 arası olmalıdır")]
        public int Bathrooms { get; set; }
        
        public int Floor { get; set; }
        
        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [NotMapped]
        public IFormFile? ImageUpload { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Foreign Keys
        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
        
        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}