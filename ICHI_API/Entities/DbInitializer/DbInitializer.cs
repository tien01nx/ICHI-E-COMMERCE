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
                        RoleName = AppSettings.ADMIN,
                        CreateDate = DateTime.Now,
                        CreateBy = AppSettings.ADMIN,
                        ModifiedDate = DateTime.Now,
                        ModifiedBy = AppSettings.ADMIN
                    },
                    new Role
                    {
                        RoleName = AppSettings.USER,
                        CreateDate = DateTime.Now,
                        CreateBy = AppSettings.ADMIN,
                        ModifiedDate = DateTime.Now,
                        ModifiedBy = AppSettings.ADMIN
                    },
                    new Role
                    {
                        RoleName = AppSettings.EMPLOYEE,
                        CreateDate = DateTime.Now,
                        CreateBy = AppSettings.ADMIN,
                        ModifiedDate = DateTime.Now,
                        ModifiedBy = AppSettings.ADMIN
                    }
                };

        _db.Roles.AddRange(rolesToAdd);
        _db.SaveChanges();
        string salt = BCrypt.Net.BCrypt.GenerateSalt();
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword("Admin", salt);
        var user = new User
        {
          UserName = AppSettings.ADMIN,

          Password = hashedPassword,
          IsLocked = false,
          CreateDate = DateTime.Now,
          CreateBy = AppSettings.ADMIN,
          ModifiedDate = DateTime.Now,
          ModifiedBy = AppSettings.ADMIN
        };
        _db.Users.Add(user);
        _db.SaveChanges();

        var Role = _db.Roles.Where(x => x.RoleName == AppSettings.ADMIN).FirstOrDefault();
        var userRole = new UserRole
        {
          RoleId = Role.Id,
          UserId = user.Id,
        };
        _db.UserRoles.Add(userRole);

        _db.SaveChanges();
      }

      return;
    }
  }
}
