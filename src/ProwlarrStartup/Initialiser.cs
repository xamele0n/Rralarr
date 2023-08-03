using NzbDrone.Core.Applications;
using NzbDrone.Core.Applications.Lidarr;
using NzbDrone.Core.Applications.Radarr;
using NzbDrone.Core.Applications.Sonarr;
using NzbDrone.Core.Configuration;
using NzbDrone.Core.ThingiProvider;

namespace ProwlarrStartup;

public class Initialiser
{
    private readonly IProviderFactory<IApplication, ApplicationDefinition> _providerFactory;
    private readonly IConfigFileProvider _config;

    public Initialiser(IProviderFactory<IApplication,ApplicationDefinition> providerFactory, IConfigFileProvider config)
    {
        _providerFactory = providerFactory;
        _config = config;
    }

    public void Init()
    {
        _providerFactory.Create(new ApplicationDefinition
        {
            Enable = true,
            Implementation = nameof(Radarr),
            Name = nameof(Radarr),
            ImplementationName = nameof(Radarr),
            SyncLevel = ApplicationSyncLevel.FullSync,
            Settings = new RadarrSettings
            {
                ApiKey = _config.ApiKey,
            }
        });
        _providerFactory.Create(new ApplicationDefinition
        {
            Enable = true,
            Implementation = nameof(Sonarr),
            Name = nameof(Sonarr),
            ImplementationName = nameof(Sonarr),
            SyncLevel = ApplicationSyncLevel.FullSync,
            Settings = new SonarrSettings()
            {
                ApiKey = _config.ApiKey,
            }
        });
        _providerFactory.Create(new ApplicationDefinition
        {
            Enable = true,
            Implementation = nameof(Lidarr),
            Name = nameof(Lidarr),
            ImplementationName = nameof(Lidarr),
            SyncLevel = ApplicationSyncLevel.FullSync,
            Settings = new LidarrSettings()
            {
                ApiKey = _config.ApiKey,
            }
        });
    }
}