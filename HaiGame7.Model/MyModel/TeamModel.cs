using System;
using System.Collections.Generic;

namespace HaiGame7.Model.MyModel
{
    public class TeamModel
    {
        public Nullable<int> Creater { get; set; }
        public int TeamID { get; set; }
        public string TeamName { get; set; }
        public int UserCount { get; set; }
        public string TeamLogo { get; set; }
        public string TeamDescription { get; set; }
        public string TeamType { get; set; }
        public Nullable<int> FightScore { get; set; }
        public Nullable<int> Asset { get; set; }
        public Nullable<int> IsDeault { get; set; }
        public Nullable<int> WinCount { get; set; }
        public Nullable<int> LoseCount { get; set; }
        public Nullable<int> FollowCount { get; set; }
        public string Role { get; set; }
        public string CreateTime { get; set; }
        public string RecruitContent { get; set; }
        public string CreaterPicture { get; set; }
        public List<TeamUserModel> TeamUser { get; set; }
    }
}
