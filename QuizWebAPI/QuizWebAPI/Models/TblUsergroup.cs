using System;
using System.Collections.Generic;

#nullable disable

namespace QuizWebAPI.Models
{
    public partial class TblUsergroup
    {
        public TblUsergroup()
        {
            TblQuestions = new HashSet<TblQuestion>();
            TblUserScores = new HashSet<TblUserScore>();
            TblUsers = new HashSet<TblUser>();
        }

        public int UsergroupId { get; set; }
        public string UsergroupName { get; set; }

        public virtual ICollection<TblQuestion> TblQuestions { get; set; }
        public virtual ICollection<TblUserScore> TblUserScores { get; set; }
        public virtual ICollection<TblUser> TblUsers { get; set; }
    }
}
