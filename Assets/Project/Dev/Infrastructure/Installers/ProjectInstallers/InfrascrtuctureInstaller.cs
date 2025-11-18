using Cinemachine;
using Project.Data.HeroLocalData;
using Project.Dev.GamePlay.Items.Event;
using Project.Dev.GamePlay.Items.Handler;
using Project.Dev.GamePlay.Items.Interface;
using Project.Dev.Infrastructure.AssetManager;
using Project.Dev.Infrastructure.Factories;
using Project.Dev.Infrastructure.Factories.Interfaces;
using Project.Dev.Infrastructure.Registers.Hero;
using Project.Dev.Infrastructure.SceneManagment;
using Project.Dev.Services.CinemachineService;
using Project.Dev.Services.InputService;
using Project.Dev.Services.Interfaces;
using Project.Dev.Services.LevelProgress;
using Project.Dev.Services.Logging;
using Project.Dev.Services.RxEventService;
using Project.Dev.Services.StaticDataService;
using UnityEngine;
using Zenject;

namespace Project.Dev.Infrastructure.Installers.ProjectInstallers
{
    public class InfrascrtuctureInstaller : MonoInstaller
    {
        [SerializeField] private HeroLocalData _heroLocalData;
        [SerializeField] private GameObject cinemachine;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AddressableProvider>().AsSingle();
            Container.Bind<SceneLoader>().AsSingle();
            BindServices();
            BindHeroRegistry();
            BindFactories();
            BindItemsHandler();
        }

        private void BindServices()
        {
            Container.Bind<ILoggingService>().To<LoggingService>().AsSingle().NonLazy();
            BindCinemachineService();
            Container.Bind<IInputService>().To<InputService>().AsSingle().NonLazy();
            Container.Bind<IRxEventService>().To<RxEventService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<StaticDataService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LevelProgressServiceResolver>()
                .AsSingle()
                .CopyIntoDirectSubContainers();
            Container.BindInterfacesAndSelfTo<LevelProgressService>().AsSingle().NonLazy();
        }

        private void BindFactories()
        {
            Container.Bind<HeroLocalData>().FromInstance(_heroLocalData);
            Container.BindInterfacesAndSelfTo<StateFactories>().AsSingle();
            Container.Bind<IHeroFactorie>().To<HeroFactorie>().AsSingle();
            Container.Bind<IStageFactorie>().To<StageFactorie>().AsSingle();
            Container.Bind<IUIFactorie>().To<UIFactorie>().AsSingle();
        }

        private void BindCinemachineService()
        {
            var cinemachineMovePrefab = Instantiate(cinemachine, this.transform)
                .GetComponent<CinemachineVirtualCamera>();
            cinemachineMovePrefab.gameObject.name = "CinemachineMoveCamera";

            Container.BindInterfacesAndSelfTo<CinemachineService>().AsSingle()
                .WithArguments(cinemachineMovePrefab)
                .NonLazy();
        }

        private void BindItemsHandler()
        {
            Container.Bind<IItemEventHandler<ItemCoffeEvent>>().To<ItemCoffeHandler>().AsTransient();
        }

        private void BindHeroRegistry()
        {
            Container.Bind<HeroRegistry>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<HeroInteractionRegister>().AsSingle();
            Container.BindInterfacesAndSelfTo<HeroMoveRegister>().AsSingle();
        }
    }
}
