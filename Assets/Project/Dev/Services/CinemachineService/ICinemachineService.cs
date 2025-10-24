using Cinemachine;
using UnityEngine;

namespace Project.Dev.Services.CinemachineService
{
    public interface ICinemachineService
    {
        void HeroCamera(GameObject hero);
        CinemachinePOV Pov { get; }

    }
}
