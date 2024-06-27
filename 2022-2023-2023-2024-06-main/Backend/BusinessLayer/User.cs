using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class User
    {
        public string Email { get; set; }
        private string Password { get; set; }
        private bool isLoggedin { get; set; }     
        internal UserDTO _userDTO { get; set; }
        private List<int> boardIds { get;}

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        internal User(string email, string password)
        {
            this.Email = email;
            this.Password = password;
            isLoggedin = true;
            _userDTO = new UserDTO(email, password, 1);
            boardIds = new List<int>();
        }

        /// <summary>
        /// Checks if the user is logged-in.
        /// </summary>
        /// <returns>True if the user is logged-in , False otherwise.</returns>
        internal bool IsLoggedin() { return isLoggedin; }

        /// <summary>
        /// The method logs in a user into the system, the user must be registered.
        /// </summary>
        /// <param name="password"></param>
        /// <returns>True if the login succeeded, False otherwise.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal bool Login(string password)
        {
            log.Info("Checking if the user is already logged-in.");
           // if (isLoggedin)
            //{
            //    log.Debug("logged-in user attempt.");
            //    throw new InvalidOperationException("The user is already logged-in!");
            //}
            log.Info("Checking if the password is correct.");
            if (this.Password.Equals(password))
            {
                isLoggedin = true;
                log.Info("log-in done successfully.");
                _userDTO.isLogged = 1;
                return true;
            }
            log.Debug("Incorrect password.");
            return false;
        }
        /// <summary>
        /// Logs-out a user of the system.
        /// </summary>
        /// <returns>True if the operation succeeded, False otherwise.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal bool Logout()
        {
            log.Info("Checking if the user already logged-out.");
            if (!isLoggedin)
            {
                log.Debug("logged-out user attempt.");
                return false;
                throw new InvalidOperationException("The user is already logged-out!");
            }
            _userDTO.isLogged = 0;
            isLoggedin = false;
            log.Info("log-out done successfully.");
            return true;
        }
        /// <summary>
        /// Adds the ID of a board to the list of boards the user has.
        /// </summary>
        /// <param name="boardId"></param>
        /// <exception cref="ArgumentException"></exception>
        public void AddBoardToList(int boardId)
        {
            if (boardId < 0 || boardIds.Contains(boardId))
            {
                log.Error($"user: {Email}, attempted to add illigel board id to his list");
                throw new ArgumentException("Board id is not valid to enter the borad list of user");
            }
            else
                boardIds.Add(boardId);
            log.Info($"user:{Email} added to his board's list. boardId:{boardId}");
        }

        /// <summary>
        /// Deletes the ID of a board from the list of boards the user has.
        /// </summary>
        /// <param name="boardId"></param>
        /// <exception cref="ArgumentException"></exception>
        public void DeleteBoardFromList(int boardId)
        {
            Console.WriteLine(boardId);
            if (boardId < 0)
            {
                log.Error($"user: {Email}, attempted to add illigel board id to his list");
                throw new ArgumentException("Board id not valid to enter the borad list of user");
            }
            if (!boardIds.Contains(boardId))
            {
                log.Error($"user: {Email}, attempted to remove board that not in his board list");
                throw new ArgumentException("attempted to remove board that is not in his board list");
            }
            boardIds.Remove(boardId);
            log.Info($"user: {Email} remove borad with id {boardId}");
        }
        /// <summary>
        /// Returns all the board ID's the user has.
        /// </summary>
        /// <returns></returns>
        public List<int> getBoardIds() { return boardIds; }
    }
}