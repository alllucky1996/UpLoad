using Common.Helpers;
using CropImage.Commons;
using CropImage.Handler.Crop;
using CropImage.Models;
using CropImage.Models.ViewModels;
using Emgu.CV;
using Emgu.CV.Structure;
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
        private string PreViewImage = "~/TempImage/";
        public int widthImage { get; set; }
        public int heightImage { get; set; }
        #region get 
        void baseView()
        {
            ViewBag.Image = "/Uploads/Images/Mau1.jpg";
            ViewBag.idImage = 1;
            int h;
           
            ViewBag.widthImage = CropHelper.WidthImage(Server.MapPath("~/Uploads/Images/Mau1.jpg"), out h);
            ViewBag.heightImage = h;
            ViewBag.PreViewImage = "/TempImage/tempImages.jpg";
        }
        void baseView(Image img)
        {
            ViewBag.Image = img.Uri;
            ViewBag.idImage = img.Id;
            int h;
            ViewBag.widthImage = CropHelper.WidthImage(Server.MapPath("~" + img.Uri), out h);
            ViewBag.heightImage = h;
        }

        public ActionResult Index()
        {
            baseView();
            #region drop
            var listDau = db.Daus;
            ViewBag.Dau = new SelectList(listDau, "Code", "Name");
            var listLoaiTu = db.LoaiTus;
            ViewBag.LoaiTu = new SelectList(listLoaiTu, "Code", "Name");
            #endregion
            return View();
        }
        public async Task<ActionResult> Pre(long id)
        {
            Image img = null;
            string error = "";
            if (id > 1)
            {
                img = await db.Images.FindAsync(id - 1);
                while (img == null)
                {
                    id--;
                    img = await db.Images.FindAsync(id);
                }
            }
            else
            {
                img = await db.Images.FindAsync(id);
                error = "Không có ảnh trước đó";
            }
            ViewBag.Error = error;
            baseView(img);
            #region drop
            var listDau = db.Daus;
            ViewBag.Dau = new SelectList(listDau, "Code", "Name");
            var listLoaiTu = db.LoaiTus;
            ViewBag.LoaiTu = new SelectList(listLoaiTu, "Code", "Name");
            #endregion
            return View();
            // return Json(new ExecuteResult() { Isok = true, Data = img}, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> Next(long id)
        {
            var lastImg = db.Images.OrderByDescending(u => u.Id).FirstOrDefault();
            string error = "";
            Image img;
            if (id < lastImg.Id)
            {
                img = await db.Images.FindAsync(id + 1);
                while (img == null)
                {
                    id++;
                    img = await db.Images.FindAsync(id);
                }

            }
            else
            {
                img = lastImg;
                error = "Không còn ảnh tiếp theo";
            }
            ViewBag.Error = error;
            baseView(img);
            //#region drop
            //var listDau = db.Daus;
            //ViewBag.Dau = new SelectList(listDau, "Code", "Name");
            //var listLoaiTu = db.LoaiTus;
            //ViewBag.LoaiTu = new SelectList(listLoaiTu, "Code", "Name");
            //#endregion
            return View();
            // return Json(new { Isok = true, Data = img.Uri, objectId=img.Id}, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Cau()
        {
            //  baseView();
            return PartialView();
        }
        public ActionResult Tu()
        {
            // baseView();
            #region drop
            var listDau = db.Daus;
            ViewBag.Dau = new SelectList(listDau, "Code", "Name");
            var listLoaiTu = db.LoaiTus;
            ViewBag.LoaiTu = new SelectList(listLoaiTu, "Code", "Name");
            #endregion
            return PartialView();
        }
        public ActionResult AmTiet()
        {
            //  baseView();
            #region drop
            var listDau = db.Daus;
            ViewBag.Dau = new SelectList(listDau, "Code", "Name");
            var listTuLoai = db.LoaiTus;
            ViewBag.TuLoai = new SelectList(listTuLoai, "Code", "Name");
            #endregion
            return PartialView();
        }
        public ActionResult Chu()
        {
            // baseView();
            #region drop
            var listDau = db.Daus;
            ViewBag.Dau = new SelectList(listDau, "Code", "Name");
            #endregion
            return PartialView();
        }
        #endregion
        #region post
        [HttpPost]
        public async Task<ActionResult> CropCau(ImageCroped model, long idImage)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.X == model.Y && model.Width == model.Height && model.Height == 0 && model.Y == 0) Json(new ExecuteResult() { Isok = false, Data = null, Message = "Crop k hợp lệ" });
                    var croped = new ImageCroped();
                    croped = model;
                    croped.ImageId = idImage;
                    db.ImageCropeds.Add(croped);
                    await db.SaveChangesAsync();
                    // cắt hình && show
                    var img = db.Images.Find(idImage);
                    if (img != null)
                    {
                        string pat = FileHelper.GetRunningPath();
                        string ax = Server.MapPath("~/" + img.Uri);
                        var rootImage = new Image<Bgr, byte>(ax);
                        // get Id insered 
                        db.Entry(croped).GetDatabaseValues();
                        long i = croped.Id;
                        PreViewImage = PreViewImage + idImage + "_" + i + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff") + ".jpg";
                        var ok = CropHelper.Save(CropHelper.Crop(rootImage, model.X, model.Y, model.Width, model.Height), Server.MapPath(PreViewImage));
                        if (!ok)
                        {
                            return Json(new ExecuteResult() { Isok = false, Data = PreViewImage.Replace("~", ""), Message = "Lưu đươc vào db nhưng lỗi trong quá trình xử lý ảnh", PreVeiwImage = PreViewImage.Replace("~", "") });
                        }
                    }

                    return Json(new ExecuteResult() { Isok = true, Data = PreViewImage.Replace("~", ""), Message = "Saved", PreVeiwImage = PreViewImage.Replace("~", "") });
                }
                catch (Exception ex)
                {
                    return Json(new ExecuteResult() { Isok = false, Data = null, Message = ex.Message });
                }
            }
            return Json(new ExecuteResult() { Isok = false, Data = null, Message = "Hãy nhập đủ thông tin" });

        }
        [HttpPost]
        public async Task<ActionResult> CropTu(ImageCroped model, long idImage)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.X == model.Y && model.Width == model.Height && model.Height == 0 && model.Y == 0) Json(new ExecuteResult() { Isok = false, Data = null, Message = "Crop k hợp lệ" });
                    var croped = new ImageCroped();
                    croped = model;
                    croped.ImageId = idImage;
                    db.ImageCropeds.Add(croped);
                    await db.SaveChangesAsync();
                    // cắt hình && show nếu là từ ghép 
                    var c = model.Lable.Trim().Split(' ').Length;
                    if (c > 1)
                    {
                        var img = db.Images.Find(idImage);
                        if (img != null)
                        {
                            string pat = FileHelper.GetRunningPath();
                            string ax = Server.MapPath("~/" + img.Uri);
                            var rootImage = new Image<Bgr, byte>(ax);
                            // get Id insered 
                            db.Entry(croped).GetDatabaseValues();
                            long i = croped.Id;
                            PreViewImage = PreViewImage + idImage + "_" + i + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff") + ".jpg";
                            var ok = CropHelper.Save(CropHelper.Crop(rootImage, model.X, model.Y, model.Width, model.Height), Server.MapPath(PreViewImage));
                            if (!ok)
                            {
                                return Json(new ExecuteResult() { Isok = false, Data = "", Message = "Lưu đươc vào db nhưng lỗi trong quá trình xử lý ảnh", PreVeiwImage = PreViewImage.Replace("~", "") });
                            }
                            return Json(new ExecuteResult() { Isok = true, Data = PreViewImage.Replace("~", ""), Message = "Save crop is ok", PreVeiwImage = PreViewImage.Replace("~", "") });
                        }
                    }
                    return Json(new ExecuteResult() { Isok = true, Data = null, Message = "Saved", PreVeiwImage = PreViewImage.Replace("~", "") });
                }
                catch (Exception ex)
                {
                    return Json(new ExecuteResult() { Isok = false, Data = "", Message = ex.Message });
                }
            }
            return Json(new ExecuteResult() { Isok = false, Data = "", Message = "Hãy nhập đủ thông tin" });

        }

        [HttpPost]
        public async Task<ActionResult> CropAmTiet(ImageCroped model, long idImage)
        {
            if (ModelState.IsValid)
            {
                // nhầm mức tự động chuyển hay báo ra 
                if (model.Lable.Split(' ').Count() > 1)
                {
                    // goto Tu??0
                    return Json(new ExecuteResult() { Isok = false, Data = null, Message = "Nhầm mức " });
                }
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

        [HttpPost]
        public async Task<ActionResult> CropChuOrDau(ImageCroped model, long idImage)
        {
            if (ModelState.IsValid)
            {
                if (model.Lable.Split(' ').Count() > 1)
                {
                    // goto Tu??0
                    return Json(new ExecuteResult() { Isok = false, Data = null, Message = "Nhầm mức " });
                }
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






        #endregion







        // ghi từ db vào file nhãn 
        public ActionResult WriteFile(string key)
        {
            //format: a01-000u-00-00| ok| 154| 408 768 27 51| AT| A
            // get key
           
            string keyEn = Commons.StringHelper.stringToSHA512(key);
            if (keyEn.Equals(db.Key.FirstOrDefault().Key))
            {
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
                    string path = Server.MapPath("~" + pathFile);
                    if (!System.IO.File.Exists(path))
                    {
                        FileHelper.CreateFile(path, comment);
                    }
                    foreach (var item in listLable)
                    {
                        FileHelper.AppenAllText(path, "\n" + item);
                    }
                    return Json(new ExecuteResult() { Isok = true, Data = "path", Message = "Is ok" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new ExecuteResult() { Isok = false, Data = null, Message = ex.Message });
                }
            }
            return Json(new ExecuteResult() { Isok = false, Data = null, Message ="Key k đúng hoặc k có quyền ghi file" });
        }
    }
}