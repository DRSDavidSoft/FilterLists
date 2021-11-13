﻿using System.Reflection;
using Microsoft.OpenApi.Models;

namespace FilterLists.Directory.Api;

internal static class SwaggerConfigurationExtensions
{
    public static void AddSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(o =>
        {
            o.SupportNonNullableReferenceTypes();

            o.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = "FilterLists Directory API",
                    Description = "An ASP.NET Core API serving the core FilterList information.",
                    Version = "v1",
                    //TermsOfService = "",
                    Contact = new OpenApiContact { Name = "FilterLists", Url = new Uri("https://filterlists.com") },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://github.com/collinbarrett/FilterLists/blob/master/LICENSE")
                    }
                });

            var apiXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, apiXmlFile));

            var apiContractsXmlFile = $"{typeof(Contracts.ConfigurationExtensions).Assembly.GetName().Name}.xml";
            o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, apiContractsXmlFile));

            var applicationXmlFile = $"{typeof(Application.ConfigurationExtensions).Assembly.GetName().Name}.xml";
            o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, applicationXmlFile));
        });
    }

    public static void UseSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger(o =>
        {
            o.RouteTemplate = "{documentName}/swagger.json";
            o.PreSerializeFilters.Add((swaggerDoc, httpReq) => swaggerDoc.Servers = new List<OpenApiServer>
            {
                new() { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}/api/directory" }
            });
        });
    }
}
