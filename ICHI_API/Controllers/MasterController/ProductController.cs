using Microsoft.AspNetCore.Mvc;
using ICHI_CORE.Controllers.BaseController;
using ICHI_CORE.Entities;
using ICHI_CORE.Domain.MasterModel;
using System.Net.Http.Headers;
using ICHI_CORE.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using ICHI_CORE.Helpers;
using ICHI_API.Model;
using Microsoft.AspNetCore.Hosting;


namespace ICHI_CORE.Controllers.MasterController
{
  [ApiController]
  [Route("api/[controller]")]
  public class ProductController : BaseController<Product>
  {
    private readonly IWebHostEnvironment _webHostEnvironment;
    public ProductController(PcsApiContext context, IWebHostEnvironment webHostEnvironment) : base(context)
    {
      _webHostEnvironment = webHostEnvironment;
    }
    [HttpPost("Upsert/Product")]
    public async Task<ApiResponse<Product>> UpSert([FromForm] Product product, List<IFormFile>? files)
    {
      ApiResponse<Product> result;
      try
      {
        if (product.Id == 0)
        {
          var checkProduct = await _context.Products.FirstOrDefaultAsync(x => x.ProductName == product.ProductName);
          if (checkProduct != null)
          {
            result = new ApiResponse<Product>(System.Net.HttpStatusCode.Forbidden, "Product code already exists", null);
            return result;
          }

          product.CreateBy = "Admin";
          product.ModifiedBy = "Admin";

          await _context.Products.AddAsync(product);
          await _context.SaveChangesAsync();
          if (files != null && files.Count > 0)
          {
            foreach (var file in files)
            {
              var image = new ProductImages();
              image.ProductDetailId = product.Id;
              image.ImageName = file.FileName;
              image.ImagePath = ImageHelper.AddImage(_webHostEnvironment.WebRootPath, product.Id, file);
              image.IsDefault = false;
              image.IsActive = true;
              image.IsDeleted = false;
              image.CreateBy = "Admin";
              image.ModifiedBy = "Admin";
              await _context.ProductImages.AddAsync(image);
            }
            await _context.SaveChangesAsync();
          }
          result = new ApiResponse<Product>(System.Net.HttpStatusCode.OK, "Created successfully", product);
        }
        _context.Update(product);
        await _context.SaveChangesAsync();

        var productImages = await _context.ProductImages.Where(x => x.ProductDetailId == product.Id).ToListAsync();
        // thực hiện xóa ảnh cũ
        foreach (var item in productImages)
        {
          ImageHelper.DeleteImage(_webHostEnvironment.WebRootPath, item.ImagePath);
          _context.ProductImages.Remove(item);
        }
        if (files != null)
        {
          foreach (var file in files)
          {
            var image = new ProductImages();
            image.ProductDetailId = product.Id;

            image.ImageName = file.FileName;
            image.ImagePath = ImageHelper.AddImage(_webHostEnvironment.WebRootPath, product.Id, file);
            image.IsDefault = false;
            image.IsActive = true;
            image.IsDeleted = false;
            image.CreateBy = "Admin";
            image.ModifiedBy = "Admin";
            await _context.ProductImages.AddAsync(image);
          }
        }
        await _context.SaveChangesAsync();

        result = new ApiResponse<Product>(System.Net.HttpStatusCode.OK, "Created successfully", product);
      }
      catch (Exception ex)
      {
        result = new ApiResponse<Product>(System.Net.HttpStatusCode.ExpectationFailed, ex.ToString(), null);
      }
      return result;
    }
    [HttpGet("FindAllPaged")]
    public async Task<ActionResult<ApiResponse<ICHI_API.Helpers.PagedResult<Product>>>> GetAll(
            [FromQuery(Name = "search")] string name = "",
            [FromQuery(Name = "page-size")] int pageSize = 10,
            [FromQuery(Name = "page-number")] int pageNumber = 1,
            [FromQuery(Name = "sort-direction")] string sortDir = "desc",
            [FromQuery(Name = "sort-by")] string sortBy = "Id")
    {
      ApiResponse<ICHI_API.Helpers.PagedResult<Product>> result;
      try
      {
        var query = _context.Products.Include(u => u.Category).AsQueryable().Where(u => u.isDeleted == false);

        if (!string.IsNullOrEmpty(name))
        {
          query = query.Where(e => e.ProductName.Contains(name));
        }

        var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
        query = query.OrderBy(orderBy);
        var pagedResult = await ICHI_API.Helpers.PagedResult<Product>.CreatePagedResultAsync(query, pageNumber, pageSize);

        result = new ApiResponse<ICHI_API.Helpers.PagedResult<Product>>(
             System.Net.HttpStatusCode.OK,
             "Retrieved successfully",
             pagedResult
         );
      }
      catch (Exception ex)
      {
        result = new ApiResponse<ICHI_API.Helpers.PagedResult<Product>>(System.Net.HttpStatusCode.ExpectationFailed, ex.ToString(), null);
      }
      return result;
    }
    // Lấy ra sản phẩm theo id
    [HttpGet("GetProductById/{id}")]
    public async Task<ApiResponse<ProductDTO>> GetProductById(int id)
    {
      ApiResponse<ProductDTO> result;
      try
      {
        ProductDTO data = new ProductDTO
        {
          Product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id),
          ProductImages = await _context.ProductImages.Where(x => x.ProductDetailId == id).ToListAsync(),
          CategoryProduct = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id)
        };
        if (data == null)
        {
          result = new ApiResponse<ProductDTO>(System.Net.HttpStatusCode.Forbidden, "Product does not exist! ", null);
          return result;
        }
        else
        {
          return new ApiResponse<ProductDTO>(System.Net.HttpStatusCode.OK, "Get Product success", data);
        }
      }
      catch (Exception ex)
      {
        result = new ApiResponse<ProductDTO>(System.Net.HttpStatusCode.ExpectationFailed, ex.ToString(), null);
      }
      return result;
    }
    // xóa productimage theo productId và imageName
    [HttpDelete("Delete-Image/{productId}/{imageName}")]
    public async Task<ApiResponse<ProductImages>> DeleteProductImage(int productId, string imageName)
    {
      ApiResponse<ProductImages> result;
      try
      {
        var productImage = await _context.ProductImages.FirstOrDefaultAsync(x => x.ProductDetailId == productId && x.ImageName == imageName);
        if (productImage == null)
        {
          result = new ApiResponse<ProductImages>(System.Net.HttpStatusCode.Forbidden, "ProductImage does not exist! ", null);
          return result;
        }
        if (!ImageHelper.DeleteImage(_webHostEnvironment.WebRootPath, productImage.ImagePath))
        {
          result = new ApiResponse<ProductImages>(System.Net.HttpStatusCode.Forbidden, "Xóa ảnh không thành công! ", null);
          return result;
        }
        _context.ProductImages.Remove(productImage);
        await _context.SaveChangesAsync();
        result = new ApiResponse<ProductImages>(System.Net.HttpStatusCode.OK, "Delete ProductImage success", productImage);
      }
      catch (Exception ex)
      {
        result = new ApiResponse<ProductImages>(System.Net.HttpStatusCode.ExpectationFailed, ex.ToString(), null);
      }
      return result;
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<Product>>> Delete(int id)
    {
      try
      {
        var data = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
        data.isDeleted = true;
        data.ModifiedDate = DateTime.Now;
        data.ModifiedBy = "Admin";
        await Update(data);
        var result = new ApiResponse<Product>(System.Net.HttpStatusCode.OK, "", data);
        return Ok(result);
      }
      catch (Exception ex)
      {
        return BadRequest(new ApiResponse<Product>(System.Net.HttpStatusCode.BadRequest, ex.Message, null));
      }
    }
  }

}
