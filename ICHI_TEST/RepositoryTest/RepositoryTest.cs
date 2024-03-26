using ICHI.DataAccess.Repository;
using ICHI_API.Data;
using ICHI_CORE.Domain.MasterModel;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ICHI_TEST.RepositoryTest
{
  public class RepositoryTest
  {
    /// <summary>
    ///  test case kiểm tra việc update category thành công
    /// </summary>
    [Fact]
    public void Update_Success()
    {
      // Arrange
      var context = ContextGenerator.Generator();
      var repository = new CategoryRepository(context);
      var category = new Category { ParentID = 1, CategoryLevel = 2, CategoryName = "Test Category" };
      context.Categories.Add(category);
      context.SaveChanges();

      // Act
      category.CategoryName = "Updated Category";
      repository.Update(category);
      context.SaveChanges();

      // Assert
      var updatedCategory = context.Categories.FirstOrDefault(c => c.Id == category.Id);
      Assert.NotNull(updatedCategory);
      Assert.Equal("Updated Category", updatedCategory.CategoryName);
    }
    /// <summary>
    /// test case kiểm tra việc update category không thành công
    /// </summary>
    [Fact]
    public void Update_Fail()
    {
      // Arrange
      var context = ContextGenerator.Generator();
      var repository = new CategoryRepository(context);
      var category = new Category { ParentID = 1, CategoryLevel = 2, CategoryName = "Test Category" };
      context.Categories.Add(category);
      context.SaveChanges();

      // Act
      category.CategoryName = "Updated Category";
      repository.Update(category);
      context.SaveChanges();

      // Assert
      var updatedCategory = context.Categories.FirstOrDefault(c => c.Id == category.Id);
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
      // Arrange

      var dbContext = ContextGenerator.Generator();

      var categoryRepository = new CategoryRepository(dbContext);

      var category = new Category
      {
        Id = 1,
        ParentID = 0,
        CategoryLevel = 1,
        CategoryName = "Test Category",
        Notes = "Test notes",
        IsDeleted = false
      };
      await dbContext.Categories.AddAsync(category);
      await dbContext.SaveChangesAsync();

      // Act
      var exists = categoryRepository.ExistsBy(c => c.CategoryName == "Test Category");

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
      // Arrange
      var dbContext = ContextGenerator.Generator();
      var categoryRepository = new CategoryRepository(dbContext);
      var category = new Category
      {
        Id = 1,
        ParentID = 0,
        CategoryLevel = 1,
        CategoryName = "Test Category",
        Notes = "Test notes",
        IsDeleted = false
      };
      await dbContext.Categories.AddAsync(category);
      await dbContext.SaveChangesAsync();

      // Act
      var exists = categoryRepository.ExistsBy(c => c.CategoryName == "Test Category 1");
      Assert.False(exists);
    }
    /// <summary>
    /// Test case thực hiện insert 5 bảng ghi category và kiểm tra lấy theo id thành công
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task FindByIdSuccess()
    {
      // Arrange
      var dbContext = ContextGenerator.Generator();
      var categoryRepository = new CategoryRepository(dbContext);
      List<Category> categories = new List<Category>
      {
        new Category { Id = 1, ParentID = 0, CategoryLevel = 1, CategoryName = "Test Category 1", Notes = "Test notes 1", IsDeleted = false },
        new Category { Id = 2, ParentID = 0, CategoryLevel = 1, CategoryName = "Test Category 2", Notes = "Test notes 2", IsDeleted = false },
        new Category { Id = 3, ParentID = 0, CategoryLevel = 1, CategoryName = "Test Category 3", Notes = "Test notes 3", IsDeleted = false },
        new Category { Id = 4, ParentID = 0, CategoryLevel = 1, CategoryName = "Test Category 4", Notes = "Test notes 4", IsDeleted = false },
        new Category { Id = 5, ParentID = 0, CategoryLevel = 1, CategoryName = "Test Category 5", Notes = "Test notes 5", IsDeleted = false }
      };
      await dbContext.Categories.AddRangeAsync(categories);
      await dbContext.SaveChangesAsync();

      // Act
      var result = categoryRepository.Get(u => u.Id == 1);

      // Assert
      Assert.NotNull(result);
      Assert.Equal("Test Category 1", result.CategoryName);
    }
    /// <summary>
    /// Test case thực hiện insert 5 bảng ghi category và kiểm tra lấy theo id  thất bại
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task FindByIdFail()
    {
      // Arrange
      var dbContext = ContextGenerator.Generator();
      var categoryRepository = new CategoryRepository(dbContext);
      List<Category> categories = new List<Category>
      {
        new Category { Id = 1, ParentID = 0, CategoryLevel = 1, CategoryName = "Test Category 1", Notes = "Test notes 1", IsDeleted = false },
        new Category { Id = 2, ParentID = 0, CategoryLevel = 1, CategoryName = "Test Category 2", Notes = "Test notes 2", IsDeleted = false },
        new Category { Id = 3, ParentID = 0, CategoryLevel = 1, CategoryName = "Test Category 3", Notes = "Test notes 3", IsDeleted = false },
        new Category { Id = 4, ParentID = 0, CategoryLevel = 1, CategoryName = "Test Category 4", Notes = "Test notes 4", IsDeleted = false },
        new Category { Id = 5, ParentID = 0, CategoryLevel = 1, CategoryName = "Test Category 5", Notes = "Test notes 5", IsDeleted = false }
      };
      await dbContext.Categories.AddRangeAsync(categories);
      await dbContext.SaveChangesAsync();

      // Act
      var result = categoryRepository.Get(u => u.Id == 6);

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
      var dbContext = ContextGenerator.Generator();
      var categoryRepository = new CategoryRepository(dbContext);
      var category = new Category
      {
        Id = 1,
        ParentID = 0,
        CategoryLevel = 1,
        CategoryName = "Test Category",
        Notes = "Test notes",
        IsDeleted = false
      };
      await dbContext.Categories.AddAsync(category);
      await dbContext.SaveChangesAsync();

      // Act
      categoryRepository.Remove(category);
      await dbContext.SaveChangesAsync();

      // Assert
      var deletedCategory = dbContext.Categories.FirstOrDefault(c => c.Id == category.Id);
      Assert.Null(deletedCategory);
    }
    /// <summary>
    /// test case thực hiện xóa category thất bại
    /// </summary>
    [Fact]
    public async Task DeleteFaild()
    {
      // Arrange
      var dbContext = ContextGenerator.Generator();
      var categoryRepository = new CategoryRepository(dbContext);
      var category = new Category
      {
        Id = 1,
        ParentID = 0,
        CategoryLevel = 1,
        CategoryName = "Test Category",
        Notes = "Test notes",
        IsDeleted = false
      };
      await dbContext.Categories.AddAsync(category);
      await dbContext.SaveChangesAsync();

      // Act
      categoryRepository.Remove(category);
      await dbContext.SaveChangesAsync();

      // Assert
      var deletedCategory = dbContext.Categories.FirstOrDefault(c => c.Id == category.Id);
      Assert.Null(deletedCategory);
    }
    /// <summary>
    /// test case lấy danh sách category thành công
    /// </summary>
    [Fact]
    public async Task GetAllSuccess()
    {
      // Arrange
      var dbContext = ContextGenerator.Generator();
      var categoryRepository = new CategoryRepository(dbContext);

      // Tạo ra danh sách category
      List<Category> categories = new List<Category>
      {
        new Category { Id = 1, ParentID = 0, CategoryLevel = 1, CategoryName = "Test Category 1", Notes = "Test notes 1", IsDeleted = false },
        new Category { Id = 2, ParentID = 0, CategoryLevel = 1, CategoryName = "Test Category 2", Notes = "Test notes 2", IsDeleted = false },
        new Category { Id = 3, ParentID = 0, CategoryLevel = 1, CategoryName = "Test Category 3", Notes = "Test notes 3", IsDeleted = false },
        new Category { Id = 4, ParentID = 0, CategoryLevel = 1, CategoryName = "Test Category 4", Notes = "Test notes 4", IsDeleted = false },
        new Category { Id = 5, ParentID = 0, CategoryLevel = 1, CategoryName = "Test Category 5", Notes = "Test notes 5", IsDeleted = false }
      };

      await dbContext.Categories.AddRangeAsync(categories);
      await dbContext.SaveChangesAsync();

      // kiểm tra có trả về 5 bảng ghi hay không

      // Act
      var result = categoryRepository.GetAll(null, null);

      // Assert
      Assert.NotNull(result);
      Assert.Equal(5, result.Count());
    }
    /// <summary>
    /// test case lấy danh sách category  thất bại
    /// </summary>
    [Fact]
    public async Task GetAllFaild()
    {
      // Arrange
      var dbContext = ContextGenerator.Generator();
      var categoryRepository = new CategoryRepository(dbContext);

      // Act
      var result = categoryRepository.GetAll(null, null);

      // Assert
      Assert.NotNull(result);
      Assert.Empty(result);
    }

  }
}
