using API.Model;
using ICHI_API.Helpers;
using ICHI_API.Model;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Model;
using Microsoft.AspNetCore.Mvc;

namespace ICHI_API.Service.IService
{
    public interface IEmployeeService
    {
        PagedResult<Employee> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage);

        Employee Create(Employee customer, out string strMessage);

        Employee Update(Employee customer, IFormFile? file, out string strMessage);

        Employee FindById(int id, out string strMessage);

        bool Delete(int id, out string strMessage);
    }
}
