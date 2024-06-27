using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Threading.Tasks;
using System.Reflection;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class UserDTO
    {
        public const string EmailColumnName = "Email";
        public const string PasswordColumnName = "Password";
        public const string isLoggedColumnName = "isLogged";
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private UserController _controller;
        public string Email { get; set; }

        public string Password { get; set; }

        private int _isLogged;
        public int isLogged
        {
            get => _isLogged;
            set {
                _isLogged = value;
                log.Info("Update log status in database proccess has started.");
                _controller.Update(Email, isLoggedColumnName, value);
            }
        }

        public UserDTO(string email, string password, int isLogged)
        {
            Email = email;
            Password = password;
            _isLogged = isLogged;
            _controller = new UserController();
        }
        /// <summary>
        /// Inserts user to users table
        /// </summary>
        /// <returns> boolean value whether the Insert action has succeeded </returns>
        internal bool Insert()
        {
            if (_controller.Insert(this))
            {
                return true;
            }
            throw new ArgumentException($"error when try to Insert user to DB");
        }
        /// <summary>
        /// Deletes a user from users table
        /// </summary>
        /// <returns> boolean value whether the Delete action has succeeded </returns>
        internal bool Delete()
        {
            if (_controller.Delete(this))
            {
                return true;
            }
            throw new ArgumentException($"error when try to Delete user to DB");
        }
    }
}