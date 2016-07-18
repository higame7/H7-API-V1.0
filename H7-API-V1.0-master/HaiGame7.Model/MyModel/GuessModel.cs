using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaiGame7.Model.MyModel
{
    public class GuessModel
    {
        public int GuessID { get; set; }
        public string GuessName { get; set; }
        public int STeamID { get; set; }
        public string STeamName { get; set; }
        public string STeamLogo { get; set; }
        public int ETeamID { get; set; }
        public string ETeamName { get; set; }
        public string ETeamLogo { get; set; }
        public string MatchTime { get; set; }
        public string GuessType { get; set; }
        public Nullable<decimal> STeamOdds { get; set; }
        public Nullable<decimal> ETeamOdds { get; set; }
        public string EndTime { get; set; }
        public int AllMoney { get; set; }
        public int AllUser { get; set; }
    }
}
