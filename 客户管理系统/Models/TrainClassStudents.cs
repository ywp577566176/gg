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
    
    public partial class TrainClassStudents
    {
        public int Student_ID { get; set; }
        public Nullable<int> TC_ID { get; set; }
        public Nullable<int> R_ID { get; set; }
        public Nullable<bool> Student_IsHasAllotRoom { get; set; }
    
        public virtual Rooms Rooms { get; set; }
        public virtual Students Students { get; set; }
        public virtual TrainClasses TrainClasses { get; set; }
    }
}
