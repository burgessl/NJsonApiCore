using NJsonApi.Serialization.Representations;

namespace NJsonApi.Serialization
{
    public interface ILinkBuilder
    {
        ILink FindResourceSelfLink(Context context, ILinkValueProvider linkValueProvider, IResourceMapping resourceMapping);

        ILink RelationshipSelfLink(Context context, ILinkValueProvider linkValueProvider, IResourceMapping resourceMapping, IRelationshipMapping linkMapping);

        ILink RelationshipRelatedLink(Context context, ILinkValueProvider linkValueProvider, IResourceMapping resourceMapping, IRelationshipMapping linkMapping);
    }
}