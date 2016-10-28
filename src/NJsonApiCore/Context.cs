using System;

namespace NJsonApi
{
    public class Context
    {
        private readonly string baseUri;

        public Context(Uri requestUri)
            :this(requestUri, new string[0])
        {
        }

        public Context(Uri requestUri, string[] includedResources)
        {
            RequestUri = requestUri;
            IncludedResources = includedResources;
            var authority = (UriComponents.Scheme | UriComponents.UserInfo | UriComponents.Host | UriComponents.Port);
            baseUri = new Uri(RequestUri.GetComponents(authority, UriFormat.SafeUnescaped)).AbsoluteUri;
        }


        public Uri RequestUri { get; private set; }
        public string[] IncludedResources { get; set; }

        public string BaseUri {
            get
            {
                return baseUri;
            }
        }
    }
}
