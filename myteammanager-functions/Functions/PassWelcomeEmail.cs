using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;


public class PassWelcomeEmail
{
    private readonly EmailService _emailService;

    public PassWelcomeEmail(EmailService emailService)
    {
        _emailService = emailService;
    }


    [Function("PassWelcomeEmail")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
    {
        await _emailService.SendEmailAsync();
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteStringAsync(
            "Email Sent"
        );
        return response;
    }

}