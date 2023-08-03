using DryIoc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NzbDrone.Common.EnvironmentInfo;
using NzbDrone.Common.Instrumentation;
using NzbDrone.Core.Configuration;
using NzbDrone.Core.Download;
using NzbDrone.Host;
using Rralarr.Common;

namespace ProwlarrStartup;

public static class Starter
{

    public static IHostBuilder Create(string[] args, Action<RralarrOptions<DownloadClientDefinition>> config)
    {
        var startupContext = new StartupContext(args)
        {
            Flags = {StartupContext.NO_BROWSER}
        };
        NzbDroneLogger.Register(startupContext, false, true);
        return Bootstrap.CreateConsoleHostBuilder(args, startupContext).ConfigureServices(s =>
            {
                s.Configure(config);
            })
            .ConfigureContainer<IContainer>(Configure);
    }
    
    private static void Configure(IContainer container)
    {
        container.Register<IConfigFileProvider, ExConfigFileProvider>();
        var assemblies = new[] {typeof(Starter).Assembly};
        container.RegisterMany(assemblies,
            serviceTypeCondition: type => type.IsInterface && !string.IsNullOrWhiteSpace(type.FullName) && !type.FullName.StartsWith("System"),
            reuse: Reuse.Singleton);

        container.RegisterMany(assemblies,
            serviceTypeCondition: type => !type.IsInterface && !string.IsNullOrWhiteSpace(type.FullName) && !type.FullName.StartsWith("System"),
            reuse: Reuse.Transient);
        
        container.RegisterMany<DownloadClientNotificationService>();
    }
}