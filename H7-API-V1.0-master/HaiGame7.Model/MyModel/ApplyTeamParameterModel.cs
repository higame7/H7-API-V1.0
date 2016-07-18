using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaiGame7.Model.MyModel
{
    public class ApplyTeamParameterModel
    {
        public int UserID { get; set; }
        public int TeamID { get; set; }
        public int StartPage { get; set; }
        public int PageCount { get; set; }
    }
}
