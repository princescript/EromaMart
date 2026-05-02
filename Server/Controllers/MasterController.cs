using Microsoft.AspNetCore.Mvc;
using Server.DTOs.Common;
using Server.Entities;
using Server.Services;

namespace Server.Controllers
{
    [Route("api/master")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly IBrandService _brandService;
        private readonly ICategoryService _categoryService;

        public MasterController(
            IBrandService brandService,
            ICategoryService categoryService)
        {
            _brandService = brandService;
            _categoryService = categoryService;
        }

        // ================= BRAND =================

        [HttpGet("brands")]
        public async Task<IActionResult> GetBrands()
        {
            var result = await _brandService.FindAsync(x => x.is_active);

            return Ok(new ApiResponse
            {
                Code = 200,
                Success = true,
                Message = "Brands fetched successfully",
                Data = result
            });
        }

        [HttpGet("brands/{id:int}")]
        public async Task<IActionResult> GetBrandById(int id)
        {
            var result = await _brandService.FindAsync(x => x.brand_id == id);
            var brand = result.FirstOrDefault();

            if (brand == null)
                return NotFound(new ApiResponse
                {
                    Code = 404,
                    Success = false,
                    Message = "Brand not found"
                });

            return Ok(new ApiResponse
            {
                Code = 200,
                Success = true,
                Message = "Brand fetched successfully",
                Data = brand
            });
        }

        [HttpPost("brands")]
        public async Task<IActionResult> CreateBrand([FromBody] BrandMaster brand)
        {
            if (brand == null)
                return BadRequest(new ApiResponse
                {
                    Code = 400,
                    Success = false,
                    Message = "Invalid request data"
                });

            await _brandService.AddAsync(brand);

            return Ok(new ApiResponse
            {
                Code = 200,
                Success = true,
                Message = "Brand created successfully",
                Data = brand
            });
        }

        [HttpPut("brands/{id:int}")]
        public async Task<IActionResult> UpdateBrand(int id, [FromBody] BrandMaster brand)
        {
            if (brand == null)
                return BadRequest(new ApiResponse
                {
                    Code = 400,
                    Success = false,
                    Message = "Invalid request data"
                });

            await _brandService.UpdateAsync(id, brand);

            return Ok(new ApiResponse
            {
                Code = 200,
                Success = true,
                Message = "Brand updated successfully",
                Data = brand
            });
        }

        [HttpDelete("brands/{id:int}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            await _brandService.DeleteAsync(id);

            return Ok(new ApiResponse
            {
                Code = 200,
                Success = true,
                Message = "Brand deleted successfully"
            });
        }

        // ================= CATEGORY =================

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _categoryService.FindAsync(x => x.is_active);

            return Ok(new ApiResponse
            {
                Code = 200,
                Success = true,
                Message = "Categories fetched successfully",
                Data = result
            });
        }

        [HttpGet("categories/{id:int}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var result = await _categoryService.FindAsync(x => x.category_id == id);
            var category = result.FirstOrDefault();

            if (category == null)
                return NotFound(new ApiResponse
                {
                    Code = 404,
                    Success = false,
                    Message = "Category not found"
                });

            return Ok(new ApiResponse
            {
                Code = 200,
                Success = true,
                Message = "Category fetched successfully",
                Data = category
            });
        }

        [HttpPost("categories")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryMaster category)
        {
            if (category == null)
                return BadRequest(new ApiResponse
                {
                    Code = 400,
                    Success = false,
                    Message = "Invalid request data"
                });

            await _categoryService.AddAsync(category);

            return Ok(new ApiResponse
            {
                Code = 200,
                Success = true,
                Message = "Category created successfully",
                Data = category
            });
        }

        [HttpPut("categories/{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryMaster category)
        {
            if (category == null)
                return BadRequest(new ApiResponse
                {
                    Code = 400,
                    Success = false,
                    Message = "Invalid request data"
                });

            await _categoryService.UpdateAsync(id, category);

            return Ok(new ApiResponse
            {
                Code = 200,
                Success = true,
                Message = "Category updated successfully",
                Data = category
            });
        }

        [HttpDelete("categories/{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteAsync(id);

            return Ok(new ApiResponse
            {
                Code = 200,
                Success = true,
                Message = "Category deleted successfully"
            });
        }
    }
}