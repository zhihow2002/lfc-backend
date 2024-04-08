using System.Net;

namespace ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling
{
    public class ErrorResult
    {
        public string Title { get; set; } = default!;
        public string Type { get; private set; } = default!;
        public List<string> Messages { get; set; } = new();
        public string? Source { get; set; } = default!;
        public string? TraceId { get; set; } = default!;
        public string? SupportMessage { get; set; } = default!;
        public string? SupportCode { get; set; } = "N/A";

        public bool IsSuccess { get; set; } = false;

        private int _statusCode;

        public int StatusCode
        {
            get => _statusCode;
            set
            {
                _statusCode = value;
                Type = GetRfcSection(value);
            }
        }

        private static string GetRfcSection(int statusCode)
        {
            return statusCode switch
            {
                (int)HttpStatusCode.Accepted => "https://tools.ietf.org/html/rfc7231#section-6.3.3",
                (int)HttpStatusCode.OK => "https://tools.ietf.org/html/rfc7231#section-6.3.1",
                (int)HttpStatusCode.BadRequest
                    => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                (int)HttpStatusCode.Forbidden
                    => "https://tools.ietf.org/html/rfc7231#section-6.5.3",
                (int)HttpStatusCode.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                (int)HttpStatusCode.InternalServerError
                    => "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                _ => string.Empty
            };
        }
    }
}
