using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityScreenNavigator.Runtime.Core.Modal;

namespace ScreenSystem.Modal
{
    public abstract class LifecycleModalBase : IModal, IModalLifecycleEvent, IDisposable
    {

        private readonly UniTaskCompletionSource _completeCompletionSource = new();

        private readonly CancellationTokenSource _disposeCancellationTokenSource;
        private readonly UnityScreenNavigator.Runtime.Core.Modal.Modal _modal;

        private CancellationTokenSource _exitCancellationTokenSource;

        protected LifecycleModalBase(UnityScreenNavigator.Runtime.Core.Modal.Modal modal)
        {
            _modal = modal;
            _modal.AddLifecycleEvent(this);
            _disposeCancellationTokenSource = new CancellationTokenSource();
        }
        public CancellationToken ExitCancellationToken => _exitCancellationTokenSource.Token;

        public CancellationToken DisposeCancellationToken => _disposeCancellationTokenSource.Token;

        public virtual void Dispose()
        {
            _modal.RemoveLifecycleEvent(this);
            _disposeCancellationTokenSource.Cancel();
            _disposeCancellationTokenSource.Dispose();
        }

        public UniTask OnCompleteAsync(CancellationToken cancellationToken)
        {
            return _completeCompletionSource.Task.AttachExternalCancellation(cancellationToken);
        }

        public string ModalId => _modal.Identifier;

        public IEnumerator Initialize()
        {
            var cts = BuildCancellationTokenSourceOnDispose();
            yield return InitializeAsync(cts.Token).ToCoroutine();

            cts.Cancel();
        }

        public IEnumerator WillPushEnter()
        {
            EnableExitTokenSource(true);
            var cts = BuildCancellationTokenSourceOnDispose();
            yield return WillPushEnterAsync(cts.Token).ToCoroutine();

            cts.Cancel();
        }

        public virtual void DidPushEnter()
        {
        }

        public IEnumerator WillPushExit()
        {
            EnableExitTokenSource(false);
            yield return WillPushExitAsync().ToCoroutine();
        }

        public virtual void DidPushExit()
        {
        }

        public IEnumerator WillPopEnter()
        {
            EnableExitTokenSource(true);
            var cts = BuildCancellationTokenSourceOnDispose();
            yield return WillPopEnterAsync(cts.Token).ToCoroutine();

            cts.Cancel();
        }

        public virtual void DidPopEnter()
        {
        }

        public IEnumerator WillPopExit()
        {
            EnableExitTokenSource(false);
            var cts = BuildCancellationTokenSourceOnDispose();
            yield return WillPopExitAsync(cts.Token).ToCoroutine();

            cts.Cancel();
        }

        public virtual void DidPopExit()
        {
        }

        public IEnumerator Cleanup()
        {
            var cts = BuildCancellationTokenSourceOnDispose();
            yield return CleanUpAsync(cts.Token).ToCoroutine();

            cts.Cancel();
        }

        protected virtual UniTask InitializeAsync(CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }

        protected virtual UniTask WillPushEnterAsync(CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }

        protected virtual UniTask WillPushExitAsync()
        {
            return UniTask.CompletedTask;
        }

        protected virtual UniTask WillPopEnterAsync(CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }

        protected virtual UniTask WillPopExitAsync(CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }

        protected virtual UniTask CleanUpAsync(CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }

        private void EnableExitTokenSource(bool enable)
        {
            if (enable)
            {
                _exitCancellationTokenSource = BuildCancellationTokenSourceOnDispose();
            }
            else
            {
                _exitCancellationTokenSource.Cancel();
            }
        }

        private CancellationTokenSource BuildCancellationTokenSourceOnDispose()
        {
            return CancellationTokenSource.CreateLinkedTokenSource(_disposeCancellationTokenSource.Token);
        }

        protected void Complete()
        {
            _completeCompletionSource.TrySetResult();
        }
    }
}