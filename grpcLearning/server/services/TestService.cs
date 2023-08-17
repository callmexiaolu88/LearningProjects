using System.Threading.Tasks;
using Grpc.Core;
using TestGrpc.Imples;

namespace server.services
{
    public class TestService : TestGrpc.Imples.TestService.TestServiceBase
    {
        public override Task<GetResponse> GetService(GetRequest request, ServerCallContext context)
        {
            var response = new GetResponse
            {
                Name = request.Name,
                IP = context.Host
            };
            return Task.FromResult(response);
        }
    }
}