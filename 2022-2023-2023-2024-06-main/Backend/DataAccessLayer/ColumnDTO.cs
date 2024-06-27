using IntroSE.Kanban.Backend.BusinessLayer;
using log4net;
using log4net.Config;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class ColumnDTO
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        internal const string ColumnTitleColumnName = "ColumnTitle";
        internal const string boardIdColumnName = "BoardId";
        internal const string ColumnLimitColumnName = "ColumnLimit";
        internal const string ColumnOrdinalColumnName = "ColumnOrdinal";
        private ColumnController _controller;
        internal bool isPersisted { get; set; }


        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                _controller.Update(BoardId, ColumnOrdinal, ColumnTitleColumnName, value);
            }
        }

        private int _boardId;
        public int BoardId
        {
            get => _boardId;
            set
            {
                _boardId = value;
                _controller.Update(BoardId, ColumnOrdinal, boardIdColumnName, value);
            }
        }

        
        private int _limit;
        public int Limit
        {
            get => _limit;
            set
            {
                _limit = value;
                _controller.Update(BoardId, ColumnOrdinal, ColumnLimitColumnName, value);
            }
        }

        private int _ordinal;
        public int ColumnOrdinal
        {
            get => _ordinal;
            set
            {
                _ordinal = value;
                _controller.Update(BoardId, ColumnOrdinal, ColumnOrdinalColumnName, value);
            }
        }
        /// <summary>
        /// Deletes column from columns table
        /// </summary>
        /// <returns> boolean value whether the Delete action has succeeded </returns>
        internal bool Delete()
        {
            if (_controller.Delete(this))
            {
                isPersisted = false;
                log.Info($"Succesfully deleted from 'Column' database. The column with the Title:'{Title}', which is in board:'{BoardId}'");
                return true;
            }
            log.Error($"Deletion failed for column with the Title:'{Title}', which is in board '{BoardId}'");
            throw new Exception($"Deletion failed for column with the Title:'{Title}', which is in board:'{BoardId}'");
        }
        /// <summary>
        /// Inserts column to columns table
        /// </summary>
        /// <returns> boolean value whether the Insert action has succeeded </returns>
        public bool Persist()
        {
            if (_controller.Insert(this))
            {
                isPersisted = true;
                log.Info($"Succesfully inserted column to 'Column' database. Title:'{Title}', which is in board:'{BoardId}'");
                return true;
            }
            throw new Exception($"error when try to Insert column to 'Column' database. Title:'{Title}', which is in board:'{BoardId}'");
        }

        public ColumnDTO(int boardId, string ColumnName, int ordinial, int limit)
        {
            _title = ColumnName;
            _boardId = boardId;
            _limit = limit;
            _ordinal = ordinial;
            isPersisted = false;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            _controller = new ColumnController();
        }
    }
}
