namespace ICHI_API.Notes
{
  public class Tranning
  {

    //Login Service

    //public string Login(UserLogin userLogin, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    User loginUser = ExistsByUserNameOrEmail(userLogin.UserName);
    //    if (loginUser == null)
    //    {
    //      throw new BadRequestException(Constants.ACCOUNTNOTFOUNF);
    //    }
    //    if (!BCrypt.Net.BCrypt.Verify(userLogin.Password, loginUser.Password))
    //    {
    //      loginUser.FailedPassAttemptCount++;
    //      if (loginUser.FailedPassAttemptCount >= 5)
    //      {
    //        loginUser.IsLocked = true;
    //        strMessage = Constants.ACCOUNTLOCK;
    //      }
    //      else
    //      {
    //        strMessage = Constants.PASSWORDOLDPFAIL;
    //      }
    //      _unitOfWork.User.Update(loginUser);
    //      _unitOfWork.Save();
    //      return null;
    //    }
    //    strMessage = Constants.LOGINSUCCESS;
    //    var accessToken = GenerateAccessToken(loginUser);
    //    SetJWTCookie(accessToken);
    //    return accessToken;
    //  }
    //  catch (Exception)
    //  {
    //    throw;
    //  }
    //}


    //Login PLANUML

    //    @startuml
    //participant UserLogin
    //participant LoginService
    //participant UserRepository
    //participant BCrypt
    //participant Constants
    //participant UnitOfWork

    //UserLogin -> LoginService: Login(UserLogin, out strMessage)
    //LoginService -> UserRepository: ExistsByUserNameOrEmail(UserLogin.UserName)
    //UserRepository --> LoginService: loginUser or null

    //LoginService -> LoginService: Check if loginUser is null
    //LoginService -> Constants: ACCOUNTNOTFOUND(if loginUser is null)
    //LoginService -> LoginService: Throw BadRequestException(if loginUser is null)
    //LoginService --> UserLogin: strMessage = ACCOUNTNOTFOUND(if loginUser is null)

    //LoginService -> BCrypt: Verify(UserLogin.Password, loginUser.Password)
    //BCrypt --> LoginService: true or false

    //LoginService -> LoginService: Check verification result(true or false)
    //LoginService -> loginUser: Increment FailedPassAttemptCount(if verification fails)
    //LoginService -> loginUser: Check if FailedPassAttemptCount >= 5 (if verification fails)
    //LoginService -> loginUser: Set IsLocked to true (if FailedPassAttemptCount >= 5)
    //LoginService -> Constants: ACCOUNTLOCK(if FailedPassAttemptCount >= 5)
    //LoginService -> Constants: PASSWORDOLDPFAIL(if FailedPassAttemptCount< 5)
    //LoginService -> UserRepository: Update(loginUser)

    //UserRepository -> UnitOfWork: Update(loginUser)
    //UnitOfWork --> UserRepository: Update completed

    //LoginService -> UserRepository: Save()
    //UserRepository -> UnitOfWork: Save()
    //UnitOfWork --> UserRepository: Save completed

    //LoginService --> UserLogin: Return null (if verification fails)
    //LoginService --> UserLogin: strMessage = ACCOUNTLOCK or PASSWORDOLDPFAIL(if verification fails)

    //LoginService -> LoginService: GenerateAccessToken(loginUser) (if verification is successful)
    //LoginService -> UserLogin: SetJWTCookie(accessToken)
    //LoginService --> UserLogin: accessToken(if verification is successful)
    //@enduml

    //ForgotPassword Service
    //public bool ForgotPassword(string email, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    User loginUser = ExistsByUserNameOrEmail(email);
    //    if (loginUser == null)
    //    {
    //      throw new BadRequestException(Constants.ACCOUNTNOTFOUNF);
    //    }
    //    var emailService = new EmailService(_configuration);
    //    // random mật khẩu mới
    //    Random random = new Random();
    //    string randomString = new string(Enumerable.Repeat(AppSettings.Encode, random.Next(8, 16))
    //               .Select(s => s[random.Next(s.Length)]).ToArray());
    //    string salt = BCrypt.Net.BCrypt.GenerateSalt();
    //    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(randomString, salt);
    //    User user = _unitOfWork.User.Get(u => u.Email == loginUser.Email || u.Email.Equals(email));
    //    user.Password = hashedPassword;
    //    user.ModifiedBy = loginUser.Email;
    //    user.ModifiedDate = DateTime.Now;
    //    _unitOfWork.User.Update(user);
    //    _unitOfWork.Save();
    //    string url = "Mật khẩu mới của bạn là: " + randomString;
    //    string body = "Click vào link sau để đổi mật khẩu: " + url;
    //    emailService.SendEmail(email, "Reset password", url);
    //    strMessage = Constants.SENDMAILSUCCESS;
    //    return true;
    //  }
    //  catch (BadRequestException ex)
    //  {
    //    strMessage = ex.Message;
    //    return flase;
    //  }
    //  catch (Exception)
    //  {
    //    throw;
    //  }
    //}

