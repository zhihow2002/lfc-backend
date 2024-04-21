using System.Reflection;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace ClaimsPlugin.Shared.Foundation.Features.Api.Rest.OpenApi.Processors;

internal static class ActionDescriptorExtensions
{
    internal static T? TryGetPropertyValue<T>(this ActionDescriptor? obj, string propertyName, T? defaultValue = default)
    {
        return obj?.GetType().GetRuntimeProperty(propertyName) is
        {
        } propertyInfo
            ? (T?)propertyInfo.GetValue(obj)
            : defaultValue;
    }
}
