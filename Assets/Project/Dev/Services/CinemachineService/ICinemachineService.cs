using UnityEngine;

namespace Project.Dev.Services.CinemachineService
{
    public interface ICinemachineService
    {
        void SwitchToCamera(int cameraNumber);
        void MoveCamera(GameObject hero);
        void RotationCamera(GameObject hero);

    }
}