    //ForgotPassword PLANUML
    //    @startuml
    //participant AuthController
    //participant LoginService
    //participant UserRepository
    //participant EmailService
    //participant BCrypt
    //participant Random
    //participant Constants
    //participant UnitOfWork
    //participant AppSettings

    //AuthController -> LoginService: ForgotPassword(email, out strMessage)
    //LoginService -> UserRepository: ExistsByUserNameOrEmail(email)
    //UserRepository --> LoginService: loginUser or null

    //LoginService -> LoginService: Check if loginUser is null
    //LoginService -> Constants: ACCOUNTNOTFOUND(if loginUser is null)
    //LoginService -> LoginService: Throw BadRequestException(if loginUser is null)
    //LoginService --> AuthController: strMessage = ACCOUNTNOTFOUND(if loginUser is null)
    //LoginService --> AuthController: Return false (if loginUser is null)

    //LoginService -> Random: Generate random password
    //Random --> LoginService: randomString

    //LoginService -> AppSettings: Get encode characters
    //LoginService --> AppSettings: AppSettings.Encode

    //LoginService -> BCrypt: GenerateSalt()
    //BCrypt --> LoginService: salt
    //LoginService -> BCrypt: HashPassword(randomString, salt)
    //BCrypt --> LoginService: hashedPassword

    //LoginService -> UserRepository: Get(u => u.Email == loginUser.Email or u.Email.Equals(email))
    //UserRepository --> LoginService: user

    //LoginService -> user: Update user details(Password, ModifiedBy, ModifiedDate)
    //LoginService -> UserRepository: Update(user)
    //UserRepository --> LoginService: Update completed

    //LoginService -> UnitOfWork: Save()
    //UnitOfWork --> LoginService: Save completed

    //LoginService -> LoginService: Prepare email content(url and body)
    //LoginService -> EmailService: SendEmail(email, SENDMAILSUBJECT, url)
    //EmailService --> LoginService: Email sent

    //LoginService --> AuthController: strMessage = SENDMAILSUCCESS
    //LoginService --> AuthController: Return true
    //@enduml



    //ChangePassword Service
    //public string ChangePassword(UserChangePassword user, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var userExist = ExistsByUserNameOrEmail(user.UserName);
    //    if (userExist != null && BCrypt.Net.BCrypt.Verify(user.oldPassword, userExist.Password))
    //    {
    //      string salt = BCrypt.Net.BCrypt.GenerateSalt();
    //      string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password, salt);
    //      userExist.Password = hashedPassword;
    //      userExist.ModifiedBy = user.UserName;
    //      userExist.ModifiedDate = DateTime.Now;
    //      _unitOfWork.User.Update(userExist);
    //      _unitOfWork.Save();
    //      strMessage = Constants.PASSWORDSUCCESS;
    //      return Constants.PASSWORDSUCCESS;
    //    }
    //    else
    //    {
    //      throw new BadRequestException(Constants.PASSWORDOLDPFAIL);
    //    }
    //  }
    //  catch (Exception)
    //  {
    //    throw;
    //  }
    //}

    //ChangePassword PLANUML

    //    @startuml
    //participant AuthController
    //participant LoginService
    //participant UserRepository
    //participant BCrypt
    //participant Constants
    //participant UnitOfWork

    //AuthController -> LoginService: ChangePassword(UserChangePassword, out strMessage)
    //LoginService -> UserRepository: ExistsByUserNameOrEmail(UserChangePassword.UserName)
    //UserRepository --> LoginService: userExist or null

    //LoginService -> LoginService: Check if userExist is null or password doesn't match
    //LoginService -> BCrypt: Verify(UserChangePassword.oldPassword, userExist.Password)
    //BCrypt --> LoginService: true or false

