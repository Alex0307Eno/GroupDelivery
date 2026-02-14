using isRock.LineBot;
using Microsoft.AspNetCore.Mvc;
using GroupDelivery.Handlers;
using System;
using System.Threading.Tasks;
using System.IO;



namespace GroupDelivery.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LineBotController : ControllerBase
    {
        private readonly MessageHandler _messageHandler;
        private readonly PostbackHandler _postbackHandler;

        public LineBotController(
            MessageHandler messageHandler,
            PostbackHandler postbackHandler)
        {
            _messageHandler = messageHandler;
            _postbackHandler = postbackHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            string body = string.Empty;

            try
            {
                using (var reader = new StreamReader(Request.Body))
                {
                    body = await reader.ReadToEndAsync();
                }

                Console.WriteLine("===== RAW BODY =====");
                Console.WriteLine(body);
                Console.WriteLine("====================");

                if (string.IsNullOrWhiteSpace(body))
                    return Ok();

                var events = Utility.Parsing(body);

                foreach (var e in events.events)
                {
                    Console.WriteLine("EVENT TYPE = " + e.type);

                    var replyToken = e.replyToken;
                    var userId = e.source?.userId ?? "";
                    Console.WriteLine("LINE USER ID = " + userId);

                    if (e.type == "message")
                    {
                        Console.WriteLine(" 進 Controller，準備處理 message");
                        await _messageHandler.HandleMessageAsync(e, replyToken, userId);
                    }
                    else if (e.type == "postback")
                    {
                        await _postbackHandler.HandlePostbackAsync(e, replyToken, userId);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("🔥 Webhook Exception");
                Console.WriteLine(ex.ToString());
                // Webhook 一定要回 200
            }

            return Ok();
        }
    }
}
