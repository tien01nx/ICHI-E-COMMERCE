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



    //public Employee Create(Employee model, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {

    //    var checkEmail = _unitOfWork.Employee.Get(u => u.UserId == model.UserId);
    //    if (checkEmail != null)
    //    {
    //      throw new BadRequestException(EMAILEXIST);
    //    }
    //    var checkPhone = _unitOfWork.Employee.Get(u => u.PhoneNumber == model.PhoneNumber);
    //    if (checkPhone != null)
    //    {
    //      throw new BadRequestException(PHONENUMBEREXISTCUSTOMER);
    //    }
    //    model.CreateBy = "Admin";
    //    model.ModifiedBy = "Admin";
    //    _unitOfWork.Employee.Add(model);
    //    _unitOfWork.Save();
    //    strMessage = CREATECUSTOMERSUCCESS;
    //    return model;
    //  }
    //  catch (Exception)
    //  {
    //    throw;
    //  }
    //}
    //    @startuml
    //(*) --> "Check if email exists"

    //if "Email exists?" then
    //  -->[yes] "Throw BadRequestException: EMAILEXIST"
    //  --> (*)
    //else
    //  -->[no] "Check if phone number exists"
    //if "Phone number exists?" then
    //  -->[yes] "Throw BadRequestException: PHONENUMBEREXISTCUSTOMER"
    //  --> (*)
    //else
    //  -->[no] "Set create and modified by"
    //  --> "Add employee to database"
    //  --> "Save changes"
    //  --> "Set success message"
    //  --> "Return created employee"
    //  --> (*)
    //endif
    //@enduml

    //public Employee Update(Employee model, IFormFile? file, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    // lấy thông tin Nhân viên
    //    var data = _unitOfWork.Employee.Get(u => u.Id == model.Id);
    //    if (data == null)
    //    {
    //      throw new BadRequestException(EMPLOYEENOTFOUND);
    //    }
    //    // kiểm tra email Nhân viên đã tồn tại chưa
    //    var checkEmail = _unitOfWork.User.Get(u => u.Email == model.UserId);
    //    if (checkEmail != null && checkEmail.Email != model.UserId)
    //    {
    //      throw new BadRequestException(EMAILEXIST);
    //    }
    //    // kiểm tra số điện thoại Nhân viên đã tồn tại chưa
    //    var checkPhone = _unitOfWork.Employee.Get(u => u.PhoneNumber == model.PhoneNumber);
    //    if (checkPhone != null && checkPhone.Id != model.Id)
    //    {
    //      throw new BadRequestException(PHONENUMBEREXISTCUSTOMER);
    //    }
    //    // nếu có file thì thực hiện lưu file mới và xóa file cũ đi
    //    // lấy đường dẫn ảnh file cũ
    //    if (file != null)
    //    {
    //      var user = _unitOfWork.User.Get(x => x.Email == data.UserId);
    //      string oldFile = user.Avatar;
    //      user.Avatar = ImageHelper.AddImage(_webHostEnvironment.WebRootPath, user.Email, file, AppSettings.PatchUser);
    //      user.ModifiedBy = "Admin";
    //      user.ModifiedDate = DateTime.Now;
    //      _unitOfWork.User.Update(user);
    //      // xóa file cũ
    //      if (oldFile != AppSettings.AvatarDefault)
    //      {
    //        ImageHelper.DeleteImage(_webHostEnvironment.WebRootPath, oldFile);
    //      }
    //    }
    //    model.ModifiedBy = "Admin";
    //    _unitOfWork.Employee.Update(model);
    //    _unitOfWork.Save();
    //    strMessage = UPDATEEMPLOYEESUCCESS;
    //    return model;
    //  }
    //  catch (Exception)
    //  {
    //    throw;
    //  }
    //}

    //    @startuml
    //(*) --> "Get employee information"

    //if "Employee exists?" then
    //  -->[yes] "Check if email exists"
    //  if "Email exists?" then
    //    --> [yes] "Throw BadRequestException: EMAILEXIST"
    //    --> (*)
    //  else
    //    --> [no] "Check if phone number exists"
    //    if "Phone number exists?" then
    //      --> [yes] "Throw BadRequestException: PHONENUMBEREXISTCUSTOMER"
    //      --> (*)
    //    else
    //      --> [no] "Check if file is null"
    //      if "File exists?" then
    //        -->[yes] "Update user avatar"
    //        --> "Delete old avatar"
    //        --> "Update employee"

    //      else
    //        -->[no] "updating employee"
    //      endif
    //      --> "Update employee"
    //      --> "Save changes"
    //      --> (*)
    //    endif
    //  endif
    //else
    //  -->[no] "Throw BadRequestException: EMPLOYEENOTFOUND"
    //  --> (*)
    //endif
    //@enduml


    //public bool Delete(int id, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var data = _unitOfWork.Employee.Get(u => u.Id == id && !u.isDeleted);
    //    if (data == null)
    //    {
    //      throw new BadRequestException(EMPLOYEENOTFOUND);
    //    }

    //    data.isDeleted = true;
    //    data.ModifiedDate = DateTime.Now;
    //    _unitOfWork.Employee.Update(data);
    //    _unitOfWork.Save();
    //    strMessage = DELETEEMPLOYEESUCCESS;
    //    return true;
    //  }
    //  catch (Exception)
    //  {
    //    throw;
    //  }
    //}

    //    @startuml
    //(*) --> "Get employee by ID and check if not deleted"
    //if "Employee exists?" then
    //  -->[yes] "Mark employee as deleted"
    //  --> "Update employee's modified date"
    //  --> "Update employee in database"
    //  --> "Save changes"
    //  --> "Set success message: DELETEEMPLOYEESUCCESS"
    //  --> "Return true"
    //  --> (*)
    //else
    //  -->[no] "Throw BadRequestException: EMPLOYEENOTFOUND"
    //  --> (*)
    //endif
    //@enduml


    //    public InventoryReceipt Create(InventoryReceiptDTO data, out string strMessage)
    //    {
    //      strMessage = string.Empty;
    //      try
    //      {
    //        // lấy ra employeeId theo userId 
    //        var employee = _unitOfWork.Employee.Get(u => u.UserId == data.EmployeeId);
    //        InventoryReceipt model = new InventoryReceipt
    //        {
    //          SupplierId = data.SupplierId,
    //          EmployeeId = employee.Id,
    //          Notes = data.Notes,
    //        };
    //        _unitOfWork.InventoryReceipt.Add(model);
    //        _unitOfWork.Save();
    //        // lấy ra danh sách productId trong product
    //        var product = _unitOfWork.Product.GetAll();
    //        foreach (var item in data.InventoryReceiptDetails)
    //        {
    //          var productItem = product.FirstOrDefault(u => u.Id == item.ProductId);
    //          if (productItem == null)
    //          {
    //            throw new BadRequestException(PRODUCTNOTFOUNDINVENTORY);
    //          }
    //          item.InventoryReceiptId = model.Id;
    //          item.BatchNumber = item.BatchNumber;
    //          item.ProductId = item.ProductId;
    //          item.Total = item.Total;
    //          item.Price = item.Price;
    //          _unitOfWork.InventoryReceiptDetail.Add(item);
    //        }
    //        _unitOfWork.Save();
    //        strMessage = CREATEINVENTORYSUCCESS;
    //        return model;
    //      }
    //      catch (Exception ex)
    //      {
    //        throw;
    //      }
    //    }
    //    @startuml
    //(*) --> "Get employee by user ID"

    //if "Employee found?" then
    //  -->[yes] "Create InventoryReceipt model"
    //  --> "Add InventoryReceipt model to database"
    //  --> "Get list of all products"

    //  --> "For each InventoryReceiptDetail in data"
    //  --> "Check if product exists"
    //  if "Product exists?" then
    //    --> [yes] "Set InventoryReceiptDetail properties"
    //    --> "Add InventoryReceiptDetail to database"
    //    --> "Save changes"
    //    --> "Set success message: CREATEINVENTORYSUCCESS"
    //    --> "Return InventoryReceipt model"
    //    --> (*)
    //  else
    //    --> [no] "Throw BadRequestException: PRODUCTNOTFOUNDINVENTORY"
    //   --> (*)
    //  endif

    //else
    //  -->[no] "Throw BadRequestException: EMPLOYEENOTFOUND"
    //  --> (*)
    //endif
    //@enduml



    //public InventoryReceipt Update(InventoryReceiptDTO data, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    _unitOfWork.BeginTransaction();
    //    var inventory = _unitOfWork.InventoryReceipt.Get(u => u.Id == data.Id);
    //    if (inventory == null)
    //    {
    //      throw new BadRequestException(INVENTORYNOTFOUND);
    //    }
    //    var user = _unitOfWork.Employee.Get(u => u.UserId == data.EmployeeId);
    //    inventory.SupplierId = data.SupplierId;
    //    inventory.EmployeeId = user.Id;
    //    inventory.isActive = true;
    //    inventory.Notes = data.Notes;
    //    _unitOfWork.InventoryReceipt.Update(inventory);
    //    // lấy ra danh sách productId trong product
    //    //_unitOfWork.InventoryReceiptDetail.RemoveRange(_unitOfWork.InventoryReceiptDetail.GetAll(u => u.InventoryReceiptId == data.Id));
    //    var product = _unitOfWork.Product.GetAll();
    //    foreach (var item in data.InventoryReceiptDetails)
    //    {
    //      var productItem = product.FirstOrDefault(u => u.Id == item.ProductId);
    //      if (productItem == null)
    //      {
    //        throw new BadRequestException(PRODUCTNOTFOUNDINVENTORY);
    //      }
    //      item.InventoryReceiptId = data.Id;
    //      item.BatchNumber = item.BatchNumber;
    //      item.ProductId = item.ProductId;
    //      item.Total = item.Total;
    //      item.Price = item.Price;
    //      _unitOfWork.InventoryReceiptDetail.Add(item);
    //      productItem.Quantity += item.Total;
    //      _unitOfWork.Product.Update(productItem);
    //    }

    //    _unitOfWork.Save();
    //    _unitOfWork.Commit();
    //    strMessage = UPDATEINVENTORYSUCCESS;
    //    return inventory;
    //  }
    //  catch (Exception)
    //  {
    //    _unitOfWork.Rollback();
    //    throw;
    //  }
    //}
    //    @startuml
    //(*) --> "BeginTransaction"
    //    --> "Get InventoryReceipt by ID"

    //if "InventoryReceipt found?" then
    //  -->[yes] "Get employee by user ID"
    //  --> "Update InventoryReceipt properties"
    //  --> "Update InventoryReceipt in database"
    //  --> "Remove existing InventoryReceiptDetails for InventoryReceipt"
    //  --> "Get list of all products"

    //  --> "For each InventoryReceiptDetail in data"
    //  --> "Check if product exists"
    //  if "Product exists?" then
    //    --> [yes] "Set InventoryReceiptDetail properties"
    //    --> "Add InventoryReceiptDetail to database"
    //    --> "Update product quantity"
    //      --> "Save changes"
    //  --> "Set success message: UPDATEINVENTORYSUCCESS"
    //  --> "CommitTran"
    //  --> "Return updated InventoryReceipt"
    // -->(*)
    //  else
    //    --> [no] "Throw BadRequestException: PRODUCTNOTFOUNDINVENTORY"
    // --> "RollBack"
    //  endif
    //else
    //  -->[no] "Throw BadRequestException: INVENTORYNOTFOUND"
    //  --> "RollBack"
    //  --> (*)
    //endif
    //@enduml


    //public Helpers.PagedResult<InventoryReceipt> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var query = _unitOfWork.InventoryReceipt.GetAll(includeProperties: "Supplier,Employee").AsQueryable();

    //    var trimmedName = name.Trim();

    //    // Kiểm tra xem chuỗi tên có phải là ngày không
    //    if (DateTime.TryParseExact(trimmedName, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
    //    {
    //      // Nếu là ngày, thực hiện tìm kiếm theo ngày tạo
    //      query = query.Where(e => e.CreateDate.Date == DateTime.ParseExact(trimmedName, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date);
    //    }
    //    else
    //    {
    //      // Nếu không phải là ngày, thực hiện tìm kiếm theo tên của nhà cung cấp
    //      query = query.Where(e => e.Supplier.SupplierName.Contains(trimmedName));
    //    }
    //    //var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
    //    var orderBy = sortBy switch
    //    {
    //      "FullName" => $"Employee.{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}",
    //      "SupplierName" => $"Supplier.{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}",
    //      _ => $"Id {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}"
    //    };

    //    query = query.OrderBy(orderBy);
    //    var pagedResult = Helpers.PagedResult<InventoryReceipt>.CreatePagedResult(query, pageNumber, pageSize);
    //    return pagedResult;
    //  }
    //  catch (Exception)
    //  {
    //    throw;
    //  }
    //}

    //    @startuml
    //(*) --> "Get all inventory receipts from database"

    //if (Is name a valid date?) then
    //  -->[yes] "Search by create date"
    //  --> "Apply search filter"
    //else
    //  -->[no] "Search by supplier name"
    //  --> "Apply search filter"
    //endif

    //--> "Sort results by specified criteria"
    //--> "Create paged result"
    //--> (*)
    //@enduml



    //    public Helpers.PagedResult<ProductDTO> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
    //    {
    //      strMessage = string.Empty;
    //      try
    //      {
    //        var query = _unitOfWork.Product.GetAll(u => u.isDeleted == false, "Category,Trademark").AsQueryable();

    //        if (!string.IsNullOrEmpty(name))
    //        {
    //          query = query.Where(e => e.ProductName.Contains(name.Trim()));
    //        }

    //        var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
    //        query = query.OrderBy(orderBy);
    //        var promotion = _promotionService.CheckPromotionActive();
    //        foreach (var item in query)
    //        {
    //          //item.Discount = _unitOfWork.PromotionDetail.Get(u => u.ProductId == item.Id, "Promotion")?.Promotion?.Discount ?? 0;
    //          item.Discount = promotion.Where(u => u.ProductId == item.Id).FirstOrDefault()?.Promotion?.Discount ?? 0;
    //          item.Image += _unitOfWork.ProductImages.GetAll(u => u.ProductId == item.Id).FirstOrDefault()?.ImagePath;
    //        }

    //        var pagedResult = Helpers.PagedResult<ProductDTO>.CreatePagedResult(query.Select(p => new ProductDTO
    //        {
    //          Product = p,
    //          ProductImages = _unitOfWork.ProductImages.GetAll(u => u.ProductId == p.Id, null),
    //          CategoryProduct = p.Category,
    //        }), pageNumber, pageSize);
    //        return pagedResult;
    //      }
    //      catch (Exception)
    //      {
    //        throw;
    //      }
    //    }
    //    @startuml
    //(*) --> "Retrieve all products from the database"

    //if (Is name provided?) then
    //  -->[yes] "Filter products by name"
    //  --> "Apply name filter"
    //  --> "Sort products by specified criteria"
    //else
    //  -->[no] "No name filter"
    //  --> "Continue without name filter"
    //endif

    //--> "Sort products by specified criteria"
    //--> "Check active promotions"
    //--> "Apply discounts and images"
    //--> "Create paged result"
    //--> (*)
    //@enduml


    //    public ProductDTO FindById(int id, out string strMessage)
    //    {
    //      strMessage = string.Empty;
    //      try
    //      {
    //        ProductDTO productDTO = new ProductDTO
    //        {
    //          Product = _unitOfWork.Product.Get(u => u.Id == id && u.isDeleted == false),
    //          ProductImages = _unitOfWork.ProductImages.GetAll(u => u.ProductId == id).ToList(),
    //          CategoryProduct = _unitOfWork.Category.Get(u => u.Id == id)
    //        };
    //        if (productDTO == null)
    //        {
    //          throw new BadRequestException(PRODUCTNOTFOUND);
    //        }
    //        else
    //        {
    //          return productDTO;
    //        }
    //      }
    //      catch (Exception)
    //      {
    //        throw;
    //      }
    //    }
    //    @startuml
    //(*) --> "Retrieve product by ID from the database"

    //if (Product found?) then
    //  -->[yes] "Retrieve product images"
    //  --> "Retrieve category of the product"
    //  --> "Create ProductDTO object"
    //  --> (*)
    //else
    //  -->[no] "Throw BadRequestException: PRODUCTNOTFOUND"
    //  --> (*)
    //endif
    //@enduml




    //public Product Create(Product product, List<IFormFile>? files, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  _unitOfWork.BeginTransaction();

    //  try
    //  {
    //    if (product.Id == 0)
    //    {
    //      var checkProduct = _unitOfWork.Product.Get(x => x.ProductName == product.ProductName);
    //      if (checkProduct != null)
    //      {
    //        throw new BadRequestException(PRODUCTEXIST);
    //      }
    //      product.CreateBy = "Admin";
    //      product.ModifiedBy = "Admin";

    //      _unitOfWork.Product.Add(product);
    //      _unitOfWork.Save();

    //      if (files != null && files.Count > 0)
    //      {
    //        foreach (var file in files)
    //        {
    //          if (!ImageHelper.CheckImage(file))
    //          {
    //            _unitOfWork.Rollback();
    //            throw new BadRequestException(FILEFORMAT);
    //          }
    //          var image = new ProductImages();
    //          image.ProductId = product.Id;
    //          image.ImageName = file.FileName;
    //          image.ImagePath = ImageHelper.AddImage(_webHostEnvironment.WebRootPath, product.Id.ToString(), file, AppSettings.PatchProduct);
    //          image.IsDefault = false;
    //          image.IsActive = true;
    //          image.IsDeleted = false;
    //          image.CreateBy = "Admin";
    //          image.ModifiedBy = "Admin";
    //          _unitOfWork.ProductImages.Add(image);
    //        }
    //      }
    //      strMessage = ADDPRODUCTSUCCESS;
    //    }
    //    else
    //    {
    //      _unitOfWork.Product.Update(product);
    //      _unitOfWork.Save();
    //      if (files.Count > 0)
    //      {
    //        var productImages = _unitOfWork.ProductImages.GetAll(x => x.ProductId == product.Id);

    //        foreach (var item in productImages)
    //        {
    //          ImageHelper.DeleteImage(_webHostEnvironment.WebRootPath, item.ImagePath);
    //          _unitOfWork.ProductImages.Remove(item);
    //        }
    //      }
    //      if (files != null)
    //      {
    //        foreach (var file in files)
    //        {
    //          if (!ImageHelper.CheckImage(file))
    //          {
    //            throw new BadRequestException(FILEFORMAT);
    //          }
    //          var image = new ProductImages();
    //          image.ProductId = product.Id;
    //          image.ImageName = file.FileName;
    //          image.ImagePath = ImageHelper.AddImage(_webHostEnvironment.WebRootPath, product.Id.ToString(), file, AppSettings.PatchProduct);
    //          image.IsDefault = false;
    //          image.IsActive = true;
    //          image.IsDeleted = false;
    //          image.CreateBy = "Admin";
    //          image.ModifiedBy = "Admin";
    //          _unitOfWork.ProductImages.Add(image);
    //        }
    //      }
    //      strMessage = UPDATEPRODUCTSUCCESS;
    //    }
    //    _unitOfWork.Save();
    //    _unitOfWork.Commit();
    //    return product;
    //  }
    //  catch (Exception)
    //  {
    //    throw;
    //  }
    //}
    //    @startuml
    //(*) --> "Begin Transaction"

    //if (Check if product exists) then
    //  -->[Yes] "Throw BadRequestException: PRODUCTEXIST"
    //  --> "Rollback Transaction"
    //else
    //  -->[No] "Set CreateBy and ModifiedBy"
    //  --> "Add product to database"
    //  --> "Save changes"

    //  if (Files exist and not empty?) then
    //    -->[Yes] "Loop through files"
    //    --> "Check image format"

    //    if (Image format valid?) then
    //      -->[Yes] "Add image to database"
    //      --> "Set success message"
    //    else
    //      -->[No] "Throw BadRequestException: FILEFORMAT"
    //      --> "Rollback Transaction"
    //      --> (*)
    //    endif
    //  else
    //    -->[No] "Continue without adding images"
    //  endif

    //  --> "Set success message"
    //  --> "Save changes"
    //  --> "Commit Transaction"
    //  --> "Return product"
    //  --> (*)
    //endif
    //@enduml



    //public bool Delete(int id, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var data = _unitOfWork.Product.Get(u => u.Id == id && !u.isDeleted);
    //    if (data == null)
    //    {
    //      throw new BadRequestException(PRODUCTNOTFOUND);
    //    }

    //    data.isDeleted = true;
    //    data.ModifiedDate = DateTime.Now;
    //    _unitOfWork.Product.Update(data);
    //    _unitOfWork.Save();
    //    strMessage = DELETEPRODUCTSUCCESS;
    //    return true;
    //  }
    //  catch (Exception ex)
    //  {
    //    throw;
    //  }
    //}

    //    @startuml
    //(*) --> "Get product by ID"

    //if (Product found?) then
    //  -->[Yes] "Mark product as deleted"
    //  --> "Save changes"
    //  --> "Set success message"
    //  --> (*)
    //else
    //  -->[No] "Throw BadRequestException: PRODUCTNOTFOUND"
    //  --> (*)
    //endif
    //@enduml




    //public bool DeleteProductImage(int productId, string imageName, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var productImage = _unitOfWork.ProductImages.Get(x => x.ProductId == productId && x.ImageName == imageName);
    //    if (productImage == null)
    //    {
    //      throw new BadRequestException(IMAGEPRODUCTNOTFOUND);
    //    }

    //    if (!ImageHelper.DeleteImage(_webHostEnvironment.WebRootPath, productImage.ImagePath))
    //    {
    //      throw new BadRequestException(DELETEIMAGESUCCESS);
    //    }

    //    _unitOfWork.ProductImages.Remove(productImage);
    //    _unitOfWork.Save();
    //    strMessage = DELETEIMAGESUCCESS;
    //    return true;
    //  }
    //  catch (Exception)
    //  {
    //    throw;
    //  }
    //}
    //    @startuml
    //(*) --> "Get product image by product ID and image name"

    //if (Product image found?) then
    //  -->[Yes] "Delete image file from storage"
    //  --> "Remove product image from database"
    //  --> "Save changes"
    //  --> "Set success message"
    //  --> (*)
    //else
    //  -->[No] "Throw BadRequestException: IMAGEPRODUCTNOTFOUND"
    //  --> (*)
    //endif
    //@enduml

    //trademark
    //    public Helpers.PagedResult<Trademark> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
    //    {
    //      strMessage = string.Empty;
    //      try
    //      {
    //        var query = _db.Trademarks.OrderByDescending(u => u.ModifiedDate).AsQueryable();
    //        if (!string.IsNullOrEmpty(name))
    //        {
    //          query = query.Where(e => e.TrademarkName.Contains(name.Trim()));
    //        }
    //        var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
    //        query = query.OrderBy(orderBy);
    //        var pagedResult = Helpers.PagedResult<Trademark>.CreatePagedResult(query, pageNumber, pageSize);
    //        return pagedResult;
    //      }
    //      catch (Exception)
    //      {
    //        throw;
    //      }
    //    }

    //    @startuml
    //(*) --> "Retrieve all trademarks from the database"

    //    if (Trademarks retrieved successfully?) then
    //      -->[Yes] "Filter trademarks by name if provided"
    //      --> "Sort trademarks based on sorting criteria"
    //      --> "Create paged result based on pagination parameters"
    //      --> "Return paged result"
    //      --> (*)
    //    else
    //      -->[No] "Throw exception"
    //      --> (*)
    //    endif
    //    @enduml

    //        public Trademark FindById(int id, out string strMessage)
    //    {
    //      strMessage = string.Empty;
    //      try
    //      {
    //        var data = _unitOfWork.Trademark.Get(u => u.Id == id);
    //        if (data == null)
    //        {
    //          throw new BadRequestException(TRADEMARKNOTFOUND);
    //        }
    //        return data;
    //      }
    //      catch (Exception)
    //      {
    //        throw;
    //      }
    //    }

    //    @startuml
    //(*) --> "Retrieve trademark by ID from the database"

    //    if (Trademark found?) then
    //      -->[Yes] "Return the trademark"
    //      --> (*)
    //    else
    //      -->[No] "Throw exception"
    //      --> (*)
    //    endif
    //    @enduml

    //    public Trademark Create(Trademark trademark, out string strMessage)
    //    {
    //      strMessage = string.Empty;
    //      try
    //      {

    //        var checkPhone = _unitOfWork.Trademark.Get(u => u.TrademarkName == trademark.TrademarkName.Trim());
    //        if (checkPhone != null)
    //        {
    //          throw new BadRequestException(TRADEMARKEXIST);
    //        }
    //        trademark.CreateBy = "Admin";
    //        trademark.ModifiedBy = "Admin";
    //        _unitOfWork.Trademark.Add(trademark);
    //        _unitOfWork.Save();
    //        strMessage = ADDTRADEMARKSUCCESS;
    //        return trademark;
    //      }
    //      catch (Exception)
    //      {
    //        throw;
    //      }
    //    }

    //    @startuml
    //(*) --> "Check if trademark already exists"

    //if (Trademark exists?) then
    //  -->[Yes] "Throw exception: TRADEMARKEXIST"
    //  --> (*)
    //else
    //  -->[No] "Create new trademark"
    //  --> "Save changes to database"
    //  --> "Return created trademark"
    //  --> (*)
    //endif
    //@enduml
    //    public Trademark Update(Trademark trademark, out string strMessage)
    //    {
    //      strMessage = string.Empty;
    //      try
    //      {
    //        // lấy thông tin thương hiệu
    //        var data = _unitOfWork.Trademark.Get(u => u.Id == trademark.Id);
    //        if (data == null)
    //        {
    //          throw new BadRequestException(TRADEMARKNOTFOUND);
    //        }
    //        // kiểm tra số điện thoại thương hiệu đã tồn tại chưa
    //        var trademarkName = _unitOfWork.Trademark.Get(u => u.TrademarkName == trademark.TrademarkName.Trim());
    //        if (trademarkName != null && trademarkName.Id != trademark.Id)
    //        {
    //          throw new BadRequestException(TRADEMARKEXIST);
    //        }
    //        // kiêm tra mã số thueé
    //        trademark.ModifiedBy = "Admin";
    //        _unitOfWork.Trademark.Update(trademark);
    //        _unitOfWork.Save();
    //        strMessage = UPDATETRADEMARKSUCCESS;
    //        return trademark;
    //      }
    //      catch (Exception)
    //      {
    //        throw;
    //      }
    //    }

    //    @startuml
    //(*) --> "Get trademark information by ID"

    //if (Trademark exists?) then
    //  -->[Yes] "Check if updated trademark name already exists"
    //  if (Trademark name exists for another trademark?) then
    //    -->[Yes] "Throw exception: TRADEMARKEXIST"
    //    --> (*)
    //  else
    //    -->[No] "Update trademark"
    //    --> "Save changes to database"
    //    --> "Return updated trademark"
    //    --> (*)
    //  endif
    //else
    //  -->[No] "Throw exception: TRADEMARKNOTFOUND"
    //  --> (*)
    //endif
    //@enduml


    //public bool Delete(int id, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var data = _unitOfWork.Trademark.Get(u => u.Id == id);
    //    if (data == null)
    //    {
    //      throw new BadRequestException(TRADEMARKNOTFOUND);
    //    }
    //    _unitOfWork.Trademark.Remove(data);
    //    _unitOfWork.Save();
    //    strMessage = DELETETRADEMARKSUCCESS;
    //    return true;
    //  }
    //  catch (Exception)
    //  {
    //    throw;
    //  }
    //}
    //    @startuml
    //(*) --> "Get trademark information by ID"

    //if (Trademark exists?) then
    //  -->[Yes] "Delete trademark"
    //  --> "Save changes to database"
    //  --> "Return success message"
    //  --> (*)
    //else
    //  -->[No] "Throw exception: TRADEMARKNOTFOUND"
    //  --> (*)
    //endif
    //@enduml



    //public Helpers.PagedResult<TrxTransaction> GetAll(string name, string orderStatus, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    var query = _db.TrxTransactions.OrderByDescending(u => u.OrderDate).AsQueryable();
    //    if (!string.IsNullOrEmpty(name))
    //    {
    //      query = query.Where(e => e.FullName.Contains(name.Trim()) || e.PhoneNumber.Contains(name.Trim()));
    //    }
    //    if (!string.IsNullOrEmpty(orderStatus))
    //    {
    //      query = query.Where(e => e.OrderStatus.Contains(orderStatus));
    //    }
    //    var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
    //    query = query.OrderBy(orderBy);
    //    var pagedResult = Helpers.PagedResult<TrxTransaction>.CreatePagedResult(query, pageNumber, pageSize);
    //    return pagedResult;
    //  }
    //  catch (Exception ex)
    //  {
    //    throw;
    //  }
    //}
    //    @startuml
    //(*) --> "Retrieve all trademarks from the database"

    //    if (Trademarks retrieved successfully?) then
    //      -->[Yes] "Filter trxtransaction by name if provided"
    //      --> "Sort trxtransaction based on sorting criteria"
    //      --> "Create paged result based on pagination parameters"
    //      --> "Return paged result"
    //      --> (*)
    //    else
    //      -->[No] "Throw exception"
    //      --> (*)
    //    endif
    //@enduml


    //public TrxTransactionDTO Insert(TrxTransactionDTO trxTransactionDTO, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    _unitOfWork.BeginTransaction();
    //    TrxTransaction trxTransaction = new TrxTransaction();
    //    int checkPromotion = trxTransactionDTO.Carts.Where(x => x.Discount > 0).Count();

    //    // kiểm tra thông tin product trong carts để kiểm tra còn trong chương trình khuyến mãi không
    //    var promotion = _promotionService.CheckPromotionActive().Select(x => x.ProductId);

    //    var cartProduct = trxTransactionDTO.Carts.Where(x => x.Discount > 0 && promotion.Contains(x.ProductId)).ToList();
    //    if (cartProduct.Count == 0 && checkPromotion > 0)
    //    {
    //      throw new BadRequestException(TRXTRANSACTIONPROMTION);
    //    }
    //    trxTransaction.CustomerId = GetCustomerId(trxTransactionDTO.CustomerId);

    //    trxTransaction.FullName = trxTransactionDTO?.FullName;
    //    trxTransaction.PhoneNumber = trxTransactionDTO?.PhoneNumber;
    //    trxTransaction.Address = trxTransactionDTO?.Address;
    //    trxTransaction.OrderDate = DateTime.Now;
    //    trxTransaction.OrderStatus = trxTransactionDTO.OrderStatus ?? "PENDING";
    //    trxTransaction.PaymentTypes = trxTransactionDTO.PaymentTypes;

    //    //nếu PaymentTypes = CASH thì trạng thái thanh toán là đã thanh toán
    //    if (trxTransaction.PaymentTypes == AppSettings.Cash)
    //    {
    //      trxTransaction.PaymentStatus = AppSettings.PaymentStatusApproved;
    //    }
    //    else
    //    {
    //      trxTransaction.PaymentStatus = AppSettings.PaymentStatusPending;
    //    }
    //    trxTransaction.PaymentStatus = AppSettings.PaymentStatusPending;
    //    trxTransaction.OrderTotal = trxTransactionDTO.Amount ?? 0;
    //    _unitOfWork.TrxTransaction.Add(trxTransaction);
    //    _unitOfWork.Save();

    //    //trxTransactionDTO.TrxTransactionId = trxTransaction.Id;
    //    trxTransactionDTO.Amount = trxTransaction.OrderTotal;
    //    // lấy thông tin đơn hàng theo userid từ cart
    //    foreach (var item in trxTransactionDTO.Carts)
    //    {
    //      TransactionDetail trxTransactionDetail = new TransactionDetail();
    //      trxTransactionDetail.ProductId = item.ProductId;
    //      trxTransactionDetail.Total = item.Quantity;
    //      trxTransactionDetail.Price = item.Price;
    //      trxTransactionDetail.TrxTransactionId = trxTransaction.Id;
    //      _unitOfWork.TransactionDetail.Add(trxTransactionDetail);
    //    }

    //    if (checkPromotion > 0)
    //    {
    //      UpdatePromotionDetail(cartProduct);
    //    }

    //    var listCartId = trxTransactionDTO.Carts.Select(x => x.Id).ToList();
    //    _unitOfWork.Cart.RemoveRange(_unitOfWork.Cart.GetAll(u => listCartId.Contains(u.Id)));
    //    UpdateProductQuantity(trxTransactionDTO.Carts);
    //    _unitOfWork.Save();
    //    _unitOfWork.Commit();
    //    return trxTransactionDTO;
    //  }
    //  catch (Exception)
    //  {
    //    _unitOfWork.Rollback();
    //    throw;
    //  }
    //}
    //    @startuml
    //(*) --> "Begin Transaction"

    //if (Promotion Active?) then
    //  -->[Yes] "Check if any cart item has discount"
    //  if (Any cart item has discount?) then
    //    -->[Yes] "Retrieve active promotion products"
    //    --> "Filter cart items with active promotion"
    //    -->[No] "Throw BadRequestException (TRXTRANSACTIONPROMTION)"
    //    --> "RollBack"
    //    -->(*)

    //  else
    //    -->[No] "Continue"
    //  endif
    //else
    //  -->[No] "Continue"
    //endif

    //--> "Retrieve customer ID"

    //if (Payment Type is Cash?) then
    //  -->[Yes] "Set payment status to Approved"
    //  --> "Save transaction"
    //else
    //  -->[No] "Set payment status to Pending"
    //endif

    //--> "Save transaction"
    //--> "Create TransactionDetail"

    //if (Any cart item has discount?) then
    //  -->[Yes] "Update promotion details"
    //  --> "Remove cart items"
    //else
    //  -->[No] "Remove cart items"
    //endif

    //--> "Update product quantity"
    // --> "Save transaction"
    //--> "Commit Transaction"
    //--> "Return transaction details"
    //--> (*)
    //@enduml




    //public ShoppingCartVM Update(UpdateTrxTransaction model, out string strMessage)
    //{
    //  strMessage = string.Empty;
    //  try
    //  {
    //    _unitOfWork.BeginTransaction();
    //    var data = _unitOfWork.TrxTransaction.Get(u => u.Id == model.TransactionId);
    //    if (data == null)
    //    {
    //      throw new BadRequestException(TRXTRANSACTIONNOTFOUNDORDER);
    //    }

    //    switch (model.OrderStatus)
    //    {
    //      case "PENDING":
    //        data.OrderDate = DateTime.Now;
    //        break;
    //      case "DELIVERED":
    //        data.OnholDate = DateTime.Now;
    //        data.DeliveredDate = DateTime.Now;
    //        break;
    //      case "WAITINGFORPICKUP":
    //      case "WAITINGFORDELIVERY":
    //        data.WaitingForPickupDate = DateTime.Now;
    //        data.WaitingForDeliveryDate = DateTime.Now;
    //        break;
    //      case "CANCELLED":
    //        data.CancelledDate = DateTime.Now;
    //        break;
    //    }
    //    if (data.OrderStatus == AppSettings.StatusOrderDelivered && model.OrderStatus != AppSettings.StatusOrderDelivered)
    //    {
    //      throw new BadRequestException(TRXTRANSACTIONDELIVERED);
    //    }

    //    data.OrderStatus = model.OrderStatus;
    //    _unitOfWork.TrxTransaction.Update(data);
    //    _unitOfWork.Save();
    //    _unitOfWork.Commit();
    //    ShoppingCartVM cartVM = new ShoppingCartVM();
    //    cartVM.TrxTransaction = data;
    //    strMessage = UPDATETRXTRANSACTIONSUCCESS;
    //    return cartVM;
    //  }
    //  catch (Exception)
    //  {
    //    throw;
    //  }
    //}

    //    @startuml
    //(*) --> "Begin Transaction"

    //--> "Retrieve transaction by ID"
    //if (Transaction found?) then
    //  -->[Yes] "Update transaction status and dates"
    //  if (New status is 'DELIVERED' and current status is not 'DELIVERED') then
    //    -->[Yes] "Throw BadRequestException (TRXTRANSACTIONDELIVERED)"
    //    --> (*)
    //  else
    //    -->[No] "Update Order Status"
    //  endif
    //  --> "Save updated transaction"
    //  --> "Commit Transaction"
    //  --> "Create ShoppingCartVM object"
    //  --> "Return ShoppingCartVM object"
    //else
    //  -->[No] "Throw BadRequestException (TRXTRANSACTIONNOTFOUNDORDER)"
    //  --> (*)

    //endif
    //--> (*)
    //@enduml


    //    public ShoppingCartVM GetTrxTransactionFindById(int id, out string strMessage)
    //    {
    //      strMessage = string.Empty;
    //      try
    //      {
    //        ShoppingCartVM cartVM = new ShoppingCartVM();
    //        cartVM.TrxTransaction = _unitOfWork.TrxTransaction.Get(u => u.Id == id);
    //        if (cartVM.TrxTransaction == null)
    //        {
    //          throw new BadRequestException(TRXTRANSACTIONNOTFOUNDORDEROUT);
    //        }

    //        cartVM.Customer = _unitOfWork.Customer.Get(u => u.Id == cartVM.TrxTransaction.CustomerId);
    //        cartVM.TransactionDetail = _unitOfWork.TransactionDetail.GetAll(u => u.TrxTransactionId == id, "Product");
    //        foreach (var item in cartVM.TransactionDetail)
    //        {
    //          item.ProductImage = _unitOfWork.ProductImages.Get(u => u.ProductId == item.ProductId).ImagePath;
    //        }

    //        return cartVM;
    //      }
    //      catch (Exception ex)
    //      {
    //        throw;
    //      }
    //    }
    //    @startuml
    //(*) --> "Create ShoppingCartVM object"

    //--> "Retrieve transaction by ID"
    //if (Transaction found?) then
    //  -->[Yes] "Retrieve customer information"
    //  --> "Retrieve transaction details"
    //  --> "Retrieve product images for transaction details"
    //  --> "Return ShoppingCartVM object"
    //  --> (*)
    //else
    //  -->[No] "Throw BadRequestException (TRXTRANSACTIONNOTFOUNDORDEROUT)"
    //endif
    //--> (*)
    //@enduml



    //    public CustomerTransactionDTO GetCustomerTransaction(string userid, out string strMessage)
    //    {
    //      strMessage = string.Empty;
    //      try
    //      {
    //        CustomerTransactionDTO customerTransactionDTO = new CustomerTransactionDTO();
    //        var customer = _unitOfWork.Customer.Get(u => u.UserId == userid, "User");
    //        if (customer == null)
    //        {
    //          throw new BadRequestException(TRXTRANSACTIONNOTFOUNDUSEROUT);
    //        }
    //        customerTransactionDTO.Customer = customer;
    //        customerTransactionDTO.TrxTransactions = _unitOfWork.TrxTransaction.GetAll(u => u.CustomerId == customer.Id).OrderByDescending(u => u.OrderDate).ToList();
    //        return customerTransactionDTO;
    //      }
    //      catch (Exception ex)
    //      {
    //        throw;
    //      }
    //    }
    //    @startuml
    //(*) --> "Create ShoppingCartVM object"

    //--> "Retrieve transaction by ID"
    //if (Transaction found?) then
    //  -->[Yes] "Retrieve customer information"
    //  --> "Retrieve transaction details"
    //  --> "Retrieve product images for transaction details"
    //  --> "Return ShoppingCartVM object"
    //  --> (*)
    //else
    //  -->[No] "Throw BadRequestException (TRXTRANSACTIONNOTFOUNDORDEROUT)"
    //endif
    //--> (*)
    //@enduml

  }
}
