namespace Point_of_Sale.Model.Report
{
    public class Category_Revenue
    {
        public long category_id { get; set; }   
        public string category_name { get; set;}
        public string category_code { get; set;}
        public double category_revenue { get; set;} // doanh thu
        public double category_cost_price { get; set;}//giá vốn
        public double category_profit { get; set;} // lợi nhuận 
    }
}
