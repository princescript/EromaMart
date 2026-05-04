public class ProductCreateRequest
{
    public string product_name { get; set; } = null!;
    public string? product_description { get; set; }
    public decimal price { get; set; }
    public decimal? discount_percent { get; set; }
    public string? hsn_code { get; set; }

    public int category_id { get; set; }
    public int brand_id { get; set; }

    public List<IFormFile> Files { get; set; } = new();
}