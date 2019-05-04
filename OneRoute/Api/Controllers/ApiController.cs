using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Kimmen.Patterns.Api.Typing;
using Kimmen.Patterns.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kimmen.Patterns.Api.Controllers
{
    [Route("api")]
    [ApiExplorerSettings(IgnoreApi = true)] //Ignore this controller in the ApiExplorer, as we are manually generating this.
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IRequestTypeProvider requestTypeProvider;
        private readonly JsonSerializer serializer;

        public ApiController(IMediator mediator, IRequestTypeProvider requestTypeProvider)
        {
            this.mediator = mediator;
            this.requestTypeProvider = requestTypeProvider;
            this.serializer = JsonSerializer.CreateDefault();
        }
        
        [HttpPost("{requestName}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Post(string requestName)
        {
            if (!this.requestTypeProvider.TryGetRequestType(requestName, out var targetType))
            {
                return NotFound();
            }

            var deserializedRequest = this.DeserializeBody(targetType);
            var response = await this.mediator.Send(deserializedRequest);

            return GenerateActionResult(response);
        }

        [HttpGet("{requestName}")]
        [Produces("application/json")]
        public async Task<IActionResult> Get(string requestName)
        {
            if (!this.requestTypeProvider.TryGetRequestType(requestName, out var targetType))
            {
                return NotFound();
            }

            var deserializedRequest = this.DeserializeQuery(targetType);
            var response = await this.mediator.Send(deserializedRequest);

            return GenerateActionResult(response);
        }

        private IActionResult GenerateActionResult(IHandlerResponse response)
        {
            switch (response)
            {
                case var r when r == null: throw new InvalidOperationException("Request was processed but didn't produce a response.");
                case var r when r.GetErrors().Any(): return BadRequest(r.GetErrors());
                case var r when r.FoundResult(): return Ok(r);
                default: return NotFound();
            }
        }

        private IRequest<IHandlerResponse> DeserializeBody(Type targetType)
        {
            using (var reader = new StreamReader(this.Request.Body))
            {
                return this.serializer.Deserialize(reader, targetType) as IRequest<IHandlerResponse>;
            }
        }

        private IRequest<IHandlerResponse> DeserializeQuery(Type targetType)
        {
            var bag = new JObject();

            foreach (var (key, value) in Request.Query)
            { 
                //Only handle query parameter that occurs once.
                if (value.Count == 1)
                {
                    bag[key] = value.First();
                }
            }

            return bag.ToObject(targetType) as IRequest<IHandlerResponse>;
        }
    }
}