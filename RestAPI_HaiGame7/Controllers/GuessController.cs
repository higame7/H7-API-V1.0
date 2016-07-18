/******************************************************************************

** author:zihai

** create date:2016-02-18

** update date:2016-02-18

** description : 约战restful API，
                 提供涉及到约战的服务。

******************************************************************************/

using HaiGame7.BLL;
using HaiGame7.Model.MyModel;
using HaiGame7.RestAPI.Filter;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace RestAPI_HaiGame7.Controllers
{
    /// <summary>
    /// 竞猜restful API，提供涉及到竞猜的服务。
    /// </summary>
    [AccessTokenFilter]
    [ExceptionFilter]
    public class GuessController : ApiController
    {
        //初始化Response信息
        HttpResponseMessage returnResult = new HttpResponseMessage();
        //初始化返回结果
        string jsonResult;

        #region 竞猜列表
        /// <summary>
        /// 竞猜列表
        /// </summary>
        /// <returns>
        /// 无需传参，返回值实例：[{"MessageCode":0,"Message":null},
        /// [{"GuessID":1,"GuessName":null,"STeamID":146,"STeamName":"伐木高","ETeamID":141,"ETeamName":"bugbug","MatchTime":null,"GuessType":"猜胜负","STeamOdds":1.30,"ETeamOdds":2.10,"EndTime":null,"AllMoney":0,"AllUser":0}]]
        /// </returns>
        [HttpPost]
        public HttpResponseMessage GuessList()
        {
            GuessLogic guessLogic = new GuessLogic();
            jsonResult = guessLogic.GuessList();

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region 下注
        /// <summary>
        /// 下注
        /// </summary>
        /// <param name="guess">
        /// 传参：{GuessID:"1",UserID:"64",TeamID:"146",Money:"111",Odds:"1.5"}
        /// </param>
        /// <returns>
        /// 正常：[{"MessageCode":0,"Message":""}]
        /// 氦气不足：[{"MessageCode":60001,"Message":"氦气不足"}]
        /// </returns>
        [HttpPost]
        public HttpResponseMessage Bet([FromBody] GuessRecordModel guess)
        {
            GuessLogic guessLogic = new GuessLogic();
            jsonResult = guessLogic.Bet(guess);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region 我的竞猜列表
        /// <summary>
        /// 我的竞猜列表
        /// </summary>
        /// <param name="guess">
        /// GuessID可以不传入，如果不传入是该用户下的竞猜记录
        /// 如果传入，是该用户某场竞猜下的记录。参数实例：{userid:64,guessid:1,startpage:1,pagecount:5}
        /// </param>
        /// <returns>
        /// [{"MessageCode":0,"Message":""},
        /// [{"EndTime":null,"MatchName":"精英小组赛一轮","STeamID":146,"STeamName":"伐木高","STeamLogo":"http://images.haigame7.com/logo/20160323173550XXKqu4W0Z5j3PxEIK0zW6uUR3LY=.png","ETeamID":141,"ETeamName":"bugbug","ETeamLogo":null,"BetTeamID":null,"BetTeamName":null,"BetTeamLogo":null,"GuessTime":"2016-01-01 01:01:01","Odds":0.51,"BetMoney":111},
        /// {"EndTime":null,"MatchName":"精英小组赛一轮","STeamID":146,"STeamName":"伐木高","STeamLogo":"http://images.haigame7.com/logo/20160323173550XXKqu4W0Z5j3PxEIK0zW6uUR3LY=.png","ETeamID":141,"ETeamName":"bugbug","ETeamLogo":null,"BetTeamID":null,"BetTeamName":null,"BetTeamLogo":null,"GuessTime":"2016-03-26 22:00:38","Odds":2.10,"BetMoney":0},
        /// {"EndTime":null,"MatchName":"精英小组赛一轮","STeamID":146,"STeamName":"伐木高","STeamLogo":"http://images.haigame7.com/logo/20160323173550XXKqu4W0Z5j3PxEIK0zW6uUR3LY=.png","ETeamID":141,"ETeamName":"bugbug","ETeamLogo":null,"BetTeamID":null,"BetTeamName":null,"BetTeamLogo":null,"GuessTime":"2016-03-26 22:00:59","Odds":2.10,"BetMoney":0},
        /// {"EndTime":null,"MatchName":"精英小组赛一轮","STeamID":146,"STeamName":"伐木高","STeamLogo":"http://images.haigame7.com/logo/20160323173550XXKqu4W0Z5j3PxEIK0zW6uUR3LY=.png","ETeamID":141,"ETeamName":"bugbug","ETeamLogo":null,"BetTeamID":116,"BetTeamName":"奶粉哪去了","BetTeamLogo":"http://images.haigame7.com/logo/20160127164142XXKqu4W0Z5j3PxEIK0zW6uUR3LY=.png","GuessTime":"2016-03-26 22:06:14","Odds":2.10,"BetMoney":0},
        /// {"EndTime":null,"MatchName":"精英小组赛一轮","STeamID":146,"STeamName":"伐木高","STeamLogo":"http://images.haigame7.com/logo/20160323173550XXKqu4W0Z5j3PxEIK0zW6uUR3LY=.png","ETeamID":141,"ETeamName":"bugbug","ETeamLogo":null,"BetTeamID":117,"BetTeamName":"type","BetTeamLogo":"http://images.haigame7.com/logo/20160127164454XXKqu4W0Z5j3PxEIK0zW6uUR3LY=.png","GuessTime":"2016-03-26 22:06:14","Odds":2.10,"BetMoney":0}]]
        /// </returns>
        [HttpPost]
        public HttpResponseMessage MyGuessList([FromBody] GuessParameterModel guess)
        {
            GuessLogic guessLogic = new GuessLogic();
            jsonResult = guessLogic.MyGuessList(guess);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion
    }
}
