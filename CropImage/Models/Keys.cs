using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CropImage.Models
{
    public class Keys
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; }
        public string Description { get; set; }
    }
  
}