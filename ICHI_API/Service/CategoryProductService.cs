using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Helpers;
using ICHI_API.Model;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using System.Data;
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
        var query = _unitOfWork.Category.GetAll(u => !u.IsDeleted).AsQueryable();
        if (!string.IsNullOrEmpty(name))
        {
          query = query.Where(e => e.CategoryName.Contains(name.Trim()));
        }
        var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
        query = query.OrderBy(orderBy);
        var pagedResult = Helpers.PagedResult<Category>.CreatePagedResult(query, pageNumber, pageSize);
        return pagedResult;
      }
      catch (Exception ex)
      {
        throw;
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
          throw new BadRequestException(Constants.CATEGORYNOTFOUND);
        }
        return data;
      }
      catch (Exception ex)
      {
        throw;
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
          throw new BadRequestException(Constants.CATEGORYEXIST);
        }
        var categoryParentId = _unitOfWork.Category.Get(u => u.Id == category.ParentID);
        if (categoryParentId == null)
        {
          throw new BadRequestException(Constants.CATEGORYPARENTNOTFOUND);
        }
        category.CategoryLevel = categoryParentId.CategoryLevel + 1;
        category.CreateBy = "Admin";
        category.ModifiedBy = "Admin";
        _unitOfWork.Category.Add(category);
        _unitOfWork.Save();
        strMessage = Constants.ADDCATEGORYSUCCESS;
        return category;
      }
      catch (Exception ex)
      {
        throw;
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
          throw new BadRequestException(Constants.CATEGORYNOTFOUND);
        }
        var checkCategory = _unitOfWork.Category.Get(u => u.CategoryName == category.CategoryName && u.Id != category.Id);
        if (checkCategory != null)
        {

          throw new BadRequestException(Constants.CATEGORYEXIST);
        }

        var categoryParentId = _unitOfWork.Category.Get(u => u.Id == category.ParentID);
        if (categoryParentId == null)
        {
          throw new BadRequestException(Constants.CATEGORYPARENTNOTFOUND);
        }
        category.CategoryLevel = categoryParentId.CategoryLevel + 1;
        category.ModifiedBy = "Admin";
        _unitOfWork.Category.Update(category);
        _unitOfWork.Save();
        strMessage = Constants.UPDATECATEGORYSUCCESS;
        return category;
      }
      catch (Exception)
      {
        throw;
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
          throw new BadRequestException(Constants.CATEGORYNOTFOUND);
        }
        data.IsDeleted = true;
        data.ModifiedDate = DateTime.Now;
        _unitOfWork.Category.Update(data);
        _unitOfWork.Save();
        strMessage = Constants.DELETECATEGORYSUCCESS;
        return true;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public List<Category> GetCategories(int parentID)
    {
      List<Category> reuslt = new List<Category>();
      if (parentID >= 0)
      {
        Category parent = _unitOfWork.Category.Get(u => u.Id == parentID && !u.IsDeleted);
        if (parent != null)
        {
          reuslt.Add(parent);
          List<Category> childByParents = _unitOfWork.Category.GetAll(x => !x.IsDeleted && x.ParentID == parentID).ToList();
          if (childByParents.Count > 0)
          {
            foreach (var item in childByParents)
            {
              List<Category> childs = GetCategories(item.Id);
              reuslt.AddRange(childs);
            }
          }
        }
      }

      return reuslt;
    }

    public List<Category> FindAll()
    {
      return _unitOfWork.Category.GetAll(u => !u.IsDeleted).ToList();
    }

    public List<Category> GetCategoriesByParentID(string categoryName, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var model = _unitOfWork.Category.Get(u => u.CategoryName == categoryName && !u.IsDeleted);
        var data = GetCategories(model.Id);
        return data;

      }
      catch (Exception)
      {
        throw;
      }
    }

    public void FindCategoryLevels(Category category, List<Category> categoryLevels)
    {
      categoryLevels.Add(category);

      var parentCategory = _unitOfWork.Category.Get(u => u.Id == category.ParentID && !u.IsDeleted);
      if (parentCategory != null)
      {
        FindCategoryLevels(parentCategory, categoryLevels);
      }
    }

    public List<Category> GetCategoryLevels(Category category)
    {
      List<Category> categoryLevels = new List<Category>();
      FindCategoryLevels(category, categoryLevels);
      categoryLevels.Sort((x, y) => x.ParentID.CompareTo(y.ParentID));
      return categoryLevels;
    }

    public DataTable GetData()
    {
      // Lấy dữ liệu từ cơ sở dữ liệu bằng hàm GetDataTableFromSQL
      var dataTable = _unitOfWork
        .Category
        .GetDataTableFromSQL
        ("  Select * from Products join Categories on Products.CategoryId = Categories.Id");
      return dataTable;
    }

  }
}
