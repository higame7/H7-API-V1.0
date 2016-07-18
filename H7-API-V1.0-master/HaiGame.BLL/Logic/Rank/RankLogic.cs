using HaiGame7.BLL.Enum;
using HaiGame7.BLL.Logic.Common;
using HaiGame7.Model.EFModel;
using HaiGame7.Model.MyModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;

namespace HaiGame7.BLL
{
    public class RankLogic
    {
        #region 个人排行
        public string UserRank(RankParameterModel rank)
        {
            string result = "";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();
            //个人排行：昵称，签名，氦气，战斗力，大神系数
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                //联合查询
                string sql;
                if (rank.RankType == "Asset")
                {
                    sql = "select" +
                          " t1.UserID as UserID," +
                          " t1.Address as Address," +
                          " t1.Sex as Sex," +
                          " CONVERT(varchar(100), t1.RegisterDate, 20) as RegDate," +
                          " t1.UserWebNickName as NickName," +
                          " t1.UserWebPicture as UserPicture," +
                          " t1.Hobby," +
                          " t2.GameID," +
                          " t3.GamePower," +
                          " t3.GameGrade," +
                          " (CASE WHEN(select sum(t4.VirtualMoney) from" +
                          " db_AssetRecord t4 where t4.UserID = t1.UserID) IS NULL THEN 0 ELSE (select sum(t4.VirtualMoney) from" +
                          " db_AssetRecord t4 where t4.UserID = t1.UserID) END) as Asset" +
                          " from db_User t1 left join" +
                          " db_GameIDofUser t2 on t1.UserID = t2.UserID" +
                          " left join db_GameInfoofPlatform t3" +
                          " on t2.UGID = t3.UGID" +
                          " where t2.GameType = 'DOTA2'" +
                          " order by " + rank.RankType  +" "+ rank.RankSort + ",t1.RegisterDate ";
                }
                else
                {
                    sql = "select" +
                          " t1.UserID as UserID," +
                          " t1.Address as Address," +
                          " t1.Sex as Sex," +
                          " CONVERT(varchar(100), t1.RegisterDate, 20) as RegDate," +
                          " t1.UserWebNickName as NickName," +
                          " t1.UserWebPicture as UserPicture," +
                          " t1.Hobby," +
                          " t2.GameID," +
                          " t3.GamePower," +
                          " t3.GameGrade," +
                          " (CASE WHEN(select sum(t4.VirtualMoney) from" +
                          " db_AssetRecord t4 where t4.UserID = t1.UserID) IS NULL THEN 0 ELSE (select sum(t4.VirtualMoney) from" +
                          " db_AssetRecord t4 where t4.UserID = t1.UserID) END) as Asset" +
                          " from db_User t1 left join" +
                          " db_GameIDofUser t2 on t1.UserID = t2.UserID" +
                          " left join db_GameInfoofPlatform t3" +
                          " on t2.UGID = t3.UGID" +
                          " where t2.GameType = 'DOTA2'" +
                          " order by cast(" + rank.RankType + " as int) " +" "+ rank.RankSort + ",t1.RegisterDate ";
                }
                

                 var  userRank = context.Database.SqlQuery<UserRankModel>(sql)
                                  .Skip((rank.StartPage - 1) * rank.PageCount)
                                  .Take(rank.PageCount).ToList();

                if (userRank == null)
                {
                    //无游戏数据
                    message.Message = MESSAGE.NOGAMEDATA;
                    message.MessageCode = MESSAGE.NOGAMEDATA_CODE;
                }
                else
                {
                    //循环user，添加擅长英雄图标
                    for (int i = 0; i < userRank.Count; i++)
                    {
                        //擅长英雄
                        userRank[i].HeroImage = User.GetHeroImgeByUserID(userRank[i].UserID);
                    }
                    message.Message = MESSAGE.OK;
                    message.MessageCode = MESSAGE.OK_CODE;
                }
                returnResult.Add(message);
                returnResult.Add(userRank);
            }
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 战队排行
        public string TeamRank(RankParameterModel rank)
        {
            string result = "";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();

            //团队排行：战队名称，战斗力，氦气，热度
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                var sql = "SELECT" +
                          " t.TeamID," +
                          " t.TeamName," +
                          " t.TeamDescription," +
                          " t.TeamPicture," +
                          " (CASE WHEN t.WinCount IS NULL THEN 0 ELSE t.WinCount END) as WinCount," +
                          " (CASE WHEN t.LoseCount IS NULL THEN 0 ELSE t.LoseCount END) as LoseCount," +
                          " (CASE WHEN t.FollowCount IS NULL THEN 0 ELSE t.FollowCount END) as FollowCount," +
                          " CONVERT(varchar(100), t.CreateTime, 20) as CreateTime," +
                          " t3.Content as RecruitContent," +
                          " t.CreateUserID," +
                          " t2.UserWebPicture as CreateUserLogo," +
                          " (CASE WHEN t.FightScore IS NULL THEN 0 ELSE t.FightScore END) as FightScore," +
                          " (CASE WHEN t.Asset IS NULL THEN 0 ELSE t.Asset END) as Asset," +
                          " (" +
                          " ((CASE WHEN t.WinCount IS NULL THEN 0 ELSE t.WinCount END)" +
                          " +(CASE WHEN t.LoseCount IS NULL THEN 0 ELSE t.LoseCount END)" +
                          " +(CASE WHEN t.FollowCount IS NULL THEN 0 ELSE t.FollowCount END))*10" +
                          " + (CASE WHEN t.WinCount IS NULL THEN 0 ELSE t.WinCount END)*10 +" +
                          " (CASE WHEN t.FightScore IS NULL THEN 0 ELSE t.FightScore END)+" +
                          " (CASE WHEN t.Asset IS NULL THEN 0 ELSE t.Asset END))/ 3 as HotScore" +
                          " FROM db_Team t" +
                          " LEFT JOIN db_User t2 ON t.CreateUserID=t2.UserID" +
                          " LEFT JOIN db_Recruit t3 ON t.TeamID=t3.TeamID" +
                          " WHERE t.State = 0" +
                          " ORDER BY " + rank.RankType + " " + rank.RankSort + ", t.SysTime";

                 var teamRank = context.Database.SqlQuery<TeamRankModel>(sql)
                                 .Skip((rank.StartPage - 1) * rank.PageCount)
                                 .Take(rank.PageCount).ToList();

                if (teamRank == null)
                {
                    //无游戏数据
                    message.Message = MESSAGE.NOGAMEDATA;
                    message.MessageCode = MESSAGE.NOGAMEDATA_CODE;
                }
                else
                {
                    //循环team，添加战队成员图标
                    for (int i = 0; i < teamRank.Count; i++)
                    {
                        //战队成员图标
                        teamRank[i].UserImage = Team.GetTeamUserByUserID(teamRank[i].TeamID);
                    }
                    message.Message = MESSAGE.OK;
                    message.MessageCode = MESSAGE.OK_CODE;
                }
                returnResult.Add(message);
                returnResult.Add(teamRank);
            }
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion
    }
}
