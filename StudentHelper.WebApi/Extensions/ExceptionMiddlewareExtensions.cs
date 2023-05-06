
using Microsoft.AspNetCore.Diagnostics;
using StudentHelper.Model.Models.Common;
using System.Net;

namespace StudentHelper.Model.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
            public static void ConfigureExceptionHandler(this IApplicationBuilder app)
            {
                app.UseExceptionHandler(appError =>
                {
                    appError.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "application/json";
                        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (contextFeature != null)
                        {
                            await context.Response
                                .WriteAsync(new Response(
                                    context.Response.StatusCode,
                                    false,
                                    contextFeature.Error.Message)
                                .ToString());

                        }
                    });
                });
            }
        }
    }