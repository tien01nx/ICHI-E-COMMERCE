using Microsoft.AspNetCore.Mvc;
using ICHI_CORE.Controllers.BaseController;
using ICHI_CORE.Entities;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Helpers;
using ICHI_CORE.Model;
using ICHI_CORE.NlogConfig;
using Microsoft.EntityFrameworkCore;

namespace ICHI_CORE.Controllers.MasterController
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductImagesController : BaseController<ProductImages>
    {
        public ProductImagesController(PcsApiContext context) : base(context) { }


        [HttpPost]
        [Route("UploadImage")]
        public async Task<ApiResponse<String>> UploadImage([FromBody] List<ProductImages>? productImages)
        {
            ApiResponse<String> result;
            try
            {
                byte[] imageData;
                string base64ImageData;
                int imageInsert = 0;
                int imageDelete = 0;

                foreach (var item in productImages)
                {
                    var product = _context.Products.Where(x => x.Id == item.ProductId).FirstOrDefault();
                    if (product == null)
                    {
                        result = new ApiResponse<string>(System.Net.HttpStatusCode.Forbidden, "Product does not exist! ", null);
                        return result;
                    }
                    else
                    {
                        imageData = System.IO.File.ReadAllBytes(item.ImagePath);
                        base64ImageData = Convert.ToBase64String(imageData);
                        // kiểm tra trong productimages đã có hình ảnh này chưa nếu có thì xóa đi
                        var productImage = _context.ProductImages.Where(x => x.ProductId == item.ProductId && x.ImageName == Path.GetFileName(item.ImagePath)).FirstOrDefault();

                        if (productImage != null)
                        {
                            imageDelete++;
                            _context.ProductImages.Remove(productImage);
                        }
                    }
                    item.ImageName = Path.GetFileName(item.ImagePath);
                    item.ImagePath = $"data:image/png;base64,{base64ImageData}";
                    item.IsActive = true;
                    item.IsDeleted = false;
                    _context.ProductImages.AddAsync(item);
                    imageInsert++;
                }
                _context.SaveChangesAsync();

                return new ApiResponse<string>(System.Net.HttpStatusCode.OK, $"Insert ProductImage success | insert {imageInsert} | delete {imageDelete}", null);
            }
            catch (Exception ex)
            {
                NLogger.log.Error(ex.ToString());
                result = new ApiResponse<String>(System.Net.HttpStatusCode.ExpectationFailed, ex.ToString(), null);
            }
            return result;
        }


        [HttpGet]
        [Route("GetProductImages/{productId}")]
        public async Task<ApiResponse<List<ProductImages>>> GetProductImages(int productId)
        {
            ApiResponse<List<ProductImages>> result;
            try
            {
                var productImages = _context.ProductImages.Where(x => x.ProductId == productId).ToList();
                if (productImages == null)
                {
                    result = new ApiResponse<List<ProductImages>>(System.Net.HttpStatusCode.Forbidden, "Product does not exist! ", null);
                    return result;
                }
                else
                {
                    return new ApiResponse<List<ProductImages>>(System.Net.HttpStatusCode.OK, "Get ProductImages success", productImages);
                }
            }
            catch (Exception ex)
            {
                NLogger.log.Error(ex.ToString());
                result = new ApiResponse<List<ProductImages>>(System.Net.HttpStatusCode.ExpectationFailed, ex.ToString(), null);
            }
            return result;

        }

        [HttpDelete("{id}")]

        public async Task<ActionResult<ApiResponse<ProductImages>>> Delete(int id)
        {
            try
            {
                var data = await _context.ProductImages.FirstOrDefaultAsync(x => x.Id == id);
                data.IsDeleted = true;
                data.UpdateDatetime = DateTime.Now;
                data.UpdateUserId = "Admin";
                _context.ProductImages.Update(data);
                await _context.SaveChangesAsync();
                var result = new ApiResponse<ProductImages>(System.Net.HttpStatusCode.OK, "", data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<ProductImages>(System.Net.HttpStatusCode.BadRequest, ex.Message, null));
            }
        }
    }
}