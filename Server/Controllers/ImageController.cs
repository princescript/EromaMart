using Microsoft.AspNetCore.Mvc;
using Server.DTOs.Common;
using Server.Services;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IProductImageService _service;

        public ImageController(IProductImageService service)
        {
            _service = service;
        }

        [HttpGet("product/{productId}/images")]
        public async Task<IActionResult> GetByProductId(int productId)
        {
            if (productId <= 0)
            {
                return BadRequest(new ApiResponse
                {
                    Code = 400,
                    Success = false,
                    Message = "Invalid product id. Must be greater than 0",
                    Data = null
                });
            }

            var result = await _service.FindAsync(x => x.product_id == productId);

            return Ok(new ApiResponse
            {
                Code = 200,
                Success = true,
                Message = "Images fetched successfully",
                Data = result
            });
        }
        [HttpPut("product/images/{imageId}/set-default")]
        public async Task<IActionResult> SetDefault(int imageId)
        {
            await _service.SetDefault(imageId);

            return Ok(new ApiResponse
            {
                Code = 200,
                Success = true,
                Message = "Image set as default successfully"
            });
        }

        [HttpDelete("product/images/{productId}/product-id")]
        public async Task<IActionResult> DeleteImageAll(int productId)
        {
            await _service.DeleteAllAsync(productId);

            return Ok(new ApiResponse
            {
                Code = 200,
                Success = true,
                Message = "All Image deleted successfully"
            });
        }

        [HttpDelete("product/images/{imageId}/image-id")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            await _service.DeleteAsync(imageId);

            return Ok(new ApiResponse
            {
                Code = 200,
                Success = true,
                Message = "Image deleted successfully"
            });
        }
    }
}