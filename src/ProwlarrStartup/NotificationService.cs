using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NzbDrone.Core.Download;
using NzbDrone.Core.Messaging.Events;
using NzbDrone.Core.ThingiProvider;
using NzbDrone.Core.ThingiProvider.Events;
using Rralarr.Common;

namespace ProwlarrStartup;

public abstract class NotificationService<TProvider, TDef> : IHandle<ProviderAddedEvent<TProvider>>,
    IHandle<ProviderUpdatedEvent<TProvider>>,
    IHandle<ProviderDeletedEvent<TProvider>>
    where TDef : ProviderDefinition

{
    protected readonly ILogger _logger;
    protected readonly RralarrOptions<TDef> _mediator;

    protected NotificationService(IOptions<RralarrOptions<TDef>> options,
        ILogger logger) : this(options.Value)
    {
        _logger = logger;
    }

    protected NotificationService(RralarrOptions<TDef> options)
    {
        _mediator = options;
    }

    public virtual void Handle(ProviderAddedEvent<TProvider> message)
    {
        try
        {
            _mediator.RiseOnAdded(this, (TDef) message.Definition);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
    }

    public virtual void Handle(ProviderUpdatedEvent<TProvider> message)
    {
        try
        {
            _mediator.RiseOnUpdated(this, (TDef) message.Definition);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
    }

    public abstract void Handle(ProviderDeletedEvent<TProvider> message);
}

public sealed class DownloadClientNotificationService : NotificationService<IDownloadClient, DownloadClientDefinition>
{
    private readonly IProviderFactory<IDownloadClient, DownloadClientDefinition> _providerFactory;

    public DownloadClientNotificationService(IOptions<RralarrOptions<DownloadClientDefinition>> options, ILogger<DownloadClientNotificationService> logger,
        IProviderFactory<IDownloadClient, DownloadClientDefinition> providerFactory) : base(options, logger)
    {
        _providerFactory = providerFactory;
    }

    public override void Handle(ProviderDeletedEvent<IDownloadClient> message)
    {
        try
        {
            _mediator.RiseOnDeleted(this, message.ProviderId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
    }
}