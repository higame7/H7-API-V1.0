using HaiGame7.Model.EFModel;
using HaiGame7.Model.MyModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HaiGame7.BLL.Logic.Common
{
    public class Team
    {
        #region 判断用户是否加入战队或创建战队
        public static bool IsCreateOrJoinTeam(int userID, HiGame_V1Entities context)
        {
            db_Team team = context.db_Team.
                Where(c => c.CreateUserID == userID).
                Where(c => c.State == 0).
                Where(c => c.IsDeault == 0).
                FirstOrDefault();
            if (team == null)
            {
                db_TeamUser teamUser = context.db_TeamUser.Where(c => c.UserID == userID).FirstOrDefault();
                if (teamUser == null)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region 获取我的默认战队信息
        public static TeamModel MyTeam(int userID, HiGame_V1Entities context)
        {
            TeamModel myTeam = new TeamModel();

            db_Team team = context.db_Team.
                Where(c => c.CreateUserID == userID).
                Where(c => c.State == 0).
                Where(c => c.IsDeault == 0).
                FirstOrDefault();
            if (team == null)
            {
                db_TeamUser teamUser = context.db_TeamUser.Where(c => c.UserID == userID).FirstOrDefault();
                db_Team team2 = context.db_Team.
                Where(c => c.TeamID == teamUser.TeamID).
                Where(c => c.State == 0).
                FirstOrDefault();
                if (team2 != null)
                {
                    myTeam.Asset = team2.Asset == null ? 0 : team2.Asset; ;
                    myTeam.TeamID = team2.TeamID;
                    myTeam.Creater = team2.CreateUserID;
                    myTeam.CreateTime = ((DateTime)team2.CreateTime).ToString("yyyy-MM-dd");
                    myTeam.FightScore = team2.FightScore == null ? 0 : team2.FightScore; ;
                    myTeam.FollowCount = team2.FollowCount == null ? 0 : team2.FollowCount;
                    myTeam.IsDeault = team2.IsDeault;
                    myTeam.LoseCount = team2.LoseCount == null ? 0 : team2.LoseCount;
                    myTeam.Role = "teamuser";
                    myTeam.TeamLogo = team2.TeamPicture;
                    myTeam.TeamName = team2.TeamName;
                    myTeam.TeamType = team2.TeamType;
                    myTeam.TeamDescription = team2.TeamDescription;
                    myTeam.WinCount = team2.WinCount == null ? 0 : team2.WinCount;
                    myTeam.RecruitContent = GetRecruitContentByTeamID(team2.TeamID);
                    myTeam.UserCount= GetTeamUserCountByTeamID(team2.TeamID);
                    //添加战队成员图标
                    myTeam.TeamUser = GetTeamUserByUserID(team2.TeamID);
                    //添加战队队长图标
                    myTeam.CreaterPicture = GetUserPictureByUserID(team2.CreateUserID);
                }
            }
            else
            {
                myTeam.Asset = team.Asset == null ? 0 : team.Asset;
                myTeam.Creater = team.CreateUserID;
                myTeam.TeamID = team.TeamID;
                myTeam.CreateTime = ((DateTime)team.CreateTime).ToString("yyyy-MM-dd");
                myTeam.FightScore = team.FightScore==null?0:team.FightScore;
                myTeam.FollowCount = team.FollowCount == null ? 0 : team.FollowCount;
                myTeam.IsDeault = team.IsDeault;
                myTeam.LoseCount = team.LoseCount == null ? 0 : team.LoseCount;
                myTeam.Role = "teamcreater";
                myTeam.TeamLogo = team.TeamPicture;
                myTeam.TeamName = team.TeamName;
                myTeam.TeamType = team.TeamType;
                myTeam.TeamDescription = team.TeamDescription;
                myTeam.WinCount = team.WinCount == null ? 0 : team.WinCount;
                myTeam.RecruitContent = GetRecruitContentByTeamID(team.TeamID);
                myTeam.UserCount = context.db_TeamUser.Where(c => c.TeamID == team.TeamID).ToList().Count;
                //添加战队成员图标
                myTeam.TeamUser = GetTeamUserByUserID(team.TeamID);
                //添加战队队长图标
                myTeam.CreaterPicture = GetUserPictureByUserID(team.CreateUserID);
            }

            return myTeam;
        }
        #endregion

        #region 获取我的所有战队信息
        public static List<TeamModel> MyAllTeam(int userID)
        {
            List<TeamModel> myTeamList = new List<TeamModel>();
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                List<db_Team> team = context.db_Team.
                Where(c => c.CreateUserID == userID).
                Where(c => c.State == 0).
                ToList();
                if (team == null)
                {
                    //应该不存在这种情况
                }
                else
                {
                    for (int i=0;i< team.Count;i++)
                    {
                        TeamModel myTeam = new TeamModel();
                        myTeam.Asset = team[i].Asset;
                        myTeam.Creater = team[i].CreateUserID;
                        myTeam.TeamID = team[i].TeamID;
                        myTeam.CreateTime = ((DateTime)team[i].CreateTime).ToString("yyyy-MM-dd");
                        myTeam.FightScore = team[i].FightScore;
                        myTeam.FollowCount = team[i].FollowCount;
                        myTeam.IsDeault = team[i].IsDeault;
                        myTeam.LoseCount = team[i].LoseCount;
                        myTeam.Role = "teamcreater";
                        myTeam.TeamLogo = team[i].TeamPicture;
                        myTeam.TeamName = team[i].TeamName;
                        myTeam.TeamType = team[i].TeamType;
                        myTeam.TeamDescription = team[i].TeamDescription;
                        myTeam.WinCount = team[i].WinCount;
                        myTeam.RecruitContent = GetRecruitContentByTeamID(team[i].TeamID);
                        myTeam.UserCount = GetTeamUserCountByTeamID(team[i].TeamID);
                        myTeamList.Add(myTeam);
                    }
                }
            }
            return myTeamList;
        }
        #endregion

        #region 根据战队ID获取战队信息
        public static TeamModel GetTeambyID(int teamID)
        {
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                TeamModel myTeam = new TeamModel();

                db_Team team = context.db_Team.
                    Where(c => c.TeamID == teamID).
                    FirstOrDefault();
                if (team != null)
                {
                    myTeam.Asset = team.Asset;
                    myTeam.Creater = team.CreateUserID;
                    myTeam.TeamID = team.TeamID;
                    myTeam.CreateTime = ((DateTime)team.CreateTime).ToString("yyyy-MM-dd");
                    myTeam.FightScore = team.FightScore;
                    myTeam.FollowCount = team.FollowCount;
                    myTeam.IsDeault = team.IsDeault;
                    myTeam.LoseCount = team.LoseCount;
                    myTeam.Role = "teamcreater";
                    myTeam.TeamLogo = team.TeamPicture;
                    myTeam.TeamName = team.TeamName;
                    myTeam.TeamType = team.TeamType;
                    myTeam.TeamDescription = team.TeamDescription;
                    myTeam.WinCount = team.WinCount;
                    myTeam.RecruitContent = GetRecruitContentByTeamID(team.TeamID);
                    myTeam.CreaterPicture = GetUserPictureByUserID(team.CreateUserID);
                    myTeam.TeamUser = GetTeamUserByUserID(team.TeamID);
                }
                return myTeam;
            }
        }
        #endregion

        #region 判断战队名称是否存在
        public static bool IsTeamNameExist(SimpleTeam2Model para)
        {
            bool isTeamNameExist = false;
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                TeamModel myTeam = new TeamModel();

                db_Team team = context.db_Team.
                                Where(c => c.TeamName == para.TeamName.Trim()).
                                Where(c => c.TeamID != para.TeamID).
                                FirstOrDefault();
                if (team != null)
                {
                    isTeamNameExist = true;
                }
            }
            return isTeamNameExist;
        }
        #endregion

        #region 根据UserID获取我的所有战队ID
        public static string MyAllTeamID(int userID)
        {
            string myAllTeamID = "(0)";

            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                //获取db_Team表里的数据
                var teamList = context.db_Team.Where(c => c.CreateUserID == userID).ToList();

                //获取db_TeamUser表里的数据
                if (teamList.Count == 0)
                {
                    var teamUser = context.db_TeamUser.Where(c => c.UserID == userID).FirstOrDefault();
                    if (teamUser != null)
                    {
                        myAllTeamID = "(" + teamUser.TeamID + ")";
                    }
                }
                else
                {
                    string temp = "";
                    for (int i = 0; i < teamList.Count; i++)
                    {
                        if (i == teamList.Count - 1)
                        {
                            temp = temp + teamList[i].TeamID.ToString();
                        }
                        else
                        {
                            temp = temp + teamList[i].TeamID.ToString() + ",";
                        }
                    }
                    myAllTeamID = "(" + temp + ")";
                }
            }
            return myAllTeamID;
        }
        #endregion

        #region 用户个人战斗力匹配战队
        public static List<TeamModel> TeamListByUserFightScore(TeamListParameterModel para)
        {
            List<TeamModel> teamList = new List<TeamModel>();
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                // 获取战队列表
                var sql = "SELECT" +
                    " t1.CreateUserID as Creater," +
                    " t1.TeamID," +
                    " t1.TeamName," +
                    " t1.TeamPicture as TeamLogo," +
                    " t1.TeamDescription," +
                    " t1.TeamType," +
                    " (CASE WHEN t1.FightScore IS NULL THEN 0 ELSE t1.FightScore END) as FightScore," +
                    " (CASE WHEN t1.Asset IS NULL THEN 0 ELSE t1.Asset END) as Asset," +
                    " t1.IsDeault," +
                    " (CASE WHEN t1.WinCount IS NULL THEN 0 ELSE t1.WinCount END) as WinCount," +
                    " (CASE WHEN t1.LoseCount IS NULL THEN 0 ELSE t1.LoseCount END) as LoseCount," +
                    " (CASE WHEN t1.FollowCount IS NULL THEN 0 ELSE t1.FollowCount END) as FollowCount," +
                    " CONVERT(varchar(100), t1.CreateTime, 23) as CreateTime" +
                    " FROM" +
                    " db_Team t1" +
                    " ORDER BY t1.CreateTime " + para.Sort;

                teamList = context.Database.SqlQuery<TeamModel>(sql)
                                  .Skip((para.StartPage - 1) * para.PageCount)
                                  .Take(para.PageCount).ToList();
            }
            return teamList;
        }
        #endregion

        #region 我的战队战斗力匹配战队
        public static List<TeamModel> TeamListByTeamFightScore(TeamListParameterModel para)
        {
            List<TeamModel> teamList = new List<TeamModel>();
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                // 根据战队战斗力获取战队列表
                // 查询条件：1.战队状态=0 2.战队满5人 3.剔除自己的战队
                var sql = "SELECT" +
                    " t1.CreateUserID as Creater," +
                    " t1.TeamID," +
                    " (SELECT count(t2.UserID) FROM db_TeamUser t2 WHERE t2.TeamID=t1.TeamID) as UserCount," +
                    " t1.TeamName," +
                    " t1.TeamPicture as TeamLogo," +
                    " t1.TeamDescription," +
                    " t1.TeamType," +
                    " (CASE WHEN t1.FightScore IS NULL THEN 0 ELSE t1.FightScore END) as FightScore," +
                    " (CASE WHEN t1.Asset IS NULL THEN 0 ELSE t1.Asset END) as Asset," +
                    " t1.IsDeault," +
                    " (CASE WHEN t1.WinCount IS NULL THEN 0 ELSE t1.WinCount END) as WinCount," +
                    " (CASE WHEN t1.LoseCount IS NULL THEN 0 ELSE t1.LoseCount END) as LoseCount," +
                    " (CASE WHEN t1.FollowCount IS NULL THEN 0 ELSE t1.FollowCount END) as FollowCount," +
                    " CONVERT(varchar(100), t1.CreateTime, 23) as CreateTime" +
                    " FROM" +
                    " db_Team t1" +
                    " WHERE t1.State=0 AND"+
                    " (SELECT count(t2.UserID) FROM db_TeamUser t2 WHERE t2.TeamID=t1.TeamID)>=4" +
                    " AND t1.CreateUserID!=" + para.createUserID+
                    " ORDER BY t1.CreateTime " + para.Sort;

                teamList = context.Database.SqlQuery<TeamModel>(sql)
                                  .Skip((para.StartPage - 1) * para.PageCount)
                                  .Take(para.PageCount).ToList();
            }
            return teamList;
        }
        #endregion

        #region 按战队注册时间获取战队列表
        public static List<TeamModel> TeamListByCreateDate(TeamListParameterModel para)
        {
            List<TeamModel> teamList = new List<TeamModel>();
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                // 获取战队列表
                var sql = "SELECT" +
                    " t1.CreateUserID as Creater," +
                    " t1.TeamID," +
                    " t1.TeamName," +
                    " t1.TeamPicture as TeamLogo," +
                    " t1.TeamDescription," +
                    " t1.TeamType," +
                    " (CASE WHEN t1.FightScore IS NULL THEN 0 ELSE t1.FightScore END) as FightScore," +
                    " (CASE WHEN t1.Asset IS NULL THEN 0 ELSE t1.Asset END) as Asset," +
                    " t1.IsDeault," +
                    " (CASE WHEN t1.WinCount IS NULL THEN 0 ELSE t1.WinCount END) as WinCount," +
                    " (CASE WHEN t1.LoseCount IS NULL THEN 0 ELSE t1.LoseCount END) as LoseCount," +
                    " (CASE WHEN t1.FollowCount IS NULL THEN 0 ELSE t1.FollowCount END) as FollowCount," +
                    " CONVERT(varchar(100), t1.CreateTime, 23) as CreateTime" +
                    " FROM" +
                    " db_Team t1" +
                    " ORDER BY t1.CreateTime " + para.Sort;

                teamList = context.Database.SqlQuery<TeamModel>(sql)
                                  .Skip((para.StartPage - 1) * para.PageCount)
                                  .Take(para.PageCount).ToList();
            }
            return teamList;
        }
        #endregion

        #region 根据战队ID获取战队发布招募内容
        public static string GetRecruitContentByTeamID(int teamID)
        {
            string RecruitContent="";
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                var Recruit = context.db_Recruit.Where(c => c.TeamID == teamID).FirstOrDefault();
                if (Recruit != null)
                {
                    RecruitContent = Recruit.Content;
                }
            }
            return RecruitContent;
        }
        #endregion

        #region 根据UserID获取用户头像
        public static string GetUserPictureByUserID(int? userID)
        {
            string userPicture = "";
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                var user = context.db_User.Where(c => c.UserID == userID).FirstOrDefault();
                if (user != null)
                {
                    userPicture = user.UserWebPicture;
                }
            }
            return userPicture;
        }
        #endregion

        #region 根据TeamID获取队员头像
        public static List<TeamUserModel> GetTeamUserByUserID(int teamID)
        {
            List<db_TeamUser> teamUser;
            List<TeamUserModel> teamUserList=new List<TeamUserModel>();
            
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                teamUser = context.db_TeamUser.Where(c => c.TeamID == teamID).ToList();
                if (teamUser != null)
                {
                    for(int i=0;i< teamUser.Count; i++)
                    {
                        TeamUserModel teamUserModel=new TeamUserModel();
                        var user = User.GetUserModelByUserID(Convert.ToInt32(teamUser[i].UserID));
                        teamUserModel.UserPicture = user.UserWebPicture;//人像
                        teamUserModel.UserID = user.UserID;//性别
                        teamUserModel.PhoneNumber = user.PhoneNumber;
                        teamUserModel.UserNickName = user.UserWebNickName;
                        teamUserModel.Sex = user.Sex;
                        teamUserModel.RegDate = user.RegDate;
                        teamUserModel.Address = user.Address;
                        teamUserModel.Birthday = user.Birthday;
                        teamUserModel.Hobby = user.Hobby;
                        teamUserModel.Asset = User.GetAssetByUserID(user.UserID);
                        teamUserModel.FightScore=User.GetGamePowerByUserID(user.UserID);
                        teamUserModel.HeroImage = User.GetHeroImgeByUserID(user.UserID);
                        teamUserList.Add(teamUserModel);
                    }
                }
            }
            return teamUserList;
        }
        #endregion

        #region 根据TeamID获取队员个数
        public static int GetTeamUserCountByTeamID(int teamID)
        {
            int teamUserCount;
            List<TeamUserModel> teamUserList = new List<TeamUserModel>();

            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                teamUserCount = context.db_TeamUser.Where(c => c.TeamID == teamID).ToList().Count;
            }
            return teamUserCount;
        }
        #endregion

        #region 根据主播ID判断报名人数是否已满
        public static bool IsBoBoFull(MatchParameter2Model match)
        {
            bool isOK = true;
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                var boboCount = context.db_GameRecord.Where(c => c.BoBoID == match.BoBoID)
                                                         .Where(c => c.GameID == match.MatchID)
                                                         .ToList().Count;
                var bobo = context.db_GameBoBo.Where(c => c.BoBoID == match.BoBoID)
                                              .Where(c => c.GameID == match.MatchID)
                                              .FirstOrDefault();
                if (boboCount>= bobo.Count)
                {
                    isOK = false;
                }
            }
            return isOK;
        }
        #endregion

        #region 根据TeamID获取战队战斗力
        public static int GetFightScoreByTeamID(int teamID)
        {
            int fightScore = 0;
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {

                var team = context.View_TeamUserGamePower.Where(c => c.TeamID == teamID)
                                                         .OrderByDescending(c => c.GamePower)
                                                         .Take(5).ToList();
                for (int i=0;i< team.Count;i++)
                {
                    fightScore = fightScore +Convert.ToInt32(team[i].GamePower);
                }
            }
            return fightScore;
        }
        #endregion
    }
}
