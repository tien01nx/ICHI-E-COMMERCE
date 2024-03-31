using API.Model;
using ICHI_API.Helpers;
using ICHI_API.Model;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Model;
using Microsoft.AspNetCore.Mvc;

namespace ICHI_API.Service.IService
{
    public interface IPromotionService
    {
        PagedResult<Promotion> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage);

        PromotionDTO Create(PromotionDTO model, out string strMessage);

        PromotionDTO Update(PromotionDTO model, out string strMessage);

        Promotion FindById(int id, out string strMessage);

        bool Delete(int id, out string strMessage);

    }
}
