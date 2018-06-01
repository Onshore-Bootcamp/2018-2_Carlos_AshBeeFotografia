namespace AshBeeFotografia.Mapping
{
    using AshBeeFotografia.Models;
    using DataLayer.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class UserMapper
    {
        /// <summary>
        /// Maps the User list from the datalayer to the presentation layer
        /// </summary>
        /// <param name="from">Data Object from the DataLayer</param>
        /// <returns></returns>
        public static UserPO MapDoToPO(UserDO from)
        {
            UserPO to = new UserPO();
            LoginPO login = new LoginPO();
            try
            {
                to.UserId = from.UserId;
                to.RoleId = from.RoleId;
                to.FirstName = from.Firstname;
                to.LastName = from.Lastname;
                to.Username = from.Username;
                to.Password = from.Password;
                to.Phone = from.Phone;
                to.Email = from.Email;
                login.Username = from.Username;
                login.Password = from.Password;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return to;
        }

        /// <summary>
        /// Mapping a List from the  Data Layer to the Presentation layer.
        /// </summary>
        /// <param name="from">List from the Data Layer</param>
        /// <returns></returns>
        public static List<UserPO> MapDoToPO(List<UserDO> from)
        {
            List<UserPO> to = new List<UserPO>();

            try
            {
                foreach (UserDO item in from)
                {
                    UserPO mappedItem = MapDoToPO(item);
                    to.Add(mappedItem);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return to;
        }

        public static UserDO MapPoToDO(UserPO from)
        {
            UserDO to = new UserDO();
            LoginPO login = new LoginPO();
            try
            {
                to.UserId = from.UserId;
                to.RoleId = from.RoleId;
                to.Firstname = from.FirstName;
                to.Lastname = from.LastName;
                to.Username = from.Username;
                to.Password = from.Password;
                to.Phone = from.Phone;
                to.Email = from.Email;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return to;
        }

        /// <summary>
        /// Mapping a List from the Presentation Layer
        /// </summary>
        /// <param name="from">List from the Presentation Layer</param>
        /// <returns></returns>
        public static List<UserDO> MapPoToDO(List<UserPO> from)
        {
            List<UserDO> to = new List<UserDO>();

            try
            {
                foreach (UserPO item in from)
                {
                    UserDO mappedItem = MapPoToDO(item);
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