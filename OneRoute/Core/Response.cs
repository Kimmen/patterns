using System;
using System.Collections.Generic;
using MediatR;

namespace Kimmen.Patterns.Core
{
    public interface IHandlerResponse
    {
        bool FoundResult();
        IReadOnlyCollection<ErrorDescriptions> GetErrors();
    }

    public interface IHandlerRequest<TResponse> : IRequest<IHandlerResponse> where TResponse : IHandlerResponse
    {

    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class PreferableMethodAttribute : Attribute
    {
        public bool IsQuery { get; set; }
    }
}
