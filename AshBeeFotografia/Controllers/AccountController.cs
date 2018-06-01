namespace AshBeeFotografia.Controllers
{
    using DataLayer;
    using DataLayer.Models;
    using AshBeeFotografia.Mapping;
    using AshBeeFotografia.Models;
    using System.Web.Mvc;
    using System;
    using System.Configuration;
    using System.Collections.Generic;
    using AshBeeFotografia.Custom;
    using System.Data;
    using BusinessLayer;
    using BusinessLayer.Models;

    public class AccountController : Controller
    {

        public AccountController()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            dataAccess = new UserDAO(connectionString);
            exceptionLog = new ExceptionHandler(connectionString);
            businessAccess = new BusinessLogic();
        }

        //Dependencies
        private UserDAO dataAccess;
        private ExceptionHandler exceptionLog;
        private BusinessLogic businessAccess;


        //Index Admin
        [HttpGet]
        [CheckRole("~/Home/Index", "RoleId", 1)]
        public ActionResult Index()
        {
            ActionResult oResponse = View();
            try
            {
                //Display all users to the admin and provide actions to authenticated users.                
                List<UserDO> dataObjects = dataAccess.ReadUsers();
                List<UserPO> mappedItems = UserMapper.MapDoToPO(dataObjects);

                //Set redirect to view and pass a list of users.
                oResponse = View(mappedItems);

                //Display Exceptions to Admin
                long exceptionsList = businessAccess.CountExceptions(dataAccess.ReadAllExceptions(),"Critical");
            }
            catch (Exception ex)
            {
                //Logs exception using exceptionLog class.
                exceptionLog.ExceptionLog("Critical", ex.Message, "AccountController", "Index", ex.StackTrace);
            }
            return oResponse;
        }

        #region Login
        //Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Login(LoginPO user)
        {
            //Checking if model state is valid
            ActionResult oResponse = RedirectToAction("Index", "Home");
            if (ModelState.IsValid)
            {
                try
                {
                    //Checking to see if username and password match user table.
                    UserDO fromTable = dataAccess.ViewUserByName(user.Username);
                    if (fromTable.UserId != 0 && fromTable.Password == user.Password)
                    {
                        //Username/password correct, give session ids and username.
                        Session["UserId"] = fromTable.UserId;
                        Session["Username"] = fromTable.Username;
                        Session["RoleId"] = fromTable.RoleId;
                    }
                    else
                    {
                        //Username/password did not match user table or didn't exist.                        
                        ModelState.AddModelError("Password", "Incorrect username / password.");
                        oResponse = View(user);
                    }
                }
                catch (Exception ex)
                {
                    //Logs exception using exceptionLog class.
                    exceptionLog.ExceptionLog("Critical", ex.Message, "AccountController", "Login", ex.StackTrace);
                    ModelState.AddModelError("Password", "Incorrect username / password.");
                    oResponse = View(user);
                }
            }
            else
            {
                //Let's user know that modelstate wasn't valid.
                ModelState.AddModelError("Password", "Both fields are required.");

                //Sets redirect to view passing user object.
                oResponse = View(user);
            }
            return oResponse;
        }
        #endregion

        //Logout
        [HttpGet]
        public ActionResult Logout()
        {
            //Clears session
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        #region Register
        //Register new user
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserPO user)
        {
            //Defaults redirect to Login page of account controller.
            ActionResult oResponse = RedirectToAction("Login", "Account");

            //Checking to see if admin for redirect.
            if (Session["RoleId"] != null)
            {
                if ((long)Session["RoleId"] == 1)
                {
                    //User is admin set redirect to index of account controller.
                    oResponse = RedirectToAction("Index", "Account");
                }
            }

            //Checking modelstate.
            if (ModelState.IsValid)
            {
                try
                {
                    //Instantiaing a mapped datalayer object and passing it to the datalayer.
                    UserDO to = UserMapper.MapPoToDO(user);
                    dataAccess.CreateUser(to);

                    //Sending user a message to confirm user was successfully added to table.
                    TempData["Message"] = "Registration successful.";
                }
                catch (Exception ex)
                {
                    //Logs exception using exceptionLog class.
                    exceptionLog.ExceptionLog("Critical", ex.Message, "AccountController", "Register", ex.StackTrace);
                    oResponse = View(user);
                }
            }
            else
            {
                oResponse = View(user);
            }

            return oResponse;
        }
        #endregion

        #region Modify user
        [HttpGet]
        [CheckRole("~/Home/Index", "RoleId", 1)]
        public ActionResult Modify(long userId)
        {
            //Defaults redirect to view.
            ActionResult oResponse = View();
            if (ModelState.IsValid)
            {
                try
                {
                    UserDO data = dataAccess.ViewUserById(userId);
                    UserPO display = UserMapper.MapDoToPO(data);
                    //Sets redirect to view passing User info from table using stored procedure.
                    oResponse = View(display);
                }
                catch (Exception ex)
                {
                    //Logs exception using exceptionLog class.
                    exceptionLog.ExceptionLog("Critical", ex.Message, "AccountController", "Modify", ex.StackTrace);

                    //Sets redirect to view passing userId.
                    oResponse = View(userId);
                }
            }
            else
            {
                //Redirects to view passing userId.
                oResponse = View(userId);
            }
            return oResponse;
        }

        [HttpPost]
        public ActionResult Modify(UserPO user)
        {
            ActionResult oResponse = RedirectToAction("Index");
            if (ModelState.IsValid && user.UserId != 0)
            {
                try
                {
                    UserDO to = UserMapper.MapPoToDO(user);
                    dataAccess.UpdateUser(to);
                    TempData["Message"] = $"{to.Username} successfully modified.";
                }
                catch (Exception ex)
                {
                    //Logs exception using exceptionLog class.
                    exceptionLog.ExceptionLog("Critical", ex.Message, "AccuntController", "Modify", ex.StackTrace);
                    oResponse = View(user);
                }
            }
            else
            {
                oResponse = View(user);
            }
            return oResponse;
        }
        #endregion

        #region Delete user
        [HttpGet]
        [CheckRole("~/Home/Index", "RoleId", 1)]
        public ActionResult Delete(long userId)
        {
            //Defaulting redirect to index of account controller.
            ActionResult oResponse = RedirectToAction("Index", "Account");
            try
            {
                //Deletes user from database by userId using a stored procedure.
                //Tells user that delete was successfull.
                dataAccess.DeleteUser(userId);
                TempData["Message"] = "User successfully deleted.";
            }
            catch (Exception ex)
            {
                //Logs exception using exceptionLog class.
                exceptionLog.ExceptionLog("Critical", ex.Message, "AccountController", "Delete", ex.StackTrace);
                oResponse = View(userId);
            }
            return oResponse;
        }
        #endregion
    }
}