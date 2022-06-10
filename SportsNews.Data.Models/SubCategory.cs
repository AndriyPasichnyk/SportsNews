namespace SportsNews.Data.Models
{
    public class SubCategory : ICategory
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsVisible { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
