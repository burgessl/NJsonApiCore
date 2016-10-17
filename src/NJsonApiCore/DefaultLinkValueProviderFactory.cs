using System;
using NJsonApi.Serialization.Representations.Resources;

namespace NJsonApi
{
    public class DefaultLinkValueProviderFactory : ILinkValueProviderFactory
    {
        public ILinkValueProvider Create(SingleResource resource)
        {
            return new DefaultLinkValueProvider(resource);
        }
    }
}
