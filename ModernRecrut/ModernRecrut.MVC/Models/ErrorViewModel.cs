namespace ModernRecrut.MVC.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public string StatusCode { get; set; }
        public string ErrorMsg { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
