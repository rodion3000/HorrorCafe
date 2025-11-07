using System;
using UniRx;

namespace Project.Dev.Services.Interfaces
{
    public interface IRxEventService
    {
        void Publish<T>(T message);
        IObservable<T> OnEvent<T>();
    }
}
