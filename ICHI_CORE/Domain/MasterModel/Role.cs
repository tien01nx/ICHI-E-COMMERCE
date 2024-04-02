namespace ICHI_CORE.Domain.MasterModel
{
  public class Role : MasterEntity
  {
    public string RoleName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
  }
}
