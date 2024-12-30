using AppCore.Runtime;
using AppService.Runtime;
using R3;
using UnityEngine;

namespace App.Title
{
    public class TitleView : MonoBehaviour, IView
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private CustomButton startButton;
        
        static ViewScreen ViewScreen => ComponentLocator.Get<ViewScreen>();
        public Canvas Canvas => canvas;
        private readonly Subject<Unit> onClick = new();
        public Observable<Unit> OnClick => onClick;
        
        public void Setup()
        {
            startButton.OnClicked(() => onClick.OnNext(Unit.Default));
        }
        
        public void Push()
        {
            ViewScreen.Push(this);
        }

        public void Pop()
        {
            ViewScreen.Pop();
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}