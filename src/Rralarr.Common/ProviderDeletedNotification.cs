using MediatR;

namespace Rralarr.Common;

public record ProviderDeletedNotification<TProvider>(int Provider) : INotification
{
    public ProviderDeletedNotification<T> ToProviderDeletedNotification<T>()
    {
      //  var t = ReflectionExtensions.DynamicCast<TProvider, T>(this.Provider);
        return new(this.Provider);
    }
}