using ICHI_API.Model;
using ICHI_CORE.Model;
using ICHI_CORE.NlogConfig;
using System.Net;

public class GlobalExceptionHandler
{
  public ApiResponse<T> HandleException<T>(Exception ex)
  {
    if (ex is BadRequestException badRequestEx)
    {
      return new ApiResponse<T>(HttpStatusCode.BadRequest, badRequestEx.Message, default(T));
    }
    else
    {
      NLogger.log.Error(ex.ToString());
      return new ApiResponse<T>(HttpStatusCode.InternalServerError, "Có lỗi xảy ra", default(T));
    }
  }
}
