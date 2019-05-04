using System.Collections.Generic;

namespace Kimmen.Patterns.Core
{
    public class Errors : IHandlerResponse
    {
        private readonly ErrorDescriptions[] errors;

        public Errors(string message, int code = 0)
        {
            this.errors = new[]
            {
                new ErrorDescriptions
                {
                    Code = code, Message = message
                },
            };
        }

        public Errors(params ErrorDescriptions[] errors)
        {
            this.errors = errors;
        }

        public bool FoundResult()
        {
            return false;
        }

        public IReadOnlyCollection<ErrorDescriptions> GetErrors()
        {
            return errors;
        }
    }

    public class ErrorDescriptions
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}