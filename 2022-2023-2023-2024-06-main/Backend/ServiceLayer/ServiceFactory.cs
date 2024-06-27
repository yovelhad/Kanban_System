using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class ServiceFactory
    {
        public Services create()
        {
            UserFacade uf = new UserFacade();
            UserService us = new UserService(uf);
            KanbanFacade kf = new KanbanFacade(uf);
            BoardService bs = new BoardService(kf);
            TaskService ts = new TaskService(kf);
            return new Services(us, bs, ts);
        }
    }
}
