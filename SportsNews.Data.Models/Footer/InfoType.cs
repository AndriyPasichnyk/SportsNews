namespace SportsNews.Data.Models
{
    // Company Info, Contributors, Newsletter
    public class InfoType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int FooterId { get; set; }
        public Footer Footer { get; set; }
    }
}