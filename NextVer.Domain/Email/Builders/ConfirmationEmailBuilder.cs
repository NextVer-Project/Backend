using MimeKit;
using MimeKit.Text;

namespace NextVer.Domain.Email.Builders
{
    public class ConfirmationEmailBuilder
    {
        public static EmailMessage BuildConfirmationMessage(string receiver, string username, string confirmationUrl, string firstName, string lastName, string userType, string city, string country, string registrationDateTime)
        {
            var expirationTimeInHours = 5;
            var appName = "NextVer";
            var supportEmail = "nextver8@gmail.com";

            var messageBody = @"
<!doctype html><html xmlns=""http://www.w3.org/1999/xhtml"" xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:o=""urn:schemas-microsoft-com:office:office""><head><title></title><!--[if !mso]><!--><meta http-equiv=""X-UA-Compatible"" content=""IE=edge""><!--<![endif]--><meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8""><meta name=""viewport"" content=""width=device-width,initial-scale=1""><style type=""text/css"">#outlook a { padding:0; }
          body { margin:0;padding:0;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%; }
          table, td { border-collapse:collapse;mso-table-lspace:0pt;mso-table-rspace:0pt; }
          img { border:0;height:auto;line-height:100%; outline:none;text-decoration:none;-ms-interpolation-mode:bicubic; }
          p { display:block;margin:13px 0; }</style><!--[if mso]>
        <noscript>
        <xml>
        <o:OfficeDocumentSettings>
          <o:AllowPNG/>
          <o:PixelsPerInch>96</o:PixelsPerInch>
        </o:OfficeDocumentSettings>
        </xml>
        </noscript>
        <![endif]--><!--[if lte mso 11]>
        <style type=""text/css"">
          .mj-outlook-group-fix { width:100% !important; }
        </style>
        <![endif]--><!--[if !mso]><!--><link href=""https://fonts.googleapis.com/css?family=Ubuntu:300,400,500,700"" rel=""stylesheet"" type=""text/css""><style type=""text/css"">@import url(https://fonts.googleapis.com/css?family=Ubuntu:300,400,500,700);</style><!--<![endif]--><style type=""text/css"">@media only screen and (min-width:480px) {
        .mj-column-per-100 { width:100% !important; max-width: 100%; }
.mj-column-per-25 { width:25% !important; max-width: 25%; }
      }</style><style media=""screen and (min-width:480px)"">.moz-text-html .mj-column-per-100 { width:100% !important; max-width: 100%; }
.moz-text-html .mj-column-per-25 { width:25% !important; max-width: 25%; }</style><style type=""text/css""></style><style type=""text/css"">.shadow__btn {
            display: block;
            margin: 0 auto;
            padding: 10px 20px;
            border-radius: 7px;
            border: none;
            transition: 0.5s;
            transition-property: box-shadow;
            width: 40%;
            background: linear-gradient(to left, #005aa7, #fffde4);
            box-shadow: 0 0 25px rgb(0, 140, 255);
        }
        
        .gradient {
            display: block;
            background: linear-gradient(to left, #005aa7, #fffde4);
            box-shadow: 0 0 25px rgb(0, 140, 255);
        }
        
        .shadow__btn:hover {
            box-shadow: 0 0 5px rgb(0, 140, 255), 0 0 25px rgb(0, 61, 110), 0 0 50px rgb(0, 140, 255), 0 0 100px rgb(0, 140, 255);
        }
        
        .info {
            border-collapse: separate;
            margin: 0 auto;
            margin-bottom: 10px;
            color: #bae8e8;
            border-radius: 20px;
            border: 111px solid;
            max-width: 30px;
            border-image-slice: 1;
            border-width: 3px;
            background: linear-gradient(to bottom, #0f2027, #203a43, #2c5364);
        }</style></head><body style=""word-spacing:normal;background-color:black;""><div style=""background-color:black;""><!--[if mso | IE]><table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" class="""" style=""width:600px;"" width=""600"" bgcolor=""white"" ><tr><td style=""line-height:0px;font-size:0px;mso-line-height-rule:exactly;""><![endif]--><div style=""background:white;background-color:white;margin:0px auto;border-radius:21px;max-width:600px;""><table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""background:white;background-color:white;width:100%;border-radius:21px;""><tbody><tr><td style=""border:8px;direction:ltr;font-size:0px;padding:20px 0;padding-bottom:8px;padding-top:8px;text-align:center;""><!--[if mso | IE]><table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""><tr><td class="""" width=""600px"" ><table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" class="""" style=""width:584px;"" width=""584"" bgcolor=""black"" ><tr><td style=""line-height:0px;font-size:0px;mso-line-height-rule:exactly;""><![endif]--><div style=""background:black;background-color:black;margin:0px auto;border-radius:21px;max-width:584px;""><table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""background:black;background-color:black;width:100%;border-radius:21px;""><tbody><tr><td style=""direction:ltr;font-size:0px;padding:10px;text-align:center;""><!--[if mso | IE]><table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""><tr><td class="""" style=""vertical-align:top;width:564px;"" ><![endif]--><div class=""mj-column-per-100 mj-outlook-group-fix"" style=""font-size:0px;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%;""><table border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" width=""100%""><tbody><tr><td style=""background-color:black;vertical-align:top;padding-top:40px;padding-bottom:8px;""><table border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" width=""100%""><tbody><tr><td align=""left"" style=""font-size:0px;padding:20px;word-break:break-word;""><div style=""font-family:Ubuntu, Helvetica, Arial, sans-serif;font-size:13px;line-height:1;text-align:left;color:#000000;""><h1 style=""text-transform: uppercase; font-size: 36px; font-weight: bold; margin-bottom: 1px; color: #e3f6f5; text-align: left;"">Dear,<br><b>" + username + @"</b>,</h1></div></td></tr><tr><td align=""left"" style=""font-size:0px;padding:20px;word-break:break-word;""><div style=""font-family:Ubuntu, Helvetica, Arial, sans-serif;font-size:13px;line-height:1;text-align:left;color:#000000;""><hr style=""border-radius: 25px; background: linear-gradient(to left, #005aa7, #fffde4);"" width=""96%"" size=""6px""><p style=""font-size: 20px; line-height: 1.5; color: #e3f6f5; text-align: justify; text-justify: inter-word;"">Thank you for creating an account with our application. To complete the registration process, please click the link below to verify your email address:</p></div></td></tr><tr><td align=""center"" vertical-align=""middle"" style=""font-size:0px;padding:10px 25px;word-break:break-word;""><table border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""border-collapse:separate;line-height:100%;""><tr><td align=""center"" bgcolor=""transparent"" role=""presentation"" style=""border:none;border-radius:3px;cursor:auto;mso-padding-alt:10px 25px;background:transparent;"" valign=""middle""><a style=""background: radial-gradient(circle, #bb6598, #96538c, #70437e, #4a346e, #21265b); direction:ltr;border-radius:2px;color:#e3f6f5;;display:inline-block;font-size:16px;line-height:24px;font-weight:400;text-align:center;text-decoration:none;padding:14px 20px 13px 20px;font-family:'Google Sans','Roboto',Arial,sans-serif;letter-spacing:0.75px;font-weight:normal;font-size:14px;line-height:21px;border-radius:14px""  href=""" + confirmationUrl + @""">Activate Account!</a></td></tr></table></td></tr><tr><td align=""left"" style=""font-size:0px;padding:20px;word-break:break-word;""><div style=""font-family:Ubuntu, Helvetica, Arial, sans-serif;font-size:13px;line-height:1;text-align:left;color:#000000;""><p style=""font-size: 20px; line-height: 1.5; color: #e3f6f5; text-align: justify; text-justify: inter-word;"">Or copy the following link and paste it in the address bar of your browser:<br>" + confirmationUrl + @"</p><hr style=""border-radius: 25px; background: linear-gradient(to left, #005aa7, #fffde4);"" width=""96%"" size=""9px""><br><mj-text line-height=""1.52"" font-size=""21px"" color=""white""><h3 style=""font-weight: bold; font-size: 23px; text-transform: uppercase; color: white; text-align: center;""> Account summary<br></h3></mj-text></div></td></tr></tbody></table></td></tr></tbody></table></div><!--[if mso | IE]></td><td class=""separateg-outlook"" style=""vertical-align:top;width:564px;"" ><![endif]--><div class=""mj-column-per-100 mj-outlook-group-fix separateg"" style=""font-size:0px;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%;""><table border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" width=""100%"" style=""border-collapse: separate; display: block; background-image: linear-gradient(to right, #0f0c29, #302b63, #24243e); box-shadow: 0 0 45px rgb(0, 140, 255); border-radius: 40px; margin: 0 auto; max-width: 500px;""><tbody><tr><td style=""border:3px solid #e3f6f5;border-radius:40px;vertical-align:top;padding:20px;""><table border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" width=""100%""><tbody><tr><td align=""left"" style=""font-size:0px;padding:20px;word-break:break-word;""><div style=""font-family:Ubuntu, Helvetica, Arial, sans-serif;font-size:21px;line-height:1.52;text-align:left;color:white;""><h3 style="" font-weight: bold; font-size: 18px; text-transform: uppercase; color: white; text-align: center; "">This is your account summary. Please verify that the data is correct:</h3>
                                                                                        <p style="" font-weight: bold; font-size: 23px; text-transform: uppercase; color: white;
                                                                                            text-align: center; "">Username</p>
                                                                                        <p style='margin: 0 auto; margin-bottom: 10px; color: #bae8e8; border-radius: 20px; border: 5px solid; max-width: 300px; border-image-slice: 1; border-width: 3px; background: linear-gradient(to top, black 50%, #bae8e8 ); text-align: center;'>" + username + @"</p>
                                                                                        <hr style="" border-radius: 25px; background: linear-gradient(to left, #005aa7, #fffde4); "" width="" 94% "" size="" 6px "">
                                                                                        <p style="" font-weight: bold; font-size: 23px; text-transform: uppercase; color: white; text-align: center; "">E-mail</p>
                                                                                        <p style='margin: 0 auto; margin-bottom: 10px; color: #bae8e8; border-radius: 20px; border: 5px solid; max-width: 300px; border-image-slice: 1; border-width: 3px; background: linear-gradient(to top, black 50%, #bae8e8 ); text-align: center;'>" + receiver + @"</p>
                                                                                        <hr style="" border-radius: 25px; background: linear-gradient(to left, #005aa7, #fffde4); "" width="" 94% "" size="" 6px "">
                                                                                        <p style="" font-weight: bold; font-size: 23px; text-transform: uppercase; color: white; text-align: center; "">First name</p>
                                                                                        <p style='margin: 0 auto; margin-bottom: 10px; color: #bae8e8; border-radius: 20px; border: 5px solid; max-width: 300px; border-image-slice: 1; border-width: 3px; background: linear-gradient(to top, black 50%, #bae8e8 ); text-align: center;'>" + firstName + @"</p>
                                                                                        <hr style="" border-radius: 25px; background: linear-gradient(to left, #005aa7, #fffde4); "" width="" 94% "" size="" 6px "">
                                                                                        <p style="" font-weight: bold; font-size: 23px; text-transform: uppercase; color: white; text-align: center; "">last name</p>
                                                                                        <p style='margin: 0 auto; margin-bottom: 10px; color: #bae8e8; border-radius: 20px; border: 5px solid; max-width: 300px; border-image-slice: 1; border-width: 3px; background: linear-gradient(to top, black 50%, #bae8e8 ); text-align: center;'>" + lastName + @"</p>
                                                                                        <hr style="" border-radius: 25px; background: linear-gradient(to left, #005aa7, #fffde4); "" width="" 94% "" size="" 6px "">
                                                                                        <p style="" font-weight: bold; font-size: 23px; text-transform: uppercase; color: white; text-align: center; "">Country</p>
                                                                                        <p style='margin: 0 auto; margin-bottom: 10px; color: #bae8e8; border-radius: 20px; border: 5px solid; max-width: 300px; border-image-slice: 1; border-width: 3px; background: linear-gradient(to top, black 50%, #bae8e8 ); text-align: center;'>" + country + @"</p>
                                                                                        <hr style="" border-radius: 25px; background: linear-gradient(to left, #005aa7, #fffde4); "" width="" 94% "" size="" 6px "">
                                                                                        <p style="" font-weight: bold; font-size: 23px; text-transform: uppercase; color: white; text-align: center; "">City</p>
                                                                                        <p style='margin: 0 auto; margin-bottom: 10px; color: #bae8e8; border-radius: 20px; border: 5px solid; max-width: 300px; border-image-slice: 1; border-width: 3px; background: linear-gradient(to top, black 50%, #bae8e8 ); text-align: center;'>" + city + @"</p>
                                                                                        <hr style="" border-radius: 25px; background: linear-gradient(to left, #005aa7, #fffde4); "" width="" 94% "" size="" 6px "">
                                                                                        <p style="" font-weight: bold; font-size: 23px; text-transform: uppercase; color: white; text-align: center; "">Registration Date Time</p>
                                                                                        <p style='margin: 0 auto; margin-bottom: 10px; color: #bae8e8; border-radius: 20px; border: 5px solid; max-width: 300px; border-image-slice: 1; border-width: 3px; background: linear-gradient(to top, black 50%, #bae8e8 ); text-align: center;'>" + registrationDateTime + @"</p>
                                                                                        <hr style="" border-radius: 25px; background: linear-gradient(to left, #005aa7, #fffde4); "" width="" 94% "" size="" 6px "">
                                                                                        <p style="" font-weight: bold; font-size: 23px; text-transform: uppercase; color: white; text-align: center; "">User Type</p>
                                                                                        <p style='margin: 0 auto; margin-bottom: 10px; color: #bae8e8; border-radius: 20px; border: 5px solid; max-width: 300px; border-image-slice: 1; border-width: 3px; background: linear-gradient(to top, black 50%, #bae8e8 ); text-align: center;'>" + userType + @"</p></div></td></tr></tbody></table></td></tr></tbody></table></div><!--[if mso | IE]></td><td class="""" style=""vertical-align:top;width:141px;"" ><![endif]--><div class=""mj-column-per-25 mj-outlook-group-fix"" style=""font-size:0px;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%;""><table border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""vertical-align:top;"" width=""100%""><tbody><tr><td align=""left"" style=""font-size:0px;padding:20px;word-break:break-word;""><div style=""font-family:Ubuntu, Helvetica, Arial, sans-serif;font-size:13px;line-height:1;text-align:left;color:#000000;""></div></td></tr></tbody></table></div><!--[if mso | IE]></td><td class="""" style=""vertical-align:top;width:564px;"" ><![endif]--><div class=""mj-column-per-100 mj-outlook-group-fix"" style=""font-size:0px;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%;""><table border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" width=""100%""><tbody><tr><td style=""background-color:black;vertical-align:top;padding-bottom:8px;""><table border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" width=""100%""><tbody><tr><td align=""left"" class=""h2"" style=""font-size:0px;padding:20px;word-break:break-word;""><div style=""font-family:Ubuntu, Helvetica, Arial, sans-serif;font-size:32px;line-height:1;text-align:left;color:white;""><p style=""font-size: 20px; line-height: 1.5; color: #e3f6f5; text-align: justify; text-justify: inter-word;"">Please note that the verification link is valid for <b style=""color: skyblue; text-decoration: underline; text-transform: uppercase;"">" + expirationTimeInHours + @" hours</b>. After this time, you will need to request a new verification link.</p><hr style=""border-radius: 25px; background: linear-gradient(to left, #005aa7, #fffde4);"" width=""96%"" size=""6px""><p style=""font-size: 20px; line-height: 1.5; color: #e3f6f5; text-align: justify; text-justify: inter-word;"">If you did not create an account or have received this email in error, please ignore it.</p><p style=""font-size: 20px; line-height: 1.5; color: #e3f6f5; text-align: justify; text-justify: inter-word;"">If you have any questions or need assistance, please contact our support team at <b>" + supportEmail + @"</b>.</p><hr style=""border-radius: 25px; background: linear-gradient(to left, #005aa7, #fffde4);"" width=""96%"" size=""6px""><p style=""font-size: 18px; color: #bae8e8;  padding-top: 30px;"" class=""footer"">Thank you,</p><p style=""font-size: 18px; color: #bae8e8;"" class=""footer"">" + appName + @" Team</p></div></td></tr></tbody></table></td></tr></tbody></table></div><!--[if mso | IE]></td></tr></table><![endif]--></td></tr></tbody></table></div><!--[if mso | IE]></td></tr></table></td></tr></table><![endif]--></td></tr></tbody></table></div><!--[if mso | IE]></td></tr></table><![endif]--></div></body></html>";

            return new EmailMessage
            {
                Receiver = new MailboxAddress(username, receiver),
                Subject = "NextVer Account Confirmation - Please Verify Your Email",
                Content = messageBody,
                ContentType = TextFormat.Html
            };
        }
    }
}