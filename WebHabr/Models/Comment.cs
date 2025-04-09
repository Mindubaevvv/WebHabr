using System.ComponentModel.DataAnnotations;

namespace WebHabr.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string UserId { get; set; }

        public int ArticleId { get; set; }
        public Article Article { get; set; }
    }
}
