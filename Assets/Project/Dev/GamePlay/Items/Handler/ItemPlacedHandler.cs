using Project.Dev.GamePlay.Items.Event;
using Project.Dev.GamePlay.Items.Interface;
using UnityEngine;

namespace Project.Dev.GamePlay.Items.Handler
{
    public class ItemPlacedHandler : IItemEventHandler<ItemPlacedEvent>
    {
        public void Handle(ItemPlacedEvent evt)
        {
            Debug.Log("кофе наливаеться");
        }
    }
}
