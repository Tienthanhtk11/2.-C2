namespace Point_of_Sale.Model.Report
{
    public class InOutInventorySearchModel: SearchBase
    { 
        public long warehouse_id { get; set; }
        public string? category_code { get; set; } 
    }
}
