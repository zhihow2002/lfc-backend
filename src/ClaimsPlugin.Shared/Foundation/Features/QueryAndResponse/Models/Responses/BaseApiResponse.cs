namespace ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Models.Responses
{
    public class BaseApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public BaseApiResponse(bool success, string message, T data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static BaseApiResponse<T> SuccessResponse(T data, string message = "")
        {
            return new BaseApiResponse<T>(true, message, data);
        }

        public static BaseApiResponse<T> FailureResponse(string message)
        {
            return new BaseApiResponse<T>(false, message, default(T?));
        }
    }
}
