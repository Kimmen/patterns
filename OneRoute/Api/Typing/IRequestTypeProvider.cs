using System;
using System.Collections.Generic;

namespace Kimmen.Patterns.Api.Typing
{
    public interface IRequestTypeProvider
    {
        Type GetRequestType(string name);
        Type GetResponseType(string name);
        IReadOnlyCollection<string> GetAllRequestTypeNames();
    }

    public static class RequestTypeProviderExtensions
    {
        public static bool TryGetRequestType(this IRequestTypeProvider provider, string name, out Type matchedType)
        {
            return (matchedType = provider.GetRequestType(name)) != null;
        }
    }
}