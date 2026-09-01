using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs.Common;
using Server.Entities;
using Server.Helpers;
using Server.Services;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IRedisKeyHelper _redisKeyHelper;
        private readonly IRedisService _redisService;


        public ProductsController(IProductService service,
            IRedisKeyHelper redisKeyHelper,
            IRedisService redisService)
        {
            _service = service;
            _redisKeyHelper = redisKeyHelper;
            _redisService = redisService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 20 : Math.Min(pageSize, 100);

            var key = _redisKeyHelper.Generate();

            var cached = await _redisService.GetAsync<IEnumerable<ProductMaster>>(key);

            if(cached != null){

                return Ok(new ApiResponse
                {
                    Code = 200,
                    Success = true,
                    Message = "Products fetched successfully. Cache Hit",
                    Data = cached
                });
            }
            var result = await _service.FindPagedAsync(
             x => string.IsNullOrEmpty(search)
             || x.product_name.Contains(search)
             || x.sku.Contains(search),
            page,
            pageSize);

            await _redisService.SetAsync<IEnumerable<ProductMaster>>(key, result, TimeSpan.FromMinutes(5));

            return Ok(new ApiResponse
            {
                Code = 200,
                Success = true,
                Message = "Products fetched successfully. Db Hit",
                Data = result
            });
        }

        [HttpGet("{sku}")]
        public async Task<IActionResult> Get(string sku)
        {
            var products = await _service.FindAsync(x => x.sku == sku);
            var product = products.FirstOrDefault();

            if (product == null)
            {
                return NotFound(new ApiResponse
                {
                    Code = 404,
                    Success = false,
                    Message = "Product not found.",
                    Data = null
                });
            }

            return Ok(new ApiResponse
            {
                Code = 200,
                Success = true,
                Message = "Product fetched successfully.",
                Data = product
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.AddAsync(request);

            if (result <= 0 || result == null)
            {
                return BadRequest(new ApiResponse
                {
                    Code = 400,
                    Success = false,
                    Message = "Product creation failed",
                    Data = null
                });
            }

            return StatusCode(201, new ApiResponse
            {
                Code = 201,
                Success = true,
                Message = "Product created successfully",
                Data = result
            });
        }

        [HttpPut("{sku}")]
        public async Task<IActionResult> Update(string sku, ProductMaster product)
        {
            await _service.UpdateAsync(sku, product);

            return Ok(new ApiResponse
            {
                Code = 200,
                Success = true,
                Message = "Product updated successfully.",
                Data = product
            });
        }

        [HttpDelete("{sku}")]
        public async Task<IActionResult> Delete(string sku)
        {
            await _service.DeleteAsync(sku);

            return Ok(new ApiResponse
            {
                Code = 200,
                Success = true,
                Message = "Product deleted successfully.",
                Data = null
            });
        }
    }
}
