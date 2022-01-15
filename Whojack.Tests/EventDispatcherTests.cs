using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace WhoJack.Tests
{
    public sealed class EventDispatcherTests
    {
        [Fact]
        public async Task Dispatch_GivenSomeListeners_ShouldDispatchThemByPriority()
        {
            var dispatcher = new EventDispatcher();
            var builder = new StringBuilder();
            
            dispatcher.AddHandler<TestEventOne>(e => builder.Append($"|{e.X * 2}|"), 10);
            dispatcher.AddHandler<TestEventOne>(e => builder.Append($"|{e.X * 4}|"), 20);
            dispatcher.AddHandler(new TestEventOneHandlerOne(builder));
            dispatcher.AddHandler(new TestEventOneHandlerTwo(builder));
            
            await dispatcher.Dispatch(new TestEventOne(100));

            builder.ToString().Should().Be("|10000||800||400||200|");
        }

        [Fact]
        public async Task Dispatch_GivenSomeListenersWithDefaultPriority_ShouldDispatchThemInTheOrderTheyWereAdded()
        {
            var dispatcher = new EventDispatcher();
            var builder = new StringBuilder();
            
            dispatcher.AddHandler<TestEventOne>(e => builder.Append($"{e.X * 2}"));
            dispatcher.AddHandler<TestEventOne>(e => builder.Append($"|{e.X * 4}"));
            dispatcher.AddHandler<TestEventOne>(e => builder.Append($"|{e.X * 6}"));
            dispatcher.AddHandler<TestEventOne>(e => builder.Append($"|{e.X * 8}"));
            
            await dispatcher.Dispatch(new TestEventOne(2));

            builder.ToString().Should().Be("4|8|12|16");
        }

        [Fact]
        public async Task Dispatch_GivenSomeListeners_ShouldRespectCancellationToken()
        {
            var dispatcher = new EventDispatcher();
            var builder = new StringBuilder();
            
            dispatcher.AddHandler<TestEventOne>(e => builder.Append($"|{e.X * 2}|"));
            dispatcher.AddHandler(new TestEventOneHandlerTwo(builder));
            
            await dispatcher.Dispatch(new TestEventOne(100), new CancellationToken(true));

            builder.ToString().Should().Be("|200|");
        }
        
        [Fact]
        public async Task RemoveListeners_GivenSomeListeners_ShouldRemoveOnlyListenersOfTheGivenEventType()
        {
            var dispatcher = new EventDispatcher();
            var builder = new StringBuilder();
            
            dispatcher.AddHandler<TestEventOne>(e => builder.Append(e.X));
            dispatcher.AddHandler<TestEventOne>(e => builder.Append(e.X));
            dispatcher.AddHandler<TestEventTwo>(e => builder.Append(e.Y));
            dispatcher.RemoveHandlers<TestEventOne>();
            
            await dispatcher.Dispatch(new TestEventOne(42));
            await dispatcher.Dispatch(new TestEventTwo("foo"));

            builder.ToString().Should().Be("foo");
        }
        
        [Fact]
        public async Task Clear_GivenSomeListeners_ShouldRemoveThem()
        {
            var dispatcher = new EventDispatcher();
            var builder = new StringBuilder();
            
            dispatcher.AddHandler<TestEventOne>(e => builder.Append(e.X));
            dispatcher.AddHandler(new TestEventOneHandlerOne(builder));
            dispatcher.Clear();
            
            await dispatcher.Dispatch(new TestEventOne(42));

            builder.ToString().Should().BeEmpty();
        }
    }
}
