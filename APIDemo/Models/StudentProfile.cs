//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace APIDemo.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class StudentProfile
    {
        public System.Guid guid { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Blood { get; set; }
        public Nullable<decimal> Height { get; set; }
        public Nullable<decimal> Weight { get; set; }
        public string Coupon { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}
