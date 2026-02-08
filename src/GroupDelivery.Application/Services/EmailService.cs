using GroupDelivery.Domain;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Services
{
    public class EmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }
        #region 範例：登入驗證信
        public async Task SendLoginMail(string toEmail, string loginLink)
        {
            var htmlBody = $@"
                    <!DOCTYPE html>
                    <html lang=""zh-Hant"">
                    <head>
                        <meta charset=""UTF-8"">
                        <title>登入 JO U IN</title>
                    </head>
                    <body style=""margin:0;padding:0;background-color:#f6f7f9;font-family:Arial,Helvetica,sans-serif;"">
                        <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color:#f6f7f9;padding:40px 0;"">
                            <tr>
                                <td align=""center"">
                                    <table width=""520"" cellpadding=""0"" cellspacing=""0"" style=""background:#ffffff;border-radius:12px;box-shadow:0 4px 16px rgba(0,0,0,0.08);padding:32px;"">
                    
                                        <!-- Logo -->
                                        <tr>
                                            <td align=""center"" style=""padding-bottom:24px;"">
                                                <img src=""https://your-domain.com/img/jouin-logo.png""
                                                     alt=""JO U IN""
                                                     style=""height:40px;"" />
                                            </td>
                                        </tr>
                    
                                        <!-- Title -->
                                        <tr>
                                            <td style=""font-size:20px;font-weight:bold;color:#111;text-align:center;padding-bottom:12px;"">
                                                登入 JO U IN
                                            </td>
                                        </tr>
                    
                                        <!-- Content -->
                                        <tr>
                                            <td style=""font-size:14px;color:#444;line-height:1.7;padding-bottom:28px;text-align:center;"">
                                                您剛剛要求登入 <b>JO U IN</b><br/>
                                                請點擊下方按鈕完成登入
                                            </td>
                                        </tr>
                    
                                        <!-- Button -->
                                        <tr>
                                            <td align=""center"" style=""padding-bottom:32px;"">
                                                <a href=""{loginLink}""
                                                   style=""display:inline-block;background:#22c55e;color:#ffffff;
                                                          padding:14px 32px;border-radius:8px;
                                                          text-decoration:none;font-size:15px;font-weight:bold;"">
                                                    登入
                                                </a>
                                            </td>
                                        </tr>
                    
                                        <!-- Notice -->
                                        <tr>
                                            <td style=""font-size:12px;color:#888;line-height:1.6;text-align:center;padding-bottom:16px;"">
                                                此登入連結將於 <b>10 分鐘</b> 後失效<br/>
                                                若非您本人操作，請忽略此信
                                            </td>
                                        </tr>
                    
                                        <!-- Divider -->
                                        <tr>
                                            <td style=""border-top:1px solid #eee;padding-top:16px;font-size:11px;color:#aaa;text-align:center;"">
                                                © {DateTime.Now.Year} JO U IN<br/>
                                                外送湊單平台
                                            </td>
                                        </tr>
                    
                                    </table>
                    
                                    <!-- Fallback link -->
                                    <div style=""font-size:11px;color:#aaa;margin-top:16px;text-align:center;"">
                                        若按鈕無法點擊，請複製以下連結至瀏覽器：<br/>
                                        <span style=""word-break:break-all;"">{loginLink}</span>
                                    </div>
                    
                                </td>
                            </tr>
                        </table>
                    </body>
                    </html>";

            var mail = new MailMessage
            {
                From = new MailAddress(_settings.SMTPUser, "JO U IN"),
                Subject = "登入 JO U IN",
                Body = htmlBody,
                IsBodyHtml = true
            };

            mail.To.Add(toEmail);

            using (var smtp = new SmtpClient(_settings.SMTPHost, _settings.SMTPPort))
            {
                smtp.Credentials = new NetworkCredential(
                    _settings.SMTPUser,
                    _settings.SMTPPassword);

                smtp.EnableSsl = true;

                await smtp.SendMailAsync(mail);
            }
        }
        #endregion
    }
}
