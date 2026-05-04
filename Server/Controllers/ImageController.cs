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
        private readonly ICloudinaryService _cloudinary;

        public ImageController(IProductImageService service, ICloudinaryService cloudinary)
        {
            _service = service;
            _cloudinary = cloudinary;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest(new ApiResponse
                {
                    Code = 400,
                    Success = false,
                    Message = "No files provided",
                    Data = null
                });
            }

            try
            {
                var urls = await _cloudinary.UploadMultipleAsync(files);

                if (urls == null || urls.Count == 0)
                {
                    return BadRequest(new ApiResponse
                    {
                        Code = 400,
                        Success = false,
                        Message = "Upload failed",
                        Data = null
                    });
                }

                return Ok(new ApiResponse
                {
                    Code = 200,
                    Success = true,
                    Message = "Images uploaded successfully",
                    Data = urls
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Code = 500,
                    Success = false,
                    Message = "Server error during upload",
                    Data = ex.Message
                });
            }
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