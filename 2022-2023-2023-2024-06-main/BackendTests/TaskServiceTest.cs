using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Text.Json;

namespace BackendTests
{
    class TaskServiceTest
    {
        private readonly TaskService taskService;
        public TaskServiceTest(TaskService taskService)
        {
            this.taskService = taskService;
        }
        /// <summary>
        /// This method runs tests on creating task process
        /// </summary>
        public void runAddTaskTest()
        {

            /// regular example.
            string test0 = taskService.AddTask("Almogim@gmail.com", "board1", "hello", "missions for me.", new DateTime(2024, 4, 5));
            Response res0 = JsonSerializer.Deserialize<Response>(test0)!;
            if (res0.ErrorOccured)
            {
                Console.WriteLine(res0.ErrorMessage);
            }
            else
            {
                Console.WriteLine("The task was created successfully");
            }
            string test1 = taskService.AddTask("Almogim@gmail.com", "board2", "hello2", "missions 2", new DateTime(2024, 4, 5));
            Response res1 = JsonSerializer.Deserialize<Response>(test1)!;
            if (res1.ErrorOccured)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("The task was created successfully");
            }
            string test2 = taskService.AddTask("Almogim2@gmail.com", "board1", "hello", "missions 3", new DateTime(2024, 4, 5));
            Response res2 = JsonSerializer.Deserialize<Response>(test2)!;
            if (res2.ErrorOccured)
            {
                Console.WriteLine(res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("The task was created successfully");
            }
            /**
            ///empty Title.
            string test1 = taskService.AddTask("shooki@gmail.com", "First Project", "", "missions for me.", new DateTime(2024, 4, 5));
            Response res1 = JsonSerializer.Deserialize<Response>(test1)!;
            if (res1.ErrorOccured)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("The task was created successfully");
            }

            //// 51 char in Title
            string test2 = taskService.AddTask("shooki@gmail.com", "First Project", "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxy",
                                               "missions for me.", new DateTime(2024, 4, 5));
            Response res2 = JsonSerializer.Deserialize<Response>(test2)!;
            if (res0.ErrorOccured)
            {
                Console.WriteLine(res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("The task was created successfully");
            }

            ////exactly 50 char in Title.
            string test3 = taskService.AddTask("shooki@gmail.com", "First Project", "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwx",
                                               "missions for me.", new DateTime(2024, 4, 5));
            Response res3 = JsonSerializer.Deserialize<Response>(test3)!;
            if (res0.ErrorOccured)
            {
                Console.WriteLine(res3.ErrorMessage);
            }
            else
            {
                Console.WriteLine("The task was created successfully");
            }

            ////empty  Description.
            string test4 = taskService.AddTask("shooki@gmail.com", "First Project", "Mission one", "", new DateTime(2024, 4, 5));
            Response res4 = JsonSerializer.Deserialize<Response>(test4)!;
            if (res4.ErrorOccured)
            {
                Console.WriteLine(res4.ErrorMessage);
            }
            else
            {
                Console.WriteLine("The task was created successfully");
            }

            ////301 char in Description.
            string test5 = taskService.AddTask("shooki@gmail.com", "First Project", "Mission one",
                                               "abcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghij" +
                                               "abcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghij" +
                                               "abcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijz", new DateTime(2024, 4, 5));
            Response res5 = JsonSerializer.Deserialize<Response>(test5)!;
            if (res5.ErrorOccured)
            {
                Console.WriteLine(res5.ErrorMessage);
            }
            else
            {
                Console.WriteLine("The task was created successfully");
            }

            ////exactly 300 char in Description.
            string test6 = taskService.AddTask("shooki@gmail.com", "First Project", "Mission one",
                                               "abcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghij" +
                                               "abcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghij" +
                                               "abcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghij", new DateTime(2024, 4, 5));
            Response res6 = JsonSerializer.Deserialize<Response>(test6)!;
            if (res6.ErrorOccured)
            {
                Console.WriteLine(res6.ErrorMessage);
            }
            else
            {
                Console.WriteLine("The task was created successfully");
            }

            ////Non legit date..
            string test7 = taskService.AddTask("shooki@gmail.com", "First Project", "Mission one", "missions for me.", new DateTime(1990, 4, 5));
            Response res7 = JsonSerializer.Deserialize<Response>(test7)!;
            if (res7.ErrorOccured)
            {
                Console.WriteLine(res7.ErrorMessage);
            }
            else
            {
                Console.WriteLine("The task was created successfully");
            }

            ////Create task with not existing user, it supposed to fail, we'll see it in ErrorMessage
            string testAddNotExistingUser = taskService.AddTask("Ohad5@gmail.com", "First Project", "Mission one", "missions for me.", new DateTime(1990, 4, 5));
            Response resAddNotExistingUser = JsonSerializer.Deserialize<Response>(testAddNotExistingUser)!;
            if (resAddNotExistingUser.ErrorOccured)
            {
                Console.WriteLine(resAddNotExistingUser.ErrorMessage);
            }
            else
            {
                Console.WriteLine("The task was created successfully");
            }
            **/
        }
        /// <summary>
        /// This method runs tests on updating task due date process
        /// </summary>
        public void runUpdateTaskDueDateTest()
        {
            /// regular example.
            string test8 = taskService.UpdateTaskDueDate("Almogim@gmail.com", "board1", 0, 1, new DateTime(2028, 4, 5));
            Response res10 = JsonSerializer.Deserialize<Response>(test8)!;
            if (res10.ErrorOccured)
            {
                Console.WriteLine(res10.ErrorMessage);
            }
            else
            {
                Console.WriteLine(res10.ReturnValue);
                Console.WriteLine("The task due date was updated successfully");
            }
            /**
            ////Non legit date..
            string test9 = taskService.UpdateTaskDueDate("shooki@gmail.com", "First Project", 0, 12345, new DateTime(1990, 4, 5));
            Response res9 = JsonSerializer.Deserialize<Response>(test9)!;
            if (res9.ErrorOccured)
            {
                Console.WriteLine(res9.ErrorMessage);
            }
            else
            {
                Console.WriteLine(res9.ReturnValue);
                Console.WriteLine("The task was created successfully");
            }
            **/
        }
        /// <summary>
        /// This method runs tests on updating task Title process
        /// </summary>
        public void runUpdateTaskTitleTest()
        {
            /// regular example.
            string test10 = taskService.UpdateTaskTitle("Almogim@gmail.com", "board2", 0, 1, "Mission two123");
            Response res10 = JsonSerializer.Deserialize<Response>(test10)!;
            if (res10.ErrorOccured)
            {
                Console.WriteLine("Test has passed.");
            }
            else
            {
                Console.WriteLine(res10.ReturnValue);
                Console.WriteLine("Test has failed!");
            }

            ///empty Title.
            string test11 = taskService.UpdateTaskTitle(null, "board1", 0, 12345, "");
            Response res11 = JsonSerializer.Deserialize<Response>(test11)!;
            if (res11.ErrorOccured)
            {
                Console.WriteLine("Test has passed.");
            }
            else
            {
                Console.WriteLine("Test has failed!");
            }

            //// 51 char in Title
            string test12 = taskService.UpdateTaskTitle("shooki@gmail.com", "First Project", 0, 12345, "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxy");
            Response res12 = JsonSerializer.Deserialize<Response>(test12)!;
            if (res12.ErrorOccured)
            {
                Console.WriteLine("Test has passed.");
            }
            else
            {
                Console.WriteLine("Test has failed!");
            }

            ////exactly 50 char in Title.
            string test13 = taskService.UpdateTaskTitle("shooki@gmail.com", "First Project", 0, 12345, "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwx");
            Response res13 = JsonSerializer.Deserialize<Response>(test13)!;
            if (res13.ErrorOccured)
            {
                Console.WriteLine("Test has passed.");
            }
            else
            {
                Console.WriteLine("Test has failed!");
            }
            // Should work in Program
            Response res14 = JsonSerializer.Deserialize<Response>(taskService.UpdateTaskTitle("shooki@gmail.com", "board1", 0, 1, "New Title!"))!;
            if(res14.ErrorOccured)
            {
                Console.WriteLine("Test has failed!");
            }
            else
            {
                Console.WriteLine("Test has passed.");
            }
        }
        /// <summary>
        /// This method runs tests on updating task Description process
        /// </summary>
        public void runUpdateTaskDescriptionTest()
        {
            /// regular example.
            string test14 = taskService.UpdateTaskDescription("Almogim@gmail.com", "board1", 0, 1, "svsv123");
            Response res14 = JsonSerializer.Deserialize<Response>(test14)!;
            if (res14.ErrorOccured)
            {
                Console.WriteLine(res14.ErrorMessage);
            }
            else
            {
                Console.WriteLine(res14.ReturnValue);
                Console.WriteLine("The task Description was updated successfully");
            }
            
            ////empty Description.
            string test15 = taskService.UpdateTaskDescription("shooki@gmail.com", "First Project", 0, 12345, "");
            Response res15 = JsonSerializer.Deserialize<Response>(test15)!;
            if (res15.ErrorOccured)
            {
                Console.WriteLine(res15.ErrorMessage);
            }
            else
            {
                Console.WriteLine("The task was created successfully");
            }

            ////301 char in Description.
            string test16 = taskService.UpdateTaskDescription("shooki@gmail.com", "First Project", 0, 12345,
                                                              "abcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghij" +
                                                              "abcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghij" +
                                                              "abcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijz");
            Response res16 = JsonSerializer.Deserialize<Response>(test16)!;
            if (res16.ErrorOccured)
            {
                Console.WriteLine(res16.ErrorMessage);
            }
            else
            {
                Console.WriteLine("The task was created successfully");
            }

            ////exactly 300 char in Description.
            string test17 = taskService.UpdateTaskDescription("shooki@gmail.com", "First Project", 0, 12345,
                                                              "abcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghij" +
                                                              "abcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghij" +
                                                              "abcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghij");
            Response res17 = JsonSerializer.Deserialize<Response>(test17)!;
            if (res17.ErrorOccured)
            {
                Console.WriteLine(res17.ErrorMessage);
            }
            else
            {
                Console.WriteLine("The task was created successfully");
            }
            
        }

