using isRock.LineBot;
using System;
using System.Threading.Tasks;

namespace GroupDelivery.Handlers
{
    public class MessageHandler
    {
        private readonly Bot _bot;
        

        public MessageHandler(
            Bot bot
            )
        {
            _bot = bot;
            
        }

        public async Task HandleMessageAsync(dynamic e, string replyToken, string userId)
        {
            try
            {
                _bot.ReplyMessage(replyToken, "我確定收到你的訊息");
                Console.WriteLine("已回覆成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Reply 發生錯誤：");
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
