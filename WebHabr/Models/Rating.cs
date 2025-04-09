namespace WebHabr.Models
{
    public class Rating
    {
        public int Id { get; set; }

        public int Value { get; set; } // от 1 до 5

        public string UserId { get; set; }

        public int ArticleId { get; set; }
        public Article Article { get; set; }
    }
}
