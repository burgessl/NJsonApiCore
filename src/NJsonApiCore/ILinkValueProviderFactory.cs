using NJsonApi.Serialization.Representations.Resources;

namespace NJsonApi
{
    public interface ILinkValueProviderFactory
    {
        ILinkValueProvider Create(SingleResource resource);
    }
}
