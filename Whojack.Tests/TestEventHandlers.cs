using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WhoJack.Tests
{
    internal sealed class TestEventOneHandlerOne : IEventHandler<TestEventOne>
    {
        private readonly StringBuilder? _stringBuilder;

        public int Priority => 1024;

        internal TestEventOneHandlerOne(StringBuilder? stringBuilder = null) => _stringBuilder = stringBuilder;

        public Task Handle(TestEventOne eventOne, CancellationToken? cancellationToken = default)
        {
            _stringBuilder?.Append($"|{eventOne.X * 8}|");
            return Task.CompletedTask;
        }
    }
    
    internal sealed class TestEventOneHandlerTwo : IEventHandler<TestEventOne>
    {
        private readonly StringBuilder? _stringBuilder;
        
        public int Priority => 2048;

        internal TestEventOneHandlerTwo(StringBuilder? stringBuilder = null) => _stringBuilder = stringBuilder;

        public async Task Handle(TestEventOne eventOne, CancellationToken? cancellationToken = default)
        {
            await Task.Delay(500);

            if (cancellationToken?.IsCancellationRequested ?? false)
            {
                return;
            }
            
            _stringBuilder?.Append($"|{eventOne.X * eventOne.X}|");
        }
    }
}
