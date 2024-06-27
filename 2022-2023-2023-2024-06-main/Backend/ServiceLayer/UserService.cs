using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using log4net;
using log4net.Config;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class UserService
    {       
        private readonly UserFacade _userFacade;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public UserService(UserFacade uf)
        {
            _userFacade = uf;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Log has started!");
        }
        /// <summary>
        /// Register a new user to the system
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="password">The password of the user</param>
        /// <returns>A JSON response that tells if the registration was done susccesfully.</returns>
        public string Register(string email, string password)
        {
            try
            {
                _userFacade.Register(email, password);
                log.Info("User was registered successfuly.");
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception ex)
            {
                log.Error("Registration failed.");
                return JsonSerializer.Serialize(new Response(ex.Message, null));
            }
        }
        /// <summary>
        /// Log-ins an existing user to the system.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>A JSON response object that tells if the user details were entered correctly.</returns>
        public string Login(string email, string password)
        {
            try
            {
                User u = _userFacade.Login(email, password);
                log.Info("User was connected to the system.");
                return JsonSerializer.Serialize(new Response(null, u.Email));
            }
            catch(Exception ex)
            {
                log.Error("Login failed.");
                return JsonSerializer.Serialize(new Response(ex.Message, null));
            }
        }
        /// <summary>
        /// Log-outs a signed-in user from the system.
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <returns>A Json response object that describes if the action was done successfuly.</returns>
        public string Logout(string email)
        {
            try
            {
                _userFacade.Logout(email);
                log.Info("User is out of the system.");
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception ex)
            {
                log.Error("Log-out failed.");
                return JsonSerializer.Serialize(new Response(ex.Message, null));
            }
        }
        ///<summary>This method loads all persisted data.
        /// </summary>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string LoadUsers()
        {
            try
            {
                _userFacade.LoadUsers();
                return JsonSerializer.Serialize(new Response());
            }
            catch(Exception ex)
            {
                log.Error("Could not load users.");
                return JsonSerializer.Serialize(new Response(ex.Message,null));
            }
        }

        ///<summary>This method deletes all persisted data.
        /// </summary>
        ///<returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string DeleteUsers()
        {
            try
            {
                log.Info("Users deletion thread started.");
                _userFacade.DeleteUsers();
                return JsonSerializer.Serialize(new Response());
            }
            catch(Exception ex)
            {
                log.Error("Users deletion thread failed.");
                return JsonSerializer.Serialize(new Response(ex.Message, null));
            }
        }

    }
}
