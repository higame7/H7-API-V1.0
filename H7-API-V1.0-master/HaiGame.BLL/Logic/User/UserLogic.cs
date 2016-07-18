using System.Collections.Generic;
using System.Linq;
using HaiGame7.Model.EFModel;
using HaiGame7.Model.MyModel;
using HaiGame7.BLL.Enum;
using HaiGame7.BLL.Logic.Common;
using System.Web.Script.Serialization;
using System;
using System.Security.Cryptography;
using System.Web;

namespace HaiGame7.BLL
{
    public class UserLogic
    {
        #region 登录处理
        public string Login(SimpleUserModel user)
        {
            string result="";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();

            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                // 判断手机号是否存在
                db_User dbUser = context.db_User.Where(c => c.PhoneNumber == user.PhoneNumber.Trim()).FirstOrDefault();
                if (dbUser != null)
                {
                    MD5 md5Hash = MD5.Create();
                    if (dbUser.UserPassWord == Common.GetMd5Hash(md5Hash,user.PassWord))
                    {
                        //登录成功
                        message.MessageCode = MESSAGE.OK_CODE;
                        message.Message = MESSAGE.OK;
                    }
                    else
                    {
                        //密码错误
                        message.MessageCode = MESSAGE.PWSERR_CODE;
                        message.Message = MESSAGE.PWSERR;
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

        #region 获取验证码（找回密码用）
        public string VerifyCode2(SimpleUserModel user)
        {
            string result = "";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();

            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                // 判断手机号是否存在
                db_User dbUser = context.db_User.Where(c => c.PhoneNumber == user.PhoneNumber.Trim()).FirstOrDefault();
                if (dbUser != null)
                {
                    //验证码
                    string verifyCode = Common.MathRandom(4);
                    //发送验证码
                    Dictionary<string,object> ret=Common.SendSMS(user.PhoneNumber, verifyCode);
                    //返回发送结果
                    if (ret["statusCode"].ToString()=="000000")
                    {
                        //获取验证码成功
                        message.MessageCode = MESSAGE.OK_CODE;
                        message.Message = verifyCode;
                    }
                    else
                    {
                        //获取验证码失败
                        message.MessageCode = MESSAGE.SMSERR_CODE;
                        message.Message = MESSAGE.SMSERR;
                    }
                }
                else
                {
                    //手机号不存在
                    message.MessageCode = MESSAGE.NOUSER_CODE;
                    message.Message = MESSAGE.NOUSER;
                }
            }
            returnResult.Add(message);
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 获取验证码（注册用）
        public string VerifyCode1(SimpleUserModel user)
        {
            string result = "";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();

            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                // 判断手机号是否存在
                db_User dbUser = context.db_User.Where(c => c.PhoneNumber == user.PhoneNumber.Trim()).FirstOrDefault();
                if (dbUser == null)
                {
                    //验证码
                    string verifyCode = Common.MathRandom(4);
                    //发送验证码
                    Dictionary<string, object> ret = Common.SendSMS(user.PhoneNumber, verifyCode);
                    //返回发送结果
                    if (ret["statusCode"].ToString() == "000000")
                    {
                        //手机号，验证码存储到session
                        //获取验证码成功
                        message.MessageCode = MESSAGE.OK_CODE;
                        message.Message = verifyCode;
                    }
                    else
                    {
                        //获取验证码失败
                        message.MessageCode = MESSAGE.SMSERR_CODE;
                        message.Message = MESSAGE.SMSERR;
                    }
                }
                else
                {
                    //手机号已注册
                    message.MessageCode = MESSAGE.USEREXIST_CODE;
                    message.Message = MESSAGE.USEREXIST;
                }
            }
            returnResult.Add(message);
            result = jss.Serialize(returnResult);
            return result;
        }    
        #endregion

        #region 注册
        public string Register(SimpleUserModel user)
        {
            string result = "";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();

            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                // 判断手机号是否存在
                db_User dbUser = context.db_User.Where(c => c.PhoneNumber == user.PhoneNumber.Trim()).FirstOrDefault();
                if (dbUser == null)
                {
                    db_AssetRecord assetRecord = new db_AssetRecord();
                    db_User userRecord = new db_User();

                    //判断验证码是否正确

                    //判断验证码是否过期

                    //添加信息到User表
                    userRecord.PhoneNumber = user.PhoneNumber;
                    MD5 md5Hash = MD5.Create();
                    userRecord.UserPassWord = Common.GetMd5Hash(md5Hash, user.PassWord);
                    userRecord.RegisterDate = DateTime.Now;
                    userRecord.UserWebPicture = @"http://images.haigame7.com/avatar/20160127125552WxExqw0paJXAo1AtXc4RzGYo2LE=.png";

                    context.db_User.Add(userRecord);
                    context.SaveChanges();

                    //添加信息到资产表
                    db_User regUser = context.db_User.Where(c => c.PhoneNumber == user.PhoneNumber.Trim()).FirstOrDefault();
                    Asset.AddMoneyRegister(regUser.UserID);
                    
                    //添加成功
                    message.MessageCode = MESSAGE.OK_CODE;
                    message.Message = MESSAGE.OK;
                }
                else
                {
                    //手机号已存在
                    message.MessageCode = MESSAGE.USEREXIST_CODE;
                    message.Message = MESSAGE.USEREXIST;
                }
            }
            returnResult.Add(message);
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 重置密码
        public string ResetPassWord(SimpleUserModel user)
        {
            string result = "";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();

            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                // 判断手机号是否存在
                db_User dbUser = context.db_User.Where(c => c.PhoneNumber == user.PhoneNumber.Trim()).FirstOrDefault();
                if (dbUser == null)
                {
                    //手机号不存在
                    message.MessageCode = MESSAGE.NOUSER_CODE;
                    message.Message = MESSAGE.NOUSER;
                }
                else
                {
                    //修改密码
                    MD5 md5Hash = MD5.Create();
                    dbUser.UserPassWord= Common.GetMd5Hash(md5Hash, user.PassWord);
                    context.SaveChanges();
                    //修改成功
                    message.MessageCode = MESSAGE.OK_CODE;
                    message.Message = MESSAGE.OK;
                }
            }
            returnResult.Add(message);
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 根据手机号获取我的个人信息
        public string UserInfo(SimpleUserModel user)
        {
            string result = "";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();

            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                //获取用户
                UserModel userInfo = User.GetUserModelByPhoneNumber(user.PhoneNumber);
                if (userInfo!=null)
                {
                    message.Message = MESSAGE.OK;
                    message.MessageCode = MESSAGE.OK_CODE;
                }
                else
                {
                    message.Message = MESSAGE.NOUSER;
                    message.MessageCode = MESSAGE.NOUSER_CODE;
                }
                returnResult.Add(message);
                returnResult.Add(userInfo);
            }
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 根据昵称获取个人信息
        public string UserInfoByNickName(SimpleUserModel user)
        {
            string result = "";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();

            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                //获取用户
                UserModel userInfo = User.GetUserModelByNickName(user.PhoneNumber);
                if (userInfo != null)
                {
                    message.Message = MESSAGE.OK;
                    message.MessageCode = MESSAGE.OK_CODE;
                }
                else
                {
                    message.Message = MESSAGE.NOUSER;
                    message.MessageCode = MESSAGE.NOUSER_CODE;
                }
                returnResult.Add(message);
                returnResult.Add(userInfo);
            }
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 根据UserID获取个人信息
        public string UserInfoByUserID(UserParameterModel user)
        {
            string result = "";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();

            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                //获取用户
                UserModel userInfo = User.GetUserModelByUserID(user.UserID);
                if (userInfo != null)
                {
                    message.Message = MESSAGE.OK;
                    message.MessageCode = MESSAGE.OK_CODE;
                }
                else
                {
                    message.Message = MESSAGE.NOUSER;
                    message.MessageCode = MESSAGE.NOUSER_CODE;
                }
                returnResult.Add(message);
                returnResult.Add(userInfo);
            }
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 更改个人信息
        public string UpdateUserInfo(UserModel user)
        {
            string result = "";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();

            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                //获取用户
                db_User userInfo = context.db_User.Where(c => c.PhoneNumber == user.PhoneNumber).FirstOrDefault();
                
                if (userInfo != null)
                {
                    int code = MESSAGE.OK_CODE;
                    string msg = MESSAGE.OK;

                    #region 个人信息字段
                    if (user.Address!=null)
                    {
                        userInfo.Address = user.Address;
                    }
                    if (user.Birthday != null)
                    {
                        userInfo.Birthday = DateTime.Parse(user.Birthday);
                    }
                    if (user.Hobby != null)
                    {
                        userInfo.Hobby = user.Hobby;
                    }
                    if (user.Sex != null)
                    {
                        userInfo.Sex = user.Sex;
                    }
                    if (user.UserWebNickName != null)
                    {
                        //验证昵称是否存在
                        if(User.GetUserByNickName(user.UserWebNickName.Trim()))
                        {
                            userInfo.UserWebNickName = user.UserWebNickName;
                        }
                        else
                        {
                            //昵称已存在
                            msg = MESSAGE.NICKEXIST;
                            code = MESSAGE.NICKEXIST_CODE;  
                        }
                    }
                    if (user.UserWebPicture != null)
                    {
                        userInfo.UserWebPicture = Common.Base64ToImage(user.UserWebPicture, userInfo.UserID.ToString());
                    }
                    #endregion
                    context.SaveChanges();
                    message.Message = msg;
                    message.MessageCode = code;
                }
                else
                {
                    message.Message = MESSAGE.NOUSER;
                    message.MessageCode = MESSAGE.NOUSER_CODE;
                }
            }
            returnResult.Add(message);
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 获取我的资产列表
        public string MyAssetList(UserParameterModel user)
        {
            string result = "";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();
            //获取我的资产
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                //获取用户资产列表
                var sql = "SELECT t1.VirtualMoney,CONVERT(varchar(100), t1.GainTime, 23) as GainTime,t1.GainWay,t1.Remark" +
                          " FROM db_AssetRecord t1" +
                          " WHERE t1.UserID = " + user.UserID + "ORDER BY t1.GainTime DESC";

                var assetList = context.Database.SqlQuery<AssetList>(sql)
                                 .Skip((user.StartPage - 1) * user.PageCount)
                                 .Take(user.PageCount).ToList();

                message.Message = MESSAGE.OK;
                message.MessageCode = MESSAGE.OK_CODE;
                returnResult.Add(message);
                returnResult.Add(assetList);
            }
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 获取我的总资产
        public string MyTotalAsset(SimpleUserModel user)
        {
            string result = "";
            MessageModel message = new MessageModel();
            MyAssetModel myAsset = new MyAssetModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();
            //获取我的资产
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                //获取用户
                db_User userInfo = User.GetUserByPhoneNumber(user.PhoneNumber);
                if (userInfo!=null && user.PhoneNumber!=null)
                {
                    //获取用户总资产
                    var asset = context.db_AssetRecord.Where(c => c.UserID == userInfo.UserID).Sum(c => c.VirtualMoney);
                    myAsset.TotalAsset = (int)asset;
                    //获取用户资产排名
                    myAsset.MyRank = Asset.MyRank(myAsset.TotalAsset, (DateTime)userInfo.RegisterDate);

                    message.Message = MESSAGE.OK;
                    message.MessageCode = MESSAGE.OK_CODE;
                }
                else
                {
                    message.Message = MESSAGE.NOUSER;
                    message.MessageCode = MESSAGE.NOUSER_CODE;
                }
                
                returnResult.Add(message);
                returnResult.Add(myAsset);
            }
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 我的游戏数据
        public string MyGameInfo(SimpleUserModel user)
        {
            string result = "";
            MessageModel message = new MessageModel();
            GameModel gameInfo=new GameModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();

            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {

                //获取用户
                db_User userInfo = User.GetUserByPhoneNumber(user.PhoneNumber);
                if (userInfo != null)
                {
                    // 获取用户游戏数据
                    var sql = "select t1.UserID,t1.GameID,t1.CertifyState,t2.GamePower,t1.CertifyName" +
                            " from db_GameIDofUser t1" +
                            " left join db_GameInfoofPlatform t2" +
                            " on t1.UGID = t2.UGID" +
                            " where t1.UserID = " + userInfo.UserID + " and t1.GameType = 'DOTA2'";

                    gameInfo = context.Database.SqlQuery<GameModel>(sql)
                                     .FirstOrDefault();

                    if (gameInfo == null)
                    {
                        //无游戏数据
                        message.Message = MESSAGE.NOGAMEDATA;
                        message.MessageCode = MESSAGE.NOGAMEDATA_CODE;
                    }
                    else
                    {
                        message.Message = MESSAGE.OK;
                        message.MessageCode = MESSAGE.OK_CODE;
                    }
                }
                else
                {
                    message.Message = MESSAGE.NOUSER;
                    message.MessageCode = MESSAGE.NOUSER_CODE;
                }

                returnResult.Add(message);
                returnResult.Add(gameInfo);
            }
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 提交认证游戏ID
        public string CertifyGameID(SimpleUserModel user)
        {
            string result = "";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();

            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                //获取用户
                db_User userInfo = User.GetUserByPhoneNumber(user.PhoneNumber);
                if (userInfo != null)
                {
                    db_GameIDofUser gameIDofUser=new db_GameIDofUser();
                    gameIDofUser.UserID = userInfo.UserID;   
                    gameIDofUser.GameID = user.GameID;
                    gameIDofUser.GameType = "DOTA2";
                    gameIDofUser.CertifyState = 2;//正在认证
                    gameIDofUser.CertifyName = "氦七"+Common.MathRandom(6);
                    gameIDofUser.ApplyCertifyTime = DateTime.Now;
                    context.db_GameIDofUser.Add(gameIDofUser);
                    context.SaveChanges();

                    message.Message = gameIDofUser.CertifyName;
                    message.MessageCode = MESSAGE.OK_CODE;
                }
                else
                {
                    message.Message = MESSAGE.NOUSER;
                    message.MessageCode = MESSAGE.NOUSER_CODE;
                }
            }
            returnResult.Add(message);
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 更改认证游戏ID
        public string UpdateCertifyGameID(SimpleUserModel user)
        {
            string result = "";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();

            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                //获取用户
                db_User userInfo = User.GetUserByPhoneNumber(user.PhoneNumber);
                if (userInfo != null)
                {
                    db_GameIDofUser gameIDofUser =context.db_GameIDofUser.
                                    Where(c=>c.UserID== userInfo.UserID).
                                    Where(c => c.GameType == "DOTA2").
                                    FirstOrDefault();

                    gameIDofUser.GameID = user.GameID;
                    gameIDofUser.CertifyState = 2;//正在认证
                    gameIDofUser.CertifyName = "氦七" + Common.MathRandom(6);
                    gameIDofUser.ApplyCertifyTime = DateTime.Now;
                    context.SaveChanges();
                    //返回认证昵称
                    message.Message = gameIDofUser.CertifyName;
                    message.MessageCode = MESSAGE.OK_CODE;
                }
                else
                {
                    //无用户信息
                    message.Message = MESSAGE.NOUSER;
                    message.MessageCode = MESSAGE.NOUSER_CODE;
                }
            }
            returnResult.Add(message);
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 未加入战队用户列表
        public string NoTeamUserList(UserListParameterModel para)
        {
            string result = "";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();
            List<User2Model> userInfo;
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                //查询条件：user表中没有战队信息的user信息，按注册日期排序
                //var sql = "SELECT t1.UserID,t1.PhoneNumber,t1.UserWebNickName," +
                //         "  t1.UserWebPicture,t1.UserName,t1.Address,"+
                //         "  t1.Sex,CONVERT(varchar(100), t1.Birthday, 23) as Birthday,t1.Hobby" +
                //         "  FROM"+
                //         "  db_User t1"+
                //         "  LEFT JOIN db_Team t2 ON t1.UserID = t2.CreateUserID"+
                //         "  LEFT JOIN db_TeamUser t3 ON t1.UserID = t3.UserID"+
                //         "  LEFT JOIN db_GameIDofUser t4 ON t1.UserID = t4.UserID" +
                //         "  WHERE t2.CreateUserID IS NULL AND t3.UserID IS NULL AND t4.CertifyState=1";

                var sql = "SELECT t1.UserID,t1.PhoneNumber,t1.UserWebNickName," +
                         "  t1.UserWebPicture,t1.UserName,t1.Address," +
                         "  t1.Sex,CONVERT(varchar(100), t1.Birthday, 23) as Birthday,t1.Hobby" +
                         "  FROM" +
                         "  db_User t1" +
                         "  LEFT JOIN db_Team t2 ON t1.UserID = t2.CreateUserID" +
                         "  LEFT JOIN db_TeamUser t3 ON t1.UserID = t3.UserID" +
                         "  LEFT JOIN db_GameIDofUser t4 ON t1.UserID = t4.UserID" +
                         "  WHERE t2.CreateUserID IS NULL AND t3.UserID IS NULL ";

                userInfo = context.Database.SqlQuery<User2Model>(sql)
                                 .Skip((para.StartPage - 1) * para.PageCount)
                                 .Take(para.PageCount).ToList();
                //循环user，添加擅长英雄图标
                for (int i=0;i< userInfo.Count;i++)
                {
                    ///氦气
                    userInfo[i].Asset = User.GetAssetByUserID(userInfo[i].UserID);
                    //战斗力
                    userInfo[i].GamePower= User.GetGamePowerByUserID(userInfo[i].UserID);
                    //擅长英雄
                    userInfo[i].HeroImage = User.GetHeroImgeByUserID(userInfo[i].UserID);
                }
            }
            returnResult.Add(message);
            returnResult.Add(userInfo);
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 我的消息
        public string MyMessage(UserParameterModel para)
        {
            string result = "";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();
            List<MyMessageModel> messageInfo;
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {

                //单发消息
                var sql = "SELECT" +
                            " t1.MID as MessageID,t1.Title,t1.Content," +
                            " (CASE WHEN t1.State = 0 then '未读' else '已读' end) as State," +
                            " CONVERT(varchar(100), t1.SendTime, 20) as Time" +
                            " FROM" +
                            " db_Message t1" +
                            " WHERE t1.State in (0,99) AND t1.ReceiveID =" + para.UserID+
                            " ORDER BY t1.State ,t1.SendTime DESC";
                            //" union" +
                            ////群发消息
                            //" SELECT" +
                            //" t1.MID as MessageID,t1.Title,t1.Content," +
                            //" (CASE WHEN t2.SysState IS NULL then '未读' else '已读' end) as State," +
                            //" CONVERT(varchar(100), t1.SendTime, 20) as Time" +
                            //" FROM" +
                            //" db_Message t1" +
                            //" left JOIN db_SysMessage t2 ON t1.MID = t2.MID" +
                            //" WHERE t1.SendID = 0";

                messageInfo = context.Database.SqlQuery<MyMessageModel>(sql)
                                 .Skip((para.StartPage - 1) * para.PageCount)
                                 .Take(para.PageCount).ToList();
                var sqlCount = "SELECT" +
                            " t1.MID as MessageID,t1.Title,t1.Content," +
                            " (CASE WHEN t1.State = 0 then '未读' else '已读' end) as State," +
                            " CONVERT(varchar(100), t1.SendTime, 20) as Time" +
                            " FROM" +
                            " db_Message t1" +
                            " WHERE t1.State =0 AND t1.ReceiveID =" + para.UserID;

                var messageInfoCount = context.Database.SqlQuery<MyMessageModel>(sqlCount)
                                 .ToList().Count;

                message.Message = messageInfoCount.ToString();
                message.MessageCode = MESSAGE.OK_CODE;
            }
            returnResult.Add(message);
            returnResult.Add(messageInfo);
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 消息设为已读
        public string SetMessageRead(MyMessageModel para)
        {
            string result = "";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                var myMessage=context.db_Message.Where(c => c.MID == para.MessageID).FirstOrDefault();
                myMessage.State = 99;
                context.SaveChanges();
                message.Message = "";
                message.MessageCode = MESSAGE.OK_CODE;
            }
            returnResult.Add(message);
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 删除消息
        public string DeleteMessage(MyMessageModel para)
        {
            string result = "";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                var myMessage = context.db_Message.Where(c => c.MID == para.MessageID).FirstOrDefault();
                if (myMessage!=null)
                {
                    context.db_Message.Remove(myMessage);
                }
                context.SaveChanges();
                message.Message = "";
                message.MessageCode = MESSAGE.OK_CODE;
            }
            returnResult.Add(message);
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 删除微信支付订单
        public string DeleteAssetRecord(AssetModel para)
        {
            string result = "";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();
            
            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                db_AssetRecord asset = context.db_AssetRecord.Where(c => c.OutTradeno == para.OutTradeno)
                                        .Where(c => c.TransactionID == "")
                                        .Where(c => c.VirtualMoney == 0).FirstOrDefault();
                if (asset!=null)
                {
                    context.db_AssetRecord.Remove(asset);
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

        #region 每日签到获取1氦气
        public string SignIn(UserParameterModel para)
        {
            string result = "";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();

            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                db_AssetRecord asset = context.db_AssetRecord.
                                        Where(c => c.UserID == para.UserID)
                                        .Where(c => c.GainWay == ASSET.GAINWAY_SIGN)
                                        .Where(c => c.GainTime.Value.Year == DateTime.Now.Year &&
                                        c.GainTime.Value.Month == DateTime.Now.Month &&
                                        c.GainTime.Value.Day == DateTime.Now.Day).FirstOrDefault();
                if (asset == null)
                {
                    db_AssetRecord assetRecord = new db_AssetRecord();

                    assetRecord.UserID = para.UserID;
                    assetRecord.VirtualMoney = ASSET.MONEY_SIGN;
                    assetRecord.TrueMoney = 0;
                    assetRecord.GainWay = ASSET.GAINWAY_SIGN;
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
                    message.Message = MESSAGE.OK;
                    message.MessageCode = MESSAGE.OK_CODE;
                }
                else
                {
                    //今日已签到
                    message.Message = MESSAGE.SIGN;
                    message.MessageCode = MESSAGE.SIGN_CODE;
                }
            }
            returnResult.Add(message);
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 判断今日是否签到
        public string IsSignIn(UserParameterModel para)
        {
            string result = "";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();

            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                db_AssetRecord asset = context.db_AssetRecord.
                                        Where(c => c.UserID == para.UserID)
                                        .Where(c => c.GainWay == ASSET.GAINWAY_SIGN)
                                        .Where(c => c.GainTime.Value.Year == DateTime.Now.Year &&
                                        c.GainTime.Value.Month == DateTime.Now.Month &&
                                        c.GainTime.Value.Day == DateTime.Now.Day).FirstOrDefault();
                if (asset == null)
                {
                    message.Message = MESSAGE.NOTSIGN;
                    message.MessageCode = MESSAGE.NOTSIGN_CODE;
                }
                else
                {
                    //今日已签到
                    message.Message = MESSAGE.SIGN;
                    message.MessageCode = MESSAGE.SIGN_CODE;
                }
            }
            returnResult.Add(message);
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion

        #region 苹果充值
        public string Recharge(UserRechargeParameterModel para)
        {
            string result = "";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();

            using (HiGame_V1Entities context = new HiGame_V1Entities())
            {
                
                    db_AssetRecord assetRecord = new db_AssetRecord();

                    assetRecord.UserID = para.UserID;
                    assetRecord.VirtualMoney = para.VirtualMoney;
                    assetRecord.TrueMoney = para.VirtualMoney/10;
                    assetRecord.GainWay = ASSET.GAINWAY_RECHARGE;
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
