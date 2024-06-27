using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Text.Json;

namespace IntroSE.Kanban.BackendTests
{
    class BoardServiceTests
    {
        private readonly BoardService boardService;

        public BoardServiceTests(BoardService boardService)
        {
            this.boardService = boardService;
        }
        /// <summary>
        /// This method tests the functionality of creation board with invalid email, and same boards names for the same user
        /// </summary>
        public void AddBoardTest()
        {
            Response res0 = JsonSerializer.Deserialize<Response>(boardService.AddBoard("Almogim@gmail.com", "board1"))!;
            if (res0.ErrorOccured)
            {
                Console.WriteLine(res0.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Created board successfully");
            }
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.AddBoard("Almogim@gmail.com", "board2"))!;
            if (res1.ErrorOccured)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Created board successfully");
            }
            /**
            Response res2 = JsonSerializer.Deserialize<Response>(boardService.AddBoard("Almogim2@gmail.com", "board1"))!;
            if (res2.ErrorOccured)
            {
                Console.WriteLine(res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Created board successfully");
            }
            **/
            /**
            Response res2 = JsonSerializer.Deserialize<Response>(boardService.AddBoard("almogim123", "board1"))!;
            if (res2.ErrorOccured)
            {
                Console.WriteLine(res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Created board successfully");
            }
            //// It supposed to fail because the user email doesn't exist
            Response resAddUserNotExists = JsonSerializer.Deserialize<Response>(boardService.AddBoard("omerA12@gmail.com", "board1"))!;
            if (resAddUserNotExists.ErrorOccured)
            {
                Console.WriteLine(resAddUserNotExists.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Created board successfully");
            }
            **/
        }

        /**
        /// <summary>
        /// This method tests the functionality of receiving all boards per user and the case of invalid email
        /// </summary>
        public void GetAllBoardsTest()
        {
            Response res0 = JsonSerializer.Deserialize<Response>(boardService.GetAllBoards("Almogj04@gmail.com"))!;
            if (res0.ErrorOccured)
            {
                Console.WriteLine(res0.ErrorMessage);
            }
            else
            {
                Console.WriteLine("All boards received successfully");
            }
            //// It supposed to fail because the user email doesn't exist
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.GetAllBoards("aba4324.com"))!;
            if (res1.ErrorOccured)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("All boards received successfully");
            }
        }
        **/

