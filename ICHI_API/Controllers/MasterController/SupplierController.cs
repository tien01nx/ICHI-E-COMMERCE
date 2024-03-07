using Microsoft.AspNetCore.Mvc;
using ICHI_CORE.Controllers.BaseController;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using ICHI.DataAccess.Data;
namespace ICHI_CORE.Controllers.MasterController
{
  [ApiController]
  [Route("api/[controller]")]
  public class SupplierController : BaseController<Supplier>
  {
    public SupplierController(PcsApiContext context) : base(context) { }

    [HttpGet("FindAllPaged")]
    public async Task<ActionResult<ApiResponse<ICHI_API.Helpers.PagedResult<Supplier>>>> GetAll(
                    [FromQuery(Name = "search")] string name = "",
                    [FromQuery(Name = "page-size")] int pageSize = 10,
                    [FromQuery(Name = "page-number")] int pageNumber = 1,
                    [FromQuery(Name = "sort-direction")] string sortDir = "desc",
                    [FromQuery(Name = "sort-by")] string sortBy = "Id")
    {
      ApiResponse<ICHI_API.Helpers.PagedResult<Supplier>> result;
      try
      {
        var query = _context.Suppliers.AsQueryable().Where(u => u.isDeleted == false);

        if (!string.IsNullOrEmpty(name))
        {
          query = query.Where(e => e.SupplierName.Contains(name));
        }

        var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
        query = query.OrderBy(orderBy);

        var pagedResult = await ICHI_API.Helpers.PagedResult<Supplier>.CreatePagedResultAsync(query, pageNumber, pageSize);

        result = new ApiResponse<ICHI_API.Helpers.PagedResult<Supplier>>(
             System.Net.HttpStatusCode.OK,
             "Retrieved successfully",
             pagedResult
         );
      }
      catch (Exception ex)
      {
        result = new ApiResponse<ICHI_API.Helpers.PagedResult<Supplier>>(System.Net.HttpStatusCode.ExpectationFailed, ex.ToString(), null);
      }
      return result;
    }

    // API v1/Create
    [HttpPost("Create-Supplier")]
    public async Task<ApiResponse<Supplier>> CreateSupplỉer([FromBody] Supplier supplier)
    {
      ApiResponse<Supplier> result;
      try
      {
        // kiểm tra xem mã nhà cung cấp đã tồn tại chưa
        var checkSupplier = await _context.Suppliers.FirstOrDefaultAsync(x => x.SupplierName == supplier.SupplierName);
        if (checkSupplier != null)
        {
          result = new ApiResponse<Supplier>(System.Net.HttpStatusCode.Forbidden, "Supplier code already exists", null);
          return result;
        }
        // kiểm tra email nhà cung cấp đã tồn tại chưa
        var checkEmail = await _context.Suppliers.FirstOrDefaultAsync(x => x.Email == supplier.Email);
        if (checkEmail != null)
        {
          result = new ApiResponse<Supplier>(System.Net.HttpStatusCode.Forbidden, "Email already exists", null);
          return result;
        }
        // kiểm tra số điện thoại nhà cung cấp đã tồn tại chưa
        var checkPhone = await _context.Suppliers.FirstOrDefaultAsync(x => x.PhoneNumber == supplier.PhoneNumber);
        if (checkPhone != null)
        {
          result = new ApiResponse<Supplier>(System.Net.HttpStatusCode.Forbidden, "Phone already exists", null);
          return result;
        }
        // kiêm tra mã số thueé
        var checkTaxCode = await _context.Suppliers.FirstOrDefaultAsync(x => x.TaxCode == supplier.TaxCode);
        if (checkTaxCode != null)
        {
          result = new ApiResponse<Supplier>(System.Net.HttpStatusCode.Forbidden, "Tax code already exists", null);
          return result;
        }
        supplier.CreateBy = "Admin";
        supplier.ModifiedBy = "Admin";

        await _context.Suppliers.AddAsync(supplier);
        await _context.SaveChangesAsync();
        var data = await GetByKeys(supplier);
        result = new ApiResponse<Supplier>(System.Net.HttpStatusCode.OK, "Created successfully", data);
      }
      catch (Exception ex)
      {
        result = new ApiResponse<Supplier>(System.Net.HttpStatusCode.ExpectationFailed, ex.ToString(), null);
      }
      return result;
    }


    [HttpPost("Update-Supplier")]
    public async Task<ApiResponse<Supplier>> UpdateSupplỉer([FromBody] Supplier supplier)
    {
      ApiResponse<Supplier> result;
      try
      {
        // kiểm tra xem mã nhà cung cấp đã tồn tại chưa
        var checkSupplier = await _context.Suppliers.FirstOrDefaultAsync(x => x.SupplierName == supplier.SupplierName);
        if (checkSupplier != null)
        {
          result = new ApiResponse<Supplier>(System.Net.HttpStatusCode.Forbidden, "Supplier code already exists", null);
          return result;
        }
        // kiểm tra email nhà cung cấp đã tồn tại chưa
        var checkEmail = await _context.Suppliers.FirstOrDefaultAsync(x => x.Email == supplier.Email);
        if (checkEmail != null)
        {
          result = new ApiResponse<Supplier>(System.Net.HttpStatusCode.Forbidden, "Email already exists", null);
          return result;
        }
        // kiểm tra số điện thoại nhà cung cấp đã tồn tại chưa
        var checkPhone = await _context.Suppliers.FirstOrDefaultAsync(x => x.PhoneNumber == supplier.PhoneNumber);
        if (checkPhone != null)
        {
          result = new ApiResponse<Supplier>(System.Net.HttpStatusCode.Forbidden, "Phone already exists", null);
          return result;
        }
        // kiêm tra mã số thueé
        var checkTaxCode = await _context.Suppliers.FirstOrDefaultAsync(x => x.TaxCode == supplier.TaxCode);
        if (checkTaxCode != null)
        {
          result = new ApiResponse<Supplier>(System.Net.HttpStatusCode.Forbidden, "Tax code already exists", null);
          return result;
        }
        supplier.CreateBy = "Admin";
        supplier.ModifiedBy = "Admin";
        await Update(supplier);
        var data = await GetByKeys(supplier);
        result = new ApiResponse<Supplier>(System.Net.HttpStatusCode.OK, "Created successfully", data);
      }
      catch (Exception ex)
      {
        result = new ApiResponse<Supplier>(System.Net.HttpStatusCode.ExpectationFailed, ex.ToString(), null);
      }
      return result;
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<Supplier>>> Delete(int id)
    {
      try
      {
        var data = await _context.Suppliers.FirstOrDefaultAsync(x => x.Id == id);
        data.isDeleted = true;
        data.ModifiedDate = DateTime.Now;
        data.ModifiedBy = "Admin";
        _context.Suppliers.Update(data);
        await _context.SaveChangesAsync();
        var result = new ApiResponse<Supplier>(System.Net.HttpStatusCode.OK, "", data);
        return Ok(result);
      }
      catch (Exception ex)
      {
        return BadRequest(new ApiResponse<Supplier>(System.Net.HttpStatusCode.BadRequest, ex.Message, null));
      }
    }
  }
}
