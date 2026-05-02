namespace Server.Entities
{
    public class CategoryMaster
    {
        public int category_id { get; set; }

        public string category_name { get; set; } = null!;

        public string? category_slug { get; set; }

        public bool is_active { get; set; } = true;

        public DateTime create_date { get; set; } = DateTime.Now;

        public int? create_by { get; set; }

        public DateTime? modify_date { get; set; }

        public int? modify_by { get; set; }

        public string? ip_address { get; set; }
    }
}
