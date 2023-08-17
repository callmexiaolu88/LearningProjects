using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Dapr.Actors.Runtime;
using Dapr.Actors;

namespace daprserver
{
    public interface ITest: IActor
    {
        Task<string> GetIdAsync();
    }

    public interface ITestActor : IActor
    {
        Task<string> GetAsync();
    }
    public class TestActor : Actor, ITest, ITestActor
    {
        public TestActor(ActorHost host) : base(host)
        {
        }

        public Task<string> GetAsync()
        {
            return Task.FromResult(Id.ToString());
        }

        public Task<string> GetIdAsync()
            => Task.FromResult(Id.ToString());
    }
}