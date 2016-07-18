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
    
    public partial class db_Partner
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public db_Partner()
        {
            this.db_PartnerTime = new HashSet<db_PartnerTime>();
            this.db_PlayRecordComment = new HashSet<db_PlayRecordComment>();
            this.db_PlayRecordPartner = new HashSet<db_PlayRecordPartner>();
        }
    
        public int PartnerID { get; set; }
        public string Name { get; set; }
        public string CardID { get; set; }
        public string PhoneNumber { get; set; }
        public string GameID { get; set; }
        public string NickName { get; set; }
        public string Description { get; set; }
        public string UserPicture { get; set; }
        public string Catalog { get; set; }
        public string Honor { get; set; }
        public string Grade { get; set; }
        public Nullable<System.DateTime> JoinTime { get; set; }
        public string Label { get; set; }
        public byte[] SysTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<db_PartnerTime> db_PartnerTime { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<db_PlayRecordComment> db_PlayRecordComment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<db_PlayRecordPartner> db_PlayRecordPartner { get; set; }
    }
}