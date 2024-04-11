using System.Security.Policy;

namespace FlagX0.Web.Models
{
    public class FlagViewModel
    {
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public int? Id { get; set; }
        public string? Error { get; set; }
    }
}
