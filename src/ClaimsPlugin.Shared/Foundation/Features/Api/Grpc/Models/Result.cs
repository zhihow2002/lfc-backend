using System.Collections;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;


namespace ClaimsPlugin.Shared.Foundation.Features.Api.Grpc.Models;

public class Result<T>
{
    private Result(T? data, bool isSuccess, bool isList = false, params string[] messages)
    {
        Data = data;
        Messages = messages;
        IsSuccess = isSuccess;

        if (isSuccess)
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

    public T? Data { get; set; }

    public bool IsSuccess { get; set; }

    public string[] Messages { get; set; }

    public static async Task<Result<T>> FailAsync()
    {
        await Task.CompletedTask;
        return new Result<T>(default, false);
    }

    public static async Task<Result<T>> FailAsync(params string[] messages)
    {
        await Task.CompletedTask;

        return new Result<T>(default, false, false, messages);
    }

    public static async Task<Result<T>> FailAsync<TItem>(IEnumerable<TItem> items, Func<TItem, string> func)
    {
        await Task.CompletedTask;

        return new Result<T>(default, false, false, items.Select(func).ToArray());
    }

    public static async Task<Result<T>> SuccessAsync()
    {
        await Task.CompletedTask;
        return new Result<T>(default, true);
    }

    public static async Task<Result<T>> SuccessAsync(T? data)
    {
        await Task.CompletedTask;
        return new Result<T>(data, true);
    }
}

public class Result
{
    private Result(bool isSuccess, params string[] messages)
    {
        Messages = messages;
        IsSuccess = isSuccess;
    }

    public bool IsSuccess { get; set; }

    public string[] Messages { get; set; }

    public static async Task<Result> FailAsync()
    {
        await Task.CompletedTask;
        return new Result(default);
    }

    public static async Task<Result> FailAsync(params string[] messages)
    {
        await Task.CompletedTask;

        return new Result(default, messages);
    }

    public static async Task<Result> FailAsync<TItem>(IEnumerable<TItem> items, Func<TItem, string> func)
    {
        await Task.CompletedTask;

        return new Result(false, items.Select(func).ToArray());
    }

    public static async Task<Result> SuccessAsync()
    {
        await Task.CompletedTask;
        return new Result(true);
    }
}
