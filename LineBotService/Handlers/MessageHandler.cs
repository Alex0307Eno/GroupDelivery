using isRock.LineBot;

namespace LineBotService.Handlers
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
            if (e.message == null || e.message.type != "text")
                return;

            string text = ((string)e.message.text).Trim();

            // Debug 用：任何文字都先回
            if (text.Equals("!", StringComparison.OrdinalIgnoreCase))
            {
                _bot.ReplyMessage(replyToken, "收到啦，今天平安 👍");
            }
            else
            {
                _bot.ReplyMessage(replyToken, $"我收到你說的：{text}");
            }


            _bot.ReplyMessage(replyToken, " 指令未識別。");
        }
    }
}
