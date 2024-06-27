using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BackendTests
{
    class TestUserService
    {
        private readonly UserService userService;

        public TestUserService(UserService userService)
        {
            this.userService = userService;
        }
        
        /// <summary>
        /// This function tests all the registration cases of the user.
        /// </summary>
        public void RunRegistrationTests()
        {
            string res0 = userService.Register("Almogim@gmail.com", "aA123456");
            Response message0 = JsonSerializer.Deserialize<Response>(res0)!;
            if (message0.ErrorOccured)
            {
                Console.WriteLine(message0.ErrorMessage);
            }
            else
            {
                Console.WriteLine(message0.ReturnValue);
                Console.WriteLine("Registration was done successfully.");
            }
            /**
            string res1 = userService.Register("gilb@gmail.com", "Aa123456789");
            Response message1 = JsonSerializer.Deserialize<Response>(res1)!;
            if (message1.ErrorOccured)
            {
                Console.WriteLine(message1.ErrorMessage);
            }
            else
            {
                Console.WriteLine(message1.ReturnValue);
                Console.WriteLine("Registration was done successfully.");
            }
            **/
            /**
            string res1 = userService.Register(null, "5T6y7u8i");
            Response message1 = JsonSerializer.Deserialize<Response>(res1)!;
            if (message1.ErrorOccured)
            {
                Console.WriteLine(message1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Registration was done successfully.");
            }

            string res2 = userService.Register("yovel@gmail.com", "Haddad1234!");
            Response message2 = JsonSerializer.Deserialize<Response>(res2)!;
            if (message2.ErrorOccured)
            {
                Console.WriteLine(message2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Registration was done successfully.");
            }

            string res3 = userService.Register("gilbarel@gmail.com", "SeIntro1");
            Response message3 = JsonSerializer.Deserialize<Response>(res3)!;
            if (message3.ErrorOccured)
            {
                Console.WriteLine(message3.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Registration was done successfully.");
            }

            string res4 = userService.Register("almog@gmail.com", "123456");
            Response message4 = JsonSerializer.Deserialize<Response>(res4)!;
            if (message4.ErrorOccured)
            {
                Console.WriteLine(message4.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Registration was done successfully.");
            }

            string res5 = userService.Register("david@gmail.com", "    ");
            Response message5 = JsonSerializer.Deserialize<Response>(res5)!;
            if (message5.ErrorOccured)
            {
                Console.WriteLine(message5.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Registration was done successfully.");
            }

            string res6 = userService.Register("invalidemail", "1Q2w3e4r");
            Response message6 = JsonSerializer.Deserialize<Response>(res6)!;
            if (message6.ErrorOccured)
            {
                Console.WriteLine(message6.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Registration was done successfully.");
            }
            **/
        }
        /// <summary>
        /// This function tests the Login action of the user according to its email and password.
        /// </summary>
        public void RunLoginTests()
        {
            string res7 = userService.Login("Almogim@gmail.com", "Aa12345678");
            Response message7 = JsonSerializer.Deserialize<Response>(res7)!;
            if (message7.ErrorOccured)
            {
                Console.WriteLine(message7.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Login was done successfully.");
            }
            /**
            string res8 = userService.Login("yovel@gmail.com", "123456");
            Response message8 = JsonSerializer.Deserialize<Response>(res8)!;
            if (message8.ErrorOccured)
            {
                Console.WriteLine(message8.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Login was done successfully.");
            }
            //It supposed to fail because the email doesn't exist
            string res9 = userService.Login("noreply@gmail.com", "456128");
            Response message9 = JsonSerializer.Deserialize<Response>(res9)!;
            if (message9.ErrorOccured)
            {
                Console.WriteLine(message9.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Login was done successfully.");
            }
            //It supposed to fail because the email doesn't exist
            string res10 = userService.Login(null, "IntroductiontoSE2023");
            Response message10 = JsonSerializer.Deserialize<Response>(res10)!;
            if (message10.ErrorOccured)
            {
                Console.WriteLine(message10.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Login was done successfully.");
            }
            **/

        }
        
        
        /// <summary>
        /// This function tests the entire Logout cases according to the email parameter.
        /// </summary>
        public void LogoutTests()
        {
            string res15 = userService.Logout("gilb@gmail.com");
            Response message15 = JsonSerializer.Deserialize<Response>(res15)!;
            if (message15.ErrorOccured)
            {
                Console.WriteLine(message15.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Logout was done successfully.");
            }
            /**
            string res16 = userService.Logout("almog@gmail.com");
            Response message16 = JsonSerializer.Deserialize<Response>(res16)!;
            if (message16.ErrorOccured)
            {
                Console.WriteLine(message16.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Logout was done successfully.");
            }
            //It supposed to fail because the email doesn't exist
            string res17 = userService.Logout("              ");
            Response message17 = JsonSerializer.Deserialize<Response>(res17)!;
            if (message17.ErrorOccured)
            {
                Console.WriteLine(message17.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Logout was done successfully.");
            }
            **/
        }

        /// <summary>
        /// This method tests the functionality of loading all users
        /// </summary>
        public void LoadUsersTest()
        {
            Response res0 = JsonSerializer.Deserialize<Response>(userService.LoadUsers())!;
            if (res0.ErrorOccured)
            {
                Console.WriteLine(res0.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Users have received successfully");
            }
        }
        /// <summary>
        /// This method tests the functionality of deleting all users
        /// </summary>
        public void DeleteUsersTest()
        {
            Response res0 = JsonSerializer.Deserialize<Response>(userService.DeleteUsers())!;
            if (res0.ErrorOccured)
            {
                Console.WriteLine(res0.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Users have deleted successfully");
            }
        }

    }
}
