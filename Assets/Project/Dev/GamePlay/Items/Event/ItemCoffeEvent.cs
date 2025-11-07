using Project.Dev.GamePlay.Items.Interface;
using UnityEngine;

namespace Project.Dev.GamePlay.Items.Event
{
    public class ItemCoffeEvent : IItemEvent
    {
        public GameObject Item { get; }
        public ItemCoffeEvent(GameObject item) => Item = item;
    }
}
