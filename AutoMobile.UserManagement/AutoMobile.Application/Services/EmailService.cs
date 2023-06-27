using AutoMobile.Application.Services.Interface;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task EmailVerification(string to, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration["MailSettings:Mail"]));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = "Account Verification Email - AutoMobile TopSpeed";
            BodyBuilder bodyBuilder = new BodyBuilder();



            #region Reset Email Body in HTML format
            bodyBuilder.HtmlBody = @"<!DOCTYPE html>
              <html lang='en'>
    <head>
        <meta charset='UTF-8'>
        <meta http-equiv='X-UA-Compatible' content='IE=edge'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>Email Verification</title>
    </head>
    <body>
        <div lang='en'><u></u>
            <div style='background:#fff;margin:0;padding:0'>
                <div
                    style='display:none;font-size:1px;color:#fff;line-height:1px;font-family:Helvetica,Arial,sans-serif;max-height:0px;max-width:0px;opacity:0;overflow:hidden'>
                </div>
                <table width='100%' style='background:#fff;min-width:320px' cellspacing='0' cellpadding='0'>
                    <tbody>
                        <tr>
                            <td>
                                <table width='650' align='center' border='0'
                                    style='max-width:650px;margin:0 auto;background-color:#ffffff;border-spacing:0'
                                    cellpadding='0' cellspacing='0'>
                                    <tbody>
                                        <tr>
                                            <td style='padding:28px 24px 10px'>
                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table align='center' width='476' style='max-width:476px;margin:0 auto'
                                                    cellpadding='0' cellspacing='0'>
                                                    <tbody>
                                                        <tr>
                                                            <td align='center'
                                                                style='font-size:24px;line-height:24px;font-family:""""Whyte"""",Arial,Helvetica,sans-serif;color:#000;padding:26px 37px 32px;text-align:center'>
                                                                <h2>AutoMobile TopSpeed</h2>
                                                                <strong><span class='il'>Email Verification</span></strong>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                                <table align='center' width='600' style='max-width:600px;margin:0 auto'
                                                    cellpadding='0' cellspacing='0'>
                                                    <tbody>
                                                        <tr>
                                                            <td align='center'
                                                                style='color:#000000;font-size:18px;line-height:24px;font-family:""""Whyte"""",Arial,Helvetica,sans-serif;text-align:center;padding:10px 24px 32px'>
                                                                To verify your identity, click the following verify button: 
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                                <table align='center' width='476' style='max-width:476px;margin:0 auto'
                                                    cellpadding='0' cellspacing='0'>
                                                    <tbody>
                                                        <tr>
                                                            <td align='center'
                                                                style='background:#fff;color:#000;font-weight:bold;font-size:22px;line-height:22px;font-family:""""Whyte"""",Arial,Helvetica,sans-serif;'>
                                                                    <a href=" + body + @" class='il' target=""_blank"" style='cursor:pointer'> <button id='verifyButton' style ='background-color:green;color:white; padding: 10px;border:none;cursor:pointer;font-family:inherit;font-size:16px; border-radius:8px;'> Verify Your Account</button> 
                                                                     </a>       
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                                <table align='center' width='540' style='max-width:540px;margin:0 auto'
                                                    cellpadding='0' cellspacing='0'>                            
                                                    <tbody>
                                                        <tr>
                                                            <td align='center'
                                                                style='color:#a5a5a5;font-size:14px;line-height:18px;text-align:center;font-family:""""Whyte"""",Arial,Helvetica,sans-serif;padding:10px 24px 60px'>
                                                                For security reasons, Do Not share the link with anyone.
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                            </td>
                                        </tr>
                                       
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <img src='https://ci3.googleusercontent.com/proxy/aiyqPN7pnRgk55i-Jd3NqwRgSXDhKVQkXKpYnFz-TR1gI86nPavxYnwWTLXknWZoionvgG-eIb7qnJsgnV2LYyWRQ6lQDEQOPqjydrw8MfeK7LBSAOHIsZDuL5-23huM1mbkvq5O8r64BzwSgN_-z3hoJcrOeA0sMXQ_u74gJA5qlsHMxOWisyE55GNCCZBk0XabgcKfTR6Z_Wq0iGyyZMmh4SUY1wwVvXoMGad9LaQrZyO63_jynl7xvcCbr3gMVWWTmJygoViqWbm1cccg_UudVlEqy2-QcsMyQ5OS3LeGoZVxDcRN9bG1wljhccSH4s4b2HSDAEa7UA36YEL3bK7fVDT7RA7m0adN9lcI-rtyWVXVHxQuNE4tPWi13WdOBe0etfbEAUTUc8_OVXirRRbDOKyftBWNimklJDqULAgGC-QEUr0Z3gSJFj5bT1YkFKjmlaqsjpoeEKDxemW7ljDPbegCxU0xUhoE3XZh2x3rV_YUnkA23xtuOCJkz_OXP8c7=s0-d-e1-ft#https://u3302489.ct.sendgrid.net/wf/open?upn=0LOMqLJf4t-2FdW4ttMfMXd-2F3ObCyG4mZjVA0uCvg1kdsA4sAVDkeiPMXCBq15IL7o3lJyU-2F3OYdKOpkXB1T5ajfiwMXOUQ-2BUuPAwBIulU5kgfejKaeggz3a7g9CzLpSAZiI6kljtF-2F7Z6p41yToBdeAot-2BDN6gdAO3hks8EZTwB-2BQ-2BW6A8PN0lwMS1D0QRr78cM6F8ngSmNg0Sk2bfOO-2FL3vQ4mOOtxGwzsxC01liENEf6hGto60tGRWlxAbI2Ne2VfrRwDebUd0mWtW6KkuVupeH3uSbvTVW-2Fi0dyxqSoVw-3D'
                    alt='' width='1' height='1' border='0'
                    style='height:1px!important;width:1px!important;border-width:0!important;margin-top:0!important;margin-bottom:0!important;margin-right:0!important;margin-left:0!important;padding-top:0!important;padding-bottom:0!important;padding-right:0!important;padding-left:0!important'>
                <div class='yj6qo'></div>
                <div class='adL'>
                </div>
            </div>
        </div>
    </body>
    </html>";
            #endregion


            //email.Body = new TextPart(TextFormat.Html) { Text = body};
            email.Body = bodyBuilder.ToMessageBody();

            using (var smtp = new SmtpClient())
            {
                smtp.Connect(_configuration["MailSettings:Host"], Convert.ToInt32(_configuration["MailSettings:Port"]), SecureSocketOptions.StartTls); ;
                //smtp.Authenticate("karthik@educarrerconnect.com", "hnlheijaayiihhvl");
                smtp.Authenticate(_configuration["MailSettings:Mail"], _configuration["MailSettings:Password"]);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }

        }

        public async Task ForgetPassword(string to, string body)
        {

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration["MailSettings:Mail"]));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = "Reset Password - AutoMobile TopSpeed";
            BodyBuilder bodyBuilder = new BodyBuilder();



            #region Reset Email Body in HTML format
            bodyBuilder.HtmlBody = @"<!DOCTYPE html>
              <html lang='en'>
    <head>
        <meta charset='UTF-8'>
        <meta http-equiv='X-UA-Compatible' content='IE=edge'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>Reset Password</title>
    </head>
    <body>
        <div lang='en'><u></u>
            <div style='background:#fff;margin:0;padding:0'>
                <div
                    style='display:none;font-size:1px;color:#fff;line-height:1px;font-family:Helvetica,Arial,sans-serif;max-height:0px;max-width:0px;opacity:0;overflow:hidden'>
                </div>
                <table width='100%' style='background:#fff;min-width:320px' cellspacing='0' cellpadding='0'>
                    <tbody>
                        <tr>
                            <td>
                                <table width='650' align='center' border='0'
                                    style='max-width:650px;margin:0 auto;background-color:#ffffff;border-spacing:0'
                                    cellpadding='0' cellspacing='0'>
                                    <tbody>
                                        <tr>
                                            <td style='padding:28px 24px 10px'>
                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table align='center' width='476' style='max-width:476px;margin:0 auto'
                                                    cellpadding='0' cellspacing='0'>
                                                    <tbody>
                                                        <tr>
                                                            <td align='center'
                                                                style='font-size:24px;line-height:24px;font-family:""""Whyte"""",Arial,Helvetica,sans-serif;color:#000;padding:26px 37px 32px;text-align:center'>
                                                                <h2>AutoMobile TopSpeed</h2>
                                                                <strong><span class='il'>Password Reset</span></strong>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                                <table align='center' width='600' style='max-width:600px;margin:0 auto'
                                                    cellpadding='0' cellspacing='0'>
                                                    <tbody>
                                                        <tr>
                                                            <td align='center'
                                                                style='color:#000000;font-size:18px;line-height:24px;font-family:""""Whyte"""",Arial,Helvetica,sans-serif;text-align:center;padding:10px 24px 32px'>
                                                                click the following reset button to set new password: 
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                                <table align='center' width='476' style='max-width:476px;margin:0 auto'
                                                    cellpadding='0' cellspacing='0'>
                                                    <tbody>
                                                        <tr>
                                                            <td align='center'
                                                                style='background:#fff;color:#000;font-weight:bold;font-size:22px;line-height:22px;font-family:""""Whyte"""",Arial,Helvetica,sans-serif;'>
                                                                    <a href=" + body + @" class='il' target=""_blank"" style='cursor:pointer'> <button style ='background-color:green;color:white; padding: 10px;border:none;cursor:pointer;font-family:inherit;font-size:16px; border-radius:8px;'> Password Reset</button> 
                                                                     </a>       
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                                <table align='center' width='540' style='max-width:540px;margin:0 auto'
                                                    cellpadding='0' cellspacing='0'>                            
                                                    <tbody>
                                                        <tr>
                                                            <td align='center'
                                                                style='color:#a5a5a5;font-size:14px;line-height:18px;text-align:center;font-family:""""Whyte"""",Arial,Helvetica,sans-serif;padding:10px 24px 60px'>
                                                                For security reasons, Do Not share the link with anyone.
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                            </td>
                                        </tr>
                                       
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <img src='https://ci3.googleusercontent.com/proxy/aiyqPN7pnRgk55i-Jd3NqwRgSXDhKVQkXKpYnFz-TR1gI86nPavxYnwWTLXknWZoionvgG-eIb7qnJsgnV2LYyWRQ6lQDEQOPqjydrw8MfeK7LBSAOHIsZDuL5-23huM1mbkvq5O8r64BzwSgN_-z3hoJcrOeA0sMXQ_u74gJA5qlsHMxOWisyE55GNCCZBk0XabgcKfTR6Z_Wq0iGyyZMmh4SUY1wwVvXoMGad9LaQrZyO63_jynl7xvcCbr3gMVWWTmJygoViqWbm1cccg_UudVlEqy2-QcsMyQ5OS3LeGoZVxDcRN9bG1wljhccSH4s4b2HSDAEa7UA36YEL3bK7fVDT7RA7m0adN9lcI-rtyWVXVHxQuNE4tPWi13WdOBe0etfbEAUTUc8_OVXirRRbDOKyftBWNimklJDqULAgGC-QEUr0Z3gSJFj5bT1YkFKjmlaqsjpoeEKDxemW7ljDPbegCxU0xUhoE3XZh2x3rV_YUnkA23xtuOCJkz_OXP8c7=s0-d-e1-ft#https://u3302489.ct.sendgrid.net/wf/open?upn=0LOMqLJf4t-2FdW4ttMfMXd-2F3ObCyG4mZjVA0uCvg1kdsA4sAVDkeiPMXCBq15IL7o3lJyU-2F3OYdKOpkXB1T5ajfiwMXOUQ-2BUuPAwBIulU5kgfejKaeggz3a7g9CzLpSAZiI6kljtF-2F7Z6p41yToBdeAot-2BDN6gdAO3hks8EZTwB-2BQ-2BW6A8PN0lwMS1D0QRr78cM6F8ngSmNg0Sk2bfOO-2FL3vQ4mOOtxGwzsxC01liENEf6hGto60tGRWlxAbI2Ne2VfrRwDebUd0mWtW6KkuVupeH3uSbvTVW-2Fi0dyxqSoVw-3D'
                    alt='' width='1' height='1' border='0'
                    style='height:1px!important;width:1px!important;border-width:0!important;margin-top:0!important;margin-bottom:0!important;margin-right:0!important;margin-left:0!important;padding-top:0!important;padding-bottom:0!important;padding-right:0!important;padding-left:0!important'>
                <div class='yj6qo'></div>
                <div class='adL'>
                </div>
            </div>
        </div>
    </body>
    </html>";
            #endregion


            //email.Body = new TextPart(TextFormat.Html) { Text = body};
            email.Body = bodyBuilder.ToMessageBody();

            using (var smtp = new SmtpClient())
            {
                smtp.Connect(_configuration["MailSettings:Host"], Convert.ToInt32(_configuration["MailSettings:Port"]), SecureSocketOptions.StartTls); ;
                //smtp.Authenticate("karthik@educarrerconnect.com", "hnlheijaayiihhvl");
                smtp.Authenticate(_configuration["MailSettings:Mail"], _configuration["MailSettings:Password"]);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
        }
    }
}

