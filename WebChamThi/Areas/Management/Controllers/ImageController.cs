using Common.Helpers;
using Entities.Enums;
using Entities.Models;
using Entities.Models.SystemManage;
using Entities.ViewModels;
using Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class ImageController : BaseController
    {
        #region base
        public const string CName = "HinhAnh";
        public const ModuleEnum CModule = ModuleEnum.Image;
        public const string CRoute = "hinh-anh";
        public const string CText = "hình ảnh";
        void BaseView()
        {
            ViewBag.Title = "Danh mục" + CText;
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
        public async Task<ActionResult> Create(Images model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string ma = StringHelper.KillChars(model.Code);
                    //Kiểm tra trùng mã
                    var any = await GetRespository().AnyAsync(o => o.Code == ma);
                    if (any)
                    {
                        ViewBag.Error = "Mã này đã được sử dụng! Vui lòng nhập mã khác!";
                        return View(model);
                    }
                    //Nhập trạng thái bài viết
                    var newItem = NewObject();
                    newItem = model;
                    int result = await GetRespository().CreateAsync(newItem, AccountId);
                    if (result > 0)
                    {
                        TempData["Success"] = "Thêm mới thành công " + CText;
                        return RedirectToRoute(CName + "_Index");
                    }
                    else
                    {
                        ViewBag.Error = "Không thêm được " + CText;
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Đã xảy ra lỗi: " + ex.Message;
                    return View(model);
                }
            }
            else
            {
                ViewBag.Error = "Vui lòng nhập chính xác các thông tin!";
                return View(model);
            }
        }
        [Route("cap-nhat-" + CRoute + "/{ma}", Name = CName + "_Update")]
        [ValidationPermission(Action = ActionEnum.Update, Module = CModule)]
        public async Task<ActionResult> Update(long ma)
        {
            var editingItem = await GetRespository().ReadAsync(o => o.Id == ma);
            if (editingItem == null)
            {
                TempData["Error"] = "Không tìm thấy " + CText;
                return RedirectToRoute(CName + "_Index");
            }

            //ViewBag.IdCha = new SelectList(GetRespository().GetAll().ToList(), "Id", "Name", editingItem.IdCha);
            ViewBag.Title = "Sửa " + CText;
            ViewBag.CName = CName;
            ViewBag.CText = CText;
            return View(editingItem);
        }
        [Route("cap-nhat-" + CRoute)]
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [ValidationPermission(Action = ActionEnum.Update, Module = CModule)]
        public async Task<ActionResult> Update(long Id, Images model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Cập nhật trạng thái bài viết
                    var updateItem = await GetRespository().ReadAsync(o => o.Id == Id);
                    if (updateItem == null)
                    {
                        TempData["Error"] = "Không tìm thấy " + CText;
                        return RedirectToRoute(CName + "_Index");
                    }
                    //Không cho sửa mã, nếu cho sửa phải kiểm tra trùng
                    //deleteItem.Ma = StringHelper.KillChars(model.Ma);
                    //  updateItem.Code = StringHelper.KillChars(model.Code);
                    updateItem.Name = StringHelper.KillChars(model.Name);
                    updateItem.UpdateDate = DateTime.Now;
                  
                    int result = await GetRespository().UpdateAsync(updateItem, AccountId);
                    if (result > 0)
                    {
                        TempData["Success"] = "Ghi nhận thành công!";
                        return RedirectToRoute(CName + "_Index");
                    }
                    else
                    {
                        ViewBag.Error = "Không ghi nhận được!";
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Đã xảy ra lỗi: " + ex.Message;
                    return View(model);
                }
            }
            else
            {
                ViewBag.Error = "Vui lòng nhập chính xác các thông tin!";
                return View(model);
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
        #region tìm kiếm nâng cao
        [Route("tim-kiem-" + CRoute, Name = CName + "_Search")]
        [ValidationPermission(Action = ActionEnum.Read, Module = CModule)]
        public async Task<ActionResult> Search(Images searchmodel)
        {
            BaseView();
           
            Expression<Func<Images, bool>> filter = o => (
            ( o.Code.Contains(searchmodel.Code)
            //|| o.Description.Contains(searchmodel.Description)
            //|| o.DiaChi.Contains(searchmodel.DiaChi)
            //|| o.DienThoai.Contains(searchmodel.DienThoai)
            //|| o.Email.Contains(searchmodel.Email)
            || o.Name.Contains(searchmodel.Name)
            )
           );

            var list = await GetRespository().GetAllAsync(filter);
            return View(list);
        }

      

        #endregion
    }
}