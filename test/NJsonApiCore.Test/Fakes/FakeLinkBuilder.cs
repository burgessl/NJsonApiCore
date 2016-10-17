using NJsonApi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NJsonApi.Serialization.Representations;

namespace NJsonApi.Test.Fakes
{
    internal class FakeLinkBuilder : ILinkBuilder
    {
        public ILink FindResourceSelfLink(Context context, ILinkValueProvider valueProvider, IResourceMapping resourceMapping)
        {
            return new SimpleLink(new Uri("http://example.com"));
        }

        public ILink RelationshipRelatedLink(Context context, ILinkValueProvider valueProvider, IResourceMapping resourceMapping, IRelationshipMapping linkMapping)
        {
            return new SimpleLink(new Uri("http://example.com"));
        }

        public ILink RelationshipSelfLink(Context context, ILinkValueProvider valueProvider, IResourceMapping resourceMapping, IRelationshipMapping linkMapping)
        {
            return new SimpleLink(new Uri("http://example.com"));
        }
    }
}
