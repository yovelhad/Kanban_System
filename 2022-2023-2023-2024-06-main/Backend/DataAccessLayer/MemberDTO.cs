using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class MemberDTO
    {
        public const string EmailColumnName = "Email";
        public const string BoardIDColumnName = "BoardID";
        private bool isPersisted {  get; set; }

        private MemberController _controller;
        public string Email {get; set;}
        public int BoardID { get; set;}

        public MemberDTO(string email, int boardID)
        {
            this.Email = email;
            this.BoardID = boardID;
            isPersisted = false;
            _controller = new MemberController();
        }
        /// <summary>
        /// Inserts member to members table
        /// </summary>
        /// <returns> boolean value whether the Insert action has succeeded </returns>
        public bool Persist()
        {
            if (_controller.Insert(this))
            {
                isPersisted = true;
                return true;
            }
            throw new ArgumentException($"error when try to Insert board to DB, board id");
        }
        /// <summary>
        /// Deletes member from members table
        /// </summary>
        /// <returns> boolean value whether the Delete action has succeeded </returns>
        public bool Delete()
        {
            if (_controller.Delete(this))
            {
                isPersisted = false;
                return true;
            }
            throw new ArgumentException($"error when try to Insert board to DB, board id");
        }


    }
}
