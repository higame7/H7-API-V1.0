using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaiGame7.Model.MyModel
{
    public class GuessRecordModel
    {
        public int GuessID { get; set; }
        public int UserID { get; set; }
        public int TeamID { get; set; }
        public int Money { get; set; }
        public Nullable<decimal> Odds { get; set; }
    }
}
