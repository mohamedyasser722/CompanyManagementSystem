 using CompanyManagementSystem.BLL.Interfaces;
using CompanyManagementSystem.BLL.Repositories;
using CompanyManagementSystem.DAL.Context;
using CompanyManagementSystem.DAL.Models;
using CompanyManagementSystem.PL.AutoMapper;
using CompanyManagementSystem.PL.Utilities.EmailHelper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagementSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<CompanyManagementSystemDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // allow DI  
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddAutoMapper(typeof(OrganizationProfile));

            // Allow DI for SignInManager, UserManager, RoleManager
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options=>
            {
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                
            })
                .AddEntityFrameworkStores<CompanyManagementSystemDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.
                AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
                AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Home/Error";
                });

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Add EmailService to the container
            // Allow DI for EmailService
            builder.Services.AddScoped<EmailService>();

            // Add EmailSettings to the container
			builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

            // Add DbInitializer to the container

            builder.Services.AddScoped<DbInitializer>();

			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            // Seed the database
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var dbInitializer = services.GetRequiredService<DbInitializer>();
                try
                {
                    await dbInitializer.SeedAsync(userManager, roleManager);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            await app.RunAsync();
        }
    }
}