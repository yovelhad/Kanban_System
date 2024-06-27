using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using IntroSE.Kanban.Backend.BusinessLayer;
using log4net.Config;
using log4net;
using System.IO;
using System.Reflection;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class TaskService
    {
        private readonly KanbanFacade kf;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public TaskService(KanbanFacade kf)
        {
            this.kf = kf;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Log has started!");
        }
        /// <summary>
        /// This method adds a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>A serialized response object. The response should contain a error message in case of an error.</returns>
        public String AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            try
            {
                kf.AddTask(email, boardName, title, description, dueDate);
                log.Info("Task was added successfuly.");
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                log.Error("AddTask failed.");
                return JsonSerializer.Serialize(new Response(e.Message, null));
            }
        }


        /// <summary>
        /// This method updates the due date of a task.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>A serialized response object. The response should contain a error message in case of an error.</returns>
        public string UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            try
            {
                kf.UpdateTaskDueDate(email, boardName, columnOrdinal, taskId, dueDate);
                log.Info("Task's due date updated successfuly.");
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                log.Error("UpdateTaskDueDate failed.");
                return JsonSerializer.Serialize(new Response(e.Message, null));
            }
        }
        /// <summary>
        /// Lists all the inProgress Tasks by userEmail.
        /// </summary>
        /// <param name="userEmail">Email of the user.</param>
        /// <returns>A serialized response with list of tasks. The response should contain a error message in case of an error</returns>
        public string InProgressTasks(string userEmail)
        {
            try
            {
                log.Info("In progress tasks extraction thread started.");
                return JsonSerializer.Serialize(new Response(null, kf.InProgressTasks(userEmail)));
            }
            catch (Exception e)
            {
                log.Error("In progress tasks extraction thread failed.");
                return JsonSerializer.Serialize(new Response(e.Message, null));
            }
        }
        /// <summary>
        /// This method updates task title.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>A serialized response object. The response should contain a error message in case of an error.</returns>
        public string UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            try
            {
                kf.UpdateTaskTitle(email, boardName, columnOrdinal, taskId, title);
                log.Info("Task's title updated successfuly.");
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                log.Error("UpdateTaskTitle failed.");
                return JsonSerializer.Serialize(new Response(e.Message, null));
            }
        }
        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>A serialized response object. The response should contain a error message in case of an error.</returns>
        public string UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {
            try
            {
                kf.UpdateTaskDescription(email, boardName, columnOrdinal, taskId, description);
                log.Info("Task's description updated successfuly.");
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                log.Error("UpdateTaskDescription failed.");
                return JsonSerializer.Serialize(new Response(e.Message, null));
            }
        }
        /// <summary>
        /// Delete a specific Task.
        /// </summary>
        /// <param name="id">The Task's id. must be an existed id.</param>
        /// <returns>A serialized response object. The response should contain a error message in case of an error.</returns>
        public string DeleteTask(int id, string email, string boardName)
        {
            try
            {
                kf.DeleteTask(id, email, boardName);
                log.Info("Task deleted successfuly.");
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                log.Error("DeleteTask failed.");
                return JsonSerializer.Serialize(new Response(e.Message, null));
            }
        }
        /// <summary>
        /// This method assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column number. The first column is 0, the number increases by 1 for each column</param>
        /// <param name="taskID">The task to be updated identified a task ID</param>        
        /// <param name="emailAssignee">Email of the asignee user</param>
        /// <returns>An empty response, unless an error occurs </returns>
        public string AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            try
            {
               
                kf.AssignTask(email, boardName, columnOrdinal, taskID, emailAssignee);
                log.Info("Task assignment proccess has completed successfully.");
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                log.Error("Task assignment has failed.");
                return JsonSerializer.Serialize(new Response(e.Message, null));
            }
        }
    }
}
