using Microsoft.AspNetCore.Mvc;
using Server.DTOs.Common;
using Server.Entities;
using Server.Services;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _service;

        public InventoryController(IInventoryService service)
        {
            _service = service;
        }

        [HttpPost("stock-in")]
        public async Task<IActionResult> StockIn([FromBody] InventoryMaster entity)
        {
            if (entity == null || entity.product_id <= 0 || entity.quantity <= 0)
            {
                return BadRequest(new ApiResponse
                {
                    Code = 400,
                    Success = false,
                    Message = "Invalid input data",
                    Data = null
                });
            }

            await _service.StockIn(entity.product_id, entity.quantity, 1);

            return Ok(new ApiResponse
            {
                Code = 200,
                Success = true,
                Message = "Stock added successfully",
                Data = null
            });
        }

        [HttpPost("stock-out")]
        public async Task<IActionResult> StockOut([FromBody] InventoryMaster entity)
        {
            if (entity == null || entity.product_id <= 0 || entity.quantity <= 0)
            {
                return BadRequest(new ApiResponse
                {
                    Code = 400,
                    Success = false,
                    Message = "Invalid input data",
                    Data = null
                });
            }

            await _service.StockOut(entity.product_id, entity.quantity, 1);

            return Ok(new ApiResponse
            {
                Code = 200,
                Success = true,
                Message = "Stock reduced successfully",
                Data = null
            });
        }
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetInventoryByProductId(int productId)
        {
            if (productId <= 0)
            {
                return BadRequest(new ApiResponse
                {
                    Code = 400,
                    Success = false,
                    Message = "Invalid product id",
                    Data = null
                });
            }

            var inventory = await _service.GetInventoryByProductId(productId);

            if (inventory == null)
            {
                return NotFound(new ApiResponse
                {
                    Code = 404,
                    Success = false,
                    Message = "Inventory not found",
                    Data = null
                });
            }

            return Ok(new ApiResponse
            {
                Code = 200,
                Success = true,
                Message = "Inventory fetched successfully",
                Data = inventory
            });
        }
    }
}