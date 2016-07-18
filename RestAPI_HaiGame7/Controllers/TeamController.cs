/******************************************************************************

** author:zihai

** create date:2016-02-18

** update date:2016-02-18

** description : 战队restful API，
                 提供涉及到战队的服务。

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
    /// 战队中心restful API，提供涉及到用户的服务。
    /// </summary>
    [AccessTokenFilter]
    [ExceptionFilter]
    public class TeamController : ApiController
    {
        //初始化Response信息
        HttpResponseMessage returnResult = new HttpResponseMessage();
        //初始化返回结果
        string jsonResult;

        #region 我的默认战队
        /// <summary>
        /// 我的默认战队
        /// </summary>
        /// <param name="team">
        /// 参数实例：{CreatUserID:64}
        /// </param>
        /// <returns>
        /// 返回值实例：[{"MessageCode":0,"Message":null},
        /// {"Creater":64,"TeamName":"訾1","TeamLogo":"http://images.haigame7.com/logo/20160215144709XXKqu4W0Z5j3PxEIK0zW6uUR3LY=.png","TeamDescription":"qqq","TeamType":"DOTA2","FightScore":0,"Asset":0,"IsDeault":0,"WinCount":null,"LoseCount":null,"FollowCount":null,"Role":"teamcreater","CreateTime":"2016-10-01/"}]
        /// </returns>
        [HttpPost]
        public HttpResponseMessage MyTeam([FromBody] SimpleTeamModel team)
        {
            TeamLogic teamLogic = new TeamLogic();
            jsonResult = teamLogic.MyTeam(team);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region 我的所有战队
        /// <summary>
        /// 我的所有战队
        /// </summary>
        /// <param name="team">
        /// 参数实例：{CreatUserID:64}
        /// </param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage MyAllTeam([FromBody] SimpleTeamModel team)
        {
            TeamLogic teamLogic = new TeamLogic();
            jsonResult = teamLogic.MyAllTeam(team);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region 根据战队ID获取战队信息
        /// <summary>
        /// 根据战队ID获取战队信息
        /// </summary>
        /// <param name="team">
        /// 参数实例：{TeamID:146}
        /// </param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetTeambyID([FromBody]  TeamParameterModel team)
        {
            TeamLogic teamLogic = new TeamLogic();
            jsonResult = teamLogic.GetTeambyID(team);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region 设置默认战队
        /// <summary>
        /// 设置默认战队
        /// </summary>
        /// <param name="team">
        /// 参数实例：{CreatUserID:65,TeamName:"氦7"}
        /// </param>
        /// <returns>
        /// 设置成功：{"MessageCode":0,"Message":""}
        /// </returns>
        [HttpPost]
        public HttpResponseMessage SetDefaultTeam([FromBody]  SimpleTeamModel team)
        {
            TeamLogic teamLogic = new TeamLogic();
            jsonResult = teamLogic.SetDefaultTeam(team);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region 创建战队
        /// <summary>
        /// 创建战队
        /// </summary>
        /// <param name="team">
        /// 参数实例：{CreatUserID:65,TeamName:"氦7",TeamLogo:"图片url",TeamType:"DOTA2"}
        /// </param>
        /// <returns>
        /// 创建成功：{"MessageCode":0,"Message":""}
        /// 战队名称已存在：{"MessageCode":20001,"Message":"team exist"}
        /// 无权创建：{"MessageCode":20002,"Message":"you are teamuser"}
        /// </returns>
        [HttpPost]
        public HttpResponseMessage Create([FromBody]  SimpleTeamModel team)
        {
            TeamLogic teamLogic = new TeamLogic();
            jsonResult = teamLogic.Create(team);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region 更新战队信息
        /// <summary>
        /// 更新战队信息
        /// </summary>
        /// <param name="para">
        /// 参数实例：{TeamID:65,TeamName:"氦7",TeamLogo:"Base64字符",TeamDescription:"XXXX"}
        /// </param>
        /// <returns>
        /// 更新成功：[{"MessageCode":0,"Message":""}]
        /// 战队名被占用：[{"MessageCode":200001,"Message":"team exist"}]
        /// </returns>
        [HttpPost]
        public HttpResponseMessage Update([FromBody]  SimpleTeam2Model para)
        {
            TeamLogic teamLogic = new TeamLogic();
            jsonResult = teamLogic.Update(para);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region 解散战队
        /// <summary>
        /// 解散战队
        /// </summary>
        /// <param name="team">
        /// {"CreatUserID":61,"TeamName":"ABC"}
        /// </param>
        /// <returns>
        /// 解散成功：{"MessageCode":0,"Message":""}
        /// 无权解散：{"MessageCode":20009,"Message":"you cannot delete team"}
        /// </returns>
        [HttpPost]
        public HttpResponseMessage Delete([FromBody]  SimpleTeamModel team)
        {
            TeamLogic teamLogic = new TeamLogic();
            jsonResult = teamLogic.Delete(team);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region 获取战队列表
        /// <summary>
        /// 根据不同条件，获取战队列表
        /// </summary>
        /// <param name="para">
        /// type：1.createdate 注册日期 2.userfightscore 个人战斗力匹配 3.teamfightscore 战队战斗力匹配
        /// 参数实例：{"createUserID":111,"Type":"createdate","Sort":"desc","StartPage":1,"PageCount":10}
        /// </param>
        /// <returns>
        /// 返回值实例：[{"MessageCode":0,"Message":null},
        /// [{"Creater":92,"TeamName":"氦7通天塔","TeamLogo":"http://images.haigame7.com/logo/20160308160716XXKqu4W0Z5j3PxEIK0zW6uUR3LY=.png","TeamDescription":"GTMA","TeamType":"DOTA2","FightScore":0,"Asset":0,"IsDeault":0,"WinCount":0,"LoseCount":0,"FollowCount":1,"Role":null,"CreateTime":"2016-03-08"},
        /// {"Creater":65,"TeamName":"孟庆丰","TeamLogo":"http://images.haigame7.com/logo/20160308160601XXKqu4W0Z5j3PxEIK0zW6uUR3LY=.png","TeamDescription":"123","TeamType":"DOTA2","FightScore":0,"Asset":0,"IsDeault":1,"WinCount":0,"LoseCount":0,"FollowCount":0,"Role":null,"CreateTime":"2016-03-08"},
        /// {"Creater":65,"TeamName":"123","TeamLogo":"http://images.haigame7.com/logo/20160308160540XXKqu4W0Z5j3PxEIK0zW6uUR3LY=.png","TeamDescription":"123","TeamType":"DOTA2","FightScore":0,"Asset":0,"IsDeault":1,"WinCount":0,"LoseCount":0,"FollowCount":0,"Role":null,"CreateTime":"2016-03-08"},
        /// {"Creater":66,"TeamName":"平平一","TeamLogo":"http://images.haigame7.com/logo/20160229170559XXKqu4W0Z5j3PxEIK0zW6uUR3LY=.png","TeamDescription":"栽植有东西","TeamType":"DOTA2","FightScore":0,"Asset":0,"IsDeault":1,"WinCount":0,"LoseCount":0,"FollowCount":0,"Role":null,"CreateTime":"2016-02-29"},
        /// {"Creater":65,"TeamName":"bugbug","TeamLogo":"http://images.haigame7.com/logo/20160229170507XXKqu4W0Z5j3PxEIK0zW6uUR3LY=.png","TeamDescription":"DebugDebugDebugDebug","TeamType":"DOTA2","FightScore":0,"Asset":0,"IsDeault":1,"WinCount":0,"LoseCount":0,"FollowCount":0,"Role":null,"CreateTime":"2016-02-29"}]]
        /// </returns>
        [HttpPost]
        public HttpResponseMessage TeamList([FromBody] TeamListParameterModel para)
        {
            TeamLogic teamLogic = new TeamLogic();
            jsonResult = teamLogic.TeamList(para);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region 我的申请列表
        /// <summary>
        /// 我的申请列表
        /// </summary>
        /// <param name="para">
        /// 参数实例：{UserID:64,StartPage:1,PageCount:5}
        /// </param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage MyApplyTeamList([FromBody] ApplyTeamParameterModel para)
        {
            TeamLogic teamLogic = new TeamLogic();
            jsonResult = teamLogic.ApplyTeamList(para);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region 我的受邀列表
        /// <summary>
        /// 我的受邀列表
        /// </summary>
        /// <param name="para">
        /// 参数实例：{UserID:64,StartPage:1,PageCount:5}
        /// </param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage MyInvitedTeamList([FromBody] ApplyTeamParameterModel para)
        {
            TeamLogic teamLogic = new TeamLogic();
            jsonResult = teamLogic.InvitedTeamList(para);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region 申请加入
        /// <summary>
        /// 申请加入
        /// </summary>
        /// <param name="para">
        /// 参数实例：{UserID:64,TeamID:146,StartPage:1,PageCount:5}
        /// </param>
        /// <returns>
        /// 加入成功：{"MessageCode":0,"Message":""}
        /// 已加入其它战队：{"MessageCode":20006,"Message":"already join other team"}
        /// 已向该战队发出过申请：{"MessageCode":20007,"Message":"already apply this team"}
        /// </returns>
        [HttpPost]
        public HttpResponseMessage ApplyTeam([FromBody] ApplyTeamParameterModel para)
        {
            TeamLogic teamLogic = new TeamLogic();
            jsonResult = teamLogic.ApplyTeam(para);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region 招募信息列表
        /// <summary>
        /// 招募信息列表
        /// </summary>
        /// <param name="para">
        ///  参数实例：{UserID:64,TeamID:146,StartPage:1,PageCount:5}
        /// </param>
        /// <returns>
        /// </returns>
        [HttpPost]
        public HttpResponseMessage RecruitList([FromBody] ApplyTeamParameterModel para)
        {
            TeamLogic teamLogic = new TeamLogic();
            jsonResult = teamLogic.RecruitList(para);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region 发布招募
        /// <summary>
        /// 发布招募
        /// </summary>
        /// <param name="para">
        /// 参数实例：{TeamID:146,Content:"招募信息"}
        /// </param>
        /// <returns>
        /// </returns>
        [HttpPost]
        public HttpResponseMessage SendRecruit([FromBody] RecruitParameterModel para)
        {
            TeamLogic teamLogic = new TeamLogic();
            jsonResult = teamLogic.SendRecruit(para);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region 邀请队员
        /// <summary>
        /// 邀请队员
        /// </summary>
        /// <param name="para">
        /// 参数实例：{TeamID:146,UserID:64}
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        [HttpPost]
        public HttpResponseMessage InviteUser([FromBody] InviteUserParameterModel para)
        {
            TeamLogic teamLogic = new TeamLogic();
            jsonResult = teamLogic.InviteUser(para);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region 发出邀请列表
        /// <summary>
        /// 发出邀请列表
        /// </summary>
        /// <param name="para">
        /// 参数实例：{TeamID:146,StartPage:1,PageCount:5}
        /// </param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage InvitedUserList([FromBody] ApplyTeamParameterModel para)
        {
            TeamLogic teamLogic = new TeamLogic();
            jsonResult = teamLogic.InvitedUserList(para);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region 申请加入列表
        /// <summary>
        /// 申请加入列表
        /// </summary>
        /// <param name="para">
        /// 参数实例：{TeamID:146,StartPage:1,PageCount:5}
        /// </param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage ApplyUserList([FromBody] ApplyTeamParameterModel para)
        {
            TeamLogic teamLogic = new TeamLogic();
            jsonResult = teamLogic.ApplyUserList(para);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region 我的受邀操作【同意or拒绝】
        /// <summary>
        /// 我的受邀操作【同意or拒绝】
        /// </summary>
        /// <param name="para">
        /// ISOK:0 同意，1 拒绝
        /// 参数实例：{TeamID:146,UserID:1,MessageID:5,ISOK:0}
        /// </param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage HandleMyInvited([FromBody] ApplyTeamParameter2Model para)
        {
            TeamLogic teamLogic = new TeamLogic();
            jsonResult = teamLogic.HandleMyInvited(para);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region 申请加入操作【同意or拒绝】
        /// <summary>
        /// 申请加入操作【同意or拒绝】
        /// </summary>
        /// <param name="para">
        /// ISOK:0 同意，1 拒绝
        /// 参数实例：{TeamID:146,UserID:1,MessageID:5,ISOK:0}
        /// </param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage HandleMyApply([FromBody] ApplyTeamParameter2Model para)
        {
            TeamLogic teamLogic = new TeamLogic();
            jsonResult = teamLogic.HandleMyApply(para);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

        #region 移出战队
        /// <summary>
        /// 移出战队
        /// </summary>
        /// <param name="para">
        /// 参数实例：{TeamID:146,UserID:1}
        /// </param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage RemoveUser([FromBody] ApplyTeamParameter2Model para)
        {
            TeamLogic teamLogic = new TeamLogic();
            jsonResult = teamLogic.RemoveUser(para);

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
        #endregion

    }
}
