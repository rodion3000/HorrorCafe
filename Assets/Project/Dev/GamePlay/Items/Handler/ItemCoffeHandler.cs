using Project.Dev.GamePlay.Items.Event;
using Project.Dev.GamePlay.Items.Interface;
using UniRx;
using UnityEngine;

namespace Project.Dev.GamePlay.Items.Handler
{
    public class ItemCoffeHandler : IItemEventHandler<ItemCoffeEvent>
    {
        private readonly int defaultLayer = LayerMask.NameToLayer("Default");
        private readonly int interectableLayer = LayerMask.NameToLayer("InterectableLayer");
        public void Handle(ItemCoffeEvent evt)
        {
            var item = evt.Item;

            Debug.Log("кофе наливаеться");
            var sound = item.GetComponent<AudioSource>();
            sound.Play();
            Fixation(item);
            item.layer = defaultLayer;
            Observable.Timer(System.TimeSpan.FromSeconds(5f))
                .Subscribe(_ =>
                {
                    UnFixation(item);
                    item.layer = interectableLayer;
                    if(sound != null && sound.isPlaying)
                        sound.Stop();
                    Debug.Log("кофе готов");
                });
        }

        private void Fixation(GameObject item)
        {
            if (item.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }

        }

        private void UnFixation(GameObject item)
        {
            if (item.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
        }
    }
}
