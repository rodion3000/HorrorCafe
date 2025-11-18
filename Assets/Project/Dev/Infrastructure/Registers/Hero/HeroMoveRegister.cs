using Project.Dev.GamePlay.NPC.Player1;
using Project.Dev.Infrastructure.Registers.Interface;
using UnityEngine;

namespace Project.Dev.Infrastructure.Registers.Hero
{
    public class HeroMoveRegister : IHeroRegistry
    {
        private readonly HeroRegistry _heroMove;

        public HeroMoveRegister(HeroRegistry heroMove)
        {
            _heroMove = heroMove;
        }

        public void Registry(GameObject hero)
        {
            var move = hero.GetComponent<HeroMove>();
            _heroMove.SetMove(move);
        }
    }
}
