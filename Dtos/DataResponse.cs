namespace api.Dtos
{
    public class DataResponse<T>
    {
        public string Message { get; set; }
        public T Data { get; set; }

        public DataResponse(string message, T data = default)
        {
            Message = message;
            Data = data;
        }
    }
}