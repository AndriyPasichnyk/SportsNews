namespace SportsNews.Data.Models
{
    public class SocialNetwork
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool IsShareEnabled { get; set; }

        public bool IsFollowed { get; set; }
    }
}
