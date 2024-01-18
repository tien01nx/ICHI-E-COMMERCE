using Microsoft.EntityFrameworkCore;
using ICHI_CORE.Domain;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Helpers;

namespace ICHI_CORE.Entities.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly PcsApiContext _db;

        public DbInitializer(PcsApiContext db)
        {
            _db = db;
        }
        public void Initialize()
        {
            // di chuyển nếu chúng không được áp dụng

            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }

            }
            catch (Exception)
            {

            }

            var RoleCount = _db.Roles.Count();

            if (RoleCount == 0)
            {
                var rolesToAdd = new List<Role>
                {
                    new Role
                    {
                        Name = AppSettings.ADMIN,
                        CreateDatetime = DateTime.Now,
                        CreateUserId = AppSettings.ADMIN,
                        UpdateDatetime = DateTime.Now,
                        UpdateUserId = AppSettings.ADMIN
                    },
                    new Role
                    {
                        Name = AppSettings.USER,
                        CreateDatetime = DateTime.Now,
                        CreateUserId = AppSettings.ADMIN,
                        UpdateDatetime = DateTime.Now,
                        UpdateUserId = AppSettings.ADMIN
                    },
                    new Role
                    {
                        Name = AppSettings.EMPLOYEE,
                        CreateDatetime = DateTime.Now,
                        CreateUserId = AppSettings.ADMIN,
                        UpdateDatetime = DateTime.Now,
                        UpdateUserId = AppSettings.ADMIN
                    }
                };

                _db.Roles.AddRange(rolesToAdd);
                _db.SaveChanges();
                var user = new User
                {
                    UserId = "185fe62b-1a55-4927-b5f0-927155858470",
                    UserName = AppSettings.ADMIN,
                    UsePassword = BCrypt.Net.BCrypt.HashPassword("Admin"),
                    PhoneNumber="0346790482",
                    FullName="Tiến Nguyễn",
                    Active = true,
                    CreateDatetime = DateTime.Now,
                    CreateUserId = "Admin",
                    UpdateDatetime = DateTime.Now,
                    UpdateUserId = "Admin"
                };
                _db.Users.Add(user);
                var Role = _db.Roles.Where(x => x.Name == AppSettings.ADMIN).FirstOrDefault();
                var userRole = new UserRole
                {
                    RoleId= Role.Id,
                    UserId = user.UserId,
                };
                _db.UserRoles.Add(userRole);

                _db.SaveChanges();
            }
          
            return;
        }
    }
}
