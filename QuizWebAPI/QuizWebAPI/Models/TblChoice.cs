using System;
using System.Collections.Generic;

#nullable disable

namespace QuizWebAPI.Models
{
    public partial class TblChoice
    {
        public TblChoice()
        {
            TblUserScores = new HashSet<TblUserScore>();
        }

        public int ChoiceId { get; set; }
        public string ChoiceName { get; set; }
        public int ChoiceScore { get; set; }
        public int QuestionId { get; set; }
        public int? ChoiceSort { get; set; }

        public virtual TblQuestion Question { get; set; }
        public virtual ICollection<TblUserScore> TblUserScores { get; set; }
    }
}
