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
    
    public partial class TrainClasses
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TrainClasses()
        {
            this.TrainClassRooms = new HashSet<TrainClassRooms>();
            this.TrainClassStudents = new HashSet<TrainClassStudents>();
        }
    
        public int TC_ID { get; set; }
        public string TC_Name { get; set; }
        public string TC_Grade { get; set; }
        public System.DateTime TC_BeginTime { get; set; }
        public System.DateTime TC_EndTime { get; set; }
        public byte TC_MaxAmount { get; set; }
        public Nullable<byte> TC_MaleAmount { get; set; }
        public Nullable<byte> TC_FemaleAmount { get; set; }
        public Nullable<byte> TC_Period { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TrainClassRooms> TrainClassRooms { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TrainClassStudents> TrainClassStudents { get; set; }
    }
}
