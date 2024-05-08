using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CodenApp.Sdk.Infrastructure.Services
{
    public static class EmailServices
    {
        public static async Task SendEmail(SendGridMessage message, string token)
        {
            if(message == null)
                throw new ArgumentNullException(nameof(message));
                
            message.From = new EmailAddress("not-reply@codenapp.com", "Code n' App");
            var client = new SendGridClient(token);            
            var response = await client.SendEmailAsync(message);
        }
    }
}