using System.Net.Http;
using API_BET.Dal;
using API_BET.Dal.Settings;
using API_BET.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace API_BET
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
            services.Configure<BetDatabaseSettings>(Configuration.GetSection(nameof(BetDatabaseSettings)));
            services.AddSingleton<IBetDatabaseSettings>(sp => sp.GetRequiredService<IOptions<BetDatabaseSettings>>().Value);
            services.AddControllers();
            services.AddSwaggerGen();
            services.AddSingleton<IBetDatabase, BetDatabase>();
            services.AddSingleton<HttpClient>();
            services.AddTransient<IOfferClient, OfferClient>();
            services.AddTransient<IBetService,BetService>();
            services.AddHealthChecks().AddCheck<DummyHealthCheck>("DummyHealthCheckIsOk");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BetAPI");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
