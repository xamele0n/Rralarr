using MediatR;
using NzbDrone.Core.Download;
using NzbDrone.Core.ThingiProvider;
using Rralarr.Common;

namespace SonarrStartup.Common;

public abstract class ProviderSyncNotificationHandler<T, TDef>
    where TDef : ProviderDefinition, new()
    where T : IProvider
{
    private readonly IProviderFactory<T, TDef> _providerFactory;

    protected ProviderSyncNotificationHandler(IProviderFactory<T, TDef> providerFactory)
    {
        _providerFactory = providerFactory;
    }

    public void Handle(ProviderAddedNotification<TDef> notification, CancellationToken cancellationToken = default)
    {
        notification.Provider.Id = 0;
        _providerFactory.Create(notification.Provider);
    }

    public void Handle(ProviderUpdatedNotification<TDef> notification, CancellationToken cancellationToken = default)
    {
        var existing = _providerFactory.All().FirstOrDefault(x => x.Name == notification.Provider.Name);
        if (existing is not null)
        {
            notification.Provider.Id = existing.Id;
        }
        _providerFactory.Update(notification.Provider);
    }

    public void Handle(ProviderDeletedNotification<TDef> notification, CancellationToken cancellationToken = default)
    {
        var existing = _providerFactory.Find(notification.Provider);
        if (existing is not null)
        {
            _providerFactory.Delete(existing.Id);
        }
    }
    
    public void Handle<T>(ProviderAddedNotification<T> evt) => this.Handle(evt.ToProviderAddedNotification<TDef>(), CancellationToken.None);
    public void Handle<T>(ProviderDeletedNotification<T> evt) => this.Handle(evt.ToProviderDeletedNotification<TDef>(), CancellationToken.None);
    public void Handle<T>(ProviderUpdatedNotification<T> evt) => this.Handle(evt.ToProviderUpdatedNotification<TDef>(), CancellationToken.None);
}


public class DownloadClientNotificationHandler: ProviderSyncNotificationHandler<IDownloadClient, DownloadClientDefinition>
{
    private readonly IProviderFactory<IDownloadClient, DownloadClientDefinition> _providerFactory;


    public DownloadClientNotificationHandler(IProviderFactory<IDownloadClient, DownloadClientDefinition> providerFactory) : base(providerFactory)
    {
    }
}