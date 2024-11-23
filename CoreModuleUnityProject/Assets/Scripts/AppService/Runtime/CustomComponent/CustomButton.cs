using System;
using R3;
using R3.Triggers;
using UnityEngine;

namespace AppService.Runtime
{

    [RequireComponent(typeof(ObservableEventTrigger))]
    public class CustomButton : MonoBehaviour
    {
        private ObservableEventTrigger observableEventTrigger;

        /// <summary>
        /// 連打を防止するためのクールダウン時間
        /// </summary>
        public TimeSpan TapCooldownTime => TimeSpan.FromSeconds(0.3);
        
        /// <summary>
        /// インタラクティブかどうか
        /// </summary>
        public ReactiveProperty<bool> IsInteractable { get; } = new(true);

        public Observable<Unit> OnButtonClicked => observableEventTrigger
            .OnPointerClickAsObservable().AsUnitObservable().Where(_ => IsInteractable.Value);

        public Observable<Unit> OnButtonPressed => observableEventTrigger
            .OnPointerDownAsObservable().AsUnitObservable().Where(_ => IsInteractable.Value);

        public Observable<Unit> OnButtonReleased => observableEventTrigger
            .OnPointerUpAsObservable().AsUnitObservable().Where(_ => IsInteractable.Value);

        public Observable<Unit> OnButtonEntered => observableEventTrigger
            .OnPointerEnterAsObservable().AsUnitObservable().Where(_ => IsInteractable.Value);

        public Observable<Unit> OnButtonExited => observableEventTrigger
            .OnPointerExitAsObservable().AsUnitObservable().Where(_ => IsInteractable.Value);

        public void Awake()
        {
            observableEventTrigger = gameObject.GetOrAddComponent<ObservableEventTrigger>(); 
        }
        
        public void Dispose()
        {
            IsInteractable?.Dispose();
        }

        public void SetInteractable(bool value)
        {
            IsInteractable.Value = value;
        }
    }
}