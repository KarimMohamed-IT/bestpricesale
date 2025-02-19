using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using bestpricesale.Data;
using bestpricesale.Services;

namespace bestpricesale
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);


            // Configure EF Core (using an in-memory database for this demo; replace with SQL Server or another provider as needed)
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddRazorPages();

            // Register repositories.
            builder.Services.AddScoped<IPageRepository, PageRepository>();
            builder.Services.AddScoped<ITemplateRepository, TemplateRepository>();

            // Register application services.
            builder.Services.AddScoped<IPageService, PageService>();
            builder.Services.AddScoped<ITemplateService, TemplateService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();


            // Add Razor Pages support.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            // Uncomment if you add authentication/authorization in the future:
            // app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();
            app.Run();

            //app.MapStaticAssets();
            //app.MapRazorPages()
            //   .WithStaticAssets();

        }
    }
}
