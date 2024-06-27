using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    public class BoardModel : NotifiableModelObject
    {
        private int boardId;
        public int BoardId
        {
            get => boardId;
            set
            {
                boardId = value;
                RaisePropertyChanged("BoardId");
            }
        }

        private string boardName;
        public string BoardName
        {
            get => boardName;
            set
            {
                boardName = value;
                RaisePropertyChanged("BoardName");
            }
        }
        public ObservableCollection<TaskModel> Backlog { get; set; }
        public ObservableCollection<TaskModel> InProgress { get; set; }
        public ObservableCollection<TaskModel> Done { get; set; }

        public int BacklogLimit { get; set; }
        public int InProgressLimit { get; set; }
        public int DoneLimit { get; set; }

        public BoardModel(BackendController controller, int boardId) : base(controller)
        {
            BoardId = boardId;
            BoardName = Controller.GetBoardName(BoardId);
            Backlog = new ObservableCollection<TaskModel>(Controller.GetColumn(BoardId, IntroSE.Kanban.Backend.BusinessLayer.Board.BACKLOG));
            InProgress = new ObservableCollection<TaskModel>(Controller.GetColumn(BoardId, IntroSE.Kanban.Backend.BusinessLayer.Board.IN_PROGRESS));
            Done = new ObservableCollection<TaskModel>(Controller.GetColumn(BoardId, IntroSE.Kanban.Backend.BusinessLayer.Board.DONE));

            BacklogLimit = Controller.GetColumnLimit(BoardId, IntroSE.Kanban.Backend.BusinessLayer.Board.BACKLOG);
            InProgressLimit = Controller.GetColumnLimit(BoardId, IntroSE.Kanban.Backend.BusinessLayer.Board.IN_PROGRESS);
            DoneLimit = Controller.GetColumnLimit(BoardId, IntroSE.Kanban.Backend.BusinessLayer.Board.DONE);

        }
    }
}
