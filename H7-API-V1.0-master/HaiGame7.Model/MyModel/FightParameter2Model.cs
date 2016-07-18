namespace HaiGame7.Model.MyModel
{
    public class FightParameter2Model
    {
        public int UserID { get; set; }
        public int DateID { get; set; }
        public int Money { get; set; }
        public string SFightAddress { get; set; }//挑战方输入房间号
        public string EFightAddress { get; set; }//应战方输入房间号
        public string SFightPic { get; set; }//挑战方截图
        public string EFightPic { get; set; }//应战方截图
    }
}
