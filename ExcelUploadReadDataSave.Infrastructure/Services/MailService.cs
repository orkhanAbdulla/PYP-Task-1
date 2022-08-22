
using ExcelUploadReadDataSave.Application.DTOs.ReportDto;
using ExcelUploadReadDataSave.Application.Services;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<MailService> _logger;
        public MailService(ISendGridClient sendGridClient, ILogger<MailService> logger)
        {
            _sendGridClient = sendGridClient;
            _logger = logger;
        }

        public async Task<string> SendFileAtchemenEmail(ReportAtchementlDto reportAtchementlDto)
        {
           string fromEmail=Configuration.SendGridEmailSetting("SendGridEmailSetting:FromEmail");
           string fromName=Configuration.SendGridEmailSetting("SendGridEmailSetting:FromName");

            var from = new EmailAddress(fromEmail, fromName);
              string subject = "File Attached Report";
            var to = new List<EmailAddress>();
            reportAtchementlDto.emailAddresses.ToList().ForEach(x => to.Add(new EmailAddress(x)));

           var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, to, subject, "Check Attached File", null);

            using (var fileStream = File.OpenRead(reportAtchementlDto.Atchement))
            {
                await msg.AddAttachmentAsync("file.xlsx", fileStream);
               
            }

            var response = await _sendGridClient.SendEmailAsync(msg);
            
            string message = response.IsSuccessStatusCode ? "Email Send" : "Email Sending failed";
            return message;
        }
    }
}
