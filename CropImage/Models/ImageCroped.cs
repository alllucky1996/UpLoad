using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CropImage.Models
{
    public class ImageCroped
    {
        [Key]
        public long Id { get; set; }
        public long? ImageId { get; set; }
        [ForeignKey("ImageId")]
        public virtual Image Image { get; set; }
        public string code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string LoaiTu { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public string Lable { get; set; }
        public bool? IsDauCau { get; set; }
        public bool IsOK { get; set; }
        public DateTime CreateDate { get; set; }

        public ImageCroped()
        {
            CreateDate = DateTime.Now;
        }
    }
   
}