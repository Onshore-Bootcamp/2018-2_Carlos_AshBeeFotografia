namespace AshBeeFotografia.Mapping
{
    using AshBeeFotografia.Models;
    using BusinessLayer;
    using BusinessLayer.Models;
    using DataLayer.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class AlbumMapper
    {
        public BusinessLogic albumCounter = new BusinessLogic();

        public static AlbumPO MapDoToPO(AlbumDO from)
        {
            AlbumPO to = new AlbumPO();
            try
            {
                to.AlbumId = from.AlbumId;
                to.UserId = from.UserId;
                to.AlbumName = from.AlbumName;
                to.AlbumDescription = from.AlbumDescription;
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
        public static List<AlbumPO> MapDoToPO(List<AlbumDO> from)
        {
            List<AlbumPO> to = new List<AlbumPO>();

            try
            {
                foreach (AlbumDO item in from)
                {
                    AlbumPO mappedItem = MapDoToPO(item);
                    to.Add(mappedItem);                    
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return to;
        }

        public static AlbumDO MapPoToDO(AlbumPO from)
        {
            AlbumDO to = new AlbumDO();
            try
            {
                to.AlbumId = from.AlbumId;
                to.UserId = from.UserId;
                to.AlbumName = from.AlbumName;
                to.AlbumDescription = from.AlbumDescription;
            }
            catch (Exception e)
            {
                throw e;
            }
            return to;
        }

        /// <summary>
        /// Mapping a List from the Presentation Layer
        /// </summary>
        /// <param name="from">List from the Presentation Layer</param>
        /// <returns></returns>
        public static List<AlbumDO> MapPoToDO(List<AlbumPO> from)
        {
            List<AlbumDO> to = new List<AlbumDO>();

            try
            {
                foreach (AlbumPO item in from)
                {
                    AlbumDO mappedItem = MapPoToDO(item);
                    to.Add(mappedItem);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return to;
        }







        public static PhotosBO MapPoToBO(AlbumPO from)
        {
            PhotosBO to = new PhotosBO();
            try
            {
                to.AlbumId = from.AlbumId;
                to.PhotoCount = from.PhotoCount;
            }
            catch (Exception e)
            {
                throw e;
            }
            return to;
        }

        /// <summary>
        /// Mapping a List from the Presentation Layer to Bussines Layer
        /// </summary>
        /// <param name="from">List from the Presentation Layer</param>
        /// <returns></returns>
        public static List<PhotosBO> MapPoToBO(List<AlbumPO> from)
        {
            List<PhotosBO> to = new List<PhotosBO>();

            try
            {
                foreach (AlbumPO item in from)
                {
                    PhotosBO mappedItem = MapPoToBO(item);
                    to.Add(mappedItem);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return to;
        }
    }
}