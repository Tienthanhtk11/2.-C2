namespace Point_of_Sale.Model.Report
{
    public class HistoryInventorySearchModel : SearchBase
    {
        public long warehouse_id { get; set; }
        public long product_id { get; set; }
        
    }
}
