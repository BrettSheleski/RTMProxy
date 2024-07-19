using System.Text.Json;

namespace RTMProxy.Services
{
    public class ProxyConfigService : IProxyConfigService
    {
        public ProxyConfigService(FileInfo configFile, JsonSerializerOptions? jsonSerializerOptions = null)
        {
            ConfigFile = configFile;
            JsonSerializerOptions = jsonSerializerOptions;
        }

        public FileInfo ConfigFile { get; }
        public JsonSerializerOptions? JsonSerializerOptions { get; }

        public async Task<ProxyConfig> GetAsync(CancellationToken cancellationToken = default)
        {
            ConfigFile.Refresh();

            ProxyConfig? config = null;

            if (ConfigFile.Exists)
            {
                await using (var stream = ConfigFile.OpenRead())
                {
                    config = await System.Text.Json.JsonSerializer.DeserializeAsync<ProxyConfig>(stream, cancellationToken: cancellationToken);
                }
            }

            if (config is null)
            {
                config = new ProxyConfig(
                    StreamUrl: new Uri("https://ntv1.akamaized.net/hls/live/2014075/NASA-NTV1-HLS/master.m3u8"),
                    HttpReferer: "https://www.nasa.gov/"
                    );

                await SaveAsync(config, cancellationToken);
            }

            return config;
        }

        public async Task SaveAsync(ProxyConfig config, CancellationToken cancellationToken = default)
        {
            ConfigFile.Refresh();

            await using (var stream = ConfigFile.OpenWrite())
            {
                await System.Text.Json.JsonSerializer.SerializeAsync(stream, config, options: this.JsonSerializerOptions, cancellationToken: cancellationToken);
            }
        }
    }
}
