

namespace Project.Dev.Infrastructure.Factories.Interfaces
{
    public interface IRegistryComponent<T>
    {
        void Register(T instance);
    }
}
