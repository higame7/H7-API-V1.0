/******************************************************************************

** author:zihai

** create date:2016-02-18

** update date:2016-02-18

** description : 用户restful API，
                 提供涉及到用户的服务。

******************************************************************************/

using System.Net.Http;
using System.Text;
using System.Web.Http;
using HaiGame7.RestAPI.Filter;
using HaiGame7.BLL;
using HaiGame7.Model.MyModel;
using log4net;

namespace HaiGame7.RestAPI.Controllers
{  
    /// <summary>
    /// 用户中心restful API，提供涉及到用户的服务。
    /// </summary>
    [AccessTokenFilter]
    [ExceptionFilter]
    public class UserController : ApiController
    {
        readonly private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //初始化Response信息
        HttpResponseMessage returnResult = new HttpResponseMessage();
        //初始化返回结果
        string jsonResult;

        #region Login 登录
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns>
        /// 登录成功：{"MessageCode":0,"Message":""}
        /// 手机号不存在：{"MessageCode":10001,"Message":"no user"}
        /// 密码错误：{"MessageCode":10002,"Message":"password error"}
        /// </returns>
        [HttpPost]
        public HttpResponseMessage Login([FromBody] SimpleUserModel user)
        {
            UserLogic userLogic = new UserLogic();
            jsonResult = userLogic.Login(user);

            returnResult.Content =new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region VerifyCode1 获取验证码（注册用）
        /// <summary>
        /// 获取验证码（注册用）
        /// </summary>
        /// <returns>
        /// 获取成功：{"MessageCode":0,"Message":"验证码"}
        /// 获取失败：{"MessageCode":10003,"Message":"verfitycode error"}
        /// 手机号已注册：{"MessageCode":10004,"Message":"user exist"}
        /// </returns>
        [HttpPost]
        public HttpResponseMessage VerifyCode1([FromBody] SimpleUserModel user)
        {
            UserLogic userLogic = new UserLogic();
            jsonResult = userLogic.VerifyCode1(user);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region VerifyCode2 获取验证码（找回密码用）
        /// <summary>
        /// 获取验证码（找回密码用）
        /// </summary>
        /// <returns>
        /// 获取成功：{"MessageCode":0,"Message":"验证码"}
        /// 手机号不存在：{"MessageCode":10001,"Message":"no user"}
        /// 验证码获取失败：{"MessageCode":10003,"Message":"verfitycode error"}
        /// </returns>
        [HttpPost]
        public HttpResponseMessage VerifyCode2([FromBody] SimpleUserModel user)
        {
            UserLogic userLogic = new UserLogic();
            jsonResult = userLogic.VerifyCode2(user);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region Register 注册
        /// <summary>
        /// 注册
        /// </summary>
        /// <returns>
        /// 注册成功：{"MessageCode":0,"Message":""}
        /// 手机号已存在：{"MessageCode":10004,"Message":"user exist"}
        /// 验证码错误：{"MessageCode":10005,"Message":"verifycode error"}
        /// 验证码过期：{"MessageCode":10006,"Message":"verifycode expire"}
        /// </returns>
        [HttpPost]
        public HttpResponseMessage Register([FromBody] SimpleUserModel user)
        {
            UserLogic userLogic = new UserLogic();
            jsonResult = userLogic.Register(user);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region ResetPassWord 重置密码
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <returns>
        /// 重置成功：{"MessageCode":0,"Message":""}
        /// 手机号不存在：{"MessageCode":10001,"Message":"no user"}
        /// 验证码错误：{"MessageCode":10005,"Message":"verifycode error"}
        /// 验证码过期：{"MessageCode":10006,"Message":"verifycode expire"}
        /// </returns>
        [HttpPost]
        public HttpResponseMessage ResetPassWord([FromBody] SimpleUserModel user)
        {
            UserLogic userLogic = new UserLogic();
            jsonResult = userLogic.ResetPassWord(user);

            logger.Error("验证码获取失败" + user.PassWord);
            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region UserInfo 获取个人信息
        /// <summary>
        /// 根据手机号获取个人信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns>
        /// 返回值实例：[{"MessageCode":0,"Message":""},
        /// {"UserID":64,UserInfo,"PassWord":null,"UserWebPicture":"http://images.haigame7.com/avatar/20160127162940WxExqw0paJXAo1AtXc4RzGYo2LE=.png","UserWebNickName":"不服","UserName":null,"Address":"北京-大兴区","Sex":"男","Birthday":"2016-03-18","Hobby":null}]
        /// </returns>
        [HttpPost]
        public HttpResponseMessage UserInfo([FromBody] SimpleUserModel user)
        {
            UserLogic userLogic = new UserLogic();
            jsonResult = userLogic.UserInfo(user);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region UserInfoByNickName 根据昵称获取个人信息
        /// <summary>
        /// 根据昵称获取个人信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns>
        /// 返回值实例：[{"MessageCode":0,"Message":""},
        /// {"UserID":64,"PhoneNumber":"13439843883","PassWord":null,"UserWebPicture":"http://images.haigame7.com/avatar/20160127162940WxExqw0paJXAo1AtXc4RzGYo2LE=.png","UserWebNickName":"不服","UserName":null,"Address":"北京-大兴区","Sex":"男","Birthday":"2016-03-18","Hobby":null}]
        /// </returns>
        [HttpPost]
        public HttpResponseMessage UserInfoByNickName([FromBody] SimpleUserModel user)
        {
            UserLogic userLogic = new UserLogic();
            jsonResult = userLogic.UserInfoByNickName(user);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region UserInfoByUserID 根据UserID获取个人信息
        /// <summary>
        /// 根据昵称获取个人信息
        /// </summary>
        /// <param name="user">
        /// {"UserID":184,"Message":""}
        /// </param>
        /// <returns>
        /// 返回值实例：[{"MessageCode":0,"Message":""}]
        /// </returns>
        [HttpPost]
        public HttpResponseMessage UserInfoByUserID([FromBody] UserParameterModel user)
        {
            UserLogic userLogic = new UserLogic();
            jsonResult = userLogic.UserInfoByUserID(user);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region UpdateUserInfo 更改个人信息
        /// <summary>
        /// 更改个人信息，传入手机号和要更改的字段，不更改字段不要传
        /// </summary>
        /// <param name="user"></param>
        /// <returns>
        /// 更改成功：{"MessageCode":0,"Message":""}
        /// 用户不存在：{"MessageCode":10001,"Message":"no user"}
        /// </returns>
        [HttpPost]
        public HttpResponseMessage UpdateUserInfo([FromBody] UserModel user)
        {
            UserLogic userLogic = new UserLogic();
            jsonResult = userLogic.UpdateUserInfo(user);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region MyGameInfo 我的游戏数据
        /// <summary>
        /// 我的游戏数据
        /// </summary>
        /// <param name="user"></param>
        /// <returns>
        /// 返回值实例：[{"MessageCode":0,"Message":""},
        /// {"UserID":64,"GameID":"173032376","GamePower":"0","CertifyState":0,"CertifyName":"氦七G9SJkIJQ8l+uZP4BJEVZ+aHEtLY="}]
        /// </returns>
        public HttpResponseMessage MyGameInfo([FromBody] SimpleUserModel user)
        {
            UserLogic userLogic = new UserLogic();
            jsonResult = userLogic.MyGameInfo(user);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region UpdateCertifyGameID 更改认证游戏ID
        /// <summary>
        /// 更改认证游戏ID
        /// </summary>
        /// <param name="user"></param>
        /// <returns>
        /// 提交成功：{"MessageCode":0,"Message":返回认证昵称}
        /// </returns>
        [HttpPost]
        public HttpResponseMessage UpdateCertifyGameID([FromBody] SimpleUserModel user)
        {
            UserLogic userLogic = new UserLogic();
            jsonResult = userLogic.UpdateCertifyGameID(user);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region CertifyGameID 提交认证游戏ID
        /// <summary>
        /// 提交认证游戏ID
        /// </summary>
        /// <param name="user"></param>
        /// <returns>
        /// 提交成功：{"MessageCode":0,"Message":返回认证昵称}
        /// </returns>
        [HttpPost]
        public HttpResponseMessage CertifyGameID([FromBody] SimpleUserModel user)
        {
            UserLogic userLogic = new UserLogic();
            jsonResult = userLogic.CertifyGameID(user);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region MyAssetList 我的资产列表
        /// <summary>
        /// 我的资产列表
        /// </summary>
        /// <param name="user">
        /// 参数实例：{UserID:184,StartPage:1,PageCount:10}
        /// </param>
        /// <returns>
        /// 返回值实例：[{"MessageCode":0,"Message":""},
        /// [{"VirtualMoney":0,"GainTime":"2016-02-27","GainWay":"现金充值","Remark":"2016/2/27 12:38:35 现金充值 获得虚拟币：0"},
        /// {"VirtualMoney":0,"GainTime":"2016-02-27","GainWay":"现金充值","Remark":"2016/2/27 12:38:24 现金充值 获得虚拟币：0"},
        /// {"VirtualMoney":-50,"GainTime":"2016-02-26","GainWay":"接受挑战","Remark":"2016/2/26 13:06:35 接受挑战 消费虚拟币：-50"},
        /// {"VirtualMoney":-50,"GainTime":"2016-02-15","GainWay":"接受挑战","Remark":"2016/2/15 13:15:22 接受挑战 消费虚拟币：-50"},
        /// {"VirtualMoney":-1,"GainTime":"2016-02-15","GainWay":"参加竞猜","Remark":"2016/2/15 13:06:52 参加竞猜 消费虚拟币：-1"},
        /// {"VirtualMoney":-1,"GainTime":"2016-01-28","GainWay":"参加竞猜","Remark":"2016/1/28 10:50:20 参加竞猜 消费虚拟币：-1"},
        /// {"VirtualMoney":1000,"GainTime":"2016-01-27","GainWay":"注册","Remark":"2016/1/27 13:28:46 注册 获得虚拟币：50"}]]
        /// ]
        /// </returns>
        [HttpPost]
        public HttpResponseMessage MyAssetList([FromBody] UserParameterModel user)
        {
            UserLogic userLogic = new UserLogic();
            jsonResult = userLogic.MyAssetList(user);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region MyTotalAsset 我的总资产
        /// <summary>
        /// 我的总资产，返回总氦气，和我的资产排名
        /// </summary>
        /// <param name="user"></param>
        /// <returns>
        /// 返回值实例：[{"MessageCode":0,"Message":""},{"TotalAsset":"898","MyRank":"8"}]
        /// </returns>
        [HttpPost]
        public HttpResponseMessage MyTotalAsset([FromBody] SimpleUserModel user)
        {
            UserLogic userLogic = new UserLogic();
            jsonResult = userLogic.MyTotalAsset(user);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region NoTeamUserList 未加入战队用户列表
        /// <summary>
        /// 未加入战队用户列表
        /// </summary>
        /// <param name="para">
        /// {StartPage:1,PageCount:5}
        /// </param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage NoTeamUserList([FromBody] UserListParameterModel para)
        {
            UserLogic userLogic = new UserLogic();
            jsonResult = userLogic.NoTeamUserList(para);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region MyMessage 我的消息
        /// <summary>
        /// 我的消息
        /// </summary>
        /// <param name="para">
        /// {UserID:64,StartPage:1,PageCount:5}
        /// </param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage MyMessage([FromBody] UserParameterModel para)
        {
            UserLogic userLogic = new UserLogic();
            jsonResult = userLogic.MyMessage(para);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region SetMessageRead 消息设为已读
        /// <summary>
        /// 消息设为已读
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SetMessageRead([FromBody] MyMessageModel para)
        {
            UserLogic userLogic = new UserLogic();
            jsonResult = userLogic.SetMessageRead(para);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region DeleteMessage 删除消息
        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="para">
        ///  {MessageID:64}
        /// </param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage DeleteMessage([FromBody] MyMessageModel para)
        {
            UserLogic userLogic = new UserLogic();
            jsonResult = userLogic.DeleteMessage(para);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region DeleteAssetRecord 删除微信支付订单
        /// <summary>
        /// 删除微信支付订单
        /// </summary>
        /// <param name="para">
        /// {OutTradeno:"133333333333333"}
        /// </param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage DeleteAssetRecord([FromBody] AssetModel para)
        {
            UserLogic userLogic = new UserLogic();
            jsonResult = userLogic.DeleteAssetRecord(para);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region 每日签到奖励1氦气
        /// <summary>
        /// 每日签到奖励1氦气
        /// </summary>
        /// <param name="para">
        /// {UserID:184}
        /// </param>
        /// <returns>
        /// 签到成功：[{"MessageCode":0,"Message":""}]
        /// 今日已签到：[{"MessageCode":60002,"Message":"今日已签到"}]
        /// </returns>
        [HttpPost]
        public HttpResponseMessage SignIn([FromBody] UserParameterModel para)
        {
            UserLogic userLogic = new UserLogic();
            jsonResult = userLogic.SignIn(para);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region 判断今日是否签到
        /// <summary>
        /// 判断今日是否签到
        /// </summary>
        /// <param name="para">
        /// {UserID:184}
        /// </param>
        /// <returns>
        /// 今日未签到：[{"MessageCode":60003,"Message":"今日未签到"}]
        /// 今日已签到：[{"MessageCode":60002,"Message":"今日已签到"}]
        /// </returns>
        [HttpPost]
        public HttpResponseMessage IsSignIn([FromBody] UserParameterModel para)
        {
            UserLogic userLogic = new UserLogic();
            jsonResult = userLogic.IsSignIn(para);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region 充值
        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="para">
        /// {UserID:184,VirtualMoney:100}
        /// </param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Recharge([FromBody] UserRechargeParameterModel para)
        {
            UserLogic userLogic = new UserLogic();
            jsonResult = userLogic.Recharge(para);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion
    }
}