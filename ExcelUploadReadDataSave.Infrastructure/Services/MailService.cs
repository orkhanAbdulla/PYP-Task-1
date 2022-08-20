using ExcelUploadReadDataSave.Application.DTOs.ReportDto;
using ExcelUploadReadDataSave.Application.Services;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelUploadReadDataSave.Infrastructure.Services
{
    public class MailService : IMailService
    {
        private readonly ISendGridClient _sendGridClient;
        public MailService(ISendGridClient sendGridClient, IConfiguration configuration, IFileManager fileManager)
        {
            _sendGridClient = sendGridClient;
        }

        public async Task<string> SendFileAtchemenEmail(ReportAtchementlDto reportAtchementlDto)
        {
           string fromEmail=Configuration.SendGridEmailSetting("SendGridEmailSetting:FromEmail");
           string fromName=Configuration.SendGridEmailSetting("SendGridEmailSetting:FromName");

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(fromEmail, fromName),
                Subject = "File Attached Report",
                PlainTextContent = "Check Attached File",
            };
            await msg.AddAttachmentAsync(
                reportAtchementlDto.Atchement.FileName,
                reportAtchementlDto.Atchement.OpenReadStream(),
                reportAtchementlDto.Atchement.ContentType,
                "attchement"
                ); ;  
            msg.AddTo(reportAtchementlDto.toEmail);

            var response = await _sendGridClient.SendEmailAsync(msg);

            string message = response.IsSuccessStatusCode ? "Email Send" : "Email Sending failed";
            return message;
        }
    }
}
