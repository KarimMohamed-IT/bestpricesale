using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using bestpricesale.Data;
using bestpricesale.Services;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace bestpricesale
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");;

            //builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            //builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

            // Configuration Setup
            var configuration = builder.Configuration;

            // Database Configuration
            ConfigureDatabase(builder, configuration);

            // Identity & Authentication Configuration
            ConfigureAuthenticationAndAuthorization(builder);

            // Application Services Configuration
            ConfigureApplicationServices(builder);

            var app = builder.Build();

            // Middleware Pipeline Configuration
            ConfigureMiddlewarePipeline(app);

            // Database Initialization
            InitializeDatabase(app);

            app.Run();
        }

        private static void ConfigureDatabase(WebApplicationBuilder builder, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString,
                    sqlOptions => sqlOptions.EnableRetryOnFailure()));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddMemoryCache();
        }

        private static void ConfigureAuthenticationAndAuthorization(WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireRole("Admin")
                          .RequireAuthenticatedUser());
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.SlidingExpiration = true;
            });
        }

        private static void ConfigureApplicationServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IPageService, PageService>();
            builder.Services.AddScoped<ITemplateService, TemplateService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();

            builder.Services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizeFolder("/Admin", "AdminOnly");
            });

            builder.Services.AddControllers();
            builder.Services.AddHealthChecks();
        }

        private static void ConfigureMiddlewarePipeline(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();
            app.MapControllers();
            app.MapHealthChecks("/health");
        }

        private static void InitializeDatabase(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();

                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                CreateAdminRoleIfNotExists(roleManager).Wait();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while initializing the database");
            }
        }

        private static async Task CreateAdminRoleIfNotExists(RoleManager<IdentityRole> roleManager)
        {
            const string adminRole = "Admin";
            var roleExists = await roleManager.RoleExistsAsync(adminRole);

            if (!roleExists)
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }
        }
    }
}