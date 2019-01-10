using Common.CustomAttributes;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    //[Table("DonVi")]
    [DropDown(ValueField = "Id", TextField = "Name")]
    public class Crops : Entity
    {
        [Key]
        public long Id { get; set; }
        [Display(Name = "Mã")]
        public string Code { get; set; }
        #region const
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsDeleted { get; set; }
        #endregion
        [Display(Name ="Mức xám")]
        public int? Graylevel { get; set; }
        [Display(Name = "Tọa độ X")]
        public int? X { get; set; }
        [Display(Name = "Tọa độ Y")]
        public int? Y { get; set; }
        [Display(Name = "Rộng ")]
        public int? Width { get; set; }
        [Display(Name = "Cao")]
        public int? Height { get; set; }
        [Display(Name = "Nhãn")]
        public int? Lable { get; set; }

        [Display(Name = "Crop tự hình")]
        public long? ImageID { get; set; }
        [ForeignKey("ImageID")]
        public virtual Images Images { get; set; }
        public Crops()
        {
            CreateDate = DateTime.Now;
        }
        public string Describe()
        {
            return "{crops--id: " + Id + "}";

        }
    }
}