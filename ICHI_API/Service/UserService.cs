using API.Model;
using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Extension;
using ICHI_API.Model;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.Helpers;
using ICHI_CORE.Model;
using ICHI_CORE.NlogConfig;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq.Dynamic.Core;

namespace ICHI_API.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        private PcsApiContext _db;

        public UserService(IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, PcsApiContext pcsApiContext)
        {
            _unitOfWork = unitOfWork;
            _db = pcsApiContext;
        }

        public Helpers.PagedResult<UserDTO> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
        {
            strMessage = string.Empty;
            try
            {
                var query = _unitOfWork.UserRole.GetAll(ur => ur.User != null, includeProperties: "User,Role");
                var userDTOs = query.Select(ur => new UserDTO
                {
                    User = ur.User,
                    Role = ur.Role != null ? ur.Role.RoleName : "",
                    Email = ur.User.Email,
                    Password = ur.User.Password,
                }).ToList();

                foreach (var userDTO in userDTOs)
                {
                    if (userDTO.Role == AppSettings.USER)
                    {
                        var customer = _unitOfWork.Customer.Get(c => c.UserId == userDTO.User.Email);
                        if (customer == null)
                        {
                            continue;
                        }
                        userDTO.FullName = customer.FullName;
                        userDTO.Birthday = customer.Birthday;
                        userDTO.Gender = customer.Gender;
                    }
                    else if (userDTO.Role == AppSettings.EMPLOYEE)
                    {
                        var employee = _unitOfWork.Employee.Get(e => e.UserId == userDTO.User.Email);
                        if (employee == null)
                        {
                            continue;
                        }
                        userDTO.FullName = employee.FullName;
                        userDTO.Birthday = employee.Birthday;
                        userDTO.Gender = employee.Gender;
                    }
                }

                if (!string.IsNullOrEmpty(name))
                {
                    userDTOs = userDTOs.Where(u => u.Email.Contains(name)).ToList();
                }

                var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
                userDTOs = userDTOs.AsQueryable().OrderBy(orderBy).ToList();
                var pagedResult = Helpers.PagedResult<UserDTO>.CreatePagedResult(userDTOs.AsQueryable(), pageNumber, pageSize);
                return pagedResult;
            }
            catch (Exception ex)
            {
                NLogger.log.Error(ex.ToString());
                strMessage = ex.ToString();
                return null;
            }
        }


        public UserDTO UpdateAccount(UserDTO userDTO, out string strMessage)
        {
            strMessage = string.Empty;
            try
            {
                var userRole = _unitOfWork.UserRole.Get(ur => ur.UserId == userDTO.Email, "User");
                var userRoleCu = _unitOfWork.UserRole.Get(ur => ur.UserId == userDTO.Email, "User,Role");
                if (userRole == null)
                {
                    strMessage = "Tài khoản không tồn tại";
                    return null;
                }
                // so sánh tên role người dùng truyền lên với role trong db
                if (userRoleCu.Role.RoleName != userDTO.Role)
                {
                    var role = _unitOfWork.Role.Get(r => r.RoleName == userDTO.Role);
                    if (role == null)
                    {
                        strMessage = "Role không tồn tại";
                        return null;
                    }
                    userRole.RoleId = role.Id;
                    _unitOfWork.UserRole.Update(userRole);
                    _unitOfWork.Save();
                }
                // kiểm tra userId tồn tại ở bảng nào thì update vào bảng đó là customner hoặc employee kiểm trả về là bool để xem ở bảng nào
                var isCustomer = _unitOfWork.Customer.Get(c => c.UserId == userDTO.Email) != null;
                if (isCustomer)
                {
                    var customer = _unitOfWork.Customer.Get(c => c.UserId == userDTO.Email);
                    if (customer == null)
                    {
                        strMessage = "Khách hàng không tồn tại";
                        return null;
                    }
                    customer.FullName = userDTO.FullName;
                    customer.Gender = userDTO.Gender;
                    customer.Birthday = userDTO.Birthday;
                    _unitOfWork.Customer.Update(customer);
                }
                var isEmployee = _unitOfWork.Employee.Get(e => e.UserId == userDTO.Email) != null;
                if (isEmployee)
                {
                    var employee = _unitOfWork.Employee.Get(e => e.UserId == userDTO.Email);
                    if (employee == null)
                    {
                        strMessage = "Nhân viên không tồn tại";
                        return null;
                    }
                    employee.FullName = userDTO.FullName;
                    employee.Gender = userDTO.Gender;
                    employee.Birthday = userDTO.Birthday;
                    _unitOfWork.Employee.Update(employee);
                }
                // update user khi thay đổi email
                if (!userRoleCu.User.Email.ToLower().Equals(userDTO.Email))
                {
                    var userdb = _unitOfWork.User.Get(u => u.Email == userRoleCu.UserId);
                    // kiểm tra email có tồn tại không
                    var user = _unitOfWork.User.Get(u => u.Email.ToLower().Equals(userDTO.Email) && u.Email != userRoleCu.User.Email);
                    if (user != null)
                    {
                        strMessage = "Email đã tồn tại";
                        return null;
                    }
                    userdb.Email = userDTO.Email;
                    _unitOfWork.User.Update(userdb);
                }
                _unitOfWork.Save();
                return userDTO;
            }
            catch (Exception ex)
            {
                NLogger.log.Error(ex.ToString());
                strMessage = ex.ToString();
                return null;
            }
        }
    }
}
