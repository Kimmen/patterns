using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Kimmen.Patterns.Core
{
    public static class AddUserAccount
    {
        public class Handler : IRequestHandler<Request, IHandlerResponse>
        {
            public Task<IHandlerResponse> Handle(Request request, CancellationToken cancellationToken)
            {
                return string.IsNullOrWhiteSpace(request.UserName) 
                    ? Task.FromResult(new Errors("User name is empty") as IHandlerResponse) 
                    : Task.FromResult(new Response {UserId = Guid.NewGuid() } as IHandlerResponse);
            }
        }

        public class Request : IHandlerRequest<Response>
        {
            public string GivenName{ get; set; }
            public string Surname { get; set; }
            public string UserName { get; set; }
        }

        public class Response : IHandlerResponse
        {
            public Guid UserId { get; set; }

            public bool FoundResult()
            {
                return UserId != Guid.Empty;
            }

            public IReadOnlyCollection<ErrorDescriptions> GetErrors()
            {
                return new List<ErrorDescriptions>().AsReadOnly();
            }
        }
    }
}
