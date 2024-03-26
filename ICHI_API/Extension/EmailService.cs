using ICHI_CORE.Helpers;
using MailKit.Net.Smtp;
using MimeKit;

namespace ICHI_API.Extension
{
  public class EmailService
  {
    public string Email { get; set; }
    public string Password { get; set; }
    public EmailService()
    {
      Email = AppSettings.GmailEmail;
      Password = AppSettings.GmailPassword;
    }
    public void SendEmail(string toEmail, string subject, string body)
    {
      var message = new MimeMessage();
      message.From.Add(new MailboxAddress("Cửa hàng văn phòng phẩm ICHI", "ICHIVPP.com")); // Đổi thành thông tin của bạn
      message.To.Add(new MailboxAddress("", toEmail)); // Gửi đến địa chỉ email của người dùng
      message.Subject = subject;

      var bodyBuilder = new BodyBuilder();
      bodyBuilder.TextBody = body;
      message.Body = bodyBuilder.ToMessageBody();

      using (var client = new SmtpClient())
      {
        client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
        client.Authenticate(Email, Password);

        client.Send(message);
        client.Disconnect(true);
      }
    }
  }
}
