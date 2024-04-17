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


    //strMessage = "Gửi email thành công";
    //strMessage = "Không tìm thấy sản phẩm trong giỏ hàng";
    //    strMessage = "Xóa sản phẩm khỏi giỏ hàng thành công";

    //        strMessage = "Số lượng sản phẩm không đủ";

    //    strMessage = "Thêm vào giỏ hàng thành công";

    //    strMessage = "Có lỗi xảy ra vui lòng thử lại sau!";

    //        strMessage = "Số lượng sản phẩm không đủ/ hết hàng";

    //      strMessage = "Cập nhật giỏ hàng thành công";

    //      strMessage = "Không tìm thấy sản phẩm trong giỏ hàng";
    //strMessage = "Danh mục sản phẩm không tồn tại";
    //            strMessage = "Danh mục sản phẩm đã tồn tại";
    //      strMessage = "Danh mục cha không tồn tại";
    //    strMessage = "Tạo mới danh mục thành công";
    //      strMessage = "Danh mục sản phẩm không tồn tại";
    //      strMessage = "Danh mục sản phẩm đã tồn tại";
    //      strMessage = "Danh mục cha không tồn tại";
    //    strMessage = "Cập nhật danh mục thành công";
    //      strMessage = "Danh mục sản phẩm không tồn tại";
    //    strMessage = "Xóa danh mục sản phẩm thành công";
    //strMessage = "Khách hàng không tồn tại";
    //      strMessage = "Email đã tồn tại";

    //      strMessage = "Số điện thoại đã tồn tại";
    //    strMessage = "Tạo mới thành công";
    //      strMessage = "Email đã tồn tại";
    //      strMessage = "Số điện thoại đã tồn tại";
    //    strMessage = "Cập nhật thành công";
    //      strMessage = "khách hàng không tồn tại";
    //    strMessage = "Xóa thành công";

    //strMessage = "Nhân viên không tồn tại";
    //    strMessage = "Cập nhật nhân viên thành công";
    //    strMessage = "Xóa thành công";



    //strMessage = "Không tìm thấy sản phẩm";
    //    strMessage = "Tạo phiếu nhập thành công";
    //      strMessage = "Không tìm thấy phiếu nhập";
    //        strMessage = "Không tìm thấy sản phẩm";
    //    strMessage = "Cập nhật hóa đơn nhập thành công";
    //      strMessage = "Không tìm thấy phiếu nhập";

    //strMessage = "Sản phẩm không tồn tại";
    //        strMessage = "Sản phẩm đã tồn tại";

    //            strMessage = "File không đúng định dạng hoặc dung lượng lớn hơn 10MB";

    //      strMessage = "Thêm sản phẩm thành công";

    //          strMessage = "File không đúng định dạng hoặc dung lượng lớn hơn 10MB";

    //    strMessage = "Cập nhật sản phẩm thành công";

    //    strMessage = "Cập nhật thành công";

    //      strMessage = "Sản phẩm không tồn tại";

    //    strMessage = "Xóa sản phẩm thành công";

    //      strMessage = "Hình ảnh sản phẩm không tồn tại!";

    //      strMessage = "Xóa ảnh không thành công!";



    //strMessage = "Chương trình khuyến mãi không tồn tại";

    //      strMessage = "Mã chương trình khuyến mãi đã tồn tại";

    //      strMessage = "Ngày bắt đầu phải lớn hơn hoặc bằng ngày hiện tại";

    //      strMessage = "Ngày kết thúc phải lớn hơn hoặc bằng ngày hiện tại";

    //      strMessage = "Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu";
    //        strMessage = $"Sản phẩm với Id: {string.Join(",", existingProductIds)} :Đã tồn tại trong chương trình khuyến mãi";
    //strMessage = "Tạo mới chương trình khuyến mãi thành công";


    //      strMessage = "Có lỗi xảy ra";

    //      strMessage = "Tên chương trình khuyến mãi đã tồn tại";

    //      strMessage = $"Sản phẩm với Id: {string.Join(",", existingProductIds)} :Đã tồn tại trong chương trình khuyến mãi";

    //    strMessage = "Cập nhật chương trình khuyến mãi thành công";

    //      strMessage = "Mã chương trình khuyến mãi không tồn tại";

    //    strMessage = "Xóa chương trình khuyến mãi thành công";

    //strMessage = "Nhà cung cấp không tồn tại";

    //      strMessage = "Tên nhà cung cấp đã tồn tại";
    //      strMessage = "Email đã tồn tại";
    //      strMessage = "Số điện thoại đã tồn tại";

    //      strMessage = "Mã số thuế đã tồn tại";


    //    strMessage = "Tạo mới thành công";

    //      strMessage = "Nhà cung cấp không tồn tại";

    //      strMessage = "Tên nhà cung cấp đã tồn tại";
    //      strMessage = "Email đã tồn tại";

    //      strMessage = "Số điện thoại đã tồn tại";

    //      strMessage = "Mã số thuế đã tồn tại";

    //    strMessage = "Cập nhật nhà cung cấp thành công";

    //      strMessage = "Nhà cung cấp không tồn tại";

    //    strMessage = "Xóa nhà cung cấp thành công";

    //strMessage = "Thương hiệu không tồn tại";

    //      strMessage = "Tên thương hiệu đã tồn tại";

    //    strMessage = "Tạo mới thương hiệu thành công";

    //      strMessage = "Thương hiệu không tồn tại";

    //      strMessage = "Tên thương hiệu  đã tồn tại";

    //    strMessage = "Cập nhật thương hiệu thành công";

    //    strMessage = "Có lỗi xảy ra";

    //      strMessage = "thương hiệu không tồn tại";

    //    strMessage = "Xóa thành công";

    //strMessage = "Mã khuyến mãi đã hết vui lòng thử lại sau!";

    //      strMessage = "Không tìm thấy thông tin người dùng";

    //      strMessage = "Không tìm thấy thông tin người dùng";

    //      strMessage = "Không tìm thấy đơn hàng";

    //      strMessage = "Đơn hàng đã giao không thể cập nhật trạng thái khác";

    //    strMessage = "Cập nhật đơn hàng thành công";

    //      strMessage = "Không tìm thấy đơn hàng";

    //      strMessage = "Không tìm thấy thông tin khách hàng";

    //strMessage = "Tài khoản không tồn tại";

    //        strMessage = "Role không tồn tại";

    //        strMessage = "Khách hàng không tồn tại";

    //        strMessage = "Nhân viên không tồn tại";

    //        strMessage = "Email đã tồn tại";
    //Mật khẩu mới của bạn là: 
    //    string body = "Click vào link sau để đổi mật khẩu: " + url;

    //emailService.SendEmail(email, "Reset password", url);
    public static string SENDMAILSUCCESS = "Mật khẩu mới của bạn là:";
    public static string SENDMAILBODY = "Click vào link sau để đổi mật khẩu: ";
    public static string SENDMAILSUBJECT = "Reset password";


    // User 
    public static string USERNOTFOUND = "Tài khoản không tồn tại";
    public static string USERROLENOTFOUND = "Role không tồn tại";


    //TrxTransaction
    public static string TRXTRANSACTIONPROMTION = "Mã khuyến mãi đã hết vui lòng thử lại sau!";
    public static string TRXTRANSACTIONNOTFOUNDUSER = "Không tìm thấy thông tin người dùng";
    public static string TRXTRANSACTIONNOTFOUNDORDER = "Không tìm thấy đơn hàng";
    public static string TRXTRANSACTIONDELIVERED = "Đơn hàng đã giao không thể cập nhật trạng thái khác";
    public static string UPDATETRXTRANSACTIONSUCCESS = "Cập nhật đơn hàng thành công";
    public static string TRXTRANSACTIONNOTFOUNDORDEROUT = "Không tìm thấy đơn hàng";
    public static string TRXTRANSACTIONNOTFOUNDUSEROUT = "Không tìm thấy thông tin khách hàng";

    // Trademark
    public static string TRADEMARKNOTFOUND = "Thương hiệu không tồn tại";
    public static string TRADEMARKEXIST = "Tên thương hiệu đã tồn tại";
    public static string ADDTRADEMARKSUCCESS = "Tạo mới thương hiệu thành công";
    public static string UPDATETRADEMARKSUCCESS = "Cập nhật thương hiệu thành công";
    public static string DELETETRADEMARKSUCCESS = "Xóa thành công";



    // supplier
    public static string SUPPLIERNOTFOUND = "Nhà cung cấp không tồn tại";
    public static string SUPPLIEREXIST = "Tên nhà cung cấp đã tồn tại";
    public static string TAXCODEEXIST = "Mã số thuế đã tồn tại";
    public static string ADDSUPPLIERSUCCESS = "Tạo mới thành công";
    public static string UPDATESUPPLIERSUCCESS = "Cập nhật nhà cung cấp thành công";
    public static string DELETESUPPLIERSUCCESS = "Xóa nhà cung cấp thành công";






    // promotion
    public static string PROMOTIONNOTFOUND = "Chương trình khuyến mãi không tồn tại";
    public static string PROMOTIONEXIST = "Tên chương trình khuyến mãi đã tồn tại";
    public static string PROMOTIONSTARTDATE = "Ngày bắt đầu phải lớn hơn hoặc bằng ngày hiện tại";
    public static string PROMOTIONENDDATE = "Ngày kết thúc phải lớn hơn hoặc bằng ngày hiện tại";
    public static string PROMOTIONENDDATESTARTDATE = "Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu";
    public static string PROMOTIONEXISTPRODUCT(List<int> existingProductIds) => $"Sản phẩm với Id: {string.Join(",", existingProductIds)} :Đã tồn tại trong chương trình khuyến mãi";
    public static string ERRORPROMOTION = "Có lỗi xảy ra";
    public static string ADDPROMOTIONSUCCESS = "Tạo mới chương trình khuyến mãi thành công";
    public static string PROMOTIONEXISTNAME = "Tên chương trình khuyến mãi đã tồn tại";
    public static string UPDATEPROMOTIONSUCCESS = "Cập nhật chương trình khuyến mãi thành công";
    public static string PROMOTIONNOTFOUNDID = "Mã chương trình khuyến mãi không tồn tại";
    public static string DELETEPROMOTIONSUCCESS = "Xóa chương trình khuyến mãi thành công";



    // product
    public static string PRODUCTNOTFOUND = "Sản phẩm không tồn tại";
    public static string PRODUCTEXIST = "Sản phẩm đã tồn tại";
    public static string FILEFORMAT = "File không đúng định dạng hoặc dung lượng lớn hơn 10MB";
    public static string ADDPRODUCTSUCCESS = "Thêm sản phẩm thành công";
    public static string UPDATEPRODUCTSUCCESS = "Cập nhật sản phẩm thành công";
    public static string UPDATESUCCESS = "Cập nhật thành công";
    public static string DELETEPRODUCTSUCCESS = "Xóa sản phẩm thành công";
    public static string IMAGEPRODUCTNOTFOUND = "Hình ảnh sản phẩm không tồn tại!";
    public static string DELETEIMAGESUCCESS = "Xóa ảnh không thành công!";
    public static string PRODUCTNOTFOUNDOUT = "Sản phẩm không tồn tại";



    //public static string SENDMAILSUCCESS = "Gửi email thành công";
    public static string PRODUCTCCARTNOTFOUND = "Không tìm thấy sản phẩm trong giỏ hàng";
    public static string DELETECARTSUCCESS = "Xóa sản phẩm khỏi giỏ hàng thành công";
    public static string PRODUCTNOTENOUGH = "Số lượng sản phẩm không đủ";
    public static string ADDCARTSUCCESS = "Thêm vào giỏ hàng thành công";
    public static string ERRORTRYAGAIN = "Có lỗi xảy ra vui lòng thử lại sau!";
    public static string PRODUCTNOTENOUGHOUT = "Số lượng sản phẩm không đủ/ hết hàng";
    public static string UPDATECARTSUCCESS = "Cập nhật giỏ hàng thành công";
    public static string PRODUCTCARTNOTFOUNDOUT = "Không tìm thấy sản phẩm trong giỏ hàng";

    // category
    public static string CATEGORYNOTFOUND = "Danh mục sản phẩm không tồn tại";
    public static string CATEGORYEXIST = "Danh mục sản phẩm đã tồn tại";
    public static string CATEGORYPARENTNOTFOUND = "Danh mục cha không tồn tại";
    public static string ADDCATEGORYSUCCESS = "Tạo mới danh mục thành công";
    public static string UPDATECATEGORYSUCCESS = "Cập nhật danh mục thành công";
    public static string DELETECATEGORYNOTFOUND = "Danh mục sản phẩm không tồn tại";
    public static string DELETECATEGORYSUCCESS = "Xóa danh mục sản phẩm thành công";

    // customer
    public static string CUSTOMERNOTFOUND = "Khách hàng không tồn tại";
    public static string EMAILEXIST = "Email đã tồn tại";
    public static string PHONENUMBEREXISTCUSTOMER = "Số điện thoại đã tồn tại";
    public static string CREATECUSTOMERSUCCESS = "Tạo mới thành công";
    public static string UPDATECUSTOMERSUCCESS = "Cập nhật thành công";
    public static string DELETECUSTOMERSUCCESS = "Xóa thành công";

    // employee

    public static string EMPLOYEENOTFOUND = "Nhân viên không tồn tại";
    public static string UPDATEEMPLOYEESUCCESS = "Cập nhật nhân viên thành công";
    public static string DELETEEMPLOYEESUCCESS = "Xóa thành công";

    // inventory
    public static string PRODUCTNOTFOUNDINVENTORY = "Không tìm thấy sản phẩm";
    public static string CREATEINVENTORYSUCCESS = "Tạo phiếu nhập thành công";
    public static string INVENTORYNOTFOUND = "Không tìm thấy phiếu nhập";
    public static string UPDATEINVENTORYSUCCESS = "Cập nhật hóa đơn nhập thành công";











  }
}
