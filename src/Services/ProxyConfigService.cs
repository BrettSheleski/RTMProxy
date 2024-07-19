using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Text.Json;

namespace RTMProxy.Services
{
    public class ProxyConfigService : IProxyConfigService
    {
        public ProxyConfigService(string configFile, JsonSerializerOptions? jsonSerializerOptions = null)
        {
            ConfigFile = configFile;
            JsonSerializerOptions = jsonSerializerOptions;
        }

        public string ConfigFile { get; }
        public JsonSerializerOptions? JsonSerializerOptions { get; }

        public async Task<ProxyConfig> GetAsync(CancellationToken cancellationToken = default)
        {
            ProxyConfig? config = null;

            if (File.Exists(ConfigFile))
            {
                await using (var stream = File.OpenRead(ConfigFile))
                {
                    config = await JsonSerializer.DeserializeAsync<ProxyConfig>(stream, cancellationToken: cancellationToken);
                }
            }

            if (config is null)
            {
                config = new ProxyConfig
                {
                    StreamUrl = new Uri("https://ntv1.akamaized.net/hls/live/2014075/NASA-NTV1-HLS/master.m3u8"),
                    HttpReferer = "https://www.nasa.gov/"
                };

                await SaveAsync(config, cancellationToken);
            }

            return config;
        }

        public async Task SaveAsync(ProxyConfig config, CancellationToken cancellationToken = default)
        {

            await using (var stream = new FileStream(ConfigFile, FileMode.Create))
            {
                await JsonSerializer.SerializeAsync(stream, config, options: this.JsonSerializerOptions, cancellationToken: cancellationToken);
            }
        }
    }
}
