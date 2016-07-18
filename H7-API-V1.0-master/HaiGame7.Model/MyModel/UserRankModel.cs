using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaiGame7.Model.MyModel
{
    public class UserRankModel
    {
        public int UserID { get; set; }//用户ID
        public string NickName { get; set; }//昵称
        public string UserPicture { get; set; }//头像
        public string Hobby { get; set; }//签名
        public string GameID { get; set; }//游戏ID
        public string GameGrade { get; set; }//大神系数
        public string GamePower { get; set; }//战斗力
        public int Asset { get; set; }//氦气
        public string Sex { get; set; }//性别
        public string RegDate { get; set; }//注册时间
        public List<HeroModel> HeroImage { get; set; }//擅长英雄
        public string Address { get; set; }//地区
    }
}
