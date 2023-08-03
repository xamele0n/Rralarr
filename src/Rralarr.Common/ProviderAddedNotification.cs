using Mapster;
using MediatR;

namespace Rralarr.Common;

public record ProviderAddedNotification<TProvider>(TProvider Provider) : INotification
{

    public ProviderAddedNotification<T> ToProviderAddedNotification<T>()
    {
        var t = ReflectionExtensions.DynamicCast<TProvider, T>(this.Provider);
        return new(t);
    }
}