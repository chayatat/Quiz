using System;
using System.Collections.Generic;

#nullable disable

namespace QuizWebAPI.Models
{
    public partial class TblUser
    {
        public TblUser()
        {
            TblUserScores = new HashSet<TblUserScore>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public int UsergroupId { get; set; }
        public string UserStatus { get; set; }
        public int? UserTotalScore { get; set; }

        public virtual TblUsergroup Usergroup { get; set; }
        public virtual ICollection<TblUserScore> TblUserScores { get; set; }
    }
}
