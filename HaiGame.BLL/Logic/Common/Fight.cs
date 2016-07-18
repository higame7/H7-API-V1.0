using HaiGame7.BLL.Enum;
using HaiGame7.Model.EFModel;
using HaiGame7.Model.MyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaiGame7.BLL.Logic.Common
{
    public class Fight
    {
        #region 拼接约战动态
        public static string FightState(FightStateDetailModel fightStateDetail)
        {
            string fightState = "";
            switch (fightStateDetail.CurrentState)
            {
                case "发起挑战":
                    fightState = "战队【" + fightStateDetail.STeamName + "】向战队【" + fightStateDetail.ETeamName + "】发出约战";
                    break;
                case "已应战":
                    fightState = "战队【" + fightStateDetail.ETeamName + "】接受战队【" + fightStateDetail.STeamName + "】的约战";
                    break;
                case "已认怂":
                    fightState = "战队【" + fightStateDetail.ETeamName + "】向战队【" + fightStateDetail.STeamName + "】认怂";
                    break;
                case "挑战成功":
                    fightState = "战队【" + fightStateDetail.STeamName + "】已战胜【" + fightStateDetail.ETeamName + "】，赢取"+ fightStateDetail.Money+"氦气";
                    break;
                case "守擂成功":
                    fightState = "战队【" + fightStateDetail.ETeamName + "】已战胜【" + fightStateDetail.STeamName + "】，赢取" + fightStateDetail.Money + "氦气";
                    break;

            }
            
            return fightState;
        }
        #endregion

        #region 判断角色，队员不能发起约战
        public static bool IsTeamCreater(int userID,int teamID)
        {
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                var team=context.db_Team.
                                Where(c => c.CreateUserID == userID).
                                Where(c => c.TeamID == teamID).
                                FirstOrDefault();
                if (team==null)
                {
                    //队员
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region 判断是否超出每日限额
        public static bool IsDailyLimit(int userID, int teamID)
        {
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                var sql = "SELECT" +
                          "  t1.DateID" +
                          "  FROM" +
                          "  db_DateFight t1" +
                          "  LEFT JOIN db_FightState t2" +
                          "  ON t1.DateID = t2.DateID" +
                          "  WHERE t1.STeamID = "+ teamID+" AND" +
                          "  CONVERT(varchar(100), t2.StateTime, 23) = '"+DateTime.Now.ToString("yyyy-MM-dd") +"'"+
                          "  AND t2.State = '发起挑战'";
                var dailyCount = context.Database.SqlQuery<TeamModel>(sql)
                                  .ToList().Count;
                if (dailyCount > 0)
                {
                    //队员
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region 判断是否与对方有未完成的约战
        public static bool IsFinished(int userID, int teamID)
        {
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                var sql = "SELECT" +
                          "  t1.DateID" +
                          "  FROM" +
                          "  db_DateFight t1" +
                          "  LEFT JOIN db_FightState t2" +
                          "  ON t1.DateID = t2.DateID" +
                          "  WHERE t1.STeamID = " + teamID + " AND" +
                          "  CONVERT(varchar(100), t2.StateTime, 20) = " + DateTime.Now.ToString("yyyy-MM-dd") +
                          "  AND t2.State = '发起挑战'";
                var finishCount = context.Database.SqlQuery<TeamModel>(sql)
                                  .ToList().Count;
                if (finishCount > 0)
                {
                    //队员
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region 发起约战前提条件判断
        public static MessageModel IsChallenge(int userID, int teamID,int Money)
        {
            MessageModel message = new MessageModel();
            //判断角色，队员不能发起约战
            if (IsTeamCreater(userID, teamID) == false)
            {
                message.Message = MESSAGE.USERCHALLENGE;
                message.MessageCode = MESSAGE.USERCHALLENGE_CODE;
                return message;
            }

            //判断是否超出每日限额
            if (IsDailyLimit(userID, teamID) == false)
            {
                message.Message = MESSAGE.DAILYCOUNT;
                message.MessageCode = MESSAGE.DAILYCOUNT_CODE;
                return message;
            }
            //判断氦气是否充足
            if (Asset.IsEnoughMoney(userID, Money) == false)
            {
                message.Message = MESSAGE.NOMONEY;
                message.MessageCode = MESSAGE.NOMONEY_CODE;
                return message;
            }

            //判断是否与对方有未完成的约战

            message.Message= MESSAGE.OK;
            message.MessageCode = MESSAGE.OK_CODE;
            return message;
        }
        #endregion

        #region 根据约战ID更改Team表胜负场
        public static void UpdateTeamByDateID(FightParameter2Model fight, HiGame_V1Entities context)
        {
            db_DateFight dateFight = context.db_DateFight.Where(c => c.DateID == fight.DateID).FirstOrDefault();
            db_Team sTeam = context.db_Team.Where(c => c.TeamID == dateFight.STeamID).FirstOrDefault();
            db_Team eTeam = context.db_Team.Where(c => c.TeamID == dateFight.ETeamID).FirstOrDefault();
            //挑战方胜场加1
            if(sTeam.WinCount== null)
            {
                sTeam.WinCount = 1;
            }
            else
            {
                sTeam.WinCount = sTeam.WinCount + 1;
            }
            //认怂方认怂数加1
            if (eTeam.FollowCount == null)
            {
                eTeam.FollowCount = 1;
            }
            else
            {
                eTeam.FollowCount = eTeam.FollowCount + 1;
            }
            //归还挑战方押金
            db_AssetRecord assetRecord = new db_AssetRecord();
            assetRecord.UserID = sTeam.CreateUserID;
            assetRecord.VirtualMoney = fight.Money;
            assetRecord.TrueMoney = 0;
            assetRecord.GainWay = ASSET.GAINWAY_BACK;
            assetRecord.GainTime = DateTime.Now;
            assetRecord.State = ASSET.MONEYSTATE_YES;
            assetRecord.Remark = assetRecord.GainTime + " " +
                                assetRecord.GainWay + " "
                                + ASSET.PAY_IN +
                                assetRecord.VirtualMoney.ToString();
            context.db_AssetRecord.Add(assetRecord);
        }
        #endregion
    }
}
