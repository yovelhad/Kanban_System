// See https://aka.ms/new-console-template for more information
using System;
using System.Reflection;
using BackendTests;
using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.BackendTests
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Test Enviorment!");
            UserFacade userFacade = new UserFacade();
            KanbanFacade kanbanFacade = new KanbanFacade(userFacade);
            UserService userService = new UserService(userFacade);
            BoardService boardService = new BoardService(kanbanFacade);
            TaskService taskService = new TaskService(kanbanFacade);
            TestUserService testUser = new TestUserService(userService);
            TaskServiceTest testTask = new TaskServiceTest(taskService);
            BoardServiceTests testBoard = new BoardServiceTests(boardService);
            GradingService grading = new GradingService();

            grading.DeleteData();
            userService.Register("mail@mail.com", "Password1");
            boardService.AddBoard("mail@mail.com", "board1");
            boardService.AddBoard("mail@mail.com", "board2");
            taskService.AddTask("mail@mail.com", "board1", "task1", "first", new DateTime(2024, 03, 04));
            taskService.AddTask("mail@mail.com", "board1", "task2", "second", new DateTime(2024, 03, 04));
            taskService.AddTask("mail@mail.com", "board1", "task3", "third", new DateTime(2024, 03, 04));
            taskService.AssignTask("mail@mail.com", "board1", 0, 1, "mail@mail.com");
            taskService.AssignTask("mail@mail.com", "board1", 0, 2, "mail@mail.com");
            taskService.AssignTask("mail@mail.com", "board1", 0, 3, "mail@mail.com");
            boardService.AdvanceTask("mail@mail.com", "board1", 0, 3);
            boardService.AdvanceTask("mail@mail.com", "board1", 1, 3);
            boardService.AdvanceTask("mail@mail.com", "board1", 0, 2);



            //boardService.AdvanceTask("shooki@gmail.com", "missions", 0, 1);
            //boardService.GetColumn("shooki@gmail.com", "missions", 1);

            //new BoardServiceTests(boardService).LimitColoumnTasksTest();
            //new TaskServiceTest(taskService).runAddTaskTest();
            //new BoardServiceTests(boardService).GetColoumnLimitTest();

            //userService.Login("shooki@gmail.com", "1234567");

            //new TestUserService(userService).RunRegistrationTests();

            //new BoardServiceTests(boardService).AddBoardTest();
            //userService.LoadUsers();
            //boardService.LoadBoards();
            //boardService.AddBoard("gilb@gmail.com", "board12");

            //boardService.DeleteBoard("gilb@gmail.com", "board12");
            //taskService.AddTask("gilb@gmail.com", "board12", "final", "test", new DateTime(2023, 08, 04));
            //boardService.JoinBoard("gilb@gmail.com", 1);
            //taskService.AssignTask("shooki@gmail.com", "board1", 0, 1, "gilb@gmail.com");
            //boardService.AdvanceTask("shooki@gmail.com", "board1", 0, 2);
            //boardService.TransferOwnership("shooki@gmail.com", "gilb@gmail.com", "board1");
            //boardService.TransferOwnership("gilb@gmail.com", "shooki@gmail.com", "board1");
            //boardService.TransferOwnership("hi", "shooki@gmail.com", "board1");
            //boardService.TransferOwnership(null, "shooki@gmail.com", "board1");
            //new BoardServiceTests(boardService).GetBoardNameTest();
            //new BoardServiceTests(boardService).GetUserBoardsTest();

            //new BoardServiceTests(boardService).DeleteBoardsTest();
            //new BoardServiceTests(boardService).LoadBoardsTest();
            // new BoardServiceTests(boardService).TransferOwnershipTest();
            //new TestUserService(userService).LoadUsersTest();
            //new TestUserService(userService).DeleteUsersTest();
            //new BoardServiceTests(boardService).LeaveBoardTest();

            //  new BoardServiceTests(boardService).GetUserBoardsTest();

            //boardService.LeaveBoard("gilb@gmail.com", 1);


            //new BoardServiceTests(boardService).GetBoardNameTest();
            //new BoardServiceTests(boardService).DeleteBoardsTest();
            // new BoardServiceTests(boardService).LoadBoardsTest();
            //new BoardServiceTests(boardService).TransferOwnershipTest();
            // new TestUserService(userService).LoadUsersTest();
            // new TestUserService(userService).DeleteUsersTest();
            // new BoardServiceTests(boardService).LeaveBoardTest();
            // new BoardServiceTests(boardService).JoinBoardTest();
            // new BoardServiceTests(boardService).GetUserBoardsTest();
            // new TaskServiceTest(taskService).AssignTaskTest();
            //new TestUserService(userService).RunRegistrationTests();
            // new BoardServiceTests(boardService).AddBoardTest();
            // new TaskServiceTest(taskService).runAddTaskTest();
            //new BoardServiceTests(boardService).LimitColoumnTasksTest();
            //new BoardServiceTests(boardService).GetColoumnLimitTest();
            //new BoardServiceTests(boardService).GetColoumnTest();
            //string s0 = boardService.GetColumn("Almogim@gmail.com", "board1", 0);
            //Console.WriteLine(s0);
            // new TaskServiceTest(taskService).runUpdateTaskTitleTest();
            //new TaskServiceTest(taskService).runUpdateTaskDescriptionTest();
            //new TaskServiceTest(taskService).runUpdateTaskDueDateTest();
            //new BoardServiceTests(boardService).LimitColoumnTasksTest();
            //new BoardServiceTests(boardService).GetColoumnLimitTest();
            //new BoardServiceTests(boardService).GetColoumnTest();
            //string s = boardService.GetColumn("Almogim@gmail.com", "board1", 0);
            //Console.WriteLine(s);
            //boardService.GetColumn("shooki@gmail.com", "missions",0);
            //string s = boardService.GetColumn("Almogim@gmail.com", "board1", 0);
            //Console.WriteLine(s);
            //new BoardServiceTests(boardService).GetColoumnTest();
            //new BoardServiceTests(boardService).AdvanceTaskTest();
            //new BoardServiceTests(boardService).GetColoumnTest();
            //new BoardServiceTests(boardService).GetColoumnTest();
            //string s1 = boardService.GetColumn("Almogim2@gmail.com", "board1", 0);
            //Console.WriteLine(s1);
            //string s2 = taskService.InProgressTasks("Almogim@gmail.com");
            //Console.WriteLine(s2);
            //new TaskServiceTest(taskService).InProgressTasksTest();
            //new BoardServiceTests(boardService).DeleteBoardTest();
            // new BoardServiceTests(boardService).GetColoumnTest();

            /**
            string s3 = boardService.GetColumn("Almogim@gmail.com", "board1", 0);
            Console.WriteLine(s3);
            **/

            /**
            new BoardServiceTests(boardService).AdvanceTaskTest();
            string s1 = boardService.GetColumn("Almogim@gmail.com", "board1", 1);
            Console.WriteLine(s1);
            string s2 = boardService.InProgressTasks("Almogim@gmail.com");
            Console.WriteLine(s2);
            **/

            //new BoardServiceTests(boardService).InProgressTasksTest();
            //string s3 = boardService.GetColumn("Almogim@gmail.com", "board1", 0);
            //Console.WriteLine(s3);
            //boardService.GetColumn("shooki@gmail.com", "missions",0);
            //new TestUserService(userService).RunRegistrationTests();
            //new TestUserService(userService).RunLoginTests();  
            //new BoardServiceTests(boardService).AddBoardTest();
            //new TestUserService(userService).LogoutTests();
            //new BoardServiceTests(boardService).DeleteBoardTest();
            //new BoardServiceTests(boardService).GetAllBoardsTest();
            //new BoardServiceTests(boardService).DeleteBoardTest();
            //new BoardServiceTests(boardService).LimitColoumnTasksTest();
            //new BoardServiceTests(boardService).AdvanceTaskTest();
            //new BoardServiceTests(boardService).InProgressTasksTest();
            //new BoardServiceTests(boardService).GetColoumnLimitTest();
            //new BoardServiceTests(boardService).GetColoumnNameTest();
            //new BoardServiceTests(boardService).GetColoumnTest();
            //new TaskServiceTest(taskService).runAddTaskTest();
            //new TaskServiceTest(taskService).runUpdateTaskDueDateTest();
            //new TaskServiceTest(taskService).runUpdateTaskTitleTest();
            //new TaskServiceTest(taskService).runUpdateTaskDescriptionTest();
            //new TaskServiceTest(taskService).runDeleteTaskTest();

        }
    }
}