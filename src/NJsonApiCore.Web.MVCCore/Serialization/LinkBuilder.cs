using NJsonApi.Serialization;
using NJsonApi.Serialization.Representations;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Routing.Template;

namespace NJsonApi.Web.MVCCore.Serialization
{
    public class LinkBuilder : ILinkBuilder
    {
        private readonly IApiDescriptionGroupCollectionProvider descriptionProvider;

        public LinkBuilder(IApiDescriptionGroupCollectionProvider descriptionProvider)
        {
            this.descriptionProvider = descriptionProvider;
        }

        public ILink FindResourceSelfLink(Context context, ILinkValueProvider linkValueProvider, IResourceMapping resourceMapping)
        {
            var actions = descriptionProvider.From(resourceMapping.Controller).Items;

            var action = actions.Single(a =>
                a.HttpMethod == "GET" &&
                a.ParameterDescriptions.Count(p => p.Name == "id" && !p.RouteInfo.IsOptional) == 1);

            return ToUrl(context, action, linkValueProvider);
        }

        public ILink RelationshipRelatedLink(Context context, ILinkValueProvider linkValueProvider, IResourceMapping resourceMapping, IRelationshipMapping linkMapping)
        {
            var selfLink = FindResourceSelfLink(context, linkValueProvider, resourceMapping).Href;
            var completeLink = $"{selfLink}/{linkMapping.RelationshipName}";
            return new SimpleLink(new Uri(completeLink));
        }

        public ILink RelationshipSelfLink(Context context, ILinkValueProvider linkValueProvider, IResourceMapping resourceMapping, IRelationshipMapping linkMapping)
        {
            var selfLink = FindResourceSelfLink(context, linkValueProvider, resourceMapping).Href;
            var completeLink = $"{selfLink}/relationships/{linkMapping.RelationshipName}";
            return new SimpleLink(new Uri(completeLink));
        }

        // TODO replace with UrlHelper method once RC2 has been released
        private SimpleLink ToUrl(Context context, ApiDescription action, ILinkValueProvider linkValueProvider)
        {
            var template = TemplateParser.Parse(action.RelativePath);
            var result = action.RelativePath.ToLowerInvariant();

            foreach (var parameter in template.Parameters)
            {
                object value;
                if (linkValueProvider.TryGetValue(parameter.Name, out value))
                {
                    result = result.Replace(parameter.ToPlaceholder(), value.ToString());
                }
                else
                {
                    throw new InvalidOperationException($"unknown parameter '{parameter.Name}'");
                }
            }

            return new SimpleLink(new Uri(context.BaseUri, result));
        }
    }
}