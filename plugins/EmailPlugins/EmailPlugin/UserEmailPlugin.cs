using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattElandsBlog.plugins.EmailPlugins.EmailPlugin
{
    public class UserEmailPlugin
    {
        [KernelFunction("send_email")]
        [Description("Sends an email to a recipient.")]
        public async Task SendEmailAsync(
        Kernel kernel,
        List<string> recipientEmails,
        string subject,
        string body
    )
        {
            // Add logic to send an email using the recipientEmails, subject, and body
            // For now, we'll just print out a success message to the console
            Console.WriteLine("----Email sent!----");
        }
    }
}
