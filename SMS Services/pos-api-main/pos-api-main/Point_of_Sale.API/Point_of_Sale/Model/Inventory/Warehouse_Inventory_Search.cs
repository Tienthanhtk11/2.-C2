namespace Point_of_Sale.Model.Inventory
{
    public class Warehouse_Inventory_Search: SearchBase
    {
        public long warehouse_id { get; set; }
        public byte? status_id { get; set; }
        
    }
}
