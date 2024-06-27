using System;
using System.Collections.Generic;
using log4net;
using System.Text.RegularExpressions;
using IntroSE.Kanban.Backend.DataAccessLayer;
using System.Reflection;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class UserFacade
    {
        public Dictionary<string, User> users;       

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public UserFacade()
        {
            users = new Dictionary<string, User>();
        }
        /// <summary>
        /// This method registers a new user to the system.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>a new user.</returns>
        internal User Register(string email, string password)
        {
            log.Info("Checking the credentials validation.");
            if (!isEmailValid(email))
            {
                log.Debug("User with invalid email registration attempt.");
                throw new ArgumentException("Illegal email address.");
            }
            if (!isPasswordValid(password))
            {
                log.Debug("User with invalid password registration attempt.");
                throw new ArgumentException("Your password is illegal.");
            }
            log.Info("Checking if the email already exists.");
            if (users.ContainsKey(email))
            {
                log.Debug("User with existing email address registration attempt.");
                throw new ArgumentException($"The email {email} already exists.");
            }
            UserDTO userDTO = new UserDTO(email, password, 1);
            User u = new User(email, password);
            users[email] = u;
            userDTO.Insert();
            log.Info("The user has been created in the system.");
            return u;

        }

        /// <summary>
        ///  This method logs in an existing user.
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>a user that logged in.</returns>
        internal User Login(string email, string password)
        {

            log.Info("Checking if the email is valid or exists in the system.");
            if(email == null || password == null)
            {
                log.Debug("null login credentials attempt by user.");
                throw new InvalidOperationException("invalid credentials");
            }
            if (!users.ContainsKey(email))
            {
                log.Debug(users.Count + "Unsuccesful login attempt by user.");
                throw new InvalidOperationException("The email isn't correct or does not exist.");
            }
            else
            {
                User u = users[email];
                if (!u.Login(password))
                {
                    log.Debug("Incorrect password.");
                    throw new InvalidOperationException("The password isn't correct.");
                }
                u._userDTO.isLogged = 1;
                return u;
            }
        }

        /// <summary>
        /// This method logs out a logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        internal void Logout(string email)
        {
            if (email == null)
            {
                log.Debug("null login credentials attempt by user.");
                throw new InvalidOperationException("invalid credentials");
            }
            if (!users.ContainsKey(email))
            {
                log.Debug("Unregistered user log-out attempt.");
                throw new InvalidOperationException("The email isn't correct or does not exist.");
            }
            else
            {
                User u = users[email];
                u.Logout();
                u._userDTO.isLogged = 0;
            }
        }
        /// <summary>
        /// This method checks if a password is valid. 
        /// </summary>
        /// <param name="password">A password we want to check.</param>
        /// <returns>a boolean variable, true if it was valid and false otherwise.</returns>
        internal bool isPasswordValid(string password)
        {
            log.Info("Checking the password validation.");
            if (password == null)
                return false;
            int countSmallchar = 0;
            int countLargechar = 0;
            int countNumbers = 0;
            bool longPass = password.Length >= 6 && password.Length <= 20;
            for (int i = 0; i < password.Length; i++)
            {
                if (password[i] >= 'a' && password[i] <= 'z')
                    countSmallchar++;
                if (password[i] >= 'A' && password[i] <= 'Z')
                    countLargechar++;
                if (password[i] >= '0' && password[i] <= '9')
                    countNumbers++;
            }
            if (countSmallchar < 1 || countLargechar < 1 || countNumbers < 1 || !longPass)
                return false;
            return true;
        }
        /// <summary>
        /// This method checks if an email is valid.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>a boolean variable, true if it was valid and false otherwise.</returns>
        internal bool isEmailValid(string email)
        {
            log.Info("Checking the email address validation.");
            bool ifSpaced = string.IsNullOrWhiteSpace(email);

            // Regular expression to validate the email address format
            string pattern = @"^[\w\.]+@([\w-]+\.)+[\w-]{2,4}$";

            Regex regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            return (!ifSpaced && regex.IsMatch(email));
        }

        /// <summary>
        /// This method loads all the users from the database.
        /// </summary>
        internal void LoadUsers()
        {
            UserController controller = new UserController();
            List<UserDTO>  dataBaseUsers = controller.LoadAllUsers();
            foreach(UserDTO user in dataBaseUsers)
            {
                User toAdd = new User(user.Email,user.Password);
                users.Add(user.Email,toAdd);
                log.Info(users.Count + "All users were inserted to the system.");
            }
        }

        /// <summary>
        /// This method deletes all the users from the data base.
        /// </summary>
        internal void DeleteUsers()
        {
            UserController controller = new UserController();
            bool deleted = controller.DeleteAllUsers();
            if (!deleted)
            {
                throw new Exception("Could not complete the data deletion procces.");
            }
            users.Clear();
        }

        /// <summary>
        /// This methos return a specific user.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <returns>The user we wanted to find.</returns>
        public User GetUser(string email)
        {
            if (email == null)
            {
                log.Error("GetUser got a null email as input");
                throw new ArgumentException("The email you entered is null");
            }
            if (!users.ContainsKey(email))
            {
                log.Error($"GetUser got an non-existing email as input: {email}");
                throw new ArgumentException("There isn't an account with the email you entered");
            }
            return users[email];
        }
    }
}
