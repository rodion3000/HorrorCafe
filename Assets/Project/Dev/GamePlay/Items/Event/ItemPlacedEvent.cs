using Project.Dev.GamePlay.Items.Interface;
using UnityEngine;

namespace Project.Dev.GamePlay.Items.Event
{
    public class ItemPlacedEvent : IItemEvent
    {
        public GameObject Item { get; }
        public ItemPlacedEvent(GameObject item) => Item = item;
    }
}
