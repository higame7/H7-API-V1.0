using HaiGame7.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace RestAPI_HaiGame7.Controllers
{
    /// <summary>
    /// 版本API
    /// </summary>
    public class VersionController : ApiController
    {
        //初始化Response信息
        HttpResponseMessage returnResult = new HttpResponseMessage();
        //初始化返回结果
        string jsonResult;

        /// <summary>
        /// 当前App版本
        /// </summary>
        /// <returns>
        /// 返回值：{"MessageCode":0,"Message":"版本号"}
        /// </returns>
        [HttpPost]
        public HttpResponseMessage CurrentVersion()
        {
            VersionLogic versionLogic = new VersionLogic();
            jsonResult = versionLogic.CurrentVersion();

            returnResult.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
            return returnResult;
        }
    }
}
