using System;
using Cysharp.Threading.Tasks;
using ScreenSystem.Page.Messages;
using VContainer;

namespace ScreenSystem.Page
{
    public class PageEventPublisher : IPageEventSubscriber, IDisposable
    {
        private readonly Channel<PagePopMessage> _pagePopChannel;
        private readonly Channel<PagePushMessage> _pagePushChannel;

        [Inject]
        public PageEventPublisher()
        {
            _pagePushChannel = Channel.CreateSingleConsumerUnbounded<PagePushMessage>();
            _pagePopChannel = Channel.CreateSingleConsumerUnbounded<PagePopMessage>();
        }

        public void Dispose()
        {
            _pagePushChannel.Writer.TryComplete();
            _pagePopChannel.Writer.TryComplete();
        }

        IUniTaskAsyncEnumerable<PagePushMessage> IPageEventSubscriber.OnPagePushAsyncEnumerable()
        {
            return _pagePushChannel.Reader.ReadAllAsync();
        }

        IUniTaskAsyncEnumerable<PagePopMessage> IPageEventSubscriber.OnPagePopAsyncEnumerable()
        {
            return _pagePopChannel.Reader.ReadAllAsync();
        }

        public void SendPushEvent(IPageBuilder builder)
        {
            _pagePushChannel.Writer.TryWrite(new PagePushMessage(builder));
        }

        public void SendPopEvent(bool playAnimation = true)
        {
            _pagePopChannel.Writer.TryWrite(new PagePopMessage(playAnimation));
        }
    }
}