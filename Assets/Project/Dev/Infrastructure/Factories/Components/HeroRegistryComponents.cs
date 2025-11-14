using Project.Dev.GamePlay.NPC.Player1;
using Project.Dev.Infrastructure.Factories.Interfaces;
using UnityEngine;

namespace Project.Dev.Infrastructure.Factories.Components
{
    public class HeroRegistryComponents : IRegistryComponent<GameObject>
    {
        private HeroInteraction _heroInteraction;

        public void Register(GameObject instance)
        {
            _heroInteraction = instance.GetComponent<HeroInteraction>();
        }
    }
}
