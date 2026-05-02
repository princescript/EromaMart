namespace Server.Entities
{
    public class ProductMaster
    {
        public int product_id { get; set; }
        public string product_name { get; set; } = null!;
        public string? product_description { get; set; }
        public string sku { get; set; } = null!;
        public decimal price { get; set; }
        public decimal? discount_percent { get; set; }
        public string? hsn_code { get; set; }
        public int category_id { get; set; }
        public int brand_id { get; set; }
        public bool is_active { get; set; } = true;
        public DateTime create_date { get; set; } = DateTime.UtcNow;
        public int? create_by { get; set; }
        public DateTime? modify_date { get; set; }
        public int? modify_by { get; set; }
        public string? ip_address { get; set; }

    }
}