using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using IntroSE.Kanban.Backend.BusinessLayer;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class ColumnToSend
    {
        public Column column { get; set; }
        public ColumnToSend(Column column)
        {
            this.column = column;
        }
        public List<BusinessLayer.Task> GetTasks()
        {
            return column.tasks;
        }
    }
}
