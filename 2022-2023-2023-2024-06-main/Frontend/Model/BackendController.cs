using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.BusinessLayer;

namespace Frontend.Model
{
    public class BackendController
    {
        private Services services;

        public BackendController()
        {
            services = BackendControllerFactory.GetFactory();
        }

        /// <summary>
        /// The method creates a model of a user after login
        /// </summary>
        /// <param name="username">The email of the user</param>
        /// <param name="password">The password of the user</param>
        /// <returns>A UserModel object with the user's credentials</returns>
        /// <exception cref="Exception"></exception>
        public UserModel Login(string username, string password)
        {
            Response user = JsonSerializer.Deserialize<Response>(services.USER_SERVICE.Login(username, password))!;
            if (user.ErrorOccured)
            {
                throw new Exception(user.ErrorMessage);
            }
            return new UserModel(this, username);
        }
        /// <summary>
        /// The method creates a model of a user after registration
        /// </summary>
        /// <param name="username">The email of the user</param>
        /// <param name="password">The password of the user</param>
        /// <returns>A UserModel object with the user's credentials</returns>
        /// <exception cref="Exception"></exception>
        internal UserModel Register(string username, string password)
        {
            Response reg = JsonSerializer.Deserialize<Response>(services.USER_SERVICE.Register(username, password))!;
            if (reg.ErrorOccured)
            {
                throw new Exception(reg.ErrorMessage);
            }

            return new UserModel(this, username);
        }
        /// <summary>
        /// The method creates a list of all the board models of the user.
        /// </summary>
        /// <param name="user">User model of the user</param>
        /// <returns>A list of BoardModel of the user</returns>
        /// <exception cref="Exception"></exception>
        internal List<BoardModel> GetBoardsOfUser(UserModel user)
        {
            Response response = JsonSerializer.Deserialize<Response>(services.BOARD_SERVICE.GetUserBoards(user.Email))!;
            if (response.ErrorOccured)
            {
                throw new Exception(response.ErrorMessage);
            }
            List<long> boardsIDS = JsonSerializer.Deserialize<List<long>>((JsonElement)response.ReturnValue)!;
            List<BoardModel> boardModelsOfUser = new List<BoardModel>();
            foreach (int t in boardsIDS)
            {
                boardModelsOfUser.Add(new BoardModel(this, t));
            }

            return boardModelsOfUser;
        }
        /// <summary>
        /// The method sends the board name according to its ID
        /// </summary>
        /// <param name="boardId">The board ID</param>
        /// <returns>A string of the board's name.</returns>
        /// <exception cref="Exception"></exception>
        internal string GetBoardName(int boardId)
        {
            Response reg = JsonSerializer.Deserialize<Response>(services.BOARD_SERVICE.GetBoardName(boardId))!;

            if (reg.ErrorOccured)
            {
                throw new Exception(reg.ErrorMessage);
            }
            string obj = JsonSerializer.Deserialize<string>((JsonElement)reg.ReturnValue)!;
            return obj.ToString();
        }
        /// <summary>
        /// The method sends the column of the board ID.
        /// </summary>
        /// <param name="boardId">The ID of the board</param>
        /// <param name="columnOrdinal">The column ordinal.</param>
        /// <returns>A list of the task models in the column.</returns>
        /// <exception cref="Exception"></exception>
        public List<TaskModel> GetColumn(int boardId, int columnOrdinal)
        {
            Response reg = JsonSerializer.Deserialize<Response>(services.BOARD_SERVICE.GetColumn(boardId, columnOrdinal))!;
            if (reg.ErrorOccured)
            {
                throw new Exception(reg.ErrorMessage);
            }
            ColumnToSend column = JsonSerializer.Deserialize<ColumnToSend>((JsonElement)reg.ReturnValue)!;
            //List<Task> column = new List<Task>();
            List<TaskModel> toReturn = new List<TaskModel>();
            foreach (Task t in column.GetTasks())
            {
                toReturn.Add(new TaskModel(t.Id, t.Title, t.Description, t.Assignee, t.DueDate, t.CreationTime, this));
            }
            return toReturn;
        }
        /// <summary>
        /// The method sends the limitation of tasks in the requested column.
        /// </summary>
        /// <param name="boardId">The Id of the board.</param>
        /// <param name="columnOrdinal">The column ordinal of the board.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int GetColumnLimit(int boardId, int columnOrdinal)
        {
            Response reg = JsonSerializer.Deserialize<Response>(services.BOARD_SERVICE.GetColumnLimit(boardId, columnOrdinal))!;
            if (reg.ErrorOccured)
            {
                throw new Exception(reg.ErrorMessage);
            }
            int obj = JsonSerializer.Deserialize<int>((JsonElement)reg.ReturnValue)!;
            return obj;
        }

    }
}