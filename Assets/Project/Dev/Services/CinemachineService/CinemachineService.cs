using Cinemachine;
using UnityEngine;

namespace Project.Dev.Services.CinemachineService
{
    public class CinemachineService : ICinemachineService
    {
        private CinemachineVirtualCamera _moveCamera;
        private CinemachineVirtualCamera _rotationCamera;

        public CinemachineService(CinemachineVirtualCamera moveCamera, CinemachineVirtualCamera rotationCamera)
        {
            _moveCamera = moveCamera;
            _rotationCamera = rotationCamera;
        }


        public void SwitchToCamera(int cameraNumber)
        {
            if (cameraNumber == 1)
            {
                _moveCamera.Priority = 20;
                _rotationCamera.Priority = 10;
            }
            else if (cameraNumber == 2)
            {
                _moveCamera.Priority = 10;
                _rotationCamera.Priority = 20;
            }
        }

        public void MoveCamera(GameObject hero)
        {
            _moveCamera.Priority = 10;
            _moveCamera.Follow = hero.transform;
            _moveCamera.LookAt = hero.transform;
        }

        public void RotationCamera(GameObject hero)
        {
            Transform heroSpine = hero.transform
                .Find("Model")
                .Find("GameSkeleton")
                .Find("Hips")
                .Find("Spine");

            if (heroSpine != null)
            {
                _rotationCamera.Priority = 20;
                _rotationCamera.Follow = heroSpine;
                _rotationCamera.LookAt = hero.transform;
            }

        }
    }
}
