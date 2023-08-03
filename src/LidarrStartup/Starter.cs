using DryIoc;
using LidarrStartup.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NzbDrone.Common.EnvironmentInfo;
using NzbDrone.Common.Instrumentation;
using NzbDrone.Core.Configuration;
using NzbDrone.Host;
using Rralarr.Common;

namespace LidarrStartup;

public static class Starter
{
    public static IHostBuilder Create(string[] args)
    {
        var startupContext = new StartupContext(args)
        {
            Flags = {StartupContext.NO_BROWSER}
        };
        NzbDroneLogger.Register(startupContext, false, true);
        return Bootstrap.CreateConsoleHostBuilder(args, startupContext).ConfigureContainer<IContainer>(Configure);
    }
    
    private static void Configure(IContainer container)
    {
        container.Register<DownloadClientNotificationHandler>();
        container.Register<IConfigFileProvider, ExConfigFileProvider>();
        
    }
}