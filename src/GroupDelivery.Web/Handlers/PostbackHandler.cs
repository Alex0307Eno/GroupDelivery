using isRock.LineBot;
using System;
using System.Threading.Tasks;


namespace GroupDelivery.Handlers
{
    public class PostbackHandler
    {




        public async Task<string> HandlePostbackAsync(dynamic data, string userId, string replyToken)
        {
            if (data == null)
                return "error";

            if (data.action == "A")
                return "OK";

            return "unknown";
        }

    }
}
