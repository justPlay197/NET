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
    
    public partial class bookDetail
    {
        public bookDetail()
        {
            this.book = new HashSet<book>();
        }
    
        public string book_name { get; set; }
        public string author { get; set; }
        public string book_edition { get; set; }
        public string pulisher { get; set; }
        public System.DateTime publish_date { get; set; }
        public Nullable<decimal> price { get; set; }
        public Nullable<int> count { get; set; }
    
        public virtual ICollection<book> book { get; set; }
    }
}