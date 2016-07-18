using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaiGame7.Model.MyModel
{
    public class TeamUserModel
    {
        public int UserID { get; set; }
        public Nullable<int> Asset { get; set; }
        public int FightScore { get; set; }
        public string UserPicture { get; set; }
        public string RegDate { get; set; }
        public string PhoneNumber { get; set; }
        public string UserNickName { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string Sex { get; set; }
        public string Birthday { get; set; }
        public string Hobby { get; set; }
        public List<HeroModel> HeroImage { get; set; }
    }
}
