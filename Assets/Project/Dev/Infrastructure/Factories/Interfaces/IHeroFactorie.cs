using System.Threading.Tasks;
using UnityEngine;

namespace Project.Dev.Infrastructure.Factories.Interfaces
{
    public interface IHeroFactorie
    {
        GameObject Hero { get; }
        Task WarmUp();
        void CleanUp();
        Task<GameObject> Create(Vector3 at);
    }
}
