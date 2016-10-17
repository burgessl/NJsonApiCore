using System;
using Microsoft.Extensions.DependencyInjection;
using NJsonApi.Serialization.Representations.Resources;

namespace NJsonApi
{
    public class DefaultLinkValueProvider : ILinkValueProvider
    {
        private readonly SingleResource resource;

        public DefaultLinkValueProvider(SingleResource resource)
        {
            this.resource = resource;
        }

        public bool TryGetValue(string parameterName, out object value)
        {
            if ("id".Equals(parameterName, StringComparison.OrdinalIgnoreCase))
            {
                value = resource.Id;
                return true;
            }

            return resource.Attributes.TryGetValue(parameterName, out value);
        }
    }
}
