using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaiGame7.Model.MyModel
{
    public class FightStateDetailModel
    {
        public int DateID { get; set; }
        public int STeamID { get; set; }
        public int ETeamID { get; set; }
        public string PhoneNumber { get; set; }
        public string FightTime { get; set; }
        public string STeamName { get; set; }
        public string ETeamName { get; set; }
        public int Money { get; set; }
        public string SFightAddress { get; set; }
        public string EFightAddress { get; set; }
        public string SFightPic { get; set; }
        public string EFightPic { get; set; }
        public string CurrentState { get; set; }
        public DateTime StateTime { get; set; }
        public string StateTimeStr { get; set; }
    }
}
