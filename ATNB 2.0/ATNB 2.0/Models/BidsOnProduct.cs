//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ATNB_2._0.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    [MetadataType(typeof(BidsOnProduct_MetaData))]

    public partial class BidsOnProduct
    {
        public string userId { get; set; }
        public Nullable<int> productId { get; set; }
        public string comment { get; set; }
        public Nullable<System.DateTime> dateTime { get; set; }
        public int BidId { get; set; }
    
        public virtual Product Product { get; set; }
    }
}
