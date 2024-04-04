using ICHI_API.Helpers;
using ICHI_CORE.Domain.MasterModel;

namespace ICHI_API.Service.IService
{
  public interface IPromotionService
  {
    PagedResult<Promotion> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage);

    PromotionDTO Create(PromotionDTO model, out string strMessage);

    PromotionDTO Update(PromotionDTO model, out string strMessage);

    PromotionDTO FindById(int id, out string strMessage);

    bool Delete(int id, out string strMessage);

    bool CheckProductPromotion(IQueryable<PromotionDetail> data, DateTime StartTime, DateTime EndTime);

    IEnumerable<PromotionDetail> CheckPromotionActive();

  }
}
