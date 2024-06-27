
using System;
using Frontend.Model;

namespace Frontend.ViewModel
{
    public class MainViewModel : NotifiableObject
    {
        public BackendController Controller { get; private set; }
        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                this._username = value;
            }
        }
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                this._password = value;
            }
        }
        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                this._message = value;
                RaisePropertyChanged("Message");
            }
        }

        public MainViewModel()
        {
            Controller = new BackendController();

        }
        /// <summary>
        /// Creates a model of a user after Login
        /// </summary>
        /// <returns>A UserModel with the existing user credentials</returns>
        public UserModel Login()
        {
            Message = "";
            try
            {
                return Controller.Login(Username, Password);
            }
            catch (Exception e)
            {
                Message = e.Message;
                return null;
            }
        }
        /// <summary>
        /// Creates a model of a user after Registration.
        /// </summary>
        /// <returns>A new UserModel with his user credentials</returns>
        public UserModel Register()
        {
            Message = "";
            try
            {
                UserModel u = Controller.Register(Username, Password);
                Message = "Registered successfully";
                return u;
            }
            catch (Exception e)
            {
                Message = e.Message;
                return null;
            }
        }

    }
}