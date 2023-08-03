using Microsoft.Extensions.Options;
using NzbDrone.Common.Cache;
using NzbDrone.Common.Disk;
using NzbDrone.Common.EnvironmentInfo;
using NzbDrone.Core.Datastore;
using NzbDrone.Core.Messaging.Events;

namespace NzbDrone.Core.Configuration;

internal class ExConfigFileProvider: ConfigFileProvider, IConfigFileProvider
{
    string IConfigFileProvider.UiFolder => GetValue(nameof(UiFolder), "sonarrUI", true);

    public ExConfigFileProvider(IAppFolderInfo appFolderInfo, ICacheManager cacheManager, IEventAggregator eventAggregator, IDiskProvider diskProvider) : base(appFolderInfo, cacheManager, eventAggregator, diskProvider)
    {
    }
    
    string IConfigFileProvider.UrlBase
    {
        get
        {
            var urlBase = GetValue("UrlBase", "").Trim('/');

            if (string.IsNullOrWhiteSpace(urlBase))
            {
                return urlBase;
            }

            return "/" + urlBase.Trim('/').ToLower();
        }
    }
    
    string IConfigFileProvider.ApiKey
    {
        get
        {
            var key = AppContext.GetData(nameof(ApiKey)) as string;
            if (!string.IsNullOrWhiteSpace(key))
                return key;
            key =  base.ApiKey;
#if NET7_0
            AppContext.SetData(nameof(ApiKey), key);
#else         
            AppDomain.CurrentDomain.SetData(nameof(ApiKey), key);
#endif   
            
            return key;
        }
    }
}