namespace AshBeeFotografia.Mapping
{
    using AshBeeFotografia.Models;
    using BusinessLayer.Models;
    using DataLayer.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class PhotosMapper
    {
        public static PhotosPO MapDoToPO(PhotosDO from)
        {
            PhotosPO to = new PhotosPO();
            try
            {
                to.PhotoId = from.PhotoId;
                to.AlbumId = from.AlbumId;
                to.PhotoLocation = from.PhotoLocation;
                to.PhotoName = from.PhotoName;
                to.PhotoDate = from.PhotoDate;
                to.Description = from.Description;
            }
            catch (Exception e)
            {
                throw e;
            }
            return to;
        }

        /// <summary>
        /// Mapping a List from the  Data Layer to the Presentation layer.
        /// </summary>
        /// <param name="from">List from the Data Layer</param>
        /// <returns></returns>
        public static List<PhotosPO> MapDoToPO(List<PhotosDO> from)
        {
            List<PhotosPO> to = new List<PhotosPO>();

            try
            {
                foreach (PhotosDO item in from)
                {
                    PhotosPO mappedItem = MapDoToPO(item);
                    to.Add(mappedItem);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return to;
        }

        public static PhotosDO MapPoToDO(PhotosPO from, string filePath)
        {
            PhotosDO to = new PhotosDO();
            try
            {
                to.PhotoId = from.PhotoId;
                to.AlbumId = from.AlbumId;
                to.PhotoLocation = filePath;
                to.PhotoName = from.PhotoName;
                to.PhotoDate = from.PhotoDate;
                to.Description = from.Description;
            }
            catch (Exception e)
            {
                throw e;
            }
            return to;
        }

        /// <summary>
        /// Mapping a List from the Presentation Layer To Data Layer.
        /// </summary>
        /// <param name="from">List from the Presentation Layer</param>
        /// <returns></returns>
        public static List<PhotosDO> MapPoToDO(List<PhotosPO> from, string filePath)
        {
            List<PhotosDO> to = new List<PhotosDO>();

            try
            {
                foreach (PhotosPO item in from)
                {
                    PhotosDO mappedItem = MapPoToDO(item, filePath);
                    to.Add(mappedItem);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return to;
        }


    }
}