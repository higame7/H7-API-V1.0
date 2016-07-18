using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaiGame7.Model.MyModel
{
    public class Guess2Model
    {
        public string EndTime { get; set; }
        public string MatchName { get; set; }
        public Nullable<int> STeamID { get; set; }
        public string STeamName { get; set; }
        public string STeamLogo { get; set; }
        public Nullable<int> ETeamID { get; set; }
        public string ETeamName { get; set; }
        public string ETeamLogo { get; set; }
        public Nullable<int> BetTeamID { get; set; }
        public string BetTeamName { get; set; }
        public string BetTeamLogo { get; set; }
        public string Result { get; set; }
        public string GuessTime { get; set; }
        public Nullable<decimal> Odds { get; set; }
        public Nullable<int> BetMoney { get; set; }
    }
}
