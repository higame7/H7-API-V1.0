using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaiGame7.BLL.Enum
{
    public static class ASSET
    {
        #region 定义金额
        //注册默认虚拟币50个
        public const int MONEY_REG = 50;
        //每日签到虚拟币1个
        public const int MONEY_SIGN = 1;
        #endregion

        #region 定义获取或消费途径
        public const string GAINWAY_REG = "注册";
        public const string GAINWAY_SIGN = "每日签到";
        public const string GAINWAY_RECHARGE = "现金充值";
        public const string GAINWAY_QUIZWIN = "竞猜获胜";
        public const string GAINWAY_QUIZLOSE = "竞猜失败";
        public const string GAINWAY_QUIZ = "参加竞猜";
        public const string GAINWAY_PLAY = "申请陪玩";
        public const string GAINWAY_CHALLENGE = "发起挑战";
        public const string GAINWAY_ACCEPT = "接受挑战";
        public const string GAINWAY_REJECT = "认怂金";
        public const string GAINWAY_BACK = "归还挑战金";
        public const string GAINWAY_CHALLENGE_SUCCESS = "挑战成功";
        public const string GAINWAY_CHALLENGE_FAIL = "挑战失败";
        #endregion

        #region 定义获取或消费文字
        public const string PAY_IN = "获得虚拟币：";
        public const string PAY_OUT = "消费虚拟币：";
        #endregion

        #region 定义状态
        public const string MONEYSTATE_YES = "正常记录";
        public const string SIGN_YES = "今日已签到";
        #endregion

        #region 定义竞猜结果
        public const string QUIZ_WIN = "获胜";
        public const string QUIZ_LOSE = "失败";
        public const string QUIZ_SUPPORT = "支持";

        #endregion
    }
}
