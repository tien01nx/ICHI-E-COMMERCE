using FluentFTP;
using ICHI_CORE.Domain;
using ICHI_CORE.Domain.MasterModel;
using Microsoft.AspNetCore.Hosting;
using System.Formats.Asn1;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace ICHI_CORE.Helpers
{
    public class ImageHelper
    {

        // file không đúng định dạng png , jpg, jpeg và lớn hơn 20MB thì không cho thêm

        public static bool CheckImage(IFormFile file)
        {
            try
            {
                // thực hiện kiểm tra file truyền vào
                if (file.Length > 20971520)
                {
                    return false;
                }
                if (file.ContentType != "image/png" && file.ContentType != "image/jpg" && file.ContentType != "image/jpeg")
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lưu ảnh lên server
        /// </summary>
        /// <param name="imgContent">Nội dung của ảnh</param>
        /// <param name="fileName">Tên ảnh</param>
        /// <returns></returns>
        public static string SaveImage(string imgContent, string fileName, string imageRooot)
        {
            // Lưu dữ liệu ảnh vào thư mục
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            string folderName = imageRooot;
            string imageName = fileName;
            string folderImage = Path.Combine(folderName, imageName);
            string filePath = Path.Combine(rootPath, folderName, imageName);

            if (!Directory.Exists(Path.Combine(rootPath, folderName)))
                Directory.CreateDirectory(Path.Combine(rootPath, folderName));

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            byte[] imageData = Convert.FromBase64String(imgContent);
            File.WriteAllBytes(filePath, imageData);
            return folderImage;
        }

        public static string AddImage(string webHostEnvironment, string id, IFormFile file, string patch)
        {
            try
            {
                string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string productPath = patch + id;
                string finalPath = Path.Combine(webHostEnvironment, productPath);
                if (!Directory.Exists(finalPath))
                {
                    Directory.CreateDirectory(finalPath);
                }
                using (var fileStream = new FileStream(Path.Combine(finalPath, filename), FileMode.Create))
                {
                    file.CopyTo(fileStream);

                }
                return @"\" + productPath + @"\" + filename;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool DeleteImage(string webHostEnvironment, string patch)
        {
            try
            {
                if (!string.IsNullOrEmpty(patch))
                {
                    patch = patch.TrimStart('\\', '/');
                    var imagePath = Path.Combine(webHostEnvironment, patch.Replace("/", "\\"));

                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                        return true;
                    }
                }
                return false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Hàm lấy dữ liệu ảnh từ Server
        /// </summary>
        /// <param name="imgPath">Đường dẫn tương đối của ảnh</param>
        /// <returns></returns>
        public static string GetImage(string imgPath)
        {
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(rootPath, imgPath);
            byte[] imageNameBytes = File.ReadAllBytes(filePath);
            string base64ImageName = Convert.ToBase64String(imageNameBytes);
            return base64ImageName;
        }

        /// <summary>
        /// function write data to csv
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="path"></param>
        public static string WriteCSV(string rootpath, string date, string hour, string vin, int status, string prod_line_tp, string admin_psn, int comjudge)
        {
            // check lại đường dẫn đang chưa đúng 
            string rootPath = rootpath;

            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + vin + ".csv"; // Lấy tên tệp
            var filePath = Path.Combine(rootPath, fileName);

            // Kiểm tra nếu thư mục không tồn tại, tạo thư mục mới
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }
            // Sử dụng mã hóa UTF-8
            using (var writer = new StreamWriter(filePath, false, new UTF8Encoding(true)))
            {
                // Ghi tiêu đề cột
                writer.WriteLine("日付, 時刻, 車台番号, 動作表示ステータス, ラインNO, 共通NO, 総合判定");
                writer.WriteLine($"{date},{hour},{vin},{status},{prod_line_tp},{admin_psn},{comjudge}");
            }
            return fileName;
        }
        public static void UploadDataToFtp(string ftpServer, string userName, string password, string localPath, string dataToWrite)
        {
            // Tạo địa chỉ FTP
            Uri serverUri = new Uri(ftpServer + "/" + dataToWrite);

            // Tạo yêu cầu FTP
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(serverUri);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(userName, password);

            // Đọc dữ liệu từ tập tin cục bộ
            if (System.IO.File.Exists(localPath))
            {

                using (Stream localFileStream = System.IO.File.OpenRead(localPath))
                using (Stream requestStream = request.GetRequestStream())
                {
                    localFileStream.CopyTo(requestStream);
                }
            }
            // Nhận phản hồi từ server (nếu cần)
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
            }
        }
    }
}
