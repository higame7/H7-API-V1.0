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
    public class Asset
    {
        #region 获取我的资产排名
        public static int MyRank(int myAsset, DateTime regDate)
        {
            int totalCount = 0;
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                //
                var sql = "select t1.UserID,t2.RegisterDate," +
                          "  sum(t1.VirtualMoney) as totalAsset" +
                          "  from db_AssetRecord t1" +
                          "  left join db_User t2 on t1.UserID = t2.UserID" +
                          "  group by t1.UserID,t2.RegisterDate" +
                          "  order by totalAsset desc";

                totalCount = context.Database.SqlQuery<TotalAssetModel>(sql)
                                 .Where(c => c.TotalAsset >= myAsset)
                                 
                                 .ToList().Count();
            }
            return totalCount;
        }
        #endregion

        #region AddMoneyRegister 注册时用户默认获得虚拟币
        /// <summary>
        /// 注册时用户默认获得虚拟币
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns>true|false</returns>
        public static bool AddMoneyRegister(int userID)
        {
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                db_AssetRecord assetRecord = new db_AssetRecord();

                assetRecord.UserID = userID;
                assetRecord.VirtualMoney = ASSET.MONEY_REG;
                assetRecord.TrueMoney = 0;
                assetRecord.GainWay = ASSET.GAINWAY_REG;
                assetRecord.GainTime = DateTime.Now;
                assetRecord.State = ASSET.MONEYSTATE_YES;
                //时间+操作+收入支出金额
                assetRecord.Remark = assetRecord.GainTime + " " +
                                    assetRecord.GainWay + " "
                                    + ASSET.PAY_IN +
                                    assetRecord.VirtualMoney.ToString();

                //将充值记录加入资产记录表
                context.db_AssetRecord.Add(assetRecord);
                context.SaveChanges();
                return true;
            }
            
        }
        #endregion

        #region 判断氦气是否充足
        public static bool IsEnoughMoney(int userID,int Money)
        {
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                int asset = Convert.ToInt32(context.db_AssetRecord.Where(c => c.UserID == userID).Sum(c => c.VirtualMoney));
                //氦气不足
                if (asset < Money)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion
    }
}
