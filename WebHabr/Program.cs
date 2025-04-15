using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebHabr.Data;

namespace WebHabr
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<IdentityContext>(options =>
                options.UseNpgsql(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            //builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddRoles<IdentityRole>() // ќЅя«ј“≈Ћ№Ќќ дл€ работы с рол€ми!
            //    .AddEntityFrameworkStores<IdentityContext>();
            //builder.Services.AddControllersWithViews();

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false; // если не хочешь подтверждение e-mail
            })
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();
            //app.MapRazorPages()
            //   .WithStaticAssets();

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await SeedRolesAsync(roleManager);
            }

            async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
            {
                string[] roles = { "Admin", "Author", "User" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }


            app.Run();
        }
    }
}
