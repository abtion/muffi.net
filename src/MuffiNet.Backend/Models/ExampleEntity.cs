using System.ComponentModel.DataAnnotations;

namespace MuffiNet.Backend.Models
{
    public class ExampleEntity
    {
        [Required]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(30)]
        public string Phone { get; set; }
    }
}
