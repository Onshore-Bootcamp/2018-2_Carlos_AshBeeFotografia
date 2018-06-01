namespace BusinessLayer
{
    using BusinessLayer.Models;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BusinessLogic
    {


        public BusinessLogic()
        {

        }

        public long CountExceptions(DataTable log, string level)
        {
            var results =
                from myRow in log.Rows.Cast<DataRow>()
                where myRow.Field<string>("Level") == level
                select myRow;

            long count = results.Count<DataRow>();


            return count;
        }



        public List<PhotosBO> CountPhotosInAlbum(DataTable photos) 
        {
            List<PhotosBO> count = new List<PhotosBO>();
            //hold on to unique ids
            List<long> albumIds = new List<long>();

            //Take album id from each row and store into list.
            foreach (DataRow id in photos.Rows)
            {
                albumIds.Add((long)id["AlbumId"]);
            }
            //filter any non-unique ids.
            albumIds = albumIds.Distinct().ToList();

            //Count how many photos are attatched to the id.
            foreach (long id in albumIds)
            {

                //filtering photos by album id.
                var results = from myRow in photos.Rows.Cast<DataRow>()            
                                where myRow.Field<long>("AlbumId") == id
                                select myRow;

                PhotosBO photo = new PhotosBO();
                photo.AlbumId = id;
                photo.PhotoCount = results.Count<DataRow>();

                count.Add(photo);
            }

            return count;
        }
    }
}
