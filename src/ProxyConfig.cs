using System.ComponentModel.DataAnnotations;

namespace RTMProxy
{
    public record ProxyConfig([Required]Uri StreamUrl, string? HttpReferer = null);
}
