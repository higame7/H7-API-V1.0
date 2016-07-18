using HaiGame7.Model.EFModel;
using HaiGame7.Model.MyModel;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Linq;
using HaiGame7.BLL.Enum;
using System;
using HaiGame7.BLL.Logic.Common;
using System.Web.Configuration;

namespace HaiGame7.BLL
{
    public class MatchLogic
    {
        #region 赛事列表
        public string MatchList()
        {
            string result = "";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();
            List<MatchModel> matchList = new List<MatchModel>();
            //赛事列表信息
            using (HiGame_V1Entities context=new HiGame_V1Entities())
            {
                var sql = "SELECT" +
                          " t1.GameID as MatchID," +
                          " t1.GameName as MatchName," +
                          " t1.Introduce as Introduce," +
                          " t1.ShowPicture" +
                          " FROM db_Game t1"+
                          " WHERE t1.State =0";

                matchList = context.Database.SqlQuery<MatchModel>(sql)
                                 .ToList();
            }
            message.MessageCode = MESSAGE.OK_CODE;
            message.Message = MESSAGE.OK;
            returnResult.Add(message);
            returnResult.Add(matchList);
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 主播列表
        public string BoBoList(MatchModel match)
        {
            string result = "";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();
            List<BoBoModel> boboList = new List<BoBoModel>();
            
            //主播列表
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                var sql = "SELECT" +
                          " t1.GameID as MatchID," +
                          " t3.GameID as GameID," +
                          " t3.BoBoID as BoBoID," +
                          " t2.TalkShow as TalkShow," +
                          " t2.Count," +
                          " t2.TalkShow," +
                          " t3.Name," +
                          " t3.UserPicture," +
                          " t3.Sex," +
                          " t3.Introduce" +
                          " FROM db_Game t1" +
                          " JOIN db_GameBoBo t2 ON t1.GameID=t2.GameID" +
                          " JOIN db_BoBo t3 ON t2.BoBoID=t3.BoBoID" +
                          " WHERE t1.State =0 AND t1.GameID="+ match.MatchID;

                boboList = context.Database.SqlQuery<BoBoModel>(sql)
                                 .ToList();
            }
            message.MessageCode = MESSAGE.OK_CODE;
            message.Message = MESSAGE.OK;
            returnResult.Add(message);
            returnResult.Add(boboList);
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 主播已报名名额
        public string BoBoCount(MatchParameterModel match)
        {
            string result = "";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();
            BoBoCountModel joinCount = new BoBoCountModel();

            //主播列表
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                var sql = "SELECT" +
                          " Count(t1.GameRecordID) as JoinCount" +
                          " FROM db_GameRecord t1" +
                          " LEFT JOIN db_Team t2 ON t1.TeamID=t2.TeamID" +
                          " WHERE t2.State=0 AND t1.GameID =" + match.MatchID+ " AND t1.BoBoID=" + match.BoBoID;

                joinCount = context.Database.SqlQuery<BoBoCountModel>(sql)
                                 .FirstOrDefault();
            }
            message.MessageCode = MESSAGE.OK_CODE;
            message.Message = MESSAGE.OK;
            returnResult.Add(message);
            returnResult.Add(joinCount);
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 报名参赛
        public string JoinMatch(MatchParameter2Model match)
        {
            string result = "";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();

            //报名参赛
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                db_User user = User.GetUserByPhoneNumber(match.PhoneNumber);
                if(user!=null)
                {
                    //判断是否队长，队员不可报名
                    var team=context.db_Team.Where(c => c.TeamID == match.TeamID).
                                    Where(c => c.CreateUserID == user.UserID).
                                    FirstOrDefault();
                    if (team==null)
                    {
                        message.MessageCode = MESSAGE.CANNOTJOINMATCH_CODE;
                        message.Message = MESSAGE.CANNOTJOINMATCH;
                    }
                    else
                    {
                        //判断队伍是否满5人
                        var teamCount = context.db_TeamUser.Where(c => c.TeamID == match.TeamID).ToList().Count;
                        if (teamCount<4)
                        {
                            message.MessageCode = MESSAGE.TEAMUSERNOTENOUGH_CODE;
                            message.Message = MESSAGE.TEAMUSERNOTENOUGH;
                        }
                        else
                        {
                            //判断报名队伍数是否已满
                            if (Team.IsBoBoFull(match) == true)
                            {
                                //判断队伍是否已报名
                                db_GameRecord gameRecord = new db_GameRecord();
                                gameRecord.BoBoID = match.BoBoID;
                                gameRecord.GameID = match.MatchID;
                                gameRecord.TeamID = match.TeamID;
                                gameRecord.ApplyTIme = DateTime.Now;
                                gameRecord.UserID = user.UserID;
                                gameRecord.IsCancel = 0;
                                gameRecord.State = 0;
                                context.db_GameRecord.Add(gameRecord);
                                context.SaveChanges();
                                message.MessageCode = MESSAGE.OK_CODE;
                                message.Message = MESSAGE.OK;
                            }
                            else
                            {
                                //名额已满
                                message.MessageCode = MESSAGE.MATCHFULL_CODE;
                                message.Message = MESSAGE.MATCHFULL;
                            }
                        }
                    }
                }
                else
                {
                    //用户未存在
                    message.MessageCode = MESSAGE.NOUSER_CODE;
                    message.Message = MESSAGE.NOUSER;
                }
                
            }
            returnResult.Add(message);
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 取消报名参赛
        public string QuitMatch(MatchParameter2Model match)
        {
            string result = "";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();

            //取消报名参赛
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                db_User user = User.GetUserByPhoneNumber(match.PhoneNumber);
                //判断是否队长，队员不可报名
                var team = context.db_Team.Where(c => c.TeamID == match.TeamID).
                                Where(c => c.CreateUserID == user.UserID).
                                FirstOrDefault();
                if (team == null)
                {
                    message.MessageCode = MESSAGE.CANNOTQUITMATCH_CODE;
                    message.Message = MESSAGE.CANNOTQUITMATCH;
                }
                else
                {
                    db_GameRecord gameRecord = context.db_GameRecord.
                                            Where(c => c.GameID == match.MatchID).
                                            Where(c => c.TeamID == match.TeamID).FirstOrDefault();

                    //删除队伍已报名信息
                    if (gameRecord != null)
                    {
                        context.db_GameRecord.Remove(gameRecord);
                        context.SaveChanges();
                    }
                    message.MessageCode = MESSAGE.OK_CODE;
                    message.Message = MESSAGE.OK;
                }
            }
            returnResult.Add(message);
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 我的已报名信息
        public string MyJoinMatch(MatchParameter2Model match)
        {
            string result = "";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();
            MyBoBoModel myBobo = new MyBoBoModel();

            message.MessageCode = MESSAGE.NOJOINMATCH_CODE;
            message.Message = MESSAGE.NOJOINMATCH;
            //报名参赛
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                db_User user = User.GetUserByPhoneNumber(match.PhoneNumber);
                if (user != null)
                {
                    var matchRecord=context.db_GameRecord
                        .Where(c => c.GameID == match.MatchID)
                        .Where(c => c.TeamID == match.TeamID)
                        .FirstOrDefault();
                    if (matchRecord != null)
                    {
                        var bobo=context.db_BoBo
                        .Where(c => c.BoBoID == matchRecord.BoBoID)
                        .FirstOrDefault();
                        if (bobo!=null)
                        {
                            myBobo.Name = bobo.Name;
                            myBobo.ApplyTime = ((DateTime)matchRecord.ApplyTIme).ToString("yyyy-MM-dd hh:mm:ss");
                            message.MessageCode = MESSAGE.OK_CODE;
                            message.Message = MESSAGE.OK;
                        }
                    }
                }
            }

            returnResult.Add(message);
            returnResult.Add(myBobo);
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 我的参赛列表
        public string MyMatchList(MatchParameter3Model match)
        {
            string result = "";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();
            List<MyMatchModel> myMatchList = new List<MyMatchModel>();

            //主播列表
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                string teamID=Team.MyAllTeamID(match.UserID);
                string sql;
                if (match.State==0)
                {
                    sql = "SELECT" +
                          " t1.ResultID as MatchID," +
                          " t1.GameStage as MatchName," +
                          " t1.HomeTeamID as STeamID," +
                          " t2.TeamName as STeamName," +
                          " t2.TeamPicture as STeamLogo," +
                          " t1.CustomerTeamID as ETeamID," +
                          " t3.TeamName as ETeamName," +
                          " t3.TeamPicture as ETeamLogo," +
                          "  CONVERT(varchar(100), t1.EndTime, 20) as EndTime," +
                          " t1.Result " +
                          " FROM db_FightResult t1" +
                          " LEFT JOIN db_Team t2 ON t1.HomeTeamID=t2.TeamID" +
                          " LEFT JOIN db_Team t3 ON t1.CustomerTeamID=t3.TeamID" +
                          " WHERE (t1.HomeTeamID IN " + teamID +
                          " OR t1.CustomerTeamID IN " + teamID + ")"+
                          " AND t1.Result='未开赛' " +
                          " ORDER BY t1.EndTime DESC";
                }
                else
                {
                    sql = "SELECT" +
                          " t1.ResultID as MatchID," +
                          " t1.GameStage as MatchName," +
                          " t1.HomeTeamID as STeamID," +
                          " t2.TeamName as STeamName," +
                          " t2.TeamPicture as STeamLogo," +
                          " t1.CustomerTeamID as ETeamID," +
                          " t3.TeamName as ETeamName," +
                          " t3.TeamPicture as ETeamLogo," +
                          "  CONVERT(varchar(100), t1.EndTime, 20) as EndTime," +
                          " t1.Result " +
                          " FROM db_FightResult t1" +
                          " LEFT JOIN db_Team t2 ON t1.HomeTeamID=t2.TeamID" +
                          " LEFT JOIN db_Team t3 ON t1.CustomerTeamID=t3.TeamID" +
                          " WHERE (t1.HomeTeamID IN " + teamID +
                          " OR t1.CustomerTeamID IN " + teamID + ")" +
                          " AND t1.Result<>'未开赛' " +
                          " ORDER BY t1.EndTime DESC";
                }
                
                myMatchList = context.Database.SqlQuery<MyMatchModel>(sql)
                                 .Skip((match.StartPage - 1) * match.PageCount)
                                 .Take(match.PageCount).ToList();
            }
            message.MessageCode = MESSAGE.OK_CODE;
            message.Message = MESSAGE.OK;
            returnResult.Add(message);
            returnResult.Add(myMatchList);
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 主播赛程日期列表
        public string MatchDateList(MatchParameterModel para)
        {
            string result = "";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();
            List<MatchDateModel> dateList = new List<MatchDateModel>();

            //主播赛程列表
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                var sql = "SELECT" +
                        " CONVERT(varchar(100), t1.StartTime, 23) as StartTime" +
                        " FROM db_FightResult t1" +
                        " WHERE t1.BoBoID= " + para.BoBoID +
                        " AND t1.GameID=" + para.MatchID +
                        " GROUP BY CONVERT(varchar(100), t1.StartTime, 23)"+
                        " ORDER BY CONVERT(varchar(100), t1.StartTime, 23)";

                dateList = context.Database.SqlQuery<MatchDateModel>(sql)
                                    .ToList();

            }
            message.MessageCode = MESSAGE.OK_CODE;
            message.Message = MESSAGE.OK;
            returnResult.Add(message);
            returnResult.Add(dateList);
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 主播赛程列表,通过主播ID、赛事ID、赛程日期获取赛事列表
        public string BoBoMatchList(MatchParameterModel para)
        {
            string result = "";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();
            List<MyMatchModel> boboMatchList = new List<MyMatchModel>();

            //主播赛程列表2
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {

                var sql2 = "SELECT" +
                        " t1.ResultID as MatchID," +
                        " t1.GameStage as MatchName," +
                        " t1.HomeTeamID as STeamID," +
                        " t2.TeamName as STeamName," +
                        " t2.TeamPicture as STeamLogo," +
                        " t1.CustomerTeamID as ETeamID," +
                        " t3.TeamName as ETeamName," +
                        " t3.TeamPicture as ETeamLogo," +
                        "  CONVERT(varchar(100), t1.EndTime, 20) as EndTime," +
                        " t1.Result " +
                        " FROM db_FightResult t1" +
                        " LEFT JOIN db_Team t2 ON t1.HomeTeamID=t2.TeamID" +
                        " LEFT JOIN db_Team t3 ON t1.CustomerTeamID=t3.TeamID" +
                        " WHERE t1.BoBoID= " + para.BoBoID +
                        " AND t1.GameID=" + para.MatchID +
                        " AND CONVERT(varchar(100), t1.EndTime, 23)=" + "'" + para.MatchTime + "'"+
                        " ORDER BY t1.EndTime DESC";

                boboMatchList = context.Database.SqlQuery<MyMatchModel>(sql2)
                                .ToList();
            }
            message.MessageCode = MESSAGE.OK_CODE;
            message.Message = MESSAGE.OK;
            returnResult.Add(message);
            returnResult.Add(boboMatchList);
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 赛事状态
        public string MatchState(MatchParameterModel para)
        {
            string result = "";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();


            //主播赛程列表
            MatchStateModel matchState=new MatchStateModel();
            matchState.MatchState = Convert.ToInt32(WebConfigurationManager.AppSettings["matchstate"]);

            message.MessageCode = MESSAGE.OK_CODE;
            message.Message = MESSAGE.OK;
            returnResult.Add(message);
            returnResult.Add(matchState);
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion
    }
}
