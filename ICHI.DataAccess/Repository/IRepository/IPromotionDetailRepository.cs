using ICHI_CORE.Domain.MasterModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ICHI.DataAccess.Repository.IRepository
{
  public interface IPromotionDetailRepository : IRepository<PromotionDetail>
  {

    void Update(PromotionDetail obj);

    bool ExistsBy(Expression<Func<PromotionDetail, bool>> filter);

  }
}
