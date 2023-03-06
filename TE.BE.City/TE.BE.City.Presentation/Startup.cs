using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TE.BE.City.Domain.Interfaces;
using TE.BE.City.Domain;
using TE.BE.City.Infra.Data;
using TE.BE.City.Infra.Data.Repository;
using TE.BE.City.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper;
using TE.BE.City.Presentation.Mappings;
using System;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace TE.BE.City.Presentation
{
    /// <summary>
    /// Startup class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Startup method
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration method
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "City API"
                });
            });
                        
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 27));

            services.AddDbContext<TEBECityContext>(options => {
                options.UseMySql(connectionString, serverVersion);
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            });

            // Configure JWT token authentication and authorization
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });

            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            // Dependency injection
            services.AddScoped(typeof(TEBECityContext));
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

            services.AddScoped(typeof(IWaterService), typeof(WaterService));
            services.AddScoped(typeof(IUserService), typeof(UserService));
            services.AddScoped(typeof(ILightService), typeof(LightService));
            services.AddScoped(typeof(ISewerService), typeof(SewerService));
            services.AddScoped(typeof(ITrashService), typeof(TrashService));
            services.AddScoped(typeof(ICollectService), typeof(CollectService));
            services.AddScoped(typeof(IAsphaltService), typeof(AsphaltService));
            services.AddScoped(typeof(IPublicServiceService), typeof(PublicServiceService));

            services.AddScoped(typeof(IUserDomain), typeof(UserDomain));

            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddRazorPages().AddRazorRuntimeCompilation();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(x => x
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
                app.UseHttpsRedirection();
            }
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "City API"); });

            app.UseMvc();
             
            /*
            app.Run(async context => {
                context.Response.Redirect("swagger/index.html");
            });
            */
            
            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Pages")),
                RequestPath = "/Pages",
                EnableDefaultFiles = true
            });

            app.UseStaticFiles();
        }
    }
}
