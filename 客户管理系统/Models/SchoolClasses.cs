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
    
    public partial class SchoolClasses
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SchoolClasses()
        {
            this.Students = new HashSet<Students>();
        }
    
        public int SC_ID { get; set; }
        public Nullable<int> School_ID { get; set; }
        public Nullable<int> Specialty_ID { get; set; }
        public string SC_Name { get; set; }
        public Nullable<int> SC_FemaleAmount { get; set; }
        public Nullable<int> SC_MaleAmount { get; set; }
        public string SC_Teacher { get; set; }
        public string SC_Supervisor { get; set; }
    
        public virtual Schools Schools { get; set; }
        public virtual Specialties Specialties { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Students> Students { get; set; }
    }
}
