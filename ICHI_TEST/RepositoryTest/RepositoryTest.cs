using ICHI.DataAccess.Repository;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_CORE.Domain.MasterModel;

namespace ICHI_TEST.RepositoryTest
{
  public class RepositoryTest
  {
    private readonly PcsApiContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public RepositoryTest()
    {
      _context = ContextGenerator.Generator();
      _unitOfWork = new UnitOfWork(_context);
      CreateData();

    }

    // Tạo data mẫu
    private void CreateData()
    {
      for (int i = 0; i < 30; i++)
      {
        _unitOfWork.Category.Add(new Category
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
      _unitOfWork.Save();
    }
    /// <summary>
    ///  test case kiểm tra việc create category thành công
    /// </summary>
    [Fact]
    public void Create_Success()
    {
      // Arrange
      Category category = new Category
      {
        CategoryName = "Test Category Create",
        ParentID = 0,
        CategoryLevel = 1,
        Notes = "Notes",
        IsDeleted = false,
        CreateBy = "Admin",
        ModifiedBy = "Admin"
      };

      // Act
      _unitOfWork.Category.Add(category);
      _unitOfWork.Save();

      // Assert
      var updatedCategory = _unitOfWork.Category.Get(u => u.CategoryName == "Test Category Create");
      Assert.NotNull(updatedCategory);
      Assert.Equal("Test Category Create", updatedCategory.CategoryName);
    }
    /// <summary>
    ///  test case kiểm tra việc update category thành công
    /// </summary>
    [Fact]
    public void Update_Success()
    {
      // Arrange
      var model = _unitOfWork.Category.Get(u => u.Id == 1, tracked: true);

      // Act
      model.CategoryName = "Updated Category Sussess";
      _unitOfWork.Category.Update(model);
      _unitOfWork.Save();

      // Assert
      var updatedCategory = _unitOfWork.Category.Get(u => u.Id == 1);
      Assert.NotNull(updatedCategory);
      Assert.Equal("Updated Category Sussess", updatedCategory.CategoryName);
    }
    /// <summary>
    /// test case kiểm tra việc update category không thành công
    /// </summary>
    [Fact]
    public void Update_Fail()
    {
      // Arrange
      var data = _unitOfWork.Category.Get(u => u.Id == 1, tracked: true);

      // Act
      data.CategoryName = "Update category Faild";
      _unitOfWork.Category.Update(data);
      _unitOfWork.Save();

      // Assert
      var updatedCategory = _unitOfWork.Category.Get(u => u.Id == 1, tracked: true);
      Assert.NotNull(updatedCategory);
      Assert.NotEqual("Test Category", updatedCategory.CategoryName);
    }
    /// <summary>
    /// test case kiểm tra việc tồn tại category
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task ExistsByNameSuccess()
    {
      // Act
      var exists = _unitOfWork.Category.ExistsBy(c => c.CategoryName == "Category 4");

      // Assert
      Assert.True(exists);
    }
    /// <summary>
    /// test case kiểm tra việc không tồn tại category
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task ExistsByNameFail()
    {
      // Act
      var exists = _unitOfWork.Category.ExistsBy(c => c.CategoryName == "Test Category 32");
      Assert.False(exists);
    }
    /// <summary>
    /// Test case thực hiện insert 5 bảng ghi category và kiểm tra lấy theo id thành công
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task FindByIdSuccess()
    {
      // Act
      var result = _unitOfWork.Category.Get(u => u.Id == 1);
      // Assert
      Assert.NotNull(result);
      Assert.Equal("Category 0", result.CategoryName);
    }
    /// <summary>
    /// Test case thực hiện insert 5 bảng ghi category và kiểm tra lấy theo id  thất bại
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task FindByIdFail()
    {
      // Act
      var result = _unitOfWork.Category.Get(u => u.Id == 50);

      // Assert
      Assert.Null(result);
    }
    /// <summary>
    /// test case thực hiện xóa category thành công
    /// </summary>
    [Fact]
    public async Task DeleteSuccess()
    {
      // Arrange
      var data = _unitOfWork.Category.Get(u => u.Id == 1, tracked: true);
      // Act
      _unitOfWork.Category.Remove(data);
      _unitOfWork.Save();
      // Assert
      var deletedCategory = _unitOfWork.Category.Get(u => u.Id == 1);
      Assert.Null(deletedCategory);
    }
    /// <summary>
    /// test case thực hiện xóa category thất bại
    /// </summary>
    [Fact]
    public async Task DeleteFaild()
    {
      // Act
      var data = _unitOfWork.Category.Get(u => u.Id == 50, tracked: true);

      if (data != null)
      {
        _unitOfWork.Category.Remove(data);
        _unitOfWork.Save();
      }

      // Assert
      var categoryAfterDelete = _unitOfWork.Category.Get(u => u.Id == 50);
      Assert.Null(categoryAfterDelete);
    }
    /// <summary>
    /// test case lấy danh sách category thành công
    /// </summary>
    [Fact]
    public async Task GetAllSuccess()
    {
      // Act
      var data = _unitOfWork.Category.GetAll().Take(5);

      // Assert
      Assert.NotNull(data);
      Assert.Equal(5, data.Count());
    }
    /// <summary>
    /// test case lấy danh sách category  thất bại
    /// </summary>
    [Fact]
    public async Task GetAllFaild()
    {
      _unitOfWork.Category.GetAll().ToList().ForEach(c =>
      {
        _unitOfWork.Category.Remove(c);
      });
      _unitOfWork.Save();
      // Act
      var result = _unitOfWork.Category.GetAll();

      // Assert
      Assert.NotNull(result);
      Assert.Empty(result);
    }
  }
}
