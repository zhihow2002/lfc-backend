using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Ardalis.Specification;
using ClaimsPlugin.Shared.Foundation.Features.DomainDrivenDesign.Interfaces;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;
using Mapster;


namespace ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Models.Responses
{
    public class PaginationResponse<T>
    {
        private PaginationResponse(
            List<T>? data,
            bool isSuccess,
            int count,
            int pageNumber,
            int pageSize,
            params string[] messages
        )
        {
            Data = data;
            IsSuccess = isSuccess;
            CurrentPage = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            Messages = messages;

            if (isSuccess)
            {
                if ((data.IsNull() || data.Count == 0) && messages.Length == 0)
                {
                    Messages = new[] { "There is no available record." };
                }
            }
        }

        protected PaginationResponse() { }

        [DataMember(Order = 1)]
        public List<T>? Data { get; set; }

        [DataMember(Order = 2)]
        public string[] Messages { get; set; } = default!;

        [IgnoreDataMember]
        public DateTime TimeGenerated => DateTime.Now;

        [DataMember(Order = 3)]
        public bool IsSuccess { get; set; }

        [DataMember(Order = 4)]
        public int CurrentPage { get; set; }

        [DataMember(Order = 5)]
        public int TotalPages { get; set; }

        [DataMember(Order = 6)]
        public int TotalCount { get; set; }

        [DataMember(Order = 7)]
        public int PageSize { get; set; }

        [DataMember(Order = 8)]
        public bool HasPreviousPage => CurrentPage > 1;

        [DataMember(Order = 9)]
        public bool HasNextPage => CurrentPage < TotalPages;

        public static async Task<PaginationResponse<T>> FailAsync()
        {
            await Task.CompletedTask;

            return new PaginationResponse<T>(default, false, 0, 1, 1);
        }

        public static async Task<PaginationResponse<T>> FailAsync(params string[] messages)
        {
            await Task.CompletedTask;

            return new PaginationResponse<T>(default, false, 0, 1, 1, messages);
        }

        public static async Task<PaginationResponse<T>> FailAsync<TItem>(
            IEnumerable<TItem> items,
            Func<TItem, string> func
        )
        {
            await Task.CompletedTask;

            return new PaginationResponse<T>(default, false, 0, 1, 1, items.Select(func).ToArray());
        }

        public static async Task<PaginationResponse<T>> FailAsync<TItem>(IEnumerable<string> items)
        {
            await Task.CompletedTask;

            return new PaginationResponse<T>(default, false, 0, 1, 1, items.ToArray());
        }

        [DoesNotReturn]
        public static async Task<PaginationResponse<T>> FailAsync<TE>(TE exception)
            where TE : Exception
        {
            await Task.CompletedTask;

            throw exception;
        }

        public static async Task<PaginationResponse<T>> SuccessAsync<TRepository>(
            IReadRepositoryBase<TRepository> repository,
            ISpecification<TRepository> spec,
            List<T> data,
            int pageNumber,
            int pageSize,
            params string[] messages
        )
            where TRepository : class, IAggregateRoot
        {
            return new PaginationResponse<T>(
                data,
                true,
                await repository.CountAsync(spec),
                pageNumber,
                pageSize,
                messages
            );
        }

        public static async Task<PaginationResponse<T>> SuccessAsync(
            List<T>? data,
            int count,
            int pageNumber,
            int pageSize,
            params string[] messages
        )
        {
            await Task.CompletedTask;

            return new PaginationResponse<T>(data, true, count, pageNumber, pageSize, messages);
        }

        public static async Task<PaginationResponse<T>> TransformAsync<TData>(
            PaginationResponse<TData> response
        )
        {
            if (response.IsSuccess)
            {
                return await SuccessAsync(
                    response.Data?.Adapt<List<T>>(),
                    response.TotalCount,
                    response.CurrentPage,
                    response.PageSize,
                    response.Messages
                );
            }

            return await FailAsync(response.Messages);
        }
    }
}
