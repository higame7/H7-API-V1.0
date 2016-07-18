using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaiGame7.Model.MyModel
{
    public class MyApplyTeamModel
    {
        public int MessageID { get; set; }
        public int TeamID { get; set; }
        public string TeamName { get; set; }
        public string TeamLogo { get; set; }
        public string TeamDescription { get; set; }
        public string State { get; set; }
        public string SendTime { get; set; }
        public int FightScore { get; set; }
        public int Asset { get; set; }
    }
}
