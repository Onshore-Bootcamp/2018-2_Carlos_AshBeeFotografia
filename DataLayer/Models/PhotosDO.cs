
using System;

namespace DataLayer.Models
{
    public class PhotosDO
    {
        public long PhotoId { get; set; }
        public long AlbumId { get; set; }
        public string PhotoLocation { get; set; }
        public string PhotoName { get; set; }
        public DateTime PhotoDate { get; set; }
        public string Description { get; set; }
    }
}
