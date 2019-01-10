using Common.CustomAttributes;
using Entities.Models;
using Entities.Models.SystemManage;
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
    public class Images : Entity
    {
        [Key]
        public long Id { get; set; }
        [Display(Name = "Mã")]
        public string Code { get; set; }
        #region const
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsDeleted { get; set; }
        [Display(Name = "Tên")]
        public string Name { get; set; }
        #endregion
        public long? AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual Account Accounts { get; set; }
        public Images()
        {
            CreateDate = DateTime.Now;
        }
        public string Describe()
        {
            return "{id: " + Id + "}";

        }
    }
}