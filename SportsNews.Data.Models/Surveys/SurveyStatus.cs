namespace SportsNews.Data.Models
{
    public class SurveyStatus
    {
        public int Id { get; set; }

        // Not Published, Published, Closed
        public string Status { get; set; }
    }
}