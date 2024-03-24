namespace ICHI_API.Model
{
  public class UserChangePassword
  {
    public string UserName { get; set; }

    public string? oldPassword { get; set; }
    public string Password { get; set; }
    public string NewPassword { get; set; }
  }
}
