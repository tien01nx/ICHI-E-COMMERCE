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
  public class CustomerController : BaseController<Customer>
  {
    public CustomerController(PcsApiContext context) : base(context) { }

    [HttpGet("FindAllPaged")]
    public async Task<ActionResult<ApiResponse<ICHI_API.Helpers.PagedResult<Customer>>>> GetAll(
                    [FromQuery(Name = "search")] string name = "",
                    [FromQuery(Name = "page-size")] int pageSize = 10,
                    [FromQuery(Name = "page-number")] int pageNumber = 1,
                    [FromQuery(Name = "sort-direction")] string sortDir = "desc",
                    [FromQuery(Name = "sort-by")] string sortBy = "Id")
    {
      ApiResponse<ICHI_API.Helpers.PagedResult<Customer>> result;
      try
      {
        var query = _context.Customers.AsQueryable().Where(u => !u.isDeleted);

        if (!string.IsNullOrEmpty(name))
        {
          query = query.Where(e => e.FullName.Contains(name));
        }

        var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
        query = query.OrderBy(orderBy);

        var pagedResult = await ICHI_API.Helpers.PagedResult<Customer>.CreatePagedResultAsync(query, pageNumber, pageSize);

        result = new ApiResponse<ICHI_API.Helpers.PagedResult<Customer>>(
             System.Net.HttpStatusCode.OK,
             "Retrieved successfully",
             pagedResult
         );
      }
      catch (Exception ex)
      {
        result = new ApiResponse<ICHI_API.Helpers.PagedResult<Customer>>(System.Net.HttpStatusCode.ExpectationFailed, ex.ToString(), null);
      }
      return result;
    }

    // API v1/Create
    [HttpPost("Create-Customer")]
    public async Task<ApiResponse<Customer>> CreateSupplỉer(Customer customer)
    {
      ApiResponse<Customer> result;
      try
      {
        // kiểm tra xem mã nhà cung cấp đã tồn tại chưa
        var checkCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.PhoneNumber == customer.PhoneNumber);
        if (checkCustomer != null)
        {
          result = new ApiResponse<Customer>(System.Net.HttpStatusCode.Forbidden, "Số điện thoại đã tồn tại", null);
          return result;
        }
        customer.CreateBy = "Admin";
        customer.ModifiedBy = "Admin";

        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
        var data = await GetByKeys(customer);
        result = new ApiResponse<Customer>(System.Net.HttpStatusCode.OK, "Created successfully", data);
      }
      catch (Exception ex)
      {
        result = new ApiResponse<Customer>(System.Net.HttpStatusCode.ExpectationFailed, ex.ToString(), null);
      }
      return result;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<Customer>>> Delete(int id)
    {
      try
      {
        var data = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
        data.isDeleted = true;
        data.ModifiedDate = DateTime.Now;
        data.ModifiedBy = "Admin";
        _context.Customers.Update(data);
        await _context.SaveChangesAsync();
        var result = new ApiResponse<Customer>(System.Net.HttpStatusCode.OK, "", data);
        return Ok(result);
      }
      catch (Exception ex)
      {
        return BadRequest(new ApiResponse<Customer>(System.Net.HttpStatusCode.BadRequest, ex.Message, null));
      }
    }

    //[HttpDelete("{id}")]
    //public async Task<ActionResult<ApiResponse<Customer>>> Delete(int id)
    //{
    //    try
    //    {
    //        var data = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
    //        data.UpdateDatetime = DateTime.Now;
    //        data.UpdateUserId = "Admin";
    //        _context.Customers.Update(data);
    //        await _context.SaveChangesAsync();
    //        var result = new ApiResponse<Customer>(System.Net.HttpStatusCode.OK, "", data);
    //        return Ok(result);
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(new ApiResponse<Customer>(System.Net.HttpStatusCode.BadRequest, ex.Message, null));
    //    }
    //}
  }
}