    //LoginService -> LoginService: Check verification result(true or false)
    //LoginService -> LoginService: If true, generate salt and hash password
    //LoginService -> BCrypt: GenerateSalt()
    //BCrypt --> LoginService: salt
    //LoginService -> BCrypt: HashPassword(UserChangePassword.Password, salt)
    //BCrypt --> LoginService: hashedPassword

    //LoginService -> userExist: Update password, modifiedBy, and modifiedDate
    //LoginService -> UserRepository: Update(userExist)
    //UserRepository --> LoginService: Update completed

    //LoginService -> UserRepository: Save()
    //UserRepository -> UnitOfWork: Save()
    //UnitOfWork --> UserRepository: Save completed

    //LoginService -> Constants: PASSWORDSUCCESS(if password change successful)
    //LoginService --> AuthController: Return PASSWORDSUCCESS(if password change successful)
    //LoginService --> AuthController: strMessage = PASSWORDSUCCESS

    //LoginService -> Constants: PASSWORDOLDPFAIL(if old password verification fails)
    //LoginService -> LoginService: Throw BadRequestException(if old password verification fails)
    //@enduml

    //Register Service
    //public string Register(UserRegister userRegister, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    using (var transaction = _db.Database.BeginTransaction())
    //    {
    //      // kiểm tra người dùng có hợp lệ không
    //      if (ExistsByPhoneNumber(userRegister.PhoneNumber.Trim()))
    //      {
    //        throw new BadRequestException(Constants.PHONENUMBEREXIST);
    //      }
    //      if (ExistsByUserNameOrEmail(userRegister.Email.Trim()) != null)
    //      {
    //        throw new BadRequestException(Constants.USEREXIST);
    //      }
    //      // mật khẩu phải > 8 kí tự, 1 chữ hoa, 1 chữ thường, 1 kí tự đặc biệt

    //      if (!userRegister.Password.Any(char.IsUpper) || !userRegister.Password.Any(char.IsLower) || !userRegister.Password.Any(char.IsDigit) || userRegister.Password.Length < 8)
    //      {
    //        throw new BadRequestException(Constants.PASSWORDREGEX);
    //      }
    //      //userRegister.Role= AppSettings.EMPLOYEE;
    //      User user = new User();
    //      MapperHelper.Map<UserRegister, User>(userRegister, user);
    //      string salt = BCrypt.Net.BCrypt.GenerateSalt();
    //      string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRegister.Password.Trim(), salt);
    //      user.Password = hashedPassword;
    //      user.IsLocked = false;
    //      user.Avatar = AppSettings.AvatarDefault;
    //      user.CreateBy = userRegister.Email;
    //      user.ModifiedBy = userRegister.Email;
    //      user.Email = userRegister.Email.Trim();
    //      _unitOfWork.User.Add(user);
    //      _unitOfWork.Save();
    //      if (userRegister.Role == AppSettings.EMPLOYEE)
    //      {
    //        // insert vào employee
    //        Employee employee = new Employee()
    //        {
    //          PhoneNumber = userRegister.PhoneNumber,
    //          FullName = userRegister.FullName,
    //          UserId = user.Email,
    //          Gender = userRegister.Gender,
    //          Birthday = userRegister.Birthday,
    //          Address = userRegister.Address,
    //        };
    //        _unitOfWork.Employee.Add(employee);
    //        _unitOfWork.Save();
    //        UserRole userRole = new UserRole()
    //        {
    //          RoleId = _unitOfWork.Role.Get(r => r.RoleName == AppSettings.EMPLOYEE).Id,
    //          UserId = user.Email,
    //        };
    //        _unitOfWork.UserRole.Add(userRole);
    //        _unitOfWork.Save();
    //      }
    //      else
    //      {
    //        // insert vào customer
    //        Customer customer = new Customer()
    //        {
    //          PhoneNumber = userRegister.PhoneNumber,
    //          FullName = userRegister.FullName,
    //          UserId = user.Email,
    //          Gender = userRegister.Gender,
    //          Birthday = userRegister.Birthday,
    //          //Address = userRegister.Address
    //        };
    //        _unitOfWork.Customer.Add(customer);
    //        _unitOfWork.Save();
    //        UserRole userRole = new UserRole()
    //        {
    //          RoleId = _unitOfWork.Role.Get(r => r.RoleName == AppSettings.USER).Id,
    //          UserId = user.Email,
    //        };
    //        _unitOfWork.UserRole.Add(userRole);
    //        _unitOfWork.Save();
    //      }
    //      transaction.Commit();
    //      strMessage = Constants.REGISTERSUCCESS;
    //      var accessToken = GenerateAccessToken(user);
    //      SetJWTCookie(accessToken);
    //      return accessToken;
    //    }
    //  }
    //  catch (Exception)
    //  {
    //    throw;
    //  }
    //}

