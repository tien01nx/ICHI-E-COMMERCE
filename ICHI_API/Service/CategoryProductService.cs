﻿using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Helpers;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.NlogConfig;
using System.Linq.Dynamic.Core;


namespace ICHI_API.Service
{
  public class CategoryProductService : ICategoryProductService
  {
    private readonly IUnitOfWork _unitOfWork;
    private PcsApiContext _db;
    public CategoryProductService(IUnitOfWork unitOfWork, IConfiguration configuration, PcsApiContext pcsApiContext)
    {
      _unitOfWork = unitOfWork;
      _db = pcsApiContext;
    }
    public Helpers.PagedResult<Category> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var query = _db.Categories.AsQueryable().Where(u => u.IsDeleted == false);
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
        strMessage = ex.ToString();
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
          strMessage = "Nhà cung cấp không tồn tại";
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
        // lấy thông tin nhà cung cấp
        var data = _unitOfWork.Category.Get(u => u.Id == category.Id);
        if (data == null)
        {
          strMessage = "Danh mục sản phẩm không tồn tại";
          return null;
        }

        category.ModifiedBy = "Admin";
        _unitOfWork.Category.Update(category);
        _unitOfWork.Save();
        strMessage = "Cập nhật nhà cung cấp thành công";
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
        var data = _unitOfWork.Category.Get(u => u.Id == id && !u.IsDeleted);
        if (data == null)
        {
          strMessage = "Nhà cung cấp không tồn tại";
          return false;
        }
        data.IsDeleted = true;
        data.ModifiedDate = DateTime.Now;
        _unitOfWork.Category.Update(data);
        _unitOfWork.Save();
        strMessage = "Xóa nhà cung cấp thành công";
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