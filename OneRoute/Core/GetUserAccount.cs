using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Kimmen.Patterns.Core
{
    public static class GetUserAccount
    {
        public class Handler : IRequestHandler<Request, IHandlerResponse>
        {
            public Task<IHandlerResponse> Handle(Request request, CancellationToken cancellationToken)
            {
                return request.UserId == Guid.Empty
                    ? Task.FromResult(new Errors("Must provide a user id") as IHandlerResponse)
                    : Task.FromResult(new Response
                    {
                        UserName = "Kimmen",
                        GivenName = "John",
                        Surname = "Doe"
                    } as IHandlerResponse);
            }
        }

        [PreferableMethod(IsQuery = true)]
        public class Request : IHandlerRequest<Response>
        {
            public Guid UserId { get; set; }
            public int Version { get; set; }
        }

        public class Response : IHandlerResponse
        {
            public string UserName { get; set; }
            public string GivenName { get; set; }
            public string Surname { get; set; }

            public bool FoundResult()
            {
                return string.IsNullOrWhiteSpace(UserName) == false;
            }

            public IReadOnlyCollection<ErrorDescriptions> GetErrors()
            {
                return new List<ErrorDescriptions>().AsReadOnly();
            }
        }
    }
}
