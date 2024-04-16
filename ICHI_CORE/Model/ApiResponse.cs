using System.Net;

namespace ICHI_CORE.Model
{
  public class ApiResponse<T>
  {
    public HttpStatusCode Code { get; set; }

    public string Message { get; set; }

    public T Data { get; set; }

    public ApiResponse(HttpStatusCode _Code, string _Message, T _Data)
    {
      Code = _Code;
      Message = _Message;
      Data = _Data;
    }
  }

  public class ApiResponseGoShip<T>
  {
    public HttpStatusCode Code { get; set; }

    public string Status { get; set; }

    public T Data { get; set; }

    public ApiResponseGoShip(HttpStatusCode _Code, string _Message, T _Data)
    {
      Code = _Code;
      Status = _Message;
      Data = _Data;
    }
  }

  public class Location
  {
    public string Id { get; set; }

    public string Name { get; set; }
  }

  public class Districts
  {
    public string Id { get; set; }

    public string Name { get; set; }

    public string City_Id { get; set; }
  }

  public class Wards
  {
    public string Id { get; set; }

    public string Name { get; set; }

    public string District_id { get; set; }
  }
}
