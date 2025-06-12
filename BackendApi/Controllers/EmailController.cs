using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using BackendApi.Models;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {


        [HttpGet("Check-email")]

        public IActionResult check()
        {
            return Ok("working Email route");
        }

        [HttpPost("send")]
        public IActionResult SendEmail([FromBody] EmailRequest request)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com") // e.g., smtp.gmail.com
                {
                    Port = 587,
                    Credentials = new NetworkCredential("pranesh962003.p@gmail.com", "kdmpmtkhjkdxlfna"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("pranesh962003.p@gmail.com"),
                    Subject = request.Subject,
                    Body = request.Body,
                    IsBodyHtml = false,
                };
                mailMessage.To.Add(request.To);

                smtpClient.Send(mailMessage);

                return Ok("Email sent!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error sending email: " + ex.Message);
            }
        }
    }
}
