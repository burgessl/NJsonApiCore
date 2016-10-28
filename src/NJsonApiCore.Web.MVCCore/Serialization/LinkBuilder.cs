using NJsonApi.Serialization;
using NJsonApi.Serialization.Representations;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Text;

namespace NJsonApi.Web.MVCCore.Serialization
{
    public class LinkBuilder : ILinkBuilder
    {
        private readonly IApiDescriptionGroupCollectionProvider descriptionProvider;

        private static readonly IDictionary<string, RouteTemplate> cachedTemplates = new Dictionary<string, RouteTemplate>();
        private static readonly IDictionary<LinkId, ILink> selfLinks = new Dictionary<LinkId, ILink>();
        private static readonly IDictionary<Type, ApiDescription> actionCache = new Dictionary<Type, ApiDescription>();
        private static readonly IDictionary<string, Uri> uriCache = new Dictionary<string, Uri>();

        public LinkBuilder(IApiDescriptionGroupCollectionProvider descriptionProvider)
        {
            this.descriptionProvider = descriptionProvider;
        }

        public ILink FindResourceSelfLink(Context context, string resourceId, IResourceMapping resourceMapping)
        {
            ApiDescription action;

            if (!actionCache.TryGetValue(resourceMapping.Controller, out action))
            {
                var actions = descriptionProvider.From(resourceMapping.Controller).Items;
                action = actions.Single(a =>
                    a.HttpMethod == "GET" &&
                    a.ParameterDescriptions.Count(p => p.Name == "id") == 1);
                actionCache[resourceMapping.Controller] = action;
            }

            ILink result;

            var linkId = new LinkId(resourceMapping.ResourceType, resourceId);
            if (!selfLinks.TryGetValue(linkId, out result))
            {
                var values = new Dictionary<string, object>();
                values.Add("id", resourceId);
                result = ToUrl(context, action, values);
                selfLinks[linkId] = result;
            }

            return result;
        }

        public ILink RelationshipRelatedLink(Context context, string resourceId, IResourceMapping resourceMapping, IRelationshipMapping linkMapping)
        {
            var selfLink = FindResourceSelfLink(context, resourceId, resourceMapping).Href;
            var completeLink = $"{selfLink}/{linkMapping.RelationshipName}";
            return new SimpleLink(completeLink);
        }

        public ILink RelationshipSelfLink(Context context, string resourceId, IResourceMapping resourceMapping, IRelationshipMapping linkMapping)
        {
            var selfLink = FindResourceSelfLink(context, resourceId, resourceMapping).Href;
            var completeLink = $"{selfLink}/relationships/{linkMapping.RelationshipName}";
            return new SimpleLink(completeLink);
        }

        // TODO replace with UrlHelper method once RC2 has been released
        private SimpleLink ToUrl(Context context, ApiDescription action, Dictionary<string, object> values)
        {
            RouteTemplate template;
            var relativePath = action.RelativePath;
            if (!cachedTemplates.TryGetValue(relativePath, out template))
            {
                template = TemplateParser.Parse(action.RelativePath);
                cachedTemplates[relativePath] = template;
            }


            var result = FillTemplate(action, template, values);

            return new SimpleLink($"{context.BaseUri}/{result}");
        }

        private static string FillTemplate(ApiDescription action, RouteTemplate template, IDictionary<string, object> values)
        {
            var sb = new StringBuilder();
            sb.Append(action.RelativePath.ToLowerInvariant());

            foreach (var parameter in template.Parameters)
            {
                var value = values[parameter.Name];
                sb.Replace(parameter.ToPlaceholder(), value.ToString());
            }

            return sb.ToString();
        }
    }

    struct LinkId
    {
        public LinkId(string type, string id)
        {
            ResourceType = type;
            ResourceId = id;
        }

        public string ResourceType { get; }

        public string ResourceId { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, this))
            {
                return true;
            }

            if (obj.GetType() == typeof(LinkId))
            {

                var other = (LinkId)obj;
                if (ReferenceEquals(null, other))
                {
                    return false;
                }


                return ResourceId.Equals(other.ResourceId) && ResourceType.Equals(other.ResourceType);
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = hash * 16777619 + ResourceType.GetHashCode();
                hash = hash * 16777619 + ResourceId.GetHashCode();
                return hash;
            }
        }
    }
}