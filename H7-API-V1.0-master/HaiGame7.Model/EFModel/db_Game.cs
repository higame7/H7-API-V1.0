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
    
    public partial class db_Game
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public db_Game()
        {
            this.db_FightResult = new HashSet<db_FightResult>();
            this.db_GameBoBo = new HashSet<db_GameBoBo>();
            this.db_GameRecord = new HashSet<db_GameRecord>();
        }
    
        public int GameID { get; set; }
        public string GameName { get; set; }
        public string ShowPicture { get; set; }
        public string Introduce { get; set; }
        public Nullable<System.DateTime> HoldTime { get; set; }
        public string HoldAddress { get; set; }
        public string HoldRule { get; set; }
        public Nullable<int> State { get; set; }
        public byte[] SysTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<db_FightResult> db_FightResult { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<db_GameBoBo> db_GameBoBo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<db_GameRecord> db_GameRecord { get; set; }
    }
}
