using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaiGame7.Model.MyModel
{
    public class GuessParameterModel
    {
        public int UserID { get; set; }
        public int GuessID { get; set; }
        public int StartPage { get; set; }
        public int PageCount { get; set; }
    }
}
