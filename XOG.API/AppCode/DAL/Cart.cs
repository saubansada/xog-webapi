//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace XOG.AppCode.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cart
    {
        public long Id { get; set; }
        public string AddedByUserId { get; set; }
        public Nullable<long> ProductVariantId { get; set; }
        public Nullable<short> Quantity { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual ProductVariant ProductVariant { get; set; }
    }
}
