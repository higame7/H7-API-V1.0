using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaiGame7.BLL.Enum
{
    public class SMS
    {
        //容联短信API端口
        public const string PORT = "8883";
        //容联短信API地址
        public const string ADDRESS = @"app.cloopen.com";
        //开发者账号
        public const string ACCOUNT_SID = @"aaf98f8950189e9b01505aadc9362abc";
        //开发者token
        public const string ACCOUNT_TOKEN = @"4aa6300b87df44d3bd6f22ea60b200fc";
        //APP_ID
        public const string APP_ID = @"aaf98f8950189e9b01505aafaf312ac7";
        //短信验证码模板号
        public const string TEMPLATE_ID = @"41433";
        //短信验证码有效时间
        public const string TIME_OUT = @"5";
    }
}
