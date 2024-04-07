using API.Model;
using ICHI_API.Helpers;
using ICHI_API.Model;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Model;
using Microsoft.AspNetCore.Mvc;

namespace ICHI_API.Service.IService
{
    public interface IInventoryReceiptService
    {
        PagedResult<InventoryReceipt> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage);
        List<Product> GetProductWithBatchNumber(out string strMessage);

        InventoryReceipt Create(InventoryReceiptDTO model, out string strMessage);
        InventoryReceipt Update(InventoryReceiptDTO model, out string strMessage);

        //InventoryReceipt Update(InventoryReceipt model, out string strMessage);

        InventoryReceiptDTO FindById(int id, out string strMessage);

        bool Delete(int id, out string strMessage);

    }
}
