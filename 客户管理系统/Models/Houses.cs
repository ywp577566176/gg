//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace 客户管理系统.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Houses
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Houses()
        {
            this.Floors = new HashSet<Floors>();
        }
    
        public int H_ID { get; set; }
        public string H_Name { get; set; }
        public string H_Remark { get; set; }
        public Nullable<byte> H_MaleOrFemale { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Floors> Floors { get; set; }
    }
}
