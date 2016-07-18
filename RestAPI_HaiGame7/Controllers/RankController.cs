/******************************************************************************

** author:zihai

** create date:2016-02-18

** update date:2016-02-18

** description : 排名restful API，
                 提供涉及到排名的服务。

******************************************************************************/

using System.Net.Http;
using System.Web.Http;
using HaiGame7.BLL;
using HaiGame7.Model.MyModel;
using System.Text;
using HaiGame7.RestAPI.Filter;

namespace HaiGame7.RestAPI.Controllers
{
    /// <summary>
    /// 排行restful API，提供涉及到排行的服务。
    /// </summary>
    [AccessTokenFilter]
    [ExceptionFilter]
    public class RankController : ApiController
    {
        //初始化Response信息
        HttpResponseMessage returnResult = new HttpResponseMessage();
        //初始化返回结果
        string jsonResult;

        #region UserRank 个人排行
        /// <summary>
        /// 个人排行
        /// </summary>
        /// <param name="rank">
        /// 参数说明：
        /// ranktype：排名类型 1.大神系数（GameGrade）2.氦气（Asset）3.战斗力（GamePower）
        /// ranksort：排序 1.倒序（desc） 2.正序（asc）
        /// startpage：页码
        /// pagecount：每页记录数
        /// 参数实例：{"ranktype":"GamePower","ranksort":"desc","startpage":1,"pagecount":5}
        /// </param>
        /// <returns>
        /// 返回值实例：[{"MessageCode":0,"Message":""},
        /// [{"NickName":null,"UserPicture":"","Hobby":null,"GameID":"106863163","GameGrade":"","GamePower":"0","Asset":774},
        /// {"NickName":null,"UserPicture":"","Hobby":null,"GameID":"173032376","GameGrade":"","GamePower":"0","Asset":898},
        /// {"NickName":null,"UserPicture":"","Hobby":null,"GameID":"89371588","GameGrade":"","GamePower":"0","Asset":1430},
        /// {"NickName":null,"UserPicture":"","Hobby":null,"GameID":"10960902","GameGrade":"","GamePower":"1200","Asset":2622},
        /// {"NickName":null,"UserPicture":"","Hobby":null,"GameID":"242950672","GameGrade":"","GamePower":"1804","Asset":10}]]
        /// </returns>
        [HttpPost]
        public HttpResponseMessage UserRank([FromBody] RankParameterModel rank)
        {
            RankLogic rankLogic = new RankLogic();
            jsonResult = rankLogic.UserRank(rank);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region TeamRank 战队排行
        /// <summary>
        /// 战队排行
        /// </summary>
        /// <param name="rank">
        /// 参数说明：
        /// ranktype：排名类型 1.热度（HotScore）2.氦气（Asset）3.战斗力（FightScore）
        /// ranksort：排序 1.倒序（desc） 2.正序（asc）
        /// startpage：页码
        /// pagecount：每页记录数
        /// 参数实例：{"ranktype":"HotScore","ranksort":"desc","startpage":1,"pagecount":5}
        /// </param>
        /// <returns>
        /// 返回值实例：[{"MessageCode":0,"Message":""},
        /// [{"TeamName":"氦7硬邦邦","TeamPicture":"","TeamDescription":"我们是一群很硬的男人","HotScore":0,"FightScore":0,"Asset":0},
        /// {"TeamName":"訾1","TeamPicture":"","TeamDescription":"qqq","HotScore":0,"FightScore":0,"Asset":0},
        /// {"TeamName":"高1","TeamPicture":"","TeamDescription":"11","HotScore":6,"FightScore":0,"Asset":0},
        /// {"TeamName":"奶粉哪去了","TeamPicture":"","TeamDescription":"专注真正好奶源，选择优质好奶粉","HotScore":3,"FightScore":0,"Asset":0},
        /// {"TeamName":"huhaoran","TeamPicture":"","TeamDescription":"huhaoran","HotScore":6,"FightScore":0,"Asset":0}]]
        /// </returns>
        [HttpPost]
        public HttpResponseMessage TeamRank([FromBody] RankParameterModel rank)
        {
            RankLogic rankLogic = new RankLogic();
            jsonResult = rankLogic.TeamRank(rank);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion
    }
}