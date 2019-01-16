using CropImage.Commons;
using CropImage.Models;
using CropImage.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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
            ViewBag.idImage = 1;
            #region drop
            var listDau = db.Daus;
            ViewBag.Dau = new SelectList(listDau, "Code", "Name");
            var listLoaiTu = db.LoaiTus;
            ViewBag.LoaiTu = new SelectList(listLoaiTu, "Code", "Name");
            #endregion
            return View();
        }
       

        public async Task<ActionResult> CropCau(ImageCroped model, long idImage)
        {
            if (ModelState.IsValid)
            {
                try
                {
                  if(model.X==model.Y&& model.Width== model.Height&& model.Height== 0&& model.Y==0) Json(new ExecuteResult() { Isok = false, Data = null, Message = "Crop k hợp lệ" });
                    var croped = new ImageCroped();
                    croped = model;
                    croped.ImageId = idImage;
                    db.ImageCropeds.Add(croped);
                    await db.SaveChangesAsync();
                    return Json( new ExecuteResult() { Isok = true, Data = 1, Message = "Saved" });
                }
                catch (Exception ex)
                {
                    return Json(new ExecuteResult() { Isok = false, Data = null, Message = ex.Message });
                }
            }
            return Json(new ExecuteResult() { Isok = false, Data = null, Message = "Hãy nhập đủ thông tin" });

        }
        public async Task<ActionResult> CropOne(ImageCroped model, long idImage)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var croped = new ImageCroped();
                    croped = model;
                    croped.ImageId = idImage;
                    db.ImageCropeds.Add(croped);
                    await db.SaveChangesAsync();
                    return Json(new ExecuteResult() { Isok = true, Data = 1, Message = "Saved" });
                }
                catch (Exception ex)
                {
                    return Json(new ExecuteResult() { Isok = false, Data = null, Message = ex.Message });
                }
            }
            return Json(new ExecuteResult() { Isok = false, Data = null, Message = "Hãy nhập đủ thông tin" });

        }

        // ghi từ db vào file nhãn 
        public  ActionResult WriteFile(long accountId)
        {
            //format: a01-000u-00-00| ok| 154| 408 768 27 51| AT| A
            try
            {
                List<string> listFileName = new List<string>();
                List<string> listLable = new List<string>();
                // lấy list nhãn đã gán
                var listCroped = db.ImageCropeds.ToList();
                // lấy ra tên hình ảnh
                foreach (var crop in listCroped)
                {
                    listFileName.Add(db.Images.Find(crop.ImageId).Uri);
                    string nameImage = db.Images.Find(crop.ImageId).Name;
                    // nhãn 
                    listLable.Add(nameImage + " " + crop.Info);
                }
                
                string comment = "#NguyenAnhDung#";
                string temp = "/TrainingFile/";
                string word = "word.txt";
                string pathFile = Path.Combine(temp, word);
                string path = Server.MapPath("~"+ pathFile);
                if (!System.IO.File.Exists(path))
                {
                    FileHelper.CreateFile(path, comment);
                }
                foreach (var item in listLable)
                {
                    FileHelper.AppenAllText(path,"\n"+ item);
                }
                return Json(new ExecuteResult() { Isok = true, Data = "path", Message = "Is ok" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new ExecuteResult() { Isok = false, Data = null, Message = ex.Message });
            }
            }
    }
}