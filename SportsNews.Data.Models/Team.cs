namespace SportsNews.Data.Models
{
    public class Team
    {
        public int Id { get; set; }

        public string Name{ get; set; }

        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }
    }
}
