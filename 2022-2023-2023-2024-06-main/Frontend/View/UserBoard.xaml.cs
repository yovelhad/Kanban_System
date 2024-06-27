using Frontend.Model;
using Frontend.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Frontend.View
{
    public partial class UserBoard : Window
    {
        private UserBoardModel viewModel;
        private UserModel user;

        public UserBoard(UserModel u, BoardModel bm)
        {
            InitializeComponent();
            user = u;
            viewModel = new UserBoardModel(bm);
            DataContext = viewModel;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            BoardsOfUserView boardsOfUserView = new BoardsOfUserView(user);
            boardsOfUserView.Show();
            this.Close();
        }
    }
}
