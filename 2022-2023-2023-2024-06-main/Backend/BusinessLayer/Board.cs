using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using log4net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Reflection;
using IntroSE.Kanban.Backend.DataAccessLayer;
using System.Collections;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class Board
    {
        private List<Column> boardColumns;
        internal int nextTaskId { get; set; }
        internal int nextBoardId { get; set; }
        internal string boardName { get; set; }

        internal string ownerEmail { get; set; }
        public BoardDTO dBoard;
        private const int DEFAULT_TASK_ID = 1;

        public const int BACKLOG = 0;
        public const int IN_PROGRESS = 1;
        public const int DONE = 2;

        internal List<string> members { get; set; }
        internal MemberDTO _memberDTO { get; set; }

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Board(string boardName, string ownerEmail, int boardId)
        {
            this.boardName = boardName;
            this.ownerEmail = ownerEmail;
            boardColumns = new List<Column>();
            nextTaskId = DEFAULT_TASK_ID;
            nextBoardId = boardId;
            members = new List<string>
            {
                ownerEmail
            };
            _memberDTO = new MemberDTO(ownerEmail, nextBoardId);
            AddColumn("backlog", BACKLOG);
            AddColumn("in progress", IN_PROGRESS);
            AddColumn("done", DONE);
            dBoard = new BoardDTO(boardId, boardName, ownerEmail);
            log.Info("Board was built in the system.");
        }
        public Board(BoardDTO dBoard)
        {
            boardName = dBoard.BoardName;
            ownerEmail = dBoard.ownerEmail;
            nextBoardId = dBoard.BoardId;
            nextTaskId = DEFAULT_TASK_ID;
            boardColumns = new List<Column>();
            members = new List<string>();
            _memberDTO = new MemberDTO(ownerEmail, nextBoardId);
            this.dBoard = new BoardDTO(nextBoardId, boardName, ownerEmail);
            log.Info("Board was built in the system.");
        }

        /// <summary>
        /// The method creates a column in the board.
        /// </summary>
        /// <param name="title">The title of the column (backlog / in-progress / done).</param>
        /// <param name="coloumnOrdinal">The index of the column (0 / 1 / 2)</param>
        internal void AddColumn(string title, int coloumnOrdinal)
        {
            Column column = new Column(title, coloumnOrdinal, nextBoardId);
            boardColumns.Add(column);
            ColumnDTO dColumn = new ColumnDTO(nextBoardId, title, coloumnOrdinal, -1);
            dColumn.Persist();
        }
        /// <summary>
        /// The method removes all the columns in the board.
        /// </summary>
        internal void RemoveAllColumns()
        {
            int index = BACKLOG;
            foreach (Column c in boardColumns)
            {
                c.DeleteTasks(index, nextBoardId);
                c.columnDTO.Delete();
                index++;
            }
        }
        /// <summary>
        /// The method adds a new task in the board.
        /// </summary>
        /// <param name="title">Title of the task</param>
        /// <param name="description">Description of the task</param>
        /// <param name="dueDate">Due date of the task</param>
        /// <param name="assignee">The email assignee of the task</param>
        /// <exception cref="ArgumentNullException"></exception>
        internal void AddTask(string title, string description, DateTime dueDate, string assignee)
            {
                if (title == null || description == null || dueDate.Equals(null))
                {
                    log.Debug("null values in task attempt.");
                    throw new ArgumentNullException("Illegal input");
                }
                foreach (Column column in boardColumns)
                {
                    if (column.columnOrdinal == BACKLOG) // add null checks
                    {
                        Task newTask = new Task(title, description, dueDate, nextTaskId);
                        newTask._taskDTO = new TaskDTO(nextTaskId, nextBoardId, 0, "", title, description, DateTime.Now, dueDate);
                        column.AddTask(newTask);
                    }
                }
                nextTaskId++;
            }
            /// <summary>
            /// The method moves the task to the requested column.
            /// </summary>
            /// <param name="columnOrdinal">The requested column to be advanced.</param>
            /// <param name="taskId">The id of the task.</param>
            /// <exception cref="Exception"></exception>
            /// <exception cref="ArgumentException"></exception>
            internal void AdvanceTask(int columnOrdinal, int taskId, string email)
            { 
                log.Info("Checking if the task already done.");
                if (email == null)
                {
                    log.Debug("null email attempt.");
                    throw new ArgumentNullException("got a null email as an input");
                }
                if (columnOrdinal == DONE)
                {
                    log.Debug("Done task advance attempt by user.");
                    throw new Exception("Task is done, can't move it");
                }
                log.Info("Checking the limitation of the next column.");
                if (GetColumn(columnOrdinal + 1).limit == GetColumn(columnOrdinal + 1).tasks.Count)
                {
                    log.Debug("Task addition to full column attempt.");
                    throw new Exception("The next column has reached the full number of tasks!");
                }
                Column c = GetColumn(columnOrdinal);
                if (c.FindTask(taskId) == null)
                {
                    log.Debug("task doesn't exist.");
                    throw new ArgumentException("task doesn't exist.");
                }
                Task task = c.FindTask(taskId);
                if (!task.Assignee.Equals(email))
                {
                    throw new Exception("Advance task can be done by its assignee only.");
                }
                Column current = GetColumn(columnOrdinal);
                current.RemoveTask(task);
                Column toAdvance = GetColumn(columnOrdinal + 1);
                task._taskDTO = new TaskDTO(taskId, nextBoardId, columnOrdinal + 1, task.Assignee, task.Title, task.Description, DateTime.Now, task.DueDate);
                toAdvance.AddTask(task);

            }
            /// <summary>
            /// The method deletes the task from the board.
            /// </summary>
            /// <param name="task"></param>
            internal void DeleteTask(Task task)
            {
                foreach (Column column in boardColumns)
                {
                    task._taskDTO = new TaskDTO(nextTaskId, nextBoardId, column.columnOrdinal, task.Assignee, task.Title, task.Description, task.CreationTime, task.DueDate);
                    column.RemoveTask(task);
                }
                log.Info("Task removed from column");
            }
            /// <summary>
            /// The method returns a list of all the in-progress tasks in the board.
            /// </summary>
            internal List<Task> InProgressTasks()
            {
                List<Task> tasks = new List<Task>();
                foreach (Column column in boardColumns)
                {
                    if (column.columnOrdinal == IN_PROGRESS)
                    {
                        foreach (Task task in column.tasks)
                            tasks.Add(task);
                    }
                }
                log.Info("In-progress tasks were collected.");
                return tasks;
            }
            /// <summary>
            /// The method updates the due date of the task.
            /// </summary>
            /// <param name="columnOrdinal">The requested column ordinal of the task.</param>
            /// <param name="taskId">The id of the task.</param>
            /// <param name="dueDate">The due date of the task.</param>
            /// <param name="email">The assignee's email of the task.</param>
            /// <exception cref="Exception"></exception>
            internal void UpdateTaskDueDate(int columnOrdinal, int taskId, DateTime dueDate, string email)
            {
                if (columnOrdinal == DONE || dueDate.Equals(null) || dueDate < DateTime.Now)
                {
                    log.Debug("Task credentials are not updatable.");
                    throw new Exception("Can't update the task due to its credentials.");
                }
                Task task = GetColumn(columnOrdinal).FindTask(taskId);
                if (task.Assignee.Equals(email))
                {
                    task.DueDate = dueDate;
                }
                else
                {
                    throw new Exception("An update to a task can be done by its assignee only.");
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="columnOrdinal">The requested column ordinal of the task.</param>
            /// <param name="taskId">The id of the task.</param>
            /// <param name="title">The title of the task to be updated.</param>
            /// <param name="email">The assignee's email of the task.</param>
            /// <exception cref="Exception"></exception>
            internal void UpdateTaskTitle(int columnOrdinal, int taskId, string title, string email)
            {
                if (columnOrdinal == DONE)
                {
                    log.Debug("Done task advance attempt by user.");
                    throw new Exception("Task is done, can't change it!");
                }
                Task task = GetColumn(columnOrdinal).FindTask(taskId);
                if (task.Assignee.Equals(email))
                {
                    task.Title = title;
                }
                else
                {
                    log.Debug("Not assigned user attempt.");
                    throw new Exception("An update to a task can be done by its assignee only.");
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="columnOrdinal">The requested column ordinal of the task.</param>
            /// <param name="taskId">The id of the task.</param>
            /// <param name="description">The description of the task to be updated.</param>
            /// <param name="email">The assignee's email of the task.</param>
            /// <exception cref="Exception"></exception>

            internal void UpdateTaskDescription(int columnOrdinal, int taskId, string description, string email)
            {
                if (columnOrdinal == DONE)
                {
                    log.Debug("Done task advance attempt by user.");
                    throw new Exception("Task is done, can't change it!");
                }
                Task task = GetColumn(columnOrdinal).FindTask(taskId);
                if (task.Assignee.Equals(email))
                {
                    task.Description = description;
                }
                else
                {
                    log.Debug("Not assigned user attempt.");
                    throw new Exception("An update to a task can be done by its assignee only.");
                }
            }
            /// <summary>
            /// The method returns the column in the requested column ordinal of the board.
            /// </summary>
            /// <param name="columnOrdinal">The requested column ordinal of the task.</param>
            /// <exception cref="Exception"></exception>
            internal Column GetColumn(int columnOrdinal)
            {
                foreach (Column column in boardColumns)
                {
                    if (column.columnOrdinal == columnOrdinal)
                    {
                        log.Info("Column was found with his ordinal number.");
                        return column;
                    }
                }
                log.Debug("Extraction from non existing column attempt.");
                throw new Exception("No such column ordinal!!");
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns>A list of all columns of the board.</returns>
            internal List<Column> GetColumns() {
                log.Info("Columns of the board were extracted.");
                return boardColumns;
            }
            /// <summary>
            /// Loads all columns of the board into the db.
            /// </summary>
            internal void LoadColumns()
            {
                ColumnController dc = new ColumnController();
                List<ColumnDTO> list = dc.LoadBoardColumns(nextBoardId);
                foreach (ColumnDTO Dcolumn in list)
                {
                    Column column = new Column(Dcolumn.Title, Dcolumn.ColumnOrdinal, Dcolumn.BoardId, Dcolumn.Limit);
                    column.LoadTasks(nextBoardId, Dcolumn.ColumnOrdinal);
                    boardColumns.Add(column);
                    nextTaskId += column.tasks.Count;
                }
                log.Info("all the columns are loaded");
            }

            /// Milestone 2 //

            /// <summary>
            /// The method transfers the ownership of the board to other user, must be existed.
            /// </summary>
            /// <param name="currentOwnerEmail">The email of the current owner of the board.</param>
            /// <param name="newOwnerEmail">The email of the new owner of the board.</param>
            /// <exception cref="Exception"></exception>
            internal void TransferOwnership(string currentOwnerEmail, string newOwnerEmail)
            {
                if (!currentOwnerEmail.Equals(ownerEmail))
                {
                    log.Debug("The owner of the board doesn't match the current owner");
                    throw new Exception("The owner of the board doesn't match the current owner");
                }
                ownerEmail = newOwnerEmail;
                dBoard = new BoardDTO(nextBoardId, boardName, ownerEmail);
                dBoard.Delete();
                dBoard.ownerEmail = newOwnerEmail;
                dBoard.Insert();
                log.Info($"user: {currentOwnerEmail} has transfered ownership of board {boardName} id: {nextBoardId} to user {newOwnerEmail} ");
            }

            /// <summary>
            /// The method adds the user as a member in the board.
            /// </summary>
            /// <param name="email">The email of the new member in the board.</param>
            /// <exception cref="Exception"></exception>
            internal void JoinBoard(string email)
            {
                if (!members.Contains(email))
                {
                    members.Add(email);
                    log.Info($"User {email} has joined the board.");
                }
                else
                {
                    log.Debug("Already existed user in board attempt.");
                    throw new Exception($"The user {email} is already in the board.");
                }
            }

            /// <summary>
            /// The method removes the email from the members of the board.
            /// </summary>
            /// <param name="email">The email of the member in the board.</param>
            /// <exception cref="Exception"></exception>

            internal void LeaveBoard(string email)
            {
                if (email == ownerEmail)
                {
                    log.Debug("Owner board leaving attempt.");
                    throw new Exception("A board owner cannot leave his own board.");
                }
                Column inprogress = GetColumn(1);
                Column backlog = GetColumn(0);
                foreach (Task task in inprogress.tasks)
                {
                    if (task.Assignee == email)
                    {
                        task.Assignee = "";
                        log.Info($"The task {task.Title} is unassigned from {email}");
                    }
                }
                foreach (Task task in backlog.tasks)
                {
                    if (task.Assignee == email)
                    {
                        task.Assignee = "";
                        log.Info($"The task {task.Title} is unassigned from {email}");
                    }
                }
                members.Remove(email);
                log.Info($"The user {email} has left the board {boardName}");
            }

        
    }
}
