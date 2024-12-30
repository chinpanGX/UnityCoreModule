using AppCore.Runtime;
using AppService.Runtime;
using AssetLoader.Application;
using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UnityEngine;

namespace App.Home
{
    public class SignupView : MonoBehaviour, IView
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private CustomButton signupButton;
        [SerializeField] private TMP_InputField userNameInputField;

        public Canvas Canvas => canvas;
        private ViewScreen ViewScreen => ComponentLocator.Get<ViewScreen>();

        private string requestUserName = "";
        private readonly Subject<string> registerRequestUserName = new();
        public Observable<string> RegisterRequestUserName => registerRequestUserName;
        
        public static async UniTask<SignupView> CreateAsync(IAssetLoader assetLoader)
        {
            var prefab = await assetLoader.LoadAsync<GameObject>("SignupView");
            var obj = Instantiate(prefab);
            obj.SetActive(false);
            return obj.GetComponentSafe<SignupView>();
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
            userNameInputField.onValueChanged.RemoveAllListeners();
            userNameInputField.onValueChanged.AddListener(value =>
                {
                    requestUserName = value;
                }
            );
            
            signupButton.OnClicked(() => registerRequestUserName.OnNext(requestUserName));
            gameObject.SetActive(true);
        }
        
        public void Close()
        {
            Destroy(gameObject);
        }
    }

}
