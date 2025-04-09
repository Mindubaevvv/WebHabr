﻿using System.ComponentModel.DataAnnotations;

namespace WebHabr.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string AuthorId { get; set; }

    }
}
