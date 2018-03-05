using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ATNB_2._0.Models
{
    public class Product_MetaData
    {
        
        [Required]
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Product Type")]
        public string Type { get; set; }

        [Required]
        [Range(1 , 9999999999999999999.999999999999999 , ErrorMessage = "Price Must be a positive Number")]
        [Display(Name = "Starting Price")]
        public Nullable<int> OfferPrice { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Product Image")]
        public string image { get; set; }
    }
}