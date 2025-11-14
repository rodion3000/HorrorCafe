using Project.Dev.GamePlay.Location;
using Project.Dev.Infrastructure.Factories.Interfaces;

namespace Project.Dev.Infrastructure.Factories.Components
{
    public class StageRegistryComponents : IRegistryComponent<LocationManager>
    {
        private LocationManager _locationManager;

        public void Register(LocationManager instance)
        {
            _locationManager = instance.GetComponent<LocationManager>();
        }
    }
}
