namespace BusinessLayer.Models
{
    using System;

    public class PhotosBO
    {
        //Album Count properties.
        public long AlbumId { get; set; }
        public long PhotoCount { get; set; }

        //ExceptionLog properties.
        public long ExceptionId { get; set; }
        public string ExceptionLevel { get; set; }
        public long ExceptionCount { get; set; }
        public string ExceptionMessage { get; set; }
        public DateTime ExceptionTimeStamp { get; set; }
        public string CurrentClass { get; set; }
        public string CurrentMethod { get; set; }
        public string StackTrace { get; set; }


    }
}
