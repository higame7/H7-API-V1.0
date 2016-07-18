using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaiGame7.Model.MyModel
{
    public class UserListParameterModel
    {
        public string RankType { get; set; }
        public string RankSort { get; set; }
        public int StartPage { get; set; }
        public int PageCount { get; set; }
    }
}
