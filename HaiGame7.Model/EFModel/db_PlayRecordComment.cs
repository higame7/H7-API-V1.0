//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace HaiGame7.Model.EFModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class db_PlayRecordComment
    {
        public int ID { get; set; }
        public Nullable<int> PlayRecordID { get; set; }
        public Nullable<int> PartnerID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string UserComment { get; set; }
        public string UserCommentVerify { get; set; }
        public string PartnerComment { get; set; }
        public string PartnerCommentVerify { get; set; }
        public Nullable<System.DateTime> UserCommentTime { get; set; }
        public Nullable<System.DateTime> PartnerCommentTime { get; set; }
        public byte[] SysTime { get; set; }
    
        public virtual db_Partner db_Partner { get; set; }
        public virtual db_PlayRecord db_PlayRecord { get; set; }
        public virtual db_User db_User { get; set; }
    }
}