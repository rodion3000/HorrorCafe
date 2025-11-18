using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Project.Dev.Infrastructure.Factories.Interfaces;
using UnityEngine;
using Project.Dev.Infrastructure.AssetManager;
using Project.Dev.Infrastructure.Registers.Interface;
using Project.Dev.Services.StaticDataService;
using Zenject;

namespace Project.Dev.Infrastructure.Factories
{
    public class HeroFactorie : IHeroFactorie
    {
        private const string HeroPrefabId = "Player";
        private readonly IStaticDataService _staticDataService;
        private readonly IAssetProvider _assetProvider;
        private readonly DiContainer _container;
        private readonly List<IHeroRegistry> _heroRegistries;

        [CanBeNull] public GameObject Hero { get; private set; }

        public HeroFactorie(IStaticDataService staticDataService, IAssetProvider assetProvider, DiContainer container
            ,List<IHeroRegistry> heroRegistries)
        {
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
            _container = container;
            _heroRegistries = heroRegistries;
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
            var heroGO = Object.Instantiate(prefab, at, Quaternion.identity);
            _container.InjectGameObject(heroGO);

            foreach (var r in _heroRegistries)
            {
                r.Registry(heroGO);
            }

            return heroGO;
        }
    }

}
