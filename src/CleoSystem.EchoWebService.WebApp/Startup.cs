// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace CleoSystem.EchoWebService.WebApp
{
    /// <summary>
    /// Web host startup logic
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configures services.
        /// </summary>
        /// <param name="services">Services collection to modify.</param>
#pragma warning disable CA1822 // Mark members as static
        public void ConfigureServices(IServiceCollection services)
#pragma warning restore CA1822 // Mark members as static
        {
            services.AddControllers();
        }

        /// <summary>
        /// Configures the application.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="env">web hosting environment details.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.MapWhen(Predicate, AppConfiguration);
        }

        private void AppConfiguration(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Run(Handler);
        }

        private async Task Handler(HttpContext context)
        {
            var requestBodyStream = new MemoryStream();

            var response = context.Response;
            response.StatusCode = 200;
            response.ContentType = "text/plain";

            var request = context.Request;
            await response.WriteAsync($"{request.Method} : {request.Path}{request.QueryString.Value}\n")
                .ConfigureAwait(false);

            foreach (KeyValuePair<string, StringValues> requestHeader in context.Request.Headers)
            {
                var values = string.Join(";", requestHeader.Value);

                await response.WriteAsync($"{requestHeader.Key} : {values}\n")
                    .ConfigureAwait(false);
            }

            await context.Request.Body.CopyToAsync(requestBodyStream);
            requestBodyStream.Seek(0, SeekOrigin.Begin);

            using (var streamReader = new StreamReader(requestBodyStream))
            {
                var requestBodyText = streamReader.ReadToEnd();

                await response.WriteAsync(requestBodyText)
                    .ConfigureAwait(false);
            }
        }

        private static bool Predicate(HttpContext arg)
        {
            return true;
        }
    }
}
