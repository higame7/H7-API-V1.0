using HaiGame7.Model.EFModel;
using HaiGame7.Model.MyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaiGame7.BLL.Logic.Common
{
    public class User
    {
        #region 通过手机号获取用户信息
        public static db_User GetUserByPhoneNumber(string phoneNumber)
        {
            db_User user;
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                user = context.db_User.Where(c => c.PhoneNumber == phoneNumber).FirstOrDefault();
            }
            return user;
        }
        #endregion

        #region 通过手机号获取用户信息
        public static UserModel GetUserModelByPhoneNumber(string phoneNumber)
        {
            UserModel user;
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                var sql = "SELECT t1.UserID,t1.PhoneNumber,t1.UserWebNickName," +
                         "  t1.UserWebPicture,t1.UserName,t1.Address,t1.Sex,CONVERT(varchar(100), t1.Birthday, 23) as Birthday,t1.Hobby" +
                         "  FROM db_User t1 WHERE t1.PhoneNumber= '"+phoneNumber+"'";

                user = context.Database.SqlQuery<UserModel>(sql)
                                 .FirstOrDefault();
                if (user != null)
                {
                    //添加擅长英雄图标
                    user.HeroImage = User.GetHeroImgeByUserID(user.UserID);
                    user.Asset = User.GetAssetByUserID(user.UserID);
                    user.GamePower = User.GetGamePowerByUserID(user.UserID);
                }
            }
            return user;
        }
        #endregion

        #region 通过UserID获取用户信息
        public static UserModel GetUserModelByUserID(int userID)
        {
            UserModel user;
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                var sql = "SELECT t1.UserID,t1.PhoneNumber,t1.UserWebNickName," +
                         " CONVERT(varchar(100), t1.RegisterDate, 20) as RegDate," +
                         "  t1.UserWebPicture,t1.UserName,t1.Address,t1.Sex,CONVERT(varchar(100), t1.Birthday, 23) as Birthday,t1.Hobby" +
                         "  FROM db_User t1 WHERE t1.UserID= " + userID + "";

                user = context.Database.SqlQuery<UserModel>(sql)
                                 .FirstOrDefault();
                if (user!=null)
                {
                    //添加擅长英雄图标
                    user.HeroImage = User.GetHeroImgeByUserID(user.UserID);
                    user.Asset = User.GetAssetByUserID(user.UserID);
                    user.GamePower = User.GetGamePowerByUserID(user.UserID);
                }
                
            }
            return user;
        }
        #endregion

        #region 通过昵称获取用户信息
        public static UserModel GetUserModelByNickName(string nickName)
        {
            UserModel user;
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                var sql = "SELECT t1.UserID,t1.PhoneNumber,t1.UserWebNickName," +
                         "  t1.UserWebPicture,t1.UserName,t1.Address,t1.Sex,CONVERT(varchar(100), t1.Birthday, 23) as Birthday,t1.Hobby" +
                         "  FROM db_User t1 WHERE t1.UserWebNickName= " + nickName + "";

                user = context.Database.SqlQuery<UserModel>(sql)
                                 .FirstOrDefault();
            }
            return user;
        }
        #endregion

        #region 判断昵称是否存在
        public static bool GetUserByNickName(string nickName)
        {
            db_User user;
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                user = context.db_User.Where(c => c.UserWebNickName == nickName).FirstOrDefault();
            }
            if (user == null)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region 通过UserID获取三个擅长英雄
        public static List<HeroModel> GetHeroImgeByUserID(int userID)
        {
            List<HeroModel> heroImage;
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                //
                var sql = "SELECT" +
                      " TOP 3 t3.HeroImage" +
                      " FROM" +
                      " db_GameIDofUser t1" +
                      " LEFT JOIN db_GameInfoofPlatform t2 ON t1.UGID = t2.UGID" +
                      " LEFT JOIN db_CommonUseHero t3 ON t3.GamePlatformID = t2.GamePlatformID" +
                      " WHERE t1.UserID = "+ userID+" AND t1.GameType = 'DOTA2'";

                heroImage = context.Database.SqlQuery<HeroModel>(sql)
                                 .ToList();
            }
            return heroImage;
        }
        #endregion

        #region 通过UserID获取我的战斗力
        public static int GetGamePowerByUserID(int userID)
        {
            GameModel gameInfo = new GameModel();
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                //
                // 获取用户游戏数据
                var sql = "select t1.UserID,t1.GameID,t1.CertifyState,t2.GamePower,t1.CertifyName" +
                        " from db_GameIDofUser t1" +
                        " left join db_GameInfoofPlatform t2" +
                        " on t1.UGID = t2.UGID" +
                        " where t1.UserID = " + userID + " and t1.GameType = 'DOTA2'";

                gameInfo = context.Database.SqlQuery<GameModel>(sql)
                                 .FirstOrDefault();

                if (gameInfo == null)
                {
                    //无游戏数据
                    return 0;
                }
            }
            return Convert.ToInt32(gameInfo.GamePower);
        }
        #endregion

        #region 通过UserID获取我的财产
        public static int? GetAssetByUserID(int userID)
        {
            int? asset;
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                //获取用户总资产
                asset = context.db_AssetRecord.Where(c => c.UserID == userID).Sum(c => c.VirtualMoney);
            }
            return asset;
        }
        #endregion

    }
}
