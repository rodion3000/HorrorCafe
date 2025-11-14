using System.Threading.Tasks;
using JetBrains.Annotations;
using Project.Dev.Infrastructure.Factories.Interfaces;
using UnityEngine;
using CustomExtensions.Functional;
using Project.Dev.GamePlay.NPC.Player1;
using Project.Dev.Infrastructure.AssetManager;
using Project.Dev.Services.StaticDataService;
using Unity.Mathematics;
using Zenject;

namespace Project.Dev.Infrastructure.Factories
{
    public class HeroFactorie : IHeroFactorie
    {
        private const string HeroPrefabId = "Player";
        private readonly IStaticDataService _staticDataService;
        private readonly IAssetProvider _assetProvider;
        private readonly DiContainer _container;
        private readonly IRegistryComponent<GameObject> _heroRegistry;

        [CanBeNull] public GameObject Hero { get; private set; }

        public HeroFactorie(IStaticDataService staticDataService, IAssetProvider assetProvider, DiContainer container,
            IRegistryComponent<GameObject> heroRegistry)
        {
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
            _container = container;
            _heroRegistry = heroRegistry;
        }

        public async Task WarmUp()
        {
           await _assetProvider.Load<GameObject>(key: HeroPrefabId);
        }

        public void CleanUp()
        {
            Hero = null;
            _assetProvider.Release(key: HeroPrefabId);
        }

        public async Task<GameObject> Create(Vector3 at)
        {
            var prefab = await _assetProvider.Load<GameObject>(HeroPrefabId);
            return Hero = Object.Instantiate(prefab, at, Quaternion.identity)
                .With(hero => _container.InjectGameObject(hero))
                .With(hero => _heroRegistry.Register(hero));
        }
    }

}
