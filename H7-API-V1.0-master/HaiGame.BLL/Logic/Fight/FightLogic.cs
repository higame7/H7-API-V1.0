using HaiGame7.BLL.Enum;
using HaiGame7.BLL.Logic.Common;
using HaiGame7.Model.EFModel;
using HaiGame7.Model.MyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace HaiGame7.BLL
{
    public class FightLogic
    {
        #region 约战动态列表
        public string AllFightList(RankParameterModel rank)
        {
            string result = "";
            MessageModel message = new MessageModel();
            List<FightStateModel> fightStateList = new List<FightStateModel>();
            
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();
            //获取约战平台约战记录的当前状态
            //个人排行：昵称，签名，氦气，战斗力，大神系数
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                //联合查询
                var sql = "SELECT" +
                           " t1.DateID,t1.STeamID,t1.ETeamID,t1.Money,t1.FightAddress,t1.CurrentState," +
                           " t2.StateTime,t3.TeamName as STeamName,t4.TeamName as ETeamName" +
                           " FROM" +
                           " db_DateFight t1 join db_FightState t2" +
                           " ON t1.DateID = t2.DateID and t1.CurrentState = t2.State" +
                           " LEFT JOIN db_Team t3" +
                           " ON t1.STeamID = t3.TeamID" +
                           " LEFT JOIN db_Team t4" +
                           " ON t1.ETeamID = t4.TeamID ORDER BY t2.StateTime DESC";

                var fightStateDetailList = context.Database.SqlQuery<FightStateDetailModel>(sql)
                                 .Skip((rank.StartPage - 1) * rank.PageCount)
                                 .Take(rank.PageCount).ToList();

                if (fightStateDetailList == null)
                {
                    //无游戏数据
                    message.Message = MESSAGE.NOGAMEDATA;
                    message.MessageCode = MESSAGE.NOGAMEDATA_CODE;
                }
                else
                {
                    foreach (FightStateDetailModel fight in fightStateDetailList)
                    {
                        //拼接返回字段信息
                        FightStateModel fightState = new FightStateModel();
                        fightState.FightAsset = fight.Money;
                        fightState.FightTime = Common.DateDiff(fight.StateTime, DateTime.Now);
                        fightState.Description = Fight.FightState(fight);
                        fightStateList.Add(fightState);
                    }
                    message.Message = MESSAGE.OK;
                    message.MessageCode = MESSAGE.OK_CODE;
                }
                returnResult.Add(message);
                returnResult.Add(fightStateList);
            }
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 发起约战
        public string MakeChallenge(ChallengeParameterModel para)
        {
            string result = "";
            MessageModel message = new MessageModel();
            List<FightStateModel> fightStateList = new List<FightStateModel>();
            HashSet<object> returnResult = new HashSet<object>();
            JavaScriptSerializer jss = new JavaScriptSerializer();

            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                //发起约战前提条件判断
                message = Fight.IsChallenge(para.UserID,para.STeamID, para.Money);
                if (message.MessageCode==0)
                {
                    //向约战记录表插入一条数据
                    db_DateFight dateFight = new db_DateFight();
                    dateFight.CurrentState = "发起挑战";
                    dateFight.STeamID = para.STeamID;
                    dateFight.ETeamID = para.ETEamID;
                    dateFight.Money = para.Money;
                    dateFight.FightTime = para.FightTime;
                    context.db_DateFight.Add(dateFight);
                    
                    //资产表插入一条数据
                    db_AssetRecord assetRecord = new db_AssetRecord();
                    assetRecord.UserID = para.UserID;
                    assetRecord.VirtualMoney = -para.Money;
                    assetRecord.TrueMoney = 0;
                    assetRecord.GainWay = ASSET.GAINWAY_CHALLENGE;
                    assetRecord.GainTime = DateTime.Now;
                    assetRecord.State = ASSET.MONEYSTATE_YES;
                    //时间+操作+收入支出金额
                    assetRecord.Remark = assetRecord.GainTime + " " +
                                        assetRecord.GainWay + " "
                                        + ASSET.PAY_OUT +
                                        assetRecord.VirtualMoney.ToString();
                    context.db_AssetRecord.Add(assetRecord);

                    //向信息表插入一条数据
                    context.SaveChanges();

                    //向约战状态表插入一条数据
                    var fight=context.db_DateFight.Where(c => c.STeamID == para.STeamID)
                                        .Where(c => c.ETeamID == para.ETEamID)
                                        .OrderByDescending(c => c.FightTime).FirstOrDefault();
                                        
                    db_FightState fightState = new db_FightState();
                    fightState.DateID = fight.DateID;
                    fightState.State = "发起挑战";
                    fightState.StateTime = DateTime.Now;
                    
                    context.db_FightState.Add(fightState);
                    context.SaveChanges();

                }
            }
                
            returnResult.Add(message);
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 我发出的约战
        public string MyFight(FightParameterModel fight)
        {
            if (fight.FightType.ToLower() == "send")
            {
                return MySendFight(fight);
            }
            else
            {
                return MyReceiveFight(fight);
            }
        }
        #endregion

        #region 我发出的约战
        public static string MySendFight(FightParameterModel fight)
        {
            string result = "";
            MessageModel message = new MessageModel();
            List<FightStateModel> fightStateList = new List<FightStateModel>();
            HashSet<object> returnResult = new HashSet<object>();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            List<FightStateDetailModel> fightSendList;

            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                string teamID = "()";
                db_User userInfo = context.db_User.Where(c => c.PhoneNumber == fight.PhoneNumber).FirstOrDefault();
                if (userInfo != null)
                {
                    teamID=Team.MyAllTeamID(userInfo.UserID);
                }
                var sql = "SELECT" +
                          " t1.DateID,t1.STeamID,t1.ETeamID,t1.Money,t1.FightAddress as SFightAddress,t1.FightAddress1 as EFightAddress,t1.CurrentState," +
                          " CONVERT(varchar(100), t1.FightTime, 20) as FightTime," +
                          " t5.PhoneNumber," +
                          " CONVERT(varchar(100), t2.StateTime, 20) as StateTimeStr," +
                          "　t3.TeamName as STeamName,t4.TeamName as ETeamName," +
                          "　t1.SFightPic as SFightPic,t1.EFightPic as EFightPic" +
                          " FROM" +
                          " db_DateFight t1 join db_FightState t2" +
                          " ON t1.DateID = t2.DateID and t1.CurrentState = t2.State" +
                          " LEFT JOIN db_Team t3" +
                          " ON t1.STeamID = t3.TeamID" +
                          " LEFT JOIN db_Team t4" +
                          " ON t1.ETeamID = t4.TeamID" +
                          " LEFT JOIN db_User t5" +
                          " ON t4.CreateUserID = t5.UserID" +
                          " WHERE t3.State=0 AND t4.State=0 AND t1.STeamID IN" + teamID +
                          " ORDER BY t2.StateTime DESC";

                fightSendList = context.Database.SqlQuery<FightStateDetailModel>(sql)
                                 .Skip((fight.StartPage - 1) * fight.PageCount)
                                 .Take(fight.PageCount).ToList();

                message.Message = MESSAGE.OK;
                message.MessageCode = MESSAGE.OK_CODE;
            }

            returnResult.Add(message);
            returnResult.Add(fightSendList);
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 我收到的约战
        public static string MyReceiveFight(FightParameterModel fight)
        {
            string result = "";
            MessageModel message = new MessageModel();
            List<FightStateModel> fightStateList = new List<FightStateModel>();
            HashSet<object> returnResult = new HashSet<object>();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            List<FightStateDetailModel> fightReceiveList;

            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                string teamID = "()";
                db_User userInfo = context.db_User.Where(c => c.PhoneNumber == fight.PhoneNumber).FirstOrDefault();
                if (userInfo != null)
                {
                    teamID=Team.MyAllTeamID(userInfo.UserID);
                }
                var sql = "SELECT" +
                          " t1.DateID,t1.STeamID,t1.ETeamID,t1.Money,t1.FightAddress as SFightAddress,t1.FightAddress1 as EFightAddress,t1.CurrentState," +
                          " CONVERT(varchar(100), t1.FightTime, 20) as FightTime," +
                          " t5.PhoneNumber," +
                          " CONVERT(varchar(100), t2.StateTime, 20) as StateTimeStr," +
                          " t3.TeamName as STeamName,t4.TeamName as ETeamName," +
                          "　t1.SFightPic as SFightPic,t1.EFightPic as EFightPic" +
                          " FROM" +
                          " db_DateFight t1 join db_FightState t2" +
                          " ON t1.DateID = t2.DateID and t1.CurrentState = t2.State" +
                          " LEFT JOIN db_Team t3" +
                          " ON t1.STeamID = t3.TeamID" +
                          " LEFT JOIN db_Team t4" +
                          " ON t1.ETeamID = t4.TeamID" +
                          " LEFT JOIN db_User t5" +
                          " ON t3.CreateUserID = t5.UserID" +
                          " WHERE t3.State=0 AND t4.State=0 AND t1.ETeamID IN" + teamID +
                          " ORDER BY t2.StateTime DESC";

                fightReceiveList = context.Database.SqlQuery<FightStateDetailModel>(sql)
                                 .Skip((fight.StartPage - 1) * fight.PageCount)
                                 .Take(fight.PageCount).ToList();

                message.Message = MESSAGE.OK;
                message.MessageCode = MESSAGE.OK_CODE;
            }

            returnResult.Add(message);
            returnResult.Add(fightReceiveList);
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 认怂
        public string Reject(FightParameter2Model fight)
        {
            string result = "";
            MessageModel message = new MessageModel();
            List<FightStateModel> fightStateList = new List<FightStateModel>();
            HashSet<object> returnResult = new HashSet<object>();
            JavaScriptSerializer jss = new JavaScriptSerializer();

            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                //1.dategight表更改当前状态
                var fightRecord=context.db_DateFight.Where(c => c.DateID == fight.DateID).FirstOrDefault();
                fightRecord.CurrentState = "已认怂";
                //2.fightstate表新增状态
                db_FightState fightState = new db_FightState();
                fightState.DateID = fight.DateID;
                fightState.State = "已认怂";
                fightState.StateTime = DateTime.Now;
                context.db_FightState.Add(fightState);

                //3.扣取一认怂金，资产表插入一条数据
                db_AssetRecord assetRecord = new db_AssetRecord();
                assetRecord.UserID = fight.UserID;
                assetRecord.VirtualMoney = -1;
                assetRecord.TrueMoney = 0;
                assetRecord.GainWay = ASSET.GAINWAY_REJECT;
                assetRecord.GainTime = DateTime.Now;
                assetRecord.State = ASSET.MONEYSTATE_YES;
                assetRecord.Remark = assetRecord.GainTime + " " +
                                    assetRecord.GainWay + " "
                                    + ASSET.PAY_OUT +
                                    assetRecord.VirtualMoney.ToString();
                context.db_AssetRecord.Add(assetRecord);

                //4.本方Team表认怂数+1,挑战方Team表胜利数+1，归还扣押挑战金
                Fight.UpdateTeamByDateID(fight, context);

                //5.认怂表新增数据
                db_Follow follow = new db_Follow();
                follow.DateID = fight.DateID;
                follow.FollowMoney = 1;
                follow.FollowTime = DateTime.Now;
                context.db_Follow.Add(follow);
                context.SaveChanges();

                message.Message = MESSAGE.OK;
                message.MessageCode = MESSAGE.OK_CODE;
            }

            returnResult.Add(message);
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 应战
        public string Accept(FightParameter2Model fight)
        {
            string result = "";
            MessageModel message = new MessageModel();
            List<FightStateModel> fightStateList = new List<FightStateModel>();
            HashSet<object> returnResult = new HashSet<object>();
            JavaScriptSerializer jss = new JavaScriptSerializer();

            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                //判断氦气是否充足
                bool isEnoughMoney = Asset.IsEnoughMoney(fight.UserID, fight.Money);
                //氦气不足
                if (isEnoughMoney == false)
                {
                    message.Message = MESSAGE.NOMONEY;
                    message.MessageCode = MESSAGE.NOMONEY_CODE;
                }
                else
                {
                    //dategight表更改当前状态
                    var fightRecord = context.db_DateFight.Where(c => c.DateID == fight.DateID).FirstOrDefault();
                    fightRecord.CurrentState = "已应战";
                    //fightstate表新增状态
                    db_FightState fightState = new db_FightState();
                    fightState.DateID = fight.DateID;
                    fightState.State = "已应战";
                    fightState.StateTime = DateTime.Now;
                    context.db_FightState.Add(fightState);

                    //扣除约战氦气，资产表插入一条数据
                    db_AssetRecord assetRecord = new db_AssetRecord();
                    assetRecord.UserID = fight.UserID;
                    assetRecord.VirtualMoney = -fight.Money;
                    assetRecord.TrueMoney = 0;
                    assetRecord.GainWay = ASSET.GAINWAY_ACCEPT;
                    assetRecord.GainTime = DateTime.Now;
                    assetRecord.State = ASSET.MONEYSTATE_YES;
                    //时间+操作+收入支出金额
                    assetRecord.Remark = assetRecord.GainTime + " " +
                                        assetRecord.GainWay + " "
                                        + ASSET.PAY_OUT +
                                        assetRecord.VirtualMoney.ToString();
                    context.db_AssetRecord.Add(assetRecord);

                    context.SaveChanges();

                    message.Message = MESSAGE.OK;
                    message.MessageCode = MESSAGE.OK_CODE;
                }  
            }

            returnResult.Add(message);
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 上传比赛ID
        public string UpdateGameID(FightParameter2Model fight)
        {
            string result = "";
            MessageModel message = new MessageModel();
            List<FightStateModel> fightStateList = new List<FightStateModel>();
            HashSet<object> returnResult = new HashSet<object>();
            JavaScriptSerializer jss = new JavaScriptSerializer();

            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                //datefight表更改当前状态
                db_DateFight fightRecord = new db_DateFight();
                fightRecord = context.db_DateFight.Where(c => c.DateID == fight.DateID).FirstOrDefault();
                if (fight.SFightAddress==null)
                {
                    fightRecord.FightAddress1 = fight.EFightAddress;
                    fightRecord.EFightPic = Common.Base64ToFightImage(fight.EFightPic,fight.DateID.ToString());
                }
                else
                {
                    fightRecord.FightAddress = fight.SFightAddress;
                    fightRecord.SFightPic = Common.Base64ToFightImage(fight.SFightPic, fight.DateID.ToString());
                }
                context.SaveChanges();

                message.Message = MESSAGE.OK;
                message.MessageCode = MESSAGE.OK_CODE;
            }

            returnResult.Add(message);
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion
    }
}
