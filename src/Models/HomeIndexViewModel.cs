
using RTMProxy.Services;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace RTMProxy.Models
{
    public class HomeIndexViewModel
    {
        [Required]
        public ProxyConfig Config { get; set; }
    }

}
