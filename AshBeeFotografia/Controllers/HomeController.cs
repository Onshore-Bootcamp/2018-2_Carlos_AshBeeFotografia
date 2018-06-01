namespace AshBeeFotografia.Controllers
{
    using AshBeeFotografia.Mapping;
    using AshBeeFotografia.Models;
    using DataLayer;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        public HomeController()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            dataAccess = new PhotosDAO(connectionString);
            exceptionLog = new ExceptionHandler(connectionString);
        }

        private PhotosDAO dataAccess;
        private ExceptionHandler exceptionLog;

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Portfolio()
        {
            List<PhotosPO> mappedPhotos = new List<PhotosPO>();
            ActionResult oResult = RedirectToAction("Index", "Home");
            try
            {
                mappedPhotos = PhotosMapper.MapDoToPO(dataAccess.ViewPhotosById(4));
                oResult = View(mappedPhotos);
            }
            catch (Exception ex)
            {
                //Logs exception using exceptionLog class.
                exceptionLog.ExceptionLog("Critical", ex.Message, "HomeController", "Portfolio", ex.StackTrace);
            }

            return oResult;
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}