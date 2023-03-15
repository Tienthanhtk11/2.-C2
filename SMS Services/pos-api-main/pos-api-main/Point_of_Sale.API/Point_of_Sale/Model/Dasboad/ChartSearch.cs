namespace Point_of_Sale.Model.Dasboad
{
    public class ChartSearch
    {
        public long warehouse_id { get; set; }
        public int type { get; set; }//1 ngày, 2 tuần, 3 tháng

        public List<ChartDataSearch> GetChartBarSearches()
        {
            List<ChartDataSearch> searches = new List<ChartDataSearch>();
            var datenow = DateTime.Now;
            switch (type)
            {
                case 1://day
                    for (int i = 0; i < 24; i++)
                    {
                        ChartDataSearch day = new ChartDataSearch
                        {
                            date_from = new DateTime(datenow.Year, datenow.Month, datenow.Day, i, 0, 0),
                            date_to = new DateTime(datenow.Year, datenow.Month, datenow.Day, i, 59, 59),
                        };

                        day.label = day.date_from.ToString("HH");
                        searches.Add(day); 
                    }
                    break;
                case 2://week
                    DateTime today = DateTime.Today;
                    int currentDayOfWeek = (int)today.DayOfWeek;
                    DateTime sunday = today.AddDays(-currentDayOfWeek);
                    DateTime monday = sunday.AddDays(1); 
                    if (currentDayOfWeek == 0)
                    {
                        monday = monday.AddDays(-7);
                    }
                    var dates = Enumerable.Range(0, 7).Select(days => monday.AddDays(days)).ToList(); 
                    foreach (var item in dates)
                    {
                        ChartDataSearch week = new ChartDataSearch
                        {
                            date_from = new DateTime(item.Year, item.Month, item.Day, 0, 0, 0),
                            date_to = new DateTime(item.Year, item.Month, item.Day, 23, 59, 59),
                        };
                        week.label = week.date_from.DayOfWeek == 0 ? "Chủ nhật" : "Thứ " + (int)(week.date_to.DayOfWeek + 1);
                        searches.Add(week);
                    }
                     
                    break;
                case 3://month
                    for (int i = 0; i < 12; i++)
                    {
                        var month = new ChartDataSearch
                        {
                            date_from = new DateTime(datenow.Year, i +1, 1, 0, 0, 0),
                            date_to = new DateTime(datenow.Year, i + 1, DateTime.DaysInMonth(datenow.Year, i +1), 23, 59, 59),
                        };
                        month.label = "Tháng " + month.date_from.Month;
                        searches.Add(month);
                    }
                    break;
                default:
                    break;
            }
            searches = searches.OrderBy(x => x.date_to).ToList();
            return searches;
        }

        public ChartDataSearch GetDataPieSearch()
        {
            ChartDataSearch search = new ChartDataSearch();
            var datenow = DateTime.Now;
            switch (type)
            {
                case 1://day
                    search.date_from = DateTime.Today;
                    search.date_to = new DateTime(datenow.Year, datenow.Month, datenow.Day, 23, 59, 59);
                    break;
                case 2://week
                    search.date_from = DateTime.Today.AddDays(-6);
                    search.date_to = new DateTime(datenow.Year, datenow.Month, datenow.Day, 23, 59, 59);
                    break;
                case 3://month
                    search.date_from = DateTime.Today.AddMonths(-1);
                    search.date_to = new DateTime(datenow.Year, datenow.Month, datenow.Day, 23, 59, 59);
                    break;
                default:
                    break;
            }
            return search;
        }
    }

    public class ChartDataSearch
    {
        public string label { get; set; }
        public DateTime date_from { get; set; }
        public DateTime date_to { get; set; }
    }
}
