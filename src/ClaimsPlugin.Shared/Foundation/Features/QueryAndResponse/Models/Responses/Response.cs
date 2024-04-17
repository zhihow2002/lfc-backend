using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;


namespace ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Models.Responses
{
    [DataContract]
    public class Response
    {
        private Response(bool isSuccess, params string[] messages)
        {
            Messages = messages;
            IsSuccess = isSuccess;
        }

        protected Response() { }

        [DataMember(Order = 1)]
        public string[] Messages { get; set; } = default!;

        [IgnoreDataMember]
        public DateTime TimeGenerated => DateTime.Now;

        [DataMember(Order = 2)]
        public bool IsSuccess { get; set; }

        public static async Task<Response> FailAsync()
        {
            await Task.CompletedTask;
            return new Response(false);
        }

        public static async Task<Response> FailAsync(params string[] messages)
        {
            await Task.CompletedTask;
            return new Response(false, messages);
        }

        public static async Task<Response> FailAsync<TItem>(
            IEnumerable<TItem> items,
            Func<TItem, string> func
        )
        {
            await Task.CompletedTask;
            return new Response(false, items.Select(func).ToArray());
        }

        public static async Task<Response> FailAsync(IEnumerable<string> items)
        {
            await Task.CompletedTask;
            return new Response(false, items.ToArray());
        }

        [DoesNotReturn]
        public static async Task<Response> FailAsync<TE>(TE exception)
            where TE : Exception
        {
            await Task.CompletedTask;

            throw exception;
        }

        public static async Task<Response> SuccessAsync()
        {
            await Task.CompletedTask;
            return new Response(true);
        }

        public static async Task<Response> SuccessAsync(params string[] messages)
        {
            await Task.CompletedTask;
            return new Response(true, messages);
        }
    }
}
