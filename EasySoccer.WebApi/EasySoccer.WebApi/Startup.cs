using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EasySoccer.BLL;
using EasySoccer.BLL.Infra;
using EasySoccer.DAL;
using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.DAL.Repositories;
using EasySoccer.WebApi.Security;
using EasySoccer.WebApi.Security.Entity;
using EasySoccer.WebApi.UoWs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;

namespace EasySoccer.WebApi
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
            services.AddCors(op => op.AddPolicy("AllowAll", b => b
                                                                .AllowAnyHeader()
                                                                .AllowAnyMethod()
                                                                .AllowAnyOrigin()
                                                                .AllowCredentials()
            ));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<IEasySoccerDbContext, EasySoccerDbContext>(
                                    x => x.UseSqlServer(Configuration.GetConnectionString("EasySoccerDbContext")));

            #region UoW's
            services.AddScoped<CompanyUoW, CompanyUoW>();
            services.AddScoped<LoginUoW, LoginUoW>();
            services.AddScoped<SoccerPitchReservationUoW, SoccerPitchReservationUoW>();
            services.AddScoped<SoccerPitchUoW, SoccerPitchUoW>();
            services.AddScoped<SoccerPitchPlanUoW, SoccerPitchPlanUoW>();
            services.AddScoped<UserUoW, UserUoW>();
            services.AddScoped<DashboardUoW, DashboardUoW>();
            #endregion

            #region BLL's
            services.AddScoped<ICompanyBLL, CompanyBLL>();
            services.AddScoped<IUserBLL, UserBLL>();
            services.AddScoped<ISoccerPitchReservationBLL, SoccerPitchReservationBLL>();
            services.AddScoped<ISoccerPitchBLL, SoccerPitchBLL>();
            services.AddScoped<ISoccerPitchPlanBLL, SoccerPitchPlanBLL>();
            #endregion

            #region Repositories
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISoccerPitchReservationRepository, SoccerPitchReservationRepository>();
            services.AddScoped<ISoccerPitchRepository, SoccerPitchRepository>();
            services.AddScoped<ISoccerPitchPlanRepository, SoccerPitchPlanRepository>();
            services.AddScoped<ISoccerPitchSoccerPitchPlanRepository, SoccerPitchSoccerPitchPlanRepository>();
            #endregion

            #region TokenConfiguration

            var signingConfigutation = new SigningConfigurations();
            services.AddSingleton(signingConfigutation);

            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                Configuration.GetSection("TokenConfigurations"))
                    .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);

            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signingConfigutation.Key;
                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;
                paramsValidation.ValidateIssuerSigningKey = true;
                paramsValidation.ValidateLifetime = true;
                paramsValidation.ClockSkew = TimeSpan.FromHours(12);
            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            #endregion
            


            #region Swagger
            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1",
                        new Info
                        {
                            Title = "Easy Soccer WebApi Documentation",
                            Version = PlatformServices.Default.Application.ApplicationVersion,
                            Description = "Documentation",
                            Contact = new Contact
                            {
                                Name = "Tarcisio Vitor"
                            }
                        });
                    c.AddSecurityDefinition("Bearer", new ApiKeyScheme { In = "header", Description = "Add Jwt Token", Name = "Authorization", Type = "apiKey" });
                    c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> { { "Bearer", Enumerable.Empty<string>() }, });

                    string caminhoAplicacao =
                        PlatformServices.Default.Application.ApplicationBasePath;
                    string nomeAplicacao =
                        PlatformServices.Default.Application.ApplicationName;
                    string caminhoXmlDoc =
                        Path.Combine(caminhoAplicacao, $"{nomeAplicacao}.xml");

                    c.IncludeXmlComments(caminhoXmlDoc);
                });
            #endregion
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("AllowAll");
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "Easy Soccer WebApi Documentation");
            });
        }
    }
}
