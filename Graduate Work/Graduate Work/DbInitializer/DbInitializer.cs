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
        public void Initialize()
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
            //create roles if they are not created
            _roleManager.CreateAsync(new IdentityRole(Roles.Role_Customer)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Roles.Role_Administrator)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Roles.Role_Moderator)).GetAwaiter().GetResult();

            return;
        }
    }
}
