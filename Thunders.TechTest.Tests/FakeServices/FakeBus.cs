using Rebus.Bus;
using Rebus.Bus.Advanced;

namespace Thunders.TechTest.Tests.FakeServices
{
    public class FakeBus : IBus
    {
        public Task SendLocal(object commandMessage, IDictionary<string, string> optionalHeaders = null)
        {
            return Task.CompletedTask;
        }

        public Task Send(object commandMessage, IDictionary<string, string> optionalHeaders = null)
        {
            return Task.CompletedTask;
        }

        public Task DeferLocal(TimeSpan delay, object message, IDictionary<string, string> optionalHeaders = null)
        {
            return Task.CompletedTask;
        }

        public Task Defer(TimeSpan delay, object message, IDictionary<string, string> optionalHeaders = null)
        {
            return Task.CompletedTask;
        }

        public Task Reply(object replyMessage, IDictionary<string, string> optionalHeaders = null)
        {
            return Task.CompletedTask;
        }

        public Task Subscribe<TEvent>()
        {
            return Task.CompletedTask;
        }

        public Task Subscribe(Type eventType)
        {
            return Task.CompletedTask;
        }

        public Task Unsubscribe<TEvent>()
        {
            return Task.CompletedTask;
        }

        public Task Unsubscribe(Type eventType)
        {
            return Task.CompletedTask;
        }

        public Task Publish(object eventMessage, IDictionary<string, string> optionalHeaders = null)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }

        public IAdvancedApi Advanced => throw new NotImplementedException();
    }
}
