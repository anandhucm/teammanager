using MailKit.Net.Smtp;
using MimeKit;
public class EmailService
{
    public async Task SendEmailAsync()
    {
        var email = new MimeMessage();
        email.From.Add(
            MailboxAddress.Parse("anandhumohanan97@gmail.com")
        );

        email.To.Add(
            MailboxAddress.Parse("anandhucm@shmsolutions.in")
        );

        email.Subject = "Welcome!";
        email.Body = new TextPart("plain")
        {
            Text = "Welcome to the Azure Functions App!"
        };

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync(
            "smtp.gmail.com",
            587,
            false
        );

        await smtp.AuthenticateAsync(
            "anandhumohanan97@gmail.com",
            "ycmo ixwb xbtr qieq"
        );

        await smtp.SendAsync(email);

        await smtp.DisconnectAsync(true);


    }
}
