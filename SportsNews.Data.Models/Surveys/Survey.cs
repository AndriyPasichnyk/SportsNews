using System;
using System.Collections.Generic;
using System.Text;

namespace SportsNews.Data.Models
{
    public class Survey
    {
        public int Id { get; set; }

        public string Question { get; set; }

        public int SurveyStatusId { get; set; }
        public SurveyStatus Status { get; set; }

        public DateTime? ClosedDate { get; set; }

        public int ResponcesCount { get; set; }
    }
}
