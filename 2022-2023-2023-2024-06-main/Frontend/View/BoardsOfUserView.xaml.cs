using System.Windows;
using Frontend.Model;
using Frontend.ViewModel;

namespace Frontend.View;

public partial class BoardsOfUserView : Window
{
    private BoardsOfUserViewModel viewModel;
    private UserModel user;

    public BoardsOfUserView(UserModel u)
    {
        InitializeComponent();
        user = u;
        viewModel = new BoardsOfUserViewModel(u);
        DataContext = viewModel;
    }
    /// <summary>
    /// Creates an event in the window after clicking the "Choose Board" button.
    /// </summary>
    /// <param name="sender"> The button that was clicked.</param>
    /// <param name="e">The event that occured</param>
    private void Choose_Button(object sender, RoutedEventArgs e)
    {
        BoardModel board = viewModel.ChoosenBoard();
        if (board != null)
        {
            UserBoard boardView = new UserBoard(user, board); //the chosenBoard Model
            boardView.Show();
            Close();
        }
    }
    /// <summary>
    /// Creates an event in the window after clicking the "Logout" button.
    /// </summary>
    /// <param name="sender">The button that was clicked.</param>
    /// <param name="e">The event that occured</param>
    private void LogOut_Click(object sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        Close();
    }
}