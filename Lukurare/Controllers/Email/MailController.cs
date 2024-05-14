using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

[Route("Email/Mail")]
public class MailController : ControllerBase
{
    private readonly IMailService mailService;

    public MailController(IMailService mailService)
    {
        this.mailService = mailService;
    }

    [HttpPost("SendMail")]
    public async Task<IActionResult> SendMail([FromBody] MailRequest request) //[FromForm]
    {
        try
        {
            await mailService.SendEmailAsync(request);
            return Ok();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
