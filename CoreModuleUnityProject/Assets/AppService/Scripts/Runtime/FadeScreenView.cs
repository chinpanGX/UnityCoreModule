﻿using AppCore.Runtime;
using AssetLoader.Application;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AppService.Runtime
{
    public class FadeScreenView : MonoBehaviour, IView
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private CanvasGroup fadeCanvasGroup;
        [Range(0.1f, 1.0f)] [SerializeField] private float durationSecond;
        public Canvas Canvas => canvas;
        static ModalScreen ModalScreen => ComponentLocator.Get<ModalScreen>();

        public static async UniTask<FadeScreenView> CreateAsync(IAssetLoader assetLoader)
        {
            var prefab = await assetLoader.LoadAsync<GameObject>("FadeScreenView");
            var obj = Instantiate(prefab);
            obj.SetActive(false);
            return obj.GetComponentSafe<FadeScreenView>();
        }

        public void Push()
        {
            ModalScreen.Push(this);
        }

        public void Pop()
        {
            ModalScreen.Pop();
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            Destroy(gameObject);
        }

        public async UniTask FadeIn()
        {
            gameObject.SetActive(true);
            var elapsedTime = 0f;
            while (elapsedTime < durationSecond)
            {
                fadeCanvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / durationSecond);
                elapsedTime += Time.deltaTime;
                await UniTask.Yield(PlayerLoopTiming.Update, destroyCancellationToken);
            }
            fadeCanvasGroup.alpha = 1;
        }

        public async UniTask FadeOut()
        {
            var elapsedTime = 0f;
            while (elapsedTime < durationSecond)
            {
                fadeCanvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / durationSecond);
                elapsedTime += Time.deltaTime;
                await UniTask.Yield(PlayerLoopTiming.Update, destroyCancellationToken);
            }
            fadeCanvasGroup.alpha = 0;
            gameObject.SetActive(false);
        }

        public void BlackOut()
        {
            fadeCanvasGroup.alpha = 1;
        }
    }
}