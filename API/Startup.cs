using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Extentions;
using API.Interfaces;
using API.Middleware;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;

        //  public IConfiguration Config { get; }
        public Startup(IConfiguration config)
        {
            _config = config;

        }

        // public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Add Application Auth sevive through Extention Methood
            services.AddApplicationServices(_config);
            services.AddControllers();
            services.AddCors();
            //Add Identity Auth sevive through Extention Methood FD
            services.AddIdentityService(_config);

            // services.AddSwaggerGen(c =>
            // {
            //     c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIv5", Version = "v1" });
            // });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
               // app.UseDeveloperExceptionPage();
               // app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv6 v1"));
            }
           

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();
            //Cross site 
            app.UseCors(x => x.AllowAnyHeader()
                .AllowAnyMethod()
                .WithOrigins("http://localhost:4200",
                "https://localhost:4200", 
                "https://web.postman.co",
                "https://web.postman.co/"));
            app.UseAuthentication();
            app.UseAuthorization();
            //to use default and static file user basically agular 
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //to use this tell api where to go after refresh of no path found
                endpoints.MapFallbackToController("Index","Fallback");
               // endpoints.MapHub<PresecceHub>("hubs/presence");
                //endpoints.MapHub<MesasgeHub>("hubs/message");

            });
        }
    }
}
