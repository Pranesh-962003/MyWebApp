namespace BackendApi.Models
{
    public class EmailRequest
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = "Test Email";
        public string Body { get; set; } = "This is a test email.";
    }
}
