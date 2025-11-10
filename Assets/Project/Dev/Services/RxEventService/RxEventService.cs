using System;
using System.Collections.Generic;
using System.Linq;
using Project.Dev.Services.Interfaces;
using UniRx;
using UnityEngine;

namespace Project.Dev.Services.RxEventService
{
    public class RxEventService : IRxEventService, IDisposable
    {
        private readonly Dictionary<Type, object> _subjects = new();

        public RxEventService()
        {
            Debug.Log($"🧠 RxEventService instance #{GetHashCode()} created");
        }

        // 📤 Публикация события
        public void Publish<T>(T message)
        {
            var messageType = typeof(T);
            bool delivered = false;

            // 🔹 1. Отправляем событие всем Subject, чей ключ совместим с типом сообщения
            foreach (var kvp in _subjects.ToArray())
            {
                var keyType = kvp.Key;

                if (keyType.IsAssignableFrom(messageType))
                {
                    try
                    {
                        var method = typeof(RxEventService)
                            .GetMethod(nameof(Emit), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                            .MakeGenericMethod(keyType, messageType);

                        method.Invoke(this, new object[] { kvp.Value, message });
                        delivered = true;
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"❌ RxEventService: Failed to deliver {messageType.Name} to {keyType.Name}: {e.Message}");
                    }
                }
            }

            if (!delivered)
                Debug.LogWarning($"⚠️ RxEventService: No subscribers for event {messageType.Name}");
        }

        // 🔧 Вспомогательный метод для типобезопасного вызова OnNext
        private void Emit<TBase, TDerived>(object subjectObj, TDerived message)
            where TDerived : TBase
        {
            var subject = (ISubject<TBase>)subjectObj;
            subject.OnNext(message);
        }

        // 📥 Подписка на события
        public IObservable<T> OnEvent<T>()
        {
            var type = typeof(T);

            if (!_subjects.TryGetValue(type, out var subject))
            {
                subject = new Subject<T>();
                _subjects[type] = subject;
                Debug.Log($"👂 RxEventService: New subject registered for {type.Name}");
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
            Debug.Log("🧹 RxEventService disposed");
        }
    }
}
