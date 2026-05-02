namespace Server.Entities
{
    public class InventoryMaster
    {
        public int inventory_id { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }
        public int? warehouse_id { get; set; }
        public bool is_active { get; set; } = true;
        public DateTime create_date { get; set; } = DateTime.UtcNow;
        public int? create_by { get; set; }
        public DateTime? modify_date { get; set; }
        public int? modify_by { get; set; }
        public string? ip_address { get; set; }

    }
}
