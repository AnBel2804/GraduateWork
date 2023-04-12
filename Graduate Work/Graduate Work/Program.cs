using Graduate_Work.Data;
using Graduate_Work.DbInitializer;
using Graduate_Work.Repository.IRepository;
using Graduate_Work.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Google.Protobuf.WellKnownTypes;

namespace Graduate_Work
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")
                ));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(opts => {
                opts.Password.RequireNonAlphanumeric = false; // требуются ли не алфавитно-цифровые символы            
            })
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IDbInitializer, DbInitializer.DbInitializer>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/User/Home/Login";
            });
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(100);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            SeedDB();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{area=User}/{controller=Home}/{action=Index}/{id?}");

            app.Run();

            void SeedDB()
            {
                using (var scope = app.Services.CreateScope())
                {
                    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                    dbInitializer.Initialize();
                }
            }
        }
    }
}