using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaiGame7.Model.MyModel
{
    public class TeamRankModel
    {
        public int TeamID { get; set; }//战队ID
        public string TeamName { get; set; }//战队名称
        public string TeamPicture { get; set; }//战队logo
        public string CreateTime { get; set; }//创建日期
        public string RecruitContent { get; set; }//发布内容
        public string TeamDescription { get; set; }//战队口号
        public int HotScore { get; set; }//热度
        public int WinCount { get; set; }//胜场
        public int LoseCount { get; set; }//负场
        public int FollowCount { get; set; }//认怂场
        public int FightScore { get; set; }//战斗力
        public int Asset { get; set; }//氦气
        public int CreateUserID { get; set; }//创建人ID
        public string CreateUserLogo { get; set; }//创建人头像
        public List<TeamUserModel> UserImage { get; set; }//队员头像
    }
}
