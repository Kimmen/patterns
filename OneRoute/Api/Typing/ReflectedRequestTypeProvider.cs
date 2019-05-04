using System;
using System.Collections.Generic;
using System.Linq;
using Kimmen.Patterns.Core;
using MediatR;

namespace Kimmen.Patterns.Api.Typing
{
    /// <summary>
    /// Provides request types that can be used by the "one-route" solution.
    /// Finds request and response types by probing Mediatr request implementations
    /// according the code convention for this solution. 
    /// </summary>
    public class ReflectedRequestTypeProvider : IRequestTypeProvider
    {
        private readonly IDictionary<string, (Type type, Type responeType)> mediatorBaseRequests;

        public ReflectedRequestTypeProvider()
        {
            this.mediatorBaseRequests = typeof(IHandlerResponse).Assembly.GetTypes()
                .Where(t => t.GetInterface(nameof(IBaseRequest)) != null)
                .Where(t => t.IsInterface == false)
                .Select(t => new { Request = t, Response = ExtractResponseType(t) })
                .ToDictionary(e => e.Request.DeclaringType.Name.ToLowerInvariant(), e => (e.Request, e.Response));
        }

        private static Type ExtractResponseType(Type type)
        {
            var mediatorRequest = type.GetInterfaces().First(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IHandlerRequest<>));
            var typedResponse = mediatorRequest.GetGenericArguments().First();

            return typedResponse;
        }

        public Type GetRequestType(string name)
        {
            return this.mediatorBaseRequests.TryGetValue(name.ToLowerInvariant(), out var entry)
                ? entry.type
                : null;
        }

        public Type GetResponseType(string name)
        {
            return this.mediatorBaseRequests.TryGetValue(name.ToLowerInvariant(), out var entry)
                ? entry.responeType
                : null;
        }

        public IReadOnlyCollection<string> GetAllRequestTypeNames()
        {
            return this.mediatorBaseRequests.Keys.ToList().AsReadOnly();
        }
    }
}