        /// <summary>
        /// This method tests the functionality of deletion of boards and cases of invalid email or non existing board name
        /// </summary>
        public void DeleteBoardTest()
        {
            Response res0 = JsonSerializer.Deserialize<Response>(boardService.DeleteBoard("Almogim@gmail.com", "board1"))!;
            if (res0.ErrorOccured)
            {
                Console.WriteLine(res0.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Board deleted successfully");
            }
            /**
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.DeleteBoard("Almogj04@gmail.com", "board15555"))!;
            if (res1.ErrorOccured)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Board deleted successfully");
            }
            //// It supposed to fail because the user email doesn't exist
            Response res2 = JsonSerializer.Deserialize<Response>(boardService.DeleteBoard("almogim12345", "board1"))!;
            if (res2.ErrorOccured)
            {
                Console.WriteLine(res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Board deleted successfully");
            }
            **/
        }
        /// <summary>
        /// This method tests the functionality of limiting number of tasks in a specific coloumn and case of invalid limitation (negative) or invalid email
        /// </summary>
        public void LimitColoumnTasksTest()
        {
            Response res0 = JsonSerializer.Deserialize<Response>(boardService.LimitColumnTasks("Almogim@gmail.com", "board1", 0, 2))!;
            if (res0.ErrorOccured)
            {
                Console.WriteLine(res0.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Coloumn was limited successfully");
            }
            /**
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.LimitColumnTasks("Almogj04@gmail.com", "board1", 9, 5))!;
            if (res1.ErrorOccured)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Coloumn was limited successfully");
            }

            Response res2 = JsonSerializer.Deserialize<Response>(boardService.LimitColumnTasks("Almogj04@gmail.com", "board1", 2, -3))!;
            if (res2.ErrorOccured)
            {
                Console.WriteLine(res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Coloumn was limited successfully");
            }
            //// It supposed to fail because the user email doesn't exist
            Response res3 = JsonSerializer.Deserialize<Response>(boardService.LimitColumnTasks("ege444", "board1", 2, 3))!;
            if (res3.ErrorOccured)
            {
                Console.WriteLine(res3.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Coloumn was limited successfully");
            }
            **/
        }
        /// <summary>
        /// This method tests the functionality of advancing task from one coloumn to another and cases of invalid ordinal number
        /// </summary>
        public void AdvanceTaskTest()
        {
            Response res0 = JsonSerializer.Deserialize<Response>(boardService.AdvanceTask("Almogim@gmail.com", "board1", 0, 1))!;
            if (res0.ErrorOccured)
            {
                Console.WriteLine(res0.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Task was advanced successfully");
            }

            Response res1 = JsonSerializer.Deserialize<Response>(boardService.AdvanceTask("Almogim@gmail.com", "board2", 0, 1))!;
            if (res1.ErrorOccured)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Task was advanced successfully");
            }
            /**
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.AdvanceTask("Almogj04@gmail.com", "board1", 0, 51))!;
            if (res1.ErrorOccured)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Task was advanced successfully");
            }

            Response res2 = JsonSerializer.Deserialize<Response>(boardService.AdvanceTask("Almogj04@gmail.com", "board1", 1, 57))!;
            if (res2.ErrorOccured)
            {
                Console.WriteLine(res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Task was advanced successfully");
            }
            //// It supposed to fail because the user email doesn't exist
            Response resAdvanceUserNotExists = JsonSerializer.Deserialize<Response>(boardService.AdvanceTask("Ari12@gmail.com", "board1", 1, 57))!;
            if (resAdvanceUserNotExists.ErrorOccured)
            {
                Console.WriteLine(resAdvanceUserNotExists.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Task was advanced successfully");
            }
            **/
        }
        /// <summary>
        /// This method tests the functionality of retrieving the limit of a specific coloumn and case of invalid email or ordinal number
        /// </summary>
        public void GetColoumnLimitTest()
        {
            Response res0 = JsonSerializer.Deserialize<Response>(boardService.GetColumnLimit("Almogim@gmail.com", "board1", 0))!;
            if (res0.ErrorOccured)
            {
                Console.WriteLine(res0.ErrorMessage);
            }
            else
            {
                Console.WriteLine(res0.ReturnValue.ToString());
                Console.WriteLine("Coloumn limit has received successfully");
            }
            /**
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.GetColumnLimit("Almogj04@gmail.com", "board1", -9))!;
            if (res1.ErrorOccured)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Coloumn limit has received successfully");
            }
            //// It supposed to fail because the user email doesn't exist
            Response res2 = JsonSerializer.Deserialize<Response>(boardService.GetColumnLimit("bademail7", "board1", 8))!;
            if (res2.ErrorOccured)
            {
                Console.WriteLine(res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Coloumn limit has received successfully");
            }
            **/
        }
        /// <summary>
        /// This method tests the functionality of retrieving the name of a specific coloumn and case of invalid email or ordinal number
        /// </summary>
        public void GetColoumnNameTest()
        {
            Response res0 = JsonSerializer.Deserialize<Response>(boardService.GetColumnName("Almogj04@gmail.com", "board1", 2))!;
            if (res0.ErrorOccured)
            {
                Console.WriteLine(res0.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Coloumn name has received successfully");
            }

            Response res1 = JsonSerializer.Deserialize<Response>(boardService.GetColumnName("Almogj04@gmail.com", "board1", -9))!;
            if (res1.ErrorOccured)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Coloumn name has received successfully");
            }
            //// It supposed to fail because the user email doesn't exist
            Response res2 = JsonSerializer.Deserialize<Response>(boardService.GetColumnName("bademail7", "board1", 8))!;
            if (res2.ErrorOccured)
            {
                Console.WriteLine(res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Coloumn name has received successfully");
            }
        }
        /// <summary>
        /// This method tests the functionality of retrieving a specific coloumn and case of invalid email or ordinal number
        /// </summary>
        public void GetColoumnTest()
        {
            Response res0 = JsonSerializer.Deserialize<Response>(boardService.GetColumn("Almogim@gmail.com", "board1", 0))!;
            if (res0.ErrorOccured)
            {
                Console.WriteLine(res0.ErrorMessage);
            }
            else
            {
                List<Backend.BusinessLayer.Task> tasks = JsonSerializer.Deserialize<List<Backend.BusinessLayer.Task>>((JsonElement)res0.ReturnValue)!;

                foreach (Backend.BusinessLayer.Task task in tasks)
                {
                    Console.WriteLine("Id: " + task.Id + ", Title: " + task.Title + ", Creation Time: " +
                    task.CreationTime + ", Description: " + task.Description + ", Due Date " + task.DueDate);
                }
                Console.WriteLine("Coloumn has received successfully");
            }
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.GetColumn("Almogim@gmail.com", "board1", 1))!;
            if (res1.ErrorOccured)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                List<Backend.BusinessLayer.Task> tasks = JsonSerializer.Deserialize<List<Backend.BusinessLayer.Task>>((JsonElement)res1.ReturnValue)!;
                
                foreach (Backend.BusinessLayer.Task task in tasks) 
                { 
                    Console.WriteLine("Id: " + task.Id + ", Title: " + task.Title + ", Creation Time: " + 
                    task.CreationTime + ", Description: " + task.Description + ", Due Date " + task.DueDate); 
                }
                Console.WriteLine("Coloumn has received successfully");
            }
            Response res2 = JsonSerializer.Deserialize<Response>(boardService.GetColumn("Almogim@gmail.com", "board2", 0))!;
            if (res2.ErrorOccured)
            {
                Console.WriteLine(res2.ErrorMessage);
            }
            else
            {
                List<Backend.BusinessLayer.Task> tasks = JsonSerializer.Deserialize<List<Backend.BusinessLayer.Task>>((JsonElement)res2.ReturnValue)!;

                foreach (Backend.BusinessLayer.Task task in tasks)
                {
                    Console.WriteLine("Id: " + task.Id + ", Title: " + task.Title + ", Creation Time: " +
                    task.CreationTime + ", Description: " + task.Description + ", Due Date " + task.DueDate);
                }
                Console.WriteLine("Coloumn has received successfully");
            }
            /**
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.GetColumn("Almogj04@gmail.com", "board1", -9))!;
            if (res1.ErrorOccured)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Coloumn has received successfully");
            }
            //// It supposed to fail because the user email doesn't exist
            Response res2 = JsonSerializer.Deserialize<Response>(boardService.GetColumn("bademail7", "board1", 8))!;
            if (res2.ErrorOccured)
            {
                Console.WriteLine(res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Coloumn has received successfully");
            }
            **/
        }

        /// <summary>
        /// This method tests the functionality of retrieving the name of a specific board and case of invalid board id
        /// </summary>
        public void GetBoardNameTest()
        {
            Response res0 = JsonSerializer.Deserialize<Response>(boardService.GetBoardName(1))!;
            if (res0.ErrorOccured)
            {
                Console.WriteLine(res0.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Board name has received successfully");
            }

            Response res1 = JsonSerializer.Deserialize<Response>(boardService.GetBoardName(2))!;
            if (res1.ErrorOccured)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Board name has received successfully");
            }
            //// It supposed to fail because the board id does not exist
            Response res2 = JsonSerializer.Deserialize<Response>(boardService.GetBoardName(150))!;
            if (res2.ErrorOccured)
            {
                Console.WriteLine(res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Board name has received successfully");
            }
        }

        /// <summary>
        /// This method tests the functionality of transfering board and cases of invalid users or not logged in
        /// </summary>
        public void TransferOwnershipTest()
        {
            Response res0 = JsonSerializer.Deserialize<Response>(boardService.TransferOwnership("Almogim@gmail.com", "Shooki@gmail.com", "board1"))!;
            if (res0.ErrorOccured)
            {
                Console.WriteLine(res0.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Board transfered successfully");
            }
            //// It supposed to fail because the owner email does not exist
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.TransferOwnership("fail@gmail.com", "Shooki@gmail.com", "board1"))!;
            if (res1.ErrorOccured)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Board transfered successfully");
            }
            //// It supposed to fail because the new owner email does not exist
            Response res2 = JsonSerializer.Deserialize<Response>(boardService.TransferOwnership("Almogim@gmail.com", "fail@gmail.com", "board1"))!;
            if (res2.ErrorOccured)
            {
                Console.WriteLine(res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Board transfered successfully");
            }
            //// It supposed to fail because the owner does not have such a board
            Response res3 = JsonSerializer.Deserialize<Response>(boardService.TransferOwnership("Almogim@gmail.com", "Shooki@gmail.com", "boardFail"))!;
            if (res2.ErrorOccured)
            {
                Console.WriteLine(res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Board transfered successfully");
            }
        }

        /// <summary>
        /// This method tests the functionality of loading all boards
        /// </summary>
        public void LoadBoardsTest()
        {
            Response res0 = JsonSerializer.Deserialize<Response>(boardService.LoadBoards())!;
            if (res0.ErrorOccured)
            {
                Console.WriteLine(res0.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Boards have received successfully");
            }
        }
        /// <summary>
        /// This method tests the functionality of deleting all boards
        /// </summary>
        public void DeleteBoardsTest()
        {
            Response res0 = JsonSerializer.Deserialize<Response>(boardService.DeleteBoards())!;
            if (res0.ErrorOccured)
            {
                Console.WriteLine(res0.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Boards have deleted successfully");
            }
        }
        /// <summary>
        /// This method tests the functionality of retrieving the user's boards, cases of incorrect email or non-existing boards.
        /// </summary>
        public void GetUserBoardsTest()
        {
            Response res0 = JsonSerializer.Deserialize<Response>(boardService.GetUserBoards("Almogim@gmail.com"))!;
            if (res0.ErrorOccured)
            {
                Console.WriteLine(res0.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Boards of the user we're received successfully");
            }

            Response res1 = JsonSerializer.Deserialize<Response>(boardService.GetUserBoards("dummyEmail@mi.com"))!;
            if (res1.ErrorOccured)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine($"Boards of the user were received successfully");
            }

            Response res2 = JsonSerializer.Deserialize<Response>(boardService.GetUserBoards(null))!;
            if (res2.ErrorOccured)
            {
                Console.WriteLine(res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine($"Boards of the user were received successfully");
            }

        }

        /// <summary>
        /// This method tests the functionality of user's joining a different board in the system.
        /// tests actions such as incorrect email, non-existing board/email etc.
        /// </summary>

        public void JoinBoardTest()
        {
            Console.WriteLine("Join Board Test");
            Response res0 = JsonSerializer.Deserialize<Response>(boardService.JoinBoard("gilb@gmail.com", 1))!;
            if (res0.ErrorOccured)
            {
                Console.WriteLine("Test has failed!");
            }
            else
            {
                Console.WriteLine("Test has passed.");
            }

            Response res1 = JsonSerializer.Deserialize<Response>(boardService.JoinBoard(null, 1))!;
            if (res1.ErrorOccured)
            {
                Console.WriteLine("Test has passed.");
            }
            else
            {
                Console.WriteLine("Test has failed!");
            }

            Response res2 = JsonSerializer.Deserialize<Response>(boardService.JoinBoard("Almogim@gmail.com", 1))!;
            if (res2.ErrorOccured)
            {
                Console.WriteLine("Test has passed.");
            }
            else
            {
                Console.WriteLine("Test has failed!");
            }

            Response res3 = JsonSerializer.Deserialize<Response>(boardService.JoinBoard("shooki@gmail.com", 124))!;
            if (res3.ErrorOccured)
            {
                Console.WriteLine("Test has passed.");
            }
            else
            {
                Console.WriteLine("Test has failed!");
            }

            Response res4 = JsonSerializer.Deserialize<Response>(boardService.JoinBoard("shooki@gmail.com", 1))!;
            if (res4.ErrorOccured)
            {
                Console.WriteLine("Test has passed.");
            }
            else
            {
                Console.WriteLine("Test has failed!");
            }

            Response res5 = JsonSerializer.Deserialize<Response>(boardService.JoinBoard("gilb@gmail.com", 1))!;
            if (res5.ErrorOccured)
            {
                Console.WriteLine("Test has passed.");
            }
            else
            {
                Console.WriteLine("Test has failed!");
            }

        }

        /// <summary>
        /// This function tests the functionality of user's leaving the board.
        /// tests cases such as invalid board or non existing board/email.
        /// </summary>
        public void LeaveBoardTest()
        {
            Console.WriteLine("Leave Board Test");
            Console.WriteLine();
            Response res3 = JsonSerializer.Deserialize<Response>(boardService.LeaveBoard("gilb@gmail.com", 123))!;
            if (res3.ErrorOccured)
            {
                Console.WriteLine("Test has passed.");
            }
            else
            {
                Console.WriteLine("Test has failed!");
            }
            
            Response res0 = JsonSerializer.Deserialize<Response>(boardService.LeaveBoard("gilb@gmail.com", 1))!;
            if (res0.ErrorOccured)
            {
                Console.WriteLine("Test has failed!");
            }
            else
            {
                Console.WriteLine("Test has passed.");
            }


            Response res1 = JsonSerializer.Deserialize<Response>(boardService.LeaveBoard("Almogim@gmail.com", 1))!;
            if (res1.ErrorOccured)
            {
                Console.WriteLine("Test has passed.");
            }
            else
            {
                Console.WriteLine("Test has failed!");
            }

            Response res2 = JsonSerializer.Deserialize<Response>(boardService.LeaveBoard("shooki@gmail.com", 1))!;
            if (res2.ErrorOccured)
            {
                Console.WriteLine("Test has passed.");
            }
            else
            {
                Console.WriteLine("Test has failed!");
            }

            Response res4 = JsonSerializer.Deserialize<Response>(boardService.LeaveBoard(null, 1))!;
            if (res4.ErrorOccured)
            {
                Console.WriteLine("Test has passed.");
            }
            else
            {
                Console.WriteLine("Test has failed!");
            }

        }
    }
}