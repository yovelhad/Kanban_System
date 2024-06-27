using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class Task
    {
        public TaskDTO _taskDTO { set; get; }
        private int _ordinalNumber;
        public int OrdinalNumber { get => _ordinalNumber; set { _ordinalNumber = value; } }       
        public int Id { get; set; }
        public DateTime CreationTime { get; set; }
        private string _title;
        public string Title { get => _title; set { _title = value; } }
        private string _description;
        public string Description { get => _description; set { _description = value; } }
        private DateTime _duedate;
        public DateTime DueDate { get => _duedate; set { _duedate = value;  } }
        private string _assignee;
        public string Assignee { get => _assignee; set { _assignee = value;  } }

        public Task() { }

        /// <summary>
        /// Creates a task object in the system.
        /// </summary>
        /// <param name="title">Title of the task.</param>
        /// <param name="description">Description of the task.</param>
        /// <param name="dueDate">Due date of the task.</param>
        /// <param name="taskId">The task's ID.</param>
        public Task(string title, string description, DateTime dueDate, int taskId)
        {
            _ordinalNumber = 0;
            Id = taskId;
            CreationTime = DateTime.Now;
            _duedate = dueDate;
            _title = title;
            _description = description;
            _assignee = "";
        }

        /// <summary>
        /// Creates a task object to be inserted to the database.
        /// </summary>
        /// <param name="dTask">TaskDTO object to be inserted to db.</param>
        public Task(TaskDTO dTask)
        {
            _title = dTask.TaskTitle;
            _description = dTask.TaskDescription;
            _duedate = dTask.TaskDueDate;
            CreationTime = dTask.TaskCreationTime;
            Id = dTask.TaskId;
            _assignee = dTask.Assignee;
        }
    }
}
