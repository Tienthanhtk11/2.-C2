namespace App.Model
{
    public class ResponseSingleContentModel<T>
    {
        public string Message { get; set; } = string.Empty;
        public int StatusCode { set; get; } = 200;
        public T Data { set; get; }
    }
}
