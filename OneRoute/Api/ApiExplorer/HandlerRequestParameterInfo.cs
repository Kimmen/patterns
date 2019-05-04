using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Kimmen.Patterns.Api.ApiExplorer
{
    internal class HandlerRequestParameterInfo : ParameterInfo
    {
        public HandlerRequestParameterInfo(Type requestType)
        {
            this.ParameterType = requestType;
        }

        public override Type ParameterType { get; }
        public override object[] GetCustomAttributes(bool inherit)
        {
            return new object[]
            {
                new RequiredAttribute()
            };
        }
    }
}