using HaiGame7.BLL.Enum;
using HaiGame7.BLL.Logic.Common;
using HaiGame7.Model.EFModel;
using HaiGame7.Model.MyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.Script.Serialization;

namespace HaiGame7.BLL
{
    public class VersionLogic
    {
        #region 当前版本
        public string CurrentVersion()
        {
            string result = "";
            MessageModel message = new MessageModel();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            HashSet<object> returnResult = new HashSet<object>();

            message.Message = WebConfigurationManager.AppSettings["verson"].ToString();
            message.MessageCode = 0;
            returnResult.Add(message);
            result = jss.Serialize(returnResult);
            return result;
        }
        #endregion
    }
}
