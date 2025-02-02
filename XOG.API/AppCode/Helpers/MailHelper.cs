﻿using XOG.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Mail;
using System.Threading.Tasks;
using XOG.SettingsHelpers;
using XOG.Areas.MyAdmin.BL;
using System.IO;
using System.Net;
using XOG.Util;

namespace XOG.Helpers
{
    public static class MailHelper
    {
        public static string CleanUpPlaceHolders(string body, int lastCount)
        {
            var maxCount = KeywordsHelper.GetKeywordValue("MaxPlaceHoldersCount").IntParse();

            for (var xCount = lastCount + 1; xCount < maxCount; xCount++)
            {
                var placeHolder = string.Format("[PLACEHOLDER{0}]", xCount);

                body = body.Replace(placeHolder, string.Empty);
            }

            return body;
        }

        public static void GetAttachments(List<Attachment> attachments, MailMessage msg)
        {
            attachments.ForEach(msg.Attachments.Add);
        }

        public static string GetEmailTemplateFromDataBase(EmailType emailFor)
        {
            return EmailTemplateBL.GetEmailMarkup(emailFor);
        }

        public static string GetEmailTemplateFromDataBase(string templateName)
        {
            var body = string.Empty;

            return body;
        }

        public static Task<string> GetEmailTemplateFromDataBaseAsync(EmailType emailFor)
        {
            return EmailTemplateBL.GetEmailMarkupAsync(emailFor);
        }

        public static string GetEmailTemplateFromFile(string templatePath)
        {
            var body = string.Empty;

            using (var reader = new StreamReader(templatePath))
            {
                body = reader.ReadToEnd();
            }

            return body;
        }

        public static MailMessage GetMessage(IdentityMessage message)
        {
            var msg = new MailMessage
            {
                From = new MailAddress(AppConfig.WebsiteMainEmail)
            };
            msg.To.Add(message.Destination);

            msg.Subject = message.Subject;

            msg.Body = message.Body;

            msg.IsBodyHtml = true;

            msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;

            msg.Priority = MailPriority.Normal;

            return msg;
        }

        public static SendGridSettings GetSendGridSettings()
        {
            return new SendGridSettings
            {
                SenderEmail = AppConfig.WebsiteMainEmail,
                SenderName = AppConfig.SiteName,
                SendGridApiKey = AppConfig.SendGridAPIKey
            };
        }

        public static void Send(IdentityMessage message)
        {
            switch (AppConfig.EmailServiceName)
            {
                case "SENDGRID":
                    SendUsingSendGrid(message);
                    break;

                case "SMTP":
                    SendUsingSmtp(message);
                    break;

                case "RELAY":
                    SendUsingRelay(message);
                    break;
            }
        }

        public static Task SendAsync(IdentityMessage message)
        {
            switch (AppConfig.EmailServiceName)
            {
                case "SENDGRID":
                    return SendUsingSendGridAsync(message);

                case "SMTP":
                    return SendUsingSmtpAsync(message, null);

                case "RELAY":
                    return SendUsingRelayAsync(message, null);
            }

            return Task.FromResult(0);
        }

        public static void SendUsingRelay(IdentityMessage message)
        {
            var msg = GetMessage(message);

            using (var smtp = new SmtpClient())
            {
                smtp.Send(msg);
            }

            msg.Dispose();
        }

        public static Task SendUsingRelayAsync(IdentityMessage message, object token)
        {
            var msg = GetMessage(message);

            using (var smtp = new SmtpClient())
            {
                smtp.SendCompleted += smtp_SendCompleted;

                token = msg;

                return Task.Run(() => smtp.SendAsync(msg, token));
            }
        }

        public static void SendUsingRelayWithAttachments(IdentityMessage message, List<Attachment> attachments)
        {
            var msg = GetMessage(message);

            GetAttachments(attachments, msg);

            using (var smtp = new SmtpClient())
            {
                smtp.Send(msg);
            }

            msg.Dispose();
        }

        public static Task SendUsingRelayWithAttachmentsAsync(IdentityMessage message, List<Attachment> attachments, object token)
        {
            var msg = GetMessage(message);

            GetAttachments(attachments, msg);

            using (var smtp = new SmtpClient())
            {
                smtp.SendCompleted += smtp_SendCompleted;

                token = msg;

                return Task.Run(() => smtp.SendAsync(msg, token));
            }
        }

        public static bool SendUsingSendGrid(IdentityMessage message)
        {
            throw new System.Exception();
            //var sendgridSettings = GetSendGridSettings();
            //return null;// SendgridEmailSender.SendMail(sendgridSettings, message.Destination, message.Subject, message.Body);
        }

        public static Task<bool> SendUsingSendGridAsync(IdentityMessage message)
        {
            throw new System.Exception();
            //var sendgridSettings = GetSendGridSettings();
            //return SendgridEmailSender.SendMailAsync(sendgridSettings, message.Destination, message.Subject, message.Body);
        }

        public static void SendUsingSmtp(IdentityMessage message)
        {
            var msg = GetMessage(message);

            using (var smtp = new SmtpClient(AppConfig.MailServer))
            {
                smtp.EnableSsl = AppConfig.EnableSsl;

                smtp.Credentials = new NetworkCredential(AppConfig.MailLogOnId, AppConfig.MailLogOnPassword);

                smtp.Port = AppConfig.MailServerPort;

                smtp.Send(msg);
            }

            msg.Dispose();
        }

        public static Task SendUsingSmtpAsync(IdentityMessage message, object token)
        {
            var msg = GetMessage(message);

            var smtp = new SmtpClient(AppConfig.MailServer);

            smtp.EnableSsl = AppConfig.EnableSsl;


            smtp.Credentials = new NetworkCredential(AppConfig.MailLogOnId, "51435143Ak");

            smtp.Port = AppConfig.MailServerPort;

            smtp.UseDefaultCredentials = false;

            smtp.SendCompleted += smtp_SendCompleted;

            token = msg;

            return Task.Run(() => smtp.SendAsync(msg, token));
        }

        public static void SendUsingSmtpWithAttachments(IdentityMessage message, List<Attachment> attachments)
        {
            var msg = GetMessage(message);

            GetAttachments(attachments, msg);

            using (var smtp = new SmtpClient(AppConfig.MailServer))
            {
                smtp.EnableSsl = AppConfig.EnableSsl;

                smtp.Credentials = new NetworkCredential(AppConfig.MailLogOnId, AppConfig.MailLogOnPassword);

                smtp.Port = AppConfig.MailServerPort;

                smtp.Send(msg);
            }

            msg.Dispose();
        }

        public static Task SendUsingSmtpWithAttachmentsAsync(IdentityMessage message, List<Attachment> attachments, object token)
        {
            var msg = GetMessage(message);

            GetAttachments(attachments, msg);

            using (var smtp = new SmtpClient(AppConfig.MailServer)
            {
                EnableSsl = AppConfig.EnableSsl,

                Credentials = new NetworkCredential(AppConfig.MailLogOnId, AppConfig.MailLogOnPassword),

                Port = AppConfig.MailServerPort
            })
            {
                smtp.SendCompleted += smtp_SendCompleted;

                token = msg;

                return Task.Run(() => smtp.SendAsync(msg, token));
            }
        }

        public static void smtp_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //var StatusMessage = e.UserState as StatusMessageJQuery;

            //if (e.Error != null)
            //{
            //    StatusMessage.MessageType = StatusMessageType.Error;
            //    StatusMessage.Message = ExceptionHelper.GetExceptionMessage(e.Error);
            //}
        }
    }
}
