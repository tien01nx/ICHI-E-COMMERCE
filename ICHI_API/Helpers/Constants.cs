namespace ICHI_API.Helpers
{
  public class Constants
  {
    //strMessage = "Tài khoản không tồn tại";
    //strMessage = "Có lỗi xảy ra";
    //strMessage = "Tài khoản đã bị khóa";
    //        strMessage = "Mật khẩu không đúng";
    //        strMessage = "Đăng nhập thành công";
    //strMessage = "Số điện thoại đã tồn tại";
    //   strMessage = "User đã tồn tại";
    //   strMessage = "Mật khẩu phải có ít nhất 8 kí tự, 1 chữ hoa, 1 chữ thường, 1 kí tự đặc biệt";
    //    strMessage = "Đăng ký tài khoản thành công";
    //            strMessage = "Tài khoản không tồn tại";

    //          strMessage = status? "Khóa tài khoản thành công" : "Mở khóa tài khoản thành công";

    //Jwt:Key
    //Jwt:ExpireDay
    //  Jwt:Issuer

    public static string JWT = "Jwt";
    public static string JWTKEY = "Jwt:Key";
    public static string JWTEXPIREDAY = "Jwt:ExpireDay";
    public static string JWTISSUER = "Jwt:Issuer";
    public static string JWTAUDIENCE = "Jwt:Audience";
    public static string SUB = "sub";
    public static string ROLE = "Role";
    public static string ROLES = "roles";

    public static string PASSWORDSUCCESS = "Đổi mật khẩu thành công";
    public static string ERROR = "Có lỗi xảy ra";
    public static string PASSWORDOLDPFAILOLD = "Mật khẩu cũ không đúng";
    public static string PASSWORDOLDPFAIL = "Mật khẩu cũ không đúng";
    public static string ACCOUNTNOTFOUNF = "Tài khoản không tồn tại";
    public static string ACCOUNTLOCK = "Tài khoản đã bị khóa";
    public static string LOGINSUCCESS = "Đăng nhập thành công";
    public static string PHONENUMBEREXIST = "Số điện thoại đã tồn tại";
    public static string USEREXIST = "User đã tồn tại";
    public static string PASSWORDREGEX = "Mật khẩu phải có ít nhất 8 kí tự, 1 chữ hoa, 1 chữ thường, 1 kí tự đặc biệt";
    public static string REGISTERSUCCESS = "Đăng ký tài khoản thành công";
    public static string ACCOUNTNOTFOUND = "Tài khoản không tồn tại";
    public static string ACCOUNTLOCKSUCCESS(bool status) => status ? "Khóa tài khoản thành công" : "Mở khóa tài khoản thành công";






  }
}
