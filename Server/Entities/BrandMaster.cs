namespace Server.Entities
{
    public class BrandMaster
    {
        public int brand_id { get; set; }
        public string brand_name { get; set; } = null!;
        public string? description { get; set; }
        public string? gst_number { get; set; }
        public string? pan_number { get; set; }
        public string? website_url { get; set; }
        public string? support_email { get; set; }
        public string? support_phone { get; set; }
        public string? headquarters_address { get; set; }
        public string? country { get; set; }
        public string? state { get; set; }
        public string? logo_url { get; set; }
        public bool is_active { get; set; } = true;
        public bool is_verified { get; set; } = false;
        public DateTime create_date { get; set; } = DateTime.UtcNow;
        public int? create_by { get; set; }
        public DateTime? modify_date { get; set; }
        public int? modify_by { get; set; }
        public string? ip_address { get; set; }
    }
}
