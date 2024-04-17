using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;

namespace ICHI_CORE.Controllers.MasterController
{
  [Route("api/ws")]
  public class WebSocketController : ControllerBase
  {
    [HttpGet]
    public async Task Get()
    {
      if (HttpContext.WebSockets.IsWebSocketRequest)
      {
        using var websocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

        // sinh ra ngẫu nhiên giá trị x,y thay đổi sau 2 giây

        var random = new Random();
        while (websocket.State == WebSocketState.Open)
        {

          int x = random.Next(1, 100);
          int y = random.Next(1, 100);
          var buffer = Encoding.UTF8.GetBytes($"{{ \"x\": {x}, \"y\": {y} }}");
          Console.WriteLine($"x : {x}, y: {y}");
          await websocket.SendAsync(
              new ArraySegment<byte>(buffer),
              WebSocketMessageType.Text, true, CancellationToken.None);
          await Task.Delay(1000);

        }
        await websocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed by the server", CancellationToken.None);
      }
    }
  }
}
