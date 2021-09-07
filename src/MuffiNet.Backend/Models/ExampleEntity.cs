using System.ComponentModel.DataAnnotations;

namespace MuffiNet.Backend.Models
{
    public class ExampleEntity
    {
        [Required]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        public string Phone { get; set; } = string.Empty;
    }
}
