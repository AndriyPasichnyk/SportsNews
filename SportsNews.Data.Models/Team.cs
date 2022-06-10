namespace SportsNews.Data.Models
{
    public class Team : ICategory
    {
        public int Id { get; set; }

        public string Name{ get; set; }

        public bool IsVisible { get; set; }

        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }
    }
}
