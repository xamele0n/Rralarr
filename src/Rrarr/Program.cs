// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
var encodings = Encoding.GetEncodings();
using var radarr = RadarrStartup.Starter.Create(args).Build();
using var sonarr = SonarrStartup.Starter.Create(args).Build();
using var lidarr = LidarrStartup.Starter.Create(args).Build();

var radarrHandler = radarr.Services.GetRequiredService<RadarrStartup.Common.DownloadClientNotificationHandler>();
var sonarrHandler = sonarr.Services.GetRequiredService<SonarrStartup.Common.DownloadClientNotificationHandler>();
var lidarrHandler = lidarr.Services.GetRequiredService<LidarrStartup.Common.DownloadClientNotificationHandler>();
using var prowlarr = ProwlarrStartup.Starter.Create(args, o =>
{
    o.OnAdded += (sender, notification) =>
    {
        try
        {
            radarrHandler.Handle(notification);
            sonarrHandler.Handle(notification);
            lidarrHandler.Handle(notification);
        }
        catch (Exception e)
        {
            Trace.WriteLine(e.ToString());
        }
    };
    o.OnUpdated += (sender, notification) =>
    {
        try
        {
            radarrHandler.Handle(notification);
            sonarrHandler.Handle(notification);
            lidarrHandler.Handle(notification);
        }
        catch (Exception e)
        {
            Trace.WriteLine(e.ToString());
        }
    };
    o.OnDeleted += (sender, notification) =>
    {
        try
        {
            radarrHandler.Handle(notification);
            sonarrHandler.Handle(notification);
            lidarrHandler.Handle(notification);
        }
        catch (Exception e)
        {
            Trace.WriteLine(e.ToString());
        }
    };
}).Build();

prowlarr.Services.GetRequiredService<IHostApplicationLifetime>().ApplicationStopping.Register(() =>
{
    lidarr.Services.GetRequiredService<IHostApplicationLifetime>().StopApplication();
    radarr.Services.GetRequiredService<IHostApplicationLifetime>().StopApplication();
    sonarr.Services.GetRequiredService<IHostApplicationLifetime>().StopApplication();
});
/// await Task.WhenAll(prowlarr.RunAsync(), Task.Run(() => radarr.RunAsync()), Task.Run(() => sonarr.RunAsync()), Task.Run(() => lidarr.RunAsync())).ConfigureAwait(false);
await Task.WhenAll(prowlarr.RunAsync(), radarr.RunAsync(), sonarr.RunAsync(), lidarr.RunAsync()).ConfigureAwait(false);
