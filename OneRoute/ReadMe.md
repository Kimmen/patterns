#Pattern: One Route

The idea is to streamline the developement an API without to explcitily specify the routes to expose. 
By exposing a RPC (Remote Procudure Call) styled API the routes are infered by founding handlers.

One Route pattern only consist of one controller that only consist of one variable route:
> /api/{name}
Where the name will be used to find an associated handler with matching input.

## Motiviation
- No need to think about wether the route is correct
- A way to streamlines the development: create new Handler and it's exposed API without creating any additional plumping code

## Scenarios
An API that is mainly consumed by just one Client and the routes path and "proper" HTTP verbs does't matter.

## Implementation
- Probes after Request types and generates a friendly name for each Request
- On calls, matches the incoming name with the corresponding Request type
- Deserializes the body / querystring to the found Request
- Uses MediatR to find a matching Hanlder for the Request

This example also have a solution to be able to generate swagger definition (using Swashbuckle), by manually exposing the Requests types via ApiExplorer.