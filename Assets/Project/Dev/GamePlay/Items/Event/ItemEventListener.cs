using System;
using Project.Dev.GamePlay.Items.Interface;
using Project.Dev.Services.Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace Project.Dev.GamePlay.Items.Event
{
    public class ItemEventListener : MonoBehaviour
    {
        private IRxEventService _eventService;
        private readonly CompositeDisposable _compositeDisposable = new();

        [Inject]
        private void Construct(IRxEventService eventService)
        {
            _eventService = eventService;
        }

        private void Start()
        {
            _eventService.OnEvent<IItemEvent>()
                .Subscribe(Dispatch)
                .AddTo(_compositeDisposable);
        }

        private void Dispatch(IItemEvent evt)
        {
            Type handlerType = typeof(IItemEventHandler<>).MakeGenericType(evt.GetType());

            object handler = ProjectContext.Instance.Container.TryResolve(handlerType);
            if (handler == null)
                return;

            var method = handlerType.GetMethod("Handle");
            method?.Invoke(handler, new object[] { evt });
        }

        private void OnDestroy()
        {
            _compositeDisposable.Dispose();
        }
    }
}
