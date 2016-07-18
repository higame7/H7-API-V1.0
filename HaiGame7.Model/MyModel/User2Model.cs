using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaiGame7.Model.MyModel
{
    public class User2Model
    {
        public int MessageID { get; set; }
        public int UserID { get; set; }
        public string PhoneNumber { get; set; }
        public string PassWord { get; set; }
        public string UserWebPicture { get; set; }
        public string UserWebNickName { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string Sex { get; set; }
        public string Birthday { get; set; }
        public string RegDate { get; set; }
        public string Hobby { get; set; }
        public List<HeroModel> HeroImage { get; set; }
        public string SendTime { get; set; }
        public string State { get; set; }
        public int GamePower { get; set; }
        public Nullable<int> Asset { get; set; }
    }
}
