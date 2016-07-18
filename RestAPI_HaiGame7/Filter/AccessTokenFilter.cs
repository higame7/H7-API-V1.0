/******************************************************************************

** author:zihai

** create date:2016-02-18

** update date:2016-02-18

** description : 对访问restful API的用户进行合法性检验，
                 每个请求都需要提供accesstoken

******************************************************************************/

using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using HaiGame7.Model.MyModel;
using HaiGame7.BLL.Enum;
using HaiGame7.BLL.Logic.Common;
using System.Web.Script.Serialization;
using System.Collections.Generic;

namespace HaiGame7.RestAPI.Filter
{
    class AccessTokenFilter: ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string json="";
            HttpResponseMessage returnResult;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            MessageModel message = new MessageModel();
            HashSet<object> returnHash = new HashSet<object>();

            try
            {
                //校验AccessToken
                string requestQuery = actionContext.RequestContext.Url.Request.RequestUri.Query;

                if (requestQuery.IndexOf("accesstoken")!=-1)
                {
                    requestQuery = requestQuery.Substring(1);
                    int startIndex = requestQuery.IndexOf("=");
                    string accesstoken = requestQuery.Substring(startIndex + 1);
                    //验证accesstoken合法性
                    bool isValidAccess = true;
                    isValidAccess=Common.IsValid(accesstoken);
                    if (isValidAccess==true)
                    {
                        return;
                    }
                    else
                    {
                        message.MessageCode = MESSAGE.ACCESSTOKENINVALID_CODE;
                        message.Message = MESSAGE.ACCESSTOKENINVALID;
                    }
                }
                else
                {
                    message.MessageCode = MESSAGE.NOACCESSTOKEN_CODE;
                    message.Message = MESSAGE.NOACCESSTOKEN;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                message.MessageCode = MESSAGE.SYSERR_CODE;
                message.Message = MESSAGE.SYSERR;
            }
            returnHash.Add(message);
            json = jss.Serialize(returnHash);
            returnResult = new HttpResponseMessage { Content = new StringContent(json, 
                                                    System.Text.Encoding.UTF8, "application/json") };
            actionContext.Response = returnResult;
        }
    }
}