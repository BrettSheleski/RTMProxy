namespace RTMProxy.Services
{

    public interface INginxService
    {
        Task<bool> IsRunningAsync(CancellationToken cancellationToken = default);
        Task StartAsync(CancellationToken cancellationToken = default);
        Task StopAsync(CancellationToken cancellationToken = default);
        Task RestartAsync(CancellationToken cancellationToken = default);

    }
}
