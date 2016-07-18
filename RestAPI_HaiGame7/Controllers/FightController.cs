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

namespace HaiGame7.RestAPI.Controllers
{
    /// <summary>
    /// 约战restful API，提供涉及到约战的服务。
    /// </summary>
    [AccessTokenFilter]
    [ExceptionFilter]
    public class FightController : ApiController
    {
        //初始化Response信息
        HttpResponseMessage returnResult = new HttpResponseMessage();
        //初始化返回结果
        string jsonResult;

        #region 约战动态列表
        /// <summary>
        /// 约战动态列表
        /// </summary>
        /// <param name="rank">
        /// 参数说明：
        /// startpage：页码
        /// pagecount：每页记录数
        /// 参数实例：{"startpage":1,"pagecount":5}
        /// </param>
        /// <returns>
        /// [{"MessageCode":0,"Message":""},
        /// [{"Description":"战队【高1】接受战队【ddddd】的约战","FightTime":"2月22日","FightAsset":50},
        /// {"Description":"战队【潜水泵战队】接受战队【国睡战队】的约战","FightTime":"2月24日","FightAsset":50},
        /// {"Description":"战队【奶粉哪去了】接受战队【潜水泵战队】的约战","FightTime":"2月24日","FightAsset":1},
        /// {"Description":"战队【aaaaaaaaaa】接受战队【国睡战队】的约战","FightTime":"2月24日","FightAsset":50},
        /// {"Description":"战队【通天塔战队】接受战队【ddddd】的约战","FightTime":"2月25日","FightAsset":50},
        /// {"Description":"战队【奶粉哪去了】向战队【通天塔战队】认怂","FightTime":"2月25日","FightAsset":50},
        /// {"Description":"战队【通天塔战队】接受战队【通地塔】的约战","FightTime":"2月26日","FightAsset":50},
        /// {"Description":"战队【huhaoran】向战队【通地塔】认怂","FightTime":"2月26日","FightAsset":2000},
        /// {"Description":"战队【huhaoran】向战队【通地塔】认怂","FightTime":"2月26日","FightAsset":2000},
        /// {"Description":"战队【訾1】接受战队【通天塔战队】的约战","FightTime":"2月26日","FightAsset":50}]]
        /// </returns>
        [HttpPost]
        public HttpResponseMessage AllFightList([FromBody] RankParameterModel rank)
        {
            FightLogic fightLogic = new FightLogic();
            jsonResult = fightLogic.AllFightList(rank);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region 发起约战
        /// <summary>
        /// 发起约战
        /// </summary>
        /// <param name="para">
        /// </param>
        /// <returns>
        /// 约战成功：[{"MessageCode":0,"Message":""}]
        /// 队员不能约战：[{"MessageCode":20004,"Message":"您是队员，不能发起约战"}]
        /// 超出每日限额：[{"MessageCode":20010,"Message":"超出约战限额，每天只可以约战一场"}]
        /// 氦气不足：[{"MessageCode":60001,"Message":"氦气不足"}]
        /// </returns>
        [HttpPost]
        public HttpResponseMessage MakeChallenge([FromBody] ChallengeParameterModel para)
        {
            FightLogic fightLogic = new FightLogic();
            jsonResult = fightLogic.MakeChallenge(para);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region 我的约战
        /// <summary>
        /// 我的约战
        /// </summary>
        /// <param name="fight">
        /// 参数实例：{PhoneNumber:"13439843883",FightType:"Send",StartPage:1,PageCount:10}
        /// </param>
        /// <returns>
        /// 返回实例：[{"MessageCode":0,"Message":""},
        /// [{"DateID":168,"STeamID":125,"ETeamID":124,"STeamName":"通天塔战队","ETeamName":"訾1","Money":50,"FightAddress":null,"CurrentState":"已应战","StateTime":"\/Date(-62135596800000)\/","StateTimeStr":"2016-02-26 13:06:35"}]]
        /// </returns>
        [HttpPost]
        public HttpResponseMessage MyFight([FromBody] FightParameterModel fight)
        {
            FightLogic fightLogic = new FightLogic();
            jsonResult = fightLogic.MyFight(fight);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region 认怂
        /// <summary>
        /// 认怂
        /// </summary>
        /// <param name="fight">
        /// 参数实例：{UserID:61,DateID:111,Money:50}
        /// </param>
        /// <returns>
        /// 返回实例：[{"MessageCode":0,"Message":""}]
        /// </returns>
        [HttpPost]
        public HttpResponseMessage Reject([FromBody] FightParameter2Model fight)
        {
            FightLogic fightLogic = new FightLogic();
            jsonResult = fightLogic.Reject(fight);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region 应战
        /// <summary>
        /// 应战
        /// </summary>
        /// <param name="fight">
        /// 参数实例：{UserID:61,DateID:111,Money:50}
        /// </param>
        /// <returns>
        /// 返回实例：[{"MessageCode":0,"Message":""}]
        /// 氦气不足：[{"MessageCode":60001,"Message":"氦气不足"}]
        /// </returns>
        [HttpPost]
        public HttpResponseMessage Accept([FromBody] FightParameter2Model fight)
        {
            FightLogic fightLogic = new FightLogic();
            jsonResult = fightLogic.Accept(fight);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region 上传比赛ID
        /// <summary>
        /// 上传比赛ID
        /// </summary>
        /// <param name="fight">
        /// 参数实例：{DateID:61,SFightAddress:"11111111"}
        /// </param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UpdateGameID([FromBody] FightParameter2Model fight)
        {
            FightLogic fightLogic = new FightLogic();
            jsonResult = fightLogic.UpdateGameID(fight);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

    }
}
