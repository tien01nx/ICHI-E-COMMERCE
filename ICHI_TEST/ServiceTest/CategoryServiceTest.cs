using ICHI.DataAccess.Repository.IRepository;
using ICHI.DataAccess.Repository;
using ICHI_API.Data;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.NlogConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICHI_API.Service.IService;
using ICHI_API.Service;
using ICHI_CORE.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ICHI_CORE.Domain;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Hosting;
using Castle.Core.Configuration;
using Moq;

namespace ICHI_TEST.ServiceTest
{
  //public Helpers.PagedResult<Category> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
  //{
  //  strMessage = string.Empty;
  //  try
  //  {
  //    //var query = _db.Categories.AsQueryable().Where(u => u.IsDeleted == false);
  //    var query = _unitOfWork.Category.GetAll(u => u!.IsDeleted).AsQueryable();
  //    if (!string.IsNullOrEmpty(name))
  //    {
  //      query = query.Where(e => e.CategoryName.Contains(name));
  //    }
  //    var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
  //    query = query.OrderBy(orderBy);
  //    var pagedResult = Helpers.PagedResult<Category>.CreatePagedResult(query, pageNumber, pageSize);
  //    return pagedResult;
  //  }
  //  catch (Exception ex)
  //  {
  //    NLogger.log.Error(ex.ToString());
  //    strMessage = ex.ToString();
  //    return null;
  //  }
  //}
  //public Category FindById(int id, out string strMessage)
  //{
  //  strMessage = string.Empty;
  //  try
  //  {
  //    var data = _unitOfWork.Category.Get(u => u.Id == id);
  //    if (data == null)
  //    {
  //      strMessage = "Danh mục sản phẩm không tồn tại";
  //      return null;
  //    }
  //    return data;
  //  }
  //  catch (Exception ex)
  //  {
  //    NLogger.log.Error(ex.ToString());
  //    strMessage = ex.ToString();
  //    return null;
  //  }
  //}
  //public Category Create(Category category, out string strMessage)
  //{
  //  strMessage = string.Empty;
  //  try
  //  {
  //    var checkCategory = _unitOfWork.Category.Get(u => u.CategoryName == category.CategoryName);
  //    if (checkCategory != null)
  //    {
  //      strMessage = "Danh mục sản phẩm đã tồn tại";
  //      return null;
  //    }

  //    category.CreateBy = "Admin";
  //    category.ModifiedBy = "Admin";
  //    _unitOfWork.Category.Add(category);
  //    _unitOfWork.Save();
  //    strMessage = "Tạo mới danh mục thành công";
  //    return category;
  //  }
  //  catch (Exception ex)
  //  {
  //    NLogger.log.Error(ex.ToString());
  //    strMessage = ex.ToString();
  //    return null;
  //  }
  //}
  //public Category Update(Category category, out string strMessage)
  //{
  //  strMessage = string.Empty;
  //  try
  //  {
  //    // lấy thông tin danh mục sản phẩm
  //    var data = _unitOfWork.Category.Get(u => u.Id == category.Id);
  //    if (data == null)
  //    {
  //      strMessage = "Danh mục sản phẩm không tồn tại";
  //      return null;
  //    }

