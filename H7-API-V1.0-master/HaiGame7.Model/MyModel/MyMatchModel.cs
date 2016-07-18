using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaiGame7.Model.MyModel
{
    public class MyMatchModel
    {
        public int MatchID { get; set; }
        public string MatchName { get; set; }
        public int STeamID { get; set; }
        public string STeamName { get; set; }
        public string STeamLogo { get; set; }
        public int ETeamID { get; set; }
        public string ETeamName { get; set; }
        public string ETeamLogo { get; set; }
        public string Result { get; set; }
        public string EndTime { get; set; }
    }
}
