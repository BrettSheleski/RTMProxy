using System.ComponentModel.DataAnnotations;

namespace RTMProxy
{
    public class ProxyConfig
    {
        [Required, Display(Name = "Stream URL")]
        public Uri StreamUrl { get; set; }

        [Display(Name = "HTTP Referer")]
        public string? HttpReferer { get; set; }
    }

}
