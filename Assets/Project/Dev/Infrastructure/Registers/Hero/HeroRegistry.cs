using Project.Dev.GamePlay.NPC.Player1;

namespace Project.Dev.Infrastructure.Registers.Hero
{
    public class HeroRegistry
    {
        public HeroInteraction HeroInteraction { get; private set; }
        public HeroMove HeroMove { get; private set; }

        public void SetInteraction(HeroInteraction heroInteraction) => HeroInteraction = heroInteraction;
        public void SetMove(HeroMove heroMove) => HeroMove = heroMove;
    }
}
