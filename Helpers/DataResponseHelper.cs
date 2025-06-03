using api.Dtos;

namespace api.Helpers
{
    public static class DataResponseHelper
    {
        public static DataResponse<T> Error<T>(string title, IEnumerable<string>? errors = null, double? elapsed = null)
        {
            return new DataResponse<T>(title)
            {
                Errors = errors,
                ElapsedMilliseconds = elapsed
            };
        }
    }
}