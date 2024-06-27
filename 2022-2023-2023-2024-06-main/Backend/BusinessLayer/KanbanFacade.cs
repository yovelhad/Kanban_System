using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using log4net;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class KanbanFacade
    {
        private UserFacade uf;
        public Dictionary<string, List<Board>> boardsPerUser;
        public int boardId;
        private Dictionary<int, Board> boardDict; // boardDict(id, Board)
        private const int DEFAULT_BOARD_ID = 1;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public KanbanFacade(UserFacade uf)
        {
            this.uf = uf;
            boardsPerUser = new Dictionary<string, List<Board>>();
            boardId = DEFAULT_BOARD_ID;
            boardDict = new Dictionary<int, Board>();
        }
        /// <summary>
        /// This method adds a board to a user account.
        /// </summary>
        /// <param name="email">The user's email(the one that wants to add a board)</param>
        /// <param name="name">The name of the board the user wants to add</param>
        internal void AddBoard(string email, string name)
        {
            log.Info($"Checking if {email} user exists and logged in.");
            if(email == null || !uf.users.ContainsKey(email))
            {
                log.Debug("Email is null or not exist.");
                throw new Exception($"{email} is an Invalid email input.");
            }
            if (uf.users[email].IsLoggedin())
            {
                if (name == null)
                    throw new ArgumentNullException("AddBoard got a null boardName as an input");
                if (boardsPerUser.ContainsKey(email))
                {
                    foreach (Board board in boardsPerUser[email])
                    {
                        if (board.boardName.Equals(name))
                        {
                            log.Debug($"{name} board is Already.");
                            throw new ArgumentException("You already added a board with this name");
                        }
                    }
                }

                List<Board> userBoards = getAllBoards(email);
                Board newBoard = new Board(name, email, boardId);
                if (boardDict.ContainsKey(boardId))
                    boardDict[boardId] = newBoard;
                else
                    boardDict.Add(boardId, newBoard);
                uf.GetUser(email).AddBoardToList(boardId);
                newBoard._memberDTO.Persist();
                newBoard.dBoard.Insert();
                userBoards.Add(newBoard);
                if (boardsPerUser.ContainsKey(email))
                    boardsPerUser[email] = userBoards;
                else
                    boardsPerUser.Add(email, userBoards);
                boardId++;
                log.Info("Board was added to user.");
            }
            else
            {
                log.Debug("AddBoard by logged-out user.");
                throw new ArgumentNullException("User who aren't logged in tries to add a board");
            }
        }
        /// <summary>
        /// This method deletes a board from a user account.
        /// </summary>
        /// <param name="email">The user's email(the one that wants to delete a board)</param>
        /// <param name="name">The name of the board the user wants to delete</param>
        internal void DeleteBoard(string email, string name)
        {
            if (name == null || email == null)
            {
                log.Debug("null name or email attempt.");
                throw new ArgumentNullException("DeleteBoard got a null boardName/email for an input");
            }
            log.Info($"Cheking if  {email} is logged in.");
            if (uf.users[email].IsLoggedin())
            {
                bool isRemoved = false;
                if (boardsPerUser.ContainsKey(email))
                {
                    Board board = GetBoard(email, name);
                    if (board.boardName.Equals(name) && board.ownerEmail.Equals(email))
                    {
                        boardsPerUser[email].Remove(board);
                        board.RemoveAllColumns();
                        foreach (string userEmail in board.members)
                        {
                            uf.GetUser(userEmail).DeleteBoardFromList(board.nextBoardId);
                            MemberDTO member = new MemberDTO(email, board.nextBoardId);
                            member.Delete();
                            board.dBoard.Delete();
                            boardDict.Remove(board.nextBoardId);
                            isRemoved = true;
                            log.Info($"The board {name} removed successfuly.");
                        }
                    }

                }
                if (!isRemoved)
                {
                    log.Debug("Non existing board deletion attempt.");
                    throw new ArgumentException("You have no board with this name");
                }
            }
            else
            {
                log.Debug($" {email} is not logged-in.");
                throw new ArgumentNullException("User who aren't logged in tries to Delete a board");
            }
        }
        /// <summary>
        /// This method returns all the boards that connected to a specific user.
        /// </summary>
        /// <param name="email">The user's email(the one that wants to get his boards)</param>
        /// <returns>A list of all the boards that connected to the user.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal List<Board> getAllBoards(string email)
        {
            if (email == null)
            {
                log.Debug("Email is null");
                throw new ArgumentNullException("Invalid input exception");
            }
            if(!uf.users.ContainsKey(email))
            {
                log.Debug("Non existed user attempt.");
                throw new ArgumentNullException("The email does not exist in the system.");
            }
            if (!boardsPerUser.ContainsKey(email))
                return new List<Board> { };
            log.Info($"boards of the email {email} were collected.");
            return boardsPerUser[email];
        }

        /// <summary>
        /// This method returns a specific board that connected to a specific user.
        /// </summary>
        /// <param name="email">The user's email(the one that we wants to get his board)</param>
        /// <param name="boardName">The name of the board the we wants to get</param>
        /// <returns>The board that we wanted.</returns>

        internal Board GetBoard(string email, string boardName)
        {
            bool isFound = false;
            Board toGet = null;
            if (boardName == null || email == null)
            {
                log.Debug("null name or email attempt.");
                throw new ArgumentNullException("got a null boardName as an input");
            }
            log.Info($"Cheking if  {email} is logged in.");
            if (uf.users[email].IsLoggedin())
            {
                if (!boardsPerUser.ContainsKey(email))
                {
                    return null;
                }
                foreach (Board board in boardsPerUser[email])
                {
                    if (board.boardName.Equals(boardName))
                    {
                        toGet = board;
                        isFound = true;
                    }
                }
                if (!isFound)
                {
                    log.Debug("Non existing board extraction attempt.");
                    throw new ArgumentException("You have no board with this name");
                }
            }
            else
            {
                log.Debug($" {email} is not logged-in.");
                throw new ArgumentNullException("User who isn't logged in tries to do an action");
            }
            log.Info("Board extracted.");
            return toGet;
        }

        //coloumn related functions
        /// <summary>
        /// This mwthod returns a specific column from a specific board.
        /// </summary>
        /// <param name="email">The user's email(the one that we wants to get the column from his board)</param>
        /// <param name="boardName">The board that the column is in</param>
        /// <param name="columnOrdinal">The ordinal of the column that we wants.</param>
        /// <returns>The column that we wanted to get.</returns>
        internal Column GetColumn(string email, string boardName, int columnOrdinal)
        {
            if (email != null && uf.users.ContainsKey(email))
            {
                Board board1 = GetBoard(email, boardName);
                if (board1 == null)
                {
                    log.Debug("Not existing board attempt.");
                    throw new Exception("board does not exist");
                }
                Column toFind = board1.GetColumn(columnOrdinal);
                log.Info("Column extracted.");
                return toFind;
            }
            log.Debug("GetColumn by not existing user.");
            throw new ArgumentException("Invalid user");
        }
        /// <summary>
        /// This  method limits the number of tasks in a specific column.
        /// </summary>
        /// <param name="email">The user's email(the one that we wants to limit the number of tasks in his colum)</param>
        /// <param name="boardName">The board where the column is in.</param>
        /// <param name="columnOrdinal">The ordinal of the column that we want to limit.</param>
        /// <param name="limit">The limitation that we want to place.</param>
        internal void LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            Column toSetLimit = GetColumn(email, boardName, columnOrdinal);
            if ((toSetLimit.tasks.Count == 0 && limit == 0) || (toSetLimit.tasks.Count > limit))
            {
                log.Debug("invalid limit input");
                throw new Exception("Invalid limitation");
            }
            else
            {
                toSetLimit.limit = limit;
                toSetLimit.columnDTO.Limit = limit;
                log.Info("Column limitation was done");
            }
        }
        /// <summary>
        /// This method returns the limit on the amount of tasks that exists on the column.
        /// </summary>
        /// <param name="email">The user's email(the one that we wants to get the limit of his colum)</param>
        /// <param name="boardName">The board where the column is in</param>
        /// <param name="columnOrdinal">The ordinal of the column</param>
        /// <returns>The limit of the column we searched.</returns>
        internal int GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            Column toGetLimit = GetColumn(email, boardName, columnOrdinal);
            int limit = toGetLimit.limit;
            log.Info("Column limit was extracted.");
            return limit;
        }

        /// <summary>
        /// This function returns the name oof a specific column.
        /// </summary>
        /// <param name="email">The user's email(the one that we wants to get the name of his colum)</param>
        /// <param name="boardName">The board where the column is in</param>
        /// <param name="columnOrdinal">The ordinal of the column</param>
        /// <returns>The name of the column we searched.</returns>
        internal string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            Column toGetName = GetColumn(email, boardName, columnOrdinal);
            string name = toGetName.title;
            log.Info("Cloumn name was extracted.");
            return name;
        }

        //Task related functions
        /// <summary>
        /// This method adds a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        internal void AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            if (title == null || (title.Length > 50 || title.Length < 1))
            {
                log.Debug("Invalid task title attempt.");
                throw new Exception("Title must be max. 50 characters, not empty ");
            }
            if (description == null || description.Length > 300)
            {
                log.Debug("Invalid task description attempt.");
                throw new Exception("Description must be max. 300 characters, can be empty ");
            }
            if (dueDate < DateTime.Now)
            {
                log.Debug("Illegal task due date attempt.");
                throw new Exception("date is not legit ");
            }
            Board b1 = GetBoard(email, boardName);
            if (b1 == null)
            {
                log.Debug("Not existing board attempt.");
                throw new Exception("board does not exist");
            }
            b1.AddTask(title, description, dueDate, email);
            log.Info("Task was added by user.");
        }

        /// <summary>
        /// Lists all the inProgress Tasks by userEmail.
        /// </summary>
        /// <param name="email">Email of the user.</param>
        /// <returns>A list with all the inProgress tasks</returns>
        internal List<Task> InProgressTasks(string email)
        {
            List<Board> allBoards = getAllBoards(email);
            List<Task> allTasks = new List<Task>();
            foreach (Board b in allBoards)
            {
                List<Task> tasks = b.InProgressTasks();
                foreach (Task task in tasks)
                {
                    allTasks.Add(task);
                }
            }
            log.Info("In progress tasks were collected.");
            return allTasks;
        }

        /// <summary>
        /// This method deletes a task.
        /// </summary>
        /// <param name="id">The task's id</param>
        /// <param name="email">The email of the owner of the task</param>
        /// <param name="boardName">The name of the board that contains the task.</param>
        internal void DeleteTask(int id, string email, string boardName)
        {
            Task toRemove = null;
            Board b1 = GetBoard(email, boardName);
            if(b1 == null)           
            {
               log.Debug("Not existing board attempt.");
               throw new Exception("board does not exist");
            }           
            List<Column> boardColumn = b1.GetColumns();
            foreach (Column column in boardColumn)
            {
                toRemove = column.FindTask(id);
                if (toRemove != null)
                    break;
            }
            if (toRemove != null)
                b1.DeleteTask(toRemove);
            else
            {
                log.Debug("Non-existing task deletion attempt by user.");
                throw new Exception("Task does not exict.");
            }
        }

        /// <summary>
        /// This method advances a task to its next state (a task that is in the DONE column will not be able to progress)
        /// </summary>
        /// <param name="email">The email of the owner of the task</param>
        /// <param name="boardName">The name of the board that contains the task.</param>
        /// <param name="columnOrdinal">The ordinal of the column that contains the task.</param>
        /// <param name="taskId">The task's id</param>
        internal void AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
        {
            Board b1 = GetBoard(email, boardName);
            if (b1 == null)
            {
                log.Debug("board was not found.");
                throw new Exception("board does not exist");
            }
            b1.AdvanceTask(columnOrdinal, taskId, email);
            log.Info("task was advanced.");
        }

        /// <summary>
        /// This method updates the task's duedate.
        /// </summary>
        /// <param name="email">The email of the owner of the task</param>
        /// <param name="boardName">The name of the board that contains the task.</param>
        /// <param name="columnOrdinal">The ordinal of the column that contains the task.</param>
        /// <param name="taskId">The task's id</param>
        /// <param name="dueDate">The new duedate for the task.</param>
        internal void UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            if (dueDate < DateTime.Now)
            {
                log.Debug("Invalid due date attempt.");
                throw new Exception("Due date must be set to the future.");
            }
            Board b1 = GetBoard(email, boardName);
            if (b1 == null)
            {
                log.Debug("Not existing board attempt.");
                throw new Exception("board does not exist");
            }
            Column c = b1.GetColumn(columnOrdinal);
            Task toUpdate = c.FindTask(taskId);

            if (toUpdate != null)
                b1.UpdateTaskDueDate(columnOrdinal, taskId, dueDate, email);
            else
            {
                log.Debug("Not exist task update attempt by user.");
                throw new Exception("Task does not exict.");
            }
            log.Info("Due date of the task was updated.");
        }

        /// <summary>
        /// This method updates the task's title.
        /// </summary>
        /// <param name="email">The email of the owner of the task</param>
        /// <param name="boardName">The name of the board that contains the task.</param>
        /// <param name="columnOrdinal">The ordinal of the column that contains the task.</param>
        /// <param name="taskId">The task's id</param>
        /// <param name="title">The new title for the task.</param>
        internal void UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            if (title == null || title.Length > 50)
            {
                log.Debug("Invalid task title attempt.");
                throw new Exception("Title must be max. 50 characters, not empty ");
            }
            Board b1 = GetBoard(email, boardName);
            if(b1 == null)
            {
                log.Debug("Not existing board attempt.");
                throw new Exception("board does not exist");
            }
            Column c = b1.GetColumn(columnOrdinal);
            Task toUpdate = c.FindTask(taskId);
            if (toUpdate != null)
            {
                b1.UpdateTaskTitle(columnOrdinal, taskId, title, email);
                log.Info("Task title was updated.");
            }
            else
            {
                log.Debug("Not exist task update attempt by user.");
                throw new Exception("Task does not exist.");
            }           
        }

        /// <summary>
        /// This method updates the task's description.
        /// </summary>
        /// <param name="email">The email of the owner of the task</param>
        /// <param name="boardName">The name of the board that contains the task.</param>
        /// <param name="columnOrdinal">The ordinal of the column that contains the task.</param>
        /// <param name="taskId">The task's id</param>
        /// <param name="description">The new description for the task.</param>
        internal void UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {
            if (description == null || description.Length > 300)
            {
                log.Debug("Invalid task description attempt.");
                throw new Exception("description must be max. 300 characters, can be empty ");
            }
            Board b1 = GetBoard(email, boardName);
            if (b1 == null)
            {
                log.Debug("Not existing board attempt.");
                throw new Exception("board does not exist");
            }
            Column c = b1.GetColumn(columnOrdinal);
            Task toUpdate = c.FindTask(taskId);
            if (toUpdate != null)
            {
                b1.UpdateTaskDescription(columnOrdinal, taskId, description, email);
                log.Info("Task description was updated.");
            }
            else
            {
                log.Debug("Not exist task update attempt by user.");
                throw new Exception("Task does not exist.");
            }   
        }


        /// <summary>
        /// This method adds a user as member to an existing board.
        /// </summary>
        /// <param name="email">The email of the user that joins the board. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>

        internal void JoinBoard(string email, int boardID)
        {
            log.Info("Join Board started.");
            if (email == null || !uf.users.ContainsKey(email))
            {
                log.Debug("Not existing or invalid user attempt.");
                throw new Exception("The user is not valid or does not exist.");
            }
            User user = uf.users[email];
            if (!user.IsLoggedin())
            {
                log.Debug("Join Board by logged-out user attempt");
                throw new Exception($"The user of email {email} must be logged-in!");
            }
            if (!boardDict.ContainsKey(boardID))
            {
                log.Debug("Not existing board ID attempt.");
                throw new Exception("Non existing board ID attempt.");
            }
            Board toJoin = boardDict[boardID];
            toJoin.JoinBoard(email);    
            user.AddBoardToList(boardID);
            List<Board> boards = getAllBoards(email);
            boards.Add(toJoin);
            boardsPerUser[email] = boards;
            log.Info("Adding the member to the database.");
            MemberDTO member = new MemberDTO(email, boardID);
            member.Persist();
        }

        /// <summary>
        /// This method removes a user from the members list of a board.
        /// </summary>
        /// <param name="email">The email of the user. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        internal void LeaveBoard(string email, int boardID)
        {
            if (email == null || !uf.users.ContainsKey(email))
            {
                log.Debug("Not existing or invalid user attempt.");
                throw new Exception("The user is not valid or does not exist.");
            }
            User user = uf.users[email];
            if (!user.IsLoggedin())
            {
                log.Debug("Leave Board by logged-out user attempt");
                throw new Exception($"The user of email {email} must be logged-in!");
            }
            log.Info("Checking if board ID exists.");
            if (!boardDict.ContainsKey(boardID))
            {
                log.Debug("Not existing board ID attempt.");
                throw new Exception("Non existing board ID attempt.");
            }
            Board toLeave = boardDict[boardID];
            toLeave.LeaveBoard(email);
            List<Board> boards = getAllBoards(email);
            boards.Remove(toLeave);
            user.DeleteBoardFromList(boardID);
            log.Info("Removing member from database");
            MemberDTO member = new MemberDTO(email, boardID);
            member.Delete();
        }

        /// <summary>
        /// This method assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column number. The first column is 0, the number increases by 1 for each column</param>
        /// <param name="taskID">The task to be updated identified a task ID</param>        
        /// <param name="emailAssignee">Email of the asignee user</param>
        internal void AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            log.Info("Checking the if the email exists");
            if (email == null || !uf.users.ContainsKey(email))
            {
                log.Debug("Not existing or invalid user attempt.");
                throw new Exception("The user is invalid or does not exist.");
            }
            Board board1 = GetBoard(emailAssignee, boardName);
            Board board2 = GetBoard(email, boardName);
            if (board1 == null && board2 == null)
            {
                log.Debug($"No board within both users with the name '{boardName}'!");
                throw new Exception("Both users does not have a board!");
            }
            Board toAssign = board1;
            if (board1 != null)
            {
                toAssign = board1;
                if (!board1.members.Contains(email))
                {
                    log.Debug($"The email '{email}' is not a member in the board.");
                    throw new Exception("The email must be a member in the board!");
                }
            }
            else if (board2 != null)
            {
                toAssign = board2;
                if (!board2.members.Contains(emailAssignee))
                {
                    log.Debug($"The email '{emailAssignee}' is not a member in the board.");
                    throw new Exception("The email must be a member in the board!");
                }
            }
            Column col = toAssign.GetColumn(columnOrdinal);
            col.AssignTask(taskID, emailAssignee, email); ;
        }

        /// <summary>
        /// This method loads all the boards from the data base.
        /// </summary>
        internal void LoadAllBoards()
        {
            BoardController bc = new BoardController();
            IList<BoardDTO> myList = bc.LoadAllBoards();
            foreach (BoardDTO DBoard in myList)
            {
                Board tmpBoard = new Board(DBoard);
                tmpBoard.LoadColumns();
                //update dicts
                boardDict.Add(DBoard.BoardId, tmpBoard);
                if (!boardsPerUser.ContainsKey(DBoard.ownerEmail))
                {
                    List<Board> boards = new List<Board>
                    {
                        tmpBoard
                    };
                    boardsPerUser.Add(DBoard.ownerEmail, boards);
                }
                else
                    boardsPerUser[DBoard.ownerEmail].Add(tmpBoard);
            }
            boardId = myList.Count+1;
            log.Info($"all of the boards were Inserted to businessLayer BoardController");
            //upload members
            LoadBoardMembers();
        }
        /// <summary>
        /// This method loads the board's members. 
        /// </summary>
        internal void LoadBoardMembers()
        {
            List<MemberDTO> DMembers = new MemberController().LoadBoardMembers();
            foreach (MemberDTO member in DMembers)
            {
                //update board
                boardDict[member.BoardID].members.Add(member.Email);
                //update user
                uf.GetUser(member.Email).AddBoardToList(member.BoardID);
            }
            log.Info("all of the boards members  were Inserted to businessLayer BoardController");
        }

        /// <summary>
        /// This method deletes all the boards from the database.
        /// </summary>
        internal void DeleteAllBoards()
        {
            if (!new MemberController().DeleteAllMembers())
            {
                log.Error("can't Delete from MemberController");
                throw new ArgumentException("can't Delete from MemberController");
            }
            if (!new BoardController().DeleteAllBoards())
            {
                log.Error("can't Delete from BoardController");
                throw new ArgumentException("can't Delete from BoardController");
            }
            if (!new ColumnController().DeleteAllColumns())
            {
                log.Error("can't Delete from ColumnController");
                throw new ArgumentException("can't Delete from ColumnController");
            }
            if (!new TaskController().DeleteAllTasks())
            {
                log.Error("can't Delete from DalTaskController");
                throw new ArgumentException("can't Delete from DalTaskController");
            }
            boardsPerUser.Clear();
            boardDict.Clear();
            log.Info("all the board, members, columns and tasks are deleted from the db!");
        }
        /// <summary>
        /// This method returns a speccific board's name.
        /// </summary>
        /// <param name="boardId">The id of the board.</param>
        /// <returns>The board's name.</returns>
        internal string GetBoardName(int boardId)
        {
            log.Info("Board name was extracted.");
            if (boardId < 0)
                throw new ArgumentException("illegal boardId");
            if (!boardDict.ContainsKey(boardId))
                throw new ArgumentException("the board doesn't exist");
            return boardDict[boardId].boardName;
        }
        /// <summary>
        /// This method returns all of the boards that the user is a member in them.
        /// </summary>
        /// <param name="userEmail">The email of the user.</param>
        /// <returns>A list that contains the boards of the user.</returns>
        public List<int> GetUserBoards(string userEmail)
        {
            if (userEmail == null)
                throw new ArgumentException("GetUserBoards got a null userEmail for an input");
            if (!uf.users.ContainsKey(userEmail))
                throw new ArgumentException("the user doesn't exist in the system!");
            if (!uf.users[userEmail].IsLoggedin())
                throw new ArgumentException("the user isn't logged in to the system!");
            User user = uf.GetUser(userEmail);
            return user.getBoardIds();
        }

        /// <summary>
        /// This method changes the ownership of a board(only the corrent owner can do this.)
        /// </summary>
        /// <param name="currentOwnerEmail">The email of the user that own the board.</param>
        /// <param name="newOwnerEmail">The email of the user that we want as the new owner of the board.</param>
        /// <param name="boardName">The board that we to change his owner.</param>
        internal void TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            log.Info("Checking if the emails are valid.");
            if((currentOwnerEmail == null) || (newOwnerEmail == null))
            {
                log.Debug("null credentials attempt.");
                throw new Exception("Email is null");
            }
            log.Info("Checking if current owner exist.");
            if (!uf.users.ContainsKey(newOwnerEmail) || !uf.users.ContainsKey(currentOwnerEmail))
            {
                log.Debug("The user tried to transfer ownership from/to a user that does not exist. ");
                throw new Exception("E-mail/s do not exist.");
            }
            if (boardsPerUser.ContainsKey(currentOwnerEmail))
            {
                log.Info("Checking if board exist in CorrentUserBoards.");
                Board boardToChange = GetBoard(currentOwnerEmail, boardName);
                if (!boardToChange.members.Contains(newOwnerEmail))
                {
                    log.Debug("The new owner is not a member in this board.");
                    throw new Exception("Those users does not share a board.");
                }
                boardToChange.TransferOwnership(currentOwnerEmail, newOwnerEmail);
                List<Board> newList = getAllBoards(newOwnerEmail);
                if (!newList.Contains(boardToChange))
                    newList.Add(boardToChange);
                if (!boardsPerUser.ContainsKey(newOwnerEmail))
                    boardsPerUser.Add(newOwnerEmail, newList);
                log.Info("Ownership transfered succesfully.");
            }
            else
            {
                log.Debug("The current user does not have a board.");
                throw new Exception("Unhandled board within current user.");
            }
        }
        /// <summary>
        /// Retrieves the column limit
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="columnOrdinal"></param>
        /// <returns> A column limit </returns>
        /// <exception cref="NotSupportedException"></exception>
        internal int GetColumnLimit(int boardId, int columnOrdinal)
        {
            if (!boardDict.ContainsKey(boardId))
                throw new Exception($"board: {boardId} does not exist");
            Board board = boardDict[boardId];
            log.Debug("Column limit extracted succesfully");
            return board.GetColumn(columnOrdinal).limit;
        }
        /// <summary>
        /// This method returns a specific column from a specific board.
        /// </summary>
        /// <param name="email">The user's email(the one that we wants to get the column from his board)</param>
        /// <param name="boardName">The board that the column is in</param>
        /// <param name="columnOrdinal">The ordinal of the column that we wants.</param>
        /// <returns>The column that we wanted to get.</returns>
        internal Column GetColumn(int boardId, int columnOrdinal)
        {
            if (!boardDict.ContainsKey(boardId))
                throw new Exception($"Does not contain board: {boardId}");
            Board board = boardDict[boardId];
            log.Debug("Column extracted succesfully");
            return board.GetColumn(columnOrdinal);
        }
    }
}
