using System;
using System.ComponentModel.DataAnnotations;

namespace AshBeeFotografia.Models
{
    public class PhotosPO
    {
        public long PhotoId { get; set; }
        public long AlbumId { get; set; }

        
        public string PhotoLocation { get; set; }

        
        public string PhotoName { get; set; }

        
        public DateTime PhotoDate { get; set; }

        
        public string Description { get; set; }
    }
}