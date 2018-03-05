using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ATNB_2._0.Models
{
    public class BidsOnProduct_MetaData
    {
        [Required]
        public string comment { get; set; }
    }
}