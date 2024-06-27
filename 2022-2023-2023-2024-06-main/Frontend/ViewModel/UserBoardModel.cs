using Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
    public class UserBoardModel : NotifiableObject
    {
        public BoardModel Board { get; set; }
        public BackendController Controller { get; private set; }
        public string BacklogTitle { get; private set; } 
        public string InProgressTitle { get; private set; } 
        public string DoneTitle { get; private set; }

        public UserBoardModel(BoardModel boardModel)
        {
            Controller = new BackendController();
            Board = boardModel;
            BacklogTitle = "Backlog. Limit: " + Board.BacklogLimit;
            InProgressTitle = "InProgress. Limit: " + Board.InProgressLimit;
            DoneTitle = "Done. Limit: " + Board.DoneLimit;

        }
    }
}