  //    category.ModifiedBy = "Admin";
  //    _unitOfWork.Category.Update(category);
  //    _unitOfWork.Save();
  //    strMessage = "Cập nhật danh mục sản phẩm thành công";
  //    return category;
  //  }
  //  catch (Exception ex)
  //  {
  //    NLogger.log.Error(ex.ToString());
  //    strMessage = ex.ToString();
  //    return null;
  //  }
  //}
  //public bool Delete(int id, out string strMessage)
  //{
  //  strMessage = string.Empty;
  //  try
  //  {
  //    var data = _unitOfWork.Category.Get(u => u.Id == id && !u.IsDeleted);
  //    if (data == null)
  //    {
  //      strMessage = "Danh mục sản phẩm không tồn tại";
  //      return false;
  //    }
  //    data.IsDeleted = true;
  //    data.ModifiedDate = DateTime.Now;
  //    _unitOfWork.Category.Update(data);
  //    _unitOfWork.Save();
  //    strMessage = "Xóa Danh mục sản phẩm thành công";
  //    return true;
  //  }
  //  catch (Exception ex)
  //  {
  //    NLogger.log.Error(ex.ToString());
  //    strMessage = ex.ToString();
  //    return false;
  //  }
  //}
  //public class Category : MasterEntity
  //{
  //  [Required]
  //  public int ParentID { get; set; }
  //  [Required]
  //  public int CategoryLevel { get; set; }
  //  [Required]
  //  [StringLength(255)]
  //  public string CategoryName { get; set; } = string.Empty;
  //  public string Notes { get; set; } = string.Empty;
  //  public bool IsDeleted { get; set; } = false;
  //}


  public class CategoryServiceTest
  {
    private readonly PcsApiContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryProductService _categoryproductService;

    public CategoryServiceTest()
    {
      _context = ContextGenerator.Generator();
      _unitOfWork = new UnitOfWork(_context);
      CreateData();
      _categoryproductService = new CategoryProductService(_unitOfWork, _context);
    }


    // fake data Category
    public void CreateData()
    {
      // Arrange
      for (int i = 0; i < 30; i++)
      {
        _context.Categories.Add(new Category
        {
          CategoryName = $"Category {i}",
          ParentID = 0,
          CategoryLevel = 1,
          Notes = "Notes",
          IsDeleted = false,
          CreateBy = "Admin",
          ModifiedBy = "Admin"
        });
      }
      _context.SaveChanges();
    }

    /// <summary>
    /// Test case Kiểm tra lấy danh sách danh mục sản phẩm 10 bản ghi
    /// </summary>
    [Fact]
    public void GetAllSuccess()
    {
      string strMessage = string.Empty;
      // Act
      var result = _categoryproductService.GetAll("", 10, 1, "asc", "CategoryName", out strMessage);
      // Assert
      Assert.NotNull(result);
      Assert.Equal(10, result.Items.Count());
    }

    /// <summary>
    /// Test case Kiểm tra lấy danh sách danh mục sản phẩm khi không có bản ghi nào
    /// </summary>
    [Fact]
    public void GetAllFail()
    {
      string strMessage = string.Empty;
      // Act
      var result = _categoryproductService.GetAll("", 10, 10, "asc", "CategoryName", out strMessage);
      // Assert
      Assert.NotNull(result);
      Assert.Empty(result.Items);
    }

    /// <summary>
    /// Test case kiểm tra lấy danh mục sản phẩm theo id
    /// </summary>
    [Fact]
    public void FindByIdSuccess()
    {
      string strMessage = string.Empty;
      // Act
      var result = _categoryproductService.FindById(1, out strMessage);
      // Assert
      Assert.NotNull(result);
      Assert.Equal("Category 0", result.CategoryName);
    }

    /// <summary>
    /// Test case kiểm tra lấy danh mục sản phẩm theo id nhưng không tồn tại
    /// </summary>
    [Fact]
    public void FindByIdFail()
    {
      string strMessage = string.Empty;
      // Act
      var result = _categoryproductService.FindById(40, out strMessage);
      // Assert
      Assert.Null(result);
      Assert.Equal("Danh mục sản phẩm không tồn tại", strMessage);
    }

    /// <summary>
    /// Test case kiểm tra taọ mới danh mục sản phẩm thành công
    /// </summary>
    [Fact]
    public void CreateSuccess()
    {
      string strMessage = string.Empty;
      // Act
      var result = _categoryproductService.Create(new Category
      {
        CategoryName = "Category 30",
        ParentID = 0,
        CategoryLevel = 1,
        Notes = "Notes",
        IsDeleted = false,
        CreateBy = "Admin",
        ModifiedBy = "Admin"
      }, out strMessage);
      // Assert
      Assert.NotNull(result);
      Assert.Equal("Tạo mới danh mục thành công", strMessage);
    }

