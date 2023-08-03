namespace Rralarr.Common;

public class RralarrOptions<TProvider>
{
    public event EventHandler<ProviderAddedNotification<TProvider>> OnAdded;
    public event EventHandler<ProviderDeletedNotification<TProvider>> OnDeleted;
    public event EventHandler<ProviderUpdatedNotification<TProvider>> OnUpdated;

    public void RiseOnAdded(object sender, TProvider provider) =>
        OnAdded(sender, new ProviderAddedNotification<TProvider>(provider));
    
    public void RiseOnDeleted(object sender, int provider) =>
        OnDeleted(sender, new ProviderDeletedNotification<TProvider>(provider));
    
    public void RiseOnUpdated(object sender, TProvider provider) =>
        OnUpdated(sender, new ProviderUpdatedNotification<TProvider>(provider));
}