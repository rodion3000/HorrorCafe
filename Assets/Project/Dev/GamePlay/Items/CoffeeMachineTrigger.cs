using Project.Dev.GamePlay.Items.Event;
using Project.Dev.Services.Interfaces;
using UnityEngine;
using Zenject;

namespace Project.Dev.GamePlay.Items
{
    public class CoffeeMachineTrigger : MonoBehaviour
    {
        private IRxEventService _eventService;

        [Inject]
        private void Construct(IRxEventService eventService)
        {
            Debug.Log("✅ DI Injected into CoffeeMachineTrigger");
            _eventService = eventService;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Coffee"))
                return;

            Debug.Log("✅ Trigger: Item entered!");
            _eventService.Publish(new ItemCoffeEvent(other.gameObject));
        }

    }
}
