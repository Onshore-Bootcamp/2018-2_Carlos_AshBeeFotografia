using System;
using System.ComponentModel.DataAnnotations;

namespace AshBeeFotografia.Models
{
    public class AlbumPO
    {
        public long AlbumId { get; set; }
        public long UserId { get; set; }

        public long PhotoCount { get; set; }

        [Required]
        public string AlbumName { get; set; }
        [Required]
        public string AlbumDescription { get; set; }
    }
}