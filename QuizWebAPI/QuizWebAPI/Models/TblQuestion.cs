using System;
using System.Collections.Generic;

#nullable disable

namespace QuizWebAPI.Models
{
    public partial class TblQuestion
    {
        public TblQuestion()
        {
            TblChoices = new HashSet<TblChoice>();
            TblUserScores = new HashSet<TblUserScore>();
        }

        public int QuestionId { get; set; }
        public string QuestionName { get; set; }
        public int QuestionSort { get; set; }
        public int UsergroupId { get; set; }

        public virtual TblUsergroup Usergroup { get; set; }
        public virtual ICollection<TblChoice> TblChoices { get; set; }
        public virtual ICollection<TblUserScore> TblUserScores { get; set; }
    }
}
