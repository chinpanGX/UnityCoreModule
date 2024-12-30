using System;
using AppCore.Runtime;
using AppService.Runtime;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Screen = AppCore.Runtime.Screen;

namespace App.Title
{
    public class TitleView : MonoBehaviour, IView
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private CustomButton startButton;
        
        static Screen Screen => ComponentLocator.Get<Screen>();
        public Canvas Canvas => canvas;
        private readonly Subject<Unit> onClick = new();
        public Observable<Unit> OnClick => onClick;
        
        public static async UniTask<TitleView> CreateAsync()
        {
            var handle = Addressables.LoadAssetAsync<GameObject>("TitleView");
            await handle.Task;
            var gameObject = Instantiate(handle.Result);
            gameObject.SetActive(false);
            var view = gameObject.GetOrAddComponent<TitleView>();
            return view;
        } 
        
        public void Setup()
        {
            startButton.OnClicked(() => onClick.OnNext(Unit.Default));
        }
        
        public void Push()
        {
            Screen.Push(this);
        }

        public void Pop()
        {
            Screen.Pop();
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            Destroy(gameObject);
        }
    }
}