using Microsoft.AspNetCore.Mvc;
using Server.DTOs.Common;
using Server.Entities;
using Server.Services;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.FindAsync(x => x.is_active == true);

            return Ok(new ApiResponse
            {
                Code = 200,
                Success = true,
                Message = "Categories fetched successfully.",
                Data = result
            });
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var categories = await _service.FindAsync(x => x.category_id == id);
            var category = categories.FirstOrDefault();

            if (category == null)
            {
                return NotFound(new ApiResponse
                {
                    Code = 404,
                    Success = false,
                    Message = "Category not found.",
                    Data = null
                });
            }

            return Ok(new ApiResponse
            {
                Code = 200,
                Success = true,
                Message = "Category fetched successfully.",
                Data = category
            });
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryMaster category)
        {
            await _service.AddAsync(category);

            return Ok(new ApiResponse
            {
                Code = 200,
                Success = true,
                Message = "Category created successfully.",
                Data = category
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryMaster category)
        {
            await _service.UpdateAsync(id, category);

            return Ok(new ApiResponse
            {
                Code = 200,
                Success = true,
                Message = "Category updated successfully.",
                Data = category
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok(new ApiResponse
            {
                Code = 200,
                Success = true,
                Message = "Category deleted successfully.",
                Data = null
            });
        }
    }
}