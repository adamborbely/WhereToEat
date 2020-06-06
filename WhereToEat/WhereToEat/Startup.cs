using System;
using System.Data;
using System.IO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Npgsql;
using WhereToEat.Controllers;
using WhereToEat.Services;

namespace WhereToEat
{
    public class Startup
    {
        private readonly string _connectionString;
        private readonly string _uploadsDirectory;
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            _connectionString = InitConnectionString();
            _uploadsDirectory = InitUploadsDirectory(webHostEnvironment);
        }

        public IConfiguration Configuration { get; }

        private string InitUploadsDirectory(IWebHostEnvironment webHostEnvironment)
        {
            string uploadsDirectory = Path.Combine(Environment.GetEnvironmentVariable("UPLOADS_DIRECTORY") ?? webHostEnvironment.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsDirectory);
            return uploadsDirectory;
        }
        private string InitConnectionString()
        {
            string connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? "Host=localhost;Username=postgres;Password=admin;Database=WhereToEat";
            return connectionString;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddScoped<IPasswordHelper, PasswordEncrypt>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddSingleton(typeof(IStorageService), new FileStorageService(_uploadsDirectory));

            services.AddScoped<IDbConnection>(_ =>
            {
                var connection = new NpgsqlConnection(_connectionString);
                connection.Open();
                return connection;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie
            (CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.LoginPath = "/User/Login";
                options.LogoutPath = "/User/Logout";
            }
            );


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();


            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(_uploadsDirectory),
                RequestPath = "/uploads"
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
