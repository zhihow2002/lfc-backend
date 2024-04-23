//using Foundation.Features.ExceptionHandling.Middleware;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.Extensions.DependencyInjection;

//namespace Foundation.Features.ExceptionHandling;

//internal static class Startup
//{
//    internal static IServiceCollection AddExceptionMiddleware(this IServiceCollection services)
//    {
//        return services.AddScoped<ExceptionMiddleware>();
//    }

//    internal static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
//    {
//        return app.UseMiddleware<ExceptionMiddleware>();
//    }
//}
