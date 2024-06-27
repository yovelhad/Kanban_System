using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class Services
    {
        public UserService USER_SERVICE { get; set; }
        public BoardService BOARD_SERVICE { get; set; }
        public TaskService TASK_SERVICE { get; set; }
        internal Services(UserService us, BoardService bs, TaskService ts) 
        {
            USER_SERVICE = us;
            BOARD_SERVICE = bs;
            TASK_SERVICE = ts;
        }
    }
}
