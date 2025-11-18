using Project.Dev.GamePlay.NPC.Player1;
using Project.Dev.Infrastructure.Registers.Interface;
using UnityEngine;

namespace Project.Dev.Infrastructure.Registers.Hero
{
    public class HeroInteractionRegister : IHeroRegistry
    {
        private readonly HeroRegistry _heroRegistry;

        public HeroInteractionRegister(HeroRegistry heroRegistry)
        {
            _heroRegistry = heroRegistry;
        }
        public void Registry(GameObject hero)
        {
           var interaction = hero.GetComponent<HeroInteraction>();
            _heroRegistry.SetInteraction(interaction);
        }
    }
}
