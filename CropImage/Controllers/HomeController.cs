using CropImage.Models;
using CropImage.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CropImage.Controllers
{
    public class HomeController : Controller
    {
        private DataContext db = new DataContext();
        public ActionResult Index()
        {
            ViewBag.Image = "/Uploads/Images/Mau1.jpg";
            #region drop
            var listDau = db.Daus;
            ViewBag.Dau = new SelectList(listDau, "Code", "Name");
            var listLoaiTu = db.LoaiTus;
            ViewBag.LoaiTu = new SelectList(listLoaiTu, "Code", "Name");
            #endregion
            return View();
        }

        public ActionResult CropCau(ImageCroped model, long idImage)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //  if(string.IsNullOrEmpty(idImage)) Json(new ExecuteResult() { Isok = false, Data = null, Message = "Hình ảnh không tồn tại trên hệ thống" });
                    var croped = new ImageCroped();
                    croped = model;
                    croped.ImageId = idImage;
                    db.ImageCropeds.Add(croped);
                    db.SaveChanges();
                    return Json( new ExecuteResult() { Isok = true, Data = 1, Message = "Saved" });
                }
                catch (Exception ex)
                {
                    return Json(new ExecuteResult() { Isok = false, Data = null, Message = ex.Message });
                }
            }
            return Json(new ExecuteResult() { Isok = false, Data = null, Message = "Hãy nhập đủ thông tin" });

        }
    }
}