namespace Point_of_Sale.Model.Warehouse
{
    public class Warehouse_Request_Search : SearchBase
    {
        public long warehouse_id { get; set; }
        public byte? status_id { get; set; }
        public long? partner_id { get; set; } 

    }
}
