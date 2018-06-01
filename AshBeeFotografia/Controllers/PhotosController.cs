namespace AshBeeFotografia.Controllers
{
    using DataLayer;
    using DataLayer.Models;
    using AshBeeFotografia.Mapping;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using AshBeeFotografia.Models;
    using AshBeeFotografia.Custom;

    public class PhotosController : Controller
    {

        public PhotosController()
        {
            //Creating Dependencies using connection string.
            string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            dataAccess = new PhotosDAO(connectionString);
            albumData = new AlbumDAO(connectionString);
            exceptionLog = new ExceptionHandler(connectionString);
        }

        //Dependencies
        private PhotosDAO dataAccess;
        private AlbumDAO albumData;
        private ExceptionHandler exceptionLog;

        [HttpGet]
        [CheckRole("~/Home/Index", "RoleId", 1, 2, 3)]
        public ActionResult Index(long albumId)
        {
            //Instanciating a list of photo objects to fill.
            List<PhotosPO> mappedItems = new List<PhotosPO>();
            try
            {
                //Display photos that belong to user and provide actions to authenticated users.                
                List<PhotosDO> dataObjects = dataAccess.ViewPhotosById(albumId);
                mappedItems = PhotosMapper.MapDoToPO(dataObjects);
            }
            catch (Exception ex)
            {
                //Logs exception using exceptionLog class.
                exceptionLog.ExceptionLog("Critical", ex.Message, "PhotosController", "Index", ex.StackTrace);
                TempData["Error"] = ex.Message;
            }

            return View(mappedItems);
        }
        [HttpGet]
        [CheckRole("~/Home/Index", "RoleId", 1)]
        public ActionResult UploadPhoto()
        {
            //Instanciating a new list of selectlistitem to fill dropdown.
            ViewBag.DropDown = new List<SelectListItem>();
            try
            {
                //Stores Album name and Id in viewbag as a list to use for a dropdown list in view.
                List<AlbumDO> dataObjects = albumData.ReadAlbum();
                foreach (AlbumDO item in dataObjects)
                {
                    ViewBag.DropDown.Add(new SelectListItem() { Text = item.AlbumName, Value = item.AlbumId.ToString() });
                }
            }
            catch (Exception ex)
            {
                //Logs exception using exceptionLog class.
                exceptionLog.ExceptionLog("Critical", ex.Message, "PhotosController", "Index", ex.StackTrace);
            }

            return View();

        }

        [HttpPost]
        public ActionResult UploadPhoto(HttpPostedFileBase uploadedPhoto, PhotosPO photo)
        {
            //Defaults redirect to index of album controller.
            ActionResult oResult = RedirectToAction("Index", "Album");
            if (ModelState.IsValid)
            {
                try
                {
                    //Gets filepath
                    List<FileInfo> files = Directory.GetFiles("/").Select(path => new FileInfo(path)).ToList();

                    //Creates a unique id for naming save files to prevent overriding, saves file in
                    //userPhotos folder of current directory.
                    string newName = Guid.NewGuid().ToString() + uploadedPhoto.FileName.Remove(0, uploadedPhoto.FileName.IndexOf('.'));
                    string pathToSaveTo = Path.Combine(Server.MapPath("/userPhotos/"), newName);

                    //Uploads photo to userPhotos folder in current directory.
                    uploadedPhoto.SaveAs(pathToSaveTo);
                    pathToSaveTo = $"~/userPhotos/{newName}";

                    //Adds photo to table using a stored procedure and properties gathered from user input.
                    dataAccess.CreatePhoto(PhotosMapper.MapPoToDO(photo, pathToSaveTo));

                    //Lets user know that upload was successful.
                    TempData["Message"] = "Photo upload successful.";
                }
                catch (Exception ex)
                {
                    //Logs exception using exceptionLog class.
                    exceptionLog.ExceptionLog("Critical", ex.Message, "PhotosController", "UploadPhoto", ex.StackTrace);
                    TempData["Error"] = "Oops there was a problem uploading your photo, please try again.";
                }
            }
            else
            {
                //Instanciating a new list of selectlistitem to fill dropdown.
                ViewBag.DropDown = new List<SelectListItem>();
                try
                {
                    //Stores Album name and Id in viewbag as a list to use for a dropdown list in view.
                    List<AlbumDO> dataObjects = albumData.ReadAlbum();
                    foreach (AlbumDO item in dataObjects)
                    {
                        ViewBag.DropDown.Add(new SelectListItem() { Text = item.AlbumName, Value = item.AlbumId.ToString() });
                    }
                }
                catch (Exception e)
                {
                    //Logs exception using exceptionLog class.
                    exceptionLog.ExceptionLog("Critical", e.Message, "PhotosController", "UploadPhoto", e.StackTrace);
                }

                //Modelstate wasn't valid, returning photo to view.
                oResult = View(photo);
            }

            return oResult;
        }


        //Update
        [HttpGet]
        [CheckRole("~/Home/Index", "RoleId", 1, 2)]
        public ActionResult Update(long photoId)
        {
            //Getting photo by photo Id and storing in a presentation object.
            PhotosPO mappedPhoto = PhotosMapper.MapDoToPO(dataAccess.ViewPhotoByPhotoId(photoId));

            //Puts Album name and Id into viewbag as a list to use for a dropdown list in view.
            ViewBag.DropDown = new List<SelectListItem>();
            List<AlbumDO> dataObjects = albumData.ReadAlbum();
            try
            {
                foreach (AlbumDO item in dataObjects)
                {
                    //Filling a SelectListItem with with all AlbumName and AlbumId properties.
                    ViewBag.DropDown.Add(new SelectListItem() { Text = item.AlbumName, Value = item.AlbumId.ToString() });
                }
            }
            catch (Exception ex)
            {
                //Logs exception using exceptionLog class.
                exceptionLog.ExceptionLog("Critical", ex.Message, "PhotosController", "Update", ex.StackTrace);
            }

            return View(mappedPhoto);
        }

        [HttpPost]
        public ActionResult Update(PhotosPO photos)
        {
            //Defaults redirect to index of photos controller passing albumId.
            ActionResult oResult = RedirectToAction("Index", "Photos", new { albumId = photos.AlbumId });
            if (ModelState.IsValid)
            {
                try
                {
                    //Passing photo object and photo location to use in stored procedure.
                    dataAccess.UpdatePhoto(PhotosMapper.MapPoToDO(photos, photos.PhotoLocation));
                    TempData["Message"] = "Photo successfully updated.";
                }
                catch (Exception ex)
                {
                    //Logs exception using exceptionLog class.
                    exceptionLog.ExceptionLog("Critical", ex.Message, "PhotosController", "Update", ex.StackTrace);
                    oResult = View(new { albumId = photos.AlbumId });
                }
            }
            else
            {
                //returns the albumId to the view.
                oResult = View(new { albumId = photos.AlbumId });
            }

            return oResult;
        }

        //Delete
        [HttpGet]
        [CheckRole("~/Home/Index", "RoleId", 1)]
        public ActionResult Delete(long photoId)
        {
            //Defaults redirect to Index of album controllor.
            ActionResult oResult = RedirectToAction("Index", "Album");
            if (ModelState.IsValid)
            {
                try
                {
                    //Passing photo object and photo location by photoId to use in a stored procedure for delete.
                    dataAccess.DeletePhoto(photoId, dataAccess.ViewPhotoLocation(photoId));
                    TempData["Message"] = "Photo successfully deleted.";
                }
                catch (Exception ex)
                {
                    //Logs exception using exceptionLog class.
                    exceptionLog.ExceptionLog("Critical", ex.Message, "PhotosController", "Index", ex.StackTrace);                    
                    oResult = View(photoId);
                }
            }
            else
            {
                //Returning photoId to view.
                oResult = View(photoId);
            }

            return oResult;
        }
    }
}