using Microsoft.EntityFrameworkCore;
using ICHI_CORE.Domain;
using ICHI_CORE.Domain.MasterModel;
using Microsoft.AspNetCore.Http.HttpResults;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;
using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;
using Microsoft.AspNetCore.Server.IISIntegration;
using static System.Net.Mime.MediaTypeNames;
using ICHI_CORE.Helpers;

namespace ICHI_API.Data
{
  public class PcsApiContext : DbContext
  {
    public PcsApiContext(DbContextOptions<PcsApiContext> options) : base(options)
    {
    }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<InventoryReceipt> InventoryReceipts { get; set; }

    public DbSet<InventoryReceiptDetail> InventoryReceiptDetails { get; set; }
    public DbSet<Log> Logs { get; set; }

    public DbSet<Product> Products { get; set; }
    public DbSet<ProductDetail> ProductDetails { get; set; }

    public DbSet<ProductImages> ProductImages { get; set; }
    public DbSet<ProductReturn> ProductReturns { get; set; }
    public DbSet<ProductReturnDetail> ProductReturnDetails { get; set; }
    public DbSet<Promotion> Promotions { get; set; }
    public DbSet<PromotionDetail> PromotionDetails { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Trademark> Trademarks { get; set; }
    public DbSet<TrxTransaction> TrxTransactions { get; set; }
    public DbSet<TransactionDetail> TransactionDetails { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Log>().HasNoKey();
      base.OnModelCreating(modelBuilder);
      modelBuilder.Entity<Role>().HasData(
               new Role
               {
                 Id = 1,
                 RoleName = AppSettings.ADMIN,
                 Description = "Quản trị viên",
                 CreateDate = DateTime.Now,
                 CreateBy = "Admin",
                 ModifiedDate = null,
                 ModifiedBy = null
               },
              new Role
              {
                Id = 2,
                RoleName = AppSettings.EMPLOYEE,
                Description = "Nhân viên",
                CreateDate = DateTime.Now,
                CreateBy = "Admin",
                ModifiedDate = null,
                ModifiedBy = null
              },
              new Role
              {
                Id = 3,
                RoleName = AppSettings.USER,
                Description = "Người dùng",
                CreateDate = DateTime.Now,
                CreateBy = "Admin",
                ModifiedDate = null,
                ModifiedBy = null
              });

      modelBuilder.Entity<User>().HasData(
               // Admin
               new User
               {
                 Email = "tien01nx@gmail.com",
                 Avatar = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.pinterest.com%2Fpin%2F717761877848073%2F&psig=AOvVaw",
                 // Admin
                 Password = "$2a$11$iIcGb07Q8qC2x3bT2kXe4.7815/izsL8vF9tCLyKrtaCD06.HYF7.",
                 IsLocked = false,
                 FailedPassAttemptCount = 0,
                 CreateDate = DateTime.Now,
                 CreateBy = "ADMIN",
                 ModifiedDate = DateTime.Now,
                 ModifiedBy = "ADMIN"
               },
               // nhân viên
               new User
               {
                 Email = "diuthanh88@gmail.com",
                 Avatar = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.pinterest.com%2Fpin%2F717761877848073%2F&psig=AOvVaw",
                 // Admin
                 Password = "$2a$11$iIcGb07Q8qC2x3bT2kXe4.7815/izsL8vF9tCLyKrtaCD06.HYF7.",
                 IsLocked = false,
                 FailedPassAttemptCount = 0,
                 CreateDate = DateTime.Now,
                 CreateBy = "ADMIN",
                 ModifiedDate = DateTime.Now,
                 ModifiedBy = "ADMIN"
               },
               // người dùng
               new User
               {
                 Email = "s2family2001bn@gmail.com",
                 Avatar = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.pinterest.com%2Fpin%2F717761877848073%2F&psig=AOvVaw",
                 // Admin
                 Password = "$2a$11$iIcGb07Q8qC2x3bT2kXe4.7815/izsL8vF9tCLyKrtaCD06.HYF7.",
                 IsLocked = false,
                 FailedPassAttemptCount = 0,
                 CreateDate = DateTime.Now,
                 CreateBy = "ADMIN",
                 ModifiedDate = DateTime.Now,
                 ModifiedBy = "ADMIN"
               });
      modelBuilder.Entity<Customer>().HasData(new Customer
      {
        Id = 1,
        UserId = "s2family2001bn@gmail.com",
        PhoneNumber = "0123456789",
        Address = "123 Đường ABC, Quận XYZ, Thành phố HCM",
        Gender = "Nam",
        Avatar = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.pinterest.com%2Fpin%2F717761877848073%2F&psig=AOvVaw",
        Birthday = DateTime.Now,
        Email = "kh03@gmail.com",
        FullName = "Khách hàng A",
        isActive = true,
        isDeleted = false,
        CreateDate = DateTime.Now,
        CreateBy = "Admin",
        ModifiedDate = null,
        ModifiedBy = null
      });
      modelBuilder.Entity<Employee>().HasData(new Employee
      {
        Id = 1,
        UserId = "diuthanh88@gmail.com",
        PhoneNumber = "0123456789",
        Address = "123 Đường ABC, Quận XYZ, Thành",
        Avatar = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.pinterest.com%2Fpin%2F717761877848073%2F&psig=AOvVaw",
        Birthday = DateTime.Now,
        Email = "nhanvien@gmail.com",
        CreateDate = DateTime.Now,
        CreateBy = "Admin",
        ModifiedDate = null,
        ModifiedBy = null,
        FullName = "Nhân viên A",
        Gender = "Nam",
        isActive = true,
        isDeleted = false
      });
      modelBuilder.Entity<UserRole>().HasData(
        new UserRole
        {
          Id = 1,
          UserId = "tien01nx@gmail.com",
          RoleId = 1,
        },
        new UserRole
        {
          Id = 2,
          UserId = "diuthanh88@gmail.com",
          RoleId = 2,
        },
        new UserRole
        {
          Id = 3,
          UserId = "s2family2001bn@gmail.com",
          RoleId = 3,
        });

      modelBuilder.
        Entity<Category>().HasData(
               new Category
               {
                 Id = 1,
                 ParentID = 0,
                 CategoryLevel = 1,
                 CategoryName = "Dụng cụ viết",
                 Notes = "Mô tả về dụng cụ viết",
                 IsDeleted = false,
                 CreateDate = DateTime.Now,
                 CreateBy = "Admin",
                 ModifiedDate = null,
                 ModifiedBy = null
               },
              new Category
              {
                Id = 2,
                ParentID = 0,
                CategoryLevel = 1,
                CategoryName = "Giấy và sổ tay",
                Notes = "Mô tả về giấy và sổ tay",
                IsDeleted = false,
                CreateDate = DateTime.Now,
                CreateBy = "Admin",
                ModifiedDate = null,
                ModifiedBy = null
              },
              new Category
              {
                Id = 3,
                ParentID = 1,
                CategoryLevel = 2,
                CategoryName = "Bút bi",
                Notes = "Mô tả về bút bi",
                IsDeleted = false,
                CreateDate = DateTime.Now,
                CreateBy = "Admin",
                ModifiedDate = null,
                ModifiedBy = null
              },
              new Category
              {
                Id = 4,
                ParentID = 1,
                CategoryLevel = 2,
                CategoryName = "Bút mực",
                Notes = "Mô tả về bút mực",
                IsDeleted = false,
                CreateDate = DateTime.Now,
                CreateBy = "Admin",
                ModifiedDate = null,
                ModifiedBy = null
              },
              new Category
              {
                Id = 5,
                ParentID = 1,
                CategoryLevel = 2,
                CategoryName = "Bút chì",
                Notes = "Mô tả về bút chì",
                IsDeleted = false,
                CreateDate = DateTime.Now,
                CreateBy = "Admin",
                ModifiedDate = null,
                ModifiedBy = null
              },
              new Category
              {
                Id = 6,
                ParentID = 2,
                CategoryLevel = 2,
                CategoryName = "Giấy in",
                Notes = "Mô tả về giấy in",
                IsDeleted = false,
                CreateDate = DateTime.Now,
                CreateBy = "Admin",
                ModifiedDate = null,
                ModifiedBy = null
              },
              new Category
              {
                Id = 7,
                ParentID = 2,
                CategoryLevel = 2,
                CategoryName = "Sổ tay",
                Notes = "Mô tả về sổ tay",
                IsDeleted = false,
                CreateDate = DateTime.Now,
                CreateBy = "Admin",
                ModifiedDate = null,
                ModifiedBy = null
              },
              new Category
              {
                Id = 8,
                ParentID = 2,
                CategoryLevel = 2,
                CategoryName = "Sổ bìa cứng",
                Notes = "Mô tả về sổ bìa cứng",
                IsDeleted = false,
                CreateDate = DateTime.Now,
                CreateBy = "Admin",
                ModifiedDate = null,
                ModifiedBy = null
              },
              new Category
              {
                Id = 9,
                ParentID = 2,
                CategoryLevel = 2,
                CategoryName = "Sổ bìa mềm",
                Notes = "Mô tả về sổ bìa mềm",
                IsDeleted = false,
                CreateDate = DateTime.Now,
                CreateBy = "Admin",
                ModifiedDate = null,
                ModifiedBy = null
              });
      modelBuilder.Entity<Supplier>().HasData(
        new Supplier
        {
          Id = 1,
          SupplierName = "Nhà cung cấp A",
          TaxCode = "TAX001",
          Address = "123 Đường ABC, Quận XYZ, Thành phố HCM",
          PhoneNumber = "0123456789",
          Notes = "Thông tin chi tiết về nhà cung cấp A",
          Email = "info@supplierA.com",
          BankAccount = "0123456789",
          BankName = "Ngân hàng ABC, Chi nhánh XYZ",
          isActive = true,
          isDeleted = false,
          CreateDate = DateTime.Now,
          CreateBy = "Admin",
          ModifiedDate = null,
          ModifiedBy = null
        },
        new Supplier
        {
          Id = 2,
          SupplierName = "Nhà cung cấp B",
          TaxCode = "TAX002",
          Address = "456 Đường XYZ, Quận ABC, Thành phố HCM",
          PhoneNumber = "0987654321",
          Notes = "Thông tin chi tiết về nhà cung cấp B",
          Email = "info2@supplier.com",
          BankAccount = "9876543210",
          BankName = "Ngân hàng XYZ, Chi nhánh ABC",
          isActive = true,
          isDeleted = false,
          CreateDate = DateTime.Now,
          CreateBy = "Admin",
          ModifiedDate = null,
          ModifiedBy = null
        },
        new Supplier
        {
          Id = 3,
          SupplierName = "Nhà cung cấp C",
          TaxCode = "TAX003",
          Address = "789 Đường LMN, Quận PQR, Thành phố HCM",
          PhoneNumber = "0369852147",
          Notes = "Thông tin chi tiết về nhà cung cấp C",
          Email = "info3@gmail.com",
          BankAccount = "7412589630",
          BankName = "Ngân hàng LMN, Chi nhánh PQR",
          isActive = true,
          isDeleted = false,
          CreateDate = DateTime.Now,
          CreateBy = "Admin",
        },
        new Supplier
        {
          Id = 4,
          SupplierName = "Nhà cung cấp D",
          TaxCode = "TAX004",
          Address = "789 Đường LMN, Quận PQR, Thành phố HCM",
          PhoneNumber = "0369852147",
          Notes = "Thông tin chi tiết về nhà cung cấp D",
          Email = "demo2@gmail.com",
          BankAccount = "7412589630",
          BankName = "Ngân hàng LMN, Chi nhánh PQR",
          isActive = true,
          isDeleted = false,
          CreateDate = DateTime.Now,
          CreateBy = "Admin",
        },
        new Supplier
        {
          Id = 5,
          SupplierName = "Nhà cung cấp E",
          TaxCode = "TAX005",
          Address = "789 Đường LMN, Quận PQR, Thành phố HCM",
          PhoneNumber = "0369852147",
          Notes = "Thông tin chi tiết về nhà cung cấp E",
          Email = "demo2@gmail.com",
          BankAccount = "7412589630",
          BankName = "Ngân hàng LMN, Chi nhánh PQR",
          isActive = true,
          isDeleted = false,
          CreateDate = DateTime.Now,
          CreateBy = "Admin",
        });
      // fake data traremark
      modelBuilder.Entity<Trademark>().HasData(
               new Trademark
               {
                 Id = 1,
                 TrademarkName = "Thương hiệu A",
                 CreateDate = DateTime.Now,
                 CreateBy = "Admin",
                 ModifiedDate = null,
                 ModifiedBy = null
               },
              new Trademark
              {
                Id = 2,
                TrademarkName = "Thương hiệu B",
                CreateDate = DateTime.Now,
                CreateBy = "Admin",
                ModifiedDate = null,
                ModifiedBy = null
              },
              new Trademark
              {
                Id = 3,
                TrademarkName = "Thương hiệu C",
                CreateDate = DateTime.Now,
                CreateBy = "Admin",
                ModifiedDate = null,
                ModifiedBy = null
              },
              new Trademark
              {
                Id = 4,
                TrademarkName = "Thương hiệu D",
                CreateDate = DateTime.Now,
                CreateBy = "Admin",
                ModifiedDate = null,
                ModifiedBy = null
              },
              new Trademark
              {
                Id = 5,
                TrademarkName = "Thương hiệu E",
                CreateDate = DateTime.Now,
                CreateBy = "Admin",
                ModifiedDate = null,
                ModifiedBy = null
              },
                new Trademark
                {
                  Id = 6,
                  TrademarkName = "Thương hiệu F",
                  CreateDate = DateTime.Now,
                  CreateBy = "Admin",
                  ModifiedDate = null,
                  ModifiedBy = null
                },
                new Trademark
                {
                  Id = 7,
                  TrademarkName = "Thương hiệu G",
                  CreateDate = DateTime.Now,
                  CreateBy = "Admin",
                  ModifiedDate = null,
                  ModifiedBy = null
                },
                new Trademark
                {
                  Id = 8,
                  TrademarkName = "Thương hiệu H",
                  CreateDate = DateTime.Now,
                  CreateBy = "Admin",
                  ModifiedDate = null,
                  ModifiedBy = null
                });

      // data product
      //      Id TrademarkId CategoryId Color ProductName Description Price PriorityLevel Notes isActive  isDeleted CreateDate  CreateBy ModifiedDate  ModifiedBy
      //2 4 9 Trắng Bản Đồ<p> < img src = "https://salt.tikicdn.com/ts/tmp/42/36/fa/73fa41041422e798678760b2150701c4.jpg" alt = "Bản Đồ" ></ p >
      //< p > H & atilde; y kh&aacute; m ph&aacute; thế giới c & ugrave; ng cuốn bản đồ khổng lồ đầu ti&ecirc; n ở Việt Nam!S & aacute; ch gồm 52 tấm bản đồ minh họa sinh động c&aacute; c đặc điểm địa l & yacute; v & agrave; bi & ecirc; n giới ch & iacute; nh trị, giới thiệu những địa điểm nổi tiếng, những n & eacute; t đặc trưng, về động vật v&agrave; thực vật bản địa, về con người địa phương, c&aacute; c sự kiện văn h & oacute; a c&ugrave; ng nhiều th & ocirc; ng tin hấp dẫn kh & aacute; c.< br >< br > Đến với cuốn Bản đồ khổng lồ(27x37cm) gồm 52 tấm bản đồ đầy m & agrave; u sắc sống động n & agrave; y, c & aacute; c bạn nhỏ sẽ được thỏa sức kh&aacute; m ph&aacute; thế giới. C & oacute; tất cả 6 tấm bản đồ lục địa v&agrave; 42 bản đồ quốc gia. Ch & acirc; u u c & oacute; g & igrave;, ch & acirc; u & Aacute; nổi tiếng v & igrave; điều chi, kh&iacute; hậu ở ch & acirc; u Phi như thế n & agrave; o? Tất cả những chi tiết nổi bật của từng v & ugrave; ng miền, từng đất nước, như địa danh, trang phục, ẩm thực, lễ hội tập tục truyền thống, v & hellip; v & hellip; đều được liệt k&ecirc; bằng những h & igrave; nh vẽ ngộ nghĩnh đ & aacute; ng y&ecirc; u.Mỗi bản đồ c&oacute; thống k&ecirc; sơ bộ về diện t & iacute; ch, d & acirc; n số, ng&ocirc; n ngữ&hellip; để c&aacute; c bạn nhỏ nắm được th&ocirc; ng tin tổng qu&aacute; t của từng đất nước, ch & acirc; u lục. Mỗi nước đều được ph & acirc; n chia th & agrave; nh c&aacute; c v&ugrave; ng địa l & yacute; cụ thể với t&ecirc; n v&ugrave; ng được viết mờ, c&aacute; c th&agrave; nh phố lớn trong từng nước được viết bằng m&agrave; u đỏ nổi bật với chấm đỏ b&ecirc; n cạnh.< br >< br > Cuốn s & aacute; ch n&agrave; y hứa hẹn sẽ l & agrave; tấm v&eacute; đưa độc giả nhỏ du lịch khắp mọi miền tr&ecirc; n thế giới.C & aacute; c bậc phụ huynh cũng c&oacute; thể đồng h & agrave; nh c&ugrave; ng con em m&igrave; nh, c & ugrave; ng ng&acirc; m cứu từng chi tiết tr&ecirc; n mỗi tấm bản đồ, t & igrave; m hiểu v & agrave; b & agrave; n luận về c&aacute; c địa phương.Th & ocirc; ng qua việc chỉ dẫn, diễn giải cho c&aacute; c con về những th & ocirc; ng tin tr & ecirc; n bản đồ, đ & acirc; y sẽ l & agrave; cuốn s&aacute; ch tương t & aacute; c tốt để bố mẹ kết nối v&agrave; gần gũi với con m & igrave; nh hơn.< br >< br >< strong > CUỐN S & Aacute; CH N&Agrave; Y C&Oacute; G & Igrave; ĐẶC BIỆT?</ strong >< br >< br > Cuốn s & aacute; ch Bản đồ đ&atilde; được xuất bản tại hơn 30 quốc gia, b&aacute; n được hơn 3 triệu bản in, l&agrave; một trong những cuốn bản đồ ăn kh&aacute; ch nhất thế giới. Bản đồ của hai t & aacute; c giả Aleksandra Mizielińska v & agrave; Daniel Mizieliński đ & atilde; gi & agrave; nh được nhiều giải thưởng lớn, nổi bật nhất l & agrave; giải Prix Sorci & egrave; res của Ph & aacute; p v&agrave; giải Premio Andersen của &Yacute; &ndash; hai giải thưởng danh gi & aacute; cho d&ograve; ng s&aacute; ch thiếu nhi.< br >< br > C & aacute; c quốc gia đ&atilde; xuất bản &ldquo; Bản đồ&rdquo;: &Uacute; c, &Aacute; o, Bỉ, Brazil, Canada, Chile, Trung Quốc, Croatia, S&eacute; c, Ecuador, Ai Cập, Fiji, Phần Land, Ph & aacute; p, Đức, Ghana, Hy Lạp, Iceland, Ấn Độ, &Yacute;, Nhật Bản, Jordan, Madagascar, Ma Rốc, Mexico, M & ocirc; ng Cổ, Namibia, Nepal, H&agrave; Lan, New Zealand, Peru, Ba Lan, Nam Phi, Romania, Nga, T&acirc; y Ban Nha, Thụy Điển, Thụy Sĩ, Tanzania, Th & aacute; i Lan, Anh, Mỹ.< br >< br > ĐẶC BIỆT: Phi & ecirc; n bản "Bản đồ" Việt Nam đặc biệt được t&aacute; c giả vẽ ri&ecirc; ng đất nước Việt Nam.< br >< br > Để thực hiện cuốn s&aacute; ch đồ sộ n&agrave; y, hai t&aacute; c giả trẻ đ&atilde; phải mất hơn 3 năm trời. Sau khi nghi & ecirc; n cứu v & agrave; t & igrave; m hiểu kỹ lưỡng, họ lập một danh s&aacute; ch c&aacute; c th&ocirc; ng tin hấp dẫn v & agrave; th & uacute; vị với trẻ em, chọn lọc ra những chi tiết đặc sắc nhất của mỗi nước để vẽ v&agrave; o bản đồ.C & aacute; c tấm bản đồ đều được vẽ theo tỉ lệ chuẩn x&aacute; c dựa tr & ecirc; n c&aacute; c bản đồ địa l & yacute; đ & atilde; được ph&aacute; t h&agrave; nh.Hai t&aacute; c giả kh & ocirc; ng chỉ vẽ tay tất cả c & aacute; c chi tiết h&igrave; nh ảnh m & agrave; c & ograve; n d&agrave; y c&ocirc; ng thiết kế tất cả c&aacute; c ph&ocirc; ng chữ được d&ugrave; ng trong s & aacute; ch.</ p >
      //< p > Gi & aacute; sản phẩm tr & ecirc; n Tiki đ & atilde; bao gồm thuế theo luật hiện h & agrave; nh.B & ecirc; n cạnh đ & oacute;, tuỳ v&agrave; o loại sản phẩm, h&igrave; nh thức v & agrave; địa chỉ giao h&agrave; ng m&agrave; c & oacute; thể ph&aacute; t sinh th & ecirc; m chi ph & iacute; kh & aacute; c như ph & iacute; vận chuyển, phụ ph & iacute; h & agrave; ng cồng kềnh, thuế nhập khẩu(đối với đơn h & agrave; ng giao từ nước ngo & agrave; i c&oacute; gi & aacute; trị tr&ecirc; n 1 triệu đồng).....</ p > 50000.00  0   0 1 2024 - 03 - 15 15:04:03.0075389 Admin 2024 - 03 - 15 15:17:25.9444926 Admin
      //3 4 5 Trắng Vị Thần Của Những Quyết Định<p> Kh&ocirc; ng c&oacute; g & igrave; l & agrave; ngẫu nhi&ecirc; n.< br > Mọi chuyện đều l & agrave; tất nhi&ecirc; n.< br > Một cuốn s&aacute; ch t&acirc; m linh gi & uacute; p bạn giải quyết những vấn đề trong cuộc sống, c&ocirc; ng việc, t&igrave; nh cảm&hellip; Nếu bạn đang ph&acirc; n v&acirc; n trước những lựa chọn, nếu bạn đang thiếu quyết đo&aacute; n, nếu bạn kh & ocirc; ng biết tiếp theo n & ecirc; n l&agrave; m g&igrave;: h & atilde; y đặt một c&acirc; u hỏi.< br > V & agrave; h & atilde; y để những vị thần quyết định thay bạn.</ p >
      //< p > *Hai phi & ecirc; n bản để lựa chọn: b & igrave; a đen v & agrave; b & igrave; a hồng<br>*Ng & ocirc; n ngữ: tiếng Việt &amp; tiếng Anh<br>*Hướng dẫn sử dụng cuốn s & aacute; ch:< br > (1) Đặt tay l & ecirc; n b&igrave; a cuốn s & aacute; ch.< br > (2) Nhắm mắt, ho&agrave; n to&agrave; n tập trung v&agrave; o điều bạn muốn hỏi.< br > (3) Đặt một c & acirc; u hỏi(n&oacute; i to hoặc h&igrave; nh dung c & acirc; u hỏi trong đầu), tay vuốt theo m & eacute; p của c & aacute; c trang s & aacute; ch.< br > (4) Khi trực gi & aacute; c m&aacute; ch bảo thời điểm th & iacute; ch hợp, h&atilde; y mở s & aacute; ch ra v & agrave; bạn sẽ c & oacute; c & acirc; u trả lời.< br > (5) Lặp lại quy tr&igrave; nh cho c & aacute; c c&acirc; u hỏi kh & aacute; c.</ p >
      //< p > Gi & aacute; sản phẩm tr & ecirc; n Tiki đ & atilde; bao gồm thuế theo luật hiện h & agrave; nh.B & ecirc; n cạnh đ & oacute;, tuỳ v&agrave; o loại sản phẩm, h&igrave; nh thức v & agrave; địa chỉ giao h&agrave; ng m&agrave; c & oacute; thể ph&aacute; t sinh th & ecirc; m chi ph & iacute; kh & aacute; c như ph & iacute; vận chuyển, phụ ph & iacute; h & agrave; ng cồng kềnh, thuế nhập khẩu(đối với đơn h & agrave; ng giao từ nước ngo & agrave; i c&oacute; gi & aacute; trị tr&ecirc; n 1 triệu đồng).....</ p > 45000.00  0   1 1 2024 - 03 - 14 09:07:13.2704470 Admin 2024 - 03 - 15 15:17:35.8794186 Admin
      //4 5 7 Trắng Sách Hiểu Về Trái Tim(Tái Bản 2019) -Minh Niệm < p class="MsoNormal"><strong><img src = "https://vcdn.tikicdn.com/ts/tmp/a1/dd/30/0d1aa4020c3f5ece81362f1849e56a5e.jpg" alt="" width="750" height="972"></strong></p>
      //<p class="MsoNormal"><strong>Hiểu Về Tr&aacute;i Tim &ndash; Cuốn S&aacute;ch Mở Cửa Thề Giới Cảm X&uacute;c Của Mỗi Người</strong></p>
      //<p class="MsoNormal">Xuất bản lần đầu ti&ecirc;n v&agrave;o năm 2011, Hiểu Về Tr&aacute;i Tim tr&igrave;nh l&agrave;ng cũng l&uacute;c với kỷ lục: cuốn s&aacute;ch c&oacute; số lượng in lần đầu lớn nhất: 100.000 bản.Trung t&acirc;m s&aacute;ch kỷ lục Việt Nam c&ocirc;ng nhận kỳ t&iacute;ch ấy nhưng đến nay, con số ph&aacute;t h&agrave;nh của Hiểu về tr&aacute;i tim vẫn chưa c&oacute; dấu hiệu chậm lại.</p>
      //<p class="MsoNormal">L&agrave; t&aacute;c phẩm đầu tay của nh&agrave; sư Minh Niệm, người s&aacute;ng lập d&ograve;ng thiền hiểu biết(Understanding Meditation), kết hợp giữa tư tưởng Phật gi&aacute;o Đại thừa v&agrave; Thiền nguy&ecirc;n thủy Vipassana, nhưng Hiểu Về Tr&aacute;i Tim kh&ocirc;ng phải t&aacute;c phẩm thuyết gi&aacute;o về Phật ph&aacute;p.Cuốn s&aacute;ch rất &ldquo;đời&rdquo; với những ưu tư của một người tu nh&igrave;n về c&otilde;i thế.Ở đ&oacute;, c&oacute; hạnh ph&uacute;c, c&oacute; đau khổ, c&oacute; t&igrave;nh y&ecirc;u, c&oacute; c&ocirc; đơn, c&oacute; tuyệt vọng, c&oacute; lười biếng, c&oacute; yếu đuối, c&oacute; bu&ocirc;ng xả&hellip; Nhưng, tất cả những hỉ nộ &aacute;i ố ấy đều được kho&aacute;c l&ecirc;n tấm &aacute;o mới, một tấm &aacute;o tinh khiết v&agrave; xuy&ecirc;n suốt, khiến người đọc khi nh&igrave;n v&agrave;o, đều thấy mọi sự như nhẹ nh&agrave;ng hơn&hellip;</p>
      //<p class="MsoNormal"><img src = "https://vcdn.tikicdn.com/ts/product/0c/e1/06/06a3b9bfbbd775345370ff6629eadb4e.jpg" alt="" width="750" height="971"></p>
      //<p class="MsoNormal">Sinh tại Ch&acirc;u Th&agrave;nh, Tiền Giang, xuất gia tại Phật Học Viện Huệ Nghi&ecirc;m &ndash; S&agrave;i G&ograve;n, Minh Niệm từng thọ gi&aacute;o thiền sư Th&iacute;ch Nhất Hạnh tại Ph&aacute;p v&agrave; thiền sư Tejaniya tại Mỹ.Kết quả sau qu&aacute; tr&igrave;nh tu tập, lĩnh hội kiến thức&hellip; &Ocirc;ng quyết định chọn con đường hướng dẫn thiền v&agrave; khai triển t&acirc;m l&yacute; trị liệu cho giới trẻ l&agrave;m Phật sự của m&igrave;nh.Tiếp cận với nhiều người trẻ, lắng nghe thế giới quan của họ v&agrave; quan s&aacute;t những đổi thay trong đời sống hiện đại, &ocirc;ng ph&aacute;t hiện c&oacute; rất nhiều vấn đề của cuộc sống.Nhưng, tất cả, chỉ xuất ph&aacute;t từ một nguy&ecirc;n nh&acirc;n: Ch&uacute;ng ta chưa hiểu, v&agrave; chưa hiểu đ&uacute;ng về tr&aacute;i tim m&igrave;nh l&agrave; chưa cơ chế vận động của những hỉ, nộ, &aacute;i, ố trong mỗi con người. &ldquo;T&ocirc;i đ&atilde; từng quyết l&ograve;ng ra đi t&igrave;m hạnh ph&uacute;c ch&acirc;n thật.D&ugrave; thời điểm ấy, &yacute; niệm về hạnh ph&uacute;c ch&acirc;n thật trong t&ocirc;i rất mơ hồ nhưng t&ocirc;i vẫn tin rằng n&oacute; c&oacute; thật v&agrave; lu&ocirc;n hiện hữu trong thực tại.Hơn mười năm sau, t&ocirc; i mới thấy con đường.V&agrave; cũng chừng ấy năm nữa, t&ocirc;i mới tự tin đặt b&uacute;t viết về những điều m&igrave;nh đ&atilde; kh&aacute;m ph&aacute; v&agrave; trải nghiệm&hellip;&rdquo;, t&aacute;c giả chia sẻ.</p>
      //<p class="MsoNormal"><img src = "https://vcdn.tikicdn.com/ts/tmp/5a/92/d0/fbf3268e4f18030feeff2f22f2583d90.jpg" alt="" width="750" height="976"></p>
      //<p class="MsoNormal">Gần 500 trang s&aacute;ch, Hiểu Về Tr&aacute;i Tim l&agrave; những ph&aacute;c thảo r&otilde; n&eacute;t về bức tranh đời sống cảm x&uacute;c của tất cả mọi người.Người đọc sẽ t&igrave; m thấy căn nguy&ecirc;n th&agrave;nh h&igrave;nh của những x&uacute;c cảm, thấy cả việc ch&uacute;ng chi phối thế n&agrave;o đến h&agrave;nh xử thường ng&agrave;y v&agrave; quan trọng hơn cả l&agrave; c&aacute;ch thức để điều khiển ch&uacute;ng thế n&agrave;o.Kh&ocirc;ng c&oacute; c&acirc;u trả lời cuối c&ugrave;ng của việc đ&uacute;ng &ndash; sai trong từng t&igrave;nh huống nhưng Hiểu Về Tr&aacute;i Tim c&oacute; chứa trong n&oacute; ch&igrave;a kh&oacute;a để mở ra một c&aacute;nh cửa đến với thế giới mới, thế giới an lạc từ trong t&acirc;m mỗi người.Bởi, suy cho c&ugrave;ng, mỗi tr&aacute;i tim - cơ quan ch&uacute;ng ta thường gắn cho nhiệm vụ điều khiển tr&iacute; tuệ cảm x&uacute;c của con người, đều c&oacute; những nỗi niềm ri&ecirc;ng.Chỉ cần hiểu c&acirc;u chuyện của tr&aacute;i tim, tự khắc, mỗi người sẽ quyết định được c&acirc;u chuyện của ch&iacute;nh m&igrave;nh.B&iacute; quyết của sự chuyển h&oacute;a l&agrave; kh&ocirc;ng n&ecirc;n d&ugrave;ng &yacute; ch&iacute; để &aacute;p đặt hay nhồi nặn t&acirc;m m&igrave;nh trở th&agrave;nh một kiểu mẫu tốt đẹp n&agrave;o đ&oacute;. Chỉ cẩn quan s&aacute;t v&agrave; thấu hiểu ch&uacute;ng l&agrave; đủ.T&aacute;c giả nhận định: &ldquo;Việc đưa t&acirc;m thức vượt l&ecirc;n những cung bậc cao hơn để nh&igrave;n đ&uacute;ng đắn hơn về th&acirc;n phận của m&igrave;nh v&agrave; bản chất cuộc sống l&agrave; điều ho&agrave;n to&agrave;n c&oacute; thể l&agrave;m được&rdquo;.</p>
      //<p class="MsoNormal"><img src = "https://vcdn.tikicdn.com/ts/product/12/79/74/175d52e69c01d68030aac2eb7e3d33eb.jpg" alt="" width="750" height="974"></p>
      //<p class="MsoNormal">L&uacute;c sinh thời cố Gi&aacute;o sư, Tiến sĩ Trần Văn Khu&ecirc;, c&oacute; dịp tiếp cận với Hiểu Về Tr&aacute;i Tim. &Ocirc; ng nhận x&eacute;t, như một cuốn s&aacute;ch đầu ti&ecirc;n thuộc chủ đề Hạt Giống T&acirc;m Hồn do một t&aacute;c giả Việt Nam viết, cuốn s&aacute;ch sẽ gi&uacute;p người đọc hiểu được cảm x&uacute;c của t&acirc;m hồn, tr&aacute;i tim của ch&iacute;nh m&igrave;nh v&agrave; của người kh&aacute;c.Để, tận c&ugrave;ng l&agrave; loại bỏ nỗi buồn, tổn thương v&agrave; t&igrave;m được hạnh ph&uacute;c trong cuộc sống.C&oacute; lẽ, v&igrave; điều n&agrave;y m&agrave; gần 10 năm qua, Hiểu về tr&aacute;i tim vẫn l&agrave; cuốn s&aacute;ch li&ecirc;n tục được t&aacute;i bản v&agrave; chưa c&oacute; dấu hiệu &ldquo;hạ nhiệt&rdquo;. Đ&aacute;ng qu&yacute; hơn, t&ograve;an bộ lợi nhuận thu được từ việc ph&aacute;t h&agrave;nh cuốn s&aacute;ch n&agrave;y đều được chuyển về quỹ từ thiện c&ugrave;ng t&ecirc;n để gi&uacute;p đỡ trẻ em c&oacute; ho&agrave;n cảnh kh&oacute; khăn, bất hạnh tại Việt Nam.</p>
      //<p>Gi&aacute; sản phẩm tr&ecirc;n Tiki đ&atilde; bao gồm thuế theo luật hiện h&agrave;nh.B&ecirc;n cạnh đ&oacute;, tuỳ v&agrave;o loại sản phẩm, h&igrave;nh thức v&agrave; địa chỉ giao h&agrave;ng m&agrave; c&oacute; thể ph&aacute;t sinh th&ecirc;m chi ph&iacute; kh&aacute;c như ph&iacute; vận chuyển, phụ ph&iacute; h&agrave;ng cồng kềnh, thuế nhập khẩu(đối với đơn h&agrave; ng giao từ nước ngo&agrave;i c&oacute; gi&aacute; trị tr&ecirc;n 1 triệu đồng).....</p>	60000.00	0		0	0	2024-03-14 09:08:50.0207084	Admin	2024-03-14 09:08:50.0207095	Admin
      //5	4	9	Trắng hfhfgh<p>gfhbgfhfghfdhf</p> 70000.00	0	fgh 0	1	2024-03-15 15:06:38.9023894	Admin 2024-03-15 15:17:39.3063125	Admin
      //6	4	9	Hồng jghjg43443	<p>retrett</p>  777.00	0	hjgjh 1	0	2024-03-15 15:16:42.7457786	Admin 2024-03-15 15:16:42.7457816	Admin
      modelBuilder.Entity<Product>().HasData(
             new Product
             {
               Id = 2,
               TrademarkId = 4,
               CategoryId = 9,
               Color = "Trắng",
               ProductName = "Bản Đồ",
               Description = "<p><img src=\"https://salt.tikicdn.com/ts/tmp/42/36/fa/73fa41041422e798678760b2150701c4.jpg\" alt=\"Bản Đồ\"></p>\r\n<p>H&atilde;y kh&aacute;m ph&aacute; thế giới c&ugrave;ng cuốn bản đồ khổng lồ đầu ti&ecirc;n ở Việt Nam! S&aacute;ch gồm 52 tấm bản đồ minh họa sinh động c&aacute;c đặc điểm địa l&yacute; v&agrave; bi&ecirc;n giới ch&iacute;nh trị, giới thiệu những địa điểm nổi tiếng, những n&eacute;t đặc trưng, về động vật v&agrave; thực vật bản địa, về con người địa phương, c&aacute;c sự kiện văn h&oacute;a c&ugrave;ng nhiều th&ocirc;ng tin hấp dẫn kh&aacute;c.<br><br>Đến với cuốn Bản đồ khổng lồ (27x37cm) gồm 52 tấm bản đồ đầy m&agrave;u sắc sống động n&agrave;y, c&aacute;c bạn nhỏ sẽ được thỏa sức kh&aacute;m ph&aacute; thế giới. C&oacute; tất cả 6 tấm bản đồ lục địa v&agrave; 42 bản đồ quốc gia. Ch&acirc;u u c&oacute; g&igrave;, ch&acirc;u &Aacute; nổi tiếng v&igrave; điều chi, kh&iacute; hậu ở ch&acirc;u Phi như thế n&agrave;o? Tất cả những chi tiết nổi bật của từng v&ugrave;ng miền, từng đất nước, như địa danh, trang phục, ẩm thực, lễ hội tập tục truyền thống, v&hellip;v&hellip; đều được liệt k&ecirc; bằng những h&igrave;nh vẽ ngộ nghĩnh đ&aacute;ng y&ecirc;u. Mỗi bản đồ c&oacute; thống k&ecirc; sơ bộ về diện t&iacute;ch, d&acirc;n số, ng&ocirc;n ngữ&hellip; để c&aacute;c bạn nhỏ nắm được th&ocirc;ng tin tổng qu&aacute;t của từng đất nước, ch&acirc;u lục. Mỗi nước đều được ph&acirc;n chia th&agrave;nh c&aacute;c v&ugrave;ng địa l&yacute; cụ thể với t&ecirc;n v&ugrave;ng được viết mờ, c&aacute;c th&agrave;nh phố lớn trong từng nước được viết bằng m&agrave;u đỏ nổi bật với chấm đỏ b&ecirc;n cạnh.<br><br>Cuốn s&aacute;ch n&agrave;y hứa hẹn sẽ l&agrave; tấm v&eacute; đưa độc giả nhỏ du lịch khắp mọi miền tr&ecirc;n thế giới. C&aacute;c bậc phụ huynh cũng c&oacute; thể đồng h&agrave;nh c&ugrave;ng con em m&igrave;nh, c&ugrave;ng ng&acirc;m cứu từng chi tiết tr&ecirc;n mỗi tấm bản đồ, t&igrave;m hiểu v&agrave; b&agrave;n luận về c&aacute;c địa phương. Th&ocirc;ng qua việc chỉ dẫn, diễn giải cho c&aacute;c con về những th&ocirc;ng tin tr&ecirc;n bản đồ, đ&acirc;y sẽ l&agrave; cuốn s&aacute;ch tương t&aacute;c tốt để bố mẹ kết nối v&agrave; gần gũi với con m&igrave;nh hơn.<br><br><strong>CUỐN S&Aacute;CH N&Agrave;Y C&Oacute; G&Igrave; ĐẶC BIỆT?</strong><br><br>Cuốn s&aacute;ch Bản đồ đ&atilde; được xuất bản tại hơn 30 quốc gia, b&aacute;n được hơn 3 triệu bản in, l&agrave; một trong những cuốn bản đồ ăn kh&aacute;ch nhất thế giới. Bản đồ của hai t&aacute;c giả Aleksandra Mizielińska v&agrave; Daniel Mizieliński đ&atilde; gi&agrave;nh được nhiều giải thưởng lớn, nổi bật nhất l&agrave; giải Prix Sorci&egrave;res của Ph&aacute;p v&agrave; giải Premio Andersen của &Yacute; &ndash; hai giải thưởng danh gi&aacute; cho d&ograve;ng s&aacute;ch thiếu nhi.<br><br>C&aacute;c quốc gia đ&atilde; xuất bản &ldquo;Bản đồ&rdquo;: &Uacute;c, &Aacute;o, Bỉ, Brazil, Canada, Chile, Trung Quốc, Croatia, S&eacute;c, Ecuador, Ai Cập, Fiji, Phần Land, Ph&aacute;p, Đức, Ghana, Hy Lạp, Iceland, Ấn Độ, &Yacute;, Nhật Bản, Jordan, Madagascar, Ma Rốc, Mexico, M&ocirc;ng Cổ, Namibia, Nepal, H&agrave; Lan, New Zealand, Peru, Ba Lan, Nam Phi, Romania, Nga, T&acirc;y Ban Nha, Thụy Điển, Thụy Sĩ, Tanzania, Th&aacute;i Lan, Anh, Mỹ.<br><br>ĐẶC BIỆT: Phi&ecirc;n bản \"Bản đồ\" Việt Nam đặc biệt được t&aacute;c giả vẽ ri&ecirc;ng đất nước Việt Nam.<br><br>Để thực hiện cuốn s&aacute;ch đồ sộ n&agrave;y, hai t&aacute;c giả trẻ đ&atilde; phải mất hơn 3 năm trời. Sau khi nghi&ecirc;n cứu v&agrave; t&igrave;m hiểu kỹ lưỡng, họ lập một danh s&aacute;ch c&aacute;c th&ocirc;ng tin hấp dẫn v&agrave; th&uacute; vị với trẻ em, chọn lọc ra những chi tiết đặc sắc nhất của mỗi nước để vẽ v&agrave;o bản đồ. C&aacute;c tấm bản đồ đều được vẽ theo tỉ lệ chuẩn x&aacute;c dựa tr&ecirc;n c&aacute;c bản đồ địa l&yacute; đ&atilde; được ph&aacute;t h&agrave;nh. Hai t&aacute;c giả kh&ocirc;ng chỉ vẽ tay tất cả c&aacute;c chi tiết h&igrave;nh ảnh m&agrave; c&ograve;n d&agrave;y c&ocirc;ng thiết kế tất cả c&aacute;c ph&ocirc;ng chữ được d&ugrave;ng trong s&aacute;ch.</p>\r\n<p>Gi&aacute; sản phẩm tr&ecirc;n Tiki đ&atilde; bao gồm thuế theo luật hiện h&agrave;nh. B&ecirc;n cạnh đ&oacute;, tuỳ v&agrave;o loại sản phẩm, h&igrave;nh thức v&agrave; địa chỉ giao h&agrave;ng m&agrave; c&oacute; thể ph&aacute;t sinh th&ecirc;m chi ph&iacute; kh&aacute;c như ph&iacute; vận chuyển, phụ ph&iacute; h&agrave;ng cồng kềnh, thuế nhập khẩu (đối với đơn h&agrave;ng giao từ nước ngo&agrave;i c&oacute; gi&aacute; trị tr&ecirc;n 1 triệu đồng).....</p>",
               Price = 50000,
               PriorityLevel = 0,
               Quantity = 0,
               Notes = "",
               isActive = true,
               isDeleted = false,
               CreateBy = "Admin",
               CreateDate = DateTime.Now,
             },
             new Product
             {
               Id = 3,
               TrademarkId = 4,
               CategoryId = 5,
               Color = "Trắng",
               ProductName = "Vị Thần Của Những Quyết Định",
               Description = "<p>Kh&ocirc;ng c&oacute; g&igrave; l&agrave; ngẫu nhi&ecirc;n.<br>Mọi chuyện đều l&agrave; tất nhi&ecirc;n.<br>Một cuốn s&aacute;ch t&acirc;m linh gi&uacute;p bạn giải quyết những vấn đề trong cuộc sống, c&ocirc;ng việc, t&igrave;nh cảm&hellip; Nếu bạn đang ph&acirc;n v&acirc;n trước những lựa chọn, nếu bạn đang thiếu quyết định, nếu bạn kh&ocirc;ng biết tiếp theo n&ecirc;n l&agrave;m g&igrave;: h&atilde;y đặt một c&acirc;u hỏi.<br>V&agrave; h&atilde;y để những vị thần quyết định thay bạn.</p>",
               Price = 45000,
               PriorityLevel = 0,
               Quantity = 0,
               Notes = "",
               isActive = true,
               isDeleted = false,
               CreateBy = "Admin",
               CreateDate = DateTime.Now,
             },
             new Product
             {
               Id = 4,
               TrademarkId = 5,
               CategoryId = 7,
               Color = "Trắng",
               ProductName = "Sách Hiểu Về Trái Tim(Tái Bản 2019) -Minh Niệm",
               Description = "<p class=\"MsoNormal\"><strong><img src=\"https://vcdn.tikicdn.com/ts/tmp/a1/dd/30/0d1aa4020c3f5ece81362f1849e56a5e.jpg\" alt=\"\" width=\"750\" height=\"972\"></strong></p>\r\n<p class=\"MsoNormal\"><strong>Hiểu Về Tr&aacute;i Tim &ndash; Cuốn S&aacute;ch Mở Cửa Thề Giới Cảm X&uacute;c Của Mỗi Người</strong></p>\r\n<p class=\"MsoNormal\">Xuất bản lần đầu ti&ecirc;n v&agrave;o năm 2011, Hiểu Về Tr&aacute;i Tim tr&igrave;nh l&agrave;ng cũng l&uacute;c với kỷ lục: cuốn s&aacute;ch c&oacute; số lượng in lần đầu lớn nhất: 100.000 bản. Trung t&acirc;m s&aacute;ch kỷ lục Việt Nam c&ocirc;ng nhận kỳ t&iacute;ch ấy nhưng đến nay, con số ph&aacute;t h&agrave;nh của Hiểu về tr&aacute;i tim vẫn chưa c&oacute; dấu hiệu chậm lại.</p>\r\n<p class=\"MsoNormal\">L&agrave; t&aacute;c phẩm đầu tay của nh&agrave; sư Minh Niệm, người s&aacute;ng lập d&ograve;ng thiền hiểu biết (Understanding Meditation), kết hợp giữa tư tưởng Phật gi&aacute;o Đại thừa v&agrave; Thiền nguy&ecirc;n thủy Vipassana, nhưng Hiểu Về Tr&aacute;i Tim kh&ocirc;ng phải t&aacute;c phẩm thuyết gi&aacute;o về Phật ph&aacute;p. Cuốn s&aacute;ch rất &ldquo;đời&rdquo; với những ưu tư của một người tu nh&igrave;n về c&otilde;i thế. Ở đ&oacute;, c&oacute; hạnh ph&uacute;c, c&oacute; đau khổ, c&oacute; t&igrave;nh y&ecirc;u, c&oacute; c&ocirc; đơn, c&oacute; tuyệt vọng, c&oacute; lười biếng, c&oacute; yếu đuối, c&oacute; bu&ocirc;ng xả&hellip; Nhưng, tất cả những hỉ nộ &aacute;i ố ấy đều được kho&aacute;c l&ecirc;n tấm &aacute;o mới, một tấm &aacute;o tinh khiết v&agrave; xuy&ecirc;n suốt, khiến người đọc khi nh&igrave;n v&agrave;o, đều thấy mọi sự như nhẹ nh&agrave;ng hơn&hellip;</p>\r\n<p class=\"MsoNormal\"><img src=\"https://vcdn.tikicdn.com/ts/product/0c/e1/06/06a3b9bfbbd775345370ff6629eadb4e.jpg\" alt=\"\" width=\"750\" height=\"971\"></p>\r\n<p class=\"MsoNormal\">Sinh tại Ch&acirc;u Th&agrave;nh, Tiền Giang, xuất gia tại Phật Học Viện Huệ Nghi&ecirc;m &ndash; S&agrave;i G&ograve;n, Minh Niệm từng thọ gi&aacute;o thiền sư Th&iacute;ch Nhất Hạnh tại Ph&aacute;p v&agrave; thiền sư Tejaniya tại Mỹ. Kết quả sau qu&aacute; tr&igrave;nh tu tập, lĩnh hội kiến thức&hellip; &Ocirc;ng quyết định chọn con đường hướng dẫn thiền v&agrave; khai triển t&acirc;m l&yacute; trị liệu cho giới trẻ l&agrave;m Phật sự của m&igrave;nh. Tiếp cận với nhiều người trẻ, lắng nghe thế giới quan của họ v&agrave; quan s&aacute;t những đổi thay trong đời sống hiện đại, &ocirc;ng ph&aacute;t hiện c&oacute; rất nhiều vấn đề của cuộc sống. Nhưng, tất cả, chỉ xuất ph&aacute;t từ một nguy&ecirc;n nh&acirc;n: Ch&uacute;ng ta chưa hiểu, v&agrave; chưa hiểu đ&uacute;ng về tr&aacute;i tim m&igrave;nh l&agrave; chưa cơ chế vận động của những hỉ, nộ, &aacute;i, ố trong mỗi con người. &ldquo;T&ocirc;i đ&atilde; từng quyết l&ograve;ng ra đi t&igrave;m hạnh ph&uacute;c ch&acirc;n thật. D&ugrave; thời điểm ấy, &yacute; niệm về hạnh ph&uacute;c ch&acirc;n thật trong t&ocirc;i rất mơ hồ nhưng t&ocirc;i vẫn tin rằng n&oacute; c&oacute; thật v&agrave; lu&ocirc;n hiện hữu trong thực tại. Hơn mười năm sau, t&ocirc;i mới thấy con đường. V&agrave; cũng chừng ấy năm nữa, t&ocirc;i mới tự tin đặt b&uacute;t viết về những điều m&igrave;nh đ&atilde; kh&aacute;m ph&aacute; v&agrave; trải nghiệm&hellip;&rdquo;, t&aacute;c giả chia sẻ.</p>\r\n<p class=\"MsoNormal\"><img src=\"https://vcdn.tikicdn.com/ts/tmp/5a/92/d0/fbf3268e4f18030feeff2f22f2583d90.jpg\" alt=\"\" width=\"750\" height=\"976\"></p>\r\n<p class=\"MsoNormal\">Gần 500 trang s&aacute;ch, Hiểu Về Tr&aacute;i Tim l&agrave; những ph&aacute;c thảo r&otilde; n&eacute;t về bức tranh đời sống cảm x&uacute;c của tất cả mọi người. Người đọc sẽ t&igrave;m thấy căn nguy&ecirc;n th&agrave;nh h&igrave;nh của những x&uacute;c cảm, thấy cả việc ch&uacute;ng chi phối thế n&agrave;o đến h&agrave;nh xử thường ng&agrave;y v&agrave; quan trọng hơn cả l&agrave; c&aacute;ch thức để điều khiển ch&uacute;ng thế n&agrave;o. Kh&ocirc;ng c&oacute; c&acirc;u trả lời cuối c&ugrave;ng của việc đ&uacute;ng &ndash; sai trong từng t&igrave;nh huống nhưng Hiểu Về Tr&aacute;i Tim c&oacute; chứa trong n&oacute; ch&igrave;a kh&oacute;a để mở ra một c&aacute;nh cửa đến với thế giới mới, thế giới an lạc từ trong t&acirc;m mỗi người. Bởi, suy cho c&ugrave;ng, mỗi tr&aacute;i tim - cơ quan ch&uacute;ng ta thường gắn cho nhiệm vụ điều khiển tr&iacute; tuệ cảm x&uacute;c của con người, đều c&oacute; những nỗi niềm ri&ecirc;ng. Chỉ cần hiểu c&acirc;u chuyện của tr&aacute;i tim, tự khắc, mỗi người sẽ quyết định được c&acirc;u chuyện của ch&iacute;nh m&igrave;nh. B&iacute; quyết của sự chuyển h&oacute;a l&agrave; kh&ocirc;ng n&ecirc;n d&ugrave;ng &yacute; ch&iacute; để &aacute;p đặt hay nhồi nặn t&acirc;m m&igrave;nh trở th&agrave;nh một kiểu mẫu tốt đẹp n&agrave;o đ&oacute;. Chỉ cẩn quan s&aacute;t v&agrave; thấu hiểu ch&uacute;ng l&agrave; đủ. T&aacute;c giả nhận định: &ldquo;Việc đưa t&acirc;m thức vượt l&ecirc;n những cung bậc cao hơn để nh&igrave;n đ&uacute;ng đắn hơn về th&acirc;n phận của m&igrave;nh v&agrave; bản chất cuộc sống l&agrave; điều ho&agrave;n to&agrave;n c&oacute; thể l&agrave;m được&rdquo;.</p>\r\n<p class=\"MsoNormal\"><img src=\"https://vcdn.tikicdn.com/ts/product/12/79/74/175d52e69c01d68030aac2eb7e3d33eb.jpg\" alt=\"\" width=\"750\" height=\"974\"></p>\r\n<p class=\"MsoNormal\">L&uacute;c sinh thời cố Gi&aacute;o sư, Tiến sĩ Trần Văn Khu&ecirc;, c&oacute; dịp tiếp cận với Hiểu Về Tr&aacute;i Tim. &Ocirc;ng nhận x&eacute;t, như một cuốn s&aacute;ch đầu ti&ecirc;n thuộc chủ đề Hạt Giống T&acirc;m Hồn do một t&aacute;c giả Việt Nam viết, cuốn s&aacute;ch sẽ gi&uacute;p người đọc hiểu được cảm x&uacute;c của t&acirc;m hồn, tr&aacute;i tim của ch&iacute;nh m&igrave;nh v&agrave; của người kh&aacute;c. Để, tận c&ugrave;ng l&agrave; loại bỏ nỗi buồn, tổn thương v&agrave; t&igrave;m được hạnh ph&uacute;c trong cuộc sống. C&oacute; lẽ, v&igrave; điều n&agrave;y m&agrave; gần 10 năm qua, Hiểu về tr&aacute;i tim vẫn l&agrave; cuốn s&aacute;ch li&ecirc;n tục được t&aacute;i bản v&agrave; chưa c&oacute; dấu hiệu &ldquo;hạ nhiệt&rdquo;. Đ&aacute;ng qu&yacute; hơn, t&ograve;an bộ lợi nhuận thu được từ việc ph&aacute;t h&agrave;nh cuốn s&aacute;ch n&agrave;y đều được chuyển về quỹ từ thiện c&ugrave;ng t&ecirc;n để gi&uacute;p đỡ trẻ em c&oacute; ho&agrave;n cảnh kh&oacute; khăn, bất hạnh tại Việt Nam.</p>\r\n<p>Gi&aacute; sản phẩm tr&ecirc;n Tiki đ&atilde; bao gồm thuế theo luật hiện h&agrave;nh. B&ecirc;n cạnh đ&oacute;, tuỳ v&agrave;o loại sản phẩm, h&igrave;nh thức v&agrave; địa chỉ giao h&agrave;ng m&agrave; c&oacute; thể ph&aacute;t sinh th&ecirc;m chi ph&iacute; kh&aacute;c như ph&iacute; vận chuyển, phụ ph&iacute; h&agrave;ng cồng kềnh, thuế nhập khẩu (đối với đơn h&agrave;ng giao từ nước ngo&agrave;i c&oacute; gi&aacute; trị tr&ecirc;n 1 triệu đồng).....</p>",
               Price = 60000,
               PriorityLevel = 0,
               Quantity = 0,
               Notes = "",
               isActive = true,
               isDeleted = false,
               CreateBy = "Admin",
               CreateDate = DateTime.Now,
             });
      // data productimages
      //      Id ProductId ImageName ImagePath IsDefault IsActive  IsDeleted CreateDate  CreateBy ModifiedDate  ModifiedBy
      //2 2 574854f032ae36fc0d0a57b61f588965.jpg  \images\products\product - 2\66df2941 - a432 - 4b5f - a1ef - 4f07eec2e608.jpg 0 1 0 2024 - 03 - 14 09:05:29.3402840 Admin 2024 - 03 - 14 09:05:29.3402854 Admin
      //3 3 5cb2991cc6a258b7c1cc07105bccaa29.jpg  \images\products\product - 3\04be0986 - 90ea - 41b4 - ac17 - 153d52f3fe74.jpg 0 1 0 2024 - 03 - 14 09:07:13.2950630 Admin 2024 - 03 - 14 09:07:13.2950646 Admin
      //4 4 3f23c30055381c7e58af80a62ce28fa5.jpg  \images\products\product - 4\0016eca6 - 2e6e - 44d2 - 874f - b2deefb97893.jpg 0 1 0 2024 - 03 - 14 09:08:50.0324490 Admin 2024 - 03 - 14 09:08:50.0324505 Admin
      //5 5 Screenshot_3.png  \images\products\product - 5\ff69973a - bad5 - 4757 - be92 - 6792b6d4ff9e.png 0 1 0 2024 - 03 - 15 15:06:46.5061233 Admin 2024 - 03 - 15 15:06:46.5061242 Admin
      //6 6 Screenshot_7.png  \images\products\product - 6\0c3182bd - e13f - 4303 - b7d7 - c69c0616b6b8.png 0 1 0 2024 - 03 - 15 15:10:47.0928107 Admin 2024 - 03 - 15 15:10:47.0928126 Admin
      modelBuilder.Entity<ProductImages>().HasData(new ICHI_CORE.Domain.MasterModel.ProductImages
      {
        Id = 2,
        ProductId = 2,
        ImageName = "574854f032ae36fc0d0a57b61f588965.jpg",
        ImagePath = "\\images\\products\\product-2\\66df2941-a432-4b5f-a1ef-4f07eec2e608.jpg",
        IsDefault = false,
        IsActive = true,
        IsDeleted = false,
        CreateDate = DateTime.Now,
        CreateBy = "Admin",
        ModifiedDate = DateTime.Now,
        ModifiedBy = "Admin"
      },
      new ProductImages
      {
        Id = 3,
        ProductId = 3,
        ImageName = "5cb2991cc6a258b7c1cc07105bccaa29.jpg",
        ImagePath = "\\images\\products\\product-3\\04be0986-90ea-41b4-ac17-153d52f3fe74.jpg",
        IsDefault = false,
        IsActive = true,
        IsDeleted = false,
        CreateDate = DateTime.Now,
        CreateBy = "Admin",
        ModifiedDate = DateTime.Now,
        ModifiedBy = "Admin"
      },
      new ProductImages
      {
        Id = 4,
        ProductId = 4,
        ImageName = "3f23c30055381c7e58af80a62ce28fa5.jpg",
        ImagePath = "\\images\\products\\product-4\\0016eca6-2e6e-44d2-874f-b2deefb97893.jpg",
        IsDefault = false,
        IsActive = true,
        IsDeleted = false,
        CreateDate = DateTime.Now,
        CreateBy = "Admin",
        ModifiedDate = DateTime.Now,
        ModifiedBy = "Admin"
      },
      new ProductImages
      {
        Id = 5,
        ProductId = 4,
        ImageName = "Screenshot_3.png",
        ImagePath = "\\images\\products\\product-5\\ff69973a-bad5-4757-be92-6792b6d4ff9e.png",
        IsDefault = false,
        IsActive = true,
        IsDeleted = false,
        CreateDate = DateTime.Now,
        CreateBy = "Admin",
        ModifiedDate = DateTime.Now,
        ModifiedBy = "Admin"
      },
      new ProductImages
      {
        Id = 6,
        ProductId = 4,
        ImageName = "Screenshot_7.png",
        ImagePath = "\\images\\products\\product-6\\0c3182bd-e13f-4303-b7d7-c69c0616b6b8.png",
        IsDefault = false,
        IsActive = true,
        IsDeleted = false,
        CreateDate = DateTime.Now,
        CreateBy = "Admin",
        ModifiedDate = DateTime.Now,
        ModifiedBy = "Admin"
      });
    }
  }

}