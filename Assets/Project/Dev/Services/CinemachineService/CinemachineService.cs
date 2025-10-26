using Cinemachine;
using UnityEngine;

namespace Project.Dev.Services.CinemachineService
{
    public class CinemachineService : ICinemachineService
    {
        private CinemachineVirtualCamera _heroCamera;
        public CinemachinePOV Pov { get; private set; }

        public CinemachineService(CinemachineVirtualCamera heroCamera)
        {
            _heroCamera = heroCamera;
        }

        public void HeroCamera(GameObject hero)
        {
            Transform headCamera = hero.transform.Find("HeadCamera");
            _heroCamera.Follow = headCamera;
            _heroCamera.LookAt = headCamera;
            Pov = _heroCamera.GetCinemachineComponent<CinemachinePOV>();
            if (Pov == null)
            {
                Pov = _heroCamera.AddCinemachineComponent<CinemachinePOV>();
                Pov.m_VerticalAxis.m_InputAxisName = "";
                Pov.m_HorizontalAxis.m_InputAxisName = "";
                Pov.m_VerticalAxis.m_MinValue = -80f;
                Pov.m_VerticalAxis.m_MaxValue = 80f;
                Pov.m_VerticalAxis.m_AccelTime = 0f;
                Pov.m_VerticalAxis.m_DecelTime = 0f;
                Pov.m_HorizontalAxis.m_AccelTime = 0f;
                Pov.m_HorizontalAxis.m_DecelTime = 0f;
                Pov.m_VerticalAxis.m_MaxSpeed = 400f;
                Pov.m_HorizontalAxis.m_MaxSpeed = 500f;
            }
        }

    }
}
