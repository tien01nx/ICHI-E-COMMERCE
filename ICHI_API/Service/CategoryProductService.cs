using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Helpers;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Helpers;
using ICHI_CORE.NlogConfig;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;


namespace ICHI_API.Service
{
  public class CategoryProductService : ICategoryProductService
  {
    private readonly IUnitOfWork _unitOfWork;
    private PcsApiContext _db;
    public CategoryProductService(IUnitOfWork unitOfWork, PcsApiContext pcsApiContext)
    {
      _unitOfWork = unitOfWork;
      _db = pcsApiContext;
    }
    public Helpers.PagedResult<Category> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        //var query = _db.Categories.AsQueryable().Where(u => u.IsDeleted == false);
        var query = _unitOfWork.Category.GetAll(u => !u.IsDeleted).AsQueryable();
        if (!string.IsNullOrEmpty(name))
        {
          query = query.Where(e => e.CategoryName.Contains(name));
        }
        var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
        query = query.OrderBy(orderBy);
        var pagedResult = Helpers.PagedResult<Category>.CreatePagedResult(query, pageNumber, pageSize);
        return pagedResult;
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = "Không tồn tại bản ghi nào";
        return null;
      }
    }
    public Category FindById(int id, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var data = _unitOfWork.Category.Get(u => u.Id == id);
        if (data == null)
        {
          strMessage = "Danh mục sản phẩm không tồn tại";
          return null;
        }
        return data;
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return null;
      }
    }
    public Category Create(Category category, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var checkCategory = _unitOfWork.Category.Get(u => u.CategoryName == category.CategoryName);
        if (checkCategory != null)
        {
          strMessage = "Danh mục sản phẩm đã tồn tại";
          return null;
        }

        category.CreateBy = "Admin";
        category.ModifiedBy = "Admin";
        _unitOfWork.Category.Add(category);
        _unitOfWork.Save();
        strMessage = "Tạo mới danh mục thành công";
        return category;
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return null;
      }
    }
    public Category Update(Category category, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        // lấy thông tin danh mục sản phẩm
        var data = _unitOfWork.Category.Get(u => u.Id == category.Id, tracked: true);
        if (data == null)
        {
          strMessage = "Danh mục sản phẩm không tồn tại";
          return null;
        }
        if (data.CategoryName != category.CategoryName)
        {
          // Nếu tên đã thay đổi, kiểm tra trùng lặp với các danh mục khác
          var checkCategory = _unitOfWork.Category.Get(u => u.CategoryName == category.CategoryName && u.Id != category.Id, tracked: true);
          if (checkCategory != null)
          {
            strMessage = "Tên danh mục sản phẩm đã tồn tại";
            return null;
          }
        }
        data.CategoryName = category.CategoryName;
        data.CategoryLevel = category.CategoryLevel;
        data.ParentID = category.ParentID;
        data.Notes = category.Notes;
        data.ModifiedDate = DateTime.Now;
        data.ModifiedBy = category.ModifiedBy;
        category.ModifiedBy = "Admin";
        _unitOfWork.Category.Update(data);
        _unitOfWork.Save();
        strMessage = "Cập nhật danh mục sản phẩm thành công";
        return category;
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return null;
      }
    }
    public bool Delete(int id, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var data = _unitOfWork.Category.Get(u => u.Id == id && !u.IsDeleted, tracked: true);
        if (data == null)
        {
          strMessage = "Danh mục sản phẩm không tồn tại";
          return false;
        }
        data.IsDeleted = true;
        data.ModifiedDate = DateTime.Now;
        _unitOfWork.Category.Update(data);
        _unitOfWork.Save();
        strMessage = "Xóa Danh mục sản phẩm thành công";
        return true;
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return false;
      }
    }
  }
}
