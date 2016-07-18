/******************************************************************************

** author:zihai

** create date:2016-02-18

** update date:2016-02-18

** description : 对访问restful API的用户进行异常过来，
                 记录log，并返回错误提示

******************************************************************************/

using HaiGame7.BLL.Enum;
using HaiGame7.Model.MyModel;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Http.Filters;
using System.Web.Script.Serialization;

namespace HaiGame7.RestAPI.Filter
{
    /// <summary>
    /// 异常处理Filter
    /// </summary>
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// 重写OnException，记录错误log，返回系统错误
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            string result = "";
            HttpResponseMessage returnResult=new HttpResponseMessage();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            MessageModel message = new MessageModel();
            HashSet<object> returnHash = new HashSet<object>();
            //记录系统错误log
            string errMessage = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ":"+
                                actionExecutedContext.ActionContext.ControllerContext.Controller.ToString()+
                                actionExecutedContext.ActionContext.ToString()+
                                actionExecutedContext.Exception.ToString();
            logger.Error(errMessage);

            //返回系统错误提示
            message.MessageCode = MESSAGE.SYSERR_CODE;
            message.Message = MESSAGE.SYSERR;
            returnHash.Add(message);
            result = jss.Serialize(returnHash);
            returnResult.Content = new StringContent(result, Encoding.UTF8, "application/json");

            actionExecutedContext.Response = returnResult;
        }
    }
}
