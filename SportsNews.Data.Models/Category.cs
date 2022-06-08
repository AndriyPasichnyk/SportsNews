namespace SportsNews.Data.Models
{
    public class Category : ICategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool IsVisible { get; set; }

        public bool IsStatic { get; set; }
    }
}
