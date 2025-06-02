namespace api.Dtos
{
    public class DataResponse<T>
    {
        public string Title { get; set; }
        public T Data { get; set; }

        public DataResponse(string message, T data = default)
        {
            Title = message;
            Data = data;
        }
    }
}