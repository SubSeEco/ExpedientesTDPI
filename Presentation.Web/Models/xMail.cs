using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Domain.Infrastructure;
using Infrastructure.Logging;
using System.Net;
using System.Threading;

namespace Presentation.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class xMail
    {
        /// <summary>
        /// 
        /// </summary>
        public string Destinatario { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Asunto { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Destinatarios { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> ConCopia { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> Attachments { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public xMail()
        {
            ConCopia = new List<string>();
            Attachments = new List<string>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Enviar()
        {
            Thread thread = new Thread(() => SendMail(Asunto, Body, Destinatario, ConCopia, Attachments));
            thread.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetDestinatarios()
        {
            string copia = String.Join(", ", ConCopia.ToArray());
            string dest = "To: " + Destinatario + " | CC: " + copia;

            if (Attachments.Count > 0)
            {
                dest += " | Adjuntos: " + String.Join(", ", Attachments.ToArray());
            }

            return dest;
        }

        private static void SendMail(string subject, string body, string recipient, List<string> ConCopia, List<string> Attachments)
        {
            string smtpAddress = WebConfigValues.SmtpClient_Host;
            int portNumber = WebConfigValues.SmtpClient_Port;
            bool enableSSL = WebConfigValues.SmtpClient_EnableSsl;

            string emailFrom = WebConfigValues.SmtpClient_User;
            string password = WebConfigValues.SmtpClient_Password;

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFrom, WebConfigValues.Mail_Name, System.Text.Encoding.UTF8);
                mail.To.Add(recipient);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                mail.SubjectEncoding = System.Text.Encoding.UTF8;

                foreach (var item in ConCopia)
                {
                    mail.CC.Add(new MailAddress(item));
                }

                foreach (var item in Attachments)
                {
                    mail.Attachments.Add(new Attachment(item));
                }



                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(emailFrom, password);
                    smtp.EnableSsl = enableSSL;

                    try
                    {
                        smtp.Send(mail);
                    }
                    catch (System.Exception ex)
                    {
                        Logger.Execute().Error(ex);
                    }

                }
            }
        }
    }
}
