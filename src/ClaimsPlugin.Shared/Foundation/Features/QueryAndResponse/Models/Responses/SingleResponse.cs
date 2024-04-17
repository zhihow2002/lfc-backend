using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;
using Mapster;

namespace ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Models.Responses
{
    [DataContract]
    public class SingleResponse<T>
    {
        private SingleResponse(
            T? data,
            bool isSuccess,
            bool isList = false,
            params string[] messages
        )
        {
            Data = data;
            Messages = messages;
            IsSuccess = isSuccess;

            if (isSuccess && messages.Length == 0)
            {
                if (isList)
                {
                    ICollection? collection = data as ICollection;

                    if ((collection.IsNull() || collection.Count == 0) && messages.Length == 0)
                    {
                        Messages = new[] { "There is no available record." };
                    }
                }
                else
                {
                    if (data.IsNull())
                    {
                        Messages = new[] { "There is no available record." };
                    }
                }
            }
        }

        protected SingleResponse() { }

        [DataMember(Order = 1)]
        public T? Data { get; set; }

        [DataMember(Order = 2)]
        public string[] Messages { get; set; } = default!;

        [IgnoreDataMember]
        public DateTime TimeGenerated => DateTime.Now;

        [DataMember(Order = 3)]
        public bool IsSuccess { get; set; }

        public static async Task<SingleResponse<T>> FailAsync()
        {
            await Task.CompletedTask;
            return new SingleResponse<T>(default, false);
        }

        public static async Task<SingleResponse<T>> FailAsync(params string[] messages)
        {
            await Task.CompletedTask;

            return new SingleResponse<T>(default, false, false, messages);
        }

        public static async Task<SingleResponse<T>> FailAsync<TItem>(
            IEnumerable<TItem> items,
            Func<TItem, string> func
        )
        {
            await Task.CompletedTask;

            return new SingleResponse<T>(default, false, false, items.Select(func).ToArray());
        }

        public static async Task<SingleResponse<T>> FailAsync(IEnumerable<string> items)
        {
            await Task.CompletedTask;

            return new SingleResponse<T>(default, false, false, items.ToArray());
        }

        public static async Task<SingleResponse<T>> FailAsync(T? data)
        {
            await Task.CompletedTask;

            return new SingleResponse<T>(data, false);
        }

        [DoesNotReturn]
        public static async Task<SingleResponse<T>> FailAsync<TE>(TE exception)
            where TE : Exception
        {
            await Task.CompletedTask;

            throw exception;
        }

        public static async Task<SingleResponse<T>> SuccessAsync()
        {
            await Task.CompletedTask;
            return new SingleResponse<T>(default, true);
        }

        public static async Task<SingleResponse<T>> SuccessAsync(params string[] messages)
        {
            await Task.CompletedTask;
            return new SingleResponse<T>(default, true, false, messages);
        }

        public static async Task<SingleResponse<T>> SuccessAsync(T? data)
        {
            await Task.CompletedTask;
            return new SingleResponse<T>(data, true);
        }

        public static async Task<SingleResponse<T>> SuccessAsync(T? data, params string[] messages)
        {
            await Task.CompletedTask;

            return new SingleResponse<T>(data, true, false, messages);
        }

        /// <summary>
        ///     This overload adds a default no record message when the list null or empty.
        /// </summary>
        /// <param name="data">A type that implements ICollection</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws when the data is not a type of <see cref="ICollection" /></exception>
        public static async Task<SingleResponse<T>> SuccessListAsync(T? data)
        {
            await Task.CompletedTask;

            if (data is not null && data is not ICollection)
            {
                throw new ArgumentException(
                    "Expecting a type from ICollection. Use Success() or SuccessAsync() if you are passing single item to the response."
                );
            }

            return new SingleResponse<T>(data, true, true);
        }

        /// <summary>
        ///     This overload adds a default no record message when the list null or empty.
        /// </summary>
        /// <param name="data">A type that implements ICollection</param>
        /// <param name="messages"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws when the data is not a type of <see cref="ICollection" /></exception>
        public static async Task<SingleResponse<T>> SuccessListAsync(
            T? data,
            params string[] messages
        )
        {
            await Task.CompletedTask;

            if (data is not null && data is not ICollection)
            {
                throw new ArgumentException(
                    "Expecting a type from ICollection. Use Success() or SuccessAsync() if you are passing single item to the response."
                );
            }

            return new SingleResponse<T>(data, true, true, messages);
        }

        public static async Task<SingleResponse<T>> TransformAsync<TData>(
            SingleResponse<TData> response
        )
        {
            if (response.IsSuccess)
            {
                if (response.Data.IsNull())
                {
                    return await SuccessAsync();
                }

                return await SuccessAsync(response.Data.Adapt<T>());
            }

            return await FailAsync(response.Messages);
        }
    }
}
