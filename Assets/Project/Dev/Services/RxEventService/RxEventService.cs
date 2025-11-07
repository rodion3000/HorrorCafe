using System;
using System.Collections.Generic;
using Project.Dev.Services.Interfaces;
using UniRx;

namespace Project.Dev.Services.RxEventService
{
    public class RxEventService : IRxEventService, IDisposable
    {
        private readonly Dictionary<Type, object> _subjects = new();

        public void Publish<T>(T message)
        {
            if (_subjects.TryGetValue(typeof(T), out var subject))
            {
                ((ISubject<T>)subject).OnNext(message);
            }
        }

        public IObservable<T> OnEvent<T>()
        {
            if (!_subjects.TryGetValue(typeof(T), out var subject))
            {
                var newSubject = new Subject<T>();
                _subjects[typeof(T)] = newSubject;
                return newSubject.AsObservable();
            }

            return ((ISubject<T>)subject).AsObservable();
        }

        public void Dispose()
        {
            foreach (var sub in _subjects.Values)
            {
                (sub as IDisposable)?.Dispose();
            }
            _subjects.Clear();
        }
    }
}
