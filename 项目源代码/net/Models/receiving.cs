//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace net.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class receiving
    {
        public receiving()
        {
            this.bookOrder = new HashSet<bookOrder>();
            this.users = new HashSet<users>();
        }
    
        public int rece_id { get; set; }
        public string name { get; set; }
        public int tele_number { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string street { get; set; }
        public string address { get; set; }
    
        public virtual ICollection<bookOrder> bookOrder { get; set; }
        public virtual ICollection<users> users { get; set; }
    }
}
