using System;
using System.ComponentModel.DataAnnotations;

namespace LostDogApp.Models
{
    public class LostDogReportComment
    {
        public int Id { get; set; }

        [Required]
        public int LostDogReportId { get; set; }
        public LostDogReport LostDogReport { get; set; }

        [Required, StringLength(500)]
        public string Content { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
