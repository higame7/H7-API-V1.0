using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaiGame7.Model.MyModel
{
    public class TeamListParameterModel
    {
        public int createUserID { get; set; }
        public string Type { get; set; }
        public int TeamFightScore { get; set; }
        public int UserFightScore { get; set; }
        public string Sort { get; set; }
        public int StartPage { get; set; }
        public int PageCount { get; set; }
    }
}
