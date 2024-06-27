using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class Column
    {
        public string title { get; set; }
        public int boardId { get; set; }
        public List<Task> tasks { get; set; }
        public int columnOrdinal { get; set; }
        public int limit { get; set; }
        private const int DEFAULT_COLUMN_LIMIT = -1;
        public ColumnDTO columnDTO;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public Column() { }

        internal Column(string title, int columnOrdinal, int boardId)
        {
            this.title = title;
            tasks = new List<Task>();
            this.columnOrdinal = columnOrdinal;
            this.boardId = boardId;
            limit = DEFAULT_COLUMN_LIMIT;
            columnDTO = new ColumnDTO(boardId, title, columnOrdinal, limit);
            log.Info("Column created succesfully.");
        }
        internal Column(string title, int columnOrdinal, int boardId, int limit)
        {
            this.title = title;
            tasks = new List<Task>();
            this.limit = limit;
            this.columnOrdinal = columnOrdinal;
            columnDTO = new ColumnDTO(boardId, title, columnOrdinal, limit);
            log.Info("Column created succesfully.");
        }
        /// <summary>
        /// This method adds a task to a specific board.
        /// </summary>
        /// <param name="task">The task we want to add</param>
        internal void AddTask(Task task)
        {
            log.Info("Checking if the number of tasks in column reached the limit.");
            if (tasks.Count == limit)
            {
                log.Debug("there is no room for more tasks in this column, change the limit to add more.");
                throw new ArgumentException("you've reached the maximum amount of tasks.");
            }
            else
            {
                log.Info("AddTask done successfully.");
                tasks.Add(task);
                task._taskDTO.Persist();
            }
        }
        /// <summary>
        /// This method removes a task from a specific board.
        /// </summary>
        /// <param name="taskToRemove">The task we want to remove</param>
        internal void RemoveTask(Task taskToRemove)
        {
            log.Info($"Seaching for the task{taskToRemove}.");
            if (tasks.Contains(taskToRemove))
            {
                taskToRemove._taskDTO.Delete();
                log.Info($"{taskToRemove} task removal done successfully.");
                tasks.Remove(taskToRemove);
            }
        }
        /// <summary>
        /// This method finds a specific task according to thw task's id.
        /// </summary>
        /// <param name="taskId">The task's id that we want to find</param>
        /// <returns>The task you want to find.</returns>
        internal Task FindTask(int taskId)
        {
            log.Info("searching the task in the column.");
            foreach (Task task in tasks)
                if (task.Id == taskId)
                {
                    log.Info("FindTask done successfully.");
                    return task;
                }
            log.Info("Task does not exist in this column.");
            return null;
        }
        /// <summary>
        /// This method assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="taskID">The task to be updated identified a task ID</param>        
        /// <param name="emailAssignee">Email of the asignee user</param>
        /// <returns>An empty response, unless an error occurs </returns>
        internal void AssignTask(int taskID, string emailAssignee, string email)
        {
            Task task = FindTask(taskID);
            // Task can be assigned if it was unassigned or only if it's previous assignee can assign it.
            if (task != null && (task.Assignee.Equals("") || task.Assignee.Equals(email)))
            {
                task.Assignee = emailAssignee;
                log.Info($"Task has been assigned to {emailAssignee}.");
            }
            else
            {
                log.Debug("Non existing task assignment attempt.");
                throw new Exception("Task ID does not exist!");
            }
        }

        /// <summary>
        /// This method loads all the tasks from the the data base
        /// </summary>
        /// <param name="boardId">The board that the task is on.</param>
        /// <param name="columnOrdinal">The column that the task is on.</param>
        public void LoadTasks(int boardId, int columnOrdinal)
        {
            List<TaskDTO> Dtasks = new TaskController().LoadColumnTasks(boardId, columnOrdinal);
            foreach (TaskDTO dTask in Dtasks)
            {
                Task dt = new Task(dTask);
                tasks.Add(dt);
            }
            log.Info($"All of the tasks in column {columnOrdinal} uploaded from db to the column");
        }

        internal void DeleteTasks(int ordinal, int boardId)
        {
            TaskController Tcontroller = new TaskController();
            Tcontroller.RemoveColumnTasks(ordinal, boardId);
            tasks.Clear();
        }
    }
}
