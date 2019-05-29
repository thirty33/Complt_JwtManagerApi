using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Complet_JwtManager_Api.Domain.Services;
using Complet_JwtManager_Api.Helpers;
using Complet_JwtManager_Api.Models;
using Complet_JwtManager_Api.Persistence.Context;
using Complet_JwtManager_Api.Persistence.Repositories;
using Complet_JwtManager_Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Complet_JwtManager_Api
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddLogging();


            //config db context
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql("server=localhost;user id=postgres;password=postgres;database=jwt_implement_db");
            });

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration["redis:connectionString"];
            });

            services.AddSingleton<IAccountService, AccountService>();
            services.AddSingleton<IJwtHandler, JwtHandler>();
            services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddTransient<TokenManagerMiddleware>();
            services.AddTransient<ITokenManager, Services.TokenManager>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            var jwtSection = Configuration.GetSection("jwt");
            var jwtOptions = new JwtOptions();
            jwtSection.Bind(jwtOptions);
            services.AddAuthentication()
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                        ValidIssuer = jwtOptions.Issuer,
                        ValidateAudience = false,
                        ValidateLifetime = true
                    };
                });
            services.Configure<JwtOptions>(jwtSection);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddDebug().AddConsole();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseAuthentication();
            app.UseMiddleware<TokenManagerMiddleware>();
            app.UseMvc();
        }
    }
}
