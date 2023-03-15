namespace Point_of_Sale.Model.Warehouse
{
    public class UserWarehouseModel
    {
        public long user_id { get; set; }
        public long id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string code { get; set; }
        public int? province_code { get; set; } // code vi tri kho

    }
}
