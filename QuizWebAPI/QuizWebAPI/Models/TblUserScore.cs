using System;
using System.Collections.Generic;

#nullable disable

namespace QuizWebAPI.Models
{
    public partial class TblUserScore
    {
        public int UserScoreId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int UsergroupId { get; set; }
        public string UsergroupName { get; set; }
        public int QuestionId { get; set; }
        public int ChoiceId { get; set; }
        public int ChoiceScore { get; set; }
        public string UserScoreStatus { get; set; }

        public virtual TblChoice Choice { get; set; }
        public virtual TblQuestion Question { get; set; }
        public virtual TblUser User { get; set; }
        public virtual TblUsergroup Usergroup { get; set; }
    }
}
