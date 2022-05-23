using System;
using System.Collections.Generic;
using System.Text;

namespace SportsNews.Data.Models
{
    public class Banners
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int BannerStatusId { get; set; }
        public BannerStatus Status { get; set; }

        public int CategoryId { get; set; }
       // public Category PublishIn { get; set; }

        // do we need store Banner Image in database or in folder on disk???
        // if in databse - need separate table
        public string BannerImagePath { get; set; }
    }
}