    //Register PLANUML


    //    @startuml
    //    participant AuthController
    //    participant LoginService
    //participant UserRepository
    //participant UnitOfWork
    //participant BCrypt
    //    participant Constants
    //participant DatabaseTransaction

    //AuthController -> LoginService: Register(UserRegister, out strMessage)
    //LoginService -> UserRepository: ExistsByPhoneNumber(userRegister.PhoneNumber)
    //UserRepository --> LoginService: true or false

    //LoginService -> UserRepository: ExistsByUserNameOrEmail(userRegister.Email)
    //UserRepository --> LoginService: userExist or null

    //LoginService -> LoginService: Check userRegister.Password requirements
    //LoginService --> AuthController: If password does not meet requirements, return BadRequestException

    //LoginService -> LoginService: Create new user instance
    //LoginService -> BCrypt: GenerateSalt()
    //BCrypt --> LoginService: salt
    //LoginService -> BCrypt: HashPassword(userRegister.Password, salt)
    //BCrypt --> LoginService: hashedPassword

    //LoginService -> User: Assign hashedPassword, set other properties
    //LoginService -> UserRepository: Add user
    //LoginService -> UnitOfWork: Save user data
    //UnitOfWork --> LoginService: Save completed

    //LoginService -> LoginService: Check userRegister.Role
    //LoginService -> LoginService: If employee, create Employee and UserRole
    //LoginService -> LoginService: If customer, create Customer and UserRole

    //LoginService -> UserRepository: Add Employee or Customer
    //LoginService -> UnitOfWork: Save
    //UnitOfWork --> LoginService: Save completed

    //LoginService -> UserRepository: Add UserRole
    //LoginService -> UnitOfWork: Save
    //UnitOfWork --> LoginService: Save completed

    //LoginService -> DatabaseTransaction: Commit transaction
    //DatabaseTransaction --> LoginService: Transaction committed

    //LoginService --> AuthController: Return accessToken
    //LoginService --> AuthController: strMessage = Constants.REGISTERSUCCESS
    //@enduml

    //  LockAccount Service
    //public bool LockAccount(string id, bool status, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var data = _unitOfWork.User.Get(u => u.Email == id);
    //    if (data == null)
    //    {
    //      throw new BadRequestException(Constants.ACCOUNTNOTFOUND);
    //    }
    //    data.IsLocked = status;
    //    data.ModifiedDate = DateTime.Now;
    //    data.ModifiedBy = "Admin";
    //    _unitOfWork.User.Update(data);
    //    _unitOfWork.Save();
    //    strMessage = Constants.ACCOUNTLOCKSUCCESS(status);
    //    return true;
    //  }
    //  catch (Exception)
    //  {
    //    throw;
    //  }
    //}

    //LockAccount PLANUML
    //    @startuml
    //participant AuthController
    //participant LoginService
    //participant UserRepository
    //    participant UnitOfWork
    //participant Constants

    //AuthController -> LoginService: LockAccount(string id, bool status, out strMessage)
    //LoginService -> UserRepository: Get(u => u.Email == id)
    //UserRepository --> LoginService: data or null

    //LoginService -> LoginService: Check if data is null
    //LoginService -> Constants: ACCOUNTNOTFOUND(if data is null)
    //LoginService -> LoginService: Throw BadRequestException(if data is null)

    //LoginService -> data: Set IsLocked to status
    //LoginService -> data: Set ModifiedDate to DateTime.Now
    //LoginService -> data: Set ModifiedBy to "Admin"
    //LoginService -> UserRepository: Update(data)
    //UserRepository -> UnitOfWork: Update data
    //UnitOfWork --> UserRepository: Update completed

    //LoginService -> UserRepository: Save()
    //UserRepository -> UnitOfWork: Save()
    //UnitOfWork --> UserRepository: Save completed

    //LoginService -> Constants: ACCOUNTLOCKSUCCESS(status)
    //LoginService --> AuthController: Return true
    //LoginService --> AuthController: strMessage = ACCOUNTLOCKSUCCESS(status)
    //@enduml

  }
}