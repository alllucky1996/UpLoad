using Common.Helpers;
using Entities.Enums;
using Entities.Models;
using Entities.Models.SystemManage;
using Entities.ViewModels;
using Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Areas.Management.Filters;
using Web.Areas.Management.Helpers;

namespace Web.Areas.Management.Controllers
{
    /// <summary>
    /// Đơn vị là các cty
    /// </summary>
    [RouteArea("Management", AreaPrefix = "quan-ly")]
    public class UploadImageController : BaseController
    {
        #region base
        public const string CName = "UploadHinhAnh";
        public const ModuleEnum CModule = ModuleEnum.Image;
        public const string CRoute = "upload-hinh-anh";
        public const string CText = "upload hình ảnh";
        void BaseView()
        {
            ViewBag.Title = "Upload" + CText;
            ViewBag.CName = CName;
            ViewBag.CRoute = CRoute;
            ViewBag.CText = CText;
        }

        public IGenericRepository<Images> GetRespository()
        {
            return _repository.GetRepository<Images>();
        }
        public static Images NewObject()
        {
            return new Images();
        }

        public bool CanDelete(Images deleteItem)
        {
            //if (deleteItem.YeuCaus != null && deleteItem.YeuCaus.Any())
            //     return false;
            return true;
        }
        #endregion
        #region ok

        [Route("danh-muc-" + CRoute, Name = CName + "_Index")]
        [ValidationPermission(Action = ActionEnum.Read, Module = CModule)]
        public async Task<ActionResult> Index()
        {
            BaseView();
             //   Expression<Func<Images, bool>> filter = o => o.IdCha == null;
                var list = await GetRespository().GetAllAsync();
                return View(list.OrderBy(o => o.Id));
        }

        [Route("nhap" + CRoute, Name = CName + "_Create")]
        [ValidationPermission(Action = ActionEnum.Create, Module = CModule)]
        public ActionResult Create(string IdDonVi)
        {
            ViewBag.Title = "Thêm mới " + CText;
            ViewBag.CName = CName;
            ViewBag.CText = CText;
            var dsDonVi = GetRespository().GetAll().ToList();
            // lọc tiếp theo người 
            ViewBag.IdCha = new SelectList(dsDonVi, "Id", "Name", IdDonVi);
            return View();
        }
        [Route("nhap" + CRoute)]
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [ValidationPermission(Action = ActionEnum.Create, Module = CModule)]
        public async Task<ActionResult> Create(HttpPostedFileBase file)
        {
            Guid s = Guid.NewGuid();
            if (ModelState.IsValid)
            {
                try
                {
                    
                    string fullFilePath = "/Uploads/images/Source/IAM/";
                    string filePath = Server.MapPath("~/Uploads/images/Source/IAM/");
                    try
                    {
                        if (!Directory.Exists(filePath)) { DirectoryInfo di = Directory.CreateDirectory(filePath); }
                    }
                    catch { }
                    if (file != null && file.ContentLength > 0)
                    {
                        string fileName = CommonHelper.ToURL(Path.GetFileNameWithoutExtension(file.FileName), 0) + Path.GetExtension(file.FileName);
                        var path = Path.Combine(filePath, fileName);
                        if (System.IO.File.Exists(path))
                        {
                            fileName = string.Format("{0}_{1}{2}", CommonHelper.ToURL(Path.GetFileNameWithoutExtension(file.FileName), 0), String.Format("{0:dd_MM_yyyy}", DateTime.Now)+s.ToString(), Path.GetExtension(file.FileName));
                            path = Path.Combine(filePath, fileName);
                        }
                        file.SaveAs(path);
                        fullFilePath = fullFilePath + fileName;
                       
                    }




                    var newItem = NewObject();
                    newItem.AccountId = AccountId;
                    newItem.Name = fullFilePath;
                    
                    int result = await GetRespository().CreateAsync(newItem, AccountId);
                    if (result > 0)
                    {
                        TempData["Success"] = "Thêm mới thành công " + CText;
                        return RedirectToRoute(CName + "_Index");
                    }
                    else
                    {
                        ViewBag.Error = "Không thêm được " + CText;
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Đã xảy ra lỗi: " + ex.Message;
                    return View();
                }
            }
            else
            {
                ViewBag.Error = "Vui lòng nhập chính xác các thông tin!";
                return View();
            }
        }
       
        [Route("Xoa-" + CRoute + "/{code?}", Name = CName + "_Delete")]
        [HttpPost]
        [ValidationPermission(Action = ActionEnum.Delete, Module = CModule)]
        public async Task<JsonResult> Delete(long code)
        {
            try
            {
                var deleteItem = await GetRespository().ReadAsync(code);
                if (deleteItem != null)
                {
                    if (!CanDelete(deleteItem)) return Json(new { success = false, message = CText + " đang được sử dụng!" });
                    int result = await GetRespository().DeleteAsync(deleteItem, AccountId);
                    if (result > 0)
                    {
                        return Json(new { success = true, message = "Xóa thành công!" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Lỗi: Không xóa được bản ghi này!" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Lỗi: Không tìm thấy đối tượng!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
      
    }
}