        /// <summary>
        /// This method runs tests on deletion task process
        /// </summary>
        public void runDeleteTaskTest()
        {
            ////Legit id.
            string test20 = taskService.DeleteTask(1, "Almogim@gmail.com", "board1");
            Response res20 = JsonSerializer.Deserialize<Response>(test20)!;
            if (res20.ErrorOccured)
            {
                Console.WriteLine(res20.ErrorMessage);
            }
            else
            {
                Console.WriteLine("The task was deleted successfully");
            }
            /**
            ////nonlegit id.
            string test21 = taskService.DeleteTask(-3581235, "yovelhd6@gmail.com", "mission1");
            Response res21 = JsonSerializer.Deserialize<Response>(test21)!;
            if (res21.ErrorOccured)
            {
                Console.WriteLine(res21.ErrorMessage);
            }
            else
            {
                Console.WriteLine("The task was created successfully");
            }
            **/
        }
        /// <summary>
        /// This method tests the functionality of retrieving all in progress tasks by email and case of invalid email
        /// </summary>
        public void InProgressTasksTest()
        {
            Response res0 = JsonSerializer.Deserialize<Response>(taskService.InProgressTasks("shooki@gmail.com"))!;
            if (res0.ErrorOccured)
            {
                Console.WriteLine(res0.ErrorMessage);
            }
            else
            {
                List<IntroSE.Kanban.Backend.BusinessLayer.Task> tasks = JsonSerializer.Deserialize<List<IntroSE.Kanban.Backend.BusinessLayer.Task>>((JsonElement)res0.ReturnValue)!;

                foreach (IntroSE.Kanban.Backend.BusinessLayer.Task task in tasks)
                {
                    Console.WriteLine("Id: " + task.Id + ", Title: " + task.Title + ", Creation Time: " +
                    task.CreationTime + ", Description: " + task.Description + ", Due Date " + task.DueDate);
                }
                Console.WriteLine("In progress tasks were received successfully");
            }
            
            //It supposed to fail because the user email doesn't exist
            Response res1 = JsonSerializer.Deserialize<Response>(taskService.InProgressTasks("example123"))!;
            if (res1.ErrorOccured)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("In progress tasks were received successfully");
            }
            
        }
        /// <summary>
        /// The method tests the functionality of task assignment to a user.
        /// tests cases such as invalid/non-existing email account, non-exisiting board, invalid column ordinal etc.,
        /// </summary>
        public void AssignTaskTest()
        {
            Response res0 = JsonSerializer.Deserialize<Response>(taskService.AssignTask("shooki@gmail.com", "board1", 0, 1, "gilb@gmail.com"))!;
            if (res0.ErrorOccured)
            {
                Console.WriteLine("Test has failed!");
            }
            else
            {
                Console.WriteLine("Test has passed.");
            }

            Response res1 = JsonSerializer.Deserialize<Response>(taskService.AssignTask(null, "dummyboard", -200, 1, "Almogim@gmail.com"))!;
            if (res1.ErrorOccured)
            {
                Console.WriteLine("Test has passed.");
            }
            else
            {
                Console.WriteLine("Test has failed!");
            }

            Response res2 = JsonSerializer.Deserialize<Response>(taskService.AssignTask("dummyemail", "board1", 0, 1, "Almogim@gmail.com"))!;
            if (res2.ErrorOccured)
            {
                Console.WriteLine("Test has passed.");
            }
            else
            {
                Console.WriteLine("Test has failed!");
            }

            Response res3 = JsonSerializer.Deserialize<Response>(taskService.AssignTask("gilb@gmail.com", "board1", 0, 1, "shooki@gmail.com"))!;
            if (res3.ErrorOccured)
            {
                Console.WriteLine("Test has failed!");
            }
            else
            {
                Console.WriteLine("Test has passed.");
            }

            Response res4 = JsonSerializer.Deserialize<Response>(taskService.AssignTask("yovel@gmail.com", "board1", 1, 1, "gilbarel@gmail.com"))!;
            if (res4.ErrorOccured)
            {
                Console.WriteLine("Test has passed.");
            }
            else
            {
                Console.WriteLine("Test has failed!");
            }

            Response res5 = JsonSerializer.Deserialize<Response>(taskService.AssignTask("gilb@gmail.com", "board1", 0, 1, null))!;
            if (res5.ErrorOccured)
            {
                Console.WriteLine("Test has passed!");
            }
            else
            {
                Console.WriteLine("Test has failed!");
            }


        }
    }
}
