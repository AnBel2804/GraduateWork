using Graduate_Work.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Graduate_Work.Utility;

namespace Graduate_Work.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        public DbInitializer(
             UserManager<IdentityUser> userManager,
             RoleManager<IdentityRole> roleManager,
             ApplicationDbContext db
             )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }
        public async void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

            }

            if (!_roleManager.RoleExistsAsync(Roles.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(Roles.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Roles.Role_Administrator)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Roles.Role_Moderator)).GetAwaiter().GetResult();

                IdentityUser moderator = new IdentityUser()
                {
                    Email = "moderator@gmail.com",
                    UserName = "moderator"
                };
                var resultModeratorResponce = _userManager.CreateAsync(moderator, "0106Moderator2023").GetAwaiter().GetResult();
                if (resultModeratorResponce.Succeeded)
                {
                    _userManager.AddToRoleAsync(moderator, Roles.Role_Moderator).GetAwaiter().GetResult();
                }

                IdentityUser administrator = new IdentityUser()
                {
                    Email = "administrator@gmail.com",
                    UserName = "admin"
                };
                var resultAdminResponce = _userManager.CreateAsync(administrator, "0106Admin2023").GetAwaiter().GetResult();
                if (resultAdminResponce.Succeeded)
                {
                    _userManager.AddToRoleAsync(administrator, Roles.Role_Administrator).GetAwaiter().GetResult();
                }
            }

            return;
        }
    }
}
