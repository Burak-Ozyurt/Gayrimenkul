using System.ComponentModel.DataAnnotations;

namespace Gayrimenkul.Models
{
    public class Category
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Kategori adı zorunludur")]
        [StringLength(50)]
        public string? Name { get; set; }
        
        public string? Description { get; set; }
        
        // Navigation property
        public ICollection<Property>? Properties { get; set; }
    }
}