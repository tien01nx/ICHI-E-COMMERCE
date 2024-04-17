namespace ICHI_API.Notes
{
  public class DetailedProcessing
  {
    //  C:\Users\tien0\source\repos\tien01nx\ICHI-E-COMMERCE\ICHI_API\Service\AuthService.cs
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

    //    @startuml
    //(*) --> "Retrieve user by username or email"

    //if "User found?" then
    //    -->[yes] "Verify password"
    //    if "Password matches?" then
    //        -->[yes] "Generate access token"
    //        --> "Set JWT cookie"
    //        --> "Return login success message"
    //        --> (*)
    //    else
    //        -->[no] "Increment failed password attempt count"
    //        if "Failed attempt count >= 5?" then
    //            -->[yes] "Lock account"
    //            --> "Return account locked message"
    //            --> (*)
    //        else
    //            -->[no] "Return password fail message"
    //            --> (*)
    //        endif
    //    endif
    //else
    //    -->[no] "Throw BadRequestException: ACCOUNT_NOT_FOUND"
    //    --> (*)
    //endif
    //@enduml

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


    //    @startuml
    //(*) --> "Begin database transaction"
    //--> "Check if phone number exists"
    //if "Phone number exists?" then
    //    -->[yes] "Throw BadRequestException: PHONENUMBEREXIST"
    //    --> (*)
    //else
    //    -->[no] "Check if email exists"
    //    if "Email already exists?" then
    //        -->[yes] "Throw BadRequestException: USEREXIST"
    //        --> (*)
    //    else
    //        -->[no] "Validate password format"
    //        if "Password valid?" then
    //            -->[yes] "Map UserRegister to User"
    //            --> "Add user to USER table"
    //            --> "Determine user role"
    //            if "User role is Employee?" then
    //                -->[yes] "Add record to Employee table"
    //                --> "Add record to UserRole table"
    //            else
    //                -->[no] "Add record to Customer table"
    //                --> "Add record to UserRole table"
    //            endif
    //            --> "Commit data to database"
    //            --> "Generate access token"
    //            --> "Return access token and success message"
    //            --> (*)
    //        else
    //            -->[no] "Throw BadRequestException: PASSWORDREGEX"
    //            --> (*)
    //        endif
    //    endif
    //endif
    //@enduml

    //public string Login(UserLogin userLogin, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    User loginUser = ExistsByEmail(userLogin.UserName);
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


    //    @startuml
    //(*) --> "Check if user exists by email"
    //if "User exists?" then
    //    -->[yes] "Verify password"
    //    if "Password is correct?" then
    //        -->[yes] "Set success message"
    //        --> "Generate access token"
    //        --> "Set JWT cookie"
    //        --> "Return access token"
    //        --> (*)
    //    else
    //        -->[no] "Increment failed password attempt count"
    //        if "Failed password attempt count >= 5?" then
    //            -->[yes] "Lock user account"
    //            --> "Set account locked message"
    //        else
    //            -->[no] "Set password failure message"
    //        endif
    //        --> "Update user record"
    //        --> "Save changes"
    //        --> "Return null"
    //        --> (*)
    //    endif
    //else
    //    -->[no] "Throw BadRequestException: ACCOUNTNOTFOUND"
    //    --> (*)
    //endif
    //@enduml

    //public bool ChangePassword(UserChangePassword user, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var userExist = ExistsByEmail(user.UserName);
    //    if (userExist != null && BCrypt.Net.BCrypt.Verify(user.oldPassword, userExist.Password))
    //    {
    //      string salt = BCrypt.Net.BCrypt.GenerateSalt();
    //      string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password, salt);
    //      userExist.Password = hashedPassword;
    //      _unitOfWork.User.Update(userExist);
    //      _unitOfWork.Save();
    //      strMessage = Constants.PASSWORDSUCCESS;
    //      return true;
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

    //@startuml
    //(*) --> "Check if user exists by email"
    //if "User exists and old password is correct?" then
    //    -->[yes] "Generate new salt"
    //    --> "Hash new password"
    //    --> "Update user password"
    //    --> "Save changes"
    //    --> "Set success message"
    //    --> "Return true"
    //    --> (*)
    //else
    //    -->[no] "Throw BadRequestException: PASSWORDOLDPFAIL"
    //    --> (*)
    //endif
    //@enduml

    //public bool ForgotPassword(string email, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    User loginUser = ExistsByEmail(email);
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
    //    string url = SENDMAILSUCCESS + randomString;
    //    string body = SENDMAILBODY + url;
    //    emailService.SendEmail(email, SENDMAILSUBJECT, url);
    //    strMessage = SENDMAILSUCCESS;
    //    return true;
    //  }
    //  catch (Exception)
    //  {
    //    throw;
    //  }
    //}

    //    @startuml
    //(*) --> "Check if user exists by email"
    //    if "User exists?" then
    //        -->[yes] "Create a new random password"
    //        --> "Hash the new password"
    //        --> "Update user password"
    //        --> "Save changes"
    //        --> "Send new password by email"
    //        --> "Set success message"
    //        --> "Return true"
    //        --> (*)
    //    else
    //        -->[no] "Set error message"
    //        --> "Return false"
    //        --> (*)
    //    endif
    //    @enduml


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

    //    @startuml
    //(*) --> "Get user by email ID"
    //if "User found?" then
    //    -->[yes] "Update account lock status"
    //    --> "Set modified date and modified by"
    //    --> "Save changes"
    //    --> "Set success message"
    //    --> "Return true"
    //    --> (*)
    //else
    //    -->[no] "Set error message"
    //    --> "Return false"
    //    --> (*)
    //endif
    //@enduml


    // category product service

    //public Helpers.PagedResult<Category> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var query = _unitOfWork.Category.GetAll(u => !u.IsDeleted).AsQueryable();
    //    if (!string.IsNullOrEmpty(name))
    //    {
    //      query = query.Where(e => e.CategoryName.Contains(name.Trim()));
    //    }
    //    var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
    //    query = query.OrderBy(orderBy);
    //    var pagedResult = Helpers.PagedResult<Category>.CreatePagedResult(query, pageNumber, pageSize);
    //    return pagedResult;
    //  }
    //  catch (Exception ex)
    //  {
    //    throw;
    //  }
    //}


    //    @startuml
    //(*) --> "Query all categories that were not deleted"
    // --> "There is a category name provided?"
    //if "There is a category name provided?" then
    //    -->[yes] "Filter categories by name"
    //    --> "Sort categories"
    //else
    //    -->[no] "Sort categories"
    //endif
    //"Sort categories" --> "Generate pagination results"
    //"Generate pagination results" --> "Returns the pagination results"
    //"Returns the pagination results" --> (*)
    //@enduml



    //public Category FindById(int id, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var data = _unitOfWork.Category.Get(u => u.Id == id);
    //    if (data == null)
    //    {
    //      throw new BadRequestException(Constants.CATEGORYNOTFOUND);
    //    }
    //    return data;
    //  }
    //  catch (Exception ex)
    //  {
    //    throw;
    //  }
    //}

    //    @startuml
    //(*) --> "Find category by ID"
    //"Find category by ID" --> "Category exists?"
    //if "Category exists?" then
    //    -->[Yes] "Return category"
    //    --> (*)
    //else
    //    -->[No] "Throw BadRequestException: CATEGORYNOTFOUND"
    //    --> (*)
    //endif
    //@enduml

    //public Category Create(Category category, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var checkCategory = _unitOfWork.Category.Get(u => u.CategoryName == category.CategoryName);
    //    if (checkCategory != null)
    //    {
    //      throw new BadRequestException(Constants.CATEGORYEXIST);
    //    }
    //    var categoryParentId = _unitOfWork.Category.Get(u => u.Id == category.ParentID);
    //    if (categoryParentId == null)
    //    {
    //      throw new BadRequestException(Constants.CATEGORYPARENTNOTFOUND);
    //    }
    //    category.CategoryLevel = categoryParentId.CategoryLevel + 1;
    //    //category.CreateBy = "Admin";
    //    //category.ModifiedBy = "Admin";
    //    _unitOfWork.Category.Add(category);
    //    _unitOfWork.Save();
    //    strMessage = Constants.ADDCATEGORYSUCCESS;
    //    return category;
    //  }
    //  catch (Exception ex)
    //  {
    //    throw;
    //  }
    //}

    //    @startuml
    //(*) --> "Check for existing category name"
    //"Check for existing category name" --> "Category name exists?"
    //if "Category name exists?" then
    //    -->[Yes] "Throw BadRequestException: CATEGORYEXIST"
    //    --> (*)
    //else
    //    -->[No] "Check for parent category"
    //    "Check for parent category" --> "Parent category exists?"
    //    if "Parent category exists?" then
    //        -->[Yes] "Calculate category level"
    //        --> "Add category to database"
    //        --> "Save changes"
    //        --> "Set success message"
    //        --> "Return new category"
    //        --> (*)
    //    else
    //        -->[No] "Throw BadRequestException: CATEGORYPARENTNOTFOUND"
    //        --> (*)
    //    endif
    //endif
    //@enduml

    //    public Category Update(Category category, out string strMessage)
    //    {
    //      strMessage = string.Empty;
    //      try
    //      {
    //        // lấy thông tin nhà cung cấp
    //        var data = _unitOfWork.Category.Get(u => u.Id == category.Id);
    //        if (data == null)
    //        {
    //          throw new BadRequestException(Constants.CATEGORYNOTFOUND);
    //        }
    //        var checkCategory = _unitOfWork.Category.Get(u => u.CategoryName == category.CategoryName && u.Id != category.Id);
    //        if (checkCategory != null)
    //        {

    //          throw new BadRequestException(Constants.CATEGORYEXIST);
    //        }

    //        var categoryParentId = _unitOfWork.Category.Get(u => u.Id == category.ParentID);
    //        if (categoryParentId == null)
    //        {
    //          throw new BadRequestException(Constants.CATEGORYPARENTNOTFOUND);
    //        }
    //        category.CategoryLevel = categoryParentId.CategoryLevel + 1;
    //        category.ModifiedBy = "Admin";
    //        _unitOfWork.Category.Update(category);
    //        _unitOfWork.Save();
    //        strMessage = Constants.UPDATECATEGORYSUCCESS;
    //        return category;
    //      }
    //      catch (Exception)
    //      {
    //        throw;
    //      }
    //    }
    //    @startuml
    //(*) --> "Retrieve existing category data by ID"
    //"Retrieve existing category data by ID" --> "Category found?"
    //if "Category found?" then
    //    -->[Yes] "Check for duplicate category name"
    //    "Check for duplicate category name" --> "Duplicate category name found?"
    //    if "Duplicate category name found?" then
    //        -->[Yes] "Throw BadRequestException: CATEGORYEXIST"
    //        --> (*)
    //    else
    //        -->[No] "Check for parent category"
    //        "Check for parent category" --> "Parent category exists?"
    //        if "Parent category exists?" then
    //            -->[Yes] "Calculate category level"
    //            --> "Update category in database"
    //            --> "Save changes"
    //            --> "Set success message"
    //            --> "Return updated category"
    //            --> (*)
    //        else
    //            -->[No] "Throw BadRequestException: CATEGORYPARENTNOTFOUND"
    //            --> (*)
    //        endif
    //    endif
    //else
    //    -->[No] "Throw BadRequestException: CATEGORYNOTFOUND"
    //    --> (*)
    //endif
    //@enduml

    //public bool Delete(int id, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var data = _unitOfWork.Category.Get(u => u.Id == id);
    //    if (data == null)
    //    {
    //      throw new BadRequestException(Constants.CATEGORYNOTFOUND);
    //    }
    //    _unitOfWork.Category.Remove(data);
    //    _unitOfWork.Save();
    //    strMessage = Constants.DELETECATEGORYSUCCESS;
    //    return true;
    //  }
    //  catch (Exception)
    //  {
    //    throw;
    //  }
    //}
    //    @startuml
    //(*) --> "Retrieve category by ID"
    //"Retrieve category by ID" --> "Category found?"
    //if "Category found?" then
    //    -->[Yes] "Remove category from database"
    //    --> "Save changes"
    //    --> "Set success message"
    //    --> "Return true"
    //    --> (*)
    //else
    //    -->[No] "Throw BadRequestException: CATEGORYNOTFOUND"
    //    --> (*)
    //endif
    //@enduml


    //C:\Users\tien0\source\repos\tien01nx\ICHI-E-COMMERCE\ICHI_API\Service\CustomerService.cs

    //public Helpers.PagedResult<Customer> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var query = _db.Customers.Include(u => u.User).OrderByDescending(u => u.ModifiedDate).AsQueryable().Where(u => u.isDeleted == false);
    //    if (!string.IsNullOrEmpty(name))
    //    {
    //      query = query.Where(e => e.FullName.Contains(name.Trim()) || e.PhoneNumber.Contains(name.Trim()));
    //    }
    //    var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
    //    query = query.OrderBy(orderBy);
    //    var pagedResult = Helpers.PagedResult<Customer>.CreatePagedResult(query, pageNumber, pageSize);
    //    return pagedResult;
    //  }
    //  catch (Exception ex)
    //  {
    //    throw;
    //  }
    //}


    //    @startuml
    //    start

    //:Initialize strMessage to an empty string;

    //:Retrieve customers from the database;
    //:Filter out deleted customers(isDeleted == false);

    //if (Is name filter provided?) then(Yes)
    //  :Filter customers based on FullName or PhoneNumber containing the specified name;
    //else (No)
    //  :Skip name filtering;
    //endif

    //:Set order by with sortBy and sortDir;
    //:Apply order by clause to the query;

    //:Create paged result using CreatePagedResult method;

    //stop
    //@enduml




    //public Customer FindById(int id, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var data = _unitOfWork.Customer.Get(u => u.Id == id);
    //    if (data == null)
    //    {
    //      throw new BadRequestException(Constants.CUSTOMERNOTFOUND);
    //    }
    //    return data;
    //  }
    //  catch (Exception ex)
    //  {
    //    throw;
    //  }
    //}

    //    @startuml
    //(*) --> "Initialize strMessage to empty"
    //--> "Query database for customer by id"
    //if "Customer found?" then
    //    -->[No] "Throw BadRequestException: CUSTOMERNOTFOUND"
    //    --> (*)
    //else
    //    -->[Yes] "Return found customer data"
    //    --> (*)
    //endif
    //@enduml


    //    public Customer Create(Customer model, out string strMessage)
    //    {
    //      strMessage = string.Empty;
    //      try
    //      {

    //        var checkEmail = _unitOfWork.Customer.Get(u => u.UserId == model.UserId);
    //        if (checkEmail != null)
    //        {
    //          throw new BadRequestException(Constants.EMAILEXIST);
    //        }
    //        var checkPhone = _unitOfWork.Customer.Get(u => u.PhoneNumber == model.PhoneNumber);
    //        if (checkPhone != null)
    //        {
    //          throw new BadRequestException(Constants.PHONENUMBEREXISTCUSTOMER);
    //        }
    //        //model.CreateBy = "Admin";
    //        //model.ModifiedBy = "Admin";
    //        _unitOfWork.Customer.Add(model);
    //        _unitOfWork.Save();
    //        strMessage = Constants.CREATECUSTOMERSUCCESS;
    //        return model;
    //      }
    //      catch (Exception ex)
    //      {
    //        throw;
    //      }
    //    }

    //    @startuml
    //(*) --> "Initialize strMessage to empty"
    //--> "Check if email exists for the given UserId"
    //if "Email exists?" then
    //    -->[Yes] "Throw BadRequestException: EMAILEXIST"
    //    --> (*)
    //else
    //    -->[No] "Check if phone number exists for the given PhoneNumber"
    //    if "Phone number exists?" then
    //        -->[Yes] "Throw BadRequestException: PHONENUMBEREXISTCUSTOMER"
    //        --> (*)
    //    else
    //        -->[No] "Add the new Customer to the database"
    //        --> "Save the changes"
    //        --> "Set strMessage to CREATECUSTOMERSUCCESS"
    //        --> "Return the new Customer"
    //        --> (*)
    //    endif
    //endif
    //@enduml



    //public Customer Update(Customer customer, IFormFile? file, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    // kiểm tra email khách hàng đã tồn tại chưa
    //    var checkEmail = _unitOfWork.User.Get(u => u.Email == customer.UserId);
    //    if (checkEmail != null && checkEmail.Email != customer.UserId)
    //    {
    //      throw new BadRequestException(Constants.EMAILEXIST);
    //    }
    //    // kiểm tra số điện thoại khách hàng đã tồn tại chưa
    //    var checkPhone = _unitOfWork.Customer.Get(u => u.PhoneNumber == customer.PhoneNumber);
    //    if (checkPhone != null && checkPhone.Id != customer.Id)
    //    {
    //      throw new BadRequestException(Constants.PHONENUMBEREXISTCUSTOMER);
    //    }
    //    // nếu có file thì thực hiện lưu file mới và xóa file cũ đi
    //    // lấy đường dẫn ảnh file cũ
    //    if (file != null)
    //    {
    //      var user = _unitOfWork.User.Get(x => x.Email == customer.UserId);
    //      string oldFile = user.Avatar;
    //      user.Avatar = ImageHelper.AddImage(_webHostEnvironment.WebRootPath, user.Email, file, AppSettings.PatchUser);
    //      //user.ModifiedBy = "Admin";
    //      //user.ModifiedDate = DateTime.Now;
    //      _unitOfWork.User.Update(user);
    //      _unitOfWork.Save();
    //      // xóa file cũ
    //      if (oldFile != AppSettings.AvatarDefault)
    //      {
    //        ImageHelper.DeleteImage(_webHostEnvironment.WebRootPath, oldFile);
    //      }
    //    }

    //    customer.ModifiedBy = "Admin";
    //    _unitOfWork.Customer.Update(customer);
    //    _unitOfWork.Save();
    //    strMessage = Constants.UPDATECUSTOMERSUCCESS;
    //    return customer;
    //  }
    //  catch (Exception)
    //  {
    //    throw;
    //  }
    //}
    //    @startuml
    //    (*) --> "Initialize strMessage to empty"
    //--> "Check if email exists for the given UserId and not current UserId"
    //if "Email exists and not current?" then
    //    -->[Yes] "Throw BadRequestException: EMAILEXIST"
    //    --> (*)
    //else
    //    -->[No] "Check if phone number exists for the given PhoneNumber and not current Customer Id"
    //    if "Phone number exists and not current?" then
    //        -->[Yes] "Throw BadRequestException: PHONENUMBEREXISTCUSTOMER"
    //        --> (*)
    //    else
    //        -->[No] "Check if file is provided"
    //        if "File provided?" then
    //            -->[Yes] "Retrieve User record for the given UserId"
    //            --> "Save new file, update User's Avatar, and delete old file if necessary"
    //            --> "Save changes to the User" 
    //--> "Update Customer's ModifiedBy to Admin"
    //        else
    //            -->[No] "No file provided"
    //        endif
    //        --> "Update Customer's ModifiedBy to Admin"
    //        --> "Update the Customer record"
    //        --> "Save the changes"
    //        --> "Set strMessage to UPDATECUSTOMERSUCCESS"
    //        --> "Return the updated Customer"
    //        --> (*)
    //    endif
    //endif
    //@enduml



    //public bool Delete(int id, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var data = _unitOfWork.Customer.Get(u => u.Id == id);
    //    if (data == null)
    //    {
    //      throw new BadRequestException(Constants.CUSTOMERNOTFOUND);
    //    }
    //    data.isDeleted = true;
    //    data.ModifiedDate = DateTime.Now;
    //    _unitOfWork.Customer.Update(data);
    //    _unitOfWork.Save();
    //    strMessage = Constants.DELETECUSTOMERSUCCESS;
    //    return true;
    //  }
    //  catch (Exception)
    //  {
    //    throw;
    //  }
    //    @startuml
    //(*) --> "Initialize strMessage to empty"
    //--> "Retrieve customer record with the specified ID"
    //if "Customer record found?" then
    //    -->[No] "Throw BadRequestException: CUSTOMERNOTFOUND"
    //    --> (*)
    //else
    //    -->[Yes] "Mark the customer as deleted (isDeleted = true)"
    //    --> "Update ModifiedDate to current date and time"
    //    --> "Update the customer record in the database"
    //    --> "Save the changes"
    //    --> "Set strMessage to DELETECUSTOMERSUCCESS"
    //    --> "Return true"
    //    --> (*)
    //endif
    //@enduml


    //C:\Users\tien0\source\repos\tien01nx\ICHI-E-COMMERCE\ICHI_API\Service\EmployeeService.cs
    //public Helpers.PagedResult<Employee> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var query = _db.Employees.Include(u => u.User).OrderByDescending(u => u.ModifiedDate).AsQueryable().Where(u => u.isDeleted == false);
    //    if (!string.IsNullOrEmpty(name))
    //    {
    //      query = query.Where(e => e.FullName.Contains(name.Trim()) || e.PhoneNumber.Contains(name.Trim()));
    //    }
    //    var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
    //    query = query.OrderBy(orderBy);
    //    var pagedResult = Helpers.PagedResult<Employee>.CreatePagedResult(query, pageNumber, pageSize);
    //    return pagedResult;
    //  }
    //  catch (Exception ex)
    //  {
    //    throw;
    //  }
    //}

    //@startuml
    //(*) --> "Initialize strMessage to an empty string" 
    //    --> "Retrieve employees from the database"
    //    --> "Filter out deleted employees(isDeleted == false)"

    //    if "Is name filter provided?" then
    //        -->[yes] "Filter employees based on FullName or PhoneNumber containing the specified name"
    //        --> "Set order by clause with sortBy and sortDir"
    //        --> "Apply order by clause to the query"
    //        --> "Create paged result using CreatePagedResult method"
    //        --> (*)
    //    else
    //        -->[no] "Skip name filtering"
    //        --> "Set order by clause with sortBy and sortDir"

    //    endif
    //@enduml


    //public Employee FindById(int id, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var data = _unitOfWork.Employee.Get(u => u.Id == id && !u.isDeleted);
    //    if (data == null)
    //    {
    //      throw new BadRequestException(EMPLOYEENOTFOUND);
    //    }
    //    return data;
    //  }
    //  catch (Exception)
    //  {
    //    throw;
    //  }
    //}

    //    @startuml
    //start
    //:Retrieve employee data from the database by ID;
    //if (data is null or isDeleted?) then
    //  [Yes]
    //  :Throw BadRequestException with EMPLOYEENOTFOUND message;
    //  stop
    //else
    //  [No]
    //  :Return the employee data;
    //    stop
    //  endif
    //@enduml

  }
}
