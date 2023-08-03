using MediatR;

namespace Rralarr.Common;

public record ProviderUpdatedNotification<TProvider>(TProvider Provider) : INotification
{
    public ProviderUpdatedNotification<T> ToProviderUpdatedNotification<T>()where T: new()
    {
        var t = ReflectionExtensions.DynamicCast<TProvider, T>(this.Provider);
        return new(t);
    }
}