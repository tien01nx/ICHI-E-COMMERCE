using API.Model;
using ICHI_API.Helpers;
using ICHI_API.Model;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Model;
using Microsoft.AspNetCore.Mvc;

namespace ICHI_API.Service.IService
{
    public interface ICategoryProductService
    {
        PagedResult<Category> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage);
        List<Category> FindAll();

        Category Create(Category supplier, out string strMessage);

        Category Update(Category supplier, out string strMessage);

        Category FindById(int id, out string strMessage);

        bool Delete(int id, out string strMessage);

        List<Category> GetCategories(int parentID);

        List<Category> GetCategoriesByParentID(string categoryName, out string strMessage);

    }
}