    /// <summary>
    /// Test case kiểm tra taọ mới danh mục sản phẩm thất bại khi trùng tên
    /// </summary>
    [Fact]
    public void CreateFaid()
    {
      string strMessage = string.Empty;
      // Act
      var result = _categoryproductService.Create(new Category
      {
        CategoryName = "Category 29",
        ParentID = 0,
        CategoryLevel = 1,
        Notes = "Notes",
        IsDeleted = false,
        CreateBy = "Admin",
        ModifiedBy = "Admin"
      }, out strMessage);
      // Assert
      Assert.Null(result);
      Assert.Equal("Danh mục sản phẩm đã tồn tại", strMessage);
    }

    /// <summary>
    /// Test case thực hiện cập nhật danh mục sản phẩm thành công
    /// </summary>
    [Fact]
    public void UpdateSuccess()
    {
      string strMessage = string.Empty;
      // Act
      var result = _categoryproductService.Update(new Category
      {
        Id = 1,
        CategoryName = "Category 31",
        ParentID = 0,
        CategoryLevel = 1,
        Notes = "Notes",
        IsDeleted = false,
        CreateBy = "Admin",
        ModifiedBy = "Admin"
      }, out strMessage);
      // Assert
      Assert.NotNull(result);
      Assert.Equal("Cập nhật danh mục sản phẩm thành công", strMessage);
    }

    /// <summary>
    /// Test case thực hiện cập nhật danh mục sản phẩm thất bại khi không tồn tại id
    /// </summary>
    [Fact]
    public void UpdateFailtoId()
    {
      string strMessage = string.Empty;
      // Act
      var result = _categoryproductService.Update(new Category
      {
        Id = 40,
        CategoryName = "Category 27",
        ParentID = 0,
        CategoryLevel = 1,
        Notes = "Notes",
        IsDeleted = false,
        CreateBy = "Admin",
        ModifiedBy = "Admin"
      }, out strMessage);
      // Assert
      Assert.Null(result);
      Assert.Equal("Danh mục sản phẩm không tồn tại", strMessage);
    }

    /// <summary>
    /// Test case thực hiện cập nhật danh mục sản phẩm thất bại khi trùng CategoryName
    /// </summary>
    [Fact]
    public void UpdateFailToCategoryName()
    {
      string strMessage = string.Empty;
      // Act
      var result = _categoryproductService.Update(new Category
      {
        Id = 1,
        CategoryName = "Category 27",
        ParentID = 0,
        CategoryLevel = 1,
        Notes = "Notes",
        IsDeleted = false,
        CreateBy = "Admin",
        ModifiedBy = "Admin"
      }, out strMessage);
      // Assert
      Assert.Null(result);
      Assert.Equal("Tên danh mục sản phẩm đã tồn tại", strMessage);
    }

    /// <summary>
    /// test case thực hiện xóa danh mục sản phẩm thành công
    /// </summary> 
    [Fact]
    public void DeleteSuccess()
    {
      string strMessage = string.Empty;
      // Act
      var result = _categoryproductService.Delete(1, out strMessage);
      // Assert
      Assert.True(result);
      Assert.Equal("Xóa Danh mục sản phẩm thành công", strMessage);
    }

    /// <summary>
    /// test case thực hiện xóa danh mục sản phẩm thất bại khi không tồn tại Id
    /// </summary> 
    [Fact]
    public void DeleteFail()
    {
      string strMessage = string.Empty;
      // Act
      var result = _categoryproductService.Delete(50, out strMessage);
      // Assert
      Assert.False(result);
      Assert.Equal("Danh mục sản phẩm không tồn tại", strMessage);
    }


  }
}
