using Project.Dev.GamePlay.Items.Event;
using Project.Dev.GamePlay.Items.Interface;
using Project.Dev.Infrastructure.Registers.Hero;
using UniRx;
using UnityEngine;

namespace Project.Dev.GamePlay.Items.Handler
{
    public class ItemCoffeHandler : IItemEventHandler<ItemCoffeEvent>
    {
        private readonly int defaultLayer = LayerMask.NameToLayer("Default");
        private readonly int interectableLayer = LayerMask.NameToLayer("InterectableLayer");
        private readonly HeroRegistry _heroRegistry;
        public ItemCoffeHandler(HeroRegistry heroRegistry) => _heroRegistry = heroRegistry;
        public void Handle(ItemCoffeEvent evt)
        {
            var item = evt.Item;
            Debug.Log("кофе наливаеться");
            Fixation(item);
            item.layer = defaultLayer;
            _heroRegistry.HeroInteraction.DropObjectFlag = true;
            Observable.Timer(System.TimeSpan.FromSeconds(5f))
                .Subscribe(_ =>
                {
                    UnFixation(item);
                    item.layer = interectableLayer;
                    item.tag = "WithCoffee";
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
