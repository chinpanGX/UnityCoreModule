using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityScreenNavigator.Runtime.Core.Modal;
using VContainer;
using VContainer.Unity;

namespace ScreenSystem.Modal
{
    public class ModalManager : IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private readonly LifetimeScope _lifetimeScope;

        private readonly ModalContainer _modalContainer;


        [Inject]
        public ModalManager(ModalContainer modalContainer, LifetimeScope lifetimeScope)
        {
            _modalContainer = modalContainer;
            _lifetimeScope = lifetimeScope;
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        public async UniTask<TModal> Push<TModal>(IModalBuilder builder, CancellationToken cancellationToken)
            where TModal : class, IModal
        {
            return await Push(builder, cancellationToken) as TModal;
        }

        public async UniTask<IModal> Push(IModalBuilder builder, CancellationToken cancellationToken)
        {
            if (ModalTransitionScope.IsModalTransition)
            {
                await ModalTransitionScope.WaitTransition(cancellationToken);
            }

            using var scope = ModalTransitionScope.Transition();
            var page = await builder.Build(_modalContainer, _lifetimeScope, cancellationToken);
            return page;
        }

        public void PushAndForget(IModalBuilder builder)
        {
            PushAndForgetInternal(builder).Forget();
        }

        private async UniTaskVoid PushAndForgetInternal(IModalBuilder builder)
        {
            var (pushCanceled, modal) = await Push(builder, _cancellationTokenSource.Token).SuppressCancellationThrow();
            if (pushCanceled)
            {
                return;
            }

            if (await modal.OnCompleteAsync(_cancellationTokenSource.Token)
                    .SuppressCancellationThrow())
            {
                return;
            }

            await Pop(modal, true, _cancellationTokenSource.Token);
        }

        public async UniTask Pop(bool playAnimation, CancellationToken cancellationToken)
        {
            await PopInternal(playAnimation, cancellationToken);
        }

        public async UniTask Pop(IModal popModal, bool playAnimation, CancellationToken cancellationToken)
        {
            await PopInternal(playAnimation, cancellationToken, popModal.ModalId);
        }

        private async UniTask PopInternal(bool playAnimation, CancellationToken cancellationToken,
            string modalId = null)
        {
            if (ModalTransitionScope.IsModalTransition)
            {
                await ModalTransitionScope.WaitTransition(cancellationToken);
            }

            using var scope = ModalTransitionScope.Transition();
            if (_modalContainer.Modals.Any())
            {
                if (!string.IsNullOrEmpty(modalId) && _modalContainer.OrderedModalIds.Contains(modalId))
                {
                    await UniTask.WaitUntil(() => _modalContainer.OrderedModalIds.Last() == modalId,
                        cancellationToken: cancellationToken
                    );
                }

                var handle = _modalContainer.Pop(playAnimation);
                await handle.WithCancellation(cancellationToken);
            }
        }

        public async UniTask AllPop(bool animation, CancellationToken cancellationToken)
        {
            while (_modalContainer.Modals.Any()) await Pop(animation, cancellationToken);
        }

        private class ModalTransitionScope : IDisposable
        {

            private ModalTransitionScope()
            {
                IsModalTransition = true;
            }

            public static bool IsModalTransition { get; private set; }

            public void Dispose()
            {
                IsModalTransition = false;
            }

            public static IDisposable Transition()
            {
                return new ModalTransitionScope();
            }

            public static UniTask WaitTransition(CancellationToken token)
            {
                return UniTask.WaitUntil(() => !IsModalTransition, cancellationToken: token);
            }
        }
    }

    public static class ModalExtensions
    {
        public static UniTask Pop(this IModal modal, ModalManager modalManager, bool playAnimation,
            CancellationToken cancellationToken)
        {
            return modalManager.Pop(modal, playAnimation, cancellationToken);
        }
    }
}