using log4net.Config;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class BoardDTO
    {
        internal const string boardNameColumnName = "BoardName";
        internal const string boardIdColumnName = "BoardId";
        internal const string ownerEmailColumnName = "OwnerEmail";
        private BoardController _controller;

        private string _name;
        public string BoardName
        {
            get => _name;
            set
            {
                _controller.Update(_boardId, boardNameColumnName, value);
                _name = value;
            }
        }

        private int _boardId;
        public int BoardId
        {
            get => _boardId;
            set
            {
                _controller.Update(_boardId, boardIdColumnName, value);
            }
        }
        private string _ownerEmail;
        public string ownerEmail
    {
            get => _ownerEmail;
            set
        {
                _controller.Update(_boardId, ownerEmailColumnName, value);
            }

        }
        /// <summary>
        /// Inserts board to boards table
        /// </summary>
        /// <returns> boolean value whether the Insert action has succeeded </returns>
        public bool Insert()
        {
            if (_controller.Insert(this))
            {
                return true;
            }
            throw new ArgumentException($"error when try to Insert board to DB, board id");
        }
        /// <summary>
        /// Deletes board from boards table
        /// </summary>
        /// <returns> boolean value whether the Delete action has succeeded </returns>
        public bool Delete()
        {
            if (_controller.Delete(this))
            {
                return true;
            }
            throw new ArgumentException($"error when try to Insert board to DB, board id");
        }

        public BoardDTO(int boardId, string boardName,string ownerEmail)
        {
            _name = boardName;
            _boardId = boardId;
            _ownerEmail = ownerEmail;
            _controller = new BoardController();
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }
    }
}
