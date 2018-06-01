namespace AshBeeFotografia.Controllers
{
    using AshBeeFotografia.Custom;
    using AshBeeFotografia.Mapping;
    using AshBeeFotografia.Models;
    using BusinessLayer;
    using BusinessLayer.Models;
    using DataLayer;
    using DataLayer.Models;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class AlbumController : Controller
    {
        public AlbumController()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            dataAccess = new AlbumDAO(connectionString);
            userData = new UserDAO(connectionString);
            allPhotos = new PhotosDAO(connectionString);
            exceptionLog = new ExceptionHandler(connectionString);
        }

        //Dependencies
        private AlbumDAO dataAccess;
        private UserDAO userData;
        private PhotosDAO allPhotos;
        private ExceptionHandler exceptionLog;

        // GET: Album        
        [CheckRole("~/Home/Index", "RoleId", 1, 2, 3)]
        public ActionResult Index()
        {
            List<AlbumPO> mappedItems = new List<AlbumPO>();
            try
            {
                //Business Logic
                BusinessLogic countPhotos = new BusinessLogic();
                List<PhotosBO> fromBL = countPhotos.CountPhotosInAlbum(allPhotos.ReadPhotosForCount());

                ViewBag.PhotoCount = new List<PhotosBO>();
                foreach (PhotosBO item in fromBL)
                {
                    ViewBag.PhotoCount.Add(new PhotosBO() { AlbumId = item.AlbumId, PhotoCount = item.PhotoCount });
                }


                if ((long)Session["RoleId"] <= 2)
                {
                    //User is an Admin. Viewing all albums
                    List<AlbumDO> dataObjects = dataAccess.ReadAlbum();
                    mappedItems = AlbumMapper.MapDoToPO(dataObjects);
                }
                else
                {
                    //Display albums that belong to user and provide actions to authenticated users.                
                    List<AlbumDO> dataObjects = dataAccess.ViewAlbumByUserId((long)Session["UserId"]);
                    mappedItems = AlbumMapper.MapDoToPO(dataObjects);
                }
            }
            catch (Exception ex)
            {
                //Logs exception using exceptionLog class.
                exceptionLog.ExceptionLog("Critical", ex.Message, "AlbumController", "Index", ex.StackTrace);
            }
            return View(mappedItems);
        }



        //Create
        [HttpGet]
        [CheckRole("~/Home/Index", "RoleId", 1)]
        public ActionResult Create()
        {
            List<UserDO> dataObjects = userData.ReadUsers();
            ViewBag.DropDown = new List<SelectListItem>();
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (UserDO user in dataObjects)
                    {
                        //Adds username and user Id to a dropdown list of users in viewbag.
                        ViewBag.DropDown.Add(new SelectListItem() { Text = user.Username, Value = user.UserId.ToString() });
                    }
                }
                catch (Exception ex)
                {
                    //Logs exception using exceptionLog class.
                    exceptionLog.ExceptionLog("Critical", ex.Message, "AlbumController", "Create", ex.StackTrace);
                }
            }
            else
            {
                //Modelstate was invalid.
                try
                {
                    foreach (UserDO user in dataObjects)
                    {
                        //Adds username and user Id to a dropdown list of users in viewbag.
                        ViewBag.DropDown.Add(new SelectListItem() { Text = user.Username, Value = user.UserId.ToString() });

                    }
                }
                catch (Exception ex)
                {
                    //Logs exception using exceptionLog class.
                    exceptionLog.ExceptionLog("Critical", ex.Message, "AlbumController", "Create", ex.StackTrace);
                }
            }


            return View();
        }

        [HttpPost]
        [CheckRole("~/Account/Login", "RoleId", 1)]
        public ActionResult Create(AlbumPO album)
        {
            //Defaults redirect to index of album controller.
            ActionResult oResponse = RedirectToAction("Index", "Album");
            if (ModelState.IsValid)
            {
                try
                {
                    //Adds album to datatable using valid album.
                    dataAccess.CreateAlbum(AlbumMapper.MapPoToDO(album));
                    TempData["Message"] = "Album successfully created.";
                }
                catch (Exception e)
                {
                    //Logs exception using exceptionLog class.
                    exceptionLog.ExceptionLog("Critical", e.Message, "AlbumController", "Create", e.StackTrace);


                    try
                    {
                        List<UserDO> dataObjects = userData.ReadUsers();
                        ViewBag.DropDown = new List<SelectListItem>();
                        foreach (UserDO user in dataObjects)
                        {
                            //Adds username and user Id to a dropdown list of users in viewbag.
                            ViewBag.DropDown.Add(new SelectListItem() { Text = user.Username, Value = user.UserId.ToString() });
                        }
                    }
                    catch (Exception ex)
                    {
                        //Logs exception using exceptionLog class.
                        exceptionLog.ExceptionLog("Critical", ex.Message, "AlbumController", "Create", ex.StackTrace);
                    }

                    oResponse = View(album);
                }
            }
            else
            {
                //Modelstate was invalid.
                try
                {
                    List<UserDO> dataObjects = userData.ReadUsers();
                    ViewBag.DropDown = new List<SelectListItem>();
                    foreach (UserDO user in dataObjects)
                    {
                        //Adds username and user Id to a dropdown list of users in viewbag.
                        ViewBag.DropDown.Add(new SelectListItem() { Text = user.Username, Value = user.UserId.ToString() });
                    }
                }
                catch (Exception ex)
                {
                    //Logs exception using exceptionLog class.
                    exceptionLog.ExceptionLog("Critical", ex.Message, "AlbumController", "Create", ex.StackTrace);
                }
                oResponse = View(album);
            }
            return oResponse;
        }

        //Update
        [HttpGet]
        [CheckRole("~/Account/Login", "RoleId", 1, 2)]
        public ActionResult Update(long albumId)
        {
            AlbumPO mappedAlbum = new AlbumPO();
            ViewBag.DropDown = new List<SelectListItem>();

            //Defaults redirect to view.
            ActionResult oResponse = View();
            if (ModelState.IsValid)
            {
                try
                {
                    //List username and Id in the viewbag as to use for a dropdown list in view.
                    List<UserDO> dataObjects = userData.ReadUsers();
                    mappedAlbum = AlbumMapper.MapDoToPO(dataAccess.ViewAlbumById(albumId));

                    foreach (UserDO user in dataObjects)
                    {
                        //Adds username and user Id to a dropdown list of users in viewbag.
                        ViewBag.DropDown.Add(new SelectListItem() { Text = user.Username, Value = user.UserId.ToString() });
                    }
                    //Returns mappedalbum to view.
                    oResponse = View(mappedAlbum);
                }
                catch (Exception ex)
                {
                    //Logs exception using exceptionLog class.
                    exceptionLog.ExceptionLog("Critical", ex.Message, "AlbumController", "Update", ex.StackTrace);

                    //Sets redirect to index of album controller.
                    oResponse = RedirectToAction("Index", "Album");
                }
            }
            else
            {
                //Returns albumId to view.
                oResponse = View(albumId);
            }

            return oResponse;
        }

        [HttpPost]
        [CheckRole("~/Home/Index", "RoleId", 1, 2)]
        public ActionResult Update(AlbumPO album)
        {
            //Defaulting redirect to index of album controller
            ActionResult OResponse = RedirectToAction("Index", "Album");
            if (ModelState.IsValid)
            {
                try
                {
                    //Updates album in datatable using valid user input.
                    dataAccess.UpdateAlbum(AlbumMapper.MapPoToDO(album));
                    TempData["Message"] = "Album successfully updated.";
                }
                catch (Exception ex)
                {
                    //Logs exception using exceptionLog class.
                    exceptionLog.ExceptionLog("Critical", ex.Message, "AlbumController", "Update", ex.StackTrace);
                    OResponse = View(album);
                }
            }
            else
            {
                //Returns album to view.
                OResponse = View(album);
            }

            return OResponse;
        }


        //Delete
        [HttpGet]
        [CheckRole("~/Account/Login", "RoleId", 1)]
        public ActionResult Delete(long albumId)
        {

            List<AlbumPO> mappedAlbums = new List<AlbumPO>();
            ActionResult oResponse = RedirectToAction("Index", "Album", new { mappedAlbums });
            if (ModelState.IsValid)
            {
                try
                {
                    dataAccess.DeleteAlbum(albumId);
                    List<AlbumDO> albums = dataAccess.ReadAlbum();
                    mappedAlbums = AlbumMapper.MapDoToPO(albums);
                    TempData["Message"] = "Album successfully deleted.";
                }
                catch (Exception ex)
                {
                    //Logs exception using exceptionLog class.
                    exceptionLog.ExceptionLog("Critical", ex.Message, "AlbumController", "Delete", ex.StackTrace);

                    //Returns albumId to view.
                    oResponse = View(albumId);
                }
            }
            else
            {
                //Returns albumId to view.
                oResponse = View(albumId);
            }
            return oResponse;

        }
    }
}