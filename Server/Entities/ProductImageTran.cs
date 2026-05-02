namespace Server.Entities
{
    public class ProductImageTran
    {
        public int image_id { get; set; }
        public int product_id { get; set; }
        public string image_url { get; set; } = null!;
        public string? public_id { get; set; }
        public bool? is_default { get; set; } = false;
        public int display_order { get; set; }
        public bool is_active { get; set; } = true;
        public DateTime create_date { get; set; } = DateTime.UtcNow;
        public int? create_by { get; set; }
        public DateTime? modify_date { get; set; }
        public int? modify_by { get; set; }
        public string? ip_address { get; set; }

    }
}
//Insert image URL

//Get images by product id
//Deactivate single image
//Deactivate all images for product
//Mark default image