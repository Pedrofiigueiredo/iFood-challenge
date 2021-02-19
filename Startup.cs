using System;
using iFoodOpenWeatherSpotify.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Polly;

namespace iFoodOpenWeatherSpotify
{
  public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ServiceSettings>(Configuration.GetSection(nameof(ServiceSettings)));

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "iFoodOpenWeatherSpotify", Version = "v1" });
            });

            services.AddHttpClient<OpenWeatherService>()
                .AddTransientHttpErrorPolicy(builder =>
                    builder.WaitAndRetryAsync(10, retryAttempt => 
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                )
                .AddTransientHttpErrorPolicy(builder =>
                    builder.CircuitBreakerAsync(3, TimeSpan.FromSeconds(10))
                );

            services.AddHttpClient<SpotifyService>()
                .AddTransientHttpErrorPolicy(builder =>
                    builder.WaitAndRetryAsync(10, retryAttempt => 
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                )
                .AddTransientHttpErrorPolicy(builder => 
                    builder.CircuitBreakerAsync(3, TimeSpan.FromSeconds(10))
                );

            services.AddHealthChecks()
                .AddCheck<ExternalEndpointHealthCheck>("OpenWeather");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "iFoodOpenWeatherSpotify v1"));
            }

            if (env.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
