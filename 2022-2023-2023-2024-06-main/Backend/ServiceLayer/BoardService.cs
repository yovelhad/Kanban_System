using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using log4net;
using IntroSE.Kanban.Backend.BusinessLayer;
using System.Reflection;
using log4net.Util;
using System.Threading;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class BoardService
    {
        private readonly KanbanFacade kf;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public BoardService(KanbanFacade kf)
        {
            this.kf = kf;
            log.Info("Log has started!");
        }
        /// <summary>
        /// Adds a board to the specific user.
        /// </summary>
        /// <param name="creatorEmail">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the new board</param>
        /// <returns>A serialized response object. The response should contain a error message in case of an error</returns>
        public string AddBoard(string creatorEmail, string boardName)
        {
            try
            {
                log.Info("Board addition thread started.");
                kf.AddBoard(creatorEmail, boardName);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                log.Error("Board addition thread failed.");
                return JsonSerializer.Serialize(new Response(e.Message, null));
            }
        }
        /// <summary>
        /// Deletes a board from a specific user.
        /// </summary>
        /// <param name="creatorEmail">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board to Delete</param>
        /// <returns>A serialized response object. The response should contain a error message in case of an error</returns>
        public string DeleteBoard(string creatorEmail, string boardName)
        {
            try
            {
                log.Info("Board deletion thread started.");
                kf.DeleteBoard(creatorEmail, boardName);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                log.Error("Board deletion thread failed.");
                return JsonSerializer.Serialize(new Response(e.Message, null));
            }
        }
        /// <summary>
        /// This method advances a task to the next column
        /// </summary>
        /// <param name="creatorEmail">Email of board owner. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column Id. The first column is identified by 0, the Id increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task Id</param>
        /// <returns>A serialized empty response, unless an error occurs </returns>
        public string AdvanceTask(string creatorEmail, string boardName, int columnOrdinal, int taskId)
        {
            try
            {
                log.Info("Task advance thread started.");
                kf.AdvanceTask(creatorEmail, boardName, columnOrdinal, taskId);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                log.Error("Task advance thread failed.");
                return JsonSerializer.Serialize(new Response(e.Message, null));
            }
        }

        /// <summary>
        /// This method limits the number of tasks in a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column Id. The first column is identified by 0, the Id increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>A serialized response, unless an error occurs</returns>
        public string LimitColumnTasks(string creatorEmail, string boardName, int columnOrdinal, int limit)
        {
            try
            {
                log.Info("Column limitation thread started.");
                kf.LimitColumn(creatorEmail, boardName, columnOrdinal, limit);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                log.Error("Column limitation thread failed.");
                return JsonSerializer.Serialize(new Response(e.Message, null));
            }
        }
        /// <summary>
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column Id. The first column is identified by 0, the Id increases by 1 for each column</param>
        /// <returns>A serialized response with the column's limit, unless an error occurs</returns>
        public string GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            try
            {
                log.Info("Column limitation extraction thread started.");
                int coloumnLimit = kf.GetColumnLimit(email, boardName, columnOrdinal);
                return JsonSerializer.Serialize(new Response(null, coloumnLimit));
            }
            catch (Exception e)
            {
                log.Error("Column limitation extraction thread failed.");
                return JsonSerializer.Serialize(new Response(e.Message, null));
            }
        }
        /// <summary>
        /// This method gets the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column Id. The first column is identified by 0, the Id increases by 1 for each column</param>
        /// <returns>A serialized response with the column's name, unless an error occurs</returns>
        public string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            try
            {
                log.Info("Column name extraction thread started.");
                string coloumnName = kf.GetColumnName(email, boardName, columnOrdinal);
                return JsonSerializer.Serialize(new Response(null, coloumnName));
            }
            catch (Exception e)
            {
                log.Error("Column name extraction thread failed.");
                return JsonSerializer.Serialize(new Response(e.Message, null));
            }
        }
        /// <summary>
        /// This method returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column Id. The first column is identified by 0, the Id increases by 1 for each column</param>
        /// <returns>A serialized response with the column's tasks, unless an error occurs</returns>
        public string GetColumn(string email, string boardName, int columnOrdinal)
        {
            try
            {
                log.Info("Column extraction thread started.");
                ColumnToSend tasks = new ColumnToSend(kf.GetColumn(email, boardName, columnOrdinal));
                return JsonSerializer.Serialize(new Response(null, tasks.GetTasks()));
            }
            catch (Exception e)
            {
                log.Error("Column extraction thread failed.");
                return JsonSerializer.Serialize(new Response(e.Message, null));
            }
        }

        /// <summary>
        /// This method returns a board's name
        /// </summary>
        /// <param name="boardId">The board's ID</param>
        /// <returns>A response with the board's name, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetBoardName(int boardId)
        {
            try
            {
                log.Info("Board name extraction thread started.");
                string boardName = kf.GetBoardName(boardId);
                return JsonSerializer.Serialize(new Response(null, boardName));
            }
            catch (Exception e)
            {
                log.Error("Board name extraction thread failed.");
                return JsonSerializer.Serialize(new Response(e.Message, null));
            }
        }

        /// <summary>
        /// This method transfers a board ownership.
        /// </summary>
        /// <param name="currentOwnerEmail">Email of the current owner. Must be logged in</param>
        /// <param name="newOwnerEmail">Email of the new owner</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            try
            {
                log.Info("Ownership transfer thread started.");
                kf.TransferOwnership(currentOwnerEmail, newOwnerEmail, boardName);
                return JsonSerializer.Serialize(new Response());
            }
            catch(Exception e)
            {
                log.Error("Ownership transfer thread failed");
                return JsonSerializer.Serialize(new Response(e.Message, null));
            }
        }

        ///<summary>This method loads all persisted data.
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string LoadBoards()
        {
            try
            {
                log.Info("Boards extraction thread started.");
                kf.LoadAllBoards();
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                log.Error("Boards extraction thread failed.");
                return JsonSerializer.Serialize(new Response(e.Message, null));
            }
        }

        ///<summary>This method deletes all persisted data.
        /// </summary>
        ///<returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string DeleteBoards()
        {
            try
            {
                log.Info("Boards deletion thread started.");
                kf.DeleteAllBoards();
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                log.Error("Boards deletion thread failed.");
                return JsonSerializer.Serialize(new Response(e.Message, null));
            }
        }
        /// <summary>
        /// This method returns a list of IDs of all user's boards.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>A response with a list of IDs of all user's boards, unless an error occurs</returns>
        public string GetUserBoards(string email)
        {
            try
            {
                log.Info("Boards extraction thread started.");
                List<int> userBoards = kf.GetUserBoards(email);
                return JsonSerializer.Serialize(new Response(null, userBoards));
            }
            catch (Exception e)
            {
                log.Error("Boards extraction thread failed.");
                return JsonSerializer.Serialize(new Response(e.Message, null));
            }
        }

        /// <summary>
        /// This method adds a user as member to an existing board.
        /// </summary>
        /// <param name="email">The email of the user that joins the board. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs </returns>
        public string JoinBoard(string email, int boardID)
        {
            try
            {
                log.Info("Joining board proccess started.");
                kf.JoinBoard(email, boardID);
                return JsonSerializer.Serialize(new Response());
            }
            catch(Exception e)
            {
                log.Error("Joining board thread has failed.");
                return JsonSerializer.Serialize(new Response(e.Message, null));
            }
        }

        /// <summary>
        /// This method removes a user from the members list of a board.
        /// </summary>
        /// <param name="email">The email of the user. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs </returns>
        public string LeaveBoard(string email, int boardID)
        {
            try
            {
                log.Info("Leaving board proccess started.");
                kf.LeaveBoard(email, boardID);
                return JsonSerializer.Serialize(new Response());
            }
            catch(Exception e)
            {
                log.Error("Leaving board thread has failed.");
                return JsonSerializer.Serialize(new Response(e.Message, null));
            }

        }
        /// <summary>
        /// This method gets the columns limit via boardId
        /// </summary>
        /// <param name="boardId">boardId, must exist</param>
        /// <param name="columnOrdinal">columnOrdinal</param>
        /// <returns>BoardName if succeeded, if not messege error</returns>
        public string GetColumnLimit(int boardId, int columnOrdinal)
        {
            try
            {
                log.Info("Column limitation extraction thread started.");
                int coloumnLimit = kf.GetColumnLimit(boardId, columnOrdinal);
                return JsonSerializer.Serialize(new Response(null, coloumnLimit));
            }
            catch (Exception e)
            {
                log.Error("Column limitation extraction thread failed.");
                return JsonSerializer.Serialize(new Response(e.Message, null));
            }
        }

        /// <summary>
        /// This method returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column Id. The first column is identified by 0, the Id increases by 1 for each column</param>
        /// <returns>A serialized response with the column's tasks, unless an error occurs</returns>
        public string GetColumn(int boardId, int columnOrdinal)
        {
            try
            {
                log.Info("Column extraction thread started.");
                ColumnToSend tasks = new ColumnToSend(kf.GetColumn(boardId, columnOrdinal));
                return JsonSerializer.Serialize(new Response(null, tasks));
            }
            catch (Exception e)
            {
                log.Error("Column extraction thread failed.");
                return JsonSerializer.Serialize(new Response(e.Message, null));
            }
        }
    }
}
