namespace Point_of_Sale.Model.Dasboad
{
    public class RevenueModel
    {
        public string label { get; set; }
        public List<ChartDataModel> data { get; set; }
    }
    public class ChartDataModel
    {
        public string name { get; set; }
        public double total { get; set; }
    }
}
