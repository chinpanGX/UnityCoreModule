
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AppService.Runtime
{
    public class FadeScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup fadeCanvasGroup;
        [Range(0.1f, 1.0f)] [SerializeField] private float durationSecond;

        public async UniTask FadeIn()
        {
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
        }

        public void BlackOut()
        {
            fadeCanvasGroup.alpha = 1;
        }
    }
}