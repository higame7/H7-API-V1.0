using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaiGame7.BLL.Enum
{
    public static class MESSAGE
    {
        //执行成功
        public const int OK_CODE = 0;
        public const string OK = "";

        //没有用户
        public const int NOUSER_CODE = 10001;
        public const string NOUSER = "no user";

        //密码错误
        public const int PWSERR_CODE = 10002;
        public const string PWSERR = "password error";

        //验证码获取失败
        public const int SMSERR_CODE = 10003;
        public const string SMSERR = "verfitycode error";

        //手机号已注册
        public const int USEREXIST_CODE = 10004;
        public const string USEREXIST = "user exist";

        //验证码错误
        public const int VERIFYERR_CODE = 10005;
        public const string VERIFYERR = "verifycode error";

        //验证码过期
        public const int VERIFYEXPIRE_CODE = 10006;
        public const string VERIFYEXPIRE = "verifycode expire";

        //昵称已存在
        public const int NICKEXIST_CODE = 10007;
        public const string NICKEXIST = "nickname exist";

        //无游戏数据
        public const int NOGAMEDATA_CODE = 10008;
        public const string NOGAMEDATA = "no gamedata";

        //战队名称已存在
        public const int TEAMEXIST_CODE = 20001;
        public const string TEAMEXIST = "team exist";

        //你是队员，无权创建战队
        public const int TEAMUSER_CODE = 20002;
        public const string TEAMUSER = "you are teanuser";

        //不属于任何战队
        public const int NOTEAM_CODE = 20003;
        public const string NOTEAM = "you are noteam";

        //您是队员，不能发起约战
        public const int USERCHALLENGE_CODE = 20004;
        public const string USERCHALLENGE = "您是队员，不能发起约战";

        //参数错误
        public const int PARAERR_CODE = 20005;
        public const string PARAERR = "parameter error";

        //已经加入其它战队
        public const int JIONTEAM_CODE = 20006;
        public const string JIONTEAM = "already join other team";

        //已经发出过申请
        public const int JIONEDTEAM_CODE = 20007;
        public const string JIONEDTEAM = "already apply this team";

        //已经邀请过队员
        public const int INVITEUSER_CODE = 20008;
        public const string INVITEUSER = "already invite this user";

        //无权解散战队
        public const int DELETETEAM_CODE = 20009;
        public const string DELETETEAM = "you cannot delete team";

        //超出约战限额，每天只可以约战一场
        public const int DAILYCOUNT_CODE = 20010;
        public const string DAILYCOUNT = "超出约战限额，每天只可以约战一场";

        //战队成员已满，不能加入
        public const int USERFULL_CODE = 20011;
        public const string USERFULL = "战队成员已满，不能加入";

        //用户已经加入其它战队
        public const int USERJOINOTHERTEAM_CODE = 20012;
        public const string USERJOINOTHERTEAM = "该队员已加入其它战队";

        //每人最多2支战队
        public const int TEAMCOUNT_CODE = 20013;
        public const string TEAMCOUNT = "最多只可创建2支战队";

        //战队不足5人，无法报名
        public const int TEAMUSERNOTENOUGH_CODE = 20014;
        public const string TEAMUSERNOTENOUGH = "战队不足5人，无法报名";

        //你已经加入其它战队
        public const int YOUJOINOTHERTEAM_CODE = 20014;
        public const string YOUJOINOTHERTEAM = "你已加入其它战队";

        //系统错误
        public const int SYSERR_CODE = 40001;
        public const string SYSERR = "system error";

        //没有AccessToken
        public const int NOACCESSTOKEN_CODE = 40002;
        public const string NOACCESSTOKEN = "no accesstoken";

        //AccessToken 无效
        public const int ACCESSTOKENINVALID_CODE = 40003;
        public const string ACCESSTOKENINVALID = "invalid accesstoken";

        //无报名信息
        public const int NOJOINMATCH_CODE = 50001;
        public const string NOJOINMATCH = "no joinmatch";

        //名额已满
        public const int MATCHFULL_CODE = 50002;
        public const string MATCHFULL = "名额已满";

        //队员无权报名
        public const int CANNOTJOINMATCH_CODE = 50003;
        public const string CANNOTJOINMATCH = "队员无权报名";

        //队员无权取消
        public const int CANNOTQUITMATCH_CODE = 50003;
        public const string CANNOTQUITMATCH = "队员无权取消";

        //氦气不足
        public const int NOMONEY_CODE = 60001;
        public const string NOMONEY = "氦气不足";

        //今日已签到
        public const int SIGN_CODE = 60002;
        public const string SIGN = "今日已签到";

        //今日未签到
        public const int NOTSIGN_CODE = 60003;
        public const string NOTSIGN = "今日未签到";
    }
}
