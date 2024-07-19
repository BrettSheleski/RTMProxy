using System.Diagnostics;

namespace RTMProxy.Services
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class NginxService : INginxService
    {
        private const string Shell = "/bin/sh";

        public async Task<bool> IsRunningAsync(CancellationToken cancellationToken = default)
        {

            try
            {
                using (var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = Shell,
                        Arguments = "-c \"ps aux | grep nginx | grep -v grep\"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    }
                })
                {
                    process.Start();
                    string result = await process.StandardOutput.ReadToEndAsync(cancellationToken);
                    await process.WaitForExitAsync(cancellationToken);

                    // Check if the output contains "nginx: master process" or similar
                    return result.Contains("nginx: master process");
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task RestartAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                // Stop Nginx
                await StopAsync(cancellationToken);

                // Wait for a short period to ensure Nginx is completely stopped
                await Task.Delay(1000, cancellationToken); // Adjust the delay if necessary

                // Start Nginx
                await StartAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Handle the cancellation of the operation
                throw new OperationCanceledException("Restart operation was cancelled.");
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                throw new ApplicationException("Failed to restart Nginx.", ex);
            }
        }
        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                using (var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = Shell,
                        Arguments = "-c \"nginx\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    }
                })
                {
                    process.Start();
                    await process.StandardOutput.ReadToEndAsync(cancellationToken);
                    await process.StandardError.ReadToEndAsync(cancellationToken);
                    await process.WaitForExitAsync(cancellationToken);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception as needed, possibly logging it or rethrowing it
                throw new ApplicationException("Failed to start Nginx.", ex);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                using (var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = Shell,
                        Arguments = "-c \"nginx -s stop\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    }
                })
                {
                    process.Start();
                    await process.StandardOutput.ReadToEndAsync(cancellationToken);
                    await process.StandardError.ReadToEndAsync(cancellationToken);
                    await process.WaitForExitAsync(cancellationToken);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception as needed, possibly logging it or rethrowing it
                throw new ApplicationException("Failed to stop Nginx.", ex);
            }
        }
    }
